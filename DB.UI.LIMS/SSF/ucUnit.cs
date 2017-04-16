using System.Data;
using System.Windows.Forms;

using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucUnit : UserControl
    {
        public ucUnit()
        {
            InitializeComponent();

          
        }

        /// <summary>
        /// A DGV Item selected to control child ucControls such as ucV uc Matrix Simple, ucCC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ///check if table has no rows

            try
            {
                DataGridView dgv = sender as DataGridView;
                if (dgv.RowCount == 0)
                {
                    Interface.IReport.Msg("No Rows found in the DataGridView", "Error", false); //report
                    return;
                }
                DataRow row = Dumb.Cast<DataRow>(dgv.Rows[e.RowIndex]);


                //link to matrix, channel or vial,/rabbit data
                MatSSF.LinkToParent(ref row);

            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
                Interface.IMain.AddException(ex);
            }


        }

  

       

        private Interface Interface = null;

        /// <summary>
        /// Link the bindings sources, EXECUTES ONCE
        /// </summary>
        /// <param name="Unitbs"></param>
        /// <param name="SSFbs"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            Dumb.FD<LINAA>(ref this.lINAA);
            Dumb.FD(ref this.UnitBS);
            Dumb.FD(ref this.SSFBS);

            unitDGV.DataSource = Interface.IBS.Units;
            SSFDGV.DataSource = Interface.IBS.SSF;

            //    this.lINAA = Interface.Get() as LINAA;


        

            setBindings();
      

         

        }

        private void setBindings()
        {
            //sets all the bindings again
            BindingSource bs = Interface.IBS.Units;

            string sort = Interface.IDB.Unit.NameColumn.ColumnName + " asc";
            Dumb.LinkBS(ref bs, Interface.IDB.Unit, string.Empty, sort);

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            string text = "Text";
            string column = Interface.IDB.Unit.LastCalcColumn.ColumnName;
            Binding lastcalbs = new Binding(text, bs, column, true, mo);
            column = Interface.IDB.Unit.LastChangedColumn.ColumnName;
            Binding lastchgbs = new Binding(text, bs, column, true, mo);
            this.lastCal.TextBox.DataBindings.Add(lastcalbs);
            this.lastChg.TextBox.DataBindings.Add(lastchgbs);

       //     Interface.IBS.Units = bs; //link to binding source

        }

      

        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {

                this.unitDGV.RowHeaderMouseClick += value;


                /*
                MouseEventArgs m = null;
                m = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
                DataGridViewCellMouseEventArgs args = null;
                args = new DataGridViewCellMouseEventArgs(-1, 0, 0, 0, m);

                value.Invoke(this.unitDGV, args);

                */
            }
        }
    }
}