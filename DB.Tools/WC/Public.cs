using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

//using DB.Interfaces;
using Rsx;
using Rsx.Math;

namespace DB.Tools
{
    /// <summary>
    /// PUBLIC
    /// </summary>
    public partial class WC : DB.Tools.IWC
    {
        public void RefreshDB(bool official)
        {
            progress.Maximum++;

            System.ComponentModel.BackgroundWorker r = new System.ComponentModel.BackgroundWorker();
            r.WorkerSupportsCancellation = true;
            r.WorkerReportsProgress = true;

            r.DoWork += new System.ComponentModel.DoWorkEventHandler(r_DoWork);
            r.ProgressChanged += ProgressChanged;
            AddWorker(ref r);
            r.RunWorkerAsync(official);
        }

        public void Predict()
        {
            // this.selectedSamples
            foreach (LINAA.SubSamplesRow s in selectedSamples)
            {
                //LINAA.UnitRow u = s.GetUnitRows().AsEnumerable().FirstOrDefault();
                // if (EC.IsNuDelDetch(u)) continue;
                LINAA.MatrixRow m = s.MatrixRow;
                if (EC.IsNuDelDetch(m)) continue;
                LINAA.CompositionsRow[] coms = m.GetCompositionsRows();
                HashSet<string> elementsIfAny = new HashSet<string>(coms.Select(o => o.Element.Trim().ToUpper()).ToArray());
                //if (s.IsElementsNull()) continue;
                // HashSet<string> elementsIfAny = new HashSet<string>(System.Text.RegularExpressions.Regex.Split(s.Elements, ","));
                //if (s.IsElementsNull()) continue;
                //HashSet<string> elementsIfAny = new HashSet<string>(System.Text.RegularExpressions.Regex.Split(u.Content.uni, ","));

                double FC = 1;

                if (s.IrradiationCode.Contains("X") || s.IrradiationCode.Contains("Z")) FC = 2900;
                else if (s.IrradiationCode.Contains("Y")) FC = 200;

                foreach (string ele in elementsIfAny)
                {
                    IEnumerable<LINAA.NAARow> naas = this.Linaa.NAA
                        .Where(o => o.Sym.Trim().ToUpper().CompareTo(ele) == 0);
                    HashSet<string> isos = new HashSet<string>();
                    foreach (LINAA.NAARow n in naas)
                    {
                        if (!isos.Add(n.Iso.Trim())) continue;

                        LINAA.IRequestsAveragesRow ir = this.Linaa.IRequestsAverages.NewIRequestsAveragesRow();
                        this.Linaa.IRequestsAverages.AddIRequestsAveragesRow(ir);
                        ir.Sample = s.SubSampleName;
                        ir.NAAID = n.ID;

                        //ir.Element = n.Sym.Trim();
                        //ir.Radioisotope = n.Iso.Trim();

                        if (n.MD != 1) ir.SetColumnError(this.Linaa.IRequestsAverages.RadioisotopeColumn, "Decay scheme not straightforward\nResult might not be true");

                        if (FC == 1) ir.SetColumnError(this.Linaa.IRequestsAverages.AspColumn, "Please multiply this value by the FC factor of this channel. I couldn't find it");

                        ir.AcceptChanges();
                        if (ir.NAARow.ReactionsRowParent == null) continue;
                        LINAA.SigmasRow sigma = ir.NAARow.ReactionsRowParent.SigmasRowParent;

                        if (sigma == null) continue;

                        // IEnumerable<LINAA.MeasurementsRow> meas = s.GetMeasurementsRows();

                        //whathever values
                        if (s.IsIrradiationTotalTimeNull()) s.IrradiationTotalTime = 200;

                        LINAA.MeasurementsRow mea = Interface.IDB.Measurements.AddMeasurementsRow(s.IrradiationCode, 0, "A", s.SubSampleName + "A", DateTime.Now, 0, s, "D", 0, 5, s.IrradiationRequestsID, s.SubSamplesID, 0, false, true, s.GeometryName, "DUNNO", TimeSpan.MinValue);

                        IEnumerable<LINAA.MeasurementsRow> measurements = new LINAA.MeasurementsRow[] { mea };

                        Rate(s.Alpha, s.f, ref ir);
                        // FindDecayTimes(ref measurements); Temporal(ref measurements, ref ir);
                        ir.Asp = (s.Concentration * s.DryNet * 0.001 * 1e-6) * (MyMath.NAvg * sigma.sigma0 * 1e-24 * sigma.theta * 0.01 / ir.NAARow.ReactionsRowParent.SigmasSalRow.Mat) * (FC * 1e6 / 0.2882) * ir.R0 * MyMath.S((0.693 / n.T2), s.IrradiationTotalTime); //result in Bq
                        ir.Asp = ir.Asp * 0.001; //result in kBq
                    }
                }
            }
        }

