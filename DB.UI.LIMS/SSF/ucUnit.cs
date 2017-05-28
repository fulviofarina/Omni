using System;
using System.Data;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;
using System.Drawing;
using static DB.LINAA;
using Rsx;

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
            int rowInder = e.RowIndex;
            DataRow row = Interface.IBS.GetDataRowFromDGV(sender, rowInder);
            Interface.IBS.SelectUnitChildRow(rowInder, ref row, ref lastIndex);
            PaintRows();
        }



        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                this.unitDGV.RowHeaderMouseDoubleClick += value;
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
            errorProvider1.DataSource = Interface.IBS.Units;
            errorProvider2.DataSource = Interface.IBS.SSF;
            unitDGV.RowHeadersVisible = true;
            setBindings();

            this.unitDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.SSFDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
      
            this.unitDGV.CellValueChanged += UnitDGV_CellValueChanged;
   

            PaintRows();

        }

     

        public void AttachCtrl<T>(ref T pro)
        {
            if (pro.GetType().Equals(typeof(ucPreferences)))
            {

                Color[] arr = new Color[] { Color.FromArgb(64, 64, 64),  Color.Black, Color.FromArgb(64,64,64)};
                Color[] arr2 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.Chartreuse};
                Color[] arr3 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.LemonChiffon };
                Color[] arr4 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.Orange };

                IucPreferences pref = pro as IucPreferences;
                EventHandler dos = 
                delegate
                {
                    bool readOnly = !Interface.IPreferences.CurrentSSFPref.DoCK;
                    DataGridViewColumn columna = this.GtCKS;
                    Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr3);
                //    columna.Visible = readOnly;
                    columna = this.SSFCh;
                    Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr3);
                  //  columna.Visible = readOnly;

                };
                //invoke it
              //  dos.Invoke(null, EventArgs.Empty);

                pref.CheckChanged2 += dos;

                EventHandler trre =
          delegate
                {
                    DataGridViewColumn columna;
                    columna = this.GtM;
                    bool readOnly = !Interface.IPreferences.CurrentSSFPref.DoMatSSF;
                 //     columna.Visible = readOnly;
                   // Rsx.DGV.Control.PaintColumn(readOnly2, ref columna);
                    Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
                    columna = this.sSFDataGridViewTextBoxColumn;
                //    columna.Visible = readOnly;
                  //  Rsx.DGV.Control.PaintColumn(readOnly2, ref columna);
                    Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);

                    columna = this.GFast;
                 //    columna.Visible = readOnly;
                 //   Rsx.DGV.Control.PaintColumn(readOnly2, ref columna);
                    Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr4);
                };
                //invoke it
            //z    trre.Invoke(null, EventArgs.Empty);

                pref.CheckChanged3 += trre;

            }
        }
      

        public void PaintRows()
        {
            foreach (DataGridViewRow row in this.unitDGV.Rows)
            {
                try
                {
               
                DataGridViewCell cell = unitDGV[ToDoCol.Index, row.Index];
                paintCell(ref cell);
                }

                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                }
            }
        }

        private void UnitDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //only if it is a TODO
                DataGridView dgv = sender as DataGridView;
                if (dgv.Rows.Count == 0 || e.RowIndex < 0) return;
                if (e.ColumnIndex == ToDoCol.Index)
                {
                    DataGridViewCell cell = dgv[e.ColumnIndex, e.RowIndex];
                    paintCell(ref cell);
                }
            }
            
            catch   (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
}
        /*
        private void cellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (dgv.Rows.Count == 0 || e.RowIndex<0) return;

            UnitRow row =  Interface.IBS.GetDataRowFromDGV(sender, e.RowIndex) as UnitRow;

            if (e.ColumnIndex == ToDoCol.Index)
            {
                //invert the bool or check state
                DataGridViewCell cell = dgv[e.ColumnIndex, e.RowIndex];
                object value = cell.Value;
                cell.Value = !bool.Parse(value.ToString());
            }
        }
        */
        private static void paintCell(ref DataGridViewCell cell)
        {

            //try
           // {
                string values = cell.Value.ToString();
                UnitRow u = Caster.Cast<UnitRow>(cell.OwningRow);
                SubSamplesRow s = u.SubSamplesRow;
                bool noSample = EC.IsNuDelDetch(s);
                // Button c = cell as Button;
                //    bool ok = 
                if (!u.ToDo && !noSample && !u.SubSamplesRow.Selected)
                {
                    cell.Style.BackColor = System.Drawing.Color.DarkGreen;
                    cell.Style.ForeColor = System.Drawing.Color.DarkGreen;
                }
                else if (u.ToDo && !noSample && u.SubSamplesRow.Selected)
                {
                    cell.Style.BackColor = System.Drawing.Color.DarkOrange;
                    cell.Style.ForeColor = System.Drawing.Color.DarkOrange;
                }
                else
                {
                    cell.Style.BackColor = System.Drawing.Color.DarkRed;
                    cell.Style.ForeColor = System.Drawing.Color.DarkRed;
                    //  cell.Style.ForeColor = System.Drawing.Color.DarkRed;
                }

                cell.Style.SelectionBackColor = System.Drawing.Color.Transparent;
                cell.Style.SelectionForeColor = System.Drawing.Color.Transparent;

                cell.DataGridView.ClearSelection();
          
           
        }

        private void setBindings()
        {
            //sets all the bindings again
            BindingSource bs = Interface.IBS.Units;

            string sort = Interface.IDB.Unit.NameColumn.ColumnName + " asc";
            Rsx.Dumb.BS.LinkBS(ref bs, Interface.IDB.Unit, string.Empty, sort);

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            string text = "Text";
            string column = Interface.IDB.Unit.LastCalcColumn.ColumnName;
            Binding lastcalbs = new Binding(text, bs, column, true, mo);
            column = Interface.IDB.Unit.LastChangedColumn.ColumnName;
            Binding lastchgbs = new Binding(text, bs, column, true, mo);
            this.lastCal.TextBox.DataBindings.Add(lastcalbs);
            this.lastChg.TextBox.DataBindings.Add(lastchgbs);
            column = Interface.IDB.Unit.NameColumn.ColumnName;
            Binding b0 = Rsx.Dumb.BS.ABinding(ref bs, column);
            sampleLBL.TextBox.DataBindings.Add(b0);
           
            // Interface.IBS.Units = bs; //link to binding source
        }

        public ucUnit()
        {
            InitializeComponent();
        }
    }
}