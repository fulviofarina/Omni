using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using Rsx.Dumb; using Rsx;

namespace DB.Tools
{
    public partial class WC
    {
        /// <summary>
        /// Adds a worker
        /// </summary>
        private void AddWorker(ref System.ComponentModel.BackgroundWorker w)
        {
            if (workers == null) workers = new List<System.ComponentModel.BackgroundWorker>();
            this.workers.Add(w);
        }

        /// <summary>
        /// worker for Check
        /// </summary>
        private void checker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];

            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            IEnumerable<LINAA.SubSamplesRow> samples = args[0] as IEnumerable<LINAA.SubSamplesRow>;
            //object caller = (object)args[1];

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (Changes.HasChanges(s.GetPeaksRows()))
                {
                    object[] samDelSave = new object[] { s, false, true };
                    worker.ReportProgress((int)R.PeaksDelSave, samDelSave);  //(s,del,save)
                }
                if (Changes.HasChanges(s))
                {
                    worker.ReportProgress((int)R.SampleStatus, s); //saves changes TIP
                }
            }

            object[] args2 = new object[] { samples, true }; //true for also determining whichs needs ssf
            worker.ReportProgress((int)R.SampleInfere, args2); //refresh nodes!!!
            foreach (LINAA.SubSamplesRow s in samples)
            {
                worker.ReportProgress((int)R.SampleCheck, s); //refresh nodes!!!
                worker.ReportProgress((int)R.Progress, new object[] { null, 1 });
            }

            e.Result = e.Argument;
        }

        /// <summary>
        /// Executes the finishMethod
        /// </summary>
        private void checker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            object[] res = e.Result as object[];

            object caller = (object)res[1];

            finishMethod.Invoke(caller);
        }

        /// <summary>
        /// worker for EffiLoad
        /// </summary>
        private void effiloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            DateTime start = DateTime.Now;

            object[] args = e.Argument as object[];

            bool coin = (bool)args[1];
            IEnumerable<LINAA.SubSamplesRow> samples = (IEnumerable<LINAA.SubSamplesRow>)args[0];
            bool all = (bool)args[2];
            LINAA.GeometryRow reference = (LINAA.GeometryRow)args[3];

            IEnumerable<LINAA.MeasurementsRow> measforContingency = null;
            measforContingency = SetCOINSolid(ref samples, coin, all, ref reference);

            //Contingency plan, in case the first algorithms didn't work...
            if (measforContingency != null && measforContingency.Count() != 0)
            {
                object o = null;

                if (coin) o = Linaa.COIN;
                else o = Linaa.Solang;
                PopulateCOINSolang(coin, ref measforContingency, ref o);
                ContingencySetCOINSolid(coin, ref reference, ref measforContingency);
            }

            DateTime end = DateTime.Now;
            string loadedin = Decimal.Round(Convert.ToDecimal((end - start).TotalSeconds), 1).ToString();

            string text = string.Empty;
            if (coin) text = "Coi Factors";
            else text = "Solid Angles";
            worker.ReportProgress((int)R.SolcoiLoaded, new object[] { loadedin, text });
            worker.ReportProgress((int)R.Progress, new object[] { null, samples.Count() }); //tip
        }

        /// <summary>
        /// worker for Calculate MatSSF
        /// </summary>
        private void matssf_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            object[] args = e.Argument as object[];
            IEnumerable<LINAA.SubSamplesRow> iSDT = args[0] as IEnumerable<LINAA.SubSamplesRow>;
            char fluxtypo = (char)args[2];
            bool showMatssf = (bool)args[1];

            SystemException ex = null;

            LINAA.UnitDataTable unitDT = new LINAA.UnitDataTable();

            foreach (LINAA.SubSamplesRow iS in iSDT)
            {
                if (e.Cancel) return;

                ex = null;

                try
                {
                    LINAA.UnitRow u = null;
                    if (iS.UnitRow == null)
                    {
                        u = unitDT.NewUnitRow();

                        unitDT.AddUnitRow(u);
                    }
                    u.ChRadius = (iS.VialTypeRow.InnerRadius);
                    u.ChLength = iS.VialTypeRow.MaxFillHeight;
                    u.ChCfg = fluxtypo.ToString();
                    u.Name = iS.SubSampleName;

                    // u.Content = iS.MatrixRow.MatrixComposition; u.Mass = iS.Net; u.Diameter =
                    // (iS.Radius * 2); u.Length = iS.FillHeight;

                    MatSSF.StartupPath = Linaa.FolderPath + Resources.SSFFolder;
                   
              //      MatSSF.UNIT = u;

                  //  MatSSF.INPUT(true);

                 //   if (DB.Tools.MatSSF.RUN(!showMatssf))  // MatSSF Runned good?
                    {
                        LINAA.MatSSFDataTable ssfDT = new LINAA.MatSSFDataTable();
                        ssfDT.Constraints.Clear();
                        LINAA.SubSamplesRow aux = iS;
                        // MatSSF.Sample = iS;
                   //     MatSSF.Table = ssfDT;

                    //    String FileOut = MatSSF.OUTPUT();
                     //   MatSSF.WriteXML();
                        //now read OutPut from calculation and store it into the MatSSF data table
                        worker.ReportProgress((int)R.SSFSet, iS);   //delete previous
                        worker.ReportProgress((int)R.MergeTable, ssfDT);   //delete previous
                    }

                    unitDT.Clear();
                    unitDT.Dispose();
                    unitDT = null;
                }
                catch (SystemException exc)
                {
                    ex = exc;
                    EC.SetRowError(iS, ex);
                }

                worker.ReportProgress((int)R.SSFSave, iS);    //notify about calcs

                worker.ReportProgress((int)R.Progress, new object[] { ex, 1 }); //perform step and that's it
            }
        }

        /// <summary>
        /// Import Measurements and loads the Peaks for those measurements
        /// </summary>
        /// <param name="sender">The worker!</param>
        /// <param name="e">     
        /// Array of Args = { Sample to load measurement/peaks, a bool to transfer, a bool to refresh nodes}
        /// </param>
        private void measImport_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            SystemException ex = null;

            try
            {
                if (e.Cancel) return;

                object[] args = e.Argument as object[];
                LINAA.SubSamplesRow sample = args[0] as LINAA.SubSamplesRow;
                bool HLIsOk = (bool)args[2];
                bool transfer = (bool)args[1];

                sample.RowError = string.Empty;

                //everybody inside thiss because I would like to use it offline too!!!  (from XML file)
                //otherwise everything gets reimported (online)
                //automatically...

                LINAATableAdapters.MeasurementsTableAdapter measTA = null;
                LINAA.MeasurementsDataTable remote = null;

                if (transfer)
                {
                    measTA = new LINAATableAdapters.MeasurementsTableAdapter();
                    remote = new LINAA.MeasurementsDataTable(false);
                    remote.Constraints.Clear();
                }

                int i = 0;   //for nothing transfered but gathered from hyperLab

                if (HLIsOk && transfer)
                {
                    remote.BeginLoadData();
                    try
                    {
                        measTA.SetForHL();
                        //new HL Table Adapater so I don't interfere in multi-background workers.... keep like this
                        measTA.FillByHLSample(remote, sample.SubSampleName);
                    }
                    catch (SystemException ec)
                    {
                        ex = ec;
                    }

                    //found measurements in hyperlab (remotely)
                    //send them to local database
                    measTA.SetForLIMS();

                    if (remote.Count != 0) measTA.DeleteBySample(sample.SubSampleName);
                    else i = -1;

                    HashSet<string> measlist = new HashSet<string>();
                    IEnumerable<LINAA.MeasurementsRow> nodupli = remote.Where(o => measlist.Add(o.Measurement)).ToList();

                    foreach (LINAA.MeasurementsRow m in nodupli)
                    {
                        try
                        {
                            i += measTA.Insert(m.Project, m.MeasurementNr, m.MeasurementID, m.Measurement, m.MeasurementStart, m.LiveTime, m.Sample, m.Detector, Convert.ToInt16(m.Position), m.CountTime);
                        }
                        catch (SystemException x)
                        {
                            ex = x;
                        }
                    }
                    measlist.Clear();
                    remote.EndLoadData();
                    remote.Clear();
                }

                //populate the transfered (or previously stored) measurements

                if (transfer)
                {
                    remote.BeginLoadData();
                    try
                    {
                        measTA.DeleteNegatives();
                        measTA.FillBySample(remote, sample.SubSampleName);
                    }
                    catch (SystemException ec) { ex = ec; }

                    remote.EndLoadData();
                    //now remote is local data
                    if (remote.Count != 0)
                    {
                        IEnumerable<DataRow> meas = sample.GetMeasurementsRows();
                        if (meas.Count() != 0)
                        {
                            worker.ReportProgress((int)R.RowsDelete, meas);    //delete  old
                            worker.ReportProgress((int)R.RowsAccept, meas);    //accept deletion of old
                        }
                        meas = null;
                        worker.ReportProgress((int)R.MergeTable, remote); //merge new measurements
                    }
                    else if (i == -1) sample.RowError = "Measurements were not found in HyperLab.\nThis means that it was not possible to connect to the server or,\nthe measurements are not deconvoluted.";
                    else if (i == 0) sample.RowError = "Measurements were found in HyperLab but\nit was not possible to insert them into the local database.\nPlease send a bug-report to my vendor.";
                    else sample.RowError = "Measurements (" + i + ") were transfered from HyperLab but\nthese were not found later when querying the local database.\nPlease check the k0-X database connections";
                }
                Dumb.FD(ref measTA);
                Dumb.FD(ref remote);
            }
            catch (SystemException x) { ex = x; }

            worker.ReportProgress((int)R.Progress, new object[] { ex, 1 }); //tip
        }

        private void peakImport_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            //If import called the method, it will re-transfer the peaks and keep old SELECTION/REJECTION!! ;)
            //therefore I should never call Delete function before importing, only when delete (deleteOnly) was called.
            // If CalculateSolang or Recalculate called the method, only transfer peaks when there was nothing loaded first.

            //this is perfect like this, really,
            //only thet you need to speed it up by not deleteing twice... locally and remotely. Locally takes more time...
            //how to do this without affecting other rows of the table?

            object[] args = e.Argument as object[];

            LINAA.SubSamplesRow sample = args[0] as LINAA.SubSamplesRow;
            bool deleteOnly = (bool)args[1];
            bool transfer = (bool)args[2];
            bool HLIsOk = (bool)args[3];
            SystemException x = null;

            try
            {
                if (e.Cancel) return;
                //now transfer peaks if commanded or just auto-detect...
                //returns true if there was a transfer somewhere  (overrides this transfer)

                IList<string> elementsIfAny = null;
                if (!deleteOnly)
                {
                    LINAA.MatrixRow mat = sample.MatrixRow;
                    if (EC.IsNuDelDetch(mat)) sample.RowError = "Sample has no matrix assigned";
                    else
                    {
                        //DO NOT USE FILTER ELEMENTS!!!!!

                        IEnumerable<DataRow> compos = mat.GetCompositionsRows();
                        elementsIfAny = Hash.HashFrom<string>(compos, composCol);
                        elementsIfAny = elementsIfAny.Select(o => o.Trim()).ToList();
                        compos = null;
                    }
                }

                IEnumerable<LINAA.MeasurementsRow> measurements = LINAA.FindSelected(sample.GetMeasurementsRows()).ToList();
                foreach (LINAA.MeasurementsRow m in measurements)
                {
                    m.ClearErrors(); //clear errors
                    if (deleteOnly)  //if delete
                    {
                        Interface.IStore.DeletePeaks(m.MeasurementID);
                        continue;  //skip the rest
                    }
                    int peaksCount = m.GetPeaksRows().Count();
                    if (peaksCount == 0) transfer = true;   //overrides

                    if (transfer) transfer = HLIsOk;

                    if (transfer)
                    {
                        LINAA.PeaksHLDataTable peakshl = null;
                        LINAA.MeasurementsRow aux = m;
                        peakshl = PopulatePeaksHL(ref aux, minArea, maxUnc);
                        if (peakshl != null)
                        {
                            if (!m.HasErrors) TransferPeaksHL(ref aux, sample.SubSamplesID, sample.IrradiationRequestsID, ref peakshl);
                            if (!m.HasErrors) FilterPeaks(ref aux, windowA, windowB, minArea, maxUnc, ref elementsIfAny);
                        }
                    }
                }

                if (e.Cancel) return;

                if (transfer || deleteOnly)
                {
                    worker.ReportProgress((int)R.PeaksDelSave, new object[] { sample, true, false });
                    //now repopulate because database should be fucking clean when deleted!!!
                    SystemException ex = null;
                    LINAA.PeaksDataTable peaks = null;
                    LINAA.IPeakAveragesDataTable iAvgpeaks = null;
                    LINAA.IRequestsAveragesDataTable iavgreqs = null;
                    string samName = sample.SubSampleName;
                    try
                    {
                        peaks = PopulatePeaks(null, samName);
                        iAvgpeaks = PopulateIPeaksAverages(samName);
                        iavgreqs = PopulateIRequestsAverages(samName);
                    }
                    catch (SystemException except)
                    {
                        ex = except;
                    }

                    worker.ReportProgress((int)R.MergeTable, peaks); //merge all peaks  (plus avgs)
                    worker.ReportProgress((int)R.MergeTable, iAvgpeaks); //merge all peaks  (plus avgs)
                    worker.ReportProgress((int)R.MergeTable, iavgreqs); //merge all peaks  (plus avgs)
                    if (ex != null) worker.ReportProgress((int)R.AddException, ex); //except  (plus avgs)
                    worker.ReportProgress((int)R.PeaksDelSave, new object[] { sample, false, false });
                }
                int peaksCnt = sample.GetPeaksRows().Count();
                if (peaksCnt != 0 && !deleteOnly)
                {
                    worker.ReportProgress((int)R.SamplePeaksStatus, sample); //tip peaks recovery
                }
                if (deleteOnly)
                {
                    worker.ReportProgress((int)R.SampleInfere, new object[] { sample, true }); //refresh nodes when deleting
                    worker.ReportProgress((int)R.SampleCheck, sample); //refresh nodes when deleting
                }
            }
            catch (SystemException ex) { x = ex; }

            worker.ReportProgress((int)R.Progress, new object[] { x, 1 }); //tip
        }

        private void popul_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            IList<string> ls = e.Argument as IList<string>;
            SystemException ex = null;
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            int? id = Interface.IPopulate.IIrradiations.FindIrradiationID(this.Name);
          

            foreach (string subsample in ls)
            {
                try
                {
                    LINAA.MatSSFDataTable ssf = new LINAA.MatSSFDataTable(false);
                   
                    ////REPONER ESTO!!!
                   // byte[] arr = Interface.IPopulate.ISamples. .SSFTable;
                    //Tables.ReadDTBytes(MatSSF.StartupPath, ref arr, ref ssf);

                    //      LINAA.MatSSFDataTable ssf = ;
                    worker.ReportProgress((int)R.MergeTable, ssf); //tip
                }
                catch (SystemException x)
                {
                    ex = x;
                }


                ex = null;
                try
                {
                    LINAA.PeaksDataTable peaks = PopulatePeaks(null, subsample);
                    worker.ReportProgress((int)R.MergeTable, peaks); //tip
                    LINAA.IPeakAveragesDataTable ipeaks = PopulateIPeaksAverages(subsample);
                    worker.ReportProgress((int)R.MergeTable, ipeaks); //tip
                    LINAA.IRequestsAveragesDataTable irss = PopulateIRequestsAverages(subsample);
                    worker.ReportProgress((int)R.MergeTable, irss); //tip
                }
                catch (SystemException x)
                {
                    ex = x;
                }
                worker.ReportProgress((int)R.Progress, new object[] { ex, 1 }); //tip
            }
            ex = null;

            worker.ReportProgress((int)R.Progress, new object[] { ex, 1 }); //tip
        }

        private void ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            int percent = e.ProgressPercentage;

            switch (percent)
            {
                case (int)R.SolcoiLoaded: //for Checkin Direct solcoi
                    {
                        object[] results = e.UserState as object[];
                        Interface.IReport.Msg(results[1] + " loaded in " + results[0] + " seconds", results[1] + " loaded for " + Name);
                        Application.DoEvents();
                    }
                    break;

                case (int)R.SolcoiEnded:
                    {
                        object[] args = e.UserState as object[];
                        SolCoin solcoin = (SolCoin)args[0];
                        string[] PosGeoDetFillRad = (string[])args[2];
                        double[] energies = (double[])args[3];
                        string unit = string.Empty;
                        string enes = unit + "\n";
                        foreach (string x in PosGeoDetFillRad) unit += x + " ";
                        foreach (double x in energies) enes += x + "\n";
                        string msg = " with the efficiency calculation of: " + unit;
                        bool ok = solcoin.EndIt();
                        solcoin = null;
                        Interface.IReport.Msg(enes, msg, ok);
                        Application.DoEvents();
                    }
                    break;

                case (int)R.SSFSet: //delete matssf
                    {
                        object res = e.UserState as object;
                        LINAA.SubSamplesRow s = res as LINAA.SubSamplesRow;
                        IEnumerable<LINAA.MatSSFRow> ssfs = s.UnitRow.GetMatSSFRows();
                        Interface.IStore.Delete(ref ssfs);
                        Changes.AcceptChanges(ref ssfs);
                    }
                    break;

                case (int)R.SSFSave:
                    {
                        LINAA.SubSamplesRow s = e.UserState as LINAA.SubSamplesRow;
                        string msg = string.Empty;
                        string title = string.Empty;

                        //get Gthermal and Density and check with values
                        bool ssfError = s.CheckGthermal();
                        title = " with Self-shielding";
                        if (!ssfError) msg = "MatSSF calculation was OK for ";
                        else msg = "Check the matrix density and dimensions of your sample";
                        msg += s.SubSampleName;
                        Interface.IReport.Msg(msg, title, !ssfError);

                        IEnumerable<DataRow> ssfs = s.UnitRow.GetMatSSFRows();  //get matssf rows
                        Interface.IStore.Save(ref ssfs);
                        IList<DataRow> ls = new List<DataRow>();
                        ls.Add(s);
                        ssfs = ls.AsEnumerable();  //now enumerable is the sample
                        Interface.IStore.Save(ref ssfs);
                    }
                    break;

                case (int)R.PeaksDelSave:   //delete and save
                    {
                        object[] args = e.UserState as object[];

                        LINAA.SubSamplesRow sample = args[0] as LINAA.SubSamplesRow;

                        IEnumerable<LINAA.IPeakAveragesRow> oldipeaks = sample.GetIPeakAveragesRows();
                        IEnumerable<LINAA.IRequestsAveragesRow> oldirs = sample.GetIRequestsAveragesRows();
                        IEnumerable<LINAA.PeaksRow> oldpeaks = sample.GetPeaksRows();

                        bool deleteFirst = (bool)args[1];
                        bool save = (bool)args[2];

                        IEnumerable<DataRow> aux = null;

                        if (deleteFirst)
                        {
                            aux = oldpeaks;
                            Interface.IStore.Delete(ref aux);
                            aux = oldipeaks;
                            Interface.IStore.Delete(ref aux);
                            aux = oldirs;
                            Interface.IStore.Delete(ref aux);
                        }

                        if (save)
                        {
                            aux = oldpeaks;
                            Interface.IStore.Save(ref aux);
                            aux = oldipeaks;
                            Interface.IStore.Save(ref aux);
                            aux = oldirs;
                            Interface.IStore.Save(ref aux);
                        }
                        else
                        {
                            aux = oldpeaks;
                            Changes.AcceptChanges(ref aux);
                            aux = oldipeaks;
                            Changes.AcceptChanges(ref aux);
                            aux = oldirs;
                            Changes.AcceptChanges(ref aux);
                        }
                    }
                    break;

                case (int)R.MergeTable:
                    {
                        DataTable table = e.UserState as DataTable;

                        Tables.MergeTable(ref table, ref Linaa);
                        Dumb.FD(ref table);
                    }
                    break;

                case (int)R.RowsDelete:
                    {
                        System.Collections.Generic.IEnumerable<DataRow> oldpeaks = e.UserState as System.Collections.Generic.IEnumerable<DataRow>;
                        Interface.IStore.Delete(ref oldpeaks);
                    }
                    break;

                case (int)R.RowsSave:
                    {
                        System.Collections.Generic.IEnumerable<DataRow> oldpeaks = e.UserState as System.Collections.Generic.IEnumerable<DataRow>;
                        Interface.IStore.Save(ref oldpeaks);
                    }
                    break;

                case (int)R.RowsAccept:
                    {
                        System.Collections.Generic.IEnumerable<DataRow> oldpeaks = e.UserState as System.Collections.Generic.IEnumerable<DataRow>;
                        Changes.AcceptChanges(ref oldpeaks);   //new method
                    }
                    break;

                case (int)R.AddException:    // peaks	merge
                    {
                        SystemException x = e.UserState as SystemException;
                        if (x != null) Interface.IStore.AddException(x);
                    }
                    break;

                case (int)R.SamplePeaksStatus:
                    {
                        LINAA.SubSamplesRow sample = e.UserState as LINAA.SubSamplesRow;
                        ProgressReportSample(ref sample, percent);
                    }
                    break;

                case (int)R.SampleMeasStatus:
                    {
                        LINAA.SubSamplesRow sample = e.UserState as LINAA.SubSamplesRow;
                        ProgressReportSample(ref sample, percent);
                    }
                    break;

                case (int)R.SampleInfere:
                    {
                        object[] args = e.UserState as object[];
                        object arg0 = args[0];
                        Type tipo = arg0.GetType();
                        if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
                        {
                            LINAA.SubSamplesRow s = arg0 as LINAA.SubSamplesRow;
                            s.Selected = s.ShouldSelectIt((bool)args[1]);
                        }
                        else
                        {
                            IEnumerable<LINAA.SubSamplesRow> samples = arg0 as IEnumerable<LINAA.SubSamplesRow>;
                            foreach (LINAA.SubSamplesRow s in samples) s.Selected = s.ShouldSelectIt((bool)args[1]);
                            samples = LINAA.FindSelected(samples).ToList();
                            int need = samples.Count();
                            string text = string.Empty;
                            if (need != 0) text = "Calculation(s) needed! for " + Name;
                            else text = "Nothing needs to (or can) be calculated for " + Name;
                            Interface.IReport.Msg(need + " sample(s) needs calculations. Click 'Infere'", text);
                            Application.DoEvents();
                        }
                    }
                    break;

                case (int)R.SampleCheck:
                    {
                        // if (this.control.IsDisposed) return;
                        LINAA.SubSamplesRow s = e.UserState as LINAA.SubSamplesRow;
                        this.checkNode.Invoke(ref s);
                    }
                    break;

                case (int)R.Progress:
                    {
                        object[] res = e.UserState as object[];

                        if (res[0] != null)
                        {
                            Interface.IStore.AddException(res[0] as SystemException);
                        }
                        int sum = (int)res[1];
                        progress.Value += sum;
                        Application.DoEvents();
                    }
                    break;

                case (int)R.SampleStatus:
                    {
                        LINAA.SubSamplesRow sample = e.UserState as LINAA.SubSamplesRow;

                        string text = string.Empty;
                        string msg = string.Empty;
                        bool ok = true;
                        if (!sample.RowError.Equals(string.Empty))
                        {
                            msg = sample.RowError;
                            ok = false;
                        }
                        else msg = "OK with " + sample.SubSampleType + " " + sample.SubSampleDescription;
                        text += " when saving changes for ";
                        Interface.IReport.Msg(msg, text, ok);
                        Application.DoEvents();
                    }
                    break;

                default:
                    break;
            }
        }

        private void ProgressReportSample(ref LINAA.SubSamplesRow sample, int percent)
        {
            bool ok = true;
            string text = string.Empty;
            string msg = string.Empty;

            if (!sample.RowError.Equals(string.Empty))
            {
                ok = false;
                text = "When retrieving Sample data for ";
                msg = sample.RowError;
            }
            else if (percent == 95)
            {
                text = "When retriving Spectra for ";
                msg = "OK with " + sample.SubSampleType + " " + sample.SubSampleDescription;
            }
            else if (percent == 5)
            {
                text = "When retriving Measurements for ";
                msg = "OK with " + sample.SubSampleType + " " + sample.SubSampleDescription;
            }
            else if (percent == 15)
            {
                text = "When calculating ";
                msg = "OK with " + sample.SubSampleType + " " + sample.SubSampleDescription;
            }
            else if (percent == 45)
            {
                text = "When verifying ";
                msg = "OK with " + sample.SubSampleType + " " + sample.SubSampleDescription;
            }
            text += sample.SubSampleName;
            Interface.IReport.Msg(msg, text, ok);
            Application.DoEvents();
        }

        private void r_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bool official = (bool)e.Argument;
            SystemException ex = null;

            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            try
            {
                LINAA.NAADataTable n = PopulateNAA(official);
                worker.ReportProgress((int)R.MergeTable, n); //tip

                LINAA.k0NAADataTable k = Populatek0NAA(official);
                worker.ReportProgress((int)R.MergeTable, k); //tip
            }
            catch (SystemException x)
            {
                ex = x;
            }
            worker.ReportProgress((int)R.Progress, new object[] { ex, 1 }); //tip
        }

        private void solcoinWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            object[] args = e.Argument as object[];

            if (e.Cancel) return;

            SolCoin Solcoin = (SolCoin)args[0];
            string[] PosGeoDetFillRad = (string[])args[2];
            bool calcSolid = (bool)args[1];
            bool calcCOIs = (bool)args[4];
            double[] energies = (double[])args[3];
            bool hide = (bool)args[5];
            bool success = false;

            success = Solcoin.PrepareSampleUnit(PosGeoDetFillRad, energies, calcSolid, calcCOIs);

            if (e.Cancel) return;
            if (success) Solcoin.DoAll(hide);

            worker.ReportProgress((int)R.SolcoiEnded, args);
            worker.ReportProgress((int)R.Progress, new object[] { null, 1 });
        }
    }
}