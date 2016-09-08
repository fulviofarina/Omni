using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB;
//using DB.Interfaces;
using DB.Tools;
using Rsx;
using DB.UI;

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


        protected string pathCode = string.Empty;
        protected bool isClone=false;

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

        protected Interface Interface = null;

        protected IPeriodicTable pTable = null;


        public ucOrder ucOrder;

        protected ISubSamples iSS=null;

        public ISubSamples ISS
        {
            get { return iSS; }
            set { iSS = value; }
        }



        public LINAA.ProjectsRow ProjectsRow=null;
        protected IEnumerable<LINAA.SubSamplesRow> samples = null;

      

        protected IWC iW;
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

                    if (!EC.IsNuDelDetch(iSS.Irradiation))
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

                    if (!EC.IsNuDelDetch(iSS.Irradiation))
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
                if (TV.Tag != null) TV.BackColor = (Color)TV.Tag;

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
                Color color = this.TV.BackColor;
                if (TV.Tag == null) color = Color.Lavender;
                else if (!this.iSS.Offline) color = Color.LemonChiffon;
                else color = Color.Honeydew;
                //  TV.Enabled = false;
                TV.Tag = color;
                this.TV.BackColor = Color.MistyRose;
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
            bool haserrors = EC.HasErrors<LINAA.SubSamplesRow>(this.samples);
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
                        int added = Interface.IPopulate.ISamples.AddSamples(project, ref samsToAddSpec);
                        samsToAddSpec = null;
                    }
                }
            }

            this.samples = Interface.IPopulate.IProjects.FindByProject(project);

            Interface.IStore.Save(ref this.samples);
        }
    }
}