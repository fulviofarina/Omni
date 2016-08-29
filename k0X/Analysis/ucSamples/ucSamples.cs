using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.Tools;
using Rsx;
using VTools;
using DB.Interfaces;

namespace k0X
{
    public partial class ucSamples : UserControl
    {
        public ucSamples(ref Interface set)
        {
            this.InitializeComponent();

            this.progress.Value = 0;
            this.progress.Step = 1;
            this.progress.Maximum = 0;
            this.Orderbox.Visible = false;
            this.Cancel.Visible = false;
            this.Clon = new TreeNode[this.TV.Nodes.Count];

            //links!

            LINAA linaa = set.Get() as LINAA;
            Interface = set;

            this.pTable = Program.UserControls.OfType<ucPeriodicTable>().FirstOrDefault();

            this.TV.SuspendLayout();
            this.TV.Nodes.CopyTo(Clon, 0);
            this.TV.ShowPlusMinus = false;
            this.TV.ResumeLayout();
        }

        protected bool Waiting = false;
        protected IWC iW;
        protected string pathCode;
        protected bool isClone;
        protected System.Windows.Forms.Timer timerQM;

        protected System.Messaging.MessageQueue MQ;
        public ucOrder ucOrder;
        protected DB.UI.ISubSamples iSS;

        public DB.UI.ISubSamples ISS
        {
            get { return iSS; }
            set { iSS = value; }
        }

        public LINAA.ProjectsRow ProjectsRow;
        protected IEnumerable<LINAA.SubSamplesRow> samples = null;

        protected  Interface Interface= null;
        protected IPeriodicTable pTable;

        public bool IsClone
        {
            get { return isClone; }
            set
            {
                isClone = value;

                pathCode = this.Name;
                if (isClone) pathCode += ".Clone";
            }
        }

        public IWC W
        {
            get
            {
                try
                {
                    if (iW == null)
                    {
                        LINAA Linaa = Interface.Get() as LINAA;
                        iW = new WC(this.Name, ref this.progress, ref this.Cancel, ref Linaa);
                        iW.SetExternalMethods(this.CheckNode, this.Finished);
                        iW.SetNodes(ref TV); //link nodes
                    }
                    iW.SetPeakSearch(minAreabox.Text, maxUncbox.Text, Awindowbox.Text, Bwindowbox.Text);
                    iW.SetOverriders(fbox.Text, alphabox.Text, Gtbox.Text, Geobox.Text, asSamplebox.Checked);
                    iW.ShowSolang = showSolang.Checked;
                    iW.ShowSSF = showMATSSF.Checked;
                }
                catch (SystemException ex)
                {
                    Interface.IReport.AddException(ex);
                }
                return iW;
            }
        }

        protected void Finished(object sender)
        {
            if (sender.Equals(this.Infere)) //gotoImport
            {
                Infere.PerformClick();
            }
            else
            {
                if (!sender.Equals(this.checktsmi))
                {
                    if (!sender.Equals(this.Fetch) && !sender.Equals(this.Populate))
                    {
                        Interface.IReport.Msg("VOILÀ! with " + Name, "Done with calculations for this project!");
                    }
                    Application.OpenForms.OfType<MainForm>().First().BugReportMenu.PerformClick(); //send bug report allways..
                    Interface.IReport.Speak("Finished with " + Name);

                    Populating = true;
                    W.SelectItems(true);
                    Populating = false;

                    if (sender.Equals(this.Import)) this.SampleConcentrationsOrFCs_Click(sender, EventArgs.Empty);
                }
                else Interface.IReport.Speak("Inference was completed!");

                ButtonVisible(true);
            }
        }

