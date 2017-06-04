using System;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using VTools;

namespace DB.UI
{
    public interface ISSF
    {
        void AttachCtrl<T>(ref T pro);

        void Calculate(bool? Bkg = null);

        void Hide();

        void Set(ref Interface inter);
    }

    public partial class ucSSF : UserControl, ISSF
    {
        protected internal MatSSF MatSSF = null;
        protected static Size currentSize;
        protected internal Interface Interface = null;

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
                if (t.Equals(typeof(ucGenericCBox)))
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
                    ucSSPan.AttachCtrl(ref pro);
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

        protected internal Timer timer = null;

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {
                currentSize = this.Size;

                setTemplateControls();
                //
                setSampleControl();

                Interface.IReport.Msg("SSF Control OK", "Controls were set!");
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg("SSF Control NOT OK", "Severe error");
                Interface.IStore.AddException(ex);
            }
        }

        public void SetTimer()
        {
            this.CalcBtn.Click += delegate
            {
                Calculate(null);
            };

            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 5000;
            timer.Tick += delegate
            {
                // timer.Enabled = false;
                timer.Stop();

                if (Interface.IPreferences.CurrentPref.RunInBackground)
                {
                    this.Calculate(true);
                }
            };

            //activate TIMER ONLY WHEN APP IS IDLE OR WHEN IDLE IS RAISED
            Application.Idle += delegate
            {
                // Interface.IReport.Msg("Is Idle", string.Empty);
              //  string status = "Timer Enabled";
                bool runInBK = Interface.IPreferences.CurrentPref.RunInBackground;
                if (!timer.Enabled && runInBK)
                {
                    //Interface.IReport.Msg("Timer Enabled", string.Empty);
                    timer.Start();
                    ucSSPan.SetMessage("On");
                   // Interface.IReport.Msg(status, "");
                }
                else if (!runInBK)
                {
             //       status = "Timer Disabled";
                    timer.Stop();
                    ucSSPan.SetMessage("Idle");
                   // Interface.IReport.Msg(status, );
                }
                else ucSSPan.SetMessage("*/*");

                // Interface.IReport.Msg(status, "Is Idle");
            };
        }

        private Action<int> resetProgress;
        private EventHandler showProgress;

        private Control attachOptions<T>(T pro)
        {
            Control destiny;
            IucOptions options = pro as IucOptions;

            //SET A METHOD TO CALL BACK!!!!

            resetProgress = options.ResetProgress;
            showProgress = options.ShowProgress;

            // projBox.HideChildControl = Hide;
            destiny = this.splitContainer1.Panel2;
            return destiny;
        }

        private Control attachProjectbox<T>(T pro)
        {
            Control destiny;
            ucGenericCBox projBox = pro as ucGenericCBox;
            projBox.HideChildControl = Hide;
            destiny = this.splitContainer1.Panel1;
            //attach binding
            Interface.IBS.PropertyChangedHandler += delegate
            {
                bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
                bool isCalculating = Interface.IBS.IsCalculating;
                this.CalcBtn.Enabled = ThereIsData;
                this.cancelBtn.Enabled = ThereIsData;
                // this.CalcBtn.Enabled = ThereIsData && !isCalculating; this.cancelBtn.Enabled =
                // ThereIsData && isCalculating; this.Tab.SelectedTab = this.CalcTab; ucUnit.PaintRows();
            };
            // force refresh
            Interface.IBS.EnabledControls = true;

            //invoke
            //    bindingChanged(null, new PropertyChangedEventArgs(string.Empty));
            return destiny;
        }

        public void Calculate(bool? Bkg = null)
        {
            EventHandler callBack = delegate
            {
                timer.Start();// = true;
                              // timer.Interval = 5000; this.ucUnit.PaintRows();
            };

            if (MatSSF == null)
            {
                MatSSF = new MatSSF();
                MatSSF.Set(ref Interface, callBack, resetProgress, showProgress);
            }

            bool background = false;
            //if bkg is null take from preferences
            if (Bkg == null)
            {
                background = Interface.IPreferences.CurrentPref.RunInBackground;
            }
            //otherwise take from sender...
            else background = (bool)Bkg;
            //if not background touch controls

            if (!background)
            {
                this.ValidateChildren();
                // Cursor.Current = Cursors.WaitCursor;
                this.Tab.SelectedTab = this.CalcTab;
            }
            //calculate method
            this.cancelBtn.Click += delegate
            {
                MatSSF.IsCalculating = false;
            };

            MatSSF.Calculate(background);
            // Cursor.Current = Cursors.Default;
        }

        private void setSampleControl()
        {
            //EN ESTE ORDEN!!!!
            ucSSPan.On = this.CalcTab;
            ucSSPan.Off = this.MatrixTab;
            ucSSPan.Set(ref Interface);
            //

            this.templatesTabCtrl.Selected += delegate
            {
                bool matrix = false;
                if (templatesTabCtrl.SelectedTab == MatrixTab)
                {
                    matrix = true;
                }
                ucSSPan.ViewChanged(matrix, EventArgs.Empty);
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