        public void PopulateIsotopes()
        {
            if (cancel.Checked) return;
            int count = this.selectedSamples.Count();
            if (count == 0) return;

            System.ComponentModel.BackgroundWorker popul = new System.ComponentModel.BackgroundWorker();
            popul.DoWork += new System.ComponentModel.DoWorkEventHandler(popul_DoWork);
            popul.WorkerSupportsCancellation = true;

            popul.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(ProgressChanged);
            popul.WorkerReportsProgress = true;
            AddWorker(ref popul);

            IList<string> ls = Dumb.HashFrom<string>(selectedSamples.ToList(), samplesCol);

            count = ls.Count;
            progress.Maximum += count + 1;
            Interface.IReport.Msg("Peak identification running...", "Populating isotopes for " + Name);

            popul.RunWorkerAsync(ls);
        }

        /// <summary>
        /// Initilizes the workload engine for the given ProjectName, with a progressBar and CancelBtn
        /// </summary>
        /// <param name="Project">    The Project Name to search for</param>
        /// <param name="progressBar"></param>
        /// <param name="CancelBtton"></param>
        /// <param name="set">        </param>
        public WC(string Project, ref ToolStripProgressBar progressBar, ref ToolStripMenuItem CancelBtton, ref LINAA set)
        {
            cancel = CancelBtton;
            Name = Project;
            progress = progressBar;

            Interface = new Interface(ref set);
            Linaa = set;
            samplesCol = set.SubSamples.SubSampleNameColumn.ColumnName;
            composCol = set.Compositions.ElementColumn.ColumnName;
        }

        /*
       public void SelectItems()
       {
           IEnumerable<LINAA.SubSamplesRow> sams = this.selectedSamples;
           sams = sams.Where(s => !s.HasErrors).ToList();
           foreach (LINAA.SubSamplesRow s in sams)
           {
               s.Selected = true;
               if (s.Tag!=null)
               {
                   TreeNode snode = s.Tag as TreeNode;
                   snode.Checked = s.Selected;
               }
           }
           IEnumerable<LINAA.MeasurementsRow> meas = sams.SelectMany(s => s.GetMeasurementsRows());
           meas = meas.Where(m => !m.HasErrors).ToList();
           foreach (LINAA.MeasurementsRow m in meas)
           {
               m.Selected = true;
               if (m.Tag != null)
               {
                   TreeNode mnode = m.Tag as TreeNode;
                   mnode.Checked = m.Selected;
               }
           }
       }
         */

