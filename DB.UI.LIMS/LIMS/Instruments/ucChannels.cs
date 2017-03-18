using System.Windows.Forms;


namespace DB.UI
{
  public partial class ucChannels : UserControl
  {
    public ucChannels()
    {
      InitializeComponent();

      this.Linaa.Dispose();
      this.Linaa = null;
      this.Linaa = LIMS.Linaa;
      Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Channels);
    }
  }
}