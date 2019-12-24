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

        public string ViewString { get; internal set; }


        protected static string EDIT_STR = "EDIT";
        protected static string VIEW_STR = "VIEW";



        protected internal EventHandler changeFocusCallBack;

        public  EventHandler ChangeFocusCallBack
        { 
          get
            {
                return changeFocusCallBack;
            }
            set
            {
                changeFocusCallBack = value;
            }
        }

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

                this.matrixRTB.MouseDoubleClick += changeFocus;
                this.compositionsDGV.MouseDoubleClick += changeFocus;

                this.compositionsDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;


                this.matrixRTB.Tag = "Double-click to switch between the Edit and View mode\n\n";

                this.matrixRTB.MouseHover += Interface.IReport.ReportToolTip;
            
             //   this.compositionsDGV.CellMouseClick+= Interface.IReport.ReportToolTip;


                //    this.compositionsDGV.MouseHover += Interface.IReport.ReportToolTip;



                //  this.toolTip1.SetToolTip(this.compositionsDGV, "Double-click to edit the composition");
                //   this.toolTip1.SetToolTip(this.matrixRTB, "Double-click to edit the composition");



            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void destroy()
        {
            this.compositionsDGV.DataSource = null;
            Dumb.FD(ref this.bs);
            Dumb.FD(ref this.Linaa);
        }

        protected internal void setBindings(bool selectedMatrix)
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

        protected internal void changeFocus(object sender, EventArgs e)
        {
            ChangeFocus();
          
        }

        public void ChangeFocus()
        {
            ViewString = EDIT_STR;

            SC.Panel2Collapsed = advanced;
            SC.Panel1Collapsed = !advanced;

            advanced = !advanced;

            Interface.IBS.EndEdit();

            if (!SC.Panel2Collapsed)
            {
                focusRTB();
                ViewString = VIEW_STR;
            }

            changeFocusCallBack?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// needs to us the DGV not the BS
        /// </summary>
        protected internal void focusRTB()
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

        protected internal void findCompositionInRTB(ref CompositionsRow current)
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