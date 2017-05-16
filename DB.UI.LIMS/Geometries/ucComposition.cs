using System;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucComposition : UserControl
    {
        private bool advanced = true;
        private Interface Interface = null;
        BindingSource bsCompositions = null;
        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>

        public void Set(ref Interface LinaaInterface, bool selectedMatrix = true)
        {
            Interface = LinaaInterface;

            try
            {
                Dumb.FD(ref this.Linaa);
                Dumb.FD(ref this.bs);

                //take respective BS
                bsCompositions = Interface.IBS.SelectedCompositions;
                BindingSource Bind = Interface.IBS.SelectedMatrix;
                if (!selectedMatrix)
                {
                    Bind = Interface.IBS.Matrix;
                    bsCompositions = Interface.IBS.Compositions;
                }

                this.compositionDGV.DataSource = bsCompositions;
             //   this.compositionDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

                setBindings(ref Bind);

                this.matrixRTB.MouseLeave += focus;
                this.compositionDGV.MouseHover += focus;

                focus(null, EventArgs.Empty);
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void setBindings(ref BindingSource Bind)
        {
            string column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;
            Binding mcompoBin = BS.ABinding(ref Bind, column);
            this.matrixRTB.DataBindings.Add(mcompoBin);
         
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