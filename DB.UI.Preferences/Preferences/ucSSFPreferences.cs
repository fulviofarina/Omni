using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public interface ISSFPreferences
    {
        event EventHandler CalcDensityChanged;

        event EventHandler CalcLengthChanged;

        event EventHandler CalcMassChanged;

        event EventHandler CalcRadiusChanged;

        event EventHandler DoChilianChanged;

        event EventHandler DoMatSSFChanged;

        event EventHandler OverriderChanged;

        event EventHandler RoundingChanged;

        void Set(ref Interface inter);
    }

    public partial class ucSSFPreferences : UserControl, ISSFPreferences
    {
        protected internal EventHandler doChilianChanged = null;
        protected internal EventHandler doMatSSFChanged = null;
        protected internal Interface Interface;

        protected internal EventHandler sampleChanged = null;

        public event EventHandler CalcDensityChanged
        {
            add
            {
                this.calcDensityCheckBox.Click += value;
            }
            remove
            {
                sampleChanged -= value;
            }
        }

        public event EventHandler CalcLengthChanged
        {
            add
            {
                this.aAFillHeightCheckBox.Click += value;
            }
            remove
            {
                sampleChanged -= value;
            }
        }

        public event EventHandler CalcMassChanged
        {
            add
            {
                this.calcMassCheckBox.Click += value;
            }
            remove
            {
                sampleChanged -= value;
            }
        }

        public event EventHandler CalcRadiusChanged
        {
            add
            {
                this.aARadiusCheckBox.Click += value;
            }
            remove
            {
                sampleChanged -= value;
            }
        }

        public event EventHandler DoChilianChanged
        {
            add
            {
                this.doCKCheckBox.Click += value;
            }
            remove
            {
                doChilianChanged -= value;
            }
        }

        public event EventHandler DoMatSSFChanged
        {
            add
            {
                this.doMatSSFCheckBox.Click += value;
            }
            remove
            {
                doMatSSFChanged -= value;
            }
        }

        public event EventHandler OverriderChanged
        {
            add
            {
                this.overridesCheckBox.Click += value;
            }
            remove
            {
                this.overridesCheckBox.CheckStateChanged -= value;
            }
        }

        public event EventHandler roundingChanged = null;

        public event EventHandler RoundingChanged
        {
            add
            {
                roundingChanged += value;
            }
            remove
            {
                roundingChanged -= value;
            }
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                setSSFPreferencesbindings();

                setPreferencesbindings();

                this.doCKCheckBox.Click += DoCKCheckBox_CheckStateChanged;
                this.doMatSSFCheckBox.Click += DoMatSSFCheckBox_CheckStateChanged;

                this.aARadiusCheckBox.Click += generic;
                this.aAFillHeightCheckBox.Click += generic;
                this.calcDensityCheckBox.Click += generic;

                this.roundingTextBox.KeyUp += roundingTextBox_TextChanged;

                this.calcMassCheckBox.Click += generic;
            }
            catch (Exception ex)
            {
                inter.IStore.AddException(ex);
            }
        }

        protected internal void DoCKCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
        }

        protected internal void DoMatSSFCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
        }

        protected internal void generic(object sender, EventArgs e)
        {
        }

        protected internal void roundingTextBox_TextChanged(object sender, EventArgs e)
        {
            roundingChanged?.Invoke(sender, e);
        }

        protected internal void setEnablePreferencesBindings(ref BindingSource bs)
        {
            string col = Interface.IDB.Preferences.AdvancedEditorColumn.ColumnName;

            Binding b10 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b11 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b12 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b13 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

            Binding b14 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b16 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

            this.showOtherCheckBox.DataBindings.Add(b16);
            this.showMatSSFCheckBox.DataBindings.Add(b11);
            this.doCKCheckBox.DataBindings.Add(b12);
            this.doMatSSFCheckBox.DataBindings.Add(b13);

            this.roundingTextBox.DataBindings.Add(b14);
        }

        protected internal void setPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.Preferences;

            Hashtable prefbindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");
            setEnablePreferencesBindings(ref bs);
        }

        protected internal void setSSFPreferencesbindings()
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

        public ucSSFPreferences()
        {
            InitializeComponent();
        }
    }
}