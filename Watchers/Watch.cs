using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.Tools;

//using DB.Interfaces;
using Rsx;

namespace k0X
{
    public interface IWatch
    {
        EventHandler DetectorsHandler { get; }
        EventHandler TalkHandler { get; }
    }

    public class Watch : IWatch
    {
        public EventHandler DetectorsHandler
        {
            get { return Detectors; }
        }

        public EventHandler TalkHandler
        {
            get { return Talk; }
        }

        private System.Windows.Forms.Timer GmailCheck;
        private System.Windows.Forms.Timer ScanCheck;
        private DateTime since = DateTime.Now;
        private DateTime sinceScan = DateTime.Now;
        private Gmail.AtomFeed gfeed;
        private k0X.IWatchersForm IWatchers;

        private Interface Interface;

        public Watch(ref Interface set)
        {
            Interface = set;

            gfeed = new Gmail.AtomFeed("k0x.help", "Helpme123");

            this.GmailCheck = new System.Windows.Forms.Timer();
            this.GmailCheck.Tick += this.GmailCheck_Tick;
            this.ScanCheck = new System.Windows.Forms.Timer();
            this.ScanCheck.Tick += this.GmailCheck_Tick;

            GetLastTime(ref since, false);
            GetLastTime(ref sinceScan, true);

            Detectors(null, EventArgs.Empty);

            GmailCheck.Interval = 120000;
            GmailCheck.Enabled = true;

            ScanCheck.Interval = 10000;
            ScanCheck.Enabled = true;
        }

        private void GetLastTime(ref DateTime sinc, bool scan)
        {
            if ((Interface.IPreferences.IsSpectraPathOk))
            {
                string file = Properties.CmdRes.DO;
                if (scan) file = Properties.CmdRes.SCAN;
                string path = Interface.IPreferences.CurrentPref.Spectra + file;
                if (System.IO.File.Exists(path))
                {
                    string text = System.IO.File.ReadAllText(path);
                    long lon = Convert.ToInt64(text);
                    sinc = DateTime.FromFileTime(lon);
                }
            }
            else sinc = DateTime.Now;
        }

        private void GmailCheck_Tick(object sender, EventArgs e)
        {
            string CMDprfix = k0X.Properties.CmdRes.DO;

            bool scan = false;
            DateTime toUse = since;
            if (sender.Equals(this.ScanCheck))
            {
                CMDprfix = Properties.CmdRes.SCAN;
                ScanCheck.Enabled = false;
                scan = true;
                toUse = sinceScan;
            }
            else this.GmailCheck.Enabled = false;

            Application.DoEvents();

            IList<object[]> lsCommands = null;
            try
            {
                lsCommands = Gmail.CheckCmds(ref toUse, ref gfeed, CMDprfix);
            }
            catch (SystemException ex)
            {
            }

            if (lsCommands != null)
            {
                foreach (object[] arr in lsCommands)
                {
                    try
                    {
                        string subject = (string)arr[0];
                        string email = (string)arr[1];
                        string summary = (string)arr[2];
                        DateTime received = (DateTime)arr[3];

                        if (scan) DetScan(ref email, ref summary, ref received);
                        else CheckAFeed(ref subject, ref email, ref summary, ref received);
                    }
                    catch (SystemException ex)
                    {
                        Interface.IMain.AddException(ex);
                    }
                }
            }

            if (!scan) this.GmailCheck.Enabled = true;
            else ScanCheck.Enabled = true;
        }

        private void CheckAFeed(ref string subject, ref string email, ref string summary, ref DateTime received)
        {
            if (subject.Contains(k0X.Properties.CmdRes.TALK))
            {
                string speech = subject.Replace(k0X.Properties.CmdRes.TALK, null).Trim();
                speech = speech.Substring(0, speech.LastIndexOf('-'));
                string from = summary.Substring(0, summary.LastIndexOf('@'));
                speech = received.ToLocalTime() + ":\t" + speech;
                object[] sin = new object[] { speech, from };
                this.Talk(sin, EventArgs.Empty);
            }
            else if (subject.Contains(k0X.Properties.CmdRes.GET))
            {
                string path = subject.Replace(k0X.Properties.CmdRes.GET, null).Trim();
                SendFile(email, path);
            }
            else if (subject.Contains(k0X.Properties.CmdRes.DIR))
            {
                string path = subject.Replace(k0X.Properties.CmdRes.DIR, null).Trim();
                path += "\\";
                SendDirList(email, path);
            }
            else if (subject.Length == 3)// Contains(Properties.CmdRes.ADD) || Contains(Properties.CmdRes.FIX))
            {
                if (Interface.IPreferences.IsSpectraPathOk) IWatchers.AddOrFixSchedule(subject, email, summary);
            }
            else if (!subject.Contains(k0X.Properties.CmdRes.REBOOT))
            {
                string[] array = subject.Split(' ');
                if (array.Length < 2) return;
                string cmd = array[0].Trim();
                string det = array[1].Trim();

                if (cmd.Length == 4 || cmd.Length == 5)
                {
                    if (Interface.IPreferences.IsSpectraPathOk) IWatchers.ExecuteCmd(email, cmd, det);
                }
            }

            SetLastTime(ref since, received, false);

            if (subject.Contains(k0X.Properties.CmdRes.REBOOT))
            {
                Reboot(email);
                return;
            }
        }

