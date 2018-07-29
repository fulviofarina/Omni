using System;
using System.Windows.Forms;

namespace k0X
{
  public partial class ucSchAcqs : UserControl
  {
    public ucSchAcqs(ref DB.LINAA set)
    {
      InitializeComponent();

      this.Linaa.Dispose();
      this.Linaa = null;
      this.Linaa = set;

      Auxiliar f = new Auxiliar();
      f.Populate(this);
      f.Text = "Measurement Scheduler";
      f.ControlBox = true;
      f.WindowState = FormWindowState.Minimized;
      f.Show();

      Rsx.Dumb.BS.LinkBS(ref this.BS, this.Linaa.SchAcqs, string.Empty, "StartOn desc");
    }

    private void schAcqsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
    {
      this.Validate();
      this.BS.EndEdit();
      this.Linaa.SaveTable<DB.LINAA.SchAcqsDataTable>();
    }

    private void Refresh_Click(object sender, EventArgs e)
    {
      this.Linaa.PopulateScheduledAcqs();
    }
  }
}