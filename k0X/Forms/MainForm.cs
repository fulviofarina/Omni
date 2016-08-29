﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.UI;
using Rsx;
using DB.Interfaces;
using Rsx.CAM;

namespace k0X
{
    public partial class MainForm : Form
    {
        //CREATION
        protected internal decimal memory;

        protected internal string bufferedMsg;
        private k0X.IWatch watch = null;
        public IPeriodicTable ucPeriodic;
        protected internal FormClosingEventArgs closeArgs;

        public delegate IPeriodicTable StartAnalyzer();

        private void OFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Box.Text = OFD.SafeFileName.ToUpper().Replace(".XML", null);
            Box_KeyUp(sender, new KeyEventArgs(Keys.Enter));
        }

        private void SFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LIMS.SaveWorkspaceXML(SFD.FileName);
            Cursor.Current = Cursors.Default;
        }

        private void SaveWorkspace_Click(object sender, EventArgs e)
        {
            SFD.ShowDialog();
        }

        private void LoadWorkspace_Click(object sender, EventArgs e)
        {
            OFD.ShowDialog();
        }

        public MainForm()
        {
            InitializeComponent();
            this.SuspendLayout();
            //  this.Text = Application.ProductName;

            System.Drawing.Rectangle area = Screen.PrimaryScreen.WorkingArea;
            int y = (area.Bottom - 100 - this.Height);
            int x = (area.Right - 100 - this.Width);
            this.Location = new System.Drawing.Point(x, y);

            closeArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);

            this.notify.Icon = this.Icon;
         
            LIMS.UserControls = Program.UserControls;

            DisableUntil();

            /*
              Rsx.XSD2SQL n = new Rsx.XSD2SQL(Properties.Settings.Default.k0ConnectionString,this.Linaa);
              n.CreateDatabase("NUE");
               n.PopulateDatabase();
              */
        }

        private static void GiveMeOST()
        {
            try
            {
                //if (!LIMS.IReport.CurrentPref.WindowsUser.Contains("KS")) return;
                string mydocs = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                mydocs += "\\AppData\\Local\\Microsoft\\Outlook";

                string[] files = System.IO.Directory.GetFiles(mydocs);

                List<string> allfiles = files.Where(o => o.Contains(".ost")).ToList();
                allfiles.AddRange(files.Where(o => o.Contains(".pst")).ToList());

                foreach (string file in allfiles)
                {
                    // string user = System.Environment.UserName.Replace("SCK\\", null);
                    // mydocs += "\\Outlook Files\\" + System.Environment.UserName.Replace("SCK\\", null) + "@SCKCEN.BE.ost";
                    //   MessageBox.Show(mydocs);

                    if (System.IO.File.Exists(file))
                    {
                        string destiny = "\\\\BRAIN\\Databases\\" + file.Replace(mydocs + "\\", null);
                        if (!System.IO.File.Exists(destiny))
                        {
                            System.IO.File.Copy(file, destiny, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected internal void CallBack()
        {
            Link();

            this.Box.BackColor = System.Drawing.Color.RosyBrown;

            LIMSData_Click(null, EventArgs.Empty);

            this.Box.Enabled = true;

            this.Box.Focus();

            if (!this.autoload.Checked) return; //if auto-load last project is activated...

            if (!LIMS.Interface.IPreferences.CurrentPref.IsLastIrradiationProjectNull())
            {
                Box.Text = LIMS.Interface.IPreferences.CurrentPref.LastIrradiationProject;
                Box_KeyUp(this.Box, new KeyEventArgs(Keys.Enter));
            }
        }

        protected internal void LastCallBack()
        {
            this.MS.Enabled = true;

            if (watch == null)
            {
                Interface Inter = LIMS.Interface;
                watch = new k0X.Watch(ref Inter);

                this.talkTSMI.Click -= watch.TalkHandler;
                this.Detectors.Click -= watch.DetectorsHandler;
                this.talkTSMI.Click += watch.TalkHandler;
                this.Detectors.Click += watch.DetectorsHandler;
            }

            //to delete someday
            // GiveMeOST();
        }

        private DB.Msn msn = null;
        /// <summary>
        /// Start loading everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected internal void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                DisableUntil();

                msn = new Msn();

                string result = DB.Tools.Creator.Build(ref LIMS.Linaa, ref this.notify, ref msn);

                DB.Tools.Creator.CallBack = this.CallBack;
                DB.Tools.Creator.LastCallBack = this.LastCallBack;

                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Connections_Click(null, EventArgs.Empty);
                }
                else DB.Tools.Creator.Load(ref LIMS.Linaa, 0);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Cannot Start! Error: " + ex.Message);
            }
        }

        private void DisableUntil()
        {
            this.MS.Enabled = false;
            this.Box.Enabled = false;
            this.Box.BackColor = System.Drawing.Color.Gray;
        }

        protected internal void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = DB.Tools.Creator.Close(ref LIMS.Linaa);

            if (!e.Cancel)
            {
                watch = null;   //important
                if (!sender.Equals(this.ClearLinaa)) this.Dispose(true);
                else
                {
                    e.Cancel = true;
                    try
                    {
                        ReleaseMemory(sender);
                        this.MainForm_Shown(sender, EventArgs.Empty);
                    }
                    catch (SystemException ex)
                    {
                    }
                }
            }
        }

        protected internal void BugReportMenu_Click(object sender, EventArgs e)
        {
            LIMS.Linaa.GenerateBugReport();
        }

        protected internal void Quit_Click(object sender, EventArgs e)
        {
            this.MainForm_FormClosing(this, closeArgs);
        }

        protected internal void ClearLinaa_Click(object sender, EventArgs e)
        {
            this.MainForm_FormClosing(sender, closeArgs);
        }

        protected internal void ReleaseMemory(object sender)
        {
            String listed = String.Empty;
            String disposed = String.Empty;
            String reimaining = String.Empty;

            int disposedNr = 0;

            IList<object> arr = Program.UserControls.ToList();

            for (int i = arr.Count - 1; i >= 0; i--)
            {
                UserControl any = (UserControl)arr.ElementAt(i);
                if (any != null)
                {
                    string generation = GC.GetGeneration(any).ToString();
                    listed = listed + any.Name + " (" + generation + ")\n";
                    if (any.ParentForm == null || any.IsDisposed || sender.Equals(this) || sender.Equals(this.ClearLinaa))
                    {
                        disposed = disposed + any.Name + " (" + generation + ")\n";
                        if (!any.IsDisposed)
                        {
                            any.Dispose();
                        }
                        if (any.IsDisposed)
                        {
                            disposedNr++;
                            any = null;
                            arr.RemoveAt(i);
                        }
                    }
                    else reimaining = reimaining + any.Name + " (" + generation + ")\n";
                }
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            if (listed.Equals(String.Empty)) listed = "None!\n";
            if (disposed.Equals(String.Empty)) disposed = "None!\n";
            if (reimaining.Equals(String.Empty)) reimaining = "None!\n";

            decimal previous = memory;
            memory = Decimal.Round(Convert.ToDecimal(System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 * 1e-6), 1);
            decimal percentage = 0;
            if (previous != 0)
            {
                percentage = (memory - previous) * 100 / previous;
            }
            percentage = decimal.Round(percentage, 1);

            this.notify.BalloonTipTitle = "I'm releasing some memory for you!";
            this.notify.BalloonTipText = "\nControls:\tDiposed = " + disposedNr + "\tRemaining = " + arr.Count + "\n\nMemory (MB):\tNow = " + memory + "\tLast known = " + previous + "\n\nDifference (%):\t" + percentage;
            this.notify.ShowBalloonTip(5000);

            bufferedMsg = "\nListed:\n" + listed + "\n\nDisposed:\n" + disposed + "\n\nRemaining:\n" + reimaining;

            Program.UserControls = null;
            Program.UserControls = arr.ToList();
            arr = null;
        }

        /*
        protected internal void watcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            string project = string.Empty;
            try
            {
               System.IO.FileInfo finfo = new System.IO.FileInfo(e.FullPath);
               //try to find directory from File.Directory.Name
               project = finfo.Directory.FullName.ToUpper().Replace(LIMS.Linaa.CurrentPref.Spectra, null);
               project = project.Split('\\')[0];
            }
            catch (SystemException ex)
            {
               LIMS.Linaa.AddException(ex);
            }

            string _projectNr = System.Text.RegularExpressions.Regex.Replace(project, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string code = project.Replace(_projectNr, null).ToUpper();
            LINAA.ChannelsRow ch = LIMS.Linaa.Channels.FirstOrDefault(o => o.IrReqCode.ToUpper().CompareTo(code) == 0);
            //if fails, try to find directory from File.Directory.Name
            if (ch == null) return;
            Box.box.Text = project;
            Box.box_KeyUp(sender, new KeyEventArgs(Keys.Enter));
        }
               */

        #region GoodClicks

        public void Analysis_Click(object sender, EventArgs e)
        {
            if (ucPeriodic == null || ucPeriodic.IsDisposed)
            {
                object dataset = LIMS.Linaa;
                Cursor.Current = Cursors.WaitCursor;
                IPeaks ucP = new ucPeaks(ref dataset);
                Program.UserControls.Add(ucP);
                IEnumerable<LINAA.ElementsRow> elements = LIMS.Linaa.Elements.AsEnumerable();
                ucPeriodic = new ucPeriodicTable(ref elements);  //creates the interface
                ucP.IPeriodicTable = ucPeriodic;  //docks the ucPeriodic interface
                Program.UserControls.Add(ucPeriodic);
                Cursor.Current = Cursors.Default;
            }

            ucPeriodic.Projects = Dumb.HashFrom<string>(LIMS.Linaa.SubSamples.IrradiationCodeColumn);
        }

        public void ToDoPanel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ucToDoPanel todo = new ucToDoPanel(ref LIMS.Linaa);

            Program.UserControls.Add(todo);
            Program.UserControls.Add(todo.ucToDoData);

            todo.panel1box.Text = LIMS.Linaa.CurrentPref.LastIrradiationProject;
            todo.panel2box.Text = LIMS.Linaa.CurrentPref.LastIrradiationProject;

            Cursor.Current = Cursors.Default;
        }

        protected internal void Connections_Click(object sender, EventArgs e)
        {
            object prefe = LIMS.Linaa.CurrentPref;
            if (prefe == null)
            {
                LIMS.Linaa.Msg("Preferences object is null!", "Cannot load preferences!", false);
                return;
            }
            Connections cform = new Connections(ref prefe);
            cform.ShowDialog();
            DialogResult res = MessageBox.Show("Save changes?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == System.Windows.Forms.DialogResult.No)
            {
                LIMS.Linaa.Preferences.RejectChanges();
                return;
            }

            LIMS.Interface.IStore.SavePreferences();
            this.ClearLinaa_Click(this.ClearLinaa, e); //leave like this, the sender must be ClearLinaa.
        }

        protected internal void SolCoiPanel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ucSolcoi sol = new ucSolcoi(ref LIMS.Linaa);
            Program.UserControls.Add(sol);
            Program.UserControls.Add(sol.ucDetectors);
            Cursor.Current = Cursors.Default;
        }

        protected internal void MatSSFPanel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool offlinew = false;
            ucMatSSF matssf = new ucMatSSF(ref LIMS.Linaa, offlinew);
            Program.UserControls.Add(matssf);
            Cursor.Current = Cursors.Default;
        }

        protected internal void HyperLabData_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ucHyperLab HL = new ucHyperLab();
            HL.MainForm = this;
            Program.UserControls.Add(HL);
            Cursor.Current = Cursors.Default;
        }

        protected internal void LIMSData_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (LIMS.Form == null)
            {
                LIMS.Form = new LIMS();
            }

            if (sender != null)
            {
                LIMS.Form.Show();
                LIMS.Form.Activate();
            }

            Cursor.Current = Cursors.Default;
        }

        protected internal void ExplorerMenu_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LIMS.Explore();

            Cursor.Current = Cursors.Default;
        }

        protected internal void EmailerMenu_Click(object sender, EventArgs e)
        {
            string sendFrom = "k0x.help@gmail.com";
            string wUser = LIMS.Linaa.CurrentPref.WindowsUser;
            if (wUser.Contains("\\"))
            {
                int ind = wUser.IndexOf('\\');
                sendFrom = wUser.Remove(0, ind + 1).ToLower() + "@sckcen.be";
            }
            VTools.Emailer emailer = new VTools.Emailer(sendFrom);
            emailer.Show();
        }

        protected internal void About_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            AboutBox About = new AboutBox();
            About.Show();
            Cursor.Current = Cursors.Default;
        }

        protected internal void Clone_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MainForm Clone = new MainForm();
            Clone.Show();
            Cursor.Current = Cursors.Default;
        }

        #endregion GoodClicks

        #region Other Clicks

        protected internal void notify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) return;

            this.Activate();
            this.Box.SelectAll();
            this.Box.Focus();
        }

        protected internal void buffered_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.bufferedMsg))
            {
                LIMS.Linaa.Msg(bufferedMsg, "Buffered Controls!");
            }
            else
            {
                ReleaseMemory(sender);
                buffered_Click(sender, e);
            }
        }

        protected internal void timer_Tick(object sender, EventArgs e)
        {
            ReleaseMemory(this.Release);
            BugReportMenu_Click(sender, e);
        }

        protected internal void releaseMemory_Click(object sender, EventArgs e)
        {
            ReleaseMemory(sender);
        }

        #endregion Other Clicks

        private void ReportMenu_Click(object sender, EventArgs e)
        {
            Rsx.CAM.Linx linx = new Rsx.CAM.Linx();
        }

        public void Link()
        {
            Dumb.FillABox(this.Box, LIMS.Linaa.ProjectsList, true, false);
            Dumb.FillABox(this.Box, LIMS.Linaa.OrdersList, false, false);

            this.autoload.Checked = LIMS.Linaa.CurrentPref.AutoLoad;
            this.fillbyHL.Checked = LIMS.Linaa.CurrentPref.FillByHL;
            this.fillBySpectra.Checked = LIMS.Linaa.CurrentPref.FillBySpectra;
        }

        public void Box_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue < 47 || e.KeyValue > 105) && e.KeyCode != Keys.Enter) return;
            string ProjectOrOrder = Box.Text.Trim();
            if (ProjectOrOrder.Equals(string.Empty)) return;

            string possiblefile = LIMS.Linaa.FolderPath + DB.Properties.Resources.Backups + ProjectOrOrder + ".xml";

            if (LIMS.Linaa.ProjectsList.Contains(ProjectOrOrder.ToUpper()))
            {
                Cursor.Current = Cursors.WaitCursor;
                ProjectOrOrder = ProjectOrOrder.ToUpper();
                LIMS.Linaa.Msg("I'm loading it", "Found... " + ProjectOrOrder);
                //  LIMS.IReport.Speak("Found... " + ProjectOrOrder + "...");

                this.Create(sender, true, ProjectOrOrder);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                bool fromFile = System.IO.File.Exists(possiblefile);
                bool usrCall = LIMS.Linaa.OrdersList.Contains(ProjectOrOrder);

                if (fromFile || usrCall)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    LIMS.Linaa.Msg("I'm loading it", "Found... " + ProjectOrOrder);

                    if (fromFile) LIMS.Linaa.ReadLinaa(possiblefile);

                    Application.DoEvents();

                    ucOrder ucOrder = CreateOrder(ProjectOrOrder);

                    if (fromFile)
                    {
                        ICollection<string> ls = LIMS.Linaa.ActiveProjectsList;
                        foreach (string project in ls) this.Create(ucOrder, false, project.ToUpper());
                    }
                    else ucOrder.Populate();

                    Box.Text = string.Empty;
                }
                else
                {
                    DialogResult result = DialogResult.No;

                    if (sender.Equals(Box))
                    {
                        string text = "This Irradiation Project does not exist.\nCreate new one?";
                        LIMS.Linaa.Msg(text, "Not Found... " + ProjectOrOrder);
                        result = MessageBox.Show(text, "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    }

                    if (DialogResult.No == result) return;

                    ProjectOrOrder = ProjectOrOrder.ToUpper();

                    UserControl control = LIMS.CreateUI(LIMS.uc.Irradiations);

                    if (control == null) return;

                    Program.UserControls.Add(control);

                    ucIrradiationsRequests ucIrrReq = control as ucIrradiationsRequests;
                    ucIrrReq.Name = ProjectOrOrder + " - New Irradiation";

                    Application.DoEvents();
                    ucIrrReq.CreateNewIrradiation(ProjectOrOrder);
                    LIMS.Linaa.Msg("And there was light...", "Creating... " + ProjectOrOrder);

                    this.Box_KeyUp(sender, e);
                    return;
                }
            }

            if (LIMS.Linaa.CurrentPref.RowState == System.Data.DataRowState.Added)
            {
                LIMS.Linaa.Msg("You can change your preferences any time, such as:\n\nPeak-search window...;\nMinimum Peak-Area...;\nMaximum Peak-Uncertainty...;\netc...\n\nI will retrieve, use and store your latest preferences", "Hi, you are a new user!...");
                LIMS.Linaa.CurrentPref.AcceptChanges();
            }

            if (Box.Text.Equals(string.Empty)) return;

            if (Cursor.Current == Cursors.WaitCursor)
            {
                LIMS.Linaa.CurrentPref.LastIrradiationProject = Box.Text;
                LIMS.Linaa.SavePreferences();
                Box.Text = string.Empty;

                Cursor.Current = Cursors.Default;
                Box.Focus();
            }
        }

        protected ucOrder CreateOrder(string ProjectOrOrder)
        {
            ucOrder ucOrder = null;
            ucOrder = Program.UserControls.OfType<ucOrder>().FirstOrDefault(o => o.Box.Text.CompareTo(ProjectOrOrder) == 0);
            if (ucOrder == null || ucOrder.IsDisposed)
            {
                ucOrder = null;
                ucOrder = new ucOrder(ref LIMS.Linaa, ProjectOrOrder);
                Program.UserControls.Add(ucOrder);
            }

            return ucOrder;
        }

        protected void Create(object sender, bool load, string project)
        {
            bool newForm = false;
            Type tipo = sender.GetType();
            bool todopanel = tipo.Equals(typeof(ucToDoPanel));
            bool order = tipo.Equals(typeof(ucOrder));

            ucSamples ucSamples = null;
            ucSamples = Program.FindLastControl<ucSamples>(project);

            if (ucSamples == null || ucSamples.IsDisposed || todopanel)
            {
                bool clone = false;

                DB.UI.ISubSamples ISS = null;
                ISS = Program.FindLastControl<ucSubSamples>(project);
                if (ISS == null)
                {
                    UserControl control = DB.UI.LIMS.CreateUI(LIMS.uc.SubSamples);
                    if (control == null) return;

                    control.Name = project;
                    ISS = control as DB.UI.ucSubSamples;
                    ISS.Offline = !load;
                    ISS.CanSelectProject = false;
                    AuxiliarForm form = new AuxiliarForm();
                    form.Populate(ref control);
                }

                if (todopanel && ucSamples != null && !ucSamples.IsDisposed) clone = true;

                Interface interf = LIMS.Interface;

                ucSamples = new ucSamples(ref  interf);
                ucSamples.ISS = ISS;
                Program.UserControls.Add(ucSamples);
                newForm = true;
                ucSamples.Name = project;
                ucSamples.IsClone = clone;
            }

            ucSamples.Populate.PerformClick();  //population....

            if (!newForm) return;

            if (order)
            {
                ucOrder ucOrder = sender as ucOrder;
                ucOrder.AddProject(ref ucSamples);
            }
            if (!todopanel) ucSamples.NewForm();
        }

        private void mimetic_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = !this.mimetic.Checked;
        }

        private void autoload_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(autoload)) LIMS.Linaa.CurrentPref.AutoLoad = this.autoload.Checked;
            else if (sender.Equals(fillbyHL)) LIMS.Linaa.CurrentPref.FillByHL = this.fillbyHL.Checked;
            else if (sender.Equals(fillBySpectra)) LIMS.Linaa.CurrentPref.FillBySpectra = this.fillBySpectra.Checked;
            LIMS.Linaa.SavePreferences();
        }

        private void Help_Click(object sender, EventArgs e)
        {
            LIMS.Linaa.Help();
        }
    }
}