        /// <summary>
        /// Calculates Rates and Decay, SDs
        /// </summary>
        public void CalculatePeaks(bool monitorsOnly, bool uncsOnly)
        {
            if (cancel.Checked) return;
            int count = this.selectedSamples.Count();
            if (count == 0) return;

            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples.ToList();

            double Fc = 0;
            double FcCD = 0;

            string title = string.Empty;
            string msg = string.Empty;
            title = "Calculating SDs from " + samples.Count() + " samples";
            try
            {
                if (!monitorsOnly && uncsOnly)
                {
                    //take average Fc from monitors
                    Fc = Linaa.IRequestsAverages.AvgOfFCs(Name);
                    FcCD = Linaa.IRequestsAverages.AvgOfFCs(Name + DB.Properties.Misc.Cd);

                    if (Fc != 1) msg = "FC = " + Decimal.Round(Convert.ToDecimal(Fc), 0);
                    else if (FcCD != 1) msg = "FC (Cadmium) = " + Decimal.Round(Convert.ToDecimal(FcCD), 0);
                    else msg = "Something is wrong with the averaged FC (equal to one)";
                }
                else
                {
                    msg = "Starting...";
                    title = "Calculating NAA data from " + samples.Count() + " samples";
                }
            }
            catch (SystemException ex)
            {
                Linaa.AddException(ex);
                msg = "Something is wrong with the averaged FC (not a number)";
            }

            Interface.IReport.Msg(msg, title);

            progress.Maximum += count;

            foreach (LINAA.SubSamplesRow s in samples)
            {
                //cancell before computing if needed
                if (cancel.Checked) return;

                if (monitorsOnly && s.MonitorsRow == null) continue;
                else if (!monitorsOnly && s.MonitorsRow != null) continue;

                if (uncsOnly)
                {
                    if (s.StandardsRow == null && s.MonitorsRow == null && !s.ENAA) s.Concentration = Fc;
                    else if (s.StandardsRow == null && s.MonitorsRow == null) s.Concentration = FcCD;
                }
                if (!uncsOnly) s.RowError = string.Empty;
                bool GoodOverride = true;
                if (!uncsOnly) GoodOverride = s.Override(alpha, f, geo, gt, asSamples);
                if (GoodOverride)
                {
                    IEnumerable<LINAA.IRequestsAveragesRow> isos = s.GetIRequestsAveragesRows();
                    if (isos.Count() != 0)
                    {
                        int percent = 15;
                        if (uncsOnly)
                        {
                            FindSDs(ref isos);
                            percent = 45;
                        }
                        else Calculate(ref isos, true, false);
                        LINAA.SubSamplesRow aux = s;
                        ProgressReportSample(ref aux, percent);
                    }
                    else s.RowError = "Calculate Rates Module Error: IRequestsAveragesRows were not found!";
                }
                else s.RowError = "Not calculated because one of the overriders is not in the correct input format\n\nPlease correct this in the 'Options Menu' and then click 'Recalculate'";
            }

            progress.Value += count;
        }

        /// <summary>
        /// Checks samples
        /// </summary>
        public void Check(object sender)
        {
            if (cancel.Checked) return;
            int count = this.selectedSamples.Count();
            if (count == 0) return;

            progress.Maximum += count;
            Interface.IReport.Msg("Finding out what needs to be calculated....", "Auto-Inference activated for " + Name);

            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples.ToList();

            System.ComponentModel.BackgroundWorker checker = new System.ComponentModel.BackgroundWorker();
            checker.DoWork += new System.ComponentModel.DoWorkEventHandler(checker_DoWork);
            checker.WorkerSupportsCancellation = true;
            checker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(checker_RunWorkerCompleted);
            checker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(ProgressChanged);
            checker.WorkerReportsProgress = true;
            AddWorker(ref checker);
            checker.RunWorkerAsync(new object[] { samples, sender });
        }

        /// <summary>
        /// Load the Peaks
        /// </summary>
        /// <param name="samples">   samples whose peaks should be loaded when peaks not found</param>
        /// <param name="deleteOnly">true when peaks should be deleted instead of loaded</param>
        /// <param name="transfer">  true for forcing peaks to be transfered</param>
        public void LoadPeaks(bool deleteOnly, bool transfer)
        {
            if (cancel.Checked) return;
            int count = selectedSamples.Count();
            if (count == 0) return;

            string text = string.Empty;

            bool HLisOK = IsHyperLabOK;
            if (!HLisOK)
            {
                if (transfer || deleteOnly)
                {
                    if (!deleteOnly) text = "Error when connecting to HyperLab";
                    else text = "I will not delete because connection to HyperLab cannot be restored";
                    Interface.IReport.Msg("Verify the connection of HyperLab's database", text, false);
                    Application.DoEvents();
                    return;
                }
            }

            if (deleteOnly) text = "Deleting from " + count + " samples";
            else if (transfer) text = "Importing gamma peaks from " + count + " samples";
            else text = "Checking gamma peaks from " + count + " samples";
            Interface.IReport.Msg("Please wait...", text);

            progress.Maximum += count;
            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples.ToList();
            foreach (LINAA.SubSamplesRow sample in samples)
            {
                if (cancel.Checked) return;
                System.ComponentModel.BackgroundWorker peakImporter = new System.ComponentModel.BackgroundWorker();
                peakImporter.WorkerSupportsCancellation = true;
                peakImporter.WorkerReportsProgress = true;

                peakImporter.DoWork += peakImport_DoWork;
                peakImporter.ProgressChanged += ProgressChanged;
                AddWorker(ref peakImporter);
                peakImporter.RunWorkerAsync(new object[] { sample, deleteOnly, transfer, HLisOK });
            }
        }

