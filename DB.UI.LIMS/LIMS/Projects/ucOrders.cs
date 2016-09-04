using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucOrders : UserControl
    {
        public ucOrders()
        {
            InitializeComponent();
            this.Linaa.Dispose();
            this.Linaa = null;
            this.Linaa = LIMS.Linaa;

            Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Orders);
        }
    }
}