        protected void ContinueWork(int obj, ref object tag, string label)
        {
            Interface.IReport.Msg("Resuming...", "Resuming " + this.Name);
            this.progress.Value = 0;
            this.progress.Maximum = 0;

            string calculus = string.Empty;

            if (label.Contains("Run")) CRun(ref tag, ref calculus);
            else if (label.Contains("Delete"))
            {
                W.SelectedSamples = LINAA.FindSelected(this.samples).ToList();
                W.LoadPeaks(true, false);
                calculus = "Sched";
            }
            else if (label.Contains("Popul"))
            {
                BuildTV();
                if (!this.iSS.Offline)
                {
                    if (!isClone)
                    {
                        W.RefreshDB(!unofficialDb.Checked);
                        W.PopulateIsotopes();
                        calculus = "Fetch";
                    }
                    else
                    {
                        calculus = "Sched";
                        Analysis.Checked = false;
                    }

                    obj = Index(this.checktsmi); //Viewlarge called the method... therefore the user made modifications...
                }
                else calculus = "Sched";
            }
            else if (label.Contains("Fetch"))
            {
                bool transfer = true;

                if (this.iSS.Offline)
                {
                    if (tag.Equals(this.Fetch))
                    {
                        MessageBox.Show("You are currently working Offline (from a Workspace XML file).\nRemember to save your Workspace afterwards\notherwise the changes will not be propagated to your XML file", "Working Offline");
                    }
                    else transfer = false;
                }

                IEnumerable<LINAA.SubSamplesRow> samps = LINAA.FindSelected(this.samples).ToList();
                if (samps.Count() == 0) samps = this.samples.ToList();
                W.SelectedSamples = samps.ToList();
                W.Fetch(transfer);  //FETCH when Online only!!
                calculus = "Sched";
            }
            else if (label.Contains("Work")) CWork(ref tag, ref calculus);
            else if (label.Contains("Solcoin"))
            {
                W.CalculateSolang(doSolang.Checked, AlsoCOIS.Checked, IntegrateAs.Text);
                calculus = "Efficiency";
            }
            else if (label.Contains("Efficiency") || label.Contains("COI"))
            {
                bool coin = !label.Contains("Efficiency");

                if (!coin) calculus = "COI";
                else
                {
                    if (tag != this.calculateSolang) calculus = "NAA"; //w = Emailer.CreateQMsg(tag, "NAA", "Content");
                    else calculus = "SD";//w = Emailer.CreateQMsg(tag, "SD", "Content");
                }
                bool doSol = doSolang.Checked || readSolangToolStripMenuItem.Checked;
                if (doSol) W.EffiLoad(coin, doSolang.Checked);  //get recalculated new solid angles
            }
            else if (label.Contains("NAA") || label.Contains("SD"))
            {
                bool UncOnly = label.Contains("SD");
                if (this.autoRecalc.Checked)  //this computes allways when Recalculate was called or the user Did a Just calculation with this enable!!!
                {
                    W.CalculatePeaks(true, UncOnly);     //Calculation
                    W.CalculatePeaks(false, UncOnly);     //Calculation
                }
                if (!UncOnly)
                {
                    tag = this.Recalculate;
                    obj = Index(this.Recalculate);
                }
                if (UncOnly) calculus = "Sched";
                else calculus = "SD";
            }
            else if (label.Contains("Sched"))
            {
                W.SelectedSamples = this.samples;
                string msg = "Status of the project will be checked\nbut this will not interrupt your work";
                string title = "Checkpoint for " + this.Name + "is scheduled ;)";
                if (tag.Equals(this.Infere))
                {
                    msg = "Checking what to do...";
                    title = "Auto-Inference for " + this.Name + " is scheduled ;)";
                }
                Interface.IReport.Msg(msg, title);

                W.Check(tag);
            }

            if (!string.IsNullOrEmpty(calculus)) SendQMsg(obj, calculus);
        }

