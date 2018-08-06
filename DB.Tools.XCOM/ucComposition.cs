using DB.Tools;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DB.LINAA;

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
                destroy();

                Interface = LinaaInterface;

                setBindings(selectedMatrix);

                EventHandler enabled = delegate
                {
                    bool ctrlcanBeenable = Interface.IBS.EnabledControls;
                    this.Enabled = ctrlcanBeenable;
                };

                Interface.IBS.EnableControlsChanged += enabled;

                this.Disposed += delegate
                  {
                      Interface.IBS.EnableControlsChanged -= enabled;
                  };

                this.compositionsDGV.RowHeadersVisible = false;
                this.matrixRTB.MouseDoubleClick += ChangeFocus;
                this.compositionsDGV.MouseDoubleClick += ChangeFocus;

                this.compositionsDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void destroy()
        {
            this.compositionsDGV.DataSource = null;
            Dumb.FD(ref this.bs);
            Dumb.FD(ref this.Linaa);
        }

        private void setBindings(bool selectedMatrix)
        {
            BindingSource bsCompositions = Interface.IBS.SelectedCompositions;
            //take respective BS
            BindingSource Bind = Interface.IBS.SelectedMatrix;

            // bsCompositions = Bind;
            if (!selectedMatrix)
            {
                Bind = Interface.IBS.Matrix;
                bsCompositions = Interface.IBS.Compositions;
            }
            else
            {
                //I think SelectedCompositions is not used
                this.compositionsDGV.BackgroundColor = System.Drawing.Color.FromArgb(255, 35, 35, 35);
            }

            string column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;
            Binding mcompoBin = Rsx.Dumb.BS.ABinding(ref Bind, column);
            this.matrixRTB.DataBindings.Add(mcompoBin);

            this.compositionsDGV.DataSource = bsCompositions;
        }

        public void ChangeFocus(object sender, EventArgs e)
        {
            SC.Panel2Collapsed = advanced;
            SC.Panel1Collapsed = !advanced;

            advanced = !advanced;

            Interface.IBS.EndEdit();

            if (!SC.Panel2Collapsed)
            {
                focusRTB();
            }
        }

        /// <summary>
        /// needs to us the DGV not the BS
        /// </summary>
        private void focusRTB()
        {
            IEnumerable<CompositionsRow> compos = Rsx.Dumb.Caster.Cast<CompositionsRow>(this.compositionsDGV);

            CompositionsRow current = Rsx.Dumb.Caster.Cast<CompositionsRow>(this.compositionsDGV.CurrentRow);

            if (compos == null) return;
            if (current == null) return;

            MatrixRow m = current.MatrixRow;

            bool recode = true;
            Interface.IPopulate.IGeometry.AddCompositions(ref m, ref compos, null, recode);

            this.compositionsDGV.Refresh();
            //yes again
            current = Rsx.Dumb.Caster.Cast<CompositionsRow>(this.compositionsDGV.CurrentRow);

            findCompositionInRTB(ref current);
            this.matrixRTB.Focus();
        }

        private void findCompositionInRTB(ref CompositionsRow current)
        {
            this.matrixRTB.SelectionStart = this.matrixRTB.TextLength;
            if (current == null) return;

            string text = current.Element + "\t" + current.Quantity;
            int index = this.matrixRTB.Find(text);

            this.matrixRTB.SelectionStart = index;
        }

        public ucComposition()
        {
            InitializeComponent();
        }
    }
}