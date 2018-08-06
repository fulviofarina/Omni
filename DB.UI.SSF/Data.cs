using DB.Tools;
using System.Windows.Forms;

namespace DB.UI
{
    public interface IData
    {
        IDataItem INS { get; }
        IDataItem ISample { get; }
    }

    public interface IDataItem
    {
        void Set(ref Interface inter, ref IPreferences pref);
    }

    public partial class ucSSFData : UserControl, IData
    {
        public IDataItem INS
        {
            get
            {
                return ucSSFDataNS1;
            }
        }

        public IDataItem ISample
        {
            get
            {
                return ucSSFDataSample1;
            }
        }

        public ucSSFData()
        {
            InitializeComponent();
        }
    }
}