        protected void CWork(ref object tag, ref string calculus)
        {
            LINAA Linaa = (LINAA)Interface.Get();
            Linaa.ToDoResAvg.Clear();
            Linaa.ToDoRes.Clear();

            W.SelectedSamples = LINAA.FindSelected(this.samples).ToList();
            int count = W.SelectedSamples.Count();

            bool IsInfere = tag.Equals(this.Infere);

            if (count == 0)
            {
                //NOTHING SELECTED !
                string msg = string.Empty;
                string title = string.Empty;
                bool ok = false;
                if (IsInfere) //auto-inference called the method
                {
                    msg = "Samples with insufficient data provided where not considered";
                    title = "Nothing needs to (or can) be calculated so far for " + this.Name;
                    ok = true;
                }
                else //auto-inference did not call the method (import, efficiency, matssf, recalc)
                {
                    msg = "0 samples selected. Please use the checkboxes";
                    title = "Select at least 1 sample or measurement to store for " + this.Name;
                }
                Interface.IReport.Msg(msg, title, ok);

                ButtonVisible(true);
                return;
            }

            bool IsImport = tag.Equals(this.Import);
            bool IsSSF = tag.Equals(this.CalculateSSF);

            bool IsFPeaks = tag.Equals(this.fetchPeaks);
            bool IsreCalc = tag.Equals(this.Recalculate);
            bool IsSol = tag.Equals(this.calculateSolang);
            bool canPass = (IsImport || IsInfere || IsFPeaks);
            bool forceTools = (!IsInfere && !IsFPeaks);

            Interface.IReport.Msg("All right! Calculations will start shortly", "Background calculations for " + this.Name);

            if (forceTools)
            {
                //do not delete peaks when importing, it will delete them anyway but al least Selection/Rejection is not lost!!!!
                if (IsSol || IsImport) doSolang.Checked = true;
                if (IsSSF || IsImport) this.doMATSSF.Checked = true;
                if (IsreCalc || IsSSF) autoRecalc.Checked = true;
            }
            else
            {
                doSolang.Checked = false;
                doMATSSF.Checked = false;
                autoRecalc.Checked = true;
                readSolangToolStripMenuItem.Checked = true;
            }

            Preferences(false);

            this.progress.Value = 0;
            this.progress.Maximum = 0;

            if (canPass)
            {
                bool transfer = false;
                transfer = (IsImport || doSolang.Checked || IsFPeaks);
                W.LoadPeaks(false, transfer);  //import or recalculate
            }

            //keep going, when matssf or import!!!
            if (IsImport || IsSSF || IsInfere)
            {
                W.CalculateMatSSF(doMATSSF.Checked, fluxTypebox.Text[0]);
                if (IsSSF) calculus = "NAA";
            }

            if (!IsSSF)
            {
                //when not matssf --> solang, import or recalc
                if (canPass || IsSol) calculus = "Solcoin";
                else if (!IsFPeaks) calculus = "Efficiency";
                else calculus = "SD";
            }
        }

        protected void CRun(ref object tag, ref string calculus)
        {
            IEnumerable<ucSamples> controls = null;
            controls = Program.UserControls.OfType<ucSamples>().Where(o => !o.IsDisposed);
            controls = controls.Where(o => o.Waiting);	  //get controls waiting
            controls = controls.Where(o => !o.Equals(this));	 //controls except this one
            int working = controls.Count();

            bool isChk = tag.Equals(this.checktsmi);
            bool isPopul = tag.Equals(this.Populate);
            bool isPoC = (isPopul || isChk);
            bool isFetch = tag.Equals(this.Fetch);

            int MaxNumber = 2;
            if (isPoC) MaxNumber = 1;
            if (working < MaxNumber)
            {
                Waiting = true;

                //  iSS.Irradiation = this.Linaa.IrradiationRequests.FindByIrradiationCode(this.Name);

                Preferences(true); //important, load peak filter settings first!!!

                if (isFetch) calculus = "Fetch";	//fetch meas
                else if (isPoC)
                {
                    // bool wasPopulatedOnce = (this.samples != null); //this is because I want to know if ViewLarg called the method or populate..
                    FindSamples();
                    W.SelectedSamples = this.samples.ToList();
                    if (isChk) calculus = "Sched";
                    else calculus = "Popul"; //it was populate who called...
                }
                else calculus = "Work";	//do work
            }
            else calculus = "Run"; //restart the waiting period..
        }

