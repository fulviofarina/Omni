using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using Rsx.Dumb;
using VTools;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSF : UserControl
    {
        private static Size currentSize;

        private bool cancelCalculations = false;
        private Interface Interface = null;
     //   private Action<int> resetProgress;
        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pro"></param>
        public void AttachCtrl<T>(ref T pro)
        {
            try
            {
                Control destiny = null;
                if (pro.GetType().Equals(typeof(ucProjectBox)))
                {
                    ucProjectBox projBox = pro as ucProjectBox;

                    projBox.HideChildControl = Hide;
                    destiny = this.splitContainer1.Panel1;
                    projBox.CallBack += delegate
                    {
                        refreshProject();
                    };
                }
                else if (pro.GetType().Equals(typeof(ucOptions)))
                {
                    IucOptions options = pro as IucOptions;
                    MatSSF.StartupPath = Interface.IStore.FolderPath + Resources.SSFFolder;
                    //SET A METHOD TO CALL BACK!!!!
                    EventHandler callBack = delegate
                    {
                        this.ucUnit.PaintRows();
                    };
                    MatSSF = new MatSSF();
                    MatSSF.Set(ref Interface, callBack, options.ResetProgress,options.ShowProgress);

                    // projBox.HideChildControl = Hide;
                    destiny = this.splitContainer1.Panel2;
                }
                else if (pro.GetType().Equals(typeof(ucUnit)))
                {
                    //link to bindings
                    this.CalcTab.Controls.Clear();
                    ucUnit.Dispose();
                    ucUnit = pro as ucUnit;
                    ucUnit.Dock = DockStyle.Fill;
                    destiny = this.CalcTab;
                    // ucUnit.Set(ref Interface);

                    ucMS.Set(ref Interface);
                    ucMS.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;

                    // OTHER CONTROLS
                    ucCC1.Set(ref Interface);
                    ucCC1.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;

                    ucVcc.Set(ref Interface);
                    ucVcc.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;

                }
                else
                {

                 
               
                    ucSSFControl.AttachCtrl(ref pro);
                    ucUnit.AttachCtrl(ref pro);
                 
                }

                destiny?.Controls.Add(pro as Control);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void refreshProject()
        {
            bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
            this.CalcBtn.Enabled = ThereIsData;
            this.CalcBtn.Visible = ThereIsData;
            this.cancelBtn.Visible = ThereIsData;
            ucSSFControl.Disabler(ThereIsData);
            ucUnit.PaintRows();
        
        }

     

        public void Calculate(object sender, EventArgs e)
        {
            Interface.IBS.EndEdit();
            // this.ValidateChildren();

            Cursor.Current = Cursors.WaitCursor;

          

            this.CalcBtn.Enabled = false;
            this.cancelBtn.Enabled = true;
            this.Visible = true;
            this.ParentForm.Visible = true;

            this.Tab.SelectedTab = this.CalcTab;

            MatSSF.Calculate();

            this.CalcBtn.Enabled = true;
     
            // Creator.SaveInFull(true);
            this.cancelBtn.Enabled = false;

            Cursor.Current = Cursors.Default;

        }



        /// <summary>
        /// A function to show hide this control and mimetize
        /// </summary>
        public new void Hide()
        {
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            DaCONTAINER.FixedPanel = FixedPanel.Panel1;
            bool hidden = DaCONTAINER.Panel2Collapsed;
            ParentForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            //show
            if (hidden)
            {
                SSFPlitter.Visible = true;
                this.Size = currentSize;
            }
            else
            {
                this.Size = new Size((int)(DaCONTAINER.Panel1.Width * 0.66), DaCONTAINER.Panel1.Height);
                SSFPlitter.Visible = false;
            }

            DaCONTAINER.Panel2Collapsed = !hidden;
        }

   

   

        protected MatSSF MatSSF;
        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {

                ucSSFControl.On = this.CalcTab;
                ucSSFControl.Off = this.MatrixTab;
                ucSSFControl.Set(ref Interface);

                this.templatesTabCtrl.Selected += delegate
                {
                    if (templatesTabCtrl.SelectedTab == MatrixTab)
                    {
                        ucSSFControl.ViewChanged(true,EventArgs.Empty);
                    }
                 else
                    { 
                        ucSSFControl.ViewChanged(false, EventArgs.Empty);
                    }
                };

                //set calculation options
                //   Interface.IBS.EndEdit();

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

     

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            cancelCalculations = true;
            // this.cancelBtn.Enabled = false;
        }

        public ucSSF()
        {
            InitializeComponent();

            currentSize = this.Size;
            this.cancelBtn.Enabled = false;
        }
    }
}