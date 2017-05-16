using System;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

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
            Dumb.FD(ref this.VialBS);
     
            this.vialDGV.DataSource = Interface.IBS.Vial;  //= this.VialBS;
            this.VialBN.BindingSource = Interface.IBS.Vial;

            this.vialDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.bnVialAddItem.Click += this.addNewVialChannel_Click;//  new System.EventHandler(this.addNewVialChannel_Click);
        }

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                this.vialDGV.RowHeaderMouseDoubleClick += value; // new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);
            }
        }

        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>

        private void addNewVialChannel_Click(object sender, EventArgs e)
        {
            //IS A VIAL OR CONTAINER
            try
            {
                LINAA.VialTypeRow v = Interface.IDB.VialType.NewVialTypeRow();
                v.IsRabbit = true;
                Interface.IDB.VialType.AddVialTypeRow(v);
                Interface.IBS.Update(v);
            }
            catch (Exception ex)
            {

                Interface.IStore.AddException(ex);
            }
           
        }
    }
}