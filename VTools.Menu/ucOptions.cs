using System;
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
    public partial class ucOptions : UserControl, IucOptions
    {
        public ucOptions()
        {
            InitializeComponent();
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
                aboutToolStripMenuItem.Click += value;
            }

            remove
            {
                aboutToolStripMenuItem.Click -= value;
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
            this.progressBar.Maximum = max;
            this.progressBar.Value = 0;

        }

     

      
       
    }
}