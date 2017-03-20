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

            Dumb.FD<LINAA>(ref this.lINAA);

            System.EventHandler addNew = this.addNewVialChannel_Click;

            this.bnVialAddItem.Click += addNew;//  new System.EventHandler(this.addNewVialChannel_Click);
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;
            this.lINAA = Interface.Get() as LINAA;

            //rabbit column
            string column = this.lINAA.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = this.lINAA.VialType.InnerRadiusColumn.ColumnName + " asc";
            Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
        }

        public void RefreshVCC()
        {
            LINAA.UnitRow u = MatSSF.UNIT;
            string column;
            column = this.lINAA.VialType.VialTypeIDColumn.ColumnName;
            int id = u.SubSamplesRow.VialTypeRow.VialTypeID;
            this.VialBS.Position = this.VialBS.Find(column, id);
        }

        public void EndEdit()
        {
            this.VialBS.EndEdit();
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

                this.vialDGV.RowHeaderMouseClick += value; // new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);
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
            //IS A VIAL OR CONTAINER

            //     bool isRabbit = !sender.Equals(this.bnVialAddItem);

            LINAA.VialTypeRow v = this.lINAA.VialType.NewVialTypeRow();
            v.IsRabbit = false;
            this.lINAA.VialType.AddVialTypeRow(v);

            colName = this.lINAA.VialType.VialTypeIDColumn.ColumnName;
            bs = this.VialBS;
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