        protected void Preferences(bool load)
        {
            try
            {
                LINAA.PreferencesRow preferences = Interface.IPreferences.CurrentPref;

                if (load)
                {
                    minAreabox.Text = preferences.minArea.ToString();
                    maxUncbox.Text = preferences.maxUnc.ToString();
                    showMATSSF.Checked = preferences.ShowMatSSF;

                    showSolang.Checked = preferences.ShowSolang;
                    Awindowbox.Text = preferences.windowA.ToString();
                    Bwindowbox.Text = preferences.windowB.ToString();
                    sampleDescription.Checked = preferences.ShowSampleDescription;

                    if (!Dumb.IsNuDelDetch(iSS.Irradiation))
                    {
                        fluxTypebox.Text = iSS.Irradiation.ChannelsRow.FluxType;
                    }
                }
                else
                {
                    preferences.minArea = Convert.ToDouble(minAreabox.Text);
                    preferences.maxUnc = Convert.ToDouble(maxUncbox.Text);
                    preferences.windowA = Convert.ToDouble(Awindowbox.Text);
                    preferences.windowB = Convert.ToDouble(Bwindowbox.Text);
                    preferences.ShowMatSSF = showMATSSF.Checked;
                    preferences.ShowSolang = showSolang.Checked;
                    preferences.ShowSampleDescription = sampleDescription.Checked;
                    preferences.LastAccessDate = DateTime.Now;

                    if (!Dumb.IsNuDelDetch(iSS.Irradiation))
                    {
                        iSS.Irradiation.ChannelsRow.FluxType = fluxTypebox.Text;
                    }
                    Interface.IStore.SavePreferences();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error loading preferences\n\n" + ex.Message + "\n\n" + ex.StackTrace);
                Interface.IReport.AddException(ex);
            }
        }

        /// <summary>
        /// Hides or shows the Buttons when calculating
        /// </summary>
        ///

        protected void ButtonVisible(bool visible)
        {
            if (visible)
            {
                //LINK....
                if (this.ParentForm != null) this.ParentForm.Text = this.Name;
                this.iSS.Link();
                bool useranalysing = (pTable != null);
                if (useranalysing) pTable.Link();

                HasErrors();

                FillOverriders();

                //if (!TV.Enabled)
                //	{
                //   TV.Enabled = true;
                if (TV.Tag != null) TV.BackColor = (System.Drawing.Color)TV.Tag;

                //}

                Waiting = false;
                progress.Minimum = 0;
                progress.Maximum = 1;
                progress.Value = 1;
                timerQM.Enabled = false;
            }
            else
            {
                //Delink...
                if (Cancel.Checked) Cancel.Checked = false;
                if (this.ParentForm != null) this.ParentForm.Text = this.Name + " - Please wait...";
                bool useranalysing = (pTable != null);
                if (useranalysing) pTable.DeLink();
                this.iSS.DeLink();

                this.error.SetError(progress.Control, null);
                //  if (TV.Enabled)
                //   {
                System.Drawing.Color color = this.TV.BackColor;
                if (TV.Tag == null) color = System.Drawing.Color.Lavender;
                else if (!this.iSS.Offline) color = System.Drawing.Color.LemonChiffon;
                else color = System.Drawing.Color.Honeydew;
                //  TV.Enabled = false;
                TV.Tag = color;
                this.TV.BackColor = System.Drawing.Color.MistyRose;
                //  }
            }

            doSolang.Checked = false;
            doMATSSF.Checked = false;

            this.calculateSolang.Visible = visible;
            this.Import.Visible = visible;
            this.Recalculate.Visible = visible;
            this.Delete.Visible = visible;
            this.CalculateSSF.Visible = visible;
            this.Infere.Visible = visible;
            this.ProjectFCs.Visible = visible;
            this.ProjectConcentrations.Visible = visible;

            this.Cancel.Visible = !visible;

            Application.DoEvents();
        }

        /// <summary>
        /// If user cancelled the calculations
        /// </summary>
        protected bool Canceled()
        {
            if (Cancel.Checked)
            {
                W.CancelWorkers();

                Interface.IReport.Msg("Processes cancelled by user", "Cancelled! " + this.Name);

                ButtonVisible(true);

                return true;
            }
            else return false;
        }

        protected void CheckNode(ref LINAA.SubSamplesRow s)
        {
            //so it does not run on populating...
            Populating = true;

            try
            {
                TreeNode old = MakeSampleNode(ref s);
                LINAA.MeasurementsRow[] measurements = s.GetMeasurementsRows();
                //old.Nodes.Clear();
                foreach (LINAA.MeasurementsRow m in measurements)
                {
                    try
                    {
                        LINAA.MeasurementsRow aux = m;
                        TreeNode MeasNode = MakeMeasurementNode(ref aux);
                        SetAMeasurementNode(ref  MeasNode);
                        if (!old.Nodes.Contains(MeasNode))
                        {
                            MeasNode.Collapse();
                            old.Nodes.Add(MeasNode);	  //add childrens already
                        }
                    }
                    catch (SystemException ex)
                    {
                        Interface.IReport.AddException(ex);
                    }
                }
                SetASampleNode(this.sampleDescription.Checked, ref old);
            }
            catch (SystemException ex)
            {
                Interface.IReport.AddException(ex);
            }

            Populating = false;
        }

        protected void timerQM_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = sender as System.Windows.Forms.Timer;
            timer.Enabled = false;
            //calculating...
            if (Canceled()) return;

            System.Messaging.Message[] arr = MQ.GetAllMessages();
            if (arr.Length == 0)
            {
                timer.Enabled = true;
                return;
            }
            if (progress.Value != progress.Maximum)
            {
                timer.Enabled = true;
                return;
            }

            System.Messaging.Message w = MQ.Receive();

            string label = w.Label;
            byte[] content = w.Extension;
            int obj = (int)w.Body;

            object tag = this.ProjectMenu.DropDownItems[obj];

            ContinueWork(obj, ref tag, label);

            timer.Enabled = true;
        }

