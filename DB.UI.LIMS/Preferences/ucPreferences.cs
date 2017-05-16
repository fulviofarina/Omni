using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb; using Rsx;

namespace DB.UI
{
    public partial class ucPreferences : UserControl, IucPreferences
    {
        private Interface Interface;
        // 
        public EventHandler CheckChanged
        {

            set
            {
                this.aARadiusCheckBox.CheckedChanged += value;
                this.aAFillHeightCheckBox.CheckedChanged += value;
                this.calcDensityCheckBox.CheckedChanged += value;
                this.calcMassCheckBox.CheckedChanged += value;
            }
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
             //   this.PrefBS.DataSource = Interface.Get();
             //   this.SSFPref.DataSource = Interface.Get();
              //  this.PrefBS.DataMember = Interface.IDB.Preferences.TableName;
              //  this.SSFPref.DataMember = Interface.IDB.SSFPref.TableName;

         //       Interface.IBS.Preferences = this.PrefBS;
           //     Interface.IBS.SSFPreferences = this.SSFPref;

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
               

           //     this.PrefBS.ResetBindings(true);
             //   this.SSFPref.ResetBindings(true);
            }
            catch (Exception ex)
            {
                inter.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Sets the Rounding format "link" between the Preferences and the
        /// textBoxes. The arguments
        /// (Hashtables of bindings) for these boxes must be provided
        /// </summary>
        /// <param name="unitsTable"> </param>
        /// <param name="sampleTable"></param>
        public void SetRoundingBinding(ref Hashtable bindings)
        {
            //the bindings to round
            Hashtable units = bindings;
         //   Hashtable samples = samplebindings;
            //Do the update when this control losses focus
            this.LostFocus += delegate
              {
                  try
                  {
                      IPreferences ip = Interface.IPreferences;
                  
                      string format = this.roundingTextBox.Text;
                      if (format.Length < 2) return;
                      BS.ChangeBindingsFormat(format, ref units);
                  //    BS.ChangeBindingsFormat(format, ref samples);

                      Interface.IPreferences.CurrentSSFPref.Rounding = format;

                      //save preferences
                      Interface.IPreferences.SavePreferences();
                  }
                  catch (Exception ex)
                  {
                      Interface.IStore.AddException(ex);
                  }
              };
        }

        private void setPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.Preferences;

            Hashtable prefbindings = BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            this.fillByHLCheckBox.DataBindings.Add(prefbindings["FillByHL"] as Binding);
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings["FillBySpectra"] as Binding);

            //
            this.showSolangCheckBox.DataBindings.Add(prefbindings["ShowSolang"] as Binding);
            this.doSolangCheckBox.DataBindings.Add(prefbindings["DoSolang"] as Binding);
            this.autoLoadCheckBox.DataBindings.Add(prefbindings["AutoLoad"] as Binding);
            this.offlineCheckBox.DataBindings.Add(prefbindings["Offline"] as Binding);
            this.showSampleDescriptionCheckBox.DataBindings.Add(prefbindings["ShowSampleDescription"] as Binding);
            this.advancedEditorCheckBox.DataBindings.Add(prefbindings["AdvancedEditor"] as Binding);
    
            //text binding
            Hashtable bindings2 = BS.ArrayOfBindings(ref bs, string.Empty);

            this.maxUncTextBox.DataBindings.Add(bindings2["maxUnc"] as Binding);
            this.minAreaTextBox.DataBindings.Add(bindings2["minArea"] as Binding);
            this.windowBTextBox.DataBindings.Add(bindings2["windowB"] as Binding);
            this.windowATextBox.DataBindings.Add(bindings2["windowA"] as Binding);
        }

        private void setSSFPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.SSFPreferences;

            Hashtable bindings = BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");

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



            Binding b2 = BS.ABinding(ref bs, Interface.IDB.SSFPref.RoundingColumn.ColumnName);
            this.roundingTextBox.DataBindings.Add(b2);
        }

        public ucPreferences()
        {
            InitializeComponent();
        }
    }
}