using System;
using System.Collections;
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
    public partial class Report
    {
        private static string bePatient = "Please be patient...";
        private static string bugReportNotGen = "Bug Report not generated!";
        private static string bugReportOnWay = "Bug Report is being populated";
        private static string bugReportProblem = "Problems with Bug Report!";
        private static string BUGS_ONTRAY = "Bug Reports on tray...";
        private static string cannotMSMQ = "Cannot initiate the Message Queue";
        private static string checkifMSMQ = "Checking if MSMQ is installed";
        private static string email = "k0x.help@gmail.com";
        private static string nothingBuggy = "Nothing seems 'buggy' this time ;)";
        private static string problemsWithReport = "Problems while generating Report!";
        private static string shouldRestart = "Do you want to restart the program now?";
        private static string startOrExit = "Restart or Exit?";
        private static string restartingOk = "Restarting succeeded...";
        //SHITTY
        ///SHITTY
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static string[] findGreeting(ref string user)
        {
            string text;
            string title;
            int hours = DateTime.Now.TimeOfDay.Hours;
            if (hours < 12) text = "Good morning " + user;
            else if ((hours < 16) && (hours >= 12)) text = "Good afternoon... " + user;
            else text = "Good evening... " + user;

            text += "\nPlease type in the name of the project";
            title = "Which project to look for?";

            return new string[] { title, text };
        }
    }

    //PRIVATE
    public partial class Report
    {
        // private MessageQueue bugQM = null;
        private IAsyncResult bugresult = null;

        private Interface Interface;

        private Pop msn = null;

   

        private static ArrayList getPathsToAttachments(ref System.Messaging.Message w)
        {
            ArrayList array = new ArrayList();
            Type tipo = w.Body.GetType();

            //assign label
            if (tipo.Equals(typeof(string[])))
            {
                string[] paths = w.Body as string[];
                array.AddRange(paths);
            }
            else
            {
                string path = w.Body as string;
                array.Add(path);
            }

            return array;
        }

        private static string sendEmail(ref MessageQueue qMsg1, ref System.Messaging.Message w, string user)
        {
            //takes the Qmessage body (usually a path to a file)
            // retrieves the file and emails it!!
            //the w.Extension contaings the message body of the mail
            //the w.label the subject of the mail
            //the w.Body is the filepath

            string qPath = qMsg1.Path;
            string lbel = w.Label;
            byte[] messageContent = w.Extension;
            ArrayList array = getPathsToAttachments(ref w);

            string sendto = string.Empty;
            if (qPath.Contains("@"))
            {
                string emailExt = qPath.Substring(qPath.LastIndexOf("."));
                sendto = qPath.Replace(emailExt, null);
                sendto = sendto.Substring(sendto.LastIndexOf(".") + 1);
                sendto += emailExt;
            }

            string bodyMsg = Rsx.Emailer.DecodeMessage(messageContent);
            string result = Rsx.Emailer.SendMessageWithAttachmentAsync(user, lbel, bodyMsg, array, ref qMsg1, sendto);
            return result;
        }

        private void qMsg_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            if (e == null) return;
            //if sender is null means that the user did not requested this.
            //if sender is null its because this is automatic...

            // if the sender is null it is because I don't want the qU to listen for exceptions that
            // must be emailed this is done when the user Asks for a bug report...

            // when sender is null... I just want the qu to store them in the Server tray and the
            // user will retrieve and email them later with Send-bugreport

            //write exceptions to a XML file is Exceptions is not empty...

            try
            {
                MessageQueue queue = sender as MessageQueue;
                string user = Environment.UserName;

                System.Messaging.Message AMessage = e.Message;

                //TWO TYPES OF SENDING, ASYNC AND SECUENTIAL
                bool isAsync = AMessage.Label.Contains("AsyncEmail");
                DateTime when = AMessage.SentTime;

                //feedback came from emailing process
                //body of message is a FileStream (the attachment of the email)
                if (isAsync)
                {
                    //was ok or not?
                    reportReceivedAsync(ref queue, ref AMessage);
                    //REPORTS THAT THE EMAIL WAS SENT OR NOT SENT
                }
                else
                {
                    string result = sendEmail(ref queue, ref AMessage, user);
                    //    AMessage.Label = "OK";
                    //reportReceivedAsync(ref queue, ref AMessage);
                    //REPORT TO SEND NOW...
                    reportResult(result, when);
                }
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
            }
        }

        private void reportReceivedAsync(ref MessageQueue qMsg1, ref System.Messaging.Message w)
        {
            bool sent = false;
            DateTime when = DateTime.Now;
            if (w.Label.Contains("OK"))
            {
                // EmailTitle = " was Sent!";
                when = w.SentTime;
                sent = true;
            }

            string EmailTitle = string.Empty;
            if (!sent)
            {
                Interface.IMain.AddException((Exception)w.Body);
                EmailTitle = " was NOT sent";
            }
            else EmailTitle = " was sent";

            bool isABugReport = w.Body.GetType().Equals(typeof(string));

            string pathToDelete = string.Empty;
            if (isABugReport) pathToDelete = w.Body as string;

            isABugReport = isABugReport && QM.QMExceptions.Equals(qMsg1.Path);

            // postprocessing...
            if (isABugReport)
            {
                EmailTitle = "Bug Report" + EmailTitle;
            }
            else
            {
                //here I could do something with the scheduler...
                EmailTitle = "Generic Report" + EmailTitle;
            }

            if (sent && isABugReport)
            {
                try
                {
                    if (System.IO.File.Exists(pathToDelete))
                    {
                        System.IO.File.Delete(pathToDelete);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IMain.AddException(ex);
                }
            }

            reportResult(EmailTitle, when);

            bugresult = qMsg1.BeginReceive();
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

            this.Msg(result + "\n\nAt " + when.ToString(), title, sending);

            if (!sending)
            {
                throw new SystemException("Failure sending email - Async email failed");
            }
        }
    }
}