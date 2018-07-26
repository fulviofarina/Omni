using DB.Tools;
using Rsx.DGV;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucMUES : UserControl
    {
        /// <summary>
        /// not used
        /// </summary>
        /// <param name="m"></param>
        private void getDataToPlot(ref MatrixRow m)
        {
            int matrixID = m.MatrixID;
            string name;
            double density;
            name = m.MatrixName;
            density = m.MatrixDensity;

            string title = string.Empty;

            double eh;//// = pref.EndEnergy;
            double el;// = pref.StartEnergy;
                      // bool logScale;
            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;
            eh = pref.EndEnergy;
            el = pref.StartEnergy;
            // logScale = pref.LogGraph;
            bool sql = true;
            MUESDataTable mues = null;
            // if (specific) mues = Interface.IPopulate.IGeometry.GetMUES(el, eh, matrixID);
            mues = Interface.IPopulate.IGeometry.GetMUES(ref m, sql);

            DataColumn ene = mues.EnergyColumn;
            DataColumn mu = mues.MATNCSColumn;

            this.grapher.SetGraph(ref ene, 1, true, 10, ref mu, 1, true, 10, title);
            this.grapher.Refresh();
            DataTable dt = ene.Table;
            Dumb.FD(ref dt);
        }
      
        protected internal Interface Interface = null;

        public void Set( ref IXCOMPreferences pref)
        {

            IXCOMPreferences preference = pref;

            try
            {
                bindDGVColumns();

                bindPreference(preference);

                refreshDGV();

                focus(true);

                grapher.Name = "graph";
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void bindPreference(IXCOMPreferences preference)
        {
            EventHandler action = delegate
            {
                refreshDGV();
            };

            preference.RoundingChanged += action;

            this.Disposed += delegate
            {
                preference.RoundingChanged -= action;
            };
        }

        public void Set(ref Interface inter)
        {
            destroy();

            Interface = inter;

          
        }


        private void destroy()
        {
            this.DGV.DataSource = null;
            Dumb.FD(ref this.bs);
            Dumb.FD(ref this.Linaa);
        }

        private void refreshDGV()
        {
            DGV.Visible = false;
            IEnumerable<MUESColumn> cols = DGV.Columns.OfType<MUESColumn>();
            foreach (MUESColumn c in cols)
            {
                if (!c.Visible && c != this.mUDataGridViewTextBoxColumn) c.Visible = true;
                c.SetRounding();
            }
            DGV.Visible = true;
        }

        public void MakeFile(string matrixID, string path)
        {
            DataGridView dgv = this.DGV;

            Rsx.DGV.Control.MakeHTMLFile(path, matrixID, ref dgv);
            Rsx.DGV.Control.MakeCSVFile(path, matrixID, ref dgv);

          
        }

        private void bindDGVColumns()
        {

            this.DGV.BackgroundColor = System.Drawing.Color.Thistle;
            this.DGV.DataSource = Interface.IBS.MUES;
            this.DGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;

            IBindableDGVColumn col = null;

            col = this.mACSDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.CSColumn.ColumnName;

            col = this.mAISDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.ISColumn.ColumnName;

            col = this.pEDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.PEColumn.ColumnName;

            col = this.pPEFDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.PPEFColumn.ColumnName;

            col = this.pPNFDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.PPNFColumn.ColumnName;

            col = this.mATNCSDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.TNCSColumn.ColumnName;

            col = this.mATCSDataGridViewTextBoxColumn;
            col.BindingPreferenceField = Interface.IDB.XCOMPref.TCSColumn.ColumnName;

            IEnumerable<MUESColumn> cols = DGV.Columns.OfType<MUESColumn>();
            foreach (MUESColumn c in cols)
            {
                c.BindingRoundingField = Interface.IDB.XCOMPref.RoundingColumn.ColumnName;
                c.BindingPreferenceRow = pref;
            }
        }

        private void focus(bool table)
        {
            SC.Panel2Collapsed = table;
            SC.Panel1Collapsed = !table;
        }


        public ucMUES()
        {
            InitializeComponent();
        }
    }
}