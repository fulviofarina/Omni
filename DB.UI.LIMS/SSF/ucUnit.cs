﻿using System.Data;
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
        private int lastColInder = -2;
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
                    MatSSF.UNIT.SetParent(ref row);
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
            this.unitDGV.CellMouseClick += cellMouseClick;
            //  this.SSFDGV.CellMouseClick += cellMouseClick;
         
        
           
            this.unitDGV.ColumnHeaderMouseClick += columnHeaderMouseClick;
            this.SSFDGV.ColumnHeaderMouseClick += columnHeaderMouseClick;

         //   this.unitDGV.CellToolTipTextNeeded += cellToolTipTextNeeded;
         //   this.SSFDGV.CellToolTipTextNeeded += cellToolTipTextNeeded;
            //     this.unitDGV.MouseHover += UnitDGV_MouseHover;
        }
        /*
        private void cellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {

            bool isSameCol = lastColInder == e.ColumnIndex;
            if (isSameCol) return;
            if (e.RowIndex != -1) return;
            lastColInder = e.ColumnIndex;
            DataGridView dgv = sender as DataGridView;
            SpeakToolTip(ref dgv, lastColInder);
            //  columnHeaderMouseClick(sender,);
            // throw new System.NotImplementedException();
        }
        */
        private  void columnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //  bool isSameCol = (lastColInder == e.ColumnIndex);
            DataGridView dgv = sender as DataGridView;
            int index = e.ColumnIndex;

            SpeakToolTip(ref dgv, index);
        }

        private void SpeakToolTip(ref DataGridView dgv, int index)
        {
            try
            {
                DataGridViewHeaderCell cell = dgv.Columns[index].HeaderCell;
                string toolTip = cell.ToolTipText;
                if (string.IsNullOrEmpty(toolTip)) return;
                //   if (e.RowIndex != -1) return;
                Interface.IReport.Speak(toolTip);
            }
            catch (System.Exception ex)
            {

                //  throw;
            }
        }


        private void cellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            bool isToDoColumn = false;
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex >= 0)
            {
                isToDoColumn = dgv.Columns[e.ColumnIndex].ValueType.Equals(typeof(bool));
            }
            if (isToDoColumn)
            {
                //invert the bool or check state
                dgv[e.ColumnIndex, e.RowIndex].Value = !bool.Parse(dgv[e.ColumnIndex, e.RowIndex].Value.ToString());

                dgv.RefreshEdit();
                // unit.ToDo = !unit.ToDo;
            }
            /*
            else if (e.RowIndex==-1)
            {
             //   if (!isSameCol)
                {
                    DataGridViewColumn colum = dgv.Columns[e.ColumnIndex];
                    if (!string.IsNullOrEmpty(colum.ToolTipText))
                    {
                        Interface.IReport.Speak(colum.ToolTipText);
                    }
                }
                    // if (e.ColumnIndex == this.g)
            }
            */

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