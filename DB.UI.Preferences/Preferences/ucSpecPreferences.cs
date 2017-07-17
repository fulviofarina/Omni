using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public interface ISpecPreferences
    {
        event EventHandler FilterChangedEvent;
        event EventHandler IndexChangedEvent;
        void Set(ref Interface inter);
    }

    public partial class ucSpecPreferences : UserControl, ISpecPreferences
    {
        protected internal Interface Interface;
   
        protected internal EventHandler filterChangedEvent = null;

        public event EventHandler FilterChangedEvent
        {
            add
            {
                filterChangedEvent += value;
            }
            remove
            {
                filterChangedEvent -= value;
            }
        }
        protected internal EventHandler indexChangedEvent = null;

        public event EventHandler IndexChangedEvent
        {
            add
            {
                indexChangedEvent += value;
            }
            remove
            {
                indexChangedEvent -= value;
            }
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                setPreferencesbindings();

                this.measBox.KeyUp += indexKey_Up;
                this.posBox.KeyUp += indexKey_Up;
                this.detbox.KeyUp += indexKey_Up;

                this.windowATextBox.KeyUp += filterKey_up;
                this.windowBTextBox.KeyUp += filterKey_up;
                this.minAreaTextBox.KeyUp += filterKey_up;
                this.maxUncTextBox.KeyUp += filterKey_up;


                //    this.measBox.KeyUp += RunInBackgroundCheckBox_CheckStateChanged;
                //   this.measBox.KeyUp += RunInBackgroundCheckBox_CheckStateChanged;
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }
        
        protected internal void filterKey_up(object sender, EventArgs e)
        {
            filterChangedEvent?.Invoke(sender, e);
        }
        protected internal void indexKey_Up(object sender, EventArgs e)
        {
            indexChangedEvent?.Invoke(sender, e);
        }

        protected internal void setCheckStateBindings(ref Hashtable prefbindings)
        {
            this.fillByHLCheckBox.DataBindings.Add(prefbindings["FillByHL"] as Binding);
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings["FillBySpectra"] as Binding);

          
            //  this.runInBackgroundCheckBox.DataBindings.Add(prefbindings["RunInBackground"] as Binding);

            //  this.doSolangCheckBox.DataBindings.Add(prefbindings["DoSolang"] as Binding);
            //   this.autoLoadCheckBox.DataBindings.Add(prefbindings["AutoLoad"] as Binding);
            //    this.offlineCheckBox.DataBindings.Add(prefbindings["Offline"] as Binding);
            //     this.showSampleDescriptionCheckBox.DataBindings.Add(prefbindings["ShowSampleDescription"] as Binding);
            //    this.advancedEditorCheckBox.DataBindings.Add(prefbindings["AdvancedEditor"] as Binding);
        }

        protected internal void setEnableBindings(ref BindingSource bs)
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
         //   Binding b09 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b10 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

         //   Binding b15 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");

         //  this.advancedEditorCheckBox.DataBindings.Add(b15);

            this.fillByHLCheckBox.DataBindings.Add(b07);
            this.fillBySpectraCheckBox.DataBindings.Add(b08);

            //
            this.posBox.DataBindings.Add(b00);
            this.measBox.DataBindings.Add(b01);
            this.detbox.DataBindings.Add(b02);

            this.maxUncTextBox.DataBindings.Add(b03);
            this.minAreaTextBox.DataBindings.Add(b04);
            this.windowBTextBox.DataBindings.Add(b05);
            this.windowATextBox.DataBindings.Add(b06);

     //       this.showSampleDescriptionCheckBox.DataBindings.Add(b09);
        }

        protected internal void setPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.Preferences;

            Hashtable prefbindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            //checkstate
            setCheckStateBindings(ref prefbindings);

            setEnableBindings(ref bs);

            //text binding
            Hashtable bindings2 = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty);
            setValueBindings(ref bindings2);

        }

        private void setValueBindings(ref Hashtable bindings2)
        {
            this.maxUncTextBox.DataBindings.Add(bindings2["maxUnc"] as Binding);
            this.minAreaTextBox.DataBindings.Add(bindings2["minArea"] as Binding);
            this.windowBTextBox.DataBindings.Add(bindings2["windowB"] as Binding);
            this.windowATextBox.DataBindings.Add(bindings2["windowA"] as Binding);

            this.measBox.DataBindings.Add(bindings2["MeasIdx"] as Binding);
            this.posBox.DataBindings.Add(bindings2["PositionIdx"] as Binding);
            this.detbox.DataBindings.Add(bindings2["DetectorIdx"] as Binding);
        }

        public ucSpecPreferences()
        {
            InitializeComponent();
        }
    }
}