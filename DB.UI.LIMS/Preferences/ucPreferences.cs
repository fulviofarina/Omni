using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public partial class ucPreferences : UserControl, IucPreferences
    {
        private EventHandler doChilianChanged = null;
        private EventHandler doMatSSFChanged = null;
        private Interface Interface;

        // public EventHandler DoChilianChanged;

        // public EventHandler DoMatSSFChanged;

        private EventHandler runInBackground = null;
        private EventHandler sampleChanged = null;

        public event EventHandler DoChilianChanged
        {
            add
            {
                // doChilianChanged += value;

                this.doCKCheckBox.Click += value;

                // this.doMatSSFCheckBox.Click += value; this.overridesCheckBox.Click += value;
            }
            remove
            {
                doChilianChanged -= value;
                // this.doCKCheckBox.CheckStateChanged -= value; this.overridesCheckBox.Click -= value;
            }
        }

        public event EventHandler DoMatSSFChanged
        {
            add
            {
                // doMatSSFChanged += value;
                this.doMatSSFCheckBox.Click += value;

                // this.overridesCheckBox.Click += value;
            }
            remove
            {
                doMatSSFChanged -= value;
                // this.doMatSSFCheckBox.CheckStateChanged -= value; this.overridesCheckBox.Click -= value;
            }
        }

        public event EventHandler OverriderChanged
        {
            add
            {
                // this.doMatSSFCheckBox.Click += value;
                this.overridesCheckBox.Click += value;
            }
            remove
            {
                this.overridesCheckBox.CheckStateChanged -= value;
            }
        }

        public event EventHandler RunInBackground
        {
            add
            {
                this.runInBackgroundCheckBox.Click += value;
            }
            remove
            {
                runInBackground -= value;
                // this.runInBackgroundCheckBox.CheckStateChanged -= value;
                // this.overridesCheckBox.Click -= value;
            }
        }

        public event EventHandler CalcRadiusChanged
        {
            add
            {
                //+= value;
                this.aARadiusCheckBox.Click += value;
             //   this.aAFillHeightCheckBox.CheckedChanged += value;
             //   this.calcDensityCheckBox.CheckedChanged += value;

              //  this.calcMassCheckBox.CheckedChanged += value;
                // this.calcMassCheckBox.CheckStateChanged += generic;
            }
            remove
            {
                sampleChanged -= value;

                // this.calcMassCheckBox.CheckStateChanged -= generic;
            }
        }
        public event EventHandler CalcLengthChanged
        {
            add
            {
                //+= value;
            //    this.aARadiusCheckBox.CheckedChanged += value;
                this.aAFillHeightCheckBox.Click += value;
            //    this.calcDensityCheckBox.CheckedChanged += value;

            //    this.calcMassCheckBox.CheckedChanged += value;
                // this.calcMassCheckBox.CheckStateChanged += generic;
            }
            remove
            {
                sampleChanged -= value;

                // this.calcMassCheckBox.CheckStateChanged -= generic;
            }
        }

        public event EventHandler CalcDensityChanged
        {
            add
            {
                //+= value;
               // this.aARadiusCheckBox.CheckedChanged += value;
              //  this.aAFillHeightCheckBox.CheckedChanged += value;
                this.calcDensityCheckBox.Click += value;

              //  this.calcMassCheckBox.CheckedChanged += value;
                // this.calcMassCheckBox.CheckStateChanged += generic;
            }
            remove
            {
                sampleChanged -= value;

                // this.calcMassCheckBox.CheckStateChanged -= generic;
            }
        }
        public event EventHandler CalcMassChanged
        {
            add
            {
                //+= value;
               // this.aARadiusCheckBox.CheckedChanged += value;
               // this.aAFillHeightCheckBox.CheckedChanged += value;
               // this.calcDensityCheckBox.CheckedChanged += value;

                this.calcMassCheckBox.Click += value;
                // this.calcMassCheckBox.CheckStateChanged += generic;
            }
            remove
            {
                sampleChanged -= value;

                // this.calcMassCheckBox.CheckStateChanged -= generic;
            }
        }


        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                setSSFPreferencesbindings();

                setPreferencesbindings();

                // this.calcMassCheckBox.Enabled = false;

                this.doCKCheckBox.Click += DoCKCheckBox_CheckStateChanged;
                //this.aARadiusCheckBox.CheckedChanged += generic;

                this.runInBackgroundCheckBox.Click += RunInBackgroundCheckBox_CheckStateChanged;
                this.doMatSSFCheckBox.Click += DoMatSSFCheckBox_CheckStateChanged;

                this.aARadiusCheckBox.Click += generic;
                this.aAFillHeightCheckBox.Click += generic;
                this.calcDensityCheckBox.Click += generic;

            //    this.calcMassCheckBox.Click += generic;
            }
            catch (Exception ex)
            {
                inter.IStore.AddException(ex);
            }
        }

        public void SetRoundingBinding(ref Hashtable bindings)
        {
            /*
            //the bindings to round
            Hashtable units = bindings;
            this.LostFocus += delegate
              {
                  try
                  {
                      IPreferences ip = Interface.IPreferences;

                      string format = this.roundingTextBox.Text;
                      if (format.Length < 2) return;
                      Rsx.Dumb.BS.ChangeBindingsFormat(format, ref units);

                      Interface.IPreferences.CurrentSSFPref.Rounding = format;

                      Interface.IPreferences.SavePreferences();
                  }
                  catch (Exception ex)
                  {
                      Interface.IStore.AddException(ex);
                  }
              };

            */
        }

        private void DoCKCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            // Interface.IPreferences.AcceptChanges(); doChilianChanged?.Invoke(sender, e);
        }

        private void DoMatSSFCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            // Interface.IPreferences.AcceptChanges();
        }

        private void generic(object sender, EventArgs e)
        {
            // Interface.IPreferences.AcceptChanges(); this.Invoke(sampleChanged );//.Invoke(sender,
            // e); this.ValidateChildren();
        }

        private void RunInBackgroundCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            runInBackground?.Invoke(sender, e);
        }

        private void setCheckStatePrefBindings(ref Hashtable prefbindings)
        {
            this.fillByHLCheckBox.DataBindings.Add(prefbindings["FillByHL"] as Binding);
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings["FillBySpectra"] as Binding);

            //
            this.showSolangCheckBox.DataBindings.Add(prefbindings["ShowSolang"] as Binding);

            this.runInBackgroundCheckBox.DataBindings.Add(prefbindings["RunInBackground"] as Binding);

            this.doSolangCheckBox.DataBindings.Add(prefbindings["DoSolang"] as Binding);
            this.autoLoadCheckBox.DataBindings.Add(prefbindings["AutoLoad"] as Binding);
            this.offlineCheckBox.DataBindings.Add(prefbindings["Offline"] as Binding);
            this.showSampleDescriptionCheckBox.DataBindings.Add(prefbindings["ShowSampleDescription"] as Binding);
            this.advancedEditorCheckBox.DataBindings.Add(prefbindings["AdvancedEditor"] as Binding);
        }

        private void setEnablePreferencesBindings(ref BindingSource bs)
        {
            string col = Interface.IDB.Preferences.AdvancedEditorColumn.ColumnName;
            Binding b00 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b01 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b02 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b03 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b04 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b05 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b06 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b07 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b08 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b09 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b10 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b11 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b12 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b13 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

            Binding b14 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b15 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b16 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

            this.advancedEditorCheckBox.DataBindings.Add(b15);

            this.showOtherCheckBox.DataBindings.Add(b16);
            // this.overridesCheckBox.DataBindings.Add(b10);
            this.showMatSSFCheckBox.DataBindings.Add(b11);
            this.doCKCheckBox.DataBindings.Add(b12);
            this.doMatSSFCheckBox.DataBindings.Add(b13);

            this.roundingTextBox.DataBindings.Add(b14);

            this.fillByHLCheckBox.DataBindings.Add(b07);
            this.fillBySpectraCheckBox.DataBindings.Add(b08);

            //
            this.showSolangCheckBox.DataBindings.Add(b00);
            this.doSolangCheckBox.DataBindings.Add(b01);
            // this.autoLoadCheckBox.DataBindings.Add(prefbindings["AutoLoad"] as Binding);
            this.offlineCheckBox.DataBindings.Add(b02);

            this.maxUncTextBox.DataBindings.Add(b03);
            this.minAreaTextBox.DataBindings.Add(b04);
            this.windowBTextBox.DataBindings.Add(b05);
            this.windowATextBox.DataBindings.Add(b06);

            this.showSampleDescriptionCheckBox.DataBindings.Add(b09);
            // this.advancedEditorCheckBox.DataBindings.Add(prefbindings["AdvancedEditor"] as
            // Binding); return bs;
        }

        private void setPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.Preferences;

            Hashtable prefbindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            //checkstate
            setCheckStatePrefBindings(ref prefbindings);

            setEnablePreferencesBindings(ref bs);

            //text binding
            Hashtable bindings2 = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty);

            this.maxUncTextBox.DataBindings.Add(bindings2["maxUnc"] as Binding);
            this.minAreaTextBox.DataBindings.Add(bindings2["minArea"] as Binding);
            this.windowBTextBox.DataBindings.Add(bindings2["windowB"] as Binding);
            this.windowATextBox.DataBindings.Add(bindings2["windowA"] as Binding);
        }

        private void setSSFPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.SSFPreferences;

            Hashtable bindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            string column = Interface.IDB.SSFPref.OverridesColumn.ColumnName;
            this.overridesCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.AARadiusColumn.ColumnName;
            this.aARadiusCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.ShowOtherColumn.ColumnName;
            this.showOtherCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName;
            this.aAFillHeightCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.DoMatSSFColumn.ColumnName;
            this.doMatSSFCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.DoCKColumn.ColumnName;
            this.doCKCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.CalcDensityColumn.ColumnName;
            this.calcDensityCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.CalcMassColumn.ColumnName;
            this.calcMassCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.LoopColumn.ColumnName;
            this.loopCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.SSFPref.ShowMatSSFColumn.ColumnName;
            this.showMatSSFCheckBox.DataBindings.Add(bindings[column] as Binding);

            Binding b2 = Rsx.Dumb.BS.ABinding(ref bs, Interface.IDB.SSFPref.RoundingColumn.ColumnName);
            this.roundingTextBox.DataBindings.Add(b2);


        }

        public ucPreferences()
        {
            InitializeComponent();
        }
    }
}