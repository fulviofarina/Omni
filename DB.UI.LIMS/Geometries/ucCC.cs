using System;
using System.Data;
using System.Windows.Forms;
using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucCC : UserControl
    {
        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        private Interface Interface = null;

        public ucCC()
        {
            InitializeComponent();

            Dumb.FD<LINAA>(ref this.lINAA);

            System.EventHandler addNew = this.addNewVialChannel_Click;

            this.addChParBn.Click += addNew;

            this.bnChannelAddItem.Click += addNew;// new System.EventHandler(this.addNewVialChannel_Click);
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;

            this.lINAA = Interface.Get() as LINAA;

            Dumb.LinkBS(ref this.ChannelBS, this.lINAA.Channels);
            string column = this.lINAA.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = this.lINAA.VialType.InnerRadiusColumn.ColumnName + " asc";
            //      Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Dumb.LinkBS(ref this.ContainerBS, this.lINAA.VialType, column + " = " + "True", innerRadCol);

            Interface.IBS.Channels = this.ChannelBS;
            Interface.IBS.Rabbit = this.ContainerBS;
        }

        public void RefreshCC()
        {
            LINAA.UnitRow u = MatSSF.UNIT;
            string column;
            column = Interface.IDB.VialType.VialTypeIDColumn.ColumnName;
            int id = u.SubSamplesRow.VialTypeRowByChCapsule_SubSamples.VialTypeID;
            BindingSource rabbitBS = Interface.IBS.Rabbit;
            rabbitBS.Position = rabbitBS.Find(column, id);
            column = Interface.IDB.Channels.ChannelsIDColumn.ColumnName;
            id = u.SubSamplesRow.IrradiationRequestsRow.ChannelsID;
            BindingSource channelBs = Interface.IBS.Channels;
            channelBs.Position = channelBs.Find(column, id);
        }

        //   private DataGridViewCellMouseEventHandler rowHeaderMouseClick = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
                // rowHeaderMouseClick = value;

                //    if (rowHeaderMouseClick != null) return;

                //  DataGridViewCellMouseEventHandler handler = value;
                //   rowHeaderMouseClick = handler;

                //  this.unitDGV.RowHeaderMouseClick += handler;
                this.ChannelDGV.RowHeaderMouseDoubleClick += value; // new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);

                this.ContainerDGV.RowHeaderMouseDoubleClick += value;// new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItemSelected);
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
                LINAA.VialTypeRow v = this.lINAA.VialType.NewVialTypeRow();
                v.IsRabbit = true;
                this.lINAA.VialType.AddVialTypeRow(v);

                colName = this.lINAA.VialType.VialTypeIDColumn.ColumnName;

                bs = this.ContainerBS;

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