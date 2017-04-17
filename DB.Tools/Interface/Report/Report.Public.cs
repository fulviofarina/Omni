using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Windows.Forms;
using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{

    public partial class Report : IReport
    {
        public Pop Msn
        {
            get { return msn; }
            set { msn = value; }
        }

        public void AskToRestart()
        {
            MessageBoxIcon i = MessageBoxIcon.Information;
            //restart or not??
            DialogResult yesnot = MessageBox.Show(shouldRestart, startOrExit, MessageBoxButtons.YesNo, i);
            if (yesnot == DialogResult.Yes)
            {
                Application.Restart();
            }
            else System.Environment.Exit(-1);
        }

        public void ChatMe(ref LINAA.PreferencesRow p)
        {
            try
            {
                string windowsUser = Interface.IPreferences.WindowsUser;
                // LINAA.PreferencesRow p = this.currentPref;

                // string comment = findHellos(ref windowsUser, ref p);

                string[] txtTitle = findGreeting(ref windowsUser);

                Msg(txtTitle[0], txtTitle[1]);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
            }
        }

        public void GenerateBugReport()
        {
            try
            {
                MessageQueue qm = GetMessageQueue(QM.QMExceptions);

                if (qm == null) return;

                generateBugReport(ref qm);

                bugresult = qm.BeginReceive();
            }
            catch (Exception ex)
            {
                this.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IMain.AddException(ex);
            }
        }

        public void GenerateReport(string labelReport, object path, string extra, string module, string email)
        {
            try
            {
                //send path as the body of the QMsg
                //put a extended body to the email please

                string queuePath = QM.QMAcquisitions + "." + module + "." + email;
                MessageQueue qm = GetMessageQueue(queuePath);
                if (qm == null) return;

                string title = labelReport + " - " + module;

                System.Messaging.Message m = Rsx.Emailer.CreateQMsg(path, title, extra);

                Rsx.Emailer.SendQMsg(ref qm, ref m);

                qm.BeginReceive();
            }
            catch (SystemException ex)
            {
                this.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IMain.AddException(ex);
            }
        }

        public MessageQueue GetMessageQueue(string QUEUE_PATH)
        {
            MessageQueue qm = null;

            try
            {
                ReceiveCompletedEventHandler ReceivedHandler = qMsg_ReceiveCompleted;
                qm = Rsx.Emailer.CreateMQ(QUEUE_PATH, ReceivedHandler);

                if (QUEUE_PATH.Contains(QM.QMAcquisitions))
                {
                    if (qm == null)
                    {
                        throw new Exception(checkifMSMQ, new Exception(cannotMSMQ));
                    }
                }
                else if (QUEUE_PATH.Contains(QM.QMExceptions))
                {
                    if (qm == null)
                    {
                        throw new Exception(bugReportProblem, new Exception(bugReportNotGen));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IMain.AddException(ex);
            }
            return qm;
        }

        /// <summary>
        /// Notifies the given message and title with an Info icon
        /// </summary>
        /// <param name="msg">  Message to display</param>
        /// <param name="title">title of the message</param>
        public void Msg(string msg, string title, bool ok = true)
        {
            this.msn.Msg(msg, title, ok);
        }

        public void ReportFinished()
        {
            if (Interface.IDB.Exceptions.Count != 0)
            {
                Speak("Loading finished! However... some errors were found");
            }
            else Speak("Loading finished!");
        }

        public void ReportProgress(int percentage)
        {
            this.msn.ReportProgress(percentage);
        }

        public void Speak(string text)
        {
            this.msn.Speak(text);
        }

        public void UserInfo()
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

            GenerateReport(enviropath, path, string.Empty, usrInfo, email);
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

            this.Msg(bePatient, bugReportOnWay);

            string path = Interface.IMain.FolderPath + Resources.Exceptions;
            IEnumerable<string> exceptions = System.IO.Directory.EnumerateFiles(path);
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
                this.Msg(BUGS_ONTRAY, cnt + " scheduled to be sent", true);
            }
            else this.Msg(bugReportNotGen, nothingBuggy);

            exceptions = null;
        }

        public Report()
        {
        }
        public Report(ref Interface inter)
        {
            Interface = inter;
        }
    }
}