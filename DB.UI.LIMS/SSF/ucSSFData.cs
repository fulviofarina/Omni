using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFData : UserControl
    {
        // private bool cancelCalculations = false;
        private Interface Interface = null;


        public void AttachCtrl<T>(ref T pro)
        {
          
            if (pro.GetType().Equals(typeof(ucPreferences)))
            {
                IucPreferences pref = pro as IucPreferences;
                Interface.IDB.SSFPref.SampleChanged = delegate
                {
                    paintColumns();

                };
                EventHandler hdl = delegate
                 {
                     string add = " (EXPERT MODE ON) ";
                     if (Interface.IPreferences.CurrentSSFPref.Overrides)
                     {

                         this.NeutronSourceLBL.Text += add;
                         this.NeutronSourceLBL.ForeColor = Color.Red;
                         /*
                         this.BellLBL.Text += add;
                         this.nFactorLBL.Text += add;
                         this.WGtLBL.Text += add;
                         this.pEpilbl.Text += add;
                         this.kEpilbl.Text += add;
                         this.pThlbl.Text += add;
                         */
                     }
                     else
                     {
                         this.NeutronSourceLBL.Text = this.NeutronSourceLBL.Text.Replace(add, null);
                         this.NeutronSourceLBL.ForeColor = Color.LightCyan;
                         /*
                         this.BellLBL.Text = this.BellLBL.Text.Replace(add,null);
                         this.nFactorLBL.Text = this.nFactorLBL.Text.Replace(add, null);
                         this.WGtLBL.Text = this.WGtLBL.Text.Replace(add, null);
                         this.pEpilbl.Text = this.pEpilbl.Text.Replace(add, null);
                         this.kEpilbl.Text = this.kEpilbl.Text.Replace(add, null);
                         this.pThlbl.Text = this.pThlbl.Text.Replace(add, null);
                         */
                     }

                 };

                Interface.IDB.SSFPref.OverriderChanged = hdl;


                hdl.Invoke(null, EventArgs.Empty);

                pref.SetRoundingBinding(ref unitBindings);

                Interface.IDB.Preferences.RunInBackground = delegate
                  {
                      Application.RaiseIdle(EventArgs.Empty);
                  };
             

            }
          
        }

     
        Color[] arr = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.White };
        Color[] arr2 = new Color[] { Color.Yellow, Color.Black, Color.Black};

        private Hashtable unitBindings;
        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                Dumb.FD(ref this.SampleBS);

                sampleDGV.DataSource = Interface.IBS.SelectedSubSample;


        
                unitBindings = setUnitBindings();

                setEnabledBindings();

                DataGridViewColumn col = this.volDataGridViewTextBoxColumn;
                Rsx.DGV.Control.PaintColumn(true, ref col, arr, arr2);

                paintColumns();

                errorProvider1.DataMember = Interface.IDB.Unit.TableName;
                errorProvider1.DataSource = Interface.IBS.Units;
                errorProvider2.DataMember = Interface.IDB.SubSamples.TableName;
                errorProvider2.DataSource = Interface.IBS.SubSamples;

                
                Interface.IBS.PropertyChangedHandler += delegate
             {

             //    sampleDGV.Visible = Interface.IBS.SubSamples.Count != 0;
             };
                
                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void paintColumns()
        {


            bool readOnly = Interface.IPreferences.CurrentSSFPref.AAFillHeight;
            DataGridViewColumn columna = this.fillHeightDataGridViewTextBoxColumn;
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);

            // Interface.IPreferences.CurrentSSFPref.EndEdit();
            columna = this.gross1DataGridViewTextBoxColumn;
            readOnly = Interface.IPreferences.CurrentSSFPref.CalcMass;
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
          

            readOnly = Interface.IPreferences.CurrentSSFPref.AARadius;
            columna = this.radiusDataGridViewTextBoxColumn;
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);

            readOnly = Interface.IPreferences.CurrentSSFPref.CalcDensity;
            columna = this.calcDensityDataGridViewTextBoxColumn;
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);

           
          //  columna.DataGridView.CancelEdit();//.// columna.DataGridView[columna.Index, 0];
          
           
        }

       
    

        private void setEnabledBindings()
        {
            string column;

            BindingSource bs = Interface.IBS.SSFPreferences;


            column = Interface.IDB.SSFPref.DoCKColumn.ColumnName;
            Binding renabled5 = new Binding("Enabled", bs, column);
            this.kthB.TextBox.DataBindings.Add(renabled5);
         

            column = Interface.IDB.SSFPref.OverridesColumn.ColumnName;
            Binding other10 = new Binding("Enabled", bs, column);
            Binding other11 = new Binding("Enabled", bs, column);
            Binding other = new Binding("Enabled", bs, column);

            this.pEpiBox.TextBox.DataBindings.Add(other10);
            this.pThBox.TextBox.DataBindings.Add(other11);
            this.kepiB.TextBox.DataBindings.Add(other);

            column = Interface.IDB.SSFPref.DoMatSSFColumn.ColumnName;
            Binding other3 = new Binding("Enabled", bs, column);
          
            Binding other2 = new Binding("Enabled", bs, column);
            Binding renabled6 = new Binding("Enabled", bs, column);

            this.chlenB.TextBox.DataBindings.Add(renabled6);
            this.chdiamB.TextBox.DataBindings.Add(other2);
            this.cfgB.ComboBox.DataBindings.Add(other3);


            column = Interface.IDB.SSFPref.OverridesColumn.ColumnName;
            Binding other4 = new Binding("Enabled", bs, column);
            Binding other5 = new Binding("Enabled", bs, column);
            Binding other6 = new Binding("Enabled", bs, column);

            this.bellfactorBox.TextBox.DataBindings.Add(other4);
            this.WGtBox.TextBox.DataBindings.Add(other5);
            this.nFactorBox.TextBox.DataBindings.Add(other6);

            this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);
        }

      

        private Hashtable setUnitBindings()
        {
            string rounding = "N3";
            rounding = Interface.IPreferences.CurrentSSFPref?.Rounding;

            //units
            UnitDataTable Unit = Interface.IDB.Unit;
            BindingSource bs = Interface.IBS.Units; //link to binding source;
            Hashtable bindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, rounding);

            string column;
            column = Unit.ChRadiusColumn.ColumnName;
            this.chdiamB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChLengthColumn.ColumnName;
            this.chlenB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.BellFactorColumn.ColumnName;
            this.bellfactorBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.WGtColumn.ColumnName;
            this.WGtBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.nFactorColumn.ColumnName;
            this.nFactorBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChCfgColumn.ColumnName;
            this.cfgB.ComboBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.kepiColumn.ColumnName;
            this.kepiB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.kthColumn.ColumnName;
            this.kthB.TextBox.DataBindings.Add(bindings[column] as Binding);

        
            column = Interface.IDB.Unit.pThColumn.ColumnName;
            this.pThBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Interface.IDB.Unit.pEpiColumn.ColumnName;
            this.pEpiBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            return bindings;
        }

     
        public ucSSFData()
        {
            InitializeComponent();

          
        }

      
    }
}