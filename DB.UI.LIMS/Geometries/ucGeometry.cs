using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucGeometry : UserControl
    {
        private System.Data.DataColumn Std = null;

        public ucGeometry()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            BS.DeLinkBS(ref this.bs);
            Dumb.FD(ref this.Linaa);
            Dumb.FD(ref this.bs);

            this.DGV.DataSource = inter.IBS.Geometry;

            bool coon = this.Linaa.Geometry.Columns.Contains("Std");
            if (!coon)
            {
                Std = new System.Data.DataColumn("Std", typeof(bool), "IIF(Count(Child(Geometry_Standards).MonitorCode)>0, 1,0)");
                this.Linaa.Geometry.Columns.Add(Std);
            }
        }

        /*
        public new void Dispose()
        {
            bool coon = Std != null;
            if (coon)
            {
                // Std = new System.Data.DataColumn("Std", typeof(bool),
                // "IIF(Count(Child(Geometry_Standards).MonitorCode)>0, 1,0)");
                this.Linaa.Geometry.Columns.Remove(Std);
            }

            base.Dispose();
        }
    */
    }
}