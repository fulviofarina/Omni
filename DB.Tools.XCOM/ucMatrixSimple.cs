using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucMatrixSimple : UserControl
    {
        protected internal Interface Interface = null;
        protected static string EDIT_STR = "EDIT";
        protected static string VIEW_STR = "VIEW";

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
            matrixDGV.DataSource = null;
            this.MatrixBN.BindingSource = null;

            Dumb.FD(ref this.lINAA);
            Dumb.FD(ref this.MatrixBS);

            BindingSource bs = null;
            if (selected)
            {
                bs = Interface.IBS.SelectedMatrix;
                matrixDGV.RowHeadersVisible = false;
                MatrixBN.AddNewItem.Visible = false;
            }
            else
            {
                bs = Interface.IBS.Matrix;
                matrixDGV.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            }

            this.MatrixBN.BindingSource = bs;
            matrixDGV.DataSource = bs;

            this.matrixDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

            this.matrixDGV.DataError += MatrixDGV_DataError;

            string column = Interface.IDB.Matrix.MatrixNameColumn.ColumnName;
            Binding mlabel = Rsx.Dumb.BS.ABinding(ref bs, column);
            this.contentNameBox.TextBox.DataBindings.Add(mlabel);

            ucComposition1.Set(ref Interface, selected);

            // this.bindingNavigatorAddNewItem.Click += addNewVialChannel_Click;// new System.EventHandler(this.addNewVialChannel_Click);

            // matrixDGV.MouseHover += dGV_MouseHover;
        }

        private void MatrixDGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {


          //  throw new System.NotImplementedException();
        }

        public ucMatrixSimple()
        {
            InitializeComponent();
            editContentLBL.Text = VIEW_STR;

            editContentLBL.Click += delegate
            {
                ChangeCompositionView();
            };
        }

        public void ChangeCompositionView()
        {
            if (editContentLBL.Text.Contains(EDIT_STR))
            {
                editContentLBL.Text = VIEW_STR;
            }
            else
            {
                editContentLBL.Text = EDIT_STR;
            }
            ucComposition1.ChangeFocus(null, System.EventArgs.Empty);
        }

    }
}