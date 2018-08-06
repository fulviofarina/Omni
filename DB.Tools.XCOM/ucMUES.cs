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
        protected internal Interface Interface = null;

        public void MakeFile(string name, string path)
        {

            DataGridView dgv = this.DGV;

            Rsx.DGV.Control.MakeHTMLFile(path, name, ref dgv, ".xls");

            Rsx.DGV.Control.MakeCSVFile(path, name, ref dgv);
        }

        public void Set(ref IXCOMPreferences pref, bool table = true)
        {

            try
            {

                bindPreference( pref);
            

                EventHandler enabledHandler = delegate
                {
                    bool ctrlcanBeenable = Interface.IBS.EnabledControls;
                    this.Enabled = ctrlcanBeenable;
            



                };
                Interface.IBS.EnableControlsChanged += enabledHandler;

                this.Disposed += delegate
                {
                    Interface.IBS.EnableControlsChanged -= enabledHandler;
                };

                refreshDGV(null, EventArgs.Empty);

      
            SC.Panel2Collapsed = table;
            SC.Panel1Collapsed = !table;

    }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        public void Set(ref Interface inter)
        {
            destroy();
            Interface = inter;
            grapher.Name = "graph";
            bindDGVColumns();
        }

        private void bindDGVColumns()
        {

            this.DGV.BackgroundColor = System.Drawing.Color.FromArgb(255, 35, 35, 35);
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

        /// <summary>
        /// ok
        /// </summary>
        /// <param name="preference"></param>
        private void bindPreference(IXCOMPreferences preference)
        {

            preference.RoundingChanged += refreshDGV;

            this.Disposed += delegate
            {
                preference.RoundingChanged -= refreshDGV;
            };
        }

        /// <summary>
        /// ok
        /// </summary>
        private void destroy()
        {
            this.DGV.DataSource = null;
            Dumb.FD(ref this.bs);
            Dumb.FD(ref this.Linaa);
        }

           

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

        private void refreshDGV(object sender, EventArgs e)
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
        public ucMUES()
        {
            InitializeComponent();
        }
    }
}