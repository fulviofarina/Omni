using System;
using System.Data;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucIrradiationsRequests : UserControl
    {
        public ucIrradiationsRequests()
        {
            InitializeComponent();
        }

        private Interface Interface;

        public void Set(ref Interface inter)
        {
            //InitializeComponent();

            Interface = inter;

            Dumb.FD(ref this.Linaa);
            Dumb.FD(ref this.BS);
            // this.Linaa = inter.Get();

            // filter = string.Empty;

            // this.BS = Interface.IBS.Irradiations;
            DGV.DataSource = Interface.IBS.Irradiations;
            this.channelBox.ComboBox.DisplayMember = Interface.IDB.Channels.ChannelNameColumn.ColumnName;

            this.channelBox.ComboBox.DataSource = Interface.IBS.Channels;

            // this.channelBox.TextChanged += (this.channelBox_SelectedIndexChanged);
            // this.DGV.CellPainting += DGV_CellPainting; this.DGV.CellPainting +=(this.DGV_CellPainting);

            if (this.channelBox.Items.Count != 0)
            {
                this.channelBox.Text = this.channelBox.Items[0].ToString();
            }

            // Dumb.BS.LinkBS(ref this.BS, Interface.IDB.IrradiationRequests, filter, sortColumn + " desc");

            // this.channelBox.TextChanged += (this.channelBox_SelectedIndexChanged);
        }

        public void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            System.Data.DataRowView v = (System.Data.DataRowView)(DGV.Rows[e.RowIndex].DataBoundItem);
            LINAA.IrradiationRequestsRow r = (LINAA.IrradiationRequestsRow)v.Row;
            if (r.ChannelsRow != null)
            {
                string code = r.ChannelsRow.IrReqCode;
                if (code.Contains("X")) e.CellStyle.BackColor = System.Drawing.Color.Lavender;
                else if (code.Contains("Z")) e.CellStyle.BackColor = System.Drawing.Color.LemonChiffon;
                else if (code.Contains("Y")) e.CellStyle.BackColor = System.Drawing.Color.Moccasin;
            }
        }

        public void RowAdded(ref DataRow row)
        {
            try
            {
                LINAA.IrradiationRequestsRow ir = row as LINAA.IrradiationRequestsRow;

                if (ir.ChannelsRow == null)
                {
                    LINAA.ChannelsRow c = Caster.Cast<LINAA.ChannelsRow>(this.channelBox.ComboBox.SelectedItem);

                    ir.ChannelsRow = c;

                    Interface.IBS.CurrentChanged(ir);
                }
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        public bool ShouldPaintCell(object sender, DataGridViewCellPaintingEventArgs e)
        {
            return (IrradiationCodeColumn.Index == e.ColumnIndex);
        }
    }
}