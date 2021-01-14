using DB.Tools;
using Rsx.Dumb;
using System.Windows.Forms;

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
            Dumb.FD(ref this.Linaa);
            Dumb.FD(ref this.BS);
            // this.Linaa = inter.Get();
            DGV.DataSource = inter.IBS.Channels;
            // Rsx.Dumb.BS.LinkBS(ref this.BS, this.Linaa.Channels);
        }
    }
}