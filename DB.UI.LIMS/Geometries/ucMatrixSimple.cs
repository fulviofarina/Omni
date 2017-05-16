using System;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

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
            Dumb.FD(ref this.lINAA);
            Dumb.FD(ref this.MatrixBS);

            matrixDGV.DataSource = Interface.IBS.Matrix;
            this.matrixDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.MatrixBN.BindingSource = Interface.IBS.Matrix;

            string column = Interface.IDB.Matrix.MatrixNameColumn.ColumnName;
            Binding mlabel = BS.ABinding(ref Interface.IBS.Matrix, column);
            this.contentNameBox.TextBox.DataBindings.Add(mlabel);

            ucComposition1.Set(ref Interface, false);

            this.bindingNavigatorAddNewItem.Click += addNewVialChannel_Click;//  new System.EventHandler(this.addNewVialChannel_Click);

            // matrixDGV.MouseHover += dGV_MouseHover;
        }

        private void addNewVialChannel_Click(object sender, EventArgs e)
        {
            //IS A VIAL OR CONTAINER
            //   BindingSource bss = this.MatrixBN.BindingSource;

            try
            {

           
     
                LINAA.MatrixRow v = Interface.IDB.Matrix.NewMatrixRow();
                Interface.IDB.Matrix.AddMatrixRow(v);
                Interface.IBS.Update(v);
          
            }
            catch (Exception ex)
            {

                Interface.IStore.AddException(ex);
            }
        }


        public ucMatrixSimple()
        {
            InitializeComponent();
        }
    }
}