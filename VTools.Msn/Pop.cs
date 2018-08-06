using System;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace VTools
{
    public partial class Pop : UserControl, IPop
    {
        protected internal int _DISPLAY_INTERVAL = 4000;

        protected internal string _LAST_MSG_SPEAK = string.Empty;
        protected internal string _LAST_MSG = string.Empty;

        protected internal SpeechSynthesizer _LORITO;

        protected internal Timer _TIMER;

        protected internal Font _FONT = new Font("Verdana", 16, FontStyle.Italic);
        private string logFilePath = string.Empty;

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

        public string LastSpokenMessage
        {
            get
            {
                return _LAST_MSG_SPEAK;
            }
        }

        public string LastMessage
        {
            get
            {
                return _LAST_MSG;
            }
        }

        public string CurrentMessage
        {
            get
            {
                return this.textBoxDescription.Text;
            }
        }

        public string LogFilePath
        {
            get
            {
                return logFilePath;
            }

            set
            {
                logFilePath = value;

                FillLog("**************************STARTED**************************");
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
            f.Tag = this;

            // f.StartPosition = FormStartPosition.CenterScreen; f.Opacity = 0;
        }

        public void Msg(string msg, string title, object ok = null, bool accumulate = false)
        {
            EventHandler hdl = delegate
            {
                makeDefaultIcon(ok);
                messageMainFunction(msg, title, accumulate);
            };
            this?.Invoke(hdl);
        }

        private void makeDefaultIcon(object ok)
        {
            if (ok != null)
            {
                if (ok.GetType().Equals(typeof(bool)))
                {
                    string pre = "OK";
                    if (!(bool)ok)
                    {
                        pre = "=(";
                    }
                    makeDefinedIcon(pre, Color.White);
                }
                else if (ok.GetType().Equals(typeof(int)))
                {
                    makeProgressIcon((int)ok);
                }
            }
        }

        private void messageMainFunction(string msg, string title, bool accumulate = false)
        {
            try
            {
                _LAST_MSG = this.textBoxDescription.Text;
                if (accumulate) this.textBoxDescription.Text = _LAST_MSG + "\n" + msg;
                else this.textBoxDescription.Text = msg;
                this.title.Text = title;

                FillLog(msg);

                // Show(); Application.DoEvents();
            }
            catch (Exception ex)
            {
            }
        }

        public void FillLog(string msg)
        {
            System.IO.File.AppendAllText(logFilePath, DateTime.Now.ToLocalTime() + "\t" + msg + "\n");
        }

        private string reporAppender = string.Empty;

        public void ReportProgress(int percentage)
        {
            //    EventHandler hdl = delegate
            //{
            //    makeProgressIcon(percentage);
            //this.notify.Icon = Rsx.Notifier.MakeIcon(percentage.ToString(), seg, System.Drawing.Color.White);

            // Application.DoEvents();

            string msg = "Loading... ";
            string title = "Please wait...";
            if (percentage == 0) reporAppender = string.Empty;

            reporAppender += percentage.ToString() + "\n";

            if (percentage == 100)
            {
                msg = "Loading completed! ";
                // makeDefinedIcon(":)", Color.White);
                title = "Ready...";

                FillLog(reporAppender);
            }

            Msg(msg, title, percentage, false);

            // messageMainFunction(msg + percentage, title);

            // Application.DoEvents(); Application.DoEvents(); }; this.Invoke(hdl);
        }

        private void makeDefinedIcon(string content, Color color)
        {
            this.iconic.Image?.Dispose();

            Image okImg = Notifier.MakeBitMap(content, _FONT, color);
            this.iconic.Image = okImg;
        }

        private void makeProgressIcon(int percentage)
        {
            string progreso = percentage.ToString() + "%";
            Color color = Color.White;
            makeDefinedIcon(progreso, color);
        }

        // protected SpeechLib.SpVoice lorito;
        public void Speak(string text)
        {
            try
            {
                if (_LAST_MSG_SPEAK.CompareTo(text) == 0) return;
                _LAST_MSG_SPEAK = text;
                if (_LORITO == null)
                {
                    this._LORITO = new SpeechSynthesizer();
                    // this.lorito = new SpeechLib.SpVoice();
                }
                System.ComponentModel.BackgroundWorker worker3 = new System.ComponentModel.BackgroundWorker();
                worker3.DoWork += new System.ComponentModel.DoWorkEventHandler(speakDoWork);
                worker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(speakCompleted);
                worker3.RunWorkerAsync(text);
            }
            catch (Exception)
            {
            }
        }

        protected void timerTick(object sender, EventArgs e)
        {
            _TIMER.Stop();
            if (this.Visible)
            {
                this.Visible = false;
                if (this.ParentForm != null)
                {
                    if (this.ParentForm.Tag != null)
                    {
                        if (this.ParentForm.Tag.Equals(this))
                        {
                            this.ParentForm.Visible = false;
                        }
                    }
                }
                FillLog("************************** Went Idle (Hidden) **************************");
            }
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
            _TIMER.Stop();
            _TIMER.Start();

            if (this.Visible)
            {
            }
            else
            {
                this.Visible = true;

                if (this.ParentForm != null)
                {
                    this.ParentForm.Visible = true;
                }
            }
            this.Focus();
        }

        public Pop()
        {
        }

        public Pop(bool withForm)
        {
            InitializeComponent();

            BorderStyle = BorderStyle.Fixed3D;
            Location = new Point(3, 32);
            Name = "msn";
            Padding = new Padding(9);
            Size = new Size(512, 113);
            TabIndex = 6;

            this.Text = string.Empty;

            this.title.Text = string.Empty;
            this.textBoxDescription.Text = string.Empty;

            _TIMER = new Timer();

            _TIMER.Tick += timerTick;
            _TIMER.Interval = _DISPLAY_INTERVAL;

            this.textBoxDescription.TextChanged += msgChanged;

            this.pictureBox1.MouseDoubleClick += textBoxDescription_MouseDoubleClick;
            // this.textBoxDescription.MouseDoubleClick += textBoxDescription_MouseDoubleClick;

            if (withForm)
            {
                MakeForm();
            }
        }

        private void textBoxDescription_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string newFile = LogFilePath + ".copy.doc";
                System.IO.File.Copy(LogFilePath, newFile, true);
                System.Diagnostics.Process.Start("explorer.exe", newFile);
            }
            catch (Exception)
            {
            }
        }
    }
}