        protected void SendQMsg(int obj, string calculus)
        {
            System.Messaging.Message w = null;
            w = Rsx.Emailer.CreateQMsg(obj, calculus, "Content");
            Rsx.Emailer.SendQMsg(ref MQ, ref w);
        }

        private void Import_Click(object sender, EventArgs e)
        {
            //When Importing --> MatSSF, Load Peaks (with re-transfer), Solang and recalculate (NAA)
            //When MatSSF ===> only MatSFF of coourse
            //When CalculateSolang =>  Load Peaks (without re-transfer unless not found), Solang and recalculate (NAA)...
            //When Recalculate --> Load Peaks (without re-transfer unless not found), recalculate (NAA)

            string toDo = "Run";

            if (sender.Equals(this.Delete))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete all calculated data available for the samples or measurements selected?\n\n" +
                "This will NOT affect any sample data and its available measurements. Recalculation can be done once more at any time.\nHowever current self-shielding results, " +
                "calculated concentrations / FCs and gamma-lines selection/rejection information will be lost.\n\nContinue?", "Delete Analysis...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;

                toDo = "Delete";
            }

            if (MQ == null)
            {
                MQ = Rsx.Emailer.CreateMQ(DB.Properties.Resources.QMWorks + "." + pathCode, null);
            }
            if (MQ == null)
            {
                Interface.IReport.Msg("Check if MSMQ wether is installed", "Cannot initiate the Message Queue", false);
                return;
            }
            else MQ.Purge();

            if (timerQM == null)
            {
                timerQM = new Timer(this.components);
                timerQM.Interval = 200;
                timerQM.Tick += timerQM_Tick;
            }
            timerQM.Tag = null;
            timerQM.Enabled = true;

            ButtonVisible(false);

            int obj = Index(sender);
            SendQMsg(obj, toDo);
        }

        protected int Index(object sender)
        {
            return this.ProjectMenu.DropDownItems.IndexOf((ToolStripItem)sender);
        }

        /// <summary>
        /// Add this control to a new form
        /// </summary>
        public void NewForm()
        {
            AuxiliarForm form2 = new AuxiliarForm();
            form2.Tag = this;
            form2.Text = this.Name;
            form2.ShowIcon = false;
            UserControl control = this;
            form2.Populate(ref control);
            form2.Show();
            form2.AutoSize = true;
            form2.MinimizeBox = false;
            form2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            if (ProjectsRow != null)
            {
                int x = 0;
                int y = 0;
                if (!ProjectsRow.IsXNull()) x = ProjectsRow.X;
                if (!ProjectsRow.IsYNull()) y = ProjectsRow.Y;
                form2.Location = new System.Drawing.Point(x, y);
            }
        }

        protected bool IsEmpty()
        {
            bool empty = false;
            if (samples == null) empty = true;
            else if (this.samples.Count() == 0) empty = true;

            if (empty)
            {
                this.progress.Value = this.progress.Maximum;
                this.error.SetError(this.progress.Control, "The Irradiation Project " + this.Name + " has no samples attached\n\nClick at the right to add samples to it");
                if (this.ParentForm != null) this.ParentForm.Text = this.Name + " - EMPTY";
            }
            return empty;
        }

        protected void HasErrors()
        {
            bool haserrors = Dumb.HasErrors<LINAA.SubSamplesRow>(this.samples);
            if (haserrors)
            {
                this.error.SetError(this.progress.Control, "Not all the necessary information was provided for Irradiation Project " + this.Name + " (or some of the sample's data requires your attention)\n\nClick at the right to check this");
            }
            else this.error.SetError(this.progress.Control, null);
        }

