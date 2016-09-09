using System.Windows.Forms;

namespace DB.UI
{
  public partial class ucStandards : UserControl
  {
    public ucStandards()
    {
      InitializeComponent();
      this.Linaa.Dispose();
      this.Linaa = null;
      this.Linaa = LIMS.Linaa;

      Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Standards);
    }

    private void DGV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
      if (e.RowIndex < 0) return;
      DataGridViewRow r = DGV.Rows[e.RowIndex];
      //Rsx.DGV.Control.PaintChanges(ref r, this.descripCol.Name);
    }
  }
}