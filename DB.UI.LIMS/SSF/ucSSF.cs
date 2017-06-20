using System;
using System.Diagnostics;
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
                else
                {
                    //main child
                    ucSSPan.AttachCtrl(ref pro);
                    // ucUnit.AttachCtrl(ref pro);
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

                //
                ucUnit.Set(ref Interface);
              
            //    ucUnit.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                //
                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                // OTHER CONTROLS
                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                //

                Interface.IBS.PropertyChangedHandler += delegate
                {
                    bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
                    bool isCalculating = Interface.IBS.IsCalculating;
                    this.CalcBtn.Enabled = ThereIsData;
                    this.cancelBtn.Enabled = ThereIsData;

                    ucUnit.DGVRefresher.Invoke(null, EventArgs.Empty);

                    // this.CalcBtn.Enabled = ThereIsData && !isCalculating; this.cancelBtn.Enabled =
                    // ThereIsData && isCalculating; this.Tab.SelectedTab = this.CalcTab; ucUnit.PaintRows();
                };

              

                this.CalcBtn.Click += delegate
                {
                    Calculate(null);

                };

                setSampleControl();

                IOptions options = LIMS.GetOptions();
                resetProgress = options.ResetProgress;
                showProgress = options.ShowProgress;
                // projBox.HideChildControl = Hide;
                this.splitContainer1.Panel2.Controls.Add(options as Control);

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

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
         //   ramCounter = new PerformanceCounter("Memory", "Available MBytes");
           

            timer = new Timer();
          
            timer.Interval = 5000;
            timer.Tick += delegate
            {
            //    string mb = getAvailableRAM();
                string cpu = getCurrentCpuUsage();
                bool runInBK = Interface.IPreferences.CurrentPref.RunInBackground;
             
                string mb = Decimal.Round(Convert.ToDecimal(Environment.WorkingSet * 1e-6), 0).ToString();
                mb += "Mb RAM";
                string status = "Idle; ";
              
                if (Interface.IPreferences.CurrentPref.RunInBackground)
                {
                    timer.Stop();
                    this.Calculate(true);
                   

                }
                if (MatSSF != null)
                {
                  if (MatSSF.IsCalculating) status = "ON; ";
                }
                ucSSPan.SetMessage(status + mb +"; " + cpu );

                ucSSPan.RefreshChart().Invoke(null,EventArgs.Empty);

            };
            timer.Enabled = true;

         
            /*
            //activate TIMER ONLY WHEN APP IS IDLE OR WHEN IDLE IS RAISED
            timer.Tick += delegate
            {
             //   Environment.Workin
             
             
            //    string mb = Decimal.Round(Convert.ToDecimal(Environment.WorkingSet * 1e-6),1).ToString();
                if (!timer.Enabled && runInBK)
                {
                    timer.Start();
                  //  ucSSPan.SetMessage("On");
                }
                else if (!runInBK)
                {
                    timer.Stop();
                   
                  //  ucSSPan.SetMessage("Idle");
                }
               
            };
            */
        }
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        // Consume like this:

public string getCurrentCpuUsage()
        {
            string valor = Decimal.Round(Convert.ToDecimal(cpuCounter.NextValue()), 0) + "% CPU";
            return valor;
        }

        public string getAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }


        private Action<int> resetProgress;
        private EventHandler showProgress;

        private Control attachProjectbox<T>(T pro)
        {

            Control destiny;
            ucGenericCBox projBox = pro as ucGenericCBox;
            projBox.HideChildControl = Hide;
            destiny = this.splitContainer1.Panel1;
            //attach binding

            // force refresh Interface.IBS.EnabledControls = true;

            //invoke
            //    bindingChanged(null, new PropertyChangedEventArgs(string.Empty));
            return destiny;
        }

        public void Calculate(bool? Bkg = null)
        {
            EventHandler callBack = delegate
            {

               // timer.Start();// = true;
            };

            if (MatSSF == null)
            {
                MatSSF = new MatSSF();


                EventHandler showProg = showProgress;
                showProg +=  ucUnit.DGVRefresher;

                this.cancelBtn.Click += delegate
                {
                    if (MatSSF!=null)   MatSSF.IsCalculating = false;
                };

                MatSSF.Set(ref Interface, callBack, resetProgress, showProg);
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


            //save
            if (!background) Creator.SaveInFull(true);
         
            MatSSF.RunAll(background);

         
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

        public ucSSF()
        {
            InitializeComponent();
        }
    }
}