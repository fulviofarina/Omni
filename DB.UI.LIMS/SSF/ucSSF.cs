using System;
using System.Collections.Generic;
using System.ComponentModel;
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

   //     private bool cancelCalculations = false;
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
                Type t = pro.GetType();
                if (t.Equals(typeof(ucProjectBox)))
                {
                    ucProjectBox projBox = pro as ucProjectBox;
                    projBox.HideChildControl = Hide;
                    destiny = this.splitContainer1.Panel1;
                    Interface.IBS.PropertyChanged += IBS_PropertyChanged;
                
                }
                else if (t.Equals(typeof(ucOptions)))
                {
                    IucOptions options = pro as IucOptions;
                  
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
                else
                {
                    //main child
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

        private void IBS_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
            bool isCalculating = Interface.IBS.IsCalculating;
            this.CalcBtn.Visible = ThereIsData && !isCalculating;
            this.cancelBtn.Visible = ThereIsData && isCalculating;
            this.Tab.SelectedTab = this.CalcTab;
            ucUnit.PaintRows();
          
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

   

   

        protected MatSSF MatSSF=null;
        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {
                currentSize = this.Size;
          //      this.cancelBtn.Enabled = false;
                this.CalcBtn.Click += delegate
                    {
                        this.ValidateChildren();
                        Cursor.Current = Cursors.WaitCursor;
                        MatSSF.Calculate();
                        Cursor.Current = Cursors.Default;
                    };
                this.cancelBtn.Click += delegate
                    {
                        MatSSF.IsCalculating = false;
                    };

                //
                ucUnit.Set(ref Interface);
                ucUnit.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                //
                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                // OTHER CONTROLS
                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                //
            //EN ESTE ORDEN!!!!
                ucSSFControl.On = this.CalcTab;
                ucSSFControl.Off = this.MatrixTab;
                ucSSFControl.Set(ref Interface);
                //

                this.templatesTabCtrl.Selected += delegate
                {
                    bool matrix = false;
                    if (templatesTabCtrl.SelectedTab == MatrixTab)
                    {
                        matrix = true;
                    }
                    ucSSFControl.ViewChanged(matrix, EventArgs.Empty);
                };

                Interface.IReport.Msg("SSF Control OK", "Controls were set!");
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg("SSF Control NOT OK", "Severe error");
                Interface.IStore.AddException(ex);
            }
        }


        public ucSSF()
        {
            InitializeComponent();
       
        }
    }
}