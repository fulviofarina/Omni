using DB.Tools;
using Rsx.Dumb;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucOrders : UserControl
    {
        public ucOrders()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();

            Rsx.Dumb.BS.LinkBS(ref this.BS, this.Linaa.Orders);
        }
    }
}