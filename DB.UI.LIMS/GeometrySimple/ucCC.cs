using System;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

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

         //   System.EventHandler addNew = this.addNewVialChannel_Click;

          //  this.addChParBn.Click += addNew;

          //  this.bnChannelAddItem.Click += addNew;// new System.EventHandler(this.addNewVialChannel_Click);
        }

        public void Set(ref Interface LinaaInterface)
        {
            Interface = LinaaInterface;

            Dumb.FD(ref this.lINAA);
            Dumb.FD(ref this.ChannelBS);
            Dumb.FD(ref this.ContainerBS);
            // this.lINAA = Interface.Get() as LINAA;

            ChannelDGV.DataSource = Interface.IBS.Channels;
            ContainerDGV.DataSource = Interface.IBS.Rabbit;
            this.ContainerDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.ChannelDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;

            this.channelParBN.BindingSource = Interface.IBS.Channels;
            this.ContainerBN.BindingSource = Interface.IBS.Rabbit;


            this.ChannelDGV.CellClick += ChannelDGV_CellClick;
      //      DataGridViewButtonColumn col = this.fluxTypeDGVColumn as DataGridViewButtonColumn; // . Button;


        }

        private void ChannelDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != fluxTypeDGVColumn.Index) return;
           
            LINAA.ChannelsRow row = (LINAA.ChannelsRow)(Interface.ICurrent.Channel );

            if (row.FluxType.Contains("0")) row.FluxType = MatSSF.Types[1];
            else if (row.FluxType.Contains("1")) row.FluxType = MatSSF.Types[2];
            else row.FluxType = MatSSF.Types[0];
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
        /// <param name="e">     </param>

       
    }
}