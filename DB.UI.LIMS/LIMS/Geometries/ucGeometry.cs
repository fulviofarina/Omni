using System.Windows.Forms;
using Rsx;

namespace DB.UI
{
  public partial class ucGeometry : UserControl
  {
    private System.Data.DataColumn Std = null;

    public ucGeometry()
    {
      InitializeComponent();
      Dumb.DeLinkBS(ref this.BS);
      this.Linaa.Dispose();
      this.Linaa = null;
      this.Linaa = LIMS.Linaa;

      bool coon = this.Linaa.Geometry.Columns.Contains("Std");
      if (!coon)
      {
        Std = new System.Data.DataColumn("Std", typeof(bool), "IIF(Count(Child(Geometry_Standards).MonitorCode)>0, 1,0)");
        this.Linaa.Geometry.Columns.Add(Std);
      }

      Dumb.LinkBS(ref this.BS, this.Linaa.Geometry, string.Empty, "CreationDateTime desc");
    }

    /*
    public new void Dispose()
    {
        bool coon = Std != null;
        if (coon)
        {
            //  Std = new System.Data.DataColumn("Std", typeof(bool), "IIF(Count(Child(Geometry_Standards).MonitorCode)>0, 1,0)");
            this.Linaa.Geometry.Columns.Remove(Std);
        }

        base.Dispose();
    }
*/
  }
}