using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucSSFData : UserControl
    {
        protected Color[] arr = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.White };

        protected Color[] arr2 = new Color[] { Color.Yellow, Color.Black, Color.Black };

        // private bool cancelCalculations = false;
        protected internal Interface Interface = null;

        protected static string EXPERT_MODE_ON_MSG = "EXPERT MODE was activated:\n\n"
              + "You are now allowed to alter all the default, fundamental paramaters of the calculation methods.\n\n"
     + "Keep in mind that changing the fundamental values of each method was not intended by their creators, "
      + "therefore the corresponding results are marked with an (*) symbol and there is no guarantee on their " +
      "accuracy nor validity.\n\n"
      + "You can return to the DEFAULT MODE at any time.\n\n";

        protected static string EXPERT_MODE_ON_TITLE = "EXPERT MODE ACTIVATED";
        protected internal Hashtable unitBindings;
        protected static string BINDING_MSG = "Data Controls were loaded!";
        protected static string BINDING_OK = "Bindings OK";
        Hashtable bindings = null;
        public void AttachCtrl<T>(ref T pro)
        {
            if (pro.GetType().Equals(typeof(ucPreferences)))
            {
                IucPreferences pref = pro as IucPreferences;

                linkColumns();

                EventHandler hdl = delegate
                {
                    this.sampleDGV.Invalidate();
                    Application.DoEvents();
                };
                pref.CalcMassChanged += hdl;
                pref.CalcDensityChanged += hdl;
                pref.CalcRadiusChanged += hdl;
                pref.CalcLengthChanged += hdl;

                //TODO: Bidnings
                pref.SetRoundingBinding(ref bindings);

                EventHandler popup =
delegate
{
    if (Interface.IPreferences.CurrentSSFPref.Overrides)
    {
        MessageBox.Show(EXPERT_MODE_ON_MSG, EXPERT_MODE_ON_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Interface.IReport.Msg(EXPERT_MODE_ON_MSG, EXPERT_MODE_ON_TITLE);
    }
};
                pref.OverriderChanged += popup;

             ucSSFDataNS1.AttachCtrl(ref pro);
            }
        }

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                Dumb.FD(ref this.SampleBS);

                bindings = ucSSFDataNS1.Set(ref Interface);

                sampleDGV.DataSource = Interface.IBS.SelectedSubSample;

                DataGridViewColumn col = this.volDataGridViewTextBoxColumn;
                Rsx.DGV.Control.PaintColumn(true, ref col, arr, arr2);

             //   paintColumns();

                errorProvider2.DataMember = Interface.IDB.SubSamples.TableName;
                errorProvider2.DataSource = Interface.IBS.SubSamples;

                Interface.IReport.Msg(BINDING_MSG, BINDING_OK);
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void linkColumns()
        {
            LINAA.SSFPrefRow pref = Interface.IPreferences.CurrentSSFPref;
        
            SampleColumn columna = this.fillHeightDataGridViewTextBoxColumn;
      
            columna.BindingPreferenceField = Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName;
            columna.BindingPreferenceRow = pref;

            columna = this.gross1DataGridViewTextBoxColumn;
            columna.BindingPreferenceField = Interface.IDB.SSFPref.CalcMassColumn.ColumnName;
            columna.BindingPreferenceRow = pref;


            columna = this.radiusDataGridViewTextBoxColumn;
            columna.BindingPreferenceField = Interface.IDB.SSFPref.AARadiusColumn.ColumnName;
            columna.BindingPreferenceRow = pref;



            columna = this.calcDensityDataGridViewTextBoxColumn;
            columna.BindingPreferenceField = Interface.IDB.SSFPref.CalcDensityColumn.ColumnName;
            columna.BindingPreferenceRow = pref;


      
            //  Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
        }
        /*
        protected internal void paintColumns()
        {
            bool readOnly = Interface.IPreferences.CurrentSSFPref.AAFillHeight;
            DataGridViewColumn columna = this.fillHeightDataGridViewTextBoxColumn;
            //     Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
            columna.ReadOnly = readOnly;

            columna = this.gross1DataGridViewTextBoxColumn;
            readOnly = Interface.IPreferences.CurrentSSFPref.CalcMass;
            //   Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
            columna.ReadOnly = readOnly;

            readOnly = Interface.IPreferences.CurrentSSFPref.AARadius;
            columna = this.radiusDataGridViewTextBoxColumn;
            //  Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
            columna.ReadOnly = readOnly;

            readOnly = Interface.IPreferences.CurrentSSFPref.CalcDensity;
            columna = this.calcDensityDataGridViewTextBoxColumn;
            columna.ReadOnly = readOnly;

            this.sampleDGV.Invalidate();
            //  Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
        }
*/
        public ucSSFData()
        {
            InitializeComponent();
        }
    }
}