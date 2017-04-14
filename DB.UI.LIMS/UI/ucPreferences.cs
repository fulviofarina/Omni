using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucPreferences : UserControl, IucPreferences
    {
        private Interface Interface;
        // 

        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                this.PrefBS.DataSource = Interface.Get();
                this.SSFPref.DataSource = Interface.Get();
                this.PrefBS.DataMember = Interface.IDB.Preferences.TableName;
                this.SSFPref.DataMember = Interface.IDB.SSFPref.TableName;

                Interface.IBS.Preferences = this.PrefBS;
                Interface.IBS.SSFPreferences = this.SSFPref;

                setSSFPreferencesbindings();

                setPreferencesbindings();

                this.fillByHLCheckBox.Enabled = false;
                this.windowBTextBox.Enabled = false;
                this.maxUncTextBox.Enabled = false;
                this.windowATextBox.Enabled = false;
                this.minAreaTextBox.Enabled = false;
                this.offlineCheckBox.Enabled = false;
                this.doSolangCheckBox.Enabled = false;
                this.showSolangCheckBox.Enabled = false;
                this.fillBySpectraCheckBox.Enabled = false;
                this.showSampleDescriptionCheckBox.Enabled = false;

                this.PrefBS.ResetBindings(true);
                this.SSFPref.ResetBindings(true);
            }
            catch (Exception ex)
            {
                inter.IMain.AddException(ex);
            }
        }

        /// <summary>
        /// Sets the Rounding format "link" between the Preferences and the
        /// textBoxes. The arguments
        /// (Hashtables of bindings) for these boxes must be provided
        /// </summary>
        /// <param name="unitsTable"> </param>
        /// <param name="sampleTable"></param>
        public void SetRoundingBinding(ref Hashtable unitsTable, ref Hashtable sampleTable)
        {
            //the bindings to round
            Hashtable bindings, samplebindings;
            bindings = unitsTable;
            samplebindings = sampleTable;

            //Do the update when this control losses focus
            this.LostFocus += delegate
              {
                  try
                  {
                      IPreferences ip = Interface.IPreferences;

                      string format = this.roundingTextBox.Text;
                      if (format.Length < 2) return;
                      Dumb.ChangeBindingsFormat(format, ref bindings);
                      Dumb.ChangeBindingsFormat(format, ref samplebindings);

                      Interface.IPreferences.CurrentSSFPref.Rounding = format;

                      //save preferences
                      Interface.IPreferences.SavePreferences();
                  }
                  catch (Exception ex)
                  {
                      Interface.IMain.AddException(ex);
                  }
              };
        }

        private void setPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.Preferences;

            Hashtable prefbindings = Dumb.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            this.fillByHLCheckBox.DataBindings.Add(prefbindings["FillByHL"] as Binding);
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings["FillBySpectra"] as Binding);

            //
            this.showSolangCheckBox.DataBindings.Add(prefbindings["ShowSolang"] as Binding);
            this.doSolangCheckBox.DataBindings.Add(prefbindings["DoSolang"] as Binding);
            this.autoLoadCheckBox.DataBindings.Add(prefbindings["AutoLoad"] as Binding);
            this.offlineCheckBox.DataBindings.Add(prefbindings["Offline"] as Binding);
            this.showSampleDescriptionCheckBox.DataBindings.Add(prefbindings["ShowSampleDescription"] as Binding);

            //text binding
            Hashtable bindings2 = Dumb.ArrayOfBindings(ref bs, string.Empty);

            this.maxUncTextBox.DataBindings.Add(bindings2["maxUnc"] as Binding);
            this.minAreaTextBox.DataBindings.Add(bindings2["minArea"] as Binding);
            this.windowBTextBox.DataBindings.Add(bindings2["windowB"] as Binding);
            this.windowATextBox.DataBindings.Add(bindings2["windowA"] as Binding);
        }

        private void setSSFPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.SSFPreferences;

            Hashtable bindings = Dumb.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            this.aARadiusCheckBox.DataBindings.Add(bindings["AARadius"] as Binding);

            this.showOtherCheckBox.DataBindings.Add(bindings["ShowOther"] as Binding);
            this.aAFillHeightCheckBox.DataBindings.Add(bindings["AAFillHeight"] as Binding);
            this.doMatSSFCheckBox.DataBindings.Add(bindings["DoMatSSF"] as Binding);
            this.doCKCheckBox.DataBindings.Add(bindings["DoCK"] as Binding);
            this.calcDensityCheckBox.DataBindings.Add(bindings["CalcDensity"] as Binding);

            this.loopCheckBox.DataBindings.Add(bindings["Loop"] as Binding);
            this.showMatSSFCheckBox.DataBindings.Add(bindings["ShowMatSSF"] as Binding);

            Binding b2 = new Binding("Text", bs, Interface.IDB.SSFPref.RoundingColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);
            this.roundingTextBox.DataBindings.Add(b2);
        }

        public ucPreferences()
        {
            InitializeComponent();
        }
    }
}