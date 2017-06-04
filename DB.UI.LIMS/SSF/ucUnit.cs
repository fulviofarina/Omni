using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucUnit : UserControl
    {
        protected internal Interface Interface = null;
        protected internal int lastIndex = -2;

        protected static string ASTERISK = " *";

      
        protected static string gch = "Gt(Ch)";

        protected static string gech = "Ge(Ch)";
        protected static string gem = "Ge(M)";
        protected static string gfast = "GFast(M)";
        protected static string gm = "Gt(M)";
        protected static string sep = "but";
        protected static string TOOLTIP_TEXT = " but with user-defined parameters";

        /// <summary>
        /// A DGV Item selected to control child ucControls such as ucV uc Matrix Simple, ucCC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
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

        public void DgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int rowInder = e.RowIndex;
            DataRow row = Interface.IBS.GetDataRowFromDGV(sender, rowInder);
            Interface.IBS.SelectUnitOrChildRow(rowInder, ref row, ref lastIndex);
            this.unitDGV.ClearSelection();
            // PaintRows();
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


         //   this.unitDGV.CellMouseDoubleClick += delegate

          //    {
                 
           //   };
          
           

            EventHandler handler = delegate
             {
                 Application.RaiseIdle(EventArgs.Empty);
             };

            Interface.IDB.Unit.InvokeCalculations += handler;
            Interface.IDB.SubSamples.InvokeCalculations += handler;

        }         

        private void configurePreferences(ref IucPreferences pref)
        {
            Color[] arr = new Color[] { Color.FromArgb(64, 64, 64), Color.Black, Color.FromArgb(64, 64, 64) };
            Color[] arr2 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.Chartreuse };
            Color[] arr3 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.LemonChiffon };
            Color[] arr4 = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.Orange };

            EventHandler chilian = delegate
           {
               paintChilianRelated(arr, arr3);
           };

            chilian.Invoke(null, EventArgs.Empty);

            pref.DoChilianChanged += chilian;

            EventHandler matssf = delegate
     {
         paintMatSSFRelated(arr, arr2, arr4);
         
     };

            matssf.Invoke(null, EventArgs.Empty);
            pref.DoMatSSFChanged += matssf;
      
            EventHandler overrider = chilian;
            overrider += matssf;

            pref.OverriderChanged += overrider;
        }

        private void paintChilianRelated(Color[] arr, Color[] arr3)
        {
            // string gfast = "GFast";

            // string sep = "but";

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

        /*
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

        */

        public ucUnit()
        {
            InitializeComponent();
        }
    }
}