using System.Data;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucUnit : UserControl
    {
        private Interface Interface = null;

        /// <summary>
        /// A DGV Item selected to control child ucControls such as ucV uc Matrix Simple, ucCC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>

        private int lastIndex = -2;

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
                DataRow row = Caster.Cast<DataRow>(dgv.Rows[e.RowIndex]);

                bool isToDoColumn = false;

                if (e.ColumnIndex >= 0)
                {
                    isToDoColumn = row[e.ColumnIndex].GetType().Equals(typeof(bool));
                }
                if (isToDoColumn)
                {
                    //invert the bool or check state
                    row[e.ColumnIndex] = !(bool)row[e.ColumnIndex];
                    // unit.ToDo = !unit.ToDo;
                }
                bool isUnuit = row.GetType().Equals(typeof(LINAA.UnitRow));
                if (isUnuit && lastIndex != e.RowIndex)
                {
                    //so it does not select and reselect everytime;
                    lastIndex = e.RowIndex;

                    LINAA.UnitRow unit = row as LINAA.UnitRow;

                    // if ((row as LINAA.UnitRow) == unit) return;

                    //   Interface.IDB.MatSSF.Clear();
                    //if it is a checheakble column box
                    //then check it automatically

                    //update bindings

                    Interface.IBS.Update<LINAA.SubSamplesRow>(unit?.SubSamplesRow, false, true);

                    Interface.IBS.Update<LINAA.UnitRow>(unit, true, false);
                }
                else
                {
                    //link to matrix, channel or vial,/rabbit data
                    MatSSF.LinkToParent(ref row);
                    string tipo = row.GetType().ToString();

                    Interface.IReport.Msg("Unit values updated with Template Item", "Updated!", false); //report
                }

                //then it will be updated
            }
            catch (System.Exception ex)
            {
                // Interface.IReport.Msg(ex.StackTrace, ex.Message);
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Link the bindings sources, EXECUTES ONCE
        /// </summary>
        /// <param name="Unitbs"></param>
        /// <param name="SSFbs"> </param>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            Dumb.FD<LINAA>(ref this.lINAA);
            Dumb.FD(ref this.UnitBS);
            Dumb.FD(ref this.SSFBS);

            unitDGV.DataSource = Interface.IBS.Units;
            SSFDGV.DataSource = Interface.IBS.SSF;

            // this.lINAA = Interface.Get() as LINAA;

            setBindings();

            // this.unitDGV.RowHeaderMouseClick += DgvItemSelected; this.unitDGV.SelectionChanged += UnitDGV_SelectionChanged;
            this.unitDGV.CellMouseClick += DgvItemSelected;
        }

        private void setBindings()
        {
            //sets all the bindings again
            BindingSource bs = Interface.IBS.Units;

            string sort = Interface.IDB.Unit.NameColumn.ColumnName + " asc";
            BS.LinkBS(ref bs, Interface.IDB.Unit, string.Empty, sort);

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            string text = "Text";
            string column = Interface.IDB.Unit.LastCalcColumn.ColumnName;
            Binding lastcalbs = new Binding(text, bs, column, true, mo);
            column = Interface.IDB.Unit.LastChangedColumn.ColumnName;
            Binding lastchgbs = new Binding(text, bs, column, true, mo);
            this.lastCal.TextBox.DataBindings.Add(lastcalbs);
            this.lastChg.TextBox.DataBindings.Add(lastchgbs);

            // Interface.IBS.Units = bs; //link to binding source
        }

        public ucUnit()
        {
            InitializeComponent();
        }
    }
}