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

            this.UnitBS.CurrentChanged += UnitBS_CurrentChanged;
        }

        private void UnitBS_CurrentChanged(object sender, System.EventArgs e)
        {
            DataRow r = (Interface.IBS.Units.Current as DataRowView).Row;
            MatSSF.UNIT = r as LINAA.UnitRow;
            string sampleIDvalue = Interface.IDB.SubSamples.SubSamplesIDColumn.ColumnName;
            Interface.IBS.SubSamples.Position = Interface.IBS.SubSamples.Find(sampleIDvalue, MatSSF.UNIT.SampleID);

            // if (this.unitBS.Count == 0) return;
            //   MatSSF.UNIT = row as LINAA.UnitRow;
            MatSSF.ReadXML();

            //  LINAA.UnitRow   u = MatSSF.UNIT;
            string column = MatSSF.Table.UnitIDColumn.ColumnName;
            string sortCol = MatSSF.Table.TargetIsotopeColumn.ColumnName;
            string unitID = MatSSF.UNIT.UnitID.ToString();
            string filter = column + " is " + unitID;

            Dumb.LinkBS(ref this.SSFBS, MatSSF.Table, filter, sortCol);
        }

        public void DeLink()
        {
            Dumb.DeLinkBS(ref this.SSFBS);
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
            Interface.IBS.Units = this.UnitBS; //link to binding source

            MatSSF.Table = Interface.IDB.MatSSF;

            //sets all the bindings again
            BindingSource bs = Interface.IBS.Units;
            string sort = Interface.IDB.Unit.NameColumn.ColumnName + " asc";
            Dumb.LinkBS(ref bs, Interface.IDB.Unit, string.Empty, sort);

            //set binding sources

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            string text = "Text";
            string column = Interface.IDB.Unit.LastCalcColumn.ColumnName;
            Binding lastcalbs = new Binding(text, bs, column, true, mo);
            column = Interface.IDB.Unit.LastChangedColumn.ColumnName;
            Binding lastchgbs = new Binding(text, bs, column, true, mo);
            this.lastCal.TextBox.DataBindings.Add(lastcalbs);
            this.lastChg.TextBox.DataBindings.Add(lastchgbs);
        }

        //     private DataGridViewCellMouseEventHandler rowHeaderMouseClick = null;

        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                // rowHeaderMouseClick = value;

                //      if (rowHeaderMouseClick != null) return;

                //    DataGridViewCellMouseEventHandler handler = value;
                //     rowHeaderMouseClick = handler;

                this.unitDGV.RowHeaderMouseClick += value;

                MouseEventArgs m = null;
                m = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
                DataGridViewCellMouseEventArgs args = null;
                args = new DataGridViewCellMouseEventArgs(-1, 0, 0, 0, m);

                value.Invoke(this.unitDGV, args);
            }
        }
    }
}