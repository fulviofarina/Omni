﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.Tools;
using DB.UI;
using Rsx.Dumb;
using VTools;

namespace k0X
{
    public partial class MainForm : Form
    {
        public IPeriodicTable ucPeriodic;
        private Pop msn = null;
        private bool shouldWatchEmail = false;
        private k0X.IWatch watch = null;

        public delegate IPeriodicTable StartAnalyzer();

        public static void Create(object sender, bool load, string project)
        {
            bool newForm = false; //deserves a new form?

            //sender type
            Type tipo = sender.GetType();

            //is a todo panel?
            bool todopanel = tipo.Equals(typeof(ucToDoPanel));

            //is an order?
            bool order = tipo.Equals(typeof(ucOrder));

            ucSamples ucSamples = null;
            //find last control with that project name
            ucSamples = Creator.Util.FindLastControl<ucSamples>(project);

            if (ucSamples == null || ucSamples.IsDisposed || todopanel)
            {
                DB.UI.ucSubSamples ISS = null;
                //find subsample control
                ISS = Creator.Util.FindLastControl<ucSubSamples>(project);
                if (ISS == null)
                {
                    UserControl control = DB.UI.LIMSUI.CreateUI(ControlNames.SubSamples);
                    if (control == null)
                    {
                        return;
                    }
                    // control.Name = project;
                    ISS = control as DB.UI.ucSubSamples;

                    ISS.Name = project;
                    // ISS.Project = project;
                    ISS.projectbox.Offline = !load;
                    ISS.projectbox.Enabled = false;
                    ISS.ucContent.Set(ref LIMSUI.Interface);
                    AuxiliarForm form = new AuxiliarForm();
                    form.Populate(ref control);
                }

                //clone or not to clone
                bool clone = (todopanel && ucSamples != null && !ucSamples.IsDisposed);

                // Interface interf = LIMS.Interface;

                ucSamples = new ucSamples(ref LIMSUI.Interface);
                ucSamples.ISubS = ISS;
                ucSamples.Name = project;
                Creator.UserControls.Add(ucSamples);
                newForm = true;

                ucSamples.IsClone = clone;
            }

            ucSamples.Populate.PerformClick();  //population....

            if (!newForm)
            {
                return;
            }
            if (order)
            {
                ucOrder ucOrder = sender as ucOrder;
                ucOrder.AddProject(ref ucSamples);
            }
            if (!todopanel)
            {
                ucSamples.NewForm();
            }
        }

        /// <summary>
        /// Start loading everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        public static ucOrder CreateOrder(string ProjectOrOrder)
        {
            ucOrder ucOrder = null;
            ucOrder = Creator.UserControls.OfType<ucOrder>().FirstOrDefault(o => o.Box.Text.CompareTo(ProjectOrOrder) == 0);
            if (ucOrder == null || ucOrder.IsDisposed)
            {
                ucOrder = null;
                ucOrder = new ucOrder(ref LIMSUI.Linaa, ProjectOrOrder);
                Creator.UserControls.Add(ucOrder);
            }

            return ucOrder;
        }

