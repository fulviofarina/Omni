using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Windows.Forms;
using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>
    //STATIC
 

    //PRIVATE
 


 
    public partial class Report : IReport
    {

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

        public Pop Msn
        {
            get { return msn; }
            set { msn = value; }
        }

        // private System.Drawing.Icon mainIcon = null;
       

        public void ChatMe(ref LINAA.PreferencesRow p)
        {
            try
            {
                string windowsUser = Interface.IPreferences.WindowsUser;
                // LINAA.PreferencesRow p = this.currentPref;

            //    string comment = findHellos(ref windowsUser, ref p);

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
                //if sender is null means that the user did not requested this.
                //if sender is null its because this is automatic...

                // if the sender is null it is because I don't want the qU to listen for exceptions
                // that must be emailed this is done when the user Asks for a bug report...

                if (bugQM == null)
                {
                    bugQM = Emailer.CreateMQ(QM.QMExceptions, this.qMsg_ReceiveCompleted);
                }
                if (bugQM == null)
                {
                    this.Msg(checkifMSMQ, cannotMSMQ, false);
                    return;
                }

                // when sender is null... I just want the qu to store them in the Server tray and the
                // user will retrieve and email them later with Send-bugreport
                this.Msg(bePatient, bugReportOnWay);

                //write exceptions to a XML file is Exceptions is not empty...

                string path = Interface.IStore.SaveExceptions();
                path = Interface.IMain.FolderPath + DB.Properties.Resources.Exceptions;
                IEnumerable<string> exceptions = System.IO.Directory.EnumerateFiles(path);

                int cnt = exceptions.Count();
                if (cnt != 0)
                {
                    foreach (string excFile in exceptions)
                    {
                        System.Messaging.Message w = Emailer.CreateQMsg(excFile, "Bug Report", "Should I add more comments?");
                        Emailer.SendQMsg(ref bugQM, ref w);
                    }
                    this.msn.Msg("Bug Reports on tray...", cnt + " scheduled to be sent", System.Windows.Forms.ToolTipIcon.Info);
                    exceptions = null;
                }
                else this.Msg(bugReportNotGen, nothingBuggy);

                bugresult = bugQM.BeginReceive();
            }
            catch (SystemException ex)
            {
                this.Msg(ex.Message + "\n\n" + ex.StackTrace, bugReportProblem, false);
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
                MessageQueue qm = Rsx.Emailer.CreateMQ(queuePath, this.qMsg_ReceiveCompleted);
                string title = labelReport + " - " + module;
                System.Messaging.Message m = Rsx.Emailer.CreateQMsg(path, title, extra);

                Rsx.Emailer.SendQMsg(ref qm, ref m);
                qm.BeginReceive();
            }
            catch (SystemException ex)
            {
                this.Msg(ex.Message + "\n\n" + ex.StackTrace, problemsWithReport, false);
                Interface.IMain.AddException(ex);
            }
        }

        public void Msg(string msg, string title, bool ok)
        {
            this.msn.Msg(msg, title, ok);
        }

        /// <summary>
        /// Notifies the given message and title with an Info icon
        /// </summary>
        /// <param name="msg">  Message to display</param>
        /// <param name="title">title of the message</param>
        public void Msg(string msg, string title)
        {
            this.msn.Msg(msg, title, true);
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

        public Report(ref Interface inter)
        {
            Interface = inter;
        }
    }
}