using System;
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
                this.ToDo.Visible = false;
                this.matrixDensityDataGridViewTextBoxColumn1.Visible = true;
                this.matrixDGV.RowHeadersVisible = true;
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

            setBindings(selected);

            ucComposition1.Set(ref Interface, selected);


            //    this.matrixDGV.DataError += MatrixDGV_DataError;

            this.matrixDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;


        }

        private void setBindings(bool selected)
        {
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

            this.matrixDGV.DataSource = bs;

            string column = Interface.IDB.Matrix.MatrixNameColumn.ColumnName;
            Binding mlabel = Rsx.Dumb.BS.ABinding(ref bs, column);
            this.contentNameBox.TextBox.DataBindings.Add(mlabel);
        }

        public ucMatrixSimple()
        {
            InitializeComponent();

        //    this.ToDo.Visible = false;
            this.matrixDensityDataGridViewTextBoxColumn1.Visible = false;

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

        public void RefreshDGV()
        {
            matrixDGV.ClearSelection();
        }
    }
}