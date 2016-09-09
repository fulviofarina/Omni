using System;
using System.Windows.Forms;

using DB.Tools;
using System.Data;
using Rsx;


namespace DB.UI
{
    public partial class ucVCC : UserControl
    {





        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        Interface Interface = null;
        public ucVCC()
        {
            InitializeComponent();

            Dumb.FD<LINAA>(ref this.lINAA);



            System.EventHandler addNew = this.addNewVialChannel_Click;


            this.addChParBn.Click += addNew;

            this.bnChannelAddItem.Click += addNew;// new System.EventHandler(this.addNewVialChannel_Click);

            this.bnVialAddItem.Click += addNew;//  new System.EventHandler(this.addNewVialChannel_Click);



        }
        public void Set(ref Interface LinaaInterface)
        {

            Interface = LinaaInterface;

         
            this.lINAA = Interface.Get() as LINAA;


            Dumb.LinkBS(ref this.ChannelBS, this.lINAA.Channels);
            string column = this.lINAA.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = this.lINAA.VialType.InnerRadiusColumn.ColumnName + " asc";
            Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Dumb.LinkBS(ref this.ContainerBS, this.lINAA.VialType, column + " = " + "True", innerRadCol);



        }

        public void RefreshVCC()
        {
            LINAA.UnitRow u = MatSSF.UNIT;
            string column;
            column = this.lINAA.VialType.VialTypeIDColumn.ColumnName;
            int id = u.VialTypeID;
            this.VialBS.Position = this.VialBS.Find(column, id);
            id = u.ContainerID;
            this.ContainerBS.Position = this.ContainerBS.Find(column, id);
            column = this.lINAA.Channels.ChannelsIDColumn.ColumnName;

           
            id = u.ChannelID;
            this.ChannelBS.Position = this.ChannelBS.Find(column, id);

        }

        public void EndEdit()
        {
            this.VialBS.EndEdit();
            this.ChannelBS.EndEdit();
            this.ContainerBS.EndEdit();
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

                DataGridViewCellMouseEventHandler handler = value;
                rowHeaderMouseClick = handler;

              //  this.unitDGV.RowHeaderMouseClick += handler;
                this.ChannelDGV.RowHeaderMouseClick += handler; // new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);

                this.vialDGV.RowHeaderMouseClick += handler; // new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);

                this.ContainerDGV.RowHeaderMouseClick += handler;// new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);

             

            }
        }




        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void addNewVialChannel_Click(object sender, EventArgs e)
        {


          
            BindingSource bs = null;
            string colName = string.Empty;
            object idnumber = null;
            DataRow row = null;
            //IS A VIAL OR CONTAINER
            if (!sender.Equals(this.addChParBn))
            {


                bool isRabbit = !sender.Equals(this.bnVialAddItem);

                LINAA.VialTypeRow v = this.lINAA.VialType.NewVialTypeRow();
                v.IsRabbit = isRabbit;
                this.lINAA.VialType.AddVialTypeRow(v);

                colName = this.lINAA.VialType.VialTypeIDColumn.ColumnName;

                if (isRabbit)
                {
                    bs = this.ContainerBS;
                }
                else
                {
                    bs = this.VialBS;
                }
                idnumber = v.VialTypeID;

                row = v;

            }
            //IS A CHANNEL
            else
            {
                //  {
                LINAA.ChannelsRow ch = this.lINAA.Channels.NewChannelsRow();
                this.lINAA.Channels.AddChannelsRow(ch);

                colName = this.lINAA.Channels.ChannelsIDColumn.ColumnName;
                bs = this.ChannelBS;
                idnumber = ch.ChannelsID;

                row = ch;
                //   }
                //  newIndex = this.ChannelBS.Find(colName, ch.ChannelsID);
                // this.ChannelBS.Position = newIndex;
            }

            if (row.HasErrors)
            {
                string rowWithError = DB.UI.Properties.Resources.rowWithError;
                string Error = DB.UI.Properties.Resources.Error;

                Interface.IReport.Msg(rowWithError, Error);
            }

            int newIndex = bs.Find(colName, idnumber);
            bs.Position = newIndex;
        }


      



    }
}