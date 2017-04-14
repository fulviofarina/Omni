using System;
using System.Windows.Forms;

using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucMatrixSimple : UserControl
    {
        private Interface Interface = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {

                this.matrixDGV.RowHeaderMouseDoubleClick += value;
            }
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;
            Dumb.FD<LINAA>(ref this.lINAA);
            Dumb.FD(ref this.MatrixBS);
            //      this.lINAA = Interface.Get() as LINAA;


            matrixDGV.DataSource = Interface.IBS.Matrix;
        //    BindingSource bs = this.MatrixBS;

        //    Dumb.LinkBS(ref bs, Interface.IDB.Matrix);

       //     Interface.IBS.Matrix = bs;

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            bool t = true;
            string text = "Text";
            string column;

            column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;
            Binding mcompoBin = new Binding(text, Interface.IBS.Matrix, column, t, mo);

            this.matrixRTB.DataBindings.Add(mcompoBin);

            System.EventHandler addNew = this.addNewVialChannel_Click;

            this.bindingNavigatorAddNewItem.Click += addNew;//  new System.EventHandler(this.addNewVialChannel_Click);


            // DataGridViewCellMouseEventHandler handler = this.matrixDGV.RowHeaderMouseClick;
        }

        private void addNewVialChannel_Click(object sender, EventArgs e)
        {
            //IS A VIAL OR CONTAINER
            LINAA.MatrixRow v = Interface.IDB.Matrix.NewMatrixRow();
            Interface.IDB.Matrix.AddMatrixRow(v);
            Interface.IBS.Update(v);
        }

        public ucMatrixSimple()
        {
            InitializeComponent();
        }
    }
}