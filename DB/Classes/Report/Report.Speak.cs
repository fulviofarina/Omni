using System;

namespace DB
{
    public partial class LINAA : DB.Interfaces.IReport
    {
        protected SpeechLib.SpVoice lorito;

        public void Speak(string text)
        {
            if (lorito == null) this.lorito = new SpeechLib.SpVoice();
            System.ComponentModel.BackgroundWorker worker3 = new System.ComponentModel.BackgroundWorker();
            worker3.DoWork += new System.ComponentModel.DoWorkEventHandler(speaker_DoWork);
            worker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(speaker_RunWorkerCompleted);
            worker3.RunWorkerAsync(text);
        }

        internal void speaker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker3 = sender as System.ComponentModel.BackgroundWorker;
            worker3.Dispose();
            worker3 = null;
        }

        internal void speaker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string text = e.Argument as string;
                this.lorito.Speak(text);
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}