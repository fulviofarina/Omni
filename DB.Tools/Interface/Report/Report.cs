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
    public partial class Report
    {

        private static string email = "k0x.help@gmail.com";
        private static string shouldRestart = "Do you want to restart the program now?";
        private static string startOrExit = "Restart or Exit?";

        private static string bePatient = "Please be patient...";
        private static string bugReportNotGen = "Bug Report not generated!";
        private static string bugReportOnWay = "Bug Report is being populated";
        private static string bugReportProblem = "Problems with Bug Report!";
        private static string cannotMSMQ = "Cannot initiate the Message Queue";
        private static string checkifMSMQ = "Check if MSMQ wether is installed";
        private static string nothingBuggy = "Nothing seems 'buggy' this time ;)";
        private static string problemsWithReport = "Problems while generating Report!";



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

        private static string notAsync(ref MessageQueue qMsg1, ref System.Messaging.Message w, string user)
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


        private static string sendTo(ref MessageQueue qMsg1, ref System.Messaging.Message w, string user, string sendto)
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
                // if (path.Contains(DB.Properties.Resources.Exceptions)) whatsSending = " - Bug
                // Report"; else if (path.Contains(DB.Properties.Settings.Default.SpectraFolder))
                // whatsSending = " - Scheduled Acquisition";
                array.Add(path);
            }

            string result = Rsx.Emailer.SendMessageWithAttachmentAsync(user, lbel, bodyMsg, array, ref qMsg1, sendto);
            return result;
        }
    }

    //PRIVATE
    public partial class Report
    {
        private MessageQueue bugQM = null;
        private IAsyncResult bugresult = null;
        private Interface Interface;

        private Pop msn = null;

        private NotifyIcon notify = null;

        private void Async(ref MessageQueue qMsg1, ref System.Messaging.Message w)
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
                        Interface.IMain.AddException(ex);
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

        //SHITTY
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pref"></param>
        /// <returns></returns>
        private string findHellos(ref string user, ref LINAA.PreferencesRow pref)
        {
            IEnumerable<LINAA.HelloWorldRow> hellos = pref.GetHelloWorldRows();

            int count = hellos.Count();
            if (Interface.IDB.HelloWorld.Rows.Count != count)
            {
                IEnumerable<LINAA.HelloWorldRow> toremove = Interface.IDB.HelloWorld.Except(hellos);
                Interface.IStore.Delete(ref toremove);
            }

            string comment = String.Empty;

            if (count != 0 && count < 20)
            {
                Random random = new Random();
                int index = random.Next(0, count);
                comment = hellos.ElementAt(index).Comment;
                // user = hellos.ElementAt(index).k0User;
            }

            return comment;
        }

        private void qMsg_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
            if (e == null) return;
            MessageQueue qMsg1 = sender as MessageQueue;
            string user = Interface.IPreferences.WindowsUser;
            try
            {
                System.Messaging.Message w = e.Message;
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
                Interface.IMain.AddException(ex);
            }
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
    }


 
 
}