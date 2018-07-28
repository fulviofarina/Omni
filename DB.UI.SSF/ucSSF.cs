using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using VTools;

namespace DB.UI
{
    public interface ISSF
    {
        IPanel IPanel { get; }

        void Calculate(bool? Bkg = null);

        void Hide();

        void Set(ref IGenericBox pro);

        void Set(ref IOptions options);

        void Set(ref Interface inter, ref IPreferences pref);

        void SetTimer();
    }

    public partial class ucSSF : UserControl, ISSF
    {
        protected internal Interface Interface = null;
        protected internal MatSSF MatSSF = null;
        protected internal Timer timer = null;
        protected static Size currentSize;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        private Action<int> resetProgress;

        private EventHandler showProgress;

        public IPanel IPanel
        {
            get
            {
                return ucSSPan;
            }
        }

        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        public void Calculate(bool? Bkg = null)
        {
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

        public string getAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }

        public string getCurrentCpuUsage()
        {
            string valor = Decimal.Round(Convert.ToDecimal(cpuCounter.NextValue()), 0) + "% CPU";
            return valor;
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

        /// <param name="pro"></param>
        public void Set(ref IGenericBox pro)
        {
            try
            {
       
                ucGenericCBox projBox = pro as ucGenericCBox;
                projBox.HideChildControl = Hide;
                this.splitContainer1.Panel1.Controls.Add(pro as Control);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter, ref IPreferences pref)
        {
            Interface = inter;
            try
            {
                currentSize = this.Size;
                //
                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                // OTHER CONTROLS
                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.ucUnit.DgvItemSelected;
                //

                ucUnit.Set(ref Interface, ref pref);


                Interface.IBS.EnableControlsChanged += delegate
                {
                    bool ThereIsData = Interface.IBS.SubSamples.Count != 0;
                    ucCalculate1.Enabled = ThereIsData;
                    ucUnit.DGVRefresher.Invoke(null, EventArgs.Empty);
                };


                IPanel.Set(ref Interface, ref pref);

                IGenericBox box = IPanel.ISample;
                Creator.SetSampleBox(ref box);
                box = IPanel.ISampleDescription;
                Creator.SetSampleDescriptionBox(ref box);

                //EN ESTE ORDEN!!!!
                IPanel.On = this.CalcTab;
                IPanel.Off = this.MatrixTab;

                //
                this.templatesTabCtrl.Selected += delegate
                {
                    bool matrix = false;
                    if (templatesTabCtrl.SelectedTab == MatrixTab)
                    {
                        matrix = true;
                    }
                    IPanel.ViewChanged(matrix, EventArgs.Empty);
                };


             


                Interface.IReport.Msg("SSF Control OK", "Controls were set!");
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg("SSF Control NOT OK", "Severe error");
                Interface.IStore.AddException(ex);
            }
        }

        public void Set(ref IOptions options)
        {
            resetProgress = options.ResetProgress;
            showProgress = options.ShowProgress;

            EventHandler callBack = delegate
            {
            };
            EventHandler showProg = showProgress;
      
            showProg += ucUnit.DGVRefresher;

            string path = Interface.IStore.FolderPath + Resources.SSFFolder;
            MatSSF = new MatSSF();
            MatSSF.Set(path, callBack, resetProgress, showProg);
            MatSSF.Set(ref Interface);
            ucCalculate1.CancelMethod += delegate
            {
                MatSSF.IsCalculating = false;
            };
            ucCalculate1.CalculateMethod += delegate
            {
                Calculate(null);
            };

            // projBox.HideChildControl = Hide;
            this.splitContainer1.Panel2.Controls.Add(options as Control);
        }

        public void SetTimer()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            // ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            timer = new Timer();

            timer.Interval = 5000;
            timer.Tick += delegate
            {
                // string mb = getAvailableRAM();
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
                IPanel.SetMessage(status + mb + "; " + cpu);

                // ucSSPan.RefreshChart().Invoke(null,EventArgs.Empty);
            };
            timer.Enabled = true;

            /*
            //activate TIMER ONLY WHEN APP IS IDLE OR WHEN IDLE IS RAISED
            timer.Tick += delegate
            {
             // Environment.Workin

            // string mb = Decimal.Round(Convert.ToDecimal(Environment.WorkingSet * 1e-6),1).ToString();
                if (!timer.Enabled && runInBK)
                {
                    timer.Start();
                  // ucSSPan.SetMessage("On");
                }
                else if (!runInBK)
                {
                    timer.Stop();

                  // ucSSPan.SetMessage("Idle");
                }
            };
            */
        }

        // Consume like this:
     

        public ucSSF()
        {
            InitializeComponent();
        }
    }
}