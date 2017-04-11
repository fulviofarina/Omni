using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DB.UI;
using DB.Tools;

namespace k0X
{
    public partial class MainForm
    {
        private decimal memory;

        private string bufferedMsg;

        private void OFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Box.Text = OFD.SafeFileName.ToUpper().Replace(".XML", null);
            Box_KeyUp(sender, new KeyEventArgs(Keys.Enter));
        }

        private void SFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LIMS.SaveWorkspaceXML(SFD.FileName);
            Cursor.Current = Cursors.Default;
        }

        private void SaveWorkspace_Click(object sender, EventArgs e)
        {
            SFD.ShowDialog();
        }

        private void LoadWorkspace_Click(object sender, EventArgs e)
        {
            OFD.ShowDialog();
        }

        private void mimetic_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = !this.mimetic.Checked;
        }

        private void autoload_CheckedChanged(object sender, EventArgs e)
        {
            bool check = (sender as ToolStripMenuItem).Checked;
            if (sender.Equals(this.autoload))
            {
                LIMS.Interface.IPreferences.CurrentPref.AutoLoad = check;
            }
            else if (sender.Equals(this.fillbyHL))
            {
                LIMS.Interface.IPreferences.CurrentPref.FillByHL = check;
            }
            else if (sender.Equals(this.fillBySpectra))
            {
                LIMS.Interface.IPreferences.CurrentPref.FillBySpectra = check;
            }
            LIMS.Interface.IPreferences.SavePreferences();
        }

        private void Help_Click(object sender, EventArgs e)
        {
            LIMS.Interface.IPopulate.Help();
        }

        private void releaseMemory_Click(object sender, EventArgs e)
        {
            ReleaseMemory(sender);
        }

        protected internal void Connections_Click(object sender, EventArgs e)
        {
            object prefe = LIMS.Interface.IPreferences.CurrentPref;
            if (prefe == null)
            {
                LIMS.Linaa.Msg("Preferences object is null!", "Cannot load preferences!", false);
                return;
            }
            Connections cform = new Connections(ref prefe);
            cform.ShowDialog();
            DialogResult res = MessageBox.Show("Save changes?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == System.Windows.Forms.DialogResult.No)
            {
                LIMS.Linaa.Preferences.RejectChanges();
                return;
            }

            LIMS.Interface.IPreferences.SavePreferences();

            this.Quit_Click(this.ClearLinaa, e); //leave like this, the sender must be ClearLinaa.
        }

        protected internal void EmailerMenu_Click(object sender, EventArgs e)
        {
            string sendFrom = "k0x.help@gmail.com";
            string wUser = LIMS.Interface.IPreferences.CurrentPref.WindowsUser;
            if (wUser.Contains("\\"))
            {
                int ind = wUser.IndexOf('\\');
                sendFrom = wUser.Remove(0, ind + 1).ToLower() + "@sckcen.be";
            }
            VTools.Emailer emailer = new VTools.Emailer(sendFrom);
            emailer.Show();
        }

        protected internal void About_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            AboutBox About = new AboutBox();
            About.Show();
            Cursor.Current = Cursors.Default;
        }

        protected internal void Clone_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MainForm Clone = new MainForm();
            Clone.Show();
            Cursor.Current = Cursors.Default;
        }

        private void notify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) return;

            this.Activate();
            this.Box.SelectAll();
            this.Box.Focus();
        }

        private void buffered_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.bufferedMsg))
            {
                LIMS.Linaa.Msg(this.bufferedMsg, "Buffered Controls!");
            }
            else
            {
                ReleaseMemory(sender);
                buffered_Click(sender, e);
            }
        }

        private void BugReportMenu_Click(object sender, EventArgs e)
        {
            LIMS.Linaa.GenerateBugReport();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            FormClosingEventArgs closeArgs;
            closeArgs = new FormClosingEventArgs(CloseReason.UserClosing, false);

            this.MainForm_FormClosing(sender, closeArgs);
        }

        private void ReleaseMemory(object sender)
        {
            String listed = String.Empty;
            String disposed = String.Empty;
            String reimaining = String.Empty;

            int disposedNr = 0;

            IList<object> arr = LIMS.UserControls.ToList();

            for (int i = arr.Count - 1; i >= 0; i--)
            {
                UserControl any = (UserControl)arr.ElementAt(i);
                if (any != null)
                {
                    string generation = GC.GetGeneration(any).ToString();
                    listed = listed + any.Name + " (" + generation + ")\n";
                    if (any.ParentForm == null || any.IsDisposed || sender.Equals(this) || sender.Equals(this.ClearLinaa))
                    {
                        disposed = disposed + any.Name + " (" + generation + ")\n";
                        if (!any.IsDisposed)
                        {
                            any.Dispose();
                        }
                        if (any.IsDisposed)
                        {
                            disposedNr++;
                            any = null;
                            arr.RemoveAt(i);
                        }
                    }
                    else reimaining = reimaining + any.Name + " (" + generation + ")\n";
                }
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            if (listed.Equals(String.Empty)) listed = "None!\n";
            if (disposed.Equals(String.Empty)) disposed = "None!\n";
            if (reimaining.Equals(String.Empty)) reimaining = "None!\n";

            decimal previous = memory;
            memory = Decimal.Round(Convert.ToDecimal(System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 * 1e-6), 1);
            decimal percentage = 0;
            if (previous != 0)
            {
                percentage = (memory - previous) * 100 / previous;
            }
            percentage = decimal.Round(percentage, 1);

            this.notify.BalloonTipTitle = "I'm releasing some memory for you!";
            this.notify.BalloonTipText = "\nControls:\tDiposed = " + disposedNr + "\tRemaining = " + arr.Count + "\n\nMemory (MB):\tNow = " + memory + "\tLast known = " + previous + "\n\nDifference (%):\t" + percentage;
            this.notify.ShowBalloonTip(5000);

            bufferedMsg = "\nListed:\n" + listed + "\n\nDisposed:\n" + disposed + "\n\nRemaining:\n" + reimaining;

            LIMS.UserControls = null;
            LIMS.UserControls = arr.ToList();
            arr = null;
        }
    }
}