using DB.Tools;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFDataNS : UserControl, IDataItem
    {
        protected internal Interface Interface = null;

        protected static string add = " (EXPERT MODE ON) ";
        protected static string sourceVARIABLES = "NEUTRON SOURCE VARIABLES";

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter, ref IPreferences pref)
        {
            Interface = inter;
            try
            {
                setEnabledBindings();
                Hashtable unitBindings;

                unitBindings = setUnitBindings();

                // IPreferences pref = pro as IPreferences;
                EventHandler overrider = delegate
                {
                    refreshNSLabel();
                };
                pref.ISSF.OverriderChanged += overrider;
                //invoke
                overrider.Invoke(null, EventArgs.Empty);
                //refreshNSLabel();

                errorProvider1.DataMember = Interface.IDB.Unit.TableName;
                errorProvider1.DataSource = Interface.IBS.Units;

                Interface.IReport.Msg("Neutron Source Control OK", "Control loaded");
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void refreshNSLabel()
        {
            if (Interface.IPreferences.CurrentSSFPref.Overrides)
            {
                this.NeutronSourceLBL.Text = sourceVARIABLES + add;
                this.NeutronSourceLBL.ForeColor = Color.Red;
            }
            else
            {
                this.NeutronSourceLBL.Text = sourceVARIABLES;
                this.NeutronSourceLBL.ForeColor = Color.LightCyan;
            }
        }

        protected internal void setEnabledBindings()
        {
            string column;

            BindingSource bs = Interface.IBS.SSFPreferences;
            column = Interface.IDB.SSFPref.DoCKColumn.ColumnName;
            Binding renabled5 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");
            this.kthB.TextBox.DataBindings.Add(renabled5);

            column = Interface.IDB.SSFPref.OverridesColumn.ColumnName;
            Binding other10 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");
            Binding other11 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");
            Binding other = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");

            this.pEpiBox.TextBox.DataBindings.Add(other10);
            this.pThBox.TextBox.DataBindings.Add(other11);
            this.kepiB.TextBox.DataBindings.Add(other);

            column = Interface.IDB.SSFPref.DoMatSSFColumn.ColumnName;
            Binding other3 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");

            Binding other2 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");
            Binding renabled6 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");

            this.chlenB.TextBox.DataBindings.Add(renabled6);
            this.chdiamB.TextBox.DataBindings.Add(other2);
            this.cfgB.ComboBox.DataBindings.Add(other3);

            column = Interface.IDB.SSFPref.OverridesColumn.ColumnName;
            Binding other4 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");
            Binding other5 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");
            Binding other6 = Rsx.Dumb.BS.ABinding(ref bs, column, string.Empty, "Enabled");

            this.bellfactorBox.TextBox.DataBindings.Add(other4);
            this.WGtBox.TextBox.DataBindings.Add(other5);
            this.nFactorBox.TextBox.DataBindings.Add(other6);

            this.cfgB.ComboBox.Items.AddRange(Interface.IDB.MatSSFTYPES);
        }

        protected internal Hashtable setUnitBindings()
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

        public ucSSFDataNS()
        {
            InitializeComponent();
        }
    }
}