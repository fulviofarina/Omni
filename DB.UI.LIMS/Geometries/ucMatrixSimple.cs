using System.Windows.Forms;

using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucMatrixSimple : UserControl
    {
        private Interface Interface = null;

        public ucMatrixSimple()
        {
            InitializeComponent();
        }

        //   private DataGridViewCellMouseEventHandler rowHeaderMouseClick = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                // rowHeaderMouseClick = value;

                //  if (rowHeaderMouseClick != null) return;

                //   DataGridViewCellMouseEventHandler handler = value;
                //  rowHeaderMouseClick = handler;

                this.matrixDGV.RowHeaderMouseDoubleClick += value;
            }
        }

        /// <summary>
        /// Adds a new row of type VialType or Channels, which is either a Rabbit/Vial or a Channel configuration
        /// </summary>
        /// <param name="sender">The Add button that was clicked</param>
        /// <param name="e"></param>
        ///

        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void RefreshMatrix()
        {
            LINAA.UnitRow u = MatSSF.UNIT;

            int id = u.SubSamplesRow.MatrixID;
            string column = Interface.IDB.Matrix.MatrixIDColumn.ColumnName;

            Interface.IBS.Matrix.Position = Interface.IBS.Matrix.Find(column, id);
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;
            Dumb.FD<LINAA>(ref this.lINAA);
            this.lINAA = Interface.Get() as LINAA;

            BindingSource bs = this.MatrixBS;

            Dumb.LinkBS(ref bs, Interface.IDB.Matrix);

            Interface.IBS.Matrix = bs;

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            bool t = true;
            string text = "Text";
            string column;

            column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;
            Binding mcompoBin = new Binding(text, bs, column, t, mo);

            this.matrixRTB.DataBindings.Add(mcompoBin);

            // DataGridViewCellMouseEventHandler handler = this.matrixDGV.RowHeaderMouseClick;
        }
    }
}