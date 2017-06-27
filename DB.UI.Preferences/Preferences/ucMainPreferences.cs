using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public interface IMainPreferences
    {
        event EventHandler RunInBackground;

        void Set(ref Interface inter);
    }

    public partial class ucMainPreferences : UserControl, IMainPreferences
    {
        protected internal Interface Interface;

        protected internal EventHandler runInBackground = null;

        public event EventHandler RunInBackground
        {
            add
            {
                this.runInBackgroundCheckBox.Click += value;
            }
            remove
            {
                runInBackground -= value;
            }
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                setPreferencesbindings();

                this.runInBackgroundCheckBox.Click += RunInBackgroundCheckBox_CheckStateChanged;
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void RunInBackgroundCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            runInBackground?.Invoke(sender, e);
        }

        protected internal void setCheckStatePrefBindings(ref Hashtable prefbindings)
        {
            this.fillByHLCheckBox.DataBindings.Add(prefbindings["FillByHL"] as Binding);
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings["FillBySpectra"] as Binding);

            this.showSolangCheckBox.DataBindings.Add(prefbindings["ShowSolang"] as Binding);

            this.runInBackgroundCheckBox.DataBindings.Add(prefbindings["RunInBackground"] as Binding);

            this.doSolangCheckBox.DataBindings.Add(prefbindings["DoSolang"] as Binding);
            this.autoLoadCheckBox.DataBindings.Add(prefbindings["AutoLoad"] as Binding);
            this.offlineCheckBox.DataBindings.Add(prefbindings["Offline"] as Binding);
            this.showSampleDescriptionCheckBox.DataBindings.Add(prefbindings["ShowSampleDescription"] as Binding);
            this.advancedEditorCheckBox.DataBindings.Add(prefbindings["AdvancedEditor"] as Binding);
        }

        protected internal void setEnablePreferencesBindings(ref BindingSource bs)
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

            Binding b15 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

            this.advancedEditorCheckBox.DataBindings.Add(b15);

            this.fillByHLCheckBox.DataBindings.Add(b07);
            this.fillBySpectraCheckBox.DataBindings.Add(b08);

            //
            this.showSolangCheckBox.DataBindings.Add(b00);
            this.doSolangCheckBox.DataBindings.Add(b01);
            this.offlineCheckBox.DataBindings.Add(b02);

            this.maxUncTextBox.DataBindings.Add(b03);
            this.minAreaTextBox.DataBindings.Add(b04);
            this.windowBTextBox.DataBindings.Add(b05);
            this.windowATextBox.DataBindings.Add(b06);

            this.showSampleDescriptionCheckBox.DataBindings.Add(b09);
        }

        protected internal void setPreferencesbindings()
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

        public ucMainPreferences()
        {
            InitializeComponent();
        }
    }
}