﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Windows.Forms;
using DB.Properties;
using Msn;
using Rsx.Dumb; using Rsx;

namespace DB.Tools
{
    public partial class Report : IReport
    {

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
        public void ReportToolTip(object sender, EventArgs ev)
        {
            //  bool isSameCol = (lastColInder == e.ColumnIndex);
            DataGridView dgv = sender as DataGridView;
            DataGridViewCellMouseEventArgs e = ev as DataGridViewCellMouseEventArgs;
            int index = e.ColumnIndex;
            DataGridViewHeaderCell cell = dgv.Columns[index].HeaderCell;
            string toolTip = cell.ToolTipText;
            if (string.IsNullOrEmpty(toolTip)) return;
            Msg(toolTip, "This column represents");
            Speak(toolTip);

        }
        public string RestartFile
        {
            get
            {
                return Application.StartupPath + Resources.DevFiles + Resources.Restarting;
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

        public Pop Msn
        {
            get { return msn; }
            set { msn = value; }
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
        /// Ask the user to restart the computer
        /// </summary>
        public void AskToRestart()
        {
            MessageBoxIcon i = MessageBoxIcon.Information;
            //restart or not??
            DialogResult yesnot = MessageBox.Show(SHOULD_RESTART, START_OR_EXIT, MessageBoxButtons.YesNo, i);
            if (yesnot == DialogResult.Yes)
            {
                Application.Restart();
            }
            else System.Environment.Exit(-1);
        }

        /// <summary>
        /// not used
        /// </summary>
        public void ChatMe(ref LINAA.PreferencesRow p)
        {
            try
            {
                string windowsUser = Interface.IPreferences.WindowsUser;
                // LINAA.PreferencesRow p = this.currentPref;

                // string comment = findHellos(ref windowsUser, ref p);

                string[] txtTitle = generateGreeting(ref windowsUser);

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
              //  return ok;
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
                Interface.IReport.Msg(ex2.InnerException.Message, ex2.Message, false);
                Interface.IStore.AddException(ex2);
            }

          
            return ok;
        }

        /// <summary>
        /// Generates a Bug Report
        /// </summary>
        public void GenerateBugReport()
        {
            MessageQueue qm = generateMessageQueue(QM.QMExceptions);
            if (qm == null) return;

            try
            {
                generateBugReport(ref qm);

                bugresult = qm.BeginReceive();

                GenerateUserInfoReport();
            }
            catch (Exception ex)
            {
                 this.Msg(ex.InnerException.Message, ex.Message, false);
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

        // [PrincipalPermission(SecurityAction.Demand, Authenticated =true, Role =
        // @"BUILTIN\Administrators", Unrestricted = false)]
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
                Interface.IReport.Msg(ex.InnerException.Message, ex.Message, false);
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
        /// Notifies the given message and title with an Info icon
        /// </summary>
        /// <param name="msg">  Message to display</param>
        /// <param name="title">title of the message</param>
        public void Msg(string msg, string title, bool ok = true)
        {
            this.msn.Msg(msg, title, ok);
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
              Msg("Some exceptions were found.\n I will send a report","Loading finished!");
            }
        }

        /// <summary>
        /// Method to report progress to pass to e.g. progress bars, etc
        /// </summary>
        public void ReportProgress(int percentage)
        {
            this.msn.ReportProgress(percentage);
        }

        public void Speak(string text)
        {
            this.msn.Speak(text);
        }

        /// <summary>
        /// Generates a UserInfo Report
        /// </summary>
        public void GenerateUserInfoReport()
        {
            Rsx.EDB en = new EDB();

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

        // private System.Drawing.Icon mainIcon = null;
        /// <summary>
        /// What this sends is a solicitude to generate an email with the exceptions The solicitude
        /// is processed, reported and when the solicitud arrives to the Queue then the email is sent
        /// Asynchroneusly... Then a feedback from the Async operations also arrives at the Queue and reports.
        /// </summary>
        private void generateBugReport(ref MessageQueue qm)
        {
            Interface.IStore.SaveExceptions();

            Msg(BE_PATIENT, BUG_REPORT_ONWAY);

            string path = Interface.IStore.FolderPath + Resources.Exceptions;
            IEnumerable<string> exceptions = Directory.EnumerateFiles(path);
            int cnt = exceptions.Count();
            if (cnt != 0)
            {
                foreach (string excFile in exceptions)
                {
                    System.Messaging.Message w = null;
                    string bodyOfBugEmail = "Should I add more comments?";
                    w = Emailer.CreateQMsg(excFile, "Bug Report", bodyOfBugEmail);
                    Emailer.SendQMsg(ref qm, ref w);
                    // System.IO.File.Delete(excFile);
                }
                Msg(BUGS_ONTRAY, cnt + " scheduled to be sent", true);
            }
            else Msg(BUG_REPORT_FAILED, BUG_REPORT_EMPTY);

            exceptions = null;
        }

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
                // LIMS.Interface.IReport.Msn = msn; msn.ParentForm.Opacity = 100;
            }
        }
    }
}