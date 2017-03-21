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

        public ucPredict(ref LINAA set)
        {
            InitializeComponent();

            Linaa = set;

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