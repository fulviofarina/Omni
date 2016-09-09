using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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

            Dumb.FD<LINAA>(ref this.lINAA);
          

        }
        public void DeLink()
        {
            Dumb.DeLinkBS(ref this.SSFBS);
        }
        public void RefreshSSF()
        {
            // if (this.unitBS.Count == 0) return;
         //   MatSSF.UNIT = row as LINAA.UnitRow;
            MatSSF.ReadXML();

            //  LINAA.UnitRow   u = MatSSF.UNIT;
            string column = MatSSF.Table.UnitIDColumn.ColumnName;
            string sortCol = MatSSF.Table.TargetIsotopeColumn.ColumnName;
            string unitID = MatSSF.UNIT.ToString();

            Dumb.LinkBS(ref this.SSFBS, MatSSF.Table, column + " is " + unitID, sortCol);
        }


        private Interface Interface = null;

        /// <summary>
        /// Link the bindings sources, EXECUTES ONCE
        /// </summary>
        /// <param name="Unitbs"></param>
        /// <param name="SSFbs"></param>
        public void Set(ref Interface LinaaInterface)
        {

            Interface = LinaaInterface;


          
            this.lINAA = Interface.Get() as LINAA;
           
            MatSSF.Table = this.lINAA.MatSSF;

            //sets all the bindings again

            Dumb.LinkBS(ref this.UnitBS, this.lINAA.Unit, string.Empty, this.lINAA.Unit.UnitIDColumn.ColumnName + " desc");

            ///delete  first
            //  this.SSFBS.Dispose();
            //  this.unitBS.Dispose();

            ///link
            //   this.SSFBS = SSFbs;
            //   this.unitBS = Unitbs;
            ///link dgvs
            // this.unitDGV.DataSource = this.unitBS;
            //  this.SSFDGV.DataSource = this.SSFBS;

            //set binding sources
            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
           // LINAA.UnitDataTable Unit = this.lINAA.Unit;
            //BindingSource bs = this.unitBS;
           // bool t = true;

            string text = "Text";
            string column = this.lINAA.Unit.LastCalcColumn.ColumnName;
            Binding lastcalbs = new Binding(text, this.UnitBS, column, true, mo);
            column = this.lINAA.Unit.LastChangedColumn.ColumnName;
            Binding lastchgbs = new Binding(text, this.UnitBS, column, true, mo);

             this.lastCal.TextBox.DataBindings.Add(lastcalbs);
            this.lastChg.TextBox.DataBindings.Add(lastchgbs);


          


        }



        DataGridViewCellMouseEventHandler rowHeaderMouseClick = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
           

            ///FIRST TIME AND ONLY
            set
            {
                // rowHeaderMouseClick = value;

                if (rowHeaderMouseClick != null) return;

                DataGridViewCellMouseEventHandler handler =  value;
                rowHeaderMouseClick = handler;

                this.unitDGV.RowHeaderMouseClick += handler;

                MouseEventArgs m = null;
                m = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
                DataGridViewCellMouseEventArgs args = null;
                args = new DataGridViewCellMouseEventArgs(-1, 0, 0, 0, m);

                handler.Invoke(this.unitDGV, args);

            }
        }


        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



    }
}
