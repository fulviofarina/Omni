using System;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucComposition : UserControl
    {
        protected internal bool advanced = true;
        protected internal Interface Interface = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>

        public void Set(ref Interface LinaaInterface, bool selectedMatrix = true)
        {
            try
            {
                Dumb.FD(ref this.Linaa);
                this.compositionsDGV.DataSource = null;
                Dumb.FD(ref this.bs);

                Interface = LinaaInterface;
                BindingSource bsCompositions = null;
                //take respective BS

                BindingSource Bind = null;

                // bsCompositions = Bind;
                if (!selectedMatrix)
                {
                    Bind = Interface.IBS.Matrix;
                    bsCompositions = Interface.IBS.Compositions;
                }
                else
                {
                    this.compositionsDGV.BackgroundColor = System.Drawing.Color.Thistle;
                    Bind = Interface.IBS.SelectedMatrix;
                    bsCompositions = Interface.IBS.SelectedCompositions;
                }

                // this.compositionDGV.AutoGenerateColumns = true;
                this.compositionsDGV.DataMember = string.Empty;
                this.compositionsDGV.DataSource = bsCompositions;
                this.compositionsDGV.RowHeadersVisible = false;
                // this.compositionsDGV.Refresh(); this.compositionDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

                setBindings(ref Bind);

                this.matrixRTB.MouseDoubleClick += ChangeFocus;
                this.compositionsDGV.MouseDoubleClick += ChangeFocus;

                ChangeFocus(null, EventArgs.Empty);
                ChangeFocus(null, EventArgs.Empty);
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void setBindings(ref BindingSource Bind)
        {
            string column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;
            Binding mcompoBin = Rsx.Dumb.BS.ABinding(ref Bind, column);
            this.matrixRTB.DataBindings.Add(mcompoBin);
        }

        public void ChangeFocus(object sender, EventArgs e)
        {
            SC.Panel2Collapsed = advanced;
            SC.Panel1Collapsed = !advanced;

            advanced = !advanced;

            if (!SC.Panel2Collapsed)
            {
                this.matrixRTB.Focus();
            }
            // else { Interface.ICurrent.Matrix.EndEdit(); }
            Interface.IBS.EndEdit();
            // Interface.IDB.Matrix.EndLoadData();
        }

        public ucComposition()
        {
            InitializeComponent();
        }
    }
}