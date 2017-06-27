using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.Tools;
using Rsx.Dumb;
namespace k0X
{
    public interface IWatchersForm
    {
        // System.Collections.Generic.IEnumerable<ucDetWatch> DetWatchers { get; }

        /// <summary>
        /// Returns the DetectorWatcher associated to that detector.
        /// Creates a new one if none is found and force = true
        /// </summary>
        /// <param name="detector">detector watcher to open</param>
        /// <param name="force">true to create a new one if none is found</param>
        /// <returns></returns>
        ucDetWatch Open(string detector, bool force);

        void Speak(string text);

        System.Windows.Forms.ToolStripButton Clear { get; }
        System.Windows.Forms.ToolStripButton Closer { get; }
        System.Windows.Forms.ToolStripButton Hider { get; }
        System.Windows.Forms.ToolStripButton Save { get; }
        System.Windows.Forms.ToolStripButton Schedule { get; }
        System.Windows.Forms.ToolStripButton Start { get; }
        System.Windows.Forms.ToolStripButton Stop { get; }
        IEnumerable<TreeNode> Detectors { get; }
        string[] DetectorsList { get; set; }

        void Dispose();

        void PopulateNodes(string[] detList);

        void AddOrFixSchedule(string subject, string email, string summary);

        void ExecuteCmd(string email, string cmd, string det);

        void Show();
    }
}

namespace k0X
{
    public partial class WatchersForm : Form, k0X.IWatchersForm
    {
        #region Interface

        public k0X.IWatchersForm IWatchers
        {
            get
            {
                return this;
            }
        }

        public ToolStripButton Save
        {
            get
            {
                return this.save;
            }
        }

        public ToolStripButton Start
        {
            get
            {
                return this.start;
            }
        }

        public ToolStripButton Stop
        {
            get
            {
                return this.stop;
            }
        }

        public ToolStripButton Clear
        {
            get
            {
                return this.clear;
            }
        }

        public ToolStripButton Schedule
        {
            get
            {
                return this.sched;
            }
        }

        public ToolStripButton Closer
        {
            get
            {
                return this.closelbl;
            }
        }

        public ToolStripButton Hider
        {
            get
            {
                return this.hide;
            }
        }

        public IEnumerable<TreeNode> Detectors
        {
            get
            {
                return this.TV.Nodes.OfType<TreeNode>();
            }
        }

        #endregion Interface

        protected internal Interface Interface;

        protected internal IList<ucDetWatch> watchersList;
        protected internal ucSchAcqs Scheduler;

        /// <summary>
        /// A non-stoping guacamaya
        /// </summary>
        protected internal SpeechLib.SpVoice lorito;

        protected internal string[] detectorsList;

        public string[] DetectorsList
        {
            get { return detectorsList; }
            set { detectorsList = value; }
        }

        public WatchersForm(ref Interface set)
        {
            InitializeComponent();

            SchTimer.Enabled = false;

            WindowState = FormWindowState.Normal;

            Interface = set;

            this.MouseEnter += WatchersForm_MouseEnter;

            SchTimer.Interval = 60000;
            SchTimer.Enabled = true;
        }

        private void WatchersForm_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        public void PopulateNodes(string[] detList)
        {
            if (detList == null || detList.Count() == 0)
            {
                detectorsList = new string[] {
            "CALVIN",
            "PANORAMX",
            "ASTERIX",
            "HOBBES",
            "EVA",
            "OBELIX"};
            }
            else detectorsList = detList;
            TV.Nodes.Clear();
            foreach (string det in detectorsList)
            {
                TreeNode node = new TreeNode(det);
                node.Name = det;
                TV.Nodes.Add(det);
            }
            TV.CheckBoxes = true;
        }

        /// <summary>
        /// Speaks the following text
        /// </summary>
        /// <param name="text"></param>

        public void Speak(string text)
        {
            if (lorito == null) lorito = new SpeechLib.SpVoice();
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(speaker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(speaker_RunWorkerCompleted);
            worker.RunWorkerAsync(text);
        }

        protected internal void speaker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            worker.Dispose();
            worker = null;
        }

