
using Rsx.Dumb;

using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucMatrixSimple : UserControl
    {
        protected internal Interface Interface = null;

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

            ucComposition1.ChangeFocus();

            this.matrixDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.matrixDGV.Refresh();
        }

        protected internal void destroy()
        {
            matrixDGV.DataSource = null;
            this.MatrixBN.BindingSource = null;
            Dumb.FD(ref this.MatrixBS);
            Dumb.FD(ref this.lINAA);
        }

        protected internal void setBindings(bool selected)
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

        

        this.noneAll.Click+= delegate
        {
            string columnname = Interface.IDB.Matrix.ToDoColumn.ColumnName;
            string tablname = Interface.IDB.Matrix.TableName;

            Interface.IBS.SelectNoneAllToDo(tablname, columnname);

        };


            this.matrixDGV.DataSource = bs;

            string column = Interface.IDB.Matrix.MatrixNameColumn.ColumnName;
            Binding mlabel = Rsx.Dumb.BS.ABinding(ref bs, column);
            this.contentNameBox.TextBox.DataBindings.Add(mlabel);

            Interface.IBS.EnableControlsChanged += delegate
              {
                  bool ctrlcanBeenable = Interface.IBS.EnabledControls;
                  this.Enabled = ctrlcanBeenable;

                  this.matrixDGV.Invalidate(true);
                  this.matrixDGV.Refresh();
                  this.matrixDGV.ClearSelection();
              };

           
        }

     
                    

        public ucMatrixSimple()
        {
            InitializeComponent();

            // this.ToDo.Visible = false;
            this.matrixDensityDataGridViewTextBoxColumn1.Visible = false;

            ucComposition1.ChangeFocusCallBack += delegate
            {
                editContentLBL.Text = ucComposition1.ViewString;
            };

            editContentLBL.Click += delegate
            {
                ucComposition1.ChangeFocus();
            };
        }

       
    }
}