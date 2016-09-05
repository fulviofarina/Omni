using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DB
{
    public partial class LINAA : DB.Interfaces.IReport
    {
        public void ReportProgress(int percentage)
        {
            System.Drawing.Font seg = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold);

            this.msn.iconic.Image = Rsx.Notifier.MakeBitMap(percentage.ToString() + "%", seg, System.Drawing.Color.White);
            //this.notify.Icon = Rsx.Notifier.MakeIcon(percentage.ToString(), seg, System.Drawing.Color.White);

            Application.DoEvents();

            string msg = "Loading... ";
            string title = "Please wait...";
            if (percentage == 100)
            {
                msg = "Loaded ";
                this.msn.iconic.Image = Rsx.Notifier.MakeBitMap("OK", seg, System.Drawing.Color.White);
                title = "Ready to go...";
            }

            Msg(title, msg);

            if (percentage == 100) this.msn.iconic.Image = Rsx.Notifier.MakeBitMap(":)", seg, System.Drawing.Color.White);
        }

        public void ReportFinished()
        {
            try
            {
                if (Exceptions.Count != 0)
                {
                    Speak("Loading finished! However... some errors were found");
                }
                else Speak("Loading finished!");
                string comment = String.Empty;
                string user = this.currentPref.WindowsUser;

                IEnumerable<LINAA.HelloWorldRow> hellos = this.currentPref.GetHelloWorldRows();
                int count = hellos.Count();
                if (this.HelloWorld.Rows.Count != count)
                {
                    IEnumerable<LINAA.HelloWorldRow> toremove = this.HelloWorld.Except(hellos);
                    this.Delete(ref toremove);
                }
                if (count != 0 && count < 20)
                {
                    Random random = new Random();
                    int index = random.Next(0, count);
                    comment = hellos.ElementAt(index).Comment;
                    user = hellos.ElementAt(index).k0User;
                }
                this.notify.Icon = mainIcon;

                int hours = DateTime.Now.TimeOfDay.Hours;
                string text;
                if (hours < 12) text = "Good morning " + user;
                else if ((hours < 16) && (hours >= 12)) text = "Good afternoon... " + user;
                else text = "Good evening... " + user;

                text += "\nPlease type in the name of the project";
                string title = "Which project to look for?";

                Msg(text, title);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
        }

        public bool IsSpectraPathOk
        {
            get
            {
                if (string.IsNullOrEmpty(currentPref.Spectra)) return false;
                else return System.IO.Directory.Exists(currentPref.Spectra);
            }
        }

        private System.Drawing.Icon mainIcon = null;

        private System.Windows.Forms.NotifyIcon notify;

        public System.Windows.Forms.NotifyIcon Notify
        {
            get { return notify; }
            set
            {
                if (value == null) return;
                notify = value;

                if (notify.Icon == null) return;
                if (mainIcon == null) mainIcon = notify.Icon;
            }
        }

        private Msn msn;

        public Msn Msn
        {
            get { return msn; }
            set { msn = value; }
        }

        public void Msg(string msg, string title, bool ok)
        {
            string pre = string.Empty;
            System.Windows.Forms.ToolTipIcon icon = System.Windows.Forms.ToolTipIcon.Error;
            if (ok)
            {
                pre = "OK - ";
                icon = System.Windows.Forms.ToolTipIcon.Info;
            }
            else pre = "FAILED - ";
            pre += title;

            Msg(msg, pre, icon);
        }

        /// <summary>
        /// Notifies the given message and title with an Info icon
        /// </summary>
        /// <param name="msg">Message to display</param>
        /// <param name="title">title of the message</param>
        public void Msg(string msg, string title)
        {
            System.Windows.Forms.ToolTipIcon icon = System.Windows.Forms.ToolTipIcon.Info;
            Msg(msg, title, icon);
        }

        public void Msg(string msg, string title, System.Windows.Forms.ToolTipIcon icon)
        {
            //   this.notify.BalloonTipTitle = title;
            // this.notify.BalloonTipText = msg;
            //       this.notify.BalloonTipIcon = icon;

            //******** this.notify.ShowBalloonTip(5000, title, msg, icon);

            msn.Msg(msg, title);
            msn.Show();

            System.Windows.Forms.Application.DoEvents();
        }
    }
}