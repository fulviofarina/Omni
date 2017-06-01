using System;
using System.Drawing;
using System.Windows.Forms;

namespace Msn
{
    public partial class Pop : UserControl, IPop
    {
        protected internal int _DISPLAY_INTERVAL = 16000;

        protected internal string _LAST_MSG_SPEAK = string.Empty;

        protected internal System.Speech.Synthesis.SpeechSynthesizer _LORITO;

        protected internal Timer _TIMER;

        protected internal Font _FONT = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold);

        public int DisplayInterval
        {
            get
            {
                return _DISPLAY_INTERVAL;
            }

            set
            {
                _DISPLAY_INTERVAL = value;
            }
        }

       public string BufferMessage
        {
            get
            {
                return _LAST_MSG_SPEAK;
            }

            set
            {
                _LAST_MSG_SPEAK = value;
            }
        }

        public void MakeForm()
        {
            Form f = this.ParentForm;
            if (f == null)
            {
                f = new Form();
            }
            else
            {
                f.Visible = false;
                f.Controls.Remove(this);
            }
            f.Opacity = 0;
            f.AutoSize = true;
            // f.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            f.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Opacity = 0;
            f.ControlBox = false;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            f.ShowIcon = false;
            f.ShowInTaskbar = false;
            f.TopMost = false;
            f.Controls.Add(this);
            f.Opacity = 100;
            f.Visible = true;

            // f.StartPosition = FormStartPosition.CenterScreen; f.Opacity = 0;
        }

        public void Msg(string msg, string title, bool ok)
        {
            // string pre = string.Empty;
            System.Windows.Forms.ToolTipIcon icon = System.Windows.Forms.ToolTipIcon.Error;
            string pre = "OK";
            if (!ok)
            {
                pre = "=(";
            }
            this.iconic.Image = Notifier.MakeBitMap(pre, _FONT, Color.White);

            // else pre = "FAILED - "; pre += title;

            Msg(msg, title, icon);

            // Application.DoEvents();
        }

        public void Msg(string msg, string title, System.Windows.Forms.ToolTipIcon icon)
        {
            // if (InvokeRequired)
            {
                Action dele = delegate
                {
                    this.textBoxDescription.Text = msg;
                    this.title.Text = title;
                    Show();
                    Application.DoEvents();
                };
                Invoke(dele);
            }
            //catch (Exception)
            //{
            //}
        }

        public void ReportProgress(int percentage)
        {
            this.iconic.Image = Notifier.MakeBitMap(percentage.ToString() + "%", _FONT, Color.White);
            //this.notify.Icon = Rsx.Notifier.MakeIcon(percentage.ToString(), seg, System.Drawing.Color.White);

            Application.DoEvents();

            string msg = "Loading... ";
            string title = "Please wait...";
            if (percentage == 100)
            {
                msg = "Loaded ";
                this.iconic.Image = Notifier.MakeBitMap("OK", _FONT, Color.White);
                title = "Ready to go...";
            }

            Msg(title, msg, ToolTipIcon.Info);

            if (percentage == 100) this.iconic.Image = Notifier.MakeBitMap(":)", _FONT, Color.White);

            Application.DoEvents();
        }

        // protected SpeechLib.SpVoice lorito;
        public void Speak(string text)
        {
            if (BufferMessage.CompareTo(text) == 0) return;
            BufferMessage = text;
            if (_LORITO == null)
            {
                this._LORITO = new System.Speech.Synthesis.SpeechSynthesizer();

                // this.lorito = new SpeechLib.SpVoice();
            }
            System.ComponentModel.BackgroundWorker worker3 = new System.ComponentModel.BackgroundWorker();
            worker3.DoWork += new System.ComponentModel.DoWorkEventHandler(speakDoWork);
            worker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(speakCompleted);
            worker3.RunWorkerAsync(text);
        }

        protected void timerTick(object sender, EventArgs e)
        {
            _TIMER.Stop();
            if (this.Visible) this.Visible = false;
        }

        protected void speakDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string text = e.Argument as string;
                this._LORITO.Speak(text);
            }
            catch (SystemException ex)
            {
            }
        }

        protected void speakCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker3 = sender as System.ComponentModel.BackgroundWorker;
            worker3.Dispose();
            worker3 = null;
        }

        protected void msgChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                _TIMER.Stop();
                _TIMER.Start();
            }
            else this.Visible = true;
        }

        public Pop()
        {
        }

        public Pop(bool withForm)
        {
            InitializeComponent();

            BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Location = new System.Drawing.Point(3, 32);
            Name = "msn";
            Padding = new System.Windows.Forms.Padding(9);
            Size = new System.Drawing.Size(512, 113);
            TabIndex = 6;

            this.Text = string.Empty;

            this.title.Text = string.Empty;
            this.textBoxDescription.Text = string.Empty;

            _TIMER = new Timer();

            _TIMER.Tick += timerTick;
            _TIMER.Interval = _DISPLAY_INTERVAL;

            this.textBoxDescription.TextChanged += msgChanged;

            if (withForm)
            {
                MakeForm();
            }
        }
    }
}