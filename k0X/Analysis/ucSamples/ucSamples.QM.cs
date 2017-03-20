using System;
using System.Messaging;
using System.Windows.Forms;
using DB.Properties;

namespace k0X
{
    public partial class ucSamples
    {
        /// <summary>
        /// Main method called by everyone I guess
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Import_Click(object sender, EventArgs e)
        {
            //When Importing --> MatSSF, Load Peaks (with re-transfer), Solang and recalculate (NAA)
            //When MatSSF ===> only MatSFF of coourse
            //When CalculateSolang =>  Load Peaks (without re-transfer unless not found), Solang and recalculate (NAA)...
            //When Recalculate --> Load Peaks (without re-transfer unless not found), recalculate (NAA)

            string toDo = "Run";

            if (sender.Equals(this.Delete))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete all calculated data available for the samples or measurements selected?\n\n" +
                "This will NOT affect any sample data and its available measurements. Recalculation can be done once more at any time.\nHowever current self-shielding results, " +
                "calculated concentrations / FCs and gamma-lines selection/rejection information will be lost.\n\nContinue?", "Delete Analysis...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;

                toDo = "Delete";
            }

            if (MQ == null)
            {
                MQ = Rsx.Emailer.CreateMQ(QM.QMWorks + "." + pathCode, null);
            }
            if (MQ == null)
            {
                Interface.IReport.Msg("Check if MSMQ wether is installed", "Cannot initiate the Message Queue", false);
                return;
            }
            else MQ.Purge();

            if (timerQM == null)
            {
                timerQM = new Timer(this.components);
                timerQM.Interval = 200;
                timerQM.Tick += timerQM_Tick;
            }
            timerQM.Tag = null;
            timerQM.Enabled = true;

            ButtonVisible(false);

            int obj = Index(sender);
            SendQMsg(obj, toDo);
        }

        protected Timer timerQM;

        protected MessageQueue MQ;

        protected void timerQM_Tick(object sender, EventArgs e)
        {
            Timer timer = sender as Timer;
            timer.Enabled = false;
            //calculating...
            if (Canceled()) return;

            System.Messaging.Message[] arr = MQ.GetAllMessages();
            if (arr.Length == 0)
            {
                timer.Enabled = true;
                return;
            }
            if (progress.Value != progress.Maximum)
            {
                timer.Enabled = true;
                return;
            }

            System.Messaging.Message w = MQ.Receive();

            string label = w.Label;
            byte[] content = w.Extension;
            int obj = (int)w.Body;

            object tag = this.ProjectMenu.DropDownItems[obj];

            ContinueWork(obj, ref tag, label);

            timer.Enabled = true;
        }

        protected void SendQMsg(int obj, string calculus)
        {
            System.Messaging.Message w = null;
            w = Rsx.Emailer.CreateQMsg(obj, calculus, "Content");
            Rsx.Emailer.SendQMsg(ref MQ, ref w);
        }
    }
}