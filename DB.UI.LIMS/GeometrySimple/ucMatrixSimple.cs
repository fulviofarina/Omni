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

        public void Set(ref Interface LinaaInterface, bool selected = false)
        {
            Interface = LinaaInterface;
            Dumb.FD(ref this.lINAA);
            Dumb.FD(ref this.MatrixBS);

            BindingSource bs = null;
            if (selected)
            {
               bs = Interface.IBS.SelectedMatrix;
                matrixDGV.RowHeadersVisible = false;
                MatrixBN.Visible = false;
            }
            else
            {
                bs = Interface.IBS.Matrix;
                matrixDGV.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            }

            this.MatrixBN.BindingSource = bs;
            matrixDGV.DataSource = bs;

            this.matrixDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
      
            string column = Interface.IDB.Matrix.MatrixNameColumn.ColumnName;
            Binding mlabel = BS.ABinding(ref bs, column);
            this.contentNameBox.TextBox.DataBindings.Add(mlabel);

            ucComposition1.Set(ref Interface, selected);

       //     this.bindingNavigatorAddNewItem.Click += addNewVialChannel_Click;//  new System.EventHandler(this.addNewVialChannel_Click);

            // matrixDGV.MouseHover += dGV_MouseHover;
        }

     


        public ucMatrixSimple()
        {
            InitializeComponent();
        }
    }
}