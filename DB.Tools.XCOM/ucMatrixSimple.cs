using DB.Tools;
using Rsx.Dumb;
using System.Windows.Forms;
using System;

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
            set
            {
                this.ToDo.Visible = false;
                this.matrixDensityDataGridViewTextBoxColumn1.Visible = true;
                this.matrixDGV.RowHeadersVisible = true;
                this.matrixDGV.RowHeaderMouseDoubleClick += value;
            }
        }

        public void Set(ref Interface inter, bool selected = false)
        {
            Interface = inter;

            destroy();

            setBindings(selected);

            ucComposition1.Set(ref Interface, selected);

            changeCompositionView();

            this.matrixDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.matrixDGV.Refresh();


        }

        private void destroy()
        {
            matrixDGV.DataSource = null;
            this.MatrixBN.BindingSource = null;
            Dumb.FD(ref this.MatrixBS);
            Dumb.FD(ref this.lINAA);
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

            Interface.IBS.EnableControlsChanged += delegate
              {
                  this.matrixDGV.Invalidate(true);
                  this.matrixDGV.Refresh();
                  this.matrixDGV.ClearSelection();
              };

            /*
            if (bs.Count != 0)
            {
                bs.Position = 1;
                bs.Position = 0;
            }

            */
        }

        public ucMatrixSimple()
        {
            InitializeComponent();

            // this.ToDo.Visible = false;
            this.matrixDensityDataGridViewTextBoxColumn1.Visible = false;

            editContentLBL.Text = VIEW_STR;

            editContentLBL.Click += delegate
            {
                changeCompositionView();
            };
        }

        private void changeCompositionView()
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