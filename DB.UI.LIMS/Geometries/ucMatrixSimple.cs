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
            bool isMatrix = this.MatrixBN.BindingSource.Equals(Interface.IBS.Matrix);

            if (isMatrix)
            {
                LINAA.MatrixRow v = Interface.IDB.Matrix.NewMatrixRow();
                Interface.IDB.Matrix.AddMatrixRow(v);
                Interface.IBS.Update(v);
            }
            else
            {
                LINAA.CompositionsRow c = Interface.IDB.Compositions.NewCompositionsRow();
                Interface.IDB.Compositions.AddCompositionsRow(c);
                c.MatrixID = (Interface.ICurrent.Matrix as LINAA.MatrixRow).MatrixID;
            }
        }

        /*
        private void dGV_MouseHover(object sender, EventArgs e)
        {
            if (sender.Equals(this.matrixDGV)) this.MatrixBN.BindingSource = Interface.IBS.Matrix;
            else this.MatrixBN.BindingSource = Interface.IBS.Compositions;
        }
        */

        public ucMatrixSimple()
        {
            InitializeComponent();
        }
    }
}