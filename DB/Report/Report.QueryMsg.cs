using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using DB.Properties;
using Rsx;

namespace DB
{
    public partial class LINAA : IReport
    {
        public void GenerateReport(string labelReport, object path, string extra, string module, string email)
        {
            try
            {
                //send path as the body of the QMsg
                //put a extended body to the email please

                string queuePath = QM.QMAcquisitions + "." + module + "." + email;
                MessageQueue qm = Rsx.Emailer.CreateMQ(queuePath, this.qMsg_ReceiveCompleted);
                string title = labelReport + " - " + module;
                Message m = Rsx.Emailer.CreateQMsg(path, title, extra);

                Rsx.Emailer.SendQMsg(ref qm, ref m);
                qm.BeginReceive();
            }
            catch (SystemException ex)
            {
                this.Msg(ex.Message + "\n\n" + ex.StackTrace, problemsWithReport, false);
                this.AddException(ex);
            }
        }

        private static string problemsWithReport = "Problems while generating Report!";
        private static string checkifMSMQ = "Check if MSMQ wether is installed";
        private static string bugReportOnWay = "Bug Report is being populated";
        private static string cannotMSMQ = "Cannot initiate the Message Queue";
        private static string bePatient = "Please be patient...";
        private static string bugReportProblem = "Problems with Bug Report!";
        private static string nothingBuggy = "Nothing seems 'buggy' this time ;)";
        private static string bugReportNotGen = "Bug Report not generated!";

        public void GenerateBugReport()
        {
            try
            {
                //if sender is null means that the user did not requested this.
                //if sender is null its because this is automatic...

                // if the sender is null it is because I don't want the qU to listen for exceptions that must be emailed
                // this is done when the user Asks for a bug report...

                if (bugQM == null)
                {
                    bugQM = Emailer.CreateMQ(QM.QMExceptions, this.qMsg_ReceiveCompleted);
                }
                if (bugQM == null)
                {
                    this.Msg(checkifMSMQ, cannotMSMQ, false);
                    return;
                }

                // when sender is null...
                // I just want the qu to store them in the Server tray and the user will retrieve and email them later with Send-bugreport
                this.Msg(bePatient, bugReportOnWay);

                //write exceptions to a XML file is Exceptions is not empty...

                string path = this.SaveExceptions();
                path = folderPath + DB.Properties.Resources.Exceptions;
                IEnumerable<string> exceptions = System.IO.Directory.EnumerateFiles(path);

                int cnt = exceptions.Count();
                if (cnt != 0)
                {
                    foreach (string excFile in exceptions)
                    {
                        System.Messaging.Message w = Emailer.CreateQMsg(excFile, "Bug Report", "Should I add more comments?");
                        Emailer.SendQMsg(ref bugQM, ref w);
                    }
                    this.notify.ShowBalloonTip(7000, "Bug Reports on tray...", cnt + " scheduled to be sent", System.Windows.Forms.ToolTipIcon.Info);
                    exceptions = null;
                }
                else this.Msg(bugReportNotGen, nothingBuggy);

                bugresult = bugQM.BeginReceive();
            }
            catch (SystemException ex)
            {
                this.Msg(ex.Message + "\n\n" + ex.StackTrace, bugReportProblem, false);
                this.AddException(ex);
            }
        }
    }

    public partial class LINAA
    {
        private void qMsg_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
            if (e == null) return;
            MessageQueue qMsg1 = sender as MessageQueue;
            string user = this.WindowsUser;
            try
            {
                Message w = e.Message;
                //feedback came from emailing process
                //body of message is a FileStream (the attachment of the email)
                if (w.Label.Contains("AsyncEmail"))
                {
                    //was ok or not?

                    Async(ref qMsg1, ref w);
                }
                else
                {
                    string result = notAsync(ref qMsg1, ref w, user);

                    DateTime when = w.SentTime;

                    reportResult(result, when);
                }
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        private static string notAsync(ref MessageQueue qMsg1, ref Message w, string user)
        {
            //takes the Qmessage body (usually a path to a file)
            // retrieves the file and emails it!!
            //the w.Extension contains the message body of the mail
            //the w.label the subject of the mail
            //the w.Body is the filepath
            string sendto = string.Empty;
            string qPath = qMsg1.Path;
            if (qPath.Contains("@"))
            {
                string emailExt = qPath.Substring(qPath.LastIndexOf("."));
                sendto = qPath.Replace(emailExt, null);
                sendto = sendto.Substring(sendto.LastIndexOf(".") + 1);
                sendto += emailExt;
            }

            //Send
            string result = sendTo(ref qMsg1, ref w, user, sendto);

            return result;
            //read for more messages!!
            //int totalremaining = qMsg1.GetAllMessages().Length;
            //if (totalremaining > 0) qMsg1.BeginReceive();
        }

        private void reportResult(string result, DateTime when)
        {
            string whatsSending = "Status";
            string title = whatsSending + " NOT Sent!";
            bool sending = false;

            if (result.Contains("sending"))
            {
                title = "Sending " + whatsSending + "...";
                sending = true;
            }

            this.msn.Msg(result + "\n\nAt " + when.ToString(), title, sending);

            if (!sending)
            {
                throw new SystemException("Failure sending email - Async email failed");
            }
        }

        private static string sendTo(ref MessageQueue qMsg1, ref Message w, string user, string sendto)
        {
            Type tipo = w.Body.GetType();
            string bodyMsg = Rsx.Emailer.DecodeMessage(w.Extension);

            string lbel = w.Label;
            ArrayList array = new ArrayList();
            //assign label
            if (tipo.Equals(typeof(string[])))
            {
                string[] paths = w.Body as string[];
                array.AddRange(paths);
            }
            else
            {
                string path = w.Body as string;
                //  if (path.Contains(DB.Properties.Resources.Exceptions)) whatsSending = " - Bug Report";
                //  else if (path.Contains(DB.Properties.Settings.Default.SpectraFolder)) whatsSending = " - Scheduled Acquisition";
                array.Add(path);
            }

            string result = Rsx.Emailer.SendMessageWithAttachmentAsync(user, lbel, bodyMsg, array, ref qMsg1, sendto);
            return result;
        }

        private void Async(ref MessageQueue qMsg1, ref Message w)
        {
            bool sent = false;
            DateTime when = DateTime.Now;
            if (w.Label.Contains("OK"))
            {
                //  EmailTitle = " was Sent!";
                when = w.SentTime;
                sent = true;
            }

            string EmailTitle = string.Empty;
            if (!sent)
            {
                this.AddException((Exception)w.Body);
                EmailTitle = " was NOT sent";
            }
            else EmailTitle = " was sent";

            // postprocessing...
            if (QM.QMExceptions.Equals(qMsg1.Path))
            {
                EmailTitle = "Bug Report" + EmailTitle;
                if (sent && w.Body.GetType().Equals(typeof(string)))
                {
                    try
                    {
                        System.IO.File.Delete(w.Body as string);
                    }
                    catch (SystemException ex)
                    {
                        this.AddException(ex);
                    }
                }
            }
            else
            {
                //here I could do something with the scheduler...
                EmailTitle = "Generic Report" + EmailTitle;
            }

            this.msn.Msg("At " + when.ToString(), EmailTitle, sent);

            bugresult = bugQM.BeginReceive();
        }

        private MessageQueue bugQM = null;

        private IAsyncResult bugresult = null;
    }
}