using DB.Tools;
using Rsx.Dumb;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucV : UserControl
    {
        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        protected internal Interface Interface = null;

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
            // this.bnVialAddItem.Click += this.addNewVialChannel_Click;// new System.EventHandler(this.addNewVialChannel_Click);
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
    }
}