        public void Box_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue < 47 || e.KeyValue > 105) && e.KeyCode != Keys.Enter) return;

            string ProjectOrOrder = Box.Text.Trim();
            if (ProjectOrOrder.Equals(string.Empty)) return;

            string possiblefile = LIMSUI.Linaa.FolderPath + DB.Properties.Resources.Backups + ProjectOrOrder + ".xml";

            if (LIMSUI.Linaa.ProjectsList.Contains(ProjectOrOrder.ToUpper()))
            {
                Cursor.Current = Cursors.WaitCursor;
                ProjectOrOrder = ProjectOrOrder.ToUpper();
                LIMSUI.Interface.IReport.Msg("I'm loading it", "Found... " + ProjectOrOrder);
                // LIMS.IReport.Speak("Found... " + ProjectOrOrder + "...");

                Create(sender, true, ProjectOrOrder);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                bool fromFile = System.IO.File.Exists(possiblefile);
                bool usrCall = LIMSUI.Linaa.OrdersList.Contains(ProjectOrOrder);

                if (fromFile || usrCall)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    LIMSUI.Interface.IReport.Msg("I'm loading it", "Found... " + ProjectOrOrder);

                    if (fromFile) LIMSUI.Linaa.ReadLIMS(possiblefile);

                    Application.DoEvents();

                    ucOrder ucOrder = CreateOrder(ProjectOrOrder);

                    if (fromFile)
                    {
                        ICollection<string> ls = LIMSUI.Linaa.ActiveProjectsList;
                        foreach (string project in ls) Create(ucOrder, false, project.ToUpper());
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
                        LIMSUI.Interface.IReport.Msg(text, "Not Found... " + ProjectOrOrder);
                        result = MessageBox.Show(text, "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    }

                    if (DialogResult.No == result) return;

                    ProjectOrOrder = ProjectOrOrder.ToUpper();

                    UserControl control = LIMSUI.CreateUI(ControlNames.Irradiations);

                    // if (control == null) return;

                    Creator.UserControls.Add(control);

                    ucIrradiationsRequests ucIrrReq = control as ucIrradiationsRequests;
                    ucIrrReq.Name = ProjectOrOrder + " - New Irradiation";

                    Application.DoEvents();
                    LIMSUI.Interface.IPopulate.IIrradiations.AddNewIrradiation(ProjectOrOrder);
                    LIMSUI.Interface.IReport.Msg("And there was light...", "Creating... " + ProjectOrOrder);

                    this.Box_KeyUp(sender, e);
                    return;
                }
            }

            if (LIMSUI.Interface.IPreferences.CurrentPref.RowState == System.Data.DataRowState.Added)
            {
                LIMSUI.Interface.IReport.Msg("You can change your preferences any time, such as:\n\nPeak-search window...;\nMinimum Peak-Area...;\nMaximum Peak-Uncertainty...;\netc...\n\nI will retrieve, use and store your latest preferences", "Hi, you are a new user!...");
                LIMSUI.Interface.IPreferences.CurrentPref.AcceptChanges();
            }

            if (Box.Text.Equals(string.Empty)) return;

            if (Cursor.Current == Cursors.WaitCursor)
            {
                LIMSUI.Interface.IPreferences.CurrentPref.LastIrradiationProject = Box.Text;
                LIMSUI.Interface.IPreferences.SavePreferences();
                Box.Text = string.Empty;

                Cursor.Current = Cursors.Default;
                Box.Focus();
            }
        }

        public void Link()
        {
            UIControl.FillABox(this.Box, LIMSUI.Linaa.ProjectsList, true, false);
            UIControl.FillABox(this.Box, LIMSUI.Linaa.OrdersList, false, false);

            this.autoload.Checked = LIMSUI.Interface.IPreferences.CurrentPref.AutoLoad;
            this.fillbyHL.Checked = LIMSUI.Interface.IPreferences.CurrentPref.FillByHL;
            this.fillBySpectra.Checked = LIMSUI.Interface.IPreferences.CurrentPref.FillBySpectra;
        }

        public void Panel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (sender == this.ToDoPanel || sender.GetType().Equals(typeof(ucPeaks)))
            {
                ucToDoPanel todo = new ucToDoPanel(ref LIMSUI.Interface);
                Creator.UserControls.Add(todo);
                Creator.UserControls.Add(todo.ucToDoData);
                todo.panel1box.Text = LIMSUI.Interface.IPreferences.CurrentPref.LastIrradiationProject;
                todo.panel2box.Text = LIMSUI.Interface.IPreferences.CurrentPref.LastIrradiationProject;
            }
            else if (sender == this.Analysis || sender.GetType().Equals(typeof(ucSamples)))
            {
                if (ucPeriodic == null || ucPeriodic.IsDisposed)
                {
                    object dataset = LIMSUI.Linaa;

                    IPeaks ucP = new ucPeaks(ref dataset);
                    Creator.UserControls.Add(ucP);
                    IEnumerable<LINAA.ElementsRow> elements = LIMSUI.Linaa.Elements.AsEnumerable();
                    ucPeriodic = new ucPeriodicTable(ref elements);  //creates the interface
                    ucP.IPeriodicTable = ucPeriodic;  //docks the ucPeriodic interface
                    Creator.UserControls.Add(ucPeriodic);
                }

                ucPeriodic.Projects = Hash.HashFrom<string>(LIMSUI.Linaa.SubSamples.IrradiationCodeColumn);
            }
            else if (sender.Equals(this.SolCoiPanel))
            {
                ucSolcoi sol = new ucSolcoi(ref LIMSUI.Interface);
                Creator.UserControls.Add(sol);
                Creator.UserControls.Add(sol.ucDetectors);
            }
            else if (sender.Equals(this.HyperLabData))
            {
             //   ucHyperLab HL = new ucHyperLab();
              //  HL.MainForm = this;
               // LIMS.UserControls.Add(HL);
            }
            else if (sender.Equals(this.ExplorerMenu))
            {
                LIMSUI.Explore();
            }
            else if (sender.Equals(this.LIMSData))
            {
                LIMSUI.Form.Show();
                LIMSUI.Form.Activate();
            }

            Cursor.Current = Cursors.Default;
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
                    // string user = System.Environment.UserName.Replace("SCK\\", null); mydocs +=
                    // "\\Outlook Files\\" + System.Environment.UserName.Replace("SCK\\", null) +
                    // "@SCKCEN.BE.ost"; MessageBox.Show(mydocs);

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

        private void disableEnableBox(bool enable)
        {
            if (!enable)
            {
                this.MS.Enabled = false;
                this.Box.Enabled = false;
                this.Box.BackColor = System.Drawing.Color.Gray;
            }
            else
            {
                this.MS.Enabled = true;
                //reactivate boxes
                this.Box.BackColor = System.Drawing.Color.RosyBrown;

                this.Box.Enabled = true;
                this.Box.Focus();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = DB.Tools.Creator.Close();

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

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                //put gray, disable boxes
                disableEnableBox(false);

                //PopUp create
                msn = new Pop(false);

                //Build
                Creator.Set();

                IucSQLConnection IConn = new VTools.ucSQLConnection();
                // dynamic connectionControl = IConn;
                bool ok = Creator.UtilSQL.SQLPrepare(false);
                // bool ok = Creator.PrepareSQL();
                LIMSUI.Interface.IPreferences
                 .CurrentPref.IsSQL = ok;

                LIMSUI.Interface.IReport.CheckRestartFile();

                if (ok)
                {
                    LIMSUI.Interface.IPreferences.SavePreferences();

                    Creator.LoadMethods(0);
                }
                //       string result = DB.Tools.Creator.Build(ref LIMS.Linaa, ref this.notify, ref msn, 0);
                //
                //set medium callback
                DB.Tools.Creator.CallBack = delegate
                {
                    //this is OK
                    LIMSUI.Linaa = LIMSUI.Interface.Get();
                    LIMSUI.Form = new LIMS(ref LIMSUI.Interface); //make a new UI LIMS
                                            //make a new interface
                                            //      LIMS.Interface = new Interface(ref LIMS.Linaa);

                    Link(); //dont remember

                    //re-enable boxes
                    disableEnableBox(true);

                    //is autoload Activated?
                    if (!this.autoload.Checked) return; //if auto-load last project is activated...
                                                        // find project to autoload
                    bool projectNull = LIMSUI.Interface.IPreferences.CurrentPref
                    .IsLastIrradiationProjectNull();
                    if (!projectNull)
                    {
                        Box.Text = LIMSUI.Interface.IPreferences.CurrentPref.LastIrradiationProject;
                        KeyEventArgs ev = new KeyEventArgs(Keys.Enter);
                        Box_KeyUp(this.Box, ev);
                    }
                };

                //set lastcallback
                DB.Tools.Creator.LastCallBack = delegate
                {
                    if (!shouldWatchEmail) return;
                    //put this line back!!!
                    //so you can get the watcher running
                    if (watch == null)
                    {
                        // Interface Inter = LIMS.Interface;
                        watch = new k0X.Watch(ref LIMSUI.Interface);
                        this.talkTSMI.Click -= watch.TalkHandler;
                        this.Detectors.Click -= watch.DetectorsHandler;
                        this.talkTSMI.Click += watch.TalkHandler;
                        this.Detectors.Click += watch.DetectorsHandler;
                    }
                };

                //could not connect?
                if (!ok)
                {
                    MessageBox.Show("No connection", "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Connections_Click(null, EventArgs.Empty);
                }
                else
                {
                    //LOAD
                    Creator.Run();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Cannot Start! Error: " + ex.Message);
            }
        }

        private void ReportMenu_Click(object sender, EventArgs e)
        {
            Rsx.CAM.Linx linx = new Rsx.CAM.Linx();
        }

        public MainForm()
        {
            InitializeComponent();
            this.SuspendLayout();

            this.notify.Icon = this.Icon;

            System.Drawing.Rectangle area = Screen.PrimaryScreen.WorkingArea;
            int y = (area.Bottom - 100 - this.Height);
            int x = (area.Right - 100 - this.Width);
            this.Location = new System.Drawing.Point(x, y);

            //set user control list
            Creator.UserControls = new System.Collections.Generic.List<object>();

            //set the timer
            this.timer.Enabled = true;
            this.timer.Interval = 3600000;
            this.timer.Tick += delegate
            {
                ReleaseMemory(this.Release);
                LIMSUI.Interface.IReport.GenerateBugReport();
            };

            this.ResumeLayout(true);

            /*
              Rsx.XSD2SQL n = new Rsx.XSD2SQL(Properties.Settings.Default.k0ConnectionString,this.Linaa);
              n.CreateDatabase("NUE");
               n.PopulateDatabase();
              */
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
    }
}