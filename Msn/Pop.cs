﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Msn
{
    public partial class Pop : UserControl
    {
        public Pop()
        {

        }
       

        public void Msg(string msg, string title, bool ok)
        {
          //  string pre = string.Empty;
            System.Windows.Forms.ToolTipIcon icon = System.Windows.Forms.ToolTipIcon.Error;
            string pre = "OK";
            if (!ok)
            {
                pre = "=(";
               
                
            }
             this.iconic.Image = Notifier.MakeBitMap(pre, seg, Color.White);


            // else pre = "FAILED - ";
            //  pre += title;

            Msg(msg, title, icon);
        

         //   Application.DoEvents();
        }
        protected Font seg = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold);

        public void ReportProgress(int percentage)
        {
        
            this.iconic.Image = Notifier.MakeBitMap(percentage.ToString() + "%", seg, Color.White);
            //this.notify.Icon = Rsx.Notifier.MakeIcon(percentage.ToString(), seg, System.Drawing.Color.White);

            Application.DoEvents();

            string msg = "Loading... ";
            string title = "Please wait...";
            if (percentage == 100)
            {
                msg = "Loaded ";
                this.iconic.Image = Notifier.MakeBitMap("OK", seg, Color.White);
                title = "Ready to go...";
            }

            Msg(title, msg, ToolTipIcon.Info);

            if (percentage == 100) this.iconic.Image = Notifier.MakeBitMap(":)", seg, Color.White);

            Application.DoEvents();
        }

        public int DisplayInterval = 16000;
        protected System.Speech.Synthesis.SpeechSynthesizer lorito;
        //   protected SpeechLib.SpVoice lorito;

        protected string lastSpeak = string.Empty;
        public void Speak(string text)
        {
            if (lastSpeak.CompareTo(text) == 0) return;
            lastSpeak = text;
            if (lorito == null)
            {
                this.lorito = new System.Speech.Synthesis.SpeechSynthesizer();

                //    this.lorito = new SpeechLib.SpVoice();
            }
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
        //    f.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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

       //     f.StartPosition = FormStartPosition.CenterScreen;
          //  f.Opacity = 0;
       


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

            n = new Timer();

            n.Tick += n_Tick;
            n.Interval = DisplayInterval;

            this.textBoxDescription.TextChanged += textBoxDescription_TextChanged;

            if (withForm)
            {
                MakeForm();
            }
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                n.Stop();
                n.Start();
            }
            else this.Visible = true;
        }

        private Timer n;

        public void Msg(string msg, string title, System.Windows.Forms.ToolTipIcon icon)
        {
          

       //     if (InvokeRequired)
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

        private void n_Tick(object sender, EventArgs e)
        {
            n.Stop();
            if (this.Visible) this.Visible = false;
        }
    }
}