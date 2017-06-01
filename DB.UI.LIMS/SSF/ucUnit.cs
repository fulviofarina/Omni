using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucUnit : UserControl
    {
        protected static string TOOLTIP_TEXT = " but with user-defined parameters";
        protected internal Interface Interface = null;

      protected static  string EXPERT_MODE_ON_TITLE = "EXPERT MODE ACTIVATED";
        protected static string EXPERT_MODE_ON_MSG = "EXPERT MODE was activated:\n\n"
        + "You are now allowed to alter all the default, fundamental paramaters of the calculation methods.\n\n"
       + "Keep in mind that changing the fundamental values of each method was not intended by their creators, "
        + "therefore the corresponding results are marked with an (*) symbol and there is no guarantee on their " +
        "accuracy nor validity.\n\n"
        + "You can return to the DEFAULT MODE at any time.\n\n";


        /// <summary>
        /// A DGV Item selected to control child ucControls such as ucV uc Matrix Simple, ucCC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>

        protected internal int lastIndex = -2;

        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                this.unitDGV.RowHeaderMouseDoubleClick += value;
            }
        }

        public void AttachCtrl<T>(ref T pro)
        {
            if (pro.GetType().Equals(typeof(ucPreferences)))
            {
                IucPreferences pref = pro as IucPreferences;
                configurePreferences(ref pref);
            }
        }

        private void configurePreferences(ref  IucPreferences pref)
        {
            Color[] arr = new Color[] { Color.FromArgb(64, 64, 64), Color.Black, Color.FromArgb(64, 64, 64) };
            Color[] arr2 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.Chartreuse };
            Color[] arr3 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.LemonChiffon };
            Color[] arr4 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.Orange };
            EventHandler popup =
        delegate
        {
            if (Interface.IPreferences.CurrentSSFPref.Overrides)
            {
            
                MessageBox.Show(EXPERT_MODE_ON_MSG,EXPERT_MODE_ON_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Interface.IReport.Msg(EXPERT_MODE_ON_MSG, EXPERT_MODE_ON_TITLE);
            }
        };


            EventHandler chilian =
            delegate
            {
                paintChilianRelated(arr, arr3);
            };
            //  chilian += popup;

            Interface.IDB.SSFPref.DoChilianChanged = chilian;
            chilian.Invoke(null, EventArgs.Empty);

            EventHandler matssf =
      delegate
      {
          paintMatSSFRelated(arr, arr2, arr4);
      };
            //   matssf += popup;

            Interface.IDB.SSFPref.DoMatSSFChanged = matssf;
            matssf.Invoke(null, EventArgs.Empty);
            EventHandler overrider = chilian;
            overrider += matssf;
            overrider += popup;
            Interface.IDB.SSFPref.OverriderChanged = overrider;
          //  overrider.Invoke(null, EventArgs.Empty);


        }

        public void DgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int rowInder = e.RowIndex;
            DataRow row = Interface.IBS.GetDataRowFromDGV(sender, rowInder);
            Interface.IBS.SelectUnitOrChildRow(rowInder, ref row, ref lastIndex);
            PaintRows();
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
            unitDGV.RowHeadersVisible = true;
            SSFDGV.DataSource = Interface.IBS.SSF;

            errorProvider1.DataSource = Interface.IBS.Units;
            errorProvider2.DataSource = Interface.IBS.SSF;


            setBindings();

            this.unitDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.SSFDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

            this.unitDGV.CellValueChanged += UnitDGV_CellValueChanged;
            this.unitDGV.CellDoubleClick += UnitDGV_CellDoubleClick;

            Interface.IDB.Unit.InvokeCalculations += delegate
             {
                 PaintRows();
             };
        }

        private static void paintCell(ref DataGridViewCell cell)
        {
            //try
            // {
            string values = cell.Value.ToString();
            UnitRow u = Caster.Cast<UnitRow>(cell.OwningRow);
            SubSamplesRow s = u.SubSamplesRow;
            bool noSample = EC.IsNuDelDetch(s);

            Color clr2 = System.Drawing.Color.Transparent;
            Color colr = Color.DarkGreen;
            if (!noSample)
            {
                if (u.ToDo && u.IsBusy)
                {
                    colr = System.Drawing.Color.DarkOrange;
                    // cell.Style.ForeColor = System.Drawing.Color.DarkOrange;
                }
                else if (u.ToDo && !u.IsBusy)
                {
                    colr = System.Drawing.Color.DarkRed;
                    // cell.Style.ForeColor = System.Drawing.Color.DarkRed; cell.Style.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
            if (!noSample)
            {
                cell.Style.BackColor = colr;
                cell.Style.ForeColor = colr;
                cell.Style.SelectionBackColor = clr2;
                cell.Style.SelectionForeColor = clr2;

                // cell.DataGridView.ClearSelection();
            }
        }

        private void paintChilianRelated(Color[] arr, Color[] arr3)
        {
            // string gfast = "GFast";
            string gch = "Gt(Ch)";
            string gech = "Ge(Ch)";
            string ASTERISK = " *";
            string sep = "but";

            string add = ASTERISK;
            if (Interface.IPreferences.CurrentSSFPref.Overrides)
            {
                // this.SSFCh.HeaderText = "Ge(Ch) *"; this.GtCKS.HeaderText = "Gt(M) *";

                if (!SSFCh.ToolTipText.Contains(sep))
                {
                    this.SSFCh.ToolTipText += TOOLTIP_TEXT;
                    this.GtCKS.ToolTipText += TOOLTIP_TEXT;
                }
                // this.GFast.HeaderText = "GFast *";
            }
            else
            {
                add = string.Empty;
                if (SSFCh.ToolTipText.Contains(sep))
                {
                    this.SSFCh.ToolTipText = this.SSFCh.ToolTipText.Replace(TOOLTIP_TEXT, null);
                    this.GtCKS.ToolTipText = this.GtCKS.ToolTipText.Replace(TOOLTIP_TEXT, null);
                }
            }

            this.SSFCh.HeaderText = gech + add;
            this.GtCKS.HeaderText = gch + add;

            bool readOnly = !Interface.IPreferences.CurrentSSFPref.DoCK;
            DataGridViewColumn columna = this.GtCKS;
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr3);
            // columna.Visible = readOnly;
            columna = this.SSFCh;
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr3);
            // columna.Visible = readOnly;
        }

        private void paintMatSSFRelated(Color[] arr, Color[] arr2, Color[] arr4)
        {
            string gfast = "GFast(M)";
            string gm = "Gt(M)";
            string gem = "Ge(M)";
            string ASTERISK = " *";
            string sep = "but";

            string add = ASTERISK;
            if (Interface.IPreferences.CurrentSSFPref.Overrides)
            {
                if (!GFast.ToolTipText.Contains(sep))
                {
                    this.GFast.ToolTipText += TOOLTIP_TEXT;
                    this.GtM.ToolTipText += TOOLTIP_TEXT;
                    this.sSFDataGridViewTextBoxColumn.ToolTipText += TOOLTIP_TEXT;
                }
                // this.GFast.HeaderText = "GFast *";
            }
            else
            {
                add = string.Empty;
                if (GFast.ToolTipText.Contains(sep))
                {
                    this.sSFDataGridViewTextBoxColumn.ToolTipText = this.sSFDataGridViewTextBoxColumn.ToolTipText.Replace(TOOLTIP_TEXT, null);
                    this.GFast.ToolTipText = this.GFast.ToolTipText.Replace(TOOLTIP_TEXT, null);
                    this.GtM.ToolTipText = this.GtM.ToolTipText.Replace(TOOLTIP_TEXT, null);
                }

                // sSFDataGridViewTextBoxColumn.HeaderText = "Ge(M)"; this.GFast.HeaderText =
                // "GFast"; this.GtM.HeaderText = "Gt(M)";
            }

            this.GFast.HeaderText = gfast + add;
            this.GtM.HeaderText = gm + add;
            sSFDataGridViewTextBoxColumn.HeaderText = gem + add;

            DataGridViewColumn columna;
            columna = this.GtM;
            bool readOnly = !Interface.IPreferences.CurrentSSFPref.DoMatSSF;
            // columna.Visible = readOnly; Rsx.DGV.Control.PaintColumn(readOnly2, ref columna);
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);
            columna = this.sSFDataGridViewTextBoxColumn;
            // columna.Visible = readOnly; Rsx.DGV.Control.PaintColumn(readOnly2, ref columna);
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr2);

            columna = this.GFast;

            // columna.Visible = readOnly; Rsx.DGV.Control.PaintColumn(readOnly2, ref columna);
            Rsx.DGV.Control.PaintColumn(readOnly, ref columna, arr, arr4);
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

        private void UnitDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int rowInder = e.RowIndex;

            if (e.ColumnIndex == this.sampleDGVColumn.Index)
            {
                DataRow row = Interface.IBS.GetDataRowFromDGV(sender, rowInder);
                Interface.IBS.SelectUnit(ref row);
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
            catch (SystemException ex)
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

        public ucUnit()
        {
            InitializeComponent();
        }
    }
}