        protected void FillOverriders()
        {
            LINAA Linaa = (LINAA)Interface.Get();
            IList<string> ls = Dumb.HashFrom<string>(Linaa.SubSamples.GthermalColumn);
            Dumb.FillABox(this.Gtbox.ComboBox, ls, true, false);
            ls.Clear();
            ls = null;
            ls = Dumb.HashFrom<string>(Linaa.Geometry.GeometryNameColumn);
            Dumb.FillABox(this.Geobox.ComboBox, ls, true, false);
            ls.Clear();
            ls = null;
            ls = Dumb.HashFrom<string>(Linaa.Channels.AlphaColumn);
            Dumb.FillABox(this.alphabox.ComboBox, ls, true, false);
            ls.Clear();
            ls = null;
            ls = Dumb.HashFrom<string>(Linaa.Channels.fColumn);
            Dumb.FillABox(this.fbox.ComboBox, ls, true, false);
            ls.Clear();
            ls = null;
            ls = Dumb.HashFrom<string>(Linaa.SubSamples.ConcentrationColumn);
            Dumb.FillABox(this.FCbox.ComboBox, ls, true, false);
            ls.Clear();
            ls = null;
        }

        protected void FindSamples()
        {
            Application.DoEvents();
            if (!this.iSS.Project.Equals(this.Name)) this.iSS.Project = this.Name;    //gather samples from these project (this.Name)

            this.samples = null;

            string project = this.Name;

            if (!iSS.Offline)
            {
                bool fillHL = Interface.IPreferences.CurrentPref.FillByHL;
                bool fillSpec = Interface.IPreferences.CurrentPref.FillBySpectra;

                if ((fillHL || fillSpec))
                {
                    ICollection<string> samsToAddSpec = null;
                    if (fillSpec)
                    {
                        string specPath = Interface.IPreferences.CurrentPref.Spectra;
                        samsToAddSpec = DB.Tools.WC.FindSpecSamples(project, specPath);
                    }

                    if (DB.Tools.WC.IsHyperLabOK && fillHL)
                    {
                        ICollection<string> samsToAddHL = DB.Tools.WC.FindHLSamples(project);
                        if (samsToAddSpec != null)
                        {
                            samsToAddSpec = samsToAddHL.Union(samsToAddSpec).ToList();
                            samsToAddHL = null;
                        }
                        else samsToAddSpec = samsToAddHL;
                    }
                    if (samsToAddSpec != null)
                    {
                        samsToAddSpec = new HashSet<string>(samsToAddSpec);
                        int added = Interface.IPopulate.ISamples.AddSamples(project, ref	samsToAddSpec);
                        samsToAddSpec = null;
                    }
                }
            }

            this.samples = Interface.IPopulate.IProjects.FindByProject(project);

            Interface.IStore.Save(ref this.samples);
        }

        private void SampleConcentrationsOrFCs_Click(object sender, EventArgs e)
        {
            string Title = "Fc's Report - " + this.Name;

            Interface.IReport.LoadACrystalReport(Title, LINAA.ReporTypes.FcReport);
        }

        private void AnyNodeCMS_Click(object sender, EventArgs e)
        {
            if (this.TV.SelectedNode == null) return;
            object tag = this.TV.SelectedNode.Tag;
            if (tag == null) return;
            DataRow row = tag as DataRow;
            if (Rsx.Dumb.IsNuDelDetch(row)) return;

            Logger log = null;
            object o = null;
            string toprint = string.Empty;
            string title = string.Empty;
            Type tipo = tag.GetType();

            if (tipo.Equals(typeof(LINAA.MeasurementsRow)))
            {
                LINAA.MeasurementsRow m = (LINAA.MeasurementsRow)tag;
                if (sender.Equals(Peaks))
                {
                    o = m.GetPeaksRows();
                    toprint = "Peaks";
                    title = m.Measurement;
                }
            }
            else if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
            {
                LINAA.SubSamplesRow l = (LINAA.SubSamplesRow)tag;
                title = l.SubSampleName;
                if (sender.Equals(this.MeasurementsHyperLab))
                {
                    o = l.GetMeasurementsRows();
                    toprint = "Measurements";
                }
                else if (sender.Equals(this.ViewMatSSF))
                {
                    o = l.GetMatSSFRows();
                    toprint = "MatSSF Results";
                }
            }

            if (o == null) return;
            IEnumerable<DataRow> rows = o as IEnumerable<DataRow>;
            log = new Logger(rows.CopyToDataTable(), title + " - " + toprint);
            log.Show();
        }

