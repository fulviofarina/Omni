using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Math;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : ISamples
    {
        public IEnumerable<LINAA.SubSamplesRow> CreateSamplesNamesFrom(ref IEnumerable<string> hsamples)
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

        public void SetIrradiatioRequest(ref IEnumerable<LINAA.SubSamplesRow> samples, int IrrReqID)
        {
            foreach (LINAA.SubSamplesRow s in samples)
            {
                s.IrradiationRequestsID = IrrReqID;
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
                this.tableUnit.AcceptChanges();
                this.tableUnit.EndLoadData();

              //  this.MatSSF.Clear();
                //    Hashtable bindings = Dumb.ArrayOfBindings(ref bs, "N4");
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void AddMonitors(ref IEnumerable<LINAA.SubSamplesRow> samples, string project)
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

        public void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project)
        {
            string _projectNr = System.Text.RegularExpressions.Regex.Replace(project, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Collections.Generic.HashSet<int> _samplesNrs = new System.Collections.Generic.HashSet<int>();

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.MonitorsRow == null)
                {
                    if (!s.IsSubSampleNameNull())
                    {
                        _samplesNrs.Add(Convert.ToInt16(s.SubSampleName.Replace(_projectNr, null)));
                    }
                }
            }

            int _lastSampleNr = 1;

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.MonitorsRow == null)
                {
                    if (s.IsSubSampleNameNull())
                    {
                        while (!_samplesNrs.Add(_lastSampleNr)) _lastSampleNr++;
                        if (_lastSampleNr >= 10) s.SubSampleName = _projectNr + _lastSampleNr.ToString();
                        else s.SubSampleName = _projectNr + "0" + _lastSampleNr.ToString();
                    }
                    // s.IrradiationCode = project;
                    EC.CheckNull(this.SubSamples.SubSampleNameColumn, s);
                }
            }
        }

        public void SetUnits(ref IEnumerable<LINAA.SubSamplesRow> samples)
        {
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
                    u.SetChannel(ref c);
                   
                }
            }
        }

        public int AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> hsamples, bool monitors = false)
        {
            int added = 0;
            if (hsamples.Count() == 0) return added;

            // bool cd = false;
            project = project.ToUpper().Trim();
            int? id = this.IrradiationRequests.FindIrrReqID(project);

            try
            {
                SetIrradiatioRequest(ref hsamples, (int)id);

                if (monitors) AddMonitors(ref hsamples, project);
                else SetLabels(ref hsamples, project);

                SetUnits(ref hsamples);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }

            return added;
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

        public void LoadMonitorsFile(string file)
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

        public void BeginEndLoadData(bool load)
        {
            if (load)
            {
                this.Measurements.BeginLoadData();
                this.Peaks.BeginLoadData();
                this.Samples.BeginLoadData();
                this.IRequestsAverages.BeginLoadData();
                this.IPeakAverages.BeginLoadData();
            }
            else
            {
                this.Measurements.EndLoadData();
                this.Peaks.EndLoadData();
                this.Samples.EndLoadData();
                this.IRequestsAverages.EndLoadData();
                this.IPeakAverages.EndLoadData();
            }

            PopulateSelectedExpression(!load);
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
                this.tableMonitors.EndLoadData();
                tableMonitors.AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateSampleRadioisotopes()
        {
            //LINAATableAdapters.IRequestsAveragesTableAdapter irsTa = new LINAATableAdapters.IRequestsAveragesTableAdapter();
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

                AddSamples(ref newsamples);

                // this.tableSubSamples.TableNewRow -= new DataTableNewRowEventHandler(this.tableSubSamples.SubSamplesDataTable_TableNewRow);
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void AddSamples(ref SubSamplesDataTable newsamples)
        {
            this.tableSubSamples.BeginLoadData();
            this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);
            this.tableSubSamples.EndLoadData();
            this.tableSubSamples.AcceptChanges();
        }
    }
}