using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using VTools;

namespace DB.UI
{
    public partial class ucGAlgo : UserControl
    {
        protected LINAA Linaa;

        public ucGAlgo(ref LINAA set, ref IEnumerable<LINAA.SubSamplesRow> samples)
        {
            InitializeComponent();

            Linaa = set.Clone() as DB.LINAA;
            Linaa.CloneDataSet(ref set);

            ToolStripProgressBar bar = new ToolStripProgressBar();
            ToolStripMenuItem can = new ToolStripMenuItem();

            IWC w = new WC("Predict", ref bar, ref can, ref this.Linaa);
            w.SelectedSamples = samples.ToList();
            w.Predict(ref samples);

            explore();
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