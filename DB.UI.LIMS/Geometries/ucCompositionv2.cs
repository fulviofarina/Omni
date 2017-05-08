using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucComposition2 : UserControl
    {
        private bool advanced = true;
        private Interface Interface = null;
        private BindingSource bsMatrix = null;
        private BindingSource bsCompositions = null;
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
                bsMatrix = Interface.IBS.SelectedMatrix;
                if (!selectedMatrix)
                {
                    bs = Interface.IBS.Matrix;
                    bsCompositions = Interface.IBS.Compositions;
                }

            
             //   this.compositionDGV.DataSource = bsCompositions;

                string column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName;

                Binding mcompoBin = BS.ABinding(ref bs, column);

                this.matrixRTB.DataBindings.Add(mcompoBin);

           
            //    mcompoBin.BindingComplete += McompoBin_BindingComplete;
             //   column = Interface.IDB.Compositions.ElementColumn.ColumnName;
             //   Binding mcompoBin2 = BS.ABinding(ref bsCompositions, column,string.Empty,"Nodes");
                //       this.treeView1.Nodes.AddRange(mcompoBin2.i)
               this.listView1.MouseHover += focus;
                //    this.listView1.DataBindings.Add(mcompoBin2);
                //   this.listView1.Items.AddRange();

                //    bs.ListChanged += Bs_ListChanged;

             //   bs.CurrentItemChanged += Bs_CurrentItemChanged;


               // this.matrixRTB.TextChanged += MatrixRTB_TextChanged;
                this.matrixRTB.MouseLeave += focus;
              //  mcompoBin.Parse += McompoBin_Parse;
             //   mcompoBin.Format += McompoBin_Format;
               // this.compositionDGV.MouseHover += focus;

                focus(null, EventArgs.Empty);
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

   



        //   private void McompoBin_BindingComplete(object sender, BindingCompleteEventArgs e)
        //   {
        //  prepareListView();
        //   }

        private void focus(object sender, EventArgs e)
        {
            SC.Panel2Collapsed = advanced;
           SC.Panel1Collapsed = !advanced;


            //PREPARE THE LIST VIEW
       //     if (SC.Panel2Collapsed)
            {
                prepareListView();

            }
            advanced = !advanced;
        }

        private void prepareListView()
        {
            this.listView1.Items.Clear();
            this.listView1.View = View.List;

            IEnumerable<LINAA.CompositionsRow> compos = Caster.Cast<LINAA.MatrixRow>(bsMatrix.Current).GetCompositionsRows();
            string[] titles = compos
                .Select(o => o.Element.Trim() + " - " + o.Quantity.ToString()).ToArray();

            foreach (var item in titles)
            {
                ListViewItem i = new ListViewItem(item);
        
                i.Font = new System.Drawing.Font(this.listView1.Font.FontFamily, 14F, System.Drawing.FontStyle.Italic);
                this.listView1.Items.Add(item);
            }
        }

        public ucComposition2()
        {
            InitializeComponent();
        }
    }
}