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

         

            System.EventHandler addNew = this.addNewVialChannel_Click;

            this.addChParBn.Click += addNew;

            this.bnChannelAddItem.Click += addNew;// new System.EventHandler(this.addNewVialChannel_Click);
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;

            Dumb.FD(ref this.lINAA);
            Dumb.FD(ref this.ChannelBS);
            Dumb.FD(ref this.ContainerBS);
            //  this.lINAA = Interface.Get() as LINAA;

            ChannelDGV.DataSource = Interface.IBS.Channels;
            ContainerDGV.DataSource = Interface.IBS.Rabbit;

       
        }
      
        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        public DataGridViewCellMouseEventHandler RowHeaderMouseClick
        {
            ///FIRST TIME AND ONLY
            set
            {
             
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

         
        //    DataRow r;
            if (!sender.Equals(this.addChParBn))
            {
                LINAA.VialTypeRow v = Interface.IDB.VialType.NewVialTypeRow();
                v.IsRabbit = true;
                Interface.IDB.VialType.AddVialTypeRow(v);
                Interface.IBS.Update(v);

            }
            //IS A CHANNEL
            else
            {
                LINAA.ChannelsRow ch = Interface.IDB.Channels.NewChannelsRow();
                Interface.IDB.Channels.AddChannelsRow(ch);
                Interface.IBS.Update( ch);
            }

           

        }
    }
}