        private void watchDog_Click(object sender, EventArgs e)
        {
            LINAA Linaa = (LINAA)Interface.Get();

            IWatchDog wD = null;
            try
            {
                if (sender.Equals(this.watchDogToolStripMenuItem))
                {
                    wD = new ucWatchDog();

                    Program.UserControls.Add(wD);

                    wD.Link(ref Linaa, this.Name);
                    if (!Linaa.IsSpectraPathOk)
                    {
                        Interface.IReport.Msg("Make sure the right Spectra Directory Path is given in the <DB Connections> (right-click on the Notifier)!", "Could not connect to Spectra Directory", false);
                    }
                    else wD.Watch();
                }
                else
                {
                    Form f = new Form();
                    f.Text = " Measurements xTable for " + this.Name;

                    DataView view = Linaa.Measurements.AsDataView();
                    view.RowFilter = "Project LIKE '" + this.Name + "*'";
                    DataGridView dgv = new DataGridView();

                    dgv.Dock = DockStyle.Fill;
                    //  f.AutoSize = true;
                    //    f.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                    f.Controls.Add(dgv);

                    string detcol = Linaa.Measurements.DetectorColumn.ColumnName;
                    string[] filt = new string[] { Linaa.Measurements.SampleColumn.ColumnName };
                    string poscol = Linaa.Measurements.PositionColumn.ColumnName;
                    ucWatchDog.ShowXTable(ref view, ref dgv, detcol, filt, poscol);

                    f.Show();
                    f.Size = new System.Drawing.Size(dgv.Width + 10, dgv.Height + 10);
                }
            }
            catch (SystemException ex)
            {
                Interface.IReport.AddException(ex);
            }
        }

        protected void CleanNodes()
        {
            if (samples == null) return;
            foreach (LINAA.SubSamplesRow s in samples)
            {
                TreeNode n = (TreeNode)s.Tag;
                if (n != null)
                {
                    try
                    {
                        if (n.TreeView != null)
                        {
                            IntPtr a = n.TreeView.Handle;
                        }
                        else s.Tag = null;
                    }
                    catch (ObjectDisposedException ex)
                    {
                        s.Tag = null;
                    }
                }
            }
        }

        private void OptionsMenu_DropDownOpened(object sender, EventArgs e)
        {
            Preferences(true);
        }

        private void OptionsMenu_DropDownClosed(object sender, EventArgs e)
        {
            Timer t = new Timer(this.components);
            t.Interval = 2000;
            t.Tick += t_Tick;
            t.Enabled = true;
        }

        protected void t_Tick(object sender, EventArgs e)
        {
            Timer t = sender as Timer;
            t.Enabled = false;
            //timer for preferences
            Preferences(false);
            t.Dispose();
        }

        private void unofficialDb_CheckedChanged(object sender, EventArgs e)
        {
            W.RefreshDB(!unofficialDb.Checked);
        }

        public void ViewLarge_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.iSS.Daddy = this;
            //visible or not? save if necessary

            this.iSS.ChangeView();

            if (ParentForm != null)
            {
                this.ParentForm.Visible = !this.ParentForm.Visible;

                if (this.ParentForm.Visible)
                {
                    this.BuildTV();
                    this.TV.CollapseAll();

                    this.progress.Value = 0;
                    this.progress.Maximum = samples.Count();
                    foreach (LINAA.SubSamplesRow sample in samples)
                    {
                        LINAA.SubSamplesRow s = sample;
                        CheckNode(ref s);
                        this.progress.PerformStep();
                    }
                    this.TV.TopNode.Expand();
                    //DialogResult res = MessageBox.Show("Would you like to refresh the project?", "Important", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    //if (res == DialogResult.Yes) this.Populate.PerformClick();
                    //else if (res == DialogResult.Cancel) ViewLarge_Click(sender, e);
                }
            }

            Cursor.Current = Cursors.Default;
        }
    }
}