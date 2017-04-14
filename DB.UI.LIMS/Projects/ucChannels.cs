using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public partial class ucChannels : UserControl
    {
        public ucChannels()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Rsx.Dumb.FD(ref this.Linaa);
            Rsx.Dumb.FD(ref this.BS);
            // this.Linaa = inter.Get();
            DGV.DataSource = inter.IBS.Channels;
           // Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Channels);
        }
    }
}