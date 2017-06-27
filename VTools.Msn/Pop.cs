using System;
using System.Drawing;
using System.Windows.Forms;

namespace VTools
{
    public partial class Pop : UserControl, IPop
    {
        protected internal int _DISPLAY_INTERVAL = 16000;

        protected internal string _LAST_MSG_SPEAK = string.Empty;
        protected internal string _LAST_MSG = string.Empty;

        protected internal System.Speech.Synthesis.SpeechSynthesizer _LORITO;

        protected internal Timer _TIMER;

        protected internal Font _FONT = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold);
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

            // f.StartPosition = FormStartPosition.CenterScreen; f.Opacity = 0;
        }

        public void Msg(string msg, string title, bool ok=true, bool accumulate = false)
        {
            EventHandler hdl = delegate
            {
                // string pre = string.Empty;
                System.Windows.Forms.ToolTipIcon icon = System.Windows.Forms.ToolTipIcon.Error;
                string pre = "OK";
                if (!ok)
                {
                    pre = "=(";
                }
                this.iconic.Image = Notifier.MakeBitMap(pre, _FONT, Color.White);

                message(msg, title,  accumulate);
            };
            this?.Invoke(hdl);
        }


        private void message( string msg, string title, bool accumulate = false)
        {
           
               
                    try
            {
                _LAST_MSG = this.textBoxDescription.Text;
                if (accumulate) this.textBoxDescription.Text = _LAST_MSG + "\n" + msg;
                else this.textBoxDescription.Text = msg;
                this.title.Text = title;

                FillLog(msg);


                //    Show();
                //  Application.DoEvents();


            }
            catch (Exception ex)
                    {
                         
                      
                    }
                 
              
        }

        public void FillLog(string msg)
        {
            System.IO.File.AppendAllText(logFilePath, DateTime.Now.ToLocalTime() + "\t" + msg + "\n");
        }

        public void ReportProgress(int percentage)
        {

        //    EventHandler hdl = delegate
            //{
                this.iconic.Image = Notifier.MakeBitMap(percentage.ToString() + "%", _FONT, Color.White);
                //this.notify.Icon = Rsx.Notifier.MakeIcon(percentage.ToString(), seg, System.Drawing.Color.White);

            //    Application.DoEvents();

                string msg = "Loading... ";
                string title = "Please wait...";
                if (percentage == 100)
                {
                    msg = "Loaded ";
                    this.iconic.Image = Notifier.MakeBitMap(":)", _FONT, Color.White);
                    title = "Ready to go...";
                }
                message( msg +  percentage + "%", title);

          //      Application.DoEvents();
                //   Application.DoEvents();
          //  };
         //   this.Invoke(hdl);
            
        }

        // protected SpeechLib.SpVoice lorito;
        public void Speak(string text)
        {
            if (_LAST_MSG_SPEAK.CompareTo(text) == 0) return;
            _LAST_MSG_SPEAK = text;
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
            {
                FillLog("**************************IDLE**************************");

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

            this.textBoxDescription.MouseDoubleClick += TextBoxDescription_MouseDoubleClick; ;

            if (withForm)
            {
                MakeForm();
            }
        }

        private void TextBoxDescription_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", logFilePath);
            }
            catch (Exception)
            {

               
            }
           
        }
    }
}