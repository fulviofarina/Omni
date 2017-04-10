using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;

//using DB.Interfaces;
using DB.UI;

using Msn;
using Rsx;

namespace k0X
{
    public partial class MainForm : Form
    {
        public IPeriodicTable ucPeriodic;
        private Pop msn = null;
        private k0X.IWatch watch = null;

        private bool shouldWatchEmail = false;

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
            LIMS.UserControls = new System.Collections.Generic.List<object>();

            //set the timer
            this.timer.Enabled = true;
            this.timer.Interval = 3600000;
            this.timer.Tick += delegate
            {
                ReleaseMemory(this.Release);
                LIMS.Linaa.GenerateBugReport();
            };

            this.ResumeLayout(true);

            /*
              Rsx.XSD2SQL n = new Rsx.XSD2SQL(Properties.Settings.Default.k0ConnectionString,this.Linaa);
              n.CreateDatabase("NUE");
               n.PopulateDatabase();
              */
        }

        public delegate IPeriodicTable StartAnalyzer();

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

                Create(sender, true, ProjectOrOrder);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                bool fromFile = System.IO.File.Exists(possiblefile);
                bool usrCall = LIMS.Linaa.OrdersList.Contains(ProjectOrOrder);

                if (fromFile || usrCall)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    LIMS.Linaa.Msg("I'm loading it", "Found... " + ProjectOrOrder);

                    if (fromFile) LIMS.Linaa.Read(possiblefile);

                    Application.DoEvents();

                    ucOrder ucOrder = CreateOrder(ProjectOrOrder);

