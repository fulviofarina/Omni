using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Math;

//using DB.Interfaces;

namespace DB
{
    /// <summary>
    /// THIS IS QUITE CLEAN
    /// </summary>
    public partial class LINAA : ISamples
    {
        //THIS CLASS IS QUITE CLEAN SOME THINGS TO DO
        public SubSamplesRow AddSamples(ref IrradiationRequestsRow ir, string sampleName = "", bool save = true)
        {
            string project = ir.IrradiationCode.Trim().ToUpper();
            return AddSamples(project, sampleName, save);
        }

        public SubSamplesRow AddSamples(string project, string sampleName = "", bool save = true)
        {
            IList<SubSamplesRow> list = new List<SubSamplesRow>();
            SubSamplesRow s = AddSamples(sampleName);
            list.Add(s);
            IEnumerable<SubSamplesRow> samples = list;
            AddSamples(project, ref samples);
            return s;
        }

        public SubSamplesRow AddSamples(string sampleName = "")
        {
            LINAA.SubSamplesRow s = null;
            s = this.SubSamples.NewSubSamplesRow();
            s.SetName(sampleName);
            this.SubSamples.AddSubSamplesRow(s);
            return s;
        }

        public IEnumerable<LINAA.SubSamplesRow> AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> samplesToImport, bool save = true)
        {
            // project = project.ToUpper().Trim();

            IrradiationRequestsRow ir = this.FindIrradiationByCode(project);

            samplesToImport = AddSamples(ref ir, ref samplesToImport, save);
            return samplesToImport;
        }

