using System.Windows.Forms;
using Rsx;

namespace DB.UI
{
    public partial class ucStandards : UserControl
    {
        public ucStandards()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();

            Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Standards);
        }

        private void DGV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = DGV.Rows[e.RowIndex];
            //Rsx.DGV.Control.PaintChanges(ref r, this.descripCol.Name);
        }
    }
}