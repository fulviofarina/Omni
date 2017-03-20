using System.Windows.Forms;
using Rsx;

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

            Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Orders);
        }
    }
}