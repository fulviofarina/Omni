using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VTools;

namespace DB.UI
{
  public partial class ucPredict : UserControl
  {
    protected DB.LINAA Linaa;

    public ucPredict(ref DB.LINAA set, ref IEnumerable<DB.LINAA.SubSamplesRow> samples)
    {
      InitializeComponent();
      this.Linaa = set.Clone() as DB.LINAA;
      this.Linaa.InitializeComponent();

      this.Linaa.Merge(set, false, MissingSchemaAction.Ignore);

      this.Linaa.PopulateColumnExpresions();

      this.Linaa.IRequestsAverages.Clear();
      this.Linaa.IPeakAverages.Clear();

      CleanReadOnly(this.Linaa.IRequestsAverages);
      CleanReadOnly(this.Linaa.IPeakAverages);

      ToolStripProgressBar bar = new ToolStripProgressBar();
      ToolStripMenuItem can = new ToolStripMenuItem();

      DB.Tools.IWC w = new DB.Tools.WC("Predict", ref bar, ref can, ref this.Linaa);
      w.SelectedSamples = samples.ToList();
      //   w.Predict(ref samples);

      System.Data.DataSet set2 = this.Linaa;
      Explorer e = new Explorer(ref set2);
      e.Box.Text = this.Linaa.IRequestsAverages.TableName;
      Auxiliar f = new Auxiliar();
      f.Populate(e);
      f.Show();
    }

    public void CleanReadOnly(DataTable table)
    {
      foreach (DataColumn column in table.Columns)
      {
        if (column.ReadOnly && column.Expression.Equals(string.Empty) && !table.PrimaryKey.Contains(column))
        {
          column.ReadOnly = false;
        }
      }
    }
  }
}