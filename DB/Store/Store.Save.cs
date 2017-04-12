using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IStore
    {
        private bool Usehandlers = false;

        public bool Save<T>()
        {
            T table = this.Tables.OfType<T>().FirstOrDefault();

            DataTable dt = table as DataTable;
            if (dt.Rows.Count == 0) return true;

            IEnumerable<DataRow> rows = dt.AsEnumerable();

            bool saved = Save(ref rows);

            return saved;
        }

        public bool Save(string file)
        {
            bool saved = false;

            try
            {
                System.IO.File.Delete(file);
                this.WriteXml(file, XmlWriteMode.WriteSchema);
                saved = true;
                //Msg("Database has been updated to a file!\n\n" + file, "Saved");
            }
            catch (SystemException ex)
            {
             //   Msg(ex.StackTrace, ex.Message);
                AddException(ex);
            }

            return saved;
        }

        public bool Save<T>(ref IEnumerable<T> rows)
        {
            if (rows == null || rows.Count() == 0) return false;
            rows = rows.ToList();
            Type t = rows.First().GetType();

            DataTable dt = (rows.First() as DataRow).Table;

            if (Usehandlers)
            {
                Handlers(false, ref dt);
            }
            RowHandlers(ref dt, false);

            dt.BeginLoadData();
            try
            {
                if (t.Equals(typeof(IPeakAveragesRow)))
                {
                    IEnumerable<IPeakAveragesRow> ipeaks = rows.Cast<IPeakAveragesRow>();
                    this.SavePeaks(ref ipeaks);
                }
                else if (t.Equals(typeof(IRequestsAveragesRow)))
                {
                    IEnumerable<IRequestsAveragesRow> ires = rows.Cast<IRequestsAveragesRow>();
                    this.SavePeaks(ref ires);
                }
                else if (t.Equals(typeof(PeaksRow)))
                {
                    IEnumerable<PeaksRow> peaks = rows.Cast<PeaksRow>();
                    this.SavePeaks(ref peaks);
                }
                else if (t.Equals(typeof(SubSamplesRow)))
                {
                    IEnumerable<SubSamplesRow> samps = rows.Cast<SubSamplesRow>();
                    this.SaveSamples(ref samps);
                }
                else
                {
                    ///delete new, I think is better the previous one
                    //DataRow[] schs = ((IEnumerable<DataRow>)rows).ToArray();

                    ///deleted now (2 may 2012)
                    DataRow[] schs = rows.OfType<DataRow>().Where(r => Dumb.HasChanges(r)).ToArray();

                    ///deleted before
                    //	 Func<DataRow, bool> chsel = Dumb.ChangesSelector(dt);
                    //	 schs = schs.Where(chsel).ToArray();
                    //  if (schs.Count() != 0)
                    //  {
                    if (t.Equals(typeof(SchAcqsRow))) this.tAM.SchAcqsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(OrdersRow))) this.tAM.OrdersTableAdapter.Update(schs);
                    else if (t.Equals(typeof(ProjectsRow))) this.tAM.ProjectsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(MonitorsRow))) this.tAM.MonitorsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(SamplesRow)))
                    {
                        this.tAM.SamplesTableAdapter.Update(schs);
                    }
                    else if (t.Equals(typeof(MonitorsFlagsRow))) this.tAM.MonitorsFlagsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(StandardsRow))) this.tAM.StandardsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(GeometryRow))) this.tAM.GeometryTableAdapter.Update(schs);
                    else if (t.Equals(typeof(IrradiationRequestsRow))) this.tAM.IrradiationRequestsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(ChannelsRow))) this.tAM.ChannelsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(DetectorsAbsorbersRow))) this.tAM.DetectorsAbsorbersTableAdapter.Update(schs);
                    else if (t.Equals(typeof(DetectorsCurvesRow))) this.tAM.DetectorsCurvesTableAdapter.Update(schs);
                    else if (t.Equals(typeof(DetectorsDimensionsRow))) this.tAM.DetectorsDimensionsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(AcquisitionsRow))) this.tAM.AcquisitionsTableAdapter.Update(schs);
                    else if (t.Equals(typeof(HoldersRow))) this.tAM.HoldersTableAdapter.Update(schs);
                    else if (t.Equals(typeof(MatrixRow))) this.tAM.MatrixTableAdapter.Update(schs);
                    else if (t.Equals(typeof(MatSSFRow))) this.tAM.MatSSFTableAdapter.Update(schs);
                    else if (t.Equals(typeof(RefMaterialsRow))) this.tAM.MatSSFTableAdapter.Update(schs);
                    else if (t.Equals(typeof(UnitRow)))
                    {
                        /*
                        foreach (DataRow r in schs)
                        {
                            UnitRow u = (UnitRow)r;
                            u.LastChanged = DateTime.Now;
                        }
                        */
                        this.tAM.UnitTableAdapter.Update(schs);
                    }
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
                    else if (t.Equals(typeof(CompositionsRow)))
                    {
                        this.tAM.CompositionsTableAdapter.Update(schs);
                        // string path = folderPath + DB.Properties.Resources.Backups + "Compositions.xml";
                        //  this.tableCompositions.AcceptChanges();
                        // this.tableCompositions.WriteXml(path);
                    }
                    else throw new SystemException("Not implemented. Save<> Method");
                    //   }

                    //	 Usehandlers = true;
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
                //   return false;
            }

            if (Usehandlers)
            {
                Handlers(true, ref dt);

                Usehandlers = false;
            }
            else
            {
                Usehandlers = true;
                Save(ref rows);
            }

            RowHandlers(ref dt, true);

            dt.EndLoadData();

            return true;
        }

        public string SaveExceptions()
        {
            string path = string.Empty;
            try
            {
                this.Exceptions.RemoveDuplicates();
                if (this.Exceptions.Rows.Count != 0)
                {
                    long now = DateTime.Now.ToFileTimeUtc();
                    path = folderPath + DB.Properties.Resources.Exceptions + "Exceptions." + now + ".xml";
                    this.Exceptions.WriteXml(path, XmlWriteMode.WriteSchema, false);
                }
                this.Exceptions.Clear();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
                path = string.Empty;
            }

            return path;
        }

        public bool SaveLocalCopy()
        {
            bool ok = false;
            try
            {
                string LIMSPath = folderPath + DB.Properties.Resources.Linaa;
                string aux = "." + DateTime.Now.DayOfYear.ToString();
                string LIMSDayPath = LIMSPath.Replace(".xml", aux + ".xml");

                if (System.IO.File.Exists(LIMSPath))
                {
                    System.IO.File.Copy(LIMSPath, LIMSDayPath, true);
                    System.IO.File.Delete(LIMSPath);
                }
                WriteXml(LIMSPath, XmlWriteMode.WriteSchema);
                SaveExceptions();
                ok = true;
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return ok;
        }

        public bool SaveRemote(ref IEnumerable<DataTable> tables, bool takeChanges)
        {
            bool ok = false;
            try
            {
                if (takeChanges)
                {
                    foreach (System.Data.DataTable t in tables)
                    {
                        IEnumerable<DataRow> rows = t.AsEnumerable();
                        Save(ref rows);
                    }
                    ok = true;
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return ok;
        }

        public void SaveSSF()
        {
            try
            {
                Save<LINAA.MatrixDataTable>();
                Save<LINAA.VialTypeDataTable>();
                Save<LINAA.UnitDataTable>();
                Save<LINAA.ChannelsDataTable>();
            }
            catch (SystemException ex)
            {
              //  Msg(ex.StackTrace, ex.Message);
                this.AddException(ex);
            }
        }

        public bool SaveSSF(bool offline, string file)
        {
            bool save = false;

            if (!offline)
            {
                SaveSSF();
            }
            else
            {
                //writes the xml file (Offline)
                Save(file);
            }

            save = true;

            return save;
        }

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

        /*
        public bool SaveOLD<T>(ref IEnumerable<T> rows)
        {
            if (rows == null || rows.Count() == 0) return false;
            rows = rows.ToList();
            Type t = rows.First().GetType();

            DataTable dt = (rows.First() as DataRow).Table;

            if (Usehandlers)
            {
                Handlers(false, ref  dt);
                RowHandlers(ref  dt, false);
            }
            dt.BeginLoadData();
            try
            {
                if (t.Equals(typeof(IPeakAveragesRow)))
                {
                    IEnumerable<IPeakAveragesRow> ipeaks = rows.Cast<IPeakAveragesRow>();
                    this.SavePeaks(ref ipeaks);
                }
                else if (t.Equals(typeof(IRequestsAveragesRow)))
                {
                    IEnumerable<IRequestsAveragesRow> ires = rows.Cast<IRequestsAveragesRow>();
                    this.SavePeaks(ref ires);
                }
                else if (t.Equals(typeof(PeaksRow)))
                {
                    IEnumerable<PeaksRow> peaks = rows.Cast<PeaksRow>();
                    this.SavePeaks(ref peaks);
                }
                else if (t.Equals(typeof(SubSamplesRow)))
                {
                    IEnumerable<SubSamplesRow> samps = rows.Cast<SubSamplesRow>();
                    this.SaveSamples(ref samps);
                }
                else
                {
                    ///delete new, I think is better the previous one
                    //DataRow[] schs = ((IEnumerable<DataRow>)rows).ToArray();

                    ///deleted now (2 may 2012)
                    DataRow[] schs = rows.OfType<DataRow>().Where(r => Dumb.HasChanges(r)).ToArray();

                    ///deleted before
                    //	 Func<DataRow, bool> chsel = Dumb.ChangesSelector(dt);
                    //	 schs = schs.Where(chsel).ToArray();
                    //  if (schs.Count() != 0)
                    {
                        if (t.Equals(typeof(SchAcqsRow))) this.tAM.SchAcqsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(OrdersRow))) this.tAM.OrdersTableAdapter.Update(schs);
                        else if (t.Equals(typeof(ProjectsRow))) this.tAM.ProjectsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(MonitorsRow))) this.tAM.MonitorsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(SamplesRow))) this.tAM.SamplesTableAdapter.Update(schs);
                        else if (t.Equals(typeof(MonitorsFlagsRow))) this.tAM.MonitorsFlagsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(StandardsRow))) this.tAM.StandardsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(GeometryRow))) this.tAM.GeometryTableAdapter.Update(schs);
                        else if (t.Equals(typeof(IrradiationRequestsRow))) this.tAM.IrradiationRequestsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(ChannelsRow))) this.tAM.ChannelsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(DetectorsAbsorbersRow))) this.tAM.DetectorsAbsorbersTableAdapter.Update(schs);
                        else if (t.Equals(typeof(DetectorsCurvesRow))) this.tAM.DetectorsCurvesTableAdapter.Update(schs);
                        else if (t.Equals(typeof(DetectorsDimensionsRow))) this.tAM.DetectorsDimensionsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(AcquisitionsRow))) this.tAM.AcquisitionsTableAdapter.Update(schs);
                        else if (t.Equals(typeof(HoldersRow))) this.tAM.HoldersTableAdapter.Update(schs);
                        else if (t.Equals(typeof(MatrixRow))) this.tAM.MatrixTableAdapter.Update(schs);
                        else if (t.Equals(typeof(MatSSFRow))) this.tAM.MatSSFTableAdapter.Update(schs);
                        else if (t.Equals(typeof(RefMaterialsRow))) this.tAM.MatSSFTableAdapter.Update(schs);
                        else if (t.Equals(typeof(MeasurementsRow)))
                        {
                            this.tAM.MeasurementsTableAdapter.Connection.ConnectionString = DB.Properties.Settings.Default.NAAConnectionString;
                            this.tAM.MeasurementsTableAdapter.Update(schs);
                        }
                        else if (t.Equals(typeof(ToDoRow))) this.tAM.ToDoTableAdapter.Update(schs);
                        else if (t.Equals(typeof(VialTypeRow))) this.tAM.VialTypeTableAdapter.Update(schs);
                        else if (t.Equals(typeof(YieldsRow)))
                        {
                            this.tAM.YieldsTableAdapter.Update(schs);
                        }
                        else if (t.Equals(typeof(CompositionsRow)))
                        {
                            this.tAM.CompositionsTableAdapter.Update(schs);
                            // string path = folderPath + DB.Properties.Resources.Backups + "Compositions.xml";
                            //  this.tableCompositions.AcceptChanges();
                            // this.tableCompositions.WriteXml(path);
                        }
                        else if (t.Equals(typeof(PreferencesRow)))
                        {
                            this.SavePreferences();
                        }
                        else throw new SystemException("Not implemented. Save<> Method");
                    }

                    //	 Usehandlers = true;
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
                //   return false;
            }

            if (Usehandlers)
            {
                Handlers(true, ref  dt);
                RowHandlers(ref  dt, true);
                Usehandlers = false;
            }
            else
            {
                Usehandlers = true;
                Save(ref rows);
            }

            dt.EndLoadData();

            return true;
        }
        */

        protected internal void SavePeaks(ref IEnumerable<IRequestsAveragesRow> irequests)
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

        protected internal void SavePeaks(ref IEnumerable<IPeakAveragesRow> iavgs)
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

        protected internal void SavePeaks(ref IEnumerable<PeaksRow> peaks)
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

        private void SaveSamples(ref IEnumerable<SubSamplesRow> samps)
        {
            LINAATableAdapters.SubSamplesTableAdapter ta = new LINAATableAdapters.SubSamplesTableAdapter();

            LINAATableAdapters.UnitTableAdapter uta = new LINAATableAdapters.UnitTableAdapter();

            try
            {
                IEnumerable<SubSamplesRow> deleteIR = (samps).Where(ir => ir.RowState == DataRowState.Deleted);

                for (int i = deleteIR.Count() - 1; i >= 0; i--)
                {
                    try
                    {
                        SubSamplesRow ip = deleteIR.ElementAt(i);
                        ip.RejectChanges();

                        //delete units
                        IEnumerable<UnitRow> us = ip.GetUnitRows();
                        uta.DeleteBySampleID(ip.SubSamplesID);
                        Delete(ref us);
                        Dumb.AcceptChanges(ref us);

                        ta.Delete(ip.SubSamplesID);
                        ip.Delete();
                        ip.AcceptChanges();
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                    }
                }

                IEnumerable<SubSamplesRow> toupdate = samps.ToList();
                foreach (SubSamplesRow i in toupdate)
                {
                    try
                    {
                        bool added = (i.RowState == DataRowState.Added);
                        int? capsID = null;
                        int? sampsID = null;
                        int? matId = null;
                        int? stdId = null;
                        int? refId = null;
                        int? monId = null;
                        int? blkId = null;
                        int? chCapsID = null;
                        DateTime? inre = null;
                        DateTime? outre = null;

                        if (!i.IsChCapsuleIDNull()) chCapsID = (int?)i.ChCapsuleID;
                        if (!i.IsCapsulesIDNull()) capsID = (int?)i.CapsulesID;
                        if (!i.IsSamplesIDNull()) sampsID = (int?)i.SamplesID;
                        if (!i.IsMatrixIDNull()) matId = (int?)i.MatrixID;
                        if (!i.IsStandardsIDNull()) stdId = (int?)i.StandardsID;
                        if (!i.IsReferenceMaterialIDNull()) refId = (int?)i.ReferenceMaterialID;
                        if (!i.IsMonitorsIDNull()) monId = (int?)i.MonitorsID;
                        if (!i.IsBlankIDNull()) blkId = (int?)i.BlankID;
                        if (!i.IsInReactorNull()) inre = (DateTime?)i.InReactor;
                        if (!i.IsOutReactorNull()) outre = (DateTime?)i.OutReactor;

                        if (added)
                        {
                            ta.Insert(i.SubSampleName, i.SubSampleDescription, i.SubSampleCreationDate, sampsID, capsID, i.GeometryName, matId, i.Gross1, i.Gross2, i.Tare, i.MoistureContent, i.FillHeight, i.IrradiationRequestsID, monId, refId, stdId, blkId, i.Gthermal, i.Radius, i.DirectSolcoi, inre, outre, i.Concentration, i.ENAA, chCapsID);
                            int? ID = (int?)ta.GetSubSampleID(i.SubSampleName);
                            if (ID != null)
                            {
                                SubSamplesDataTable dt = (SubSamplesDataTable)i.Table;
                                dt.SubSamplesIDColumn.ReadOnly = false;
                                i.SubSamplesID = (int)ID;
                                dt.SubSamplesIDColumn.ReadOnly = true;
                            }
                        }
                        ta.Update(i.SubSampleName, i.SubSampleDescription, i.SubSampleCreationDate, sampsID, capsID, i.GeometryName, matId, i.Gross1, i.Gross2, i.Tare, i.MoistureContent, i.FillHeight, i.IrradiationRequestsID, monId, refId, stdId, blkId, i.Gthermal, i.Radius, i.DirectSolcoi, inre, outre, i.Concentration, i.ENAA, chCapsID, i.SubSamplesID);
                        i.AcceptChanges();

                        UnitRow[] us = i.GetUnitRows();
                        uta.Update(us);
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                        EC.SetRowError(i, ex);
                    }
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            ta.Dispose();
            ta = null;
            uta.Dispose();
            uta = null;
        }
    }
}