        private IEnumerable<SubSamplesRow> AddSamples(ref IrradiationRequestsRow ir, ref IEnumerable<SubSamplesRow> samplesToImport, bool save = true)
        {
            IEnumerable<SubSamplesRow> samplesInTable = this.FindSamplesByIrrReqID(ir.IrradiationRequestsID);
            //join them if any
            if (samplesToImport != null && samplesToImport.Count() != 0)
            {
                samplesToImport = samplesToImport.Union(samplesInTable);
            }
            else samplesToImport = samplesInTable;

            string project = ir.GetIrradiationCode();
            int _lastSampleNr = GetLastSampleNr(ref samplesToImport, project);

            //set irr request BASIC
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                s.SetParent(ref ir);
            }
            //set monitors
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                bool attachMon = EC.IsNuDelDetch(s.MonitorsRow);
                //attach monitor
                if (attachMon)
                {
                    string monName = s.GetMonitorNameFromSampleName();
                    //find monitor if any
                    LINAA.MonitorsRow mon = this.FindMonitorByName(monName);
                    s.SetParent(ref mon);
                }
                s.SetName(ref _lastSampleNr);
            }

            /*
            //set vials
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                //attach vial
                bool attachRabbit = (EC.IsNuDelDetch(s.ChCapsuleRow));
                attachRabbit = attachRabbit && !EC.IsNuDelDetch(s.IrradiationRequestsRow);
                if (attachRabbit)
                {
                    string channel = s.IrradiationRequestsRow.ChannelsRow.ChannelName;
                    IEnumerable<VialTypeRow> capsules = FindCapsules(channel);
                    LINAA.VialTypeRow c = capsules.FirstOrDefault();
                    s.SetParent(ref c);

                //    s.ChCapsuleRow;
                }

            }
            */


            Save(ref samplesToImport);

            AddUnits(ref samplesToImport);




            return samplesToImport;
        }

        private int GetLastSampleNr(ref IEnumerable<SubSamplesRow> samplesToImport, string project)
        {
            string _projectNr = System.Text.RegularExpressions.Regex.Replace(project, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string[] samplesNames = GetSampleNames(ref samplesToImport, _projectNr);
            int[] _samplesNrs = GetSampleNames(samplesNames);
            int _lastSampleNr = GetLastSampleNr(_samplesNrs);
            return _lastSampleNr;
        }


        public void AddUnits(ref IEnumerable<SubSamplesRow> samplesToImport)
        {
            foreach (LINAA.SubSamplesRow s in samplesToImport)
            {
                //attach unit
                bool attachUnit = EC.IsNuDelDetch(s.UnitRow);
                if (attachUnit)
                {
                    UnitRow u = this.Unit.NewUnitRow();
                    this.Unit.AddUnitRow(u);
                    LINAA.SubSamplesRow sample = s;
                    u.SetParent(ref sample);
                 
                }
            }
            IEnumerable<UnitRow> units = samplesToImport.Select(o => o.UnitRow);
            Save(ref units);

        }

        public IEnumerable<LINAA.SubSamplesRow> AddSamples(ref IEnumerable<string> hsamples, string project, bool salve = true)
        {
            IList<LINAA.SubSamplesRow> ls = new List<SubSamplesRow>();
            foreach (string sname in hsamples)
            {
                SubSamplesRow s = AddSamples(sname);
                ls.Add(s);
            }
            IEnumerable<SubSamplesRow> samps = ls;
            return AddSamples(project, ref samps, salve);
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

            double Ld = 0.693 / daugther.k0NAARow.NAARowParent.T3;
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
                double Lref = 0.693 / reference.k0NAARow.NAARowParent.T2;
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

        public MonitorsRow FindMonitorByName(string MonName)
        {
            string field = this.Monitors.MonNameColumn.ColumnName;
            return this.Monitors.FirstOrDefault(LINAA.SelectorByField<MonitorsRow>(MonName.Trim().ToUpper(), field));
        }

        public IEnumerable<SubSamplesRow> FindByProject(string project)
        {
            IEnumerable<SubSamplesRow> old = null;
            string cd = DB.Properties.Misc.Cd;
            string IrReqField = this.tableSubSamples.IrradiationCodeColumn.ColumnName;
            project = project.Replace(cd, null);
            old = this.tableSubSamples
                .Where(LINAA.SelectorByField<SubSamplesRow>(project, IrReqField))
                .ToList();
            IEnumerable<SubSamplesRow> oldCD = this.tableSubSamples
                .Where(LINAA.SelectorByField<SubSamplesRow>(project + cd, IrReqField));

            old = old.Union(oldCD);

            return old.ToList();
        }

        /// <summary>
        /// Finds the SampleRow with given sample name, or adds it if not found and specifically
        /// requested, using the IrrReqID given
        /// </summary>
        /// <param name="sampleName">name of sample to find</param>
        /// <param name="AddifNull"> true for adding the row if not found</param>
        /// <param name="IrrReqID">  irradiation request id to set for the sample only if added</param>
        /// <returns>A non-null SampleRow if AddIfNull is true, otherwise can be null</returns>
        public SubSamplesRow FindSample(string sampleName, bool AddifNull = false, int? IrrReqID = null)
        {
            SubSamplesRow sample = FindSample(sampleName);

            if (sample == null && AddifNull)
            {
                sample = this.tableSubSamples.NewSubSamplesRow();
                sample.SetIrradiationRequestID(IrrReqID);
                this.tableSubSamples.AddSubSamplesRow(sample);
            }
            sample.SetName(sampleName);
            //  sample.SubSampleName = sampleName;
            sample.SetCreationDate();
           
            return sample;
        }

        private SubSamplesRow FindSample(string sampleName)
        {
            string field = this.tableSubSamples.SubSampleNameColumn.ColumnName;
            string fieldVal = sampleName.Trim().ToUpper();
            SubSamplesRow sample = this.tableSubSamples
                .FirstOrDefault(LINAA.SelectorByField<SubSamplesRow>(fieldVal, field));
            return sample;
        }


        public IEnumerable<VialTypeRow> FindCapsules(string coment)
        {
            return this.VialType.Where(o => !o.IsVialTypeRefNull() && o.Comments.ToUpper().Contains(coment));
        }

        /*
public IEnumerable<SubSamplesRow> FindSamplesByIrrReqID(int? id)
{
   //find the ones that are already here
   IEnumerable<SubSamplesRow> samplesInTable = SubSamples.OfType<SubSamplesRow>()
       .Where(o =>!EC.IsNuDelDetch(o));
   samplesInTable = samplesInTable.Where(o => !o.IsIrradiationRequestsIDNull() && o.IrradiationRequestsID == id);
   ///hsamples.un =
   return samplesInTable;
}
*/

       

        public int GetLastSampleNr(int[] _samplesNrs)
        {
            int _lastSampleNr = 1;
            // while (!_samplesNrs.Add(_lastSampleNr)) _lastSampleNr++;
            if (_samplesNrs.Count() != 0)
            {
                _lastSampleNr = _samplesNrs.Max() + 1;
            }

            return _lastSampleNr;
        }

        private static int[] GetSampleNames(string[] samplesNames)
        {
            return samplesNames.Select(o => Convert.ToInt32(o)).ToArray();
        }

        public  string[] GetSampleNames(ref IEnumerable<SubSamplesRow> samples, string _projectNr)
        {
            string[] samplesNames = samples.Where(o => o.MonitorsRow == null)
                .Where(o => !o.IsSubSampleNameNull())
                .Select(o => o.SubSampleName).ToArray();
            samplesNames = samplesNames.Select(o => o.Replace(_projectNr, null))
            .ToArray();
            return samplesNames;
        }

        public IEnumerable<SubSamplesRow> FindSamplesByIrrReqID(int? IrReqID)
        {
            IEnumerable<SubSamplesRow> old = null;
            string IrReqField = this.tableSubSamples.IrradiationRequestsIDColumn.ColumnName;
            old = this.tableSubSamples
                .Where(LINAA.SelectorByField<SubSamplesRow>(IrReqID, IrReqField));
            return old.ToList();
        }

        public bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples)
        {
            foreach (LINAA.SubSamplesRow row in this.tableSubSamples)
            {
                row.SetOverride(Alpha, f, Geo, Gt, asSamples);
            }
            return this.HasErrors;
        }
    }

    public partial class LINAA : ISamples
    {
        //
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

        public void PopulatedMonitors(string file)
        {
            LINAA.MonitorsDataTable importing = new LINAA.MonitorsDataTable(false);

            importing.ReadXml(file);

            foreach (LINAA.MonitorsRow i in importing)
            {
                LINAA.MonitorsRow l = this.FindMonitorByName(i.MonName); //local monitor
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
                // IN THIS ORDER
                PopulateUnitsByProject(IrReqID);

                //2
                TAM.SubSamplesTableAdapter.DeleteNulls();
                LINAA.SubSamplesDataTable newsamples = new SubSamplesDataTable();
                TAM.SubSamplesTableAdapter.FillByID(newsamples, IrReqID);

                /*
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
              */
                this.tableSubSamples.BeginLoadData();
                this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);
                this.tableSubSamples.AcceptChanges();
                this.tableSubSamples.EndLoadData();
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

     
    }
}