        protected internal void speaker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string text = e.Argument as string;
                this.lorito.Speak(text);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void ScheduledAcqTimer_Tick(object sender, EventArgs e)
        {
            SchTimer.Enabled = false;
            int time = 300;

            try
            {
                if (!Interface.IPreferences.IsSpectraPathOk) return;

                string windowsUser = Interface.IPreferences.CurrentPref.WindowsUser.ToUpper();

                if (!windowsUser.Contains("NAA") && !windowsUser.Contains("SPECTRO")) return;

                Command_Click(SchTimer, e);

                Interface.IPopulate.ISchedAcqs.PopulateScheduledAcqs();
                IEnumerable<LINAA.SchAcqsRow> lasts = Interface.IPopulate.ISchedAcqs.FindLastSchedules();
                if (lasts != null && lasts.Count() != 0)
                {
                    string content = string.Empty;
                    foreach (LINAA.SchAcqsRow sch in lasts)
                    {
                        if (EC.IsNuDelDetch(sch)) continue;
                        LINAA.SchAcqsRow aux = sch;
                        int time2 = ResumeSchedule(ref aux);
                        if (time2 < time) time = time2;
                    }
                }
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
            if (time > 0) SchTimer.Interval = time * 1000;
            SchTimer.Enabled = true;
        }

        /// <summary>
        /// Returns the DetectorWatcher associated to that detector.
        /// Creates a new one if none is found and force = true
        /// </summary>
        /// <param name="detector">detector watcher to open</param>
        /// <param name="force">true to create a new one if none is found</param>
        /// <returns></returns>
        public ucDetWatch Open(string detector, bool force)
        {
            ucDetWatch runing = null;
            if (string.IsNullOrEmpty(detector)) return runing;

            try
            {
                runing = this.watchersList.FirstOrDefault(o => o.detBox.Text.Trim().ToUpper().CompareTo(detector.ToUpper()) == 0);
                if (runing == null && force)
                {
                    if (!Interface.IPreferences.IsSpectraPathOk) return runing;
                    //    LINAA Linaa = (LINAA)Interface.Get();
                    runing = new ucDetWatch(ref Interface);
                    Application.DoEvents();
                }

                if (runing == null) return runing;

                this.watchersList.Add(runing);
                runing.SetDetector(detector);
                if (runing.Hider.Text.CompareTo("Hide") == 0) runing.Hider.PerformClick();
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
            return runing;
        }

        protected internal int ResumeSchedule(ref DB.LINAA.SchAcqsRow lasts)
        {
            if (!lasts.IsCheckedOnNull() && !lasts.IsRefreshRateNull())
            {
                if ((DateTime.Now - lasts.CheckedOn).TotalSeconds < 4 * lasts.RefreshRate)
                {
                    lasts.NotCrashed = true;
                }
                else lasts.NotCrashed = false;
            }
            if (!lasts.NotCrashed)
            {
                ucDetWatch runing = Open(lasts.Detector, true);
            }

            return 3 * lasts.RefreshRate;
        }

        public IList<string> SpectraDirectories
        {
            get
            {
                return Rsx.Dumb.IO.GetDirectories(Interface.IPreferences.CurrentPref.Spectra);
            }
        }

        protected internal void CommandADetector(object sender, string detector)
        {
            bool open = true;
            if (sender.Equals(this.closelbl) || sender.Equals(this.hide)) open = false;
            ucDetWatch watcher = Open(detector, open);
            if (watcher != null)
            {
                if (sender.Equals(this.save)) watcher.Save.PerformClick();
                else if (sender.Equals(this.clear)) watcher.Clear.PerformClick();
                else if (sender.Equals(this.stop)) watcher.Stop.PerformClick();
                else if (sender.Equals(this.start)) watcher.Start.PerformClick();
                else if (sender.Equals(this.closelbl))
                {
                    watcher.ParentForm.Close();
                }
                else if (sender.Equals(this.hide))
                {
                    watcher.ParentForm.Visible = false;
                }
                if (open && !watcher.ParentForm.Visible) watcher.ParentForm.Visible = true;
            }
            Application.DoEvents();
        }

        public void ExecuteCmd(string email, string cmd, string det)
        {
            ucDetWatch watcher = null;

            string msg = string.Empty;
            string title = cmd + " REPORT";
            bool all = false;
            if (det.Contains(Properties.Cmds.ALL)) all = true;
            foreach (string detector in DetectorsList)
            {
                if (!detector.Contains(det) && !all) continue;
                bool open = true;

                if (cmd.Contains(Properties.Cmds.CHECK) || cmd.Contains(Properties.Cmds.CLOSE) || cmd.Contains(Properties.Cmds.SET)) open = false;

                watcher = this.Open(detector, open);

                if (watcher != null)
                {
                    msg += watcher.SetCommand(cmd, email) + "\n\n";
                    if (!all) title += " on " + detector;
                }
                else if (!open)
                {
                    msg += "No Detector Watchers are opened for " + detector + "\n";
                    msg += " For security, you must send DO OPEN " + detector;
                    msg += " first, in order to open a Detector Watcher.\nAftwerwards, you can send the commands\n\n";
                }
            }

            if (!msg.Equals(string.Empty))
            {
                Interface.IReport.GenerateReport(title, string.Empty, msg, string.Empty, email);
            }
        }

        public void AddOrFixSchedule(string subject, string email, string summary)
        {
            LINAA.SchAcqsDataTable table = ((LINAA)Interface.Get()).SchAcqs;

            string msg = "Hello\n\nExceution of command ";
            LINAA.SchAcqsRow row = null;
            if (subject.Contains(Properties.Cmds.FIX))
            {
                string id = subject.Replace(Properties.Cmds.FIX, null).Trim();
                Int32 SID = Convert.ToInt32(id);
                row = table.FindBySID(SID);
                msg += "FIX completed\n\nUpdated information about the scheduled measurement:\n\n";
            }
            else
            {
                row = table.NewSchAcqsRow();
                msg += "ADD completed\n\nNow rememeber to switch the Interrupted Status to <True> (through a FIX SID) for activating the scheduled measurement:\n\n";
            }

            HashSet<string> hs = new HashSet<string>(table.Columns.OfType<DataColumn>().Select(c => c.ColumnName));
            hs.TrimExcess();

            string[] lines = summary.Split('\n');
            foreach (string line in lines)
            {
                string[] fV = line.Split(' ');
                if (fV == null || fV.Length == 0) continue;
                for (int i = 0; i < fV.Length; i++)
                {
                    bool isField = fV[i].Contains(":") && !fV[i].Contains("T");
                    if (!isField) continue;
                    string Value = fV[i + 1].Trim();
                    string field = fV[i].Replace(":", null).Trim();
                    if (table.Columns.Contains(field))
                    {
                        Type tipo = row.Table.Columns[field].DataType;
                        row.SetField(field, Convert.ChangeType(Value, tipo));
                    }
                }
            }

            string stillMissing = string.Empty;
            if (!EC.IsNuDelDetch(row))
            {
                if (row.IsRepeatsNull()) row.Repeats = 1;
                row.Reset();
                row.Interrupted = true;
                row.User = email;
                table.AddSchAcqsRow(row);
            }

            Interface.IStore.Save<LINAA.SchAcqsDataTable>();

            foreach (string col in hs)
            {
                if (!row.IsNull(col)) hs.Remove(col);
                else stillMissing += col + " ; ";
            }

            string content = row.GetReportString();

            if (!stillMissing.Equals(string.Empty)) content += "\n\nThe Fields still missing (null) are :\t" + stillMissing;

            Interface.IReport.GenerateReport("FIX REPORT", string.Empty, msg + content, string.Empty, email);
        }

        protected internal void Command_Click(object sender, EventArgs e)
        {
            bool visible = true;
            if (sender.Equals(this.SchTimer))
            {
                sender = this.sched;
                visible = false;
            }
            if (sender.Equals(this.sched))
            {
                if (Scheduler == null || Scheduler.IsDisposed)
                {
                    LINAA linaa = (LINAA)Interface.Get();
                    Scheduler = new ucSchAcqs(ref linaa);

                    Scheduler.ParentForm.Visible = false;
                }
                Application.DoEvents();
                if (visible)
                {
                    if (!Scheduler.ParentForm.Visible)
                    {
                        Scheduler.ParentForm.Visible = visible;
                        Scheduler.ParentForm.WindowState = FormWindowState.Normal;
                        Scheduler.ParentForm.Select();
                        return;
                    }
                }
                Scheduler.ParentForm.Visible = false;
            }
            else
            {
                foreach (TreeNode tn in TV.Nodes)
                {
                    if (!tn.Checked) continue;
                    CommandADetector(sender, tn.Text);
                }
            }
        }

        protected internal void TV_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.Checked = true;
            this.CommandADetector(sender, e.Node.Text);
        }

        protected internal void WatchersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ucDetWatch watcher in this.watchersList)
            {
                watcher.ParentForm.Close();
            }

            this.SchTimer.Enabled = false;
        }
    }
}