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


                this.sampleModelBox.KeyUp += indexKey_Up;
                this.monitorModelBox.KeyUp += indexKey_Up;


                this.windowATextBox.KeyUp += filterKey_up;
                this.windowBTextBox.KeyUp += filterKey_up;
                this.minAreaTextBox.KeyUp += filterKey_up;
                this.maxUncTextBox.KeyUp += filterKey_up;


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
            if (sender.Equals(sampleModelBox))
            {
                Interface.IPreferences.CurrentSpecPref.SetIdxLength(false);
            }
            else Interface.IPreferences.CurrentSpecPref.SetIdxLength(true);
            indexChangedEvent?.Invoke(sender, e);
        }

        protected internal void setCheckStateBindings(ref Hashtable prefbindings)
        {
            this.fillByHLCheckBox.DataBindings.Add(prefbindings["FillByHL"] as Binding);
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings["FillBySpectra"] as Binding);
        }

        protected internal void setEnableBindings(ref BindingSource bs)
        {
            string col = Interface.IDB.Preferences.AdvancedEditorColumn.ColumnName;


            Binding b07 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
            Binding b08 = Rsx.Dumb.BS.ABinding(ref bs, col, string.Empty, "Enabled");
    
            this.fillByHLCheckBox.DataBindings.Add(b07);
            this.fillBySpectraCheckBox.DataBindings.Add(b08);
          
        }

        protected internal void setPreferencesbindings()
        {
            BindingSource bs = Interface.IBS.Preferences;

            Hashtable prefbindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");

            //checkstate
            setCheckStateBindings(ref prefbindings);
            setEnableBindings(ref bs);

            bs = Interface.IBS.SpecPref;
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
            this.sampleBox.DataBindings.Add(bindings2["SampleIdx"] as Binding);
            this.projectBox.DataBindings.Add(bindings2["ProjectIdx"] as Binding);
         //   this.monBox.DataBindings.Add(bindings2["MonitorIdx"] as Binding);


            this.measNrLenBox.DataBindings.Add(bindings2["MeasLength"] as Binding);
            this.posLenBox.DataBindings.Add(bindings2["PositionLength"] as Binding);
            this.detLenBox.DataBindings.Add(bindings2["DetectorLength"] as Binding);
            this.sampleLenBox.DataBindings.Add(bindings2["SampleLength"] as Binding);
            this.projectLenBox.DataBindings.Add(bindings2["ProjectLength"] as Binding);
        //    this.monLenBox.DataBindings.Add(bindings2["MonitorLength"] as Binding);
            this.sampleModelBox.DataBindings.Add(bindings2["ModelSample"] as Binding);
            this.monitorModelBox.DataBindings.Add(bindings2["ModelMonitor"] as Binding);


        }

        public ucSpecPreferences()
        {
            InitializeComponent();
        }
    }
}