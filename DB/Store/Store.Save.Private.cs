using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;

//using DB.Interfaces;
using Rsx.Dumb;
using System.IO;

namespace DB
{
 
    public partial class LINAA
    {
        private void save<T>(ref IEnumerable<T> rows)
        {
            try
            {
                bool wasPeaks = false; //tells if the rows were peaks Family of rows
                wasPeaks = savePeaks<T>(ref rows);
                if (!wasPeaks)
                {
                    saveOthers(ref rows);
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            // return rows;
        }

        private void saveOthers<T>(ref IEnumerable<T> rows)
        {
            Type t = rows.First().GetType();
            ///deleted now (2 may 2012)
            DataRow[] schs = rows.OfType<DataRow>().Where(r => Changes.HasChanges(r)).ToArray();

            // LINAATableAdapters.UnitTableAdapter uta = new LINAATableAdapters.UnitTableAdapter();
            if (t.Equals(typeof(SubSamplesRow)))
            {
                IEnumerable<SubSamplesRow> samps = schs.Cast<SubSamplesRow>();
                saveSamples(ref samps);
                // IEnumerable<UnitRow> units = samps.SelectMany(o =>
                // o.GetUnitRows()).ToArray();//.UnitRow).ToArray(); saveOthers(ref units);
            }
            else if (t.Equals(typeof(SchAcqsRow))) this.tAM.SchAcqsTableAdapter.Update(schs);
            else if (t.Equals(typeof(OrdersRow))) this.tAM.OrdersTableAdapter.Update(schs);
            else if (t.Equals(typeof(ProjectsRow))) this.tAM.ProjectsTableAdapter.Update(schs);
            else if (t.Equals(typeof(MonitorsRow))) this.tAM.MonitorsTableAdapter.Update(schs);
            else if (t.Equals(typeof(SamplesRow)))
            {
                this.tAM.SamplesTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(MonitorsFlagsRow))) this.tAM.MonitorsFlagsTableAdapter.Update(schs);
            else if (t.Equals(typeof(StandardsRow))) this.tAM.StandardsTableAdapter.Update(schs);
            else if (t.Equals(typeof(MatrixRow)))
            {
                // LINAATableAdapters.MatrixTableAdapter ta = new LINAATableAdapters.MatrixTableAdapter();
                this.tAM.MatrixTableAdapter.Update(schs);
                // ta.Update(schs);

                // ta.Dispose(); ta = null;
            }
            // else if (t.Equals(typeof(MatSSFRow))) this.tAM.MatSSFTableAdapter.Update(schs);
            else if (t.Equals(typeof(RefMaterialsRow))) this.tAM.RefMaterialsTableAdapter.Update(schs);
            else if (t.Equals(typeof(UnitRow)))
            {
                this.tAM.UnitTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(GeometryRow))) this.tAM.GeometryTableAdapter.Update(schs);
            else if (t.Equals(typeof(IrradiationRequestsRow))) this.tAM.IrradiationRequestsTableAdapter.Update(schs);
            else if (t.Equals(typeof(ChannelsRow))) this.tAM.ChannelsTableAdapter.Update(schs);
            else if (t.Equals(typeof(DetectorsAbsorbersRow))) this.tAM.DetectorsAbsorbersTableAdapter.Update(schs);
            else if (t.Equals(typeof(DetectorsCurvesRow))) this.tAM.DetectorsCurvesTableAdapter.Update(schs);
            else if (t.Equals(typeof(DetectorsDimensionsRow))) this.tAM.DetectorsDimensionsTableAdapter.Update(schs);
            // else if (t.Equals(typeof(AcquisitionsRow))) this.tAM.AcquisitionsTableAdapter.Update(schs);
            else if (t.Equals(typeof(HoldersRow))) this.tAM.HoldersTableAdapter.Update(schs);
            else if (t.Equals(typeof(MeasurementsRow)))
            {
                this.tAM.MeasurementsTableAdapter.SetForLIMS();// Connection.ConnectionString = DB.Properties.Settings.Default.NAAConnectionString;
                this.tAM.MeasurementsTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(ToDoRow))) this.tAM.ToDoTableAdapter.Update(schs);
            else if (t.Equals(typeof(VialTypeRow))) this.tAM.VialTypeTableAdapter.Update(schs);
            else if (t.Equals(typeof(YieldsRow)))
            {
                this.tAM.YieldsTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(ReactionsRow)))
            {
                this.tAM.ReactionsTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(SigmasRow)))
            {
                this.tAM.SigmasTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(SigmasSalRow)))
            {
                this.tAM.SigmasSalTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(NAARow)))
            {
                this.tAM.NAATableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(k0NAARow)))
            {
                this.tAM.k0NAATableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(pValuesRow)))
            {
                this.tAM.pValuesTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(ElementsRow)))
            {
                this.tAM.ElementsTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(tStudentRow)))
            {
                this.tAM.tStudentTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(BlanksRow)))
            {
                this.tAM.BlanksTableAdapter.Update(schs);
            }
            else if (t.Equals(typeof(CompositionsRow)))
            {
                // this.tAM.CompositionsTableAdapter.Update(schs); string path = folderPath +
                // DB.Properties.Resources.Backups + "Compositions.xml";
                // this.tableCompositions.AcceptChanges(); this.tableCompositions.WriteXml(path);
            }
            else if (t.Equals(typeof(ExceptionsRow)))
            {
                SaveExceptions();
            }
            else throw new SystemException("Not implemented. Save<> Method");
        }

        private bool savePeaks<T>(ref IEnumerable<T> rows)
        {
            bool wasPeaks = false;
            Type t = typeof(T);

            if (t.Equals(typeof(IPeakAveragesRow)))
            {
                IEnumerable<IPeakAveragesRow> ipeaks = rows.Cast<IPeakAveragesRow>();
                this.savePeaksAvg(ref ipeaks);
                wasPeaks = true;
            }
            else if (t.Equals(typeof(IRequestsAveragesRow)))
            {
                IEnumerable<IRequestsAveragesRow> ires = rows.Cast<IRequestsAveragesRow>();
                this.savePeaksIrrAvg(ref ires);
                wasPeaks = true;
            }
            else if (t.Equals(typeof(PeaksRow)))
            {
                IEnumerable<PeaksRow> peaks = rows.Cast<PeaksRow>();
                this.savePeaksRow(ref peaks);
                wasPeaks = true;
            }

            return wasPeaks;
        }
  
        private bool saveMUES_File(ref MUESDataTable mu, MatrixRow m)
        {
            byte[] arr = Rsx.Dumb.Tables.MakeDTBytes(ref mu);

            string tempfile = folderPath + Resources.XCOMFolder;
            tempfile += m.MatrixID;

            File.WriteAllBytes(tempfile, arr);

            return File.Exists(tempfile);
        }

        private void saveMUES_SQL(ref MUESDataTable mu, ref MatrixRow m)
        {
            LINAATableAdapters.MUESTableAdapter muta = new LINAATableAdapters.MUESTableAdapter();
            muta.DeleteByMatrixID(m.MatrixID);
            foreach (MUESRow row in mu.Rows)
            {
                muta.Insert(row.MatrixID, row.Energy, row.MACS, row.MAIS, row.PE, row.PPNF, row.PPEF, row.MATCS, row.MATNCS, row.Density, row.Edge);
            }
            muta.Dispose();
            muta = null;
        }

        protected internal void savePeaksIrrAvg(ref IEnumerable<IRequestsAveragesRow> irequests)
        {
            LINAATableAdapters.IRequestsAveragesTableAdapter ta = new LINAATableAdapters.IRequestsAveragesTableAdapter();

            try
            {
                IEnumerable<IRequestsAveragesRow> deleteIR = (irequests).Where(ir => ir.RowState == DataRowState.Deleted || ir.GetIPeakAveragesRows().Count() == 0);

                for (int i = deleteIR.Count() - 1; i >= 0; i--)
                {
                    try
                    {
                        IRequestsAveragesRow ip = deleteIR.ElementAt(i);
                        ip.RejectChanges();
                        ta.DeleteItem(ip.Sample, ip.NAAID);
                        ip.Delete();
                        ip.AcceptChanges();
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                    }
                }

                IEnumerable<IRequestsAveragesRow> toupdateIR = irequests.Where(ir => ir.RowState == DataRowState.Added || ir.RowState == DataRowState.Modified);
                foreach (IRequestsAveragesRow ip in toupdateIR)
                {
                    try
                    {
                        ta.DeleteItem(ip.Sample, ip.NAAID);
                        ta.InsertItem(ip.Sample, ip.SD, ip.ObsSD, ip.R0, ip.R1, ip.Ge, ip.Asp, ip.NAAID);
                        ip.AcceptChanges();
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                        EC.SetRowError(ip, ex);
                    }
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            ta.Dispose();
            ta = null;
        }

        protected internal void savePeaksAvg(ref IEnumerable<IPeakAveragesRow> iavgs)
        {
            LINAATableAdapters.IPeakAveragesTableAdapter ta = new LINAATableAdapters.IPeakAveragesTableAdapter();
            try
            {
                IEnumerable<IPeakAveragesRow> deleted = iavgs.Where(ir => ir.RowState == DataRowState.Deleted || ir.GetPeaksRows().Count() == 0 || ir.IRequestsAveragesRowParent == null);

                for (int i = deleted.Count() - 1; i >= 0; i--)
                {
                    IPeakAveragesRow ip = deleted.ElementAt(i);
                    try
                    {
                        ip.RejectChanges();
                        ta.DeleteItem(ip.Sample, ip.k0ID);
                        ip.Delete();
                        ip.AcceptChanges();
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                    }
                }

                IEnumerable<IPeakAveragesRow> toupdate = iavgs.Where(ir => ir.RowState == DataRowState.Added || ir.RowState == DataRowState.Modified);
                foreach (IPeakAveragesRow ip in toupdate)
                {
                    try
                    {
                        ta.DeleteItem(ip.Sample, ip.k0ID);
                        ta.InsertItem(ip.Sample, ip.SD, ip.ObsSD, ip.GActUnc, ip.X, ip.k0ID);
                        ip.AcceptChanges();
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                        EC.SetRowError(ip, ex);
                    }
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            ta.Dispose();
            ta = null;
        }

        protected internal void savePeaksRow(ref IEnumerable<PeaksRow> peaks)
        {
            LINAATableAdapters.PeaksTableAdapter ta = new LINAATableAdapters.PeaksTableAdapter();

            try
            {
                IEnumerable<PeaksRow> deleted = peaks.Where(r => r.RowState == DataRowState.Deleted || r.IPeakAveragesRowParent == null || r.MeasurementsRow == null || r.IRequestsAveragesRowParent == null);
                for (int i = deleted.Count() - 1; i >= 0; i--)
                {
                    try
                    {
                        PeaksRow p = deleted.ElementAt(i);
                        p.RejectChanges();
                        ta.Delete(p.PeaksID, p.MeasurementID, p.SampleID, p.ID);
                        p.Delete();
                        p.AcceptChanges();
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                    }
                }

                IEnumerable<PeaksRow> toupdate = peaks.Where(ir => ir.RowState == DataRowState.Added || ir.RowState == DataRowState.Modified);
                foreach (PeaksRow p in toupdate)
                {
                    try
                    {
                        ta.Delete(p.PeaksID, p.MeasurementID, p.SampleID, p.ID);
                        ta.Insert(p.MeasurementID, p.SampleID, p.IrradiationID, p.Area, p.AreaUncertainty, p.Energy, p.PeaksID, p.Efficiency, p.COI, p.ID, p.T0, 0);
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                        EC.SetRowError(p, ex);
                    }
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            ta.Dispose();
            ta = null;
        }

        protected internal void saveSamples(ref IEnumerable<SubSamplesRow> samps)
        {
            // LINAATableAdapters.SubSamplesTableAdapter ta = new LINAATableAdapters.SubSamplesTableAdapter();

            try
            {
                // SubSamples.EndLoadData();

                TAM.SubSamplesTableAdapter.Update(samps.ToArray());
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            // ta.Dispose(); ta = null;
        }

      
    }
}