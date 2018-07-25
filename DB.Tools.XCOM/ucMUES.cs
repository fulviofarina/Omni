using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using DB.Tools;
using Rsx.Dumb;
using static DB.LINAA;
using System.Collections.Generic;

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
         //   bool logScale;
            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;
            eh = pref.EndEnergy;
            el = pref.StartEnergy;
         //   logScale = pref.LogGraph;
            bool sql = true;
            MUESDataTable mues = null;
            //   if (specific) mues = Interface.IPopulate.IGeometry.GetMUES(el, eh, matrixID);
            mues = Interface.IPopulate.IGeometry.GetMUES(ref m, sql);

            DataColumn ene = mues.EnergyColumn;
            DataColumn mu = mues.MATNCSColumn;

            this.grapher.SetGraph(ref ene, 1, true, 10, ref mu, 1, true, 10, title);
            this.grapher.Refresh();
            DataTable dt = ene.Table;
            Dumb.FD(ref dt);
        }

      

      //  protected internal bool advanced = true;
        protected internal Interface Interface = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>

        public void Set(ref Interface LinaaInterface, ref IXCOMPreferences pref)
        {
            try
            {
                Dumb.FD(ref this.Linaa);
                this.DGV.DataSource = null;
                Dumb.FD(ref this.bs);

                Interface = LinaaInterface;
                // BindingSource bsCompositions = null;

                this.DGV.BackgroundColor = System.Drawing.Color.Thistle;

                // bsCompositions = Interface.IBS.MUES;

                // this.compositionsDGV.DataMember = string.Empty;
                this.DGV.DataSource = Interface.IBS.MUES;
                //  this.compositionsDGV.RowHeadersVisible = false;
                bindDGVColumns();
                this.DGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

                //  compositionsDGV

                //  FormClosingEventHandler closing = delegate
                //  {
                //     refreshDGV();
                //  };

                //  pref.Parent.FormClosing += closing;

                EventHandler action = delegate
                {
                    // string rounding2 = Interface.IPreferences.CurrentXCOMPref.Rounding;
                    //setDGVRounding(rounding2);
                    refreshDGV();
                };

                pref.RoundingChanged += action;

                action.Invoke(null, EventArgs.Empty);

                Focus(true);

                grapher.Name = "graph";


            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void refreshDGV()
        {

            IEnumerable<MUESColumn> cols = DGV.Columns.OfType<MUESColumn>();
            foreach (MUESColumn c in cols)
            {
                if (!c.Visible && c!=this.mUDataGridViewTextBoxColumn) c.Visible = true;
                c.SetRounding();
            }
        }

        internal void MakeFile(string matrixID, string path)
        {
         

            DataGridView dgv = this.DGV;
       //    this.matrixIDDataGridViewTextBoxColumn1.Visible = true;

            Rsx.DGV.Control.MakeHTMLFile(path, matrixID, ref dgv);
            Rsx.DGV.Control.MakeCSVFile(path, matrixID, ref dgv);

            //   this.matrixIDDataGridViewTextBoxColumn.Visible = false;

        }

        private void bindDGVColumns()
        {

           LINAA.XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;

            Rsx.DGV.IBindableDGVColumn col = null;

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


            IEnumerable<MUESColumn> cols =  DGV.Columns.OfType<MUESColumn>();
            foreach (MUESColumn c in cols)
            {
                c.BindingRoundingField = Interface.IDB.XCOMPref.RoundingColumn.ColumnName;
                c.BindingPreferenceRow = pref;//as DataRow;
            }
        }


        public void Focus(bool table)
        {
            SC.Panel2Collapsed = table;
            SC.Panel1Collapsed = !table;
    
        }

        public void PrintDGV(string file)
        {
            try
            {
                DataTable mu = Rsx.Dumb.Tables.DGVToTable(this.DGV);

                mu.WriteXml(file, XmlWriteMode.IgnoreSchema);
            }
            catch (Exception ex)
            {

               
            }
         

        }

        public ucMUES()
        {
            InitializeComponent();
        }
   
       
      
    }
}