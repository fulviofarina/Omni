using System;
using System.Collections;
using System.Windows.Forms;
using DB.Tools;
using static DB.LINAA;
using System.Collections.Generic;
using System.Data;

namespace DB.UI
{
    public interface ISpecPreferences
    {
        event EventHandler CallBackEventHandler;
       // event EventHandler PreferencesValidated;
        void Set(ref Interface inter);
    }

    public partial class ucSpecPreferences : UserControl, ISpecPreferences
    {
        protected internal Interface Interface;
   
        protected internal EventHandler callBackHandler = null;

        public event EventHandler CallBackEventHandler
        {
            add
            {
                callBackHandler += value;
            }
            remove
            {
                callBackHandler -= value;
            }
        }


        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                setPreferencesbindings();


               this.sampleModelBox.KeyUp += acceptChanges;
               this.monitorModelBox.KeyUp += acceptChanges;

                this.windowATextBox.KeyUp += filterKey_up;
                this.windowBTextBox.KeyUp += filterKey_up;
                this.minAreaTextBox.KeyUp += filterKey_up;
                this.maxUncTextBox.KeyUp += filterKey_up;

                this.Validated += validated;


            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void validated(object sender, EventArgs e)
        {
            if (Visible)
            {
                acceptChanges(sender, e);
                IEnumerable<MeasurementsRow> meas = Interface.IDB.Measurements;
                Interface.IPopulate.IMeasurements.CheckMeasurements(ref meas);
            }
        }

        protected internal void filterKey_up(object sender, EventArgs e)
        {
            acceptChanges(sender, e);

            callBackHandler?.Invoke(sender, e);
        }
        
        protected internal void acceptChanges(object sender, EventArgs e)
        {
            Interface.IPreferences.CurrentSpecPref.AcceptChanges();
        }
        

        protected internal void setCheckStateBindings(ref Hashtable prefbindings)
        {
            string name = Interface.IDB.Preferences.FillByHLColumn.ColumnName;

            this.fillByHLCheckBox.DataBindings.Add(prefbindings[name] as Binding);
            name = Interface.IDB.Preferences.FillBySpectraColumn.ColumnName;
            this.fillBySpectraCheckBox.DataBindings.Add(prefbindings[name] as Binding);
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
            string name = Interface.IDB.SpecPref.maxUncColumn.ColumnName;
            this.maxUncTextBox.DataBindings.Add(bindings2[name] as Binding);
             name = Interface.IDB.SpecPref.minAreaColumn.ColumnName;
            this.minAreaTextBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.windowBColumn.ColumnName;
            this.windowBTextBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.windowAColumn.ColumnName;
            this.windowATextBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.MeasIdxColumn.ColumnName;
            this.measBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.PositionIdxColumn.ColumnName;
            this.posBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.DetectorIdxColumn.ColumnName;
            this.detbox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.SampleIdxColumn.ColumnName;
            this.sampleBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.ProjectIdxColumn.ColumnName;
            this.projectBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.TimeDividerColumn.ColumnName;
            this.timeDivBox.DataBindings.Add(bindings2[name] as Binding);

            name = Interface.IDB.SpecPref.MeasLengthColumn.ColumnName;
            this.measNrLenBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.PositionLengthColumn.ColumnName;
            this.posLenBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.DetectorLengthColumn.ColumnName;
            this.detLenBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.SampleLengthColumn.ColumnName;
            this.sampleLenBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.ProjectLengthColumn.ColumnName;
            this.projectLenBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.ModelSampleColumn.ColumnName;
            this.sampleModelBox.DataBindings.Add(bindings2[name] as Binding);
            name = Interface.IDB.SpecPref.ModelMonitorColumn.ColumnName;
            this.monitorModelBox.DataBindings.Add(bindings2[name] as Binding);


          

        }

        public ucSpecPreferences()
        {
            InitializeComponent();
        }
    }
}