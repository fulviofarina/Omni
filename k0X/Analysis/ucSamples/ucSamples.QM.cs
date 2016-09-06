using System;
using System.Messaging;
using System.Windows.Forms;

namespace k0X
{
    public partial class ucSamples
    {
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

        protected void t_Tick(object sender, EventArgs e)
        {
            Timer t = sender as Timer;
            t.Enabled = false;
            //timer for preferences
            Preferences(false);
            t.Dispose();
        }

        protected void SendQMsg(int obj, string calculus)
        {
            System.Messaging.Message w = null;
            w = Rsx.Emailer.CreateQMsg(obj, calculus, "Content");
            Rsx.Emailer.SendQMsg(ref MQ, ref w);
        }
    }
}