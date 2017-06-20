﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;


namespace VTools
{

    public interface IOptions
    {
        //  Action AboutBoxAction { set; }
        event EventHandler DatabaseClick;
        event EventHandler HelpClick;

        //     void SetDeveloperMode(bool devMode);
        event EventHandler ExplorerClick;
        event EventHandler PreferencesClick;
        event EventHandler AboutBoxClick;
        event EventHandler ConnectionBox;
        event EventHandler SaveClick;
        void ResetProgress(int max);
        void Set();
        EventHandler ShowProgress { get; }
        bool DisableImportant { set; }

        event EventHandler RestoreFoldersClick;

        event EventHandler DropDownClicked;// { get; set; }
    }
    public partial class ucOptions : UserControl, IOptions
    {
        public ucOptions()
        {
            InitializeComponent();
        }

        public event EventHandler RestoreFoldersClick
        {
            add
            {
                this.folderRestoreTSMI.Click += value;
            }
            remove
            {
                this.folderRestoreTSMI.Click -= value;
            }
        }
        public void Set()
        {
            //basic
            //then others attached

            this.Save.Click += delegate
            {
                this.ParentForm.Validate();
            };
        }

        public event EventHandler DropDownClicked
        {
            add
            {
                this.OptionsBtn.DropDownOpened += value;
            }

            remove
            {
                this.OptionsBtn.DropDownOpened -= value;
            }
        }

        public event EventHandler PreferencesClick
        {
            add
            {
                this.preferencesTSMI.Click += value;
            }

            remove
            {
                this.preferencesTSMI.Click -= value;
            }
        }

        public event EventHandler DatabaseClick
        {

            add
            {
                limsTSMI.Click += value;
            }
            remove
            {
                limsTSMI.Click -= value;
           
            }
        }

   
        public event EventHandler AboutBoxClick
        {
            add
            {
                aboutTSMI.Click += value;
            }

            remove
            {
                aboutTSMI.Click -= value;
            }
        }
   
        public event EventHandler SaveClick
        {
            add
            {

                this.Save.Click += value;

            }

            remove
            {
           
                this.Save.Click -= value;
            
            }
        }

     
        public event EventHandler ConnectionBox
        {

            add
            {
                connectionsTSMI.Click += value;
            }
            remove
            {
                connectionsTSMI.Click -= value;
            }
        }
      
        public event EventHandler ExplorerClick
        {


            add
            {
                explorerToolStripMenuItem.Click += value;
            }
            remove
            {
                explorerToolStripMenuItem.Click -= value;
            }
        }
     
        public  EventHandler ShowProgress
        {


            get
            {
                EventHandler pros = delegate
                {
                    Application.DoEvents();
                    this.progressBar.PerformStep();
                    Application.DoEvents();
                };

                return pros;
               
            }
          
        }

        public bool DisableImportant
        {
         

            set
            {
                this.explorerToolStripMenuItem.Enabled = value;
                this.limsTSMI.Enabled = value;
            }
        }

        public  event EventHandler HelpClick
        {
            add
            {
                helpToolStripMenuItem2.Click += value;
                helpToolStripMenuItem.Click += value;
            }
            remove
            {
                helpToolStripMenuItem2.Click -= value;
                helpToolStripMenuItem.Click -= value;
            }
        }

      

        public void ResetProgress (int max)
        {
            this.progressBar.Minimum = 0;
            this.progressBar.Step = 1;
            if (max == 0)
            {

                this.progressBar.Maximum = 0;
                this.progressBar.Value = 0;


            }
            else this.progressBar.Maximum += max;
        }

     

      
       
    }
}