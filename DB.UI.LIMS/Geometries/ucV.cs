using System;
using System.Data;
using System.Windows.Forms;
using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucV : UserControl
    {
        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        private Interface Interface = null;

        public ucV()
        {
            InitializeComponent();
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;
            Dumb.FD<LINAA>(ref this.lINAA);
            this.lINAA = Interface.Get() as LINAA;

            //rabbit column
            string column = this.lINAA.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = this.lINAA.VialType.InnerRadiusColumn.ColumnName + " asc";
            Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);

            Interface.IBS.Vial = this.VialBS;

            System.EventHandler addNew = this.addNewVialChannel_Click;

            this.bnVialAddItem.Click += addNew;//  new System.EventHandler(this.addNewVialChannel_Click);
        }

        public void RefreshVCC()
        {
            LINAA.UnitRow u = MatSSF.UNIT;
            string column;
            column = Interface.IDB.VialType.VialTypeIDColumn.ColumnName;
            int id = u.SubSamplesRow.VialTypeRow.VialTypeID;
            BindingSource bs = Interface.IBS.Vial;
            bs.Position = bs.Find(column, id);
        }

        //  private DataGridViewCellMouseEventHandler rowHeaderMouseClick = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                //      if (rowHeaderMouseClick != null) return;

                //  DataGridViewCellMouseEventHandler handler = value;
                //   rowHeaderMouseClick = handler;

                //  this.unitDGV.RowHeaderMouseClick += handler;

                this.vialDGV.RowHeaderMouseDoubleClick += value; // new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);
            }
        }

        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void addNewVialChannel_Click(object sender, EventArgs e)
        {
            BindingSource bs = null;
            string colName = string.Empty;
            object idnumber = null;
            DataRow row = null;

            LINAA.VialTypeRow v = Interface.IDB.VialType.NewVialTypeRow();
            v.IsRabbit = false;
            Interface.IDB.VialType.AddVialTypeRow(v);

            colName = Interface.IDB.VialType.VialTypeIDColumn.ColumnName;
            bs = Interface.IBS.Vial;
            idnumber = v.VialTypeID;

            row = v;
            if (row.HasErrors)
            {
                string rowWithError = DB.UI.Properties.Resources.rowWithError;
                string Error = DB.UI.Properties.Resources.Error;

                Interface.IReport.Msg(rowWithError, Error);
            }

            int newIndex = bs.Find(colName, idnumber);
            bs.Position = newIndex;
        }
    }
}