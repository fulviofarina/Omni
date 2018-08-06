using Rsx.Dumb;
using Rsx.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : ISamples
    {
        /*
        /// <summary>
        /// Returns a IEnumerable of a) IRequestsRows or b) their childs (IPeaksAverages) or c) their
        /// grandchilds (the Peaks), depending on T1
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="toFilter">Rows to filter according to target and return data from</param>
        /// <param name="target">  target to filter the rows</param>
        /// <returns></returns>
                public IEnumerable<T1> Where<T1>(IEnumerable<LINAA.IRequestsAveragesRow> toFilter, string Filterfield, object valueFilter)
        {
           //to filter by target

           //filtered list of isotopes sharing same target

           toFilter = toFilter.Where(LINAA.SelectorByField<IRequestsAveragesRow>(valueFilter,Filterfield));

           //now throw array of isotopes or its childs or granchilds... (ipeaks averages and peaks)
           Type tipo = typeof(T1);
           if (tipo.Equals(typeof(LINAA.IRequestsAveragesRow)))    return toFilter.Cast<T1>();
           else
           {
              System.Collections.Generic.List<T1> output = new List<T1>();
              foreach (LINAA.IRequestsAveragesRow fil in toFilter)
              {
                 if (tipo.Equals(typeof(LINAA.IPeakAveragesRow)))
                 {
                    output.AddRange(fil.GetIPeakAveragesRows().Cast<T1>());
                 }
                 else if (tipo.Equals(typeof(LINAA.IPeakAveragesRow)))
                 {
                    IEnumerable<LINAA.IPeakAveragesRow> aux = fil.GetIPeakAveragesRows();
                    foreach (LINAA.IPeakAveragesRow ip in aux)
                    {
                       output.AddRange(ip.GetPeaksRows().Cast<T1>());
                    }
                 }
              }
              return output.AsEnumerable<T1>();
           }
        }
         */

        /// <summary>
        /// Good
        /// </summary>
        /// <param name="irradiationCode"></param>
        /// <returns></returns>
        public double CalculateAvgOfFCs(string irradiationCode)
        {
            IEnumerable<IRequestsAveragesRow> comparators = this.tableIRequestsAverages.Where(x => !EC.IsNuDelDetch(x));
            return AverageOfFCs(irradiationCode, ref comparators);
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

            PeaksRow[] dpeaks = daugther.GetPeaksRows();

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

                PeaksRow[] Rpeaks = reference.GetPeaksRows();

                foreach (PeaksRow Rp in Rpeaks)
                {
                    //find same measurement
                    Func<PeaksRow, bool> finder = o => o.Measurement.Equals(Rp.Measurement);

                    PeaksRow dp = dpeaks.FirstOrDefault(finder);

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
        public void UpdateIrradiationDates()
        {
            foreach (LINAA.MonitorsRow m in this.tableMonitors.Rows)
            {
                DateTime? dum0 = (DateTime?)this.QTA.GetOutReactorFromSubSampleDescription(m.MonName);
                if (dum0 != null)
                {
                    if ((DateTime)dum0 > m.LastIrradiationDate)
                    {
                        m.LastIrradiationDate = (DateTime)dum0;
                    }
                }
                Int32? dum1 = (Int32?)this.QTA.GetIrqIdFromSubSampleDescription(m.MonName);
                if (dum1 != null)
                {
                    LINAA.IrradiationRequestsRow r = this.IrradiationRequests.FindByIrradiationRequestsID((int)dum1);
                    if (r != null) m.LastProject = r.IrradiationCode;
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
            MonitorsDataTable importing = new MonitorsDataTable(false);

            importing.ReadXml(file);

            foreach (MonitorsRow i in importing)
            {
                MonitorsRow l = this.FindMonitorByName(i.MonName); //local monitor
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
                SubSamplesDataTable newsamples = new SubSamplesDataTable();
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

                UnitDataTable dt = new UnitDataTable();

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
    }
}