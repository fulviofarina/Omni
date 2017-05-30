using System;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using VTools;

namespace DB.UI
{
    public partial class ucSSF : UserControl
    {
        protected MatSSF MatSSF = null;
        private static Size currentSize;

        // private bool cancelCalculations = false;
        private Interface Interface = null;

        // private Action<int> resetProgress;
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
                    destiny = attachProjectbox(pro);
                }
                else if (t.Equals(typeof(ucOptions)))
                {
                    destiny = attachOptions(pro);
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

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {
                currentSize = this.Size;
                setButtons();

                setTemplateControls();
                //
                setCalculator();

                Interface.IReport.Msg("SSF Control OK", "Controls were set!");
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg("SSF Control NOT OK", "Severe error");
                Interface.IStore.AddException(ex);
            }
        }

        private Control attachOptions<T>(T pro)
        {
            Control destiny;
            IucOptions options = pro as IucOptions;

            //SET A METHOD TO CALL BACK!!!!
            EventHandler callBack = delegate
            {
                this.ucUnit.PaintRows();
            };
            MatSSF = new MatSSF();
            MatSSF.Set(ref Interface, callBack, options.ResetProgress, options.ShowProgress);

            // projBox.HideChildControl = Hide;
            destiny = this.splitContainer1.Panel2;
            return destiny;
        }

        private Control attachProjectbox<T>(T pro)
        {
            Control destiny;
            ucProjectBox projBox = pro as ucProjectBox;
            projBox.HideChildControl = Hide;
            destiny = this.splitContainer1.Panel1;
            //attach binding
            Interface.IBS.PropertyChangedHandler += delegate
            {
                bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
                bool isCalculating = Interface.IBS.IsCalculating;
                this.CalcBtn.Enabled = ThereIsData && !isCalculating;
                this.cancelBtn.Enabled = ThereIsData && isCalculating;
                // this.Tab.SelectedTab = this.CalcTab;
                ucUnit.PaintRows();
            };
            // force refresh
            Interface.IBS.EnabledControls = true;

            //invoke
            //    bindingChanged(null, new PropertyChangedEventArgs(string.Empty));
            return destiny;
        }
        private void setButtons()
        {
            // this.cancelBtn.Enabled = false;
            this.CalcBtn.Click += delegate
            {
                this.ValidateChildren();
                Cursor.Current = Cursors.WaitCursor;
                this.Tab.SelectedTab = this.CalcTab;
                MatSSF.Calculate(true);
                Cursor.Current = Cursors.Default;
            };
            this.cancelBtn.Click += delegate
            {
                MatSSF.IsCalculating = false;
            };
        }

        private void setCalculator()
        {
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
        }
        private void setTemplateControls()
        {
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
        }

        public ucSSF()
        {
            InitializeComponent();
        }
    }
}