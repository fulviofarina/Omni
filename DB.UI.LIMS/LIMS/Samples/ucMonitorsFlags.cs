using System.Windows.Forms;

namespace DB.UI
{
  public partial class ucMonitorsFlags : UserControl
  {
    public ucMonitorsFlags()
    {
      InitializeComponent();
      this.Linaa.Dispose();
      this.Linaa = null;
      this.Linaa = LIMS.Linaa;

      Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.MonitorsFlags);
    }
  }
}