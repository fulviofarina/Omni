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
using Msn;


namespace DB.UI
{
    public partial class ucUnit : UserControl
    {

        public ucUnit()
        {

            InitializeComponent();

        }

        /// <summary>
        /// Link the bindings sources, EXECUTES ONCE
        /// </summary>
        /// <param name="Unitbs"></param>
        /// <param name="SSFbs"></param>
        public void Set(ref BindingSource Unitbs, ref BindingSource SSFbs)
        {


            //sets all the bindings again


            ///delete  first
           this.SSFBS.Dispose();
            this.unitBS.Dispose();

            ///link
            this.SSFBS = SSFbs;
            this.unitBS = Unitbs;
            ///link dgvs
            this.unitDGV.DataSource = this.unitBS;
            this.SSFDGV.DataSource = this.SSFBS;

            //set binding sources
            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
           // LINAA.UnitDataTable Unit = this.lINAA.Unit;
            //BindingSource bs = this.unitBS;
           // bool t = true;

            string text = "Text";
            string column = this.lINAA.Unit.LastCalcColumn.ColumnName;
            Binding lastcalbs = new Binding(text, Unitbs, column, true, mo);
            column = this.lINAA.Unit.LastChangedColumn.ColumnName;
            Binding lastchgbs = new Binding(text, Unitbs, column, true, mo);

             this.lastCal.TextBox.DataBindings.Add(lastcalbs);
            this.lastChg.TextBox.DataBindings.Add(lastchgbs);


            this.lINAA.Dispose();
           this.lINAA = null;


        


        }





        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
           

            ///FIRST TIME AND ONLY
            set
            {
               // rowHeaderMouseClick = value;

                DataGridViewCellMouseEventHandler handler =  value;

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
