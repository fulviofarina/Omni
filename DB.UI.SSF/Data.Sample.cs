using System;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucSSFDataSample : UserControl, IDataItem
    {
        // private bool cancelCalculations = false;
        protected internal Interface Interface = null;

        protected static string BINDING_MSG = "Data Controls were loaded!";

        protected static string BINDING_OK = "Bindings OK";

        protected static string EXPERT_MODE_ON_MSG = "EXPERT MODE was activated:\n\n"
                              + "You are now allowed to alter all the default, fundamental paramaters of the calculation methods.\n\n"
     + "Keep in mind that changing the fundamental values of each method was not intended by their creators, "
      + "therefore the corresponding results are marked with an (*) symbol and there is no guarantee on their " +
      "accuracy nor validity.\n\n"
      + "You can return to the DEFAULT MODE at any time.\n\n";

        protected static string EXPERT_MODE_ON_TITLE = "EXPERT MODE ACTIVATED";
        // protected internal Hashtable unitBindings;

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter, ref IPreferences pref)
        {
            Interface = inter;

            try
            {
                Dumb.FD(ref this.SampleBS);

                sampleDGV.DataSource = Interface.IBS.SelectedSubSample;

                Color[] arr = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.White };
                Color[] arr2 = new Color[] { Color.Yellow, Color.Black, Color.Black };
                DataGridViewColumn col = this.volDataGridViewTextBoxColumn;
                Rsx.DGV.Control.PaintColumn(true, ref col, arr, arr2);

                linkColumns();

                EventHandler cleanDGV = delegate
                {
                    this.sampleDGV.Invalidate();
                };
                pref.ISSF.CalcMassChanged += cleanDGV;
                pref.ISSF.CalcDensityChanged += cleanDGV;
                pref.ISSF.CalcRadiusChanged += cleanDGV;
                pref.ISSF.CalcLengthChanged += cleanDGV;

                //TODO: Bidnings
                //    pref.SetRoundingBinding(ref bindings);

                EventHandler popup = delegate
                {
                    popupMessage();
                };
                pref.ISSF.OverriderChanged += popup;

                pref.IMain.RunInBackground += delegate
                {
                    Application.RaiseIdle(EventArgs.Empty);
                };

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

            ISampleColumn columna = null;

            columna = this.fillHeightDataGridViewTextBoxColumn;
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
        }

        private void popupMessage()
        {
            if (Interface.IPreferences.CurrentSSFPref.Overrides)
            {
                MessageBox.Show(EXPERT_MODE_ON_MSG, EXPERT_MODE_ON_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Interface.IReport.Msg(EXPERT_MODE_ON_MSG, EXPERT_MODE_ON_TITLE);
            }
        }

        public ucSSFDataSample()
        {
            InitializeComponent();
        }
    }
}