        private void DetScan(ref string email, ref string summary, ref DateTime received)
        {
            string[] auxiliar = summary.Split(' ');
            auxiliar = auxiliar[0].Split('.');

            string posSamDet = auxiliar[0].Trim().ToUpper();
            string possiblePos = auxiliar[1].Trim().ToUpper();

            string fullinfop = string.Empty;
            string schInfo = string.Empty;
            if (auxiliar.Count() >= 4)
            {
                // auxiliar[2] = auxiliar[2].ToUpper().Trim();
                // auxiliar[3] = auxiliar[3].Split(' ')[0].Trim().ToUpper();
                fullinfop = auxiliar[2] + "." + auxiliar[3];
                schInfo = auxiliar[4];
            }

            try
            {
                bool isDet = IWatchers.DetectorsList.Contains(posSamDet);

                if (isDet)
                {
                    possibleDet = posSamDet;
                    awaitingSample = true;
                    IWatchers.ExecuteCmd(email, Properties.Cmds.SETPOS + possiblePos, possibleDet);
                    if (!string.IsNullOrEmpty(fullinfop)) IWatchers.ExecuteCmd(email, Properties.Cmds.SETSAM + fullinfop, possibleDet);
                    if (!string.IsNullOrEmpty(schInfo)) IWatchers.ExecuteCmd(email, Properties.Cmds.SETSCH + schInfo, possibleDet);
                }
                else
                {
                    IWatchers.ExecuteCmd(email, Properties.Cmds.SETSAM + posSamDet, possibleDet);
                    awaitingSample = false;
                    possibleDet = string.Empty;
                }

                SetLastTime(ref sinceScan, received, true);
            }
            catch (SystemException ex)
            {
            }
        }

        private bool awaitingSample = false;
        private string possibleDet = string.Empty;

        private void SetLastTime(ref DateTime inter, DateTime received, bool scan)
        {
            bool set = false;
            if (inter <= received)
            {
                inter = received;
                inter = inter.AddMilliseconds(1000);
                set = true;
            }

            if (!set) return;

            if (Interface.IPreferences.IsSpectraPathOk)
            {
                string path = Interface.IPreferences.CurrentPref.Spectra;
                string text = inter.ToFileTime().ToString();
                string file = Properties.CmdRes.DO;
                if (scan) file = Properties.CmdRes.SCAN;
                System.IO.File.WriteAllText(path + file, text);
            }
        }

        private void SendDirList(string email, string path)
        {
            Func<string, string, string> d = (en, x) =>
            {
                en += x.ToString() + "\n";
                return en;
            };
            string msg = "Directories ";

            if (System.IO.Directory.Exists(path + "\\"))
            {
                string[] dirs = System.IO.Directory.GetDirectories(path);
                string[] files = System.IO.Directory.GetFiles(path);
                int count = dirs.Count();
                msg += "(" + count + " ) :\n\n";
                if (count != 0) msg += dirs.Aggregate(d);
                msg += "Files ";
                count = files.Count();
                msg += "(" + count + " ) :\n\n";
                if (count != 0) msg += files.Aggregate(d);
            }
            else msg = "Sorry but that directory does not exist! Check it and send the command again!";
            Interface.IReport.GenerateReport("DIR for " + path, string.Empty, msg, string.Empty, email);
        }

        private void SendFile(string email, string path)
        {
            string[] files = new string[1];
            if (System.IO.Directory.Exists(path + "\\"))
            {
                files = System.IO.Directory.GetFiles(path);
            }
            else if (System.IO.File.Exists(path))
            {
                files[0] = path;
            }

            Interface.IReport.GenerateReport("GET for " + path, files, string.Empty, string.Empty, email);
        }

        private void Talk(object sender, EventArgs e)
        {
            string msg = string.Empty;
            string fromTo = string.Empty;
            bool send = false;

            if (!sender.GetType().Equals(typeof(object[]))) send = true;
            else
            {
                object[] aux = sender as object[];
                msg = (string)aux[0];
                fromTo = (string)aux[1];
            }

            RichTextBox rtb = new RichTextBox();
            Form f = new Form();
            rtb.Dock = DockStyle.Fill;
            f.Controls.Add(rtb);

            if (send)
            {
                rtb.Text = "Please write a message without ENTERS and finish it with @Person. For example: Hi Fulvio, where are you? could you please come to see this? @Fulvio";

                f.Text = "Talk to somebody";
                rtb.SelectAll();
                f.ShowDialog();
                string[] arr = rtb.Text.Split('@');
                msg = arr[0];
                fromTo = arr[1].Trim();
                f.Dispose();
                rtb.Dispose();
                f = null;
                rtb = null;

                if (string.IsNullOrWhiteSpace(msg)) return;

                Interface.IReport.GenerateReport("DO TALK" + msg, string.Empty, fromTo + "@", "TALK", "ffarina@sckcen.be");
            }
            else
            {
                f.Show();
                f.Text = "Message from " + fromTo;
                rtb.Text = msg;
            }
        }

        private void Detectors(object sender, EventArgs e)
        {
            if (!Interface.IPreferences.IsSpectraPathOk)
            {
                Interface.IReport.Msg("Check the <DB Connections>", "Spectra directory not accesible");
            }
            else
            {
                if (IWatchers != null)
                {
                    IWatchers.Dispose();
                    IWatchers = null;
                }

                IWatchers = new k0X.WatchersForm(ref Interface);

                IWatchers.PopulateNodes(null); //uses the bundle detectors list...
                if (sender != null) IWatchers.Show();
            }
        }

        private static void Reboot(string email)
        {
            string exe = Application.ExecutablePath;
            System.IO.File.WriteAllText(Application.StartupPath + DB.Properties.Resources.Restarting, email);
            Rsx.Dumb.Process(new System.Diagnostics.Process(), Application.StartupPath, exe, string.Empty, false, false, 0);
            Application.Exit();
        }

        //   System.Diagnostics.Process notepad;
    }
}