                    if (fromFile)
                    {
                        ICollection<string> ls = LIMS.Linaa.ActiveProjectsList;
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
                        LIMS.Linaa.Msg(text, "Not Found... " + ProjectOrOrder);
                        result = MessageBox.Show(text, "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    }

                    if (DialogResult.No == result) return;

                    ProjectOrOrder = ProjectOrOrder.ToUpper();

                    UserControl control = LIMS.CreateUI(ControlNames.Irradiations);

                    if (control == null) return;

                    LIMS.UserControls.Add(control);

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

        public void Link()
        {
            Dumb.FillABox(this.Box, LIMS.Linaa.ProjectsList, true, false);
            Dumb.FillABox(this.Box, LIMS.Linaa.OrdersList, false, false);

            this.autoload.Checked = LIMS.Linaa.CurrentPref.AutoLoad;
            this.fillbyHL.Checked = LIMS.Linaa.CurrentPref.FillByHL;
            this.fillBySpectra.Checked = LIMS.Linaa.CurrentPref.FillBySpectra;
        }

        public void Panel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (sender == this.ToDoPanel || sender.GetType().Equals(typeof(ucPeaks)))
            {
                ucToDoPanel todo = new ucToDoPanel(ref LIMS.Linaa);
                LIMS.UserControls.Add(todo);
                LIMS.UserControls.Add(todo.ucToDoData);
                todo.panel1box.Text = LIMS.Linaa.CurrentPref.LastIrradiationProject;
                todo.panel2box.Text = LIMS.Linaa.CurrentPref.LastIrradiationProject;
            }
            else if (sender == this.Analysis || sender.GetType().Equals(typeof(ucSamples)))
            {
                if (ucPeriodic == null || ucPeriodic.IsDisposed)
                {
                    object dataset = LIMS.Linaa;

                    IPeaks ucP = new ucPeaks(ref dataset);
                    LIMS.UserControls.Add(ucP);
                    IEnumerable<LINAA.ElementsRow> elements = LIMS.Linaa.Elements.AsEnumerable();
                    ucPeriodic = new ucPeriodicTable(ref elements);  //creates the interface
                    ucP.IPeriodicTable = ucPeriodic;  //docks the ucPeriodic interface
                    LIMS.UserControls.Add(ucPeriodic);
                }

                ucPeriodic.Projects = Dumb.HashFrom<string>(LIMS.Linaa.SubSamples.IrradiationCodeColumn);
            }
            else if (sender.Equals(this.MatSSFPanel))
            {
                ucSSF matssf = new ucSSF();
                matssf.Set(ref LIMS.Interface);
                LIMS.UserControls.Add(matssf);
            }
            else if (sender.Equals(this.SolCoiPanel))
            {
                ucSolcoi sol = new ucSolcoi(ref LIMS.Interface);
                LIMS.UserControls.Add(sol);
                LIMS.UserControls.Add(sol.ucDetectors);
            }
            else if (sender.Equals(this.HyperLabData))
            {
                ucHyperLab HL = new ucHyperLab();
                HL.MainForm = this;
                LIMS.UserControls.Add(HL);
            }
            else if (sender.Equals(this.ExplorerMenu))
            {
                LIMS.Explore();
            }
            else if (sender.Equals(this.LIMSData))
            {
                LIMS.Form.Show();
                LIMS.Form.Activate();
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

        /// <summary>
        /// Start loading everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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
            ucSamples = LIMS.FindLastControl<ucSamples>(project);

            if (ucSamples == null || ucSamples.IsDisposed || todopanel)
            {
                DB.UI.ucSubSamples ISS = null;
                //find subsample control
                ISS = LIMS.FindLastControl<ucSubSamples>(project);
                if (ISS == null)
                {
                    UserControl control = DB.UI.LIMS.CreateUI(ControlNames.SubSamples);
                    if (control == null)
                    {
                        return;
                    }
                    //  control.Name = project;
                    ISS = control as DB.UI.ucSubSamples;

                    ISS.Name = project;
                    //  ISS.Project = project;
                    ISS.projectbox.Offline = !load;
                    ISS.projectbox.CanSelectProject = false;
                    ISS.ucContent.Set(ref LIMS.Interface);
                    AuxiliarForm form = new AuxiliarForm();
                    form.Populate(ref control);
                }

                //clone or not to clone
                bool clone = (todopanel && ucSamples != null && !ucSamples.IsDisposed);

                //  Interface interf = LIMS.Interface;

                ucSamples = new ucSamples(ref LIMS.Interface);
                ucSamples.ISubS = ISS;
                ucSamples.Name = project;
                LIMS.UserControls.Add(ucSamples);
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

        public static ucOrder CreateOrder(string ProjectOrOrder)
        {
            ucOrder ucOrder = null;
            ucOrder = LIMS.UserControls.OfType<ucOrder>().FirstOrDefault(o => o.Box.Text.CompareTo(ProjectOrOrder) == 0);
            if (ucOrder == null || ucOrder.IsDisposed)
            {
                ucOrder = null;
                ucOrder = new ucOrder(ref LIMS.Linaa, ProjectOrOrder);
                LIMS.UserControls.Add(ucOrder);
            }

            return ucOrder;
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

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                //put gray, disable boxes
                disableEnableBox(false);

                //PopUp create
                msn = new Pop(false);

                //Build

                string result = DB.Tools.Creator.Build(ref LIMS.Linaa, ref this.notify, ref msn, 0);

                //set medium callback
                DB.Tools.Creator.CallBack = delegate
                {
                    //this is OK
                    LIMS.Form = new LIMS(); //make a new UI LIMS
                    //make a new interface
                    LIMS.Interface = new Interface(ref LIMS.Linaa);

                    Link(); //dont remember

                    //re-enable boxes
                    disableEnableBox(true);

                    //is autoload Activated?
                    if (!this.autoload.Checked) return; //if auto-load last project is activated...
                                                        // find project to autoload
                    bool projectNull = LIMS.Interface.IPreferences.CurrentPref
                    .IsLastIrradiationProjectNull();
                    if (!projectNull)
                    {
                        Box.Text = LIMS.Interface.IPreferences.CurrentPref.LastIrradiationProject;
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
                        watch = new k0X.Watch(ref LIMS.Interface);
                        this.talkTSMI.Click -= watch.TalkHandler;
                        this.Detectors.Click -= watch.DetectorsHandler;
                        this.talkTSMI.Click += watch.TalkHandler;
                        this.Detectors.Click += watch.DetectorsHandler;
                    }
                };

                //could not connect?
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Connections_Click(null, EventArgs.Empty);
                }
                else
                {
                    //LOAD
                    DB.Tools.Creator.Load();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Cannot Start! Error: " + ex.Message);
            }
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

        private void ReportMenu_Click(object sender, EventArgs e)
        {
            Rsx.CAM.Linx linx = new Rsx.CAM.Linx();
        }
    }
}