        /// <summary>
        /// Fecthes the measurements from the spectrommetry database (HyperLab, etc)
        /// </summary>
        /// <param name="transfer">    true for forcing the measurements to be transfered</param>
        /// <param name="refreshNodes">true for forcing refresh of Nodes!</param>
        public void Fetch(bool transfer)
        {
            if (cancel.Checked) return;
            int count = selectedSamples.Count();
            if (count == 0) return;

            bool HLIsOK = IsHyperLabOK;

            string text = "Fetching";
            text += " measurements for ";
            Interface.IReport.Msg(text + count + " samples", text + Name);
            Application.DoEvents();

            progress.Maximum += count;
            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples.ToList();

            foreach (LINAA.SubSamplesRow sample in samples)
            {
                if (cancel.Checked) return;
                count = sample.GetMeasurementsRows().Count();
                if (count == 0 && HLIsOK) transfer = true;
                System.ComponentModel.BackgroundWorker measImporter = new System.ComponentModel.BackgroundWorker();
                measImporter.DoWork += measImport_DoWork;

                measImporter.WorkerReportsProgress = true;
                measImporter.ProgressChanged += ProgressChanged;
                measImporter.WorkerSupportsCancellation = true;
                AddWorker(ref measImporter);
                measImporter.RunWorkerAsync(new object[] { sample, transfer, HLIsOK });
            }
        }

        /// <summary>
        /// Calculates Solcoin units according to whats needed..
        /// </summary>
        public void CalculateSolang(bool DoSolang, bool AlsoCOIS, string IntegrateAs)
        {
            if (cancel.Checked) return;
            int count = selectedSamples.Count();
            if (count == 0) return;
            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples.ToList();
            System.Collections.Hashtable[] array = PrepareSolang(DoSolang, ref samples);

            System.Collections.Hashtable solcoinTable = array[1];
            System.Collections.Hashtable solcoinRefTable = array[0];

            if (solcoinTable.Count == 0 || solcoinRefTable.Count == 0)
            {
                Interface.IReport.Msg("Calculations NOT scheduled", "Efficiency Transfer for " + Name);
                return;
            }
            Interface.IReport.Msg("Calculations being scheduled...", "Efficiency Transfer for " + Name);

            if (IntegrateAs.CompareTo("As Non Disk") == 0) mode = SolCoin.IntegrationModes.AsNonDisk;
            else if (IntegrateAs.CompareTo("As Disk") == 0) mode = SolCoin.IntegrationModes.AsDisk;
            else mode = SolCoin.IntegrationModes.AsPointSource;

            if (!cancel.Checked && AlsoCOIS)
            {
                Application.DoEvents();
                CalculateCOIS(ref solcoinTable, ref solcoinRefTable);
            }
            if (!cancel.Checked)
            {
                Application.DoEvents();
                //normal part
                CalculateSolidAngles(ref solcoinTable, ref solcoinRefTable);
            }

            solcoinRefTable.Clear();
            solcoinRefTable = null;
            solcoinTable.Clear();
            solcoinTable = null;
        }

