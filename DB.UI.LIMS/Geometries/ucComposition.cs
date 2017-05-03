using System;
using System.Windows.Forms;

using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucComposition : UserControl
    {
        private bool advanced = true;
        private Interface Interface = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>

        public void Set(ref Interface LinaaInterface, bool selectedMatrix = true)
        {
            Interface = LinaaInterface;

            try
            {
                Dumb.FD(ref this.Linaa);
                Dumb.FD(ref this.BS);

                this.compositionDGV.DataSource = Interface.IBS.Compositions;

                BindingSource bs = Interface.IBS.SelectedMatrix;
                if (!selectedMatrix) bs = Interface.IBS.Matrix;

                string column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;
             
                Binding mcompoBin=   Dumb.ABinding(ref bs, column);

                this.matrixRTB.DataBindings.Add(mcompoBin);

                this.matrixRTB.MouseLeave += focus;
                this.compositionDGV.MouseHover += focus;

                focus(null, EventArgs.Empty);
            }
            catch (System.Exception ex)
            {
                Interface.IMain.AddException(ex);
            }
        }

        private void focus(object sender, EventArgs e)
        {
            SC.Panel2Collapsed = advanced;
            SC.Panel1Collapsed = !advanced;

            advanced = !advanced;
        }

        public ucComposition()
        {
            InitializeComponent();
        }
    }
}