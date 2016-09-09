using System;
using System.Windows.Forms;

namespace DB.UI
{
  public partial class ucIrradiationsRequests : UserControl
  {
    protected string sortColumn;
    protected string filter;

    public ucIrradiationsRequests()
    {
      InitializeComponent();

      this.Linaa.Dispose();
      this.Linaa = null;
      this.Linaa = LIMS.Linaa;

      filter = string.Empty;
      sortColumn = this.Linaa.IrradiationRequests.IrradiationStartDateTimeColumn.ColumnName;
      this.channelBox.ComboBox.DataSource = this.Linaa.Channels;
      this.channelBox.ComboBox.DisplayMember = this.Linaa.Channels.ChannelNameColumn.ColumnName;

      if (this.channelBox.Items.Count != 0)
      {
        this.channelBox.Text = this.channelBox.Items[0].ToString();
      }

      Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.IrradiationRequests, filter, sortColumn + " desc");
    }

    public void CreateNewIrradiation(String project)
    {
      this.BS.EndEdit();

      string projetNoCd = project.ToUpper();
      if (projetNoCd.Length < 2) return;
      else if (projetNoCd.Length > 2)
      {
        if (projetNoCd.Substring(projetNoCd.Length - 2).CompareTo(DB.Properties.Misc.Cd) == 0)
        {
          projetNoCd = projetNoCd.Replace(DB.Properties.Misc.Cd, null);
        }
      }
      LINAA.IrradiationRequestsRow i = this.Linaa.IrradiationRequests.NewIrradiationRequestsRow();
      this.Linaa.IrradiationRequests.AddIrradiationRequestsRow(i);
      if (i != null)
      {
        i.IrradiationCode = projetNoCd;
        if (i.ChannelsRow == null) this.SetIrradiationChannel.PerformClick();
        irradiationSave.PerformClick();
        this.refreshToolStripMenuItem.PerformClick();
      }
    }

    public void RowAdded(ref System.Data.DataRow row)
    {
      LINAA.IrradiationRequestsRow ir = row as LINAA.IrradiationRequestsRow;
      ir.IrradiationStartDateTime = DateTime.Now;

      ir.IrradiationCode = "New";
    }

    private void DGV_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (IrradiationCodeColumn.Index == e.ColumnIndex && e.RowIndex >= 0)
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
    }

    private void channelBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      filter = string.Empty;

      if (channelBox.Text.CompareTo(string.Empty) != 0)
      {
        string chColumn = this.Linaa.IrradiationRequests.ChannelNameColumn.ColumnName;
        filter = chColumn + " = '" + channelBox.Text + "' OR " + chColumn + " IS NULL ";
      }

      Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.IrradiationRequests, filter, sortColumn + " desc");
    }
  }
}