        /// <summary>
        /// Calculates SSF factors
        /// </summary>
        public void CalculateMatSSF(bool doSSF, char fluxType)
        {
            if (cancel.Checked) return;
            int count = selectedSamples.Count();
            if (count == 0) return;

            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples;
            if (!doSSF) samples = samples.Where(s => s.NeedsSSF).ToList();
            else samples = samples.ToList();

            count = samples.Count();
            if (count == 0) return;

            progress.Maximum += count;
            Interface.IReport.Msg("SSF calculations started for " + count + " samples", "Self-shielding calculations for " + Name);

            System.ComponentModel.BackgroundWorker matssf = new System.ComponentModel.BackgroundWorker();
            matssf.WorkerSupportsCancellation = true;
            matssf.WorkerReportsProgress = true;

            matssf.DoWork += new System.ComponentModel.DoWorkEventHandler(matssf_DoWork);
            matssf.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(ProgressChanged);
            AddWorker(ref matssf);
            matssf.RunWorkerAsync(new object[] { samples, showSSF, fluxType });
        }

        /// <summary>
        /// Cancel workers
        /// </summary>
        public void CancelWorkers()
        {
            if (workers == null || workers.Count == 0) return;

            if (workersCancelled == null) workersCancelled = new List<System.ComponentModel.BackgroundWorker>();

            for (int i = workers.Count - 1; i >= 0; i--)
            {
                System.ComponentModel.BackgroundWorker x = workers.ElementAt(i);
                x.CancelAsync();
                workers.Remove(x);
                workersCancelled.Add(x);
            }
        }

        /// <summary>
        /// Loads COIS or Efficiencies
        /// </summary>
        /// <param name="coin">   true to retrieve cois instead of efficiencies</param>
        /// <param name="samples">samples to retrieve cois or solangs from</param>
        /// <param name="all">    retrieve only for those with effi=1 or have errors</param>
        public void EffiLoad(bool coin, bool all)
        {
            if (cancel.Checked) return;
            int count = this.selectedSamples.Count();
            if (count == 0) return;

            LINAA.GeometryRow reference = Interface.IPopulate.IGeometry.FindReferenceGeometry("REF");
            if (reference == null)
            {
                string msg = "The Reference Geometry was not found! Please make sure a Geometry named 'REF' exist in the database";
                Interface.IReport.Msg(msg, "Reference Geometry (REF) not found!!!", false);
                Application.DoEvents();
                return;
            }

            progress.Maximum += count;  //important to keep here

            string text = string.Empty;
            if (coin) text = "Loading Coi factors for ";
            else text = "Loading Solid angles for ";
            Interface.IReport.Msg("Veryfing efficiencies and COI factors", text + Name);
            Application.DoEvents();
            IEnumerable<LINAA.SubSamplesRow> samples = selectedSamples.ToList();

            System.ComponentModel.BackgroundWorker effiloader = new System.ComponentModel.BackgroundWorker();
            effiloader.WorkerSupportsCancellation = true;
            effiloader.WorkerReportsProgress = true;

            effiloader.DoWork += effiloader_DoWork;
            effiloader.ProgressChanged += ProgressChanged;
            AddWorker(ref effiloader);
            effiloader.RunWorkerAsync(new object[] { samples, coin, all, reference });
        }

        public void SetNodes(ref TreeView TV)
        {
            tv = TV;
        }

        public void SelectItems(bool all)
        {
            IEnumerable<TreeNode> samplenodes = null;
            samplenodes = tv.Nodes.OfType<TreeNode>().SelectMany(o => o.Nodes.OfType<TreeNode>()).ToList();

            if (!all) samplenodes = samplenodes.Where(o => o.IsSelected).ToList();
            IEnumerable<TreeNode> measnodes = null;
            LINAA.SubSamplesRow srow = null;
            LINAA.MeasurementsRow mrow = null;

            foreach (TreeNode s in samplenodes)
            {
                srow = null;
                srow = s.Tag as LINAA.SubSamplesRow;
                if (srow.HasErrors) s.Checked = false;
                else if (all) s.Checked = true;
                srow.Selected = s.Checked;
                measnodes = null;
                measnodes = s.Nodes.OfType<TreeNode>().ToList();
                if (!all) measnodes = measnodes.Where(o => o.IsSelected).ToList();
                foreach (TreeNode m in measnodes)
                {
                    mrow = null;
                    mrow = m.Tag as LINAA.MeasurementsRow;
                    if (mrow.HasErrors) m.Checked = false;
                    else if (all) m.Checked = true;
                    mrow.Selected = m.Checked;
                }
            }
        }
    }
}