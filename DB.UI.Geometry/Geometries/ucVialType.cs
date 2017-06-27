using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

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
            Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();
            Dumb.FD(ref this.BS);

            this.DGV.DataSource = inter.IBS.Vial;  //= this.VialBS;
                                                   // Rsx.Dumb.BS.LinkBS(ref this.BS, this.Linaa.VialType);
        }
    }
}