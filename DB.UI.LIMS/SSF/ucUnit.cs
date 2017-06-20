using System;
using System.Data;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucUnit : UserControl
    {
        protected internal Interface Interface = null;
        protected internal int lastIndex = -2;

        protected static string gch = "Gt(Ch)";

        protected static string gech = "Ge(Ch)";
        protected static string gem = "Ge(M)";
        protected static string gfast = "GFast(M)";
        protected static string gm = "Gt(M)";

        protected static string ERROR = "ERROR";
        protected static string NO_ROWS = "No Rows found in the DataGridView.";


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

        public void DgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int rowInder = e.RowIndex;
            Action<string,string,bool,bool> action = Interface.IReport.Msg;
    
            DataRow row = Rsx.DGV.Control.GetDataRowFromDGV(ref sender, rowInder);

            action.Invoke(NO_ROWS, ERROR, false, false); //report

            //quitar inder, poner esto en populate..
            Interface.IBS.AssignUnitChild(ref row);

            lastIndex = rowInder;

            DGVRefresher.Invoke(sender, e);

                    
        }
        public EventHandler DGVRefresher
        {
            get
            {
         return   delegate
        {
            this.unitDGV.Invalidate();
            this.SSFDGV.Invalidate();
        };
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
            unitDGV.RowHeadersVisible = false;
            SSFDGV.DataSource = Interface.IBS.SSF;

            IPreferences pref = LIMS.GetPreferences();

            configurePreferences(ref pref);

            errorProvider1.DataSource = Interface.IBS.Units;
            errorProvider2.DataSource = Interface.IBS.SSF;

            setBindings();

            this.unitDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.SSFDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

         //   EventHandler handler = delegate
          //   {
               //  Application.RaiseIdle(EventArgs.Empty);
          //   };

           // Interface.IDB.Unit.InvokeCalculations += handler;
           // Interface.IDB.SubSamples.InvokeCalculations += handler;
        }

        private void configurePreferences(ref IPreferences pref)
        {
            string roundingBindableField = Interface.IDB.SSFPref.RoundingColumn.ColumnName;

            string bindableField2 = Interface.IDB.SSFPref.DoCKColumn.ColumnName;
            IUnitSSFColumn col4 = this.SSFCh as IUnitSSFColumn;

            col4.OriginalHeaderText = gech;
            setSSFColumn(ref col4, 2, bindableField2, roundingBindableField);

            IUnitSSFColumn col5 = this.GtCKS as IUnitSSFColumn;
            col5.OriginalHeaderText = gch;
            setSSFColumn(ref col5, 2, bindableField2, roundingBindableField);

         

            EventHandler chilian = delegate
            {
                col4.PaintHeader();
                col5.PaintHeader();
            };

            chilian.Invoke(null, EventArgs.Empty);

            pref.DoChilianChanged += chilian;
            pref.DoChilianChanged += DGVRefresher;

            string bindableField = Interface.IDB.SSFPref.DoMatSSFColumn.ColumnName;
            IUnitSSFColumn col = this.unitSSFColumn1 as IUnitSSFColumn;
            col.OriginalHeaderText = gfast;
            setSSFColumn(ref col, 3, bindableField, roundingBindableField);

            IUnitSSFColumn col2 = this.GtM as IUnitSSFColumn;
            col2.OriginalHeaderText = gm;
            setSSFColumn(ref col2, 1, bindableField, roundingBindableField);

            IUnitSSFColumn col3 = this.sSFDataGridViewTextBoxColumn as IUnitSSFColumn;
            col3.OriginalHeaderText = gem;
            setSSFColumn(ref col3, 1, bindableField, roundingBindableField);

            EventHandler matssf = delegate
            {
                col.PaintHeader();
                col2.PaintHeader();
                col3.PaintHeader();
            };

            matssf.Invoke(null, EventArgs.Empty);

            pref.DoMatSSFChanged += matssf;
            pref.DoMatSSFChanged += DGVRefresher;

            EventHandler overrider = chilian;
            overrider += matssf;
            overrider += DGVRefresher;

            DGVRefresher.Invoke(null, EventArgs.Empty);

            pref.OverriderChanged += overrider;

            EventHandler rounding = delegate
            {
                col.SetRounding();
                col2.SetRounding();
                col3.SetRounding();
                col4.SetRounding();
                col5.SetRounding();
            };

            pref.RoundingChanged += rounding;
            pref.RoundingChanged += DGVRefresher;
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

        private void setSSFColumn(ref IUnitSSFColumn col, int type, string bindingField, string roundingField)
        {
            col.SSFCellType = type;
            col.BindableAsteriskField = Interface.IDB.SSFPref.OverridesColumn.ColumnName;
            col.BindingPreferenceField = bindingField;
            col.BindingRoundingField = roundingField;
            col.BindingPreferenceRow = Interface.IPreferences.CurrentSSFPref;
        }
        /*
        private void UnitDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int rowInder = e.RowIndex;

            if (e.ColumnIndex == this.sampleDGVColumn.Index)
            {
                Action<string, string, bool, bool> action = Interface.IReport.Msg;
                DataRow row = Rsx.DGV.Control.GetDataRowFromDGV(ref sender, rowInder, action);
                //    DataRow row = Interface.IBS.GetDataRowFromDGV(sender, rowInder);
                UnitRow unit = row as UnitRow;
                string title = SAMPLE;// + unit.Name;
                title += unit.Name;
                unit.ToDo = !unit.ToDo;
                Interface.IReport.Msg(title + SELECTED_ROW, SELECTED); //report
                return unit;
            }
        }
        */
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