using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Dumb;
using Rsx.Math;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : ISamples
    {
        public int AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> hsamples, bool monitors = false)
        {
            int added = 0;
            if (hsamples.Count() == 0) return added;

            // bool cd = false;
            project = project.ToUpper().Trim();
            int? id = this.FindIrrReqID(project);

            try
            {
                //find the ones that are already here
                IEnumerable<SubSamplesRow> samples = SubSamples.OfType<SubSamplesRow>()
                    .Where(o => o.RowState != DataRowState.Deleted);
                samples = samples.Where(o => !o.IsIrradiationRequestsIDNull() && o.IrradiationRequestsID == id);
                ///hsamples.un =
                hsamples = hsamples.Union(samples);
                //hsamples = samples;

                if (monitors) addMonitors(ref hsamples, project);
                else setLabels(ref hsamples, project);

                setIrradiatioRequest(ref hsamples, (int)id);
                hsamples = Changes.GetRowsWithChanges(hsamples).OfType<SubSamplesRow>();
                Save(ref hsamples);

                IEnumerable<UnitRow> Urows = setUnits(ref hsamples);
                Save(ref Urows);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }

            return added;
        }

        public IEnumerable<LINAA.SubSamplesRow> AddSamplesFromNames(ref IEnumerable<string> hsamples)
        {
            IList<LINAA.SubSamplesRow> ls = new List<SubSamplesRow>();
            // if (hsamples.Count() == 0) return added;

            foreach (string sname in hsamples)
            {
                LINAA.SubSamplesRow s = this.SubSamples.NewSubSamplesRow();
                this.SubSamples.AddSubSamplesRow(s);
                s.SubSampleName = sname;
                ls.Add(s);
            }

            return ls;
        }

        public void BeginEndLoadData(bool load)
        {
            if (load)
            {
                this.Measurements.BeginLoadData();
                this.Peaks.BeginLoadData();
                this.Samples.BeginLoadData();
                this.IRequestsAverages.BeginLoadData();
                this.IPeakAverages.BeginLoadData();
                this.SubSamples.BeginLoadData();
                this.Unit.BeginLoadData();
            }
            else
            {
                this.Measurements.EndLoadData();
                this.Peaks.EndLoadData();
                this.Samples.EndLoadData();
                this.SubSamples.EndLoadData();
                this.Unit.EndLoadData();
                this.IRequestsAverages.EndLoadData();
                this.IPeakAverages.EndLoadData();
            }

            populateSelectedExpression(!load);
        }

        /// <summary>
        /// Returns two arrays (Y,X) of doubles containing info about the branching factor)
        /// </summary>
        /// <param name="daugther"> </param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references)
        {
            DataTable table = new DataTable();

            double Ld = 0.693 / daugther.k0NAARow.NAARow.T3;
            double Sd = MyMath.S(Ld, daugther.SubSamplesRow.IrradiationTotalTime);

            LINAA.PeaksRow[] dpeaks = daugther.GetPeaksRows();

            table.Columns.Add("Vs", typeof(string));

            foreach (IPeakAveragesRow reference in references)
            {
                if (reference.Equals(daugther)) continue;
                if (reference.n == 0) continue;

                string energy = reference.Energy.ToString();
                table.Columns.Add("X" + energy, typeof(double));
                table.Columns.Add("Y" + energy, typeof(double));
                table.Columns.Add("EC" + energy, typeof(double));
                table.Columns.Add("Res" + energy, typeof(double));
                table.Columns.Add("YCalc" + energy, typeof(double));

                //lamdas
                double Lref = 0.693 / reference.k0NAARow.NAARow.T2;
                //saturation factor
                double Sref = MyMath.S(Lref, reference.SubSamplesRow.IrradiationTotalTime);
                double DLrefd = Ld - Lref; //delta lamdaes

                LINAA.PeaksRow[] Rpeaks = reference.GetPeaksRows();

                foreach (PeaksRow Rp in Rpeaks)
                {
                    //find same measurement
                    Func<PeaksRow, bool> finder = o => o.Measurement.Equals(Rp.Measurement);

                    LINAA.PeaksRow dp = dpeaks.FirstOrDefault(finder);

                    if (dp == null) continue;

                    //decay factors
                    double Dref = MyMath.D(Lref, Rp.MeasurementsRow.DecayTime / 60);
                    double Dd = MyMath.D(Ld, dp.MeasurementsRow.DecayTime / 60);

                    //counting factors
                    double Cref = MyMath.C(Lref, Rp.MeasurementsRow.CountTime / 60);
                    double Cd = MyMath.C(Ld, dp.MeasurementsRow.CountTime / 60);

                    DataRow result = table.AsEnumerable().FirstOrDefault(o => o.Field<string>("Vs").Equals(Rp.Measurement));

                    if (result == null)
                    {
                        result = table.NewRow();
                        result.SetField<string>("Vs", Rp.Measurement);
                        table.Rows.Add(result);
                    }

                    result.SetField<double>("Y" + energy, (dp.Area) / (Rp.Area));
                    result.SetField<double>("EC" + energy, (dp.Efficiency * dp.COI) / (Rp.Efficiency * Rp.COI));

                    double aux = (Sd * Dd * Cd) / (Sref * Dref * Cref);
                    aux = Ld - (Lref * aux);
                    aux = (aux / DLrefd);

                    result.SetField<double>("X" + energy, aux);
                }

                double[] abR2 = MyMath.CurveFit.Linear.LeastSquaresFit(table.Columns["X" + energy], table.Columns["Y" + energy], table.Columns["Res" + energy], table.Columns["YCalc" + energy]);

                DataRow r = table.AsEnumerable().FirstOrDefault(o => o.Field<string>("Vs").Equals("Fit"));

                if (r == null)
                {
                    r = table.NewRow();
                    table.Rows.Add(r);
                }
                r.SetField<double>("Y" + energy, abR2[0]);
                r.SetField<double>("X" + energy, abR2[1]);
                r.SetField<double>("YCalc" + energy, reference.Energy);
                r.SetField<double>("Res" + energy, abR2[2]);
                r.SetField<double>("EC" + energy, (abR2[0] / abR2[1]));
                r.SetField<string>("Vs", "Fit");
            }

            return table;
        }

        public IEnumerable<SubSamplesRow> FindByIrReqID(int? IrReqID)
        {
            IEnumerable<SubSamplesRow> old = null;
            string IrReqField = this.tableSubSamples.IrradiationRequestsIDColumn.ColumnName;
            old = this.tableSubSamples.Where(LINAA.SelectorByField<SubSamplesRow>(IrReqID, IrReqField));
            return old.ToList();
        }

        public IList<SubSamplesRow> FindByProject(string project)
        {
            IList<SubSamplesRow> old = null;
            string cd = DB.Properties.Misc.Cd;
            string IrReqField = this.tableSubSamples.IrradiationCodeColumn.ColumnName;
            project = project.Replace(cd, null);
            old = this.tableSubSamples.Where(LINAA.SelectorByField<SubSamplesRow>(project, IrReqField)).ToList();
            IEnumerable<SubSamplesRow> oldCD = this.tableSubSamples.Where(LINAA.SelectorByField<SubSamplesRow>(project + cd, IrReqField));

            old = old.Union(oldCD).ToList();

            return old;
        }

        /// <summary>
        /// Finds the SampleRow with given sample name, or adds it if not found and specifically
        /// requested, using the IrrReqID given
        /// </summary>
        /// <param name="sampleName">name of sample to find</param>
        /// <param name="AddifNull"> true for adding the row if not found</param>
        /// <param name="IrrReqID">  irradiation request id to set for the sample only if added</param>
        /// <returns>A non-null SampleRow if AddIfNull is true, otherwise can be null</returns>
        public SubSamplesRow FindBySample(string sampleName, bool AddifNull = false, int? IrrReqID = null)
        {
            SubSamplesRow sample = this.findBySampleName(sampleName);
            if (sample == null)
            {
                sample = this.tableSubSamples.NewSubSamplesRow();
                if (IrrReqID != null) sample.IrradiationRequestsID = (int)IrrReqID;
                sample.SubSampleName = sampleName;
                sample.SubSampleCreationDate = DateTime.Now;
                this.tableSubSamples.AddSubSamplesRow(sample);
            }
            return sample;
        }

        public void PopulatedMonitors(string file)
        {
            LINAA.MonitorsDataTable importing = new LINAA.MonitorsDataTable(false);

            importing.ReadXml(file);

            foreach (LINAA.MonitorsRow i in importing)
            {
                LINAA.MonitorsRow l = this.tableMonitors.FindByMonName(i.MonName); //local monitor
                if (l == null)
                {
                    l = this.tableMonitors.NewMonitorsRow();
                    this.tableMonitors.AddMonitorsRow(l);

                    l.MonName = i.MonName;
                }
                if (l != null)
                {
                    if (l.IsLastMassDateNull())
                    {
                        l.LastMassDate = DateTime.Now.Subtract(new TimeSpan(3650, 0, 0, 0));
                    }
                    if (l.IsLastIrradiationDateNull())
                    {
                        l.LastIrradiationDate = DateTime.Now.Subtract(new TimeSpan(3650, 0, 0, 0));
                    }
                    if (!i.IsLastMassDateNull() && i.LastMassDate > l.LastMassDate)
                    {
                        if (!i.IsMonGrossMass1Null())
                        {
                            l.MonGrossMass1 = i.MonGrossMass1;
                        }
                        if (!i.IsMonGrossMass2Null())
                        {
                            l.MonGrossMass2 = i.MonGrossMass2;
                        }

                        l.LastMassDate = i.LastMassDate;
                    }
                    if (!i.IsLastMassDateNull() && i.LastIrradiationDate > l.LastIrradiationDate)
                    {
                        double daysdiff = (i.LastIrradiationDate.Subtract(l.LastIrradiationDate)).TotalDays;
                        l.LastIrradiationDate = i.LastIrradiationDate;
                    }
                }
            }
        }

        public bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples)
        {
            foreach (LINAA.SubSamplesRow row in this.tableSubSamples)
            {
                row.Override(Alpha, f, Geo, Gt, asSamples);
            }
            return this.HasErrors;
        }

        //
        public Action[] PMStd()
        {
            Action[] populatorArray = null;

            populatorArray = new Action[]   {
        PopulateStandards,
       PopulateMonitors,
         PopulateMonitorFlags,
         };

            return populatorArray;
        }

        public void PopulateMonitorFlags()
        {
            try
            {
                this.tableMonitorsFlags.BeginLoadData();
                this.tableMonitorsFlags.Clear();
                this.TAM.MonitorsFlagsTableAdapter.Fill(this.tableMonitorsFlags);
                this.tableMonitorsFlags.AcceptChanges();
                this.tableMonitorsFlags.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateMonitors()
        {
            try
            {
                this.tableMonitors.BeginLoadData();
                tableMonitors.Clear();
                TAM.MonitorsTableAdapter.DeleteNulls();
                TAM.MonitorsTableAdapter.Fill(tableMonitors);
                tableMonitors.AcceptChanges();
                this.tableMonitors.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateStandards()
        {
            try
            {
                this.tableStandards.BeginLoadData();
                this.tableStandards.Clear();
                this.TAM.StandardsTableAdapter.Fill(this.tableStandards);

                this.tableStandards.AcceptChanges();
                this.tableStandards.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateSubSamples(Int32 IrReqID)
        {
            try
            {
                // this.tableSubSamples.TableNewRow += new DataTableNewRowEventHandler(this.tableSubSamples.SubSamplesDataTable_TableNewRow);

                TAM.SubSamplesTableAdapter.DeleteNulls();

                LINAA.SubSamplesDataTable newsamples = new SubSamplesDataTable(false);
                TAM.SubSamplesTableAdapter.FillBy(newsamples, IrReqID);

                string uniquefield = newsamples.SubSampleNameColumn.ColumnName;
                string Indexfield = newsamples.SubSamplesIDColumn.ColumnName;
                TAMDeleteMethod remov = this.tAM.SubSamplesTableAdapter.Delete;
                bool duplicates = removeDuplicates(newsamples, uniquefield, Indexfield, ref remov);

                if (duplicates)
                {
                    newsamples.Clear();
                    newsamples.Dispose();
                    newsamples = null;
                    PopulateSubSamples(IrReqID);
                    return;
                }

                this.tableSubSamples.BeginLoadData();
                this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);
                this.tableSubSamples.EndLoadData();
                this.tableSubSamples.AcceptChanges();

                PopulateUnitsByProject(IrReqID);
                // this.tableSubSamples.TableNewRow -= new DataTableNewRowEventHandler(this.tableSubSamples.SubSamplesDataTable_TableNewRow);
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateUnitsByProject(int irrReqId)
        {
            try
            {
                this.tableUnit.BeginLoadData();

                LINAA.UnitDataTable dt = new UnitDataTable();

                /// this.tableUnit.Clear();
                this.TAM.UnitTableAdapter.FillByIrrReqID(dt, irrReqId);
                this.tableUnit.Merge(dt, false, MissingSchemaAction.AddWithKey);

                this.tableUnit.EndLoadData();
                this.tableUnit.AcceptChanges();
                // this.MatSSF.Clear(); Hashtable bindings = Dumb.BS.ArrayOfBindings(ref bs, "N4");
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        protected internal void populateSelectedExpression(bool setexpression)
        {
            string expression = string.Empty;
            if (setexpression)
            {
                expression = "Parent(Measurements_Peaks).Selected";
            }
            // PopulatePreferences();
            Peaks.SelectedColumn.Expression = expression;
        }

        private void addMonitors(ref IEnumerable<LINAA.SubSamplesRow> samples, string project)
        {
            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.MonitorsRow == null)
                {
                    string sampleNrOrMon = s.SubSampleName.ToString().Replace(project.Substring(1), null);
                    LINAA.MonitorsRow mon = this.Monitors.FindByMonName(sampleNrOrMon);
                    if (mon != null) s.MonitorsRow = mon;
                }
                if (s.VialTypeRow == null && s.IrradiationRequestsRow != null)
                {
                    string channel = s.IrradiationRequestsRow.ChannelName;
                    IEnumerable<VialTypeRow> capsules = this.VialType.Where(o => !o.IsVialTypeRefNull() && o.Comments.ToUpper().Contains(channel));  //the capsule for the channel
                    if (capsules.Count() != 0)
                    {
                        LINAA.VialTypeRow c = capsules.FirstOrDefault();
                        if (c != null) s.VialTypeRow = c;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the SampleRow with given sample name
        /// </summary>
        /// <param name="sampleName">name of sample to find</param>
        /// <returns>A SampleRow or null</returns>
        private SubSamplesRow findBySampleName(string sampleName)
        {
            string field = this.tableSubSamples.SubSampleNameColumn.ColumnName;
            string fieldVal = sampleName.Trim().ToUpper();
            return this.tableSubSamples.FirstOrDefault(LINAA.SelectorByField<SubSamplesRow>(fieldVal, field));
        }

        /*
        // private DataColumn[] geometric;
        private DataColumn[] masses;

        public DataColumn[] Masses
        {
            get
            {
                if (masses == null)
                {
                    masses = new DataColumn[] {
                 columnGross1,columnGross2 ,columnGrossAvg };
                }

                return masses;
            }
        }
        */
        private void setIrradiatioRequest(ref IEnumerable<LINAA.SubSamplesRow> samples, int IrrReqID)
        {
            foreach (LINAA.SubSamplesRow s in samples)
            {
                s.IrradiationRequestsID = IrrReqID;
            }
        }
        private void setLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project)
        {
            string _projectNr = System.Text.RegularExpressions.Regex.Replace(project, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            int[] _samplesNrs = null;

            // _samplesNrs = samples.Where(o => o.MonitorsRow == null) .Where(o =>
            // !o.IsSubSampleNameNull()) .SelectMany(o => o.SubSampleName.Replace(_projectNr, null))

            string[] samplesNames = samples.Where(o => o.MonitorsRow == null)
                .Where(o => !o.IsSubSampleNameNull())
                .Select(o => o.SubSampleName).ToArray();
            samplesNames = samplesNames.Select(o => o.Replace(_projectNr, null))
            .ToArray();

            _samplesNrs = samplesNames.Select(o => Convert.ToInt32(o)).ToArray();

            int _lastSampleNr = 1;
            // while (!_samplesNrs.Add(_lastSampleNr)) _lastSampleNr++;
            if (_samplesNrs.Count() != 0) _lastSampleNr = _samplesNrs.Max() + 1;

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.MonitorsRow == null)
                {
                    if (s.IsSubSampleNameNull())
                    {
                        if (_lastSampleNr >= 10) s.SubSampleName = _projectNr + _lastSampleNr.ToString();
                        else s.SubSampleName = _projectNr + "0" + _lastSampleNr.ToString();
                        _lastSampleNr++;
                    }
                    // s.IrradiationCode = project;
                    EC.CheckNull(this.SubSamples.SubSampleNameColumn, s);
                }
                // s.CalcDensity = 0;
            }
        }

        private IList<UnitRow> setUnits(ref IEnumerable<LINAA.SubSamplesRow> samples)
        {
            IList<UnitRow> list = new List<UnitRow>();

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.GetUnitRows().Count() == 0)
                {
                    UnitRow u = this.tableUnit.NewUnitRow();
                    u.ToDo = true;
                    u.LastCalc = DateTime.Now;
                    u.LastChanged = DateTime.Now.AddMinutes(1);
                    u.IrrReqID = s.IrradiationRequestsID;
                    u.SampleID = s.SubSamplesID;
                    this.tableUnit.AddUnitRow(u);
                    ChannelsRow c = s.IrradiationRequestsRow.ChannelsRow;
                    u.SetParent(ref c);
                    list.Add(u);
                }
            }

            return list;
        }
        // / /
    }
}