using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using Rsx;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFData : UserControl
    {

    //    private static Size currentSize;
        private Hashtable unitBindings, sampleBindings;
      //  private bool cancelCalculations = false;
        private Interface Interface = null;
   //     private Action<int> resetProgress;
   //     private Action showProgress;


        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pro"></param>
        public void AttachCtrl<T>(ref T pro)
        {
            Control destiny = null;
            if (pro.GetType().Equals(typeof(Msn.Pop)))
            {
                (pro as Msn.Pop).Dock = DockStyle.Fill;
                destiny = this.UnitSSFSC.Panel2;
            }
            else if (pro.GetType().Equals(typeof(BindingNavigator)))
            {
                BindingNavigator b = pro as BindingNavigator;
               // b.Items["SaveItem"].Visible = false;
             //   this.unitBN.Dispose();
                destiny = this.unitSC.Panel2;

            }
            else if (pro.GetType().Equals(typeof(ucPreferences)))
            {
                IucPreferences pref = pro as IucPreferences;
                pref.SetRoundingBinding(ref unitBindings, ref sampleBindings);
                destiny = null;

            }
            destiny?.Controls.Add(pro as Control);

        }

     

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {

                //link to bindings
                sampleBindings = setSampleBindings();
                unitBindings = setUnitBindings();

                //types
                this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);

                //set calculation options

              //  Interface.IBS.EndEdit();

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IMain.AddException(ex);
               // Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }
        }

        public void CalculateUnit(Action showProgress, ref bool cancelCalculations)
        {
            try
            {
                IPreferences ip = Interface.IPreferences;

                bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

                bool doCk = (ip.CurrentSSFPref.DoCK);

                bool doSSF = (ip.CurrentSSFPref.DoMatSSF);


                //1

                MatSSF.Table.Clear();


                showProgress?.Invoke();

                MatSSF.INPUT();
                //2
                showProgress?.Invoke();

                Interface.IReport.Msg("Input metadata generated", "Starting Calculations...");

                bool runOk = false;

                if (doSSF && !cancelCalculations) runOk = MatSSF.RUN(hide);

                //4
                showProgress?.Invoke();

                if (!cancelCalculations)
                {
                    if (runOk)
                    {
                        Interface.IReport.Msg("MatSSF Ran", "Reading Output");

                        MatSSF.OUTPUT();

                        if (MatSSF.Table.Count == 0)
                        {
                            throw new SystemException("Problems Reading MATSSF Output\n");
                        }
                    }
                    else if (doSSF)
                    {
                        Interface.IReport.Msg("MatSSF hanged...", "Something happened executing MatSSF");

                        throw new SystemException("MATSSF hanged...\n");

                        // errorB.Text += "MATSSF is still calculating stuff...\n";
                    }
                    Interface.IReport.Msg("MatSSF done", "Calculations completed!");

                }
                else Interface.IReport.Msg("MatSSF cancelled", "Calculations cancelled!");

                //5
                showProgress?.Invoke();

                //     Interface.IReport.Msg("MatSSF done", "Calculations completed!");


                if (doCk && !cancelCalculations)
                {
                    MatSSF.CHILEAN();
                    Interface.IReport.Msg("CK done", "Calculations completed!");
                }

                //convert table into subTable of Units
                if (!cancelCalculations)
                {
                    MatSSF.WriteXML();
                    //this also saves the UNITS!!
                    Interface.IStore.Save<SubSamplesDataTable>();
                }
                //6
                showProgress?.Invoke();


            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
               // Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }
        }

      

        private Hashtable setSampleBindings()
        {
            string rounding = "N4";
            rounding = Interface.IPreferences.CurrentSSFPref.Rounding;

            BindingSource bsSample = Interface.IBS.SubSamples;
            Hashtable samplebindings = null;


            SubSamplesDataTable SSamples = Interface.IDB.SubSamples;

            samplebindings = Dumb.ArrayOfBindings(ref bsSample, rounding);
            string column;
            //samples
            column = SSamples.RadiusColumn.ColumnName;
            this.radiusbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.radiusbox.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            column = SSamples.FillHeightColumn.ColumnName;
            this.lenghtbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.lenghtbox.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            column = SSamples.Gross1Column.ColumnName;
            Binding massbin = samplebindings[column] as Binding;
            this.massB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.massB.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            massbin.FormatString = "N2";
            samplebindings.Remove(massbin); //so it does not update its format!!!
            column = SSamples.SubSampleNameColumn.ColumnName;
            this.nameB.ComboBox.DataBindings.Add(samplebindings[column] as Binding);
            this.nameB.ComboBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            this.nameB.ComboBox.DisplayMember = column;
            this.nameB.ComboBox.DataSource = bsSample;
         
            this.nameB.AutoCompleteSource = AutoCompleteSource.ListItems;
            column = SSamples.VolColumn.ColumnName;
            this.volLbl.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.volLbl.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            column = SSamples.CalcDensityColumn.ColumnName;
            this.densityB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.densityB.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            return samplebindings;
        }

        private Hashtable setUnitBindings()
        {
            string rounding = "N4";
            rounding = Interface.IPreferences.CurrentSSFPref?.Rounding;

            string column;
            //units
            UnitDataTable Unit = Interface.IDB.Unit;
            BindingSource bs = Interface.IBS.Units; //link to binding source;
            Hashtable bindings = null;

            bindings = Dumb.ArrayOfBindings(ref bs, rounding);

            column = Unit.ChDiameterColumn.ColumnName;
            this.chdiamB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ChLengthColumn.ColumnName;
            this.chlenB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChCfgColumn.ColumnName;
            this.cfgB.ComboBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ContentColumn.ColumnName;
            this.matrixB.DataBindings.Add(bindings[column] as Binding);
            column = Unit.kepiColumn.ColumnName;
            this.kepiB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.kthColumn.ColumnName;
            this.kthB.TextBox.DataBindings.Add(bindings[column] as Binding);

            Binding renabled = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AARadiusColumn.ColumnName);
            this.radiusbox.TextBox.DataBindings.Add(renabled);
            Binding renabled2 = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName);
            this.lenghtbox.TextBox.DataBindings.Add(renabled2);
            Binding renabled3 = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.CalcDensityColumn.ColumnName);
            this.densityB.TextBox.DataBindings.Add(renabled3);
            Binding renabled4 = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.CalcMassColumn.ColumnName);
            this.massB.TextBox.DataBindings.Add(renabled4);

            return bindings;
        }

        public ucSSFData()
        {
            InitializeComponent();

        }
    }
}