using DB.Properties;
using Rsx;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Windows.Forms;
using VTools;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class Report : IReport
    {
        public void InstallMSMQ()
        {
            try
            {
                Msg(MSMQ_MSG, MSMQ_TITLE);

                IO.InstallMSMQ(false);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /*
        public partial class MSMQInstaller
        {
            public MSMQInstaller()
            {
                InitializeComponent();
            }

            [DllImport("kernel32")]
            private static extern IntPtr LoadLibrary(string lpFileName);[DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool FreeLibrary(IntPtr hModule);
            public override void Install(IDictionary stateSaver)

            {
                base.Install(stateSaver); bool loaded; try { IntPtr handle = LoadLibrary("Mqrt.dll"); if (handle == IntPtr.Zero || handle.ToInt32() == 0) { loaded = false; } else { loaded = true; FreeLibrary(handle); } } catch { loaded = false; }
                if (!loaded)
                {
                    if (Environment.OSVersion.Version.Major < 6)
                    // Windows XP or earlier
                    { string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "MSMQAnswer.ans"); using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName)) { writer.WriteLine("[Version]"); writer.WriteLine("Signature = \"$Windows NT$\""); writer.WriteLine(); writer.WriteLine("[Global]"); writer.WriteLine("FreshMode = Custom"); writer.WriteLine("MaintenanceMode = RemoveAll"); writer.WriteLine("UpgradeMode = UpgradeOnly"); writer.WriteLine(); writer.WriteLine("[Components]"); writer.WriteLine("msmq_Core = ON"); writer.WriteLine("msmq_LocalStorage = ON"); } using (System.Diagnostics.Process p = new System.Diagnostics.Process()) { System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo("sysocmgr.exe", "/i:sysoc.inf /u:\"" + fileName + "\""); p.StartInfo = start; p.Start(); p.WaitForExit(); } }
                    else
                    // Vista or later
                    { using (System.Diagnostics.Process p = new System.Diagnostics.Process()) { System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo("ocsetup.exe", "MSMQ-Container;MSMQ-Server /passive"); p.StartInfo = start; p.Start(); p.WaitForExit(); } }
                }
            }
        }
        */

        public IPop Msn
        {
            get { return msn; }
        }

        public string RestartFile
        {
            get
            {
                return Interface.IStore.DevPath + Resources.Restarting;
            }
        }

        /// <summary>
        /// not used
        /// </summary>
        public void GreetUser()
        {
            try
            {
                string windowsUser = Interface.IPreferences.WindowsUser;

                string[] txtTitle = generateGreeting();

                Speak(txtTitle[0]);

                Msg(txtTitle[0], txtTitle[1]);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Checks if MSMQ is installed
        /// </summary>
        public bool CheckMSMQ()
        {
            bool ok = false;

            try
            {
                // return ok;
                string test = Properties.QM.QMExceptions;
                //install MSMQ
                MessageQueue qm = Rsx.Emailer.CreateMQ(test, null);
                if (qm != null)
                {
                    ok = true;
                    qm.Dispose();
                    qm = null;
                }

                Interface.IPreferences.CurrentPref.IsMSMQ = ok;
            }
            catch (Exception ex2)
            {
                // Interface.IReport.Msg(ex2.InnerException.Message, ex2.Message, false);
                Interface.IStore.AddException(ex2);
            }

            return ok;
        }

        /// <summary>
        /// Checks the Restarting file (developer)
        /// </summary>
        public bool CheckRestartFile()
        {
            //RESTART FILE ROUTINE
            //   string cmd = RestartFile;
            bool shouldReport = File.Exists(RestartFile);

            if (shouldReport)
            {
                string email = File.ReadAllText(RestartFile);
                Interface.IReport.GenerateReport(RESTARTING_OK, string.Empty, string.Empty, Interface.IDB.DataSetName, email);
                File.Delete(RestartFile);
            }
            // shouldReport = shouldReport || Interface.IDB.Exceptions.Count != 0;

            //should send bug report?
            //    if (!shouldReport) return false;

            //yes...

            //BUG REPORT ROUTINE
            Interface.IReport.GenerateBugReport();

            return true;
        }

        /// <summary>
        /// Generates a Bug Report
        /// </summary>
        public void GenerateBugReport()
        {
            try
            {
                MessageQueue qm = generateMessageQueue(QM.QMExceptions);
                if (qm == null) return;
                generateBugReport(ref qm);

                bugresult = qm.BeginReceive();

                // GenerateUserInfoReport();
            }
            catch (Exception ex)
            {
                // this.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Generates a Generic Report
        /// </summary>
        public void GenerateReport(string labelReport, object path, string extra, string module, string email)
        {
            string queuePath = QM.QMAcquisitions + "." + module + "." + email;
            MessageQueue qm = generateMessageQueue(queuePath);
            if (qm == null) return;

            try
            {
                //send path as the body of the QMsg
                //put a extended body to the email please

                string title = labelReport + " - " + module;

                System.Messaging.Message m = Rsx.Emailer.CreateQMsg(path, title, extra);

                Rsx.Emailer.SendQMsg(ref qm, ref m);

                qm.BeginReceive();
            }
            catch (SystemException ex)
            {
                // Interface.IReport.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Generates a UserInfo Report
        /// </summary>
        public void GenerateUserInfoReport()
        {
            try
            {
                EDB en = new EDB();

                EDB.EnvironmentRow e = en.Environment.NewEnvironmentRow();

                e.OSVersion = Environment.OSVersion.VersionString;
                e.Is64Bit = Environment.Is64BitProcess;
                e.Is64BitOS = Environment.Is64BitOperatingSystem;
                e.MachineName = Environment.MachineName;
                e.CurrentPath = Environment.CurrentDirectory;
                e.CPUCount = Environment.ProcessorCount;
                e.NewLine = Environment.NewLine;
                e.UserDomainName = Environment.UserDomainName;
                e.UserName = Environment.UserName;
                e.WorkingSet = Environment.WorkingSet;
                e.Ticks = Environment.TickCount;

                en.Environment.AddEnvironmentRow(e);

                string enviropath = "Enviroment.xml";
                en.WriteXml(enviropath, System.Data.XmlWriteMode.WriteSchema);

                Dumb.FD(ref en);

                string path = e.CurrentPath + "\\" + enviropath;
                string usrInfo = "UserInfo";

                GenerateReport(enviropath, path, string.Empty, usrInfo, EMAIL_DEFAULT);
            }
            catch (SystemException ex)
            {
                // Interface.IReport.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Notifies the given message and title with an Info icon
        /// </summary>
        /// <param name="msg">  Message to display</param>
        /// <param name="title">title of the message</param>
        public void Msg(string msg, string title, object ok = null, bool accumulate = false)
        {
            this.msn.Msg(msg, title, ok, accumulate);
        }

        /// <summary>
        /// Method to report progress to pass to e.g. progress bars, etc
        /// </summary>
        public void ReportProgress(int percentage)
        {
            this.msn.ReportProgress(percentage);
        }

        public void ReportToolTip(object sender, EventArgs ev)
        {
            // bool isSameCol = (lastColInder == e.ColumnIndex);
            string toolTip = string.Empty;
            string msg = string.Empty;
            Control ctrl = sender as Control;


            try { 

          

            bool isDGV = ctrl.GetType().Equals(typeof(DataGridView));

            if (isDGV)
            {
                DataGridView dgv = ctrl as DataGridView;
                DataGridViewCellMouseEventArgs e = ev as DataGridViewCellMouseEventArgs;
                int index = e.ColumnIndex;
                DataGridViewHeaderCell cell = dgv.Columns[index].HeaderCell;
                toolTip = cell.ToolTipText;
                msg = "The column " + cell.OwningColumn.HeaderText + " represents:";



            }

            }
            catch (Exception )
            {
                //Interface.IStore.AddException(ex);
            }

            try
            { 


            if ( ctrl.Tag!=null)
           {
                toolTip = (string)ctrl.Tag;
                msg = "Hints:";
            }

            }
            catch (Exception )
            {
               // Interface.IStore.AddException(ex);
            }

            if (string.IsNullOrEmpty(toolTip)) return;

            try
            { 

            Msg(toolTip, msg);


            char parentesis = '(';
            if (toolTip.Contains(parentesis))
            {
                toolTip = toolTip.Split(parentesis)[0];
            }

            if (string.IsNullOrEmpty(toolTip)) return;
            // Msg(toolTip, "The column " + cell.OwningColumn.HeaderText + " represents:");


            Speak(toolTip);

        }
            catch (Exception )
            {
                //Interface.IStore.AddException(ex);
            }

}

        public void SendToRestartRoutine(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return;
            string cmd = Interface.IReport.RestartFile;

            try
            {
                bool shouldReport = System.IO.File.Exists(cmd);
                //it will write what to send to the Restarting File
                //but it will not send it until next restart...
                if (shouldReport)
                {
                    File.AppendAllText(cmd, texto);
                    // GenerateReport("Restarting succeeded...", string.Empty, string.Empty,
                    // DataSetName, email); System.IO.File.Delete(cmd);
                }
                else File.WriteAllText(cmd, texto);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        public void Speak(string text)
        {
            this.msn.Speak(text);
        }

        public void SpeakLoadingFinished()
        {
            //if (Interface.IDB.Exceptions.Count != 0)
            //{
            //  Speak("Loading finished! Some bugs were found.\n I will notify the programmer");
            //}
            Speak("Loading finished!");
            if (Interface.IDB.Exceptions.Count != 0)
            {
                Msg("Some exceptions were found.\n I will send a report", "Loading finished!");
            }
        }

        /// <summary>
        /// What this sends is a solicitude to generate an email with the exceptions The solicitude
        /// is processed, reported and when the solicitud arrives to the Queue then the email is sent
        /// Asynchroneusly... Then a feedback from the Async operations also arrives at the Queue and reports.
        /// </summary>
        private void generateBugReport(ref MessageQueue qm)
        {
            Interface.IStore.SaveExceptions();
            const int maxBugsTables = 15;
            Msg(BE_PATIENT, BUG_REPORT_ONWAY);

            string path = Interface.IStore.FolderPath + Resources.Exceptions;
            IEnumerable<string> exceptions = Directory.EnumerateFiles(path);
            // int cnt = exceptions.Count();
            if (exceptions.Count() != 0)
            {
                int counter = 1;
                System.Messaging.Message w = null;
                string bodyOfBugEmail = "Should I add more comments?";
                foreach (string excFile in exceptions)
                {
                    ExceptionsDataTable dt = new ExceptionsDataTable();
                    dt.ReadXml(excFile);
                    if (counter < maxBugsTables)
                    {
                        Interface.IDB.Exceptions.Merge(dt);
                        System.IO.File.Delete(excFile);
                        counter++;
                    }
                    if (counter == maxBugsTables) break;
                }
                string file = Guid.NewGuid() + ".xml";
                Interface.IDB.Exceptions.WriteXml(file);
                Interface.IDB.Exceptions.Clear();
                w = Emailer.CreateQMsg(file, "Bug Report", bodyOfBugEmail);
                Emailer.SendQMsg(ref qm, ref w);
                counter = 1;
                Msg(BUGS_ONTRAY, counter + " scheduled to be sent", true);
            }
            else Msg(BUG_REPORT_FAILED, BUG_REPORT_EMPTY);

            exceptions = null;
        }

        private MessageQueue generateMessageQueue(string QUEUE_PATH)
        {
            MessageQueue qm = null;

            try
            {
                ReceiveCompletedEventHandler ReceivedHandler = queryMsgReceived;
                qm = Rsx.Emailer.CreateMQ(QUEUE_PATH, ReceivedHandler);

                if (QUEUE_PATH.Contains(QM.QMAcquisitions))
                {
                    if (qm == null)
                    {
                        throw new Exception(MSMQ_CHECK, new Exception(MSMQ_NOT_OK));
                    }
                }
                else if (QUEUE_PATH.Contains(QM.QMExceptions))
                {
                    if (qm == null)
                    {
                        throw new Exception(BUG_REPORT_PROBLEM, new Exception(BUG_REPORT_FAILED));
                    }
                }
            }
            catch (Exception ex)
            {
                // Interface.IReport.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IStore.AddException(ex);
            }
            return qm;
        }

        /*
        private void installMSMQOLD()
        {
            string arg = " /online /get-features | find.exe /i \"mq\"";
            string arg2 = " /online /enable-feature /featurename:\"MSMQ-Server\"";
            //string pre = string.Empty;
            string pre = " /profile /user:" +  Interface.ICurrent.WindowsUser;

            string pre2 = pre;
            pre += " \"dism" + arg + "\"";
            pre2 += " \"dism" + arg2 + "\"";

            // string pre2 = "runas /profile /env /user:" + Interface.ICurrent.WindowsUser;

            System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo("runas", pre);
            System.Diagnostics.ProcessStartInfo info2 = new ProcessStartInfo("runas", pre2);
            Process pro = new Process();
            pro.StartInfo = info;
            Process pro2 = new Process();
            pro2.StartInfo = info2;

            // info.LoadUserProfile = true; info2.LoadUserProfile = true;
            info.UseShellExecute = true;
            info2.UseShellExecute = true;

            pro.Start();
            // pro. info. pro.WaitForExit(30000);

            pro2.Start();
        }

            private void installMSMQ()
        {
            string arg = "\\\"/online /get-features | find.exe /i \"mq\"\\\"";
            string arg2 = "\\\"/online /enable-feature /featurename:\"MSMQ-Server\"\\\"";
            //string pre = string.Empty;
            string pre = " /profile /user:" + "Administrator";//Interface.ICurrent.WindowsUser;

            string pre2 = pre;
            pre += " \"dism " + arg;// + "\"";
            pre2 += " \"dism " + arg2;// + "\"";

            // string pre2 = "runas /profile /env /user:" + Interface.ICurrent.WindowsUser;

            System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo("runas", pre);
            System.Diagnostics.ProcessStartInfo info2 = new ProcessStartInfo("runas", pre2);
            Process pro = new Process();
            pro.StartInfo = info;

            Process pro2 = new Process();
            pro2.StartInfo = info2;

             info.LoadUserProfile = true;
            info2.LoadUserProfile = true;
        // info.UseShellExecute = true; info2.UseShellExecute = true; info.RedirectStandardOutput =
        // true; info2.RedirectStandardOutput = true;

            pro.Start();
            // pro. pro.BeginOutputReadLine(); pro.BeginOutputReadLine();

            // string result = pro.StandardOutput.ReadLine(); info. pro.WaitForExit(30000);

            pro2.Start();
        }
        */

        /// <summary>
        /// Class constructor
        /// </summary>
        public Report()
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Report(ref Interface inter)
        {
            Interface = inter;

            // Pop msn;

            if (msn == null)
            {
                msn = new Pop(true);
                msn.LogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)
                    + "\\" + "Log." + DateTime.Today.DayOfYear + ".doc";
            }
        }
    }
}