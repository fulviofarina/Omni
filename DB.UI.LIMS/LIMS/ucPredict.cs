using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using VTools;

namespace DB.UI
{
    public partial class ucPredict : UserControl
    {
        protected LINAA Linaa;

        public ucPredict(ref LINAA set, ref IEnumerable<LINAA.SubSamplesRow> samples)
        {
            InitializeComponent();

            cloneDataSet(ref set);

            ToolStripProgressBar bar = new ToolStripProgressBar();
            ToolStripMenuItem can = new ToolStripMenuItem();

            IWC w = new WC("Predict", ref bar, ref can, ref this.Linaa);
            w.SelectedSamples = samples.ToList();
            w.Predict(ref samples);

            explore();
        }

        private void cleanReadOnly(ref DataTable table)
        {
            //    DataTable table = dt as DataTable;
            foreach (DataColumn column in table.Columns)
            {
                bool ok = column.ReadOnly;
                ok = ok && column.Expression.Equals(string.Empty);
                ok = ok && !table.PrimaryKey.Contains(column);
                if (ok) column.ReadOnly = false;
            }
        }

        private void cloneDataSet(ref LINAA set)
        {
            this.Linaa = set.Clone() as DB.LINAA;
            this.Linaa.InitializeComponent();
            this.Linaa.Merge(set, false, MissingSchemaAction.Ignore);
            this.Linaa.PopulateColumnExpresions();
            this.Linaa.IRequestsAverages.Clear();
            this.Linaa.IPeakAverages.Clear();

            DataTable table = Linaa.IRequestsAverages;
            cleanReadOnly(ref table);
            table = Linaa.IPeakAverages;
            cleanReadOnly(ref table);
        }

        private void explore()
        {
            //explore second dataset
            DataSet set2 = this.Linaa;
            Explorer e = new Explorer(ref set2);
            //name of table to show
            e.Box.Text = this.Linaa.IRequestsAverages.TableName;

            //populate form
            Auxiliar f = new Auxiliar();
            f.Populate(e);
            f.Show();
        }
    }
}