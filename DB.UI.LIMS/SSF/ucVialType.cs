using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public partial class ucVialType : UserControl
    {
        public ucVialType()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Rsx.Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();

            Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.VialType);
        }
    }
}