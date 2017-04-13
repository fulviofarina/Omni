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

        /// <summary>
        /// A binding Current Changed event to update Binding sources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitBS_CurrentChanged(object sender, System.EventArgs e)
        {
         
            MatSSF.UNIT = Interface.ICurrent.Unit as LINAA.UnitRow;
        
            Interface.IBS.Update<LINAA.SubSamplesRow>(MatSSF.UNIT.SubSamplesRow,false);
            Interface.IBS.Update<LINAA.UnitRow>(MatSSF.UNIT, true,false);
            MatSSF.ReadXML();
          
            string column = MatSSF.Table.UnitIDColumn.ColumnName;
            string sortCol = MatSSF.Table.TargetIsotopeColumn.ColumnName;
            string unitID = MatSSF.UNIT.UnitID.ToString();
            string filter = column + " is " + unitID;

            Dumb.LinkBS(ref this.SSFBS, MatSSF.Table, filter, sortCol);
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
            this.lINAA = Interface.Get() as LINAA;


            MatSSF.Table = Interface.IDB.MatSSF;

            setBindings();
      

            this.UnitBS.CurrentChanged -= UnitBS_CurrentChanged;
            this.UnitBS.CurrentChanged += UnitBS_CurrentChanged;

        }

        private void setBindings()
        {
            //sets all the bindings again
            BindingSource bs = this.UnitBS;

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

            Interface.IBS.Units = bs; //link to binding source

        }

        //     private DataGridViewCellMouseEventHandler rowHeaderMouseClick = null;

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