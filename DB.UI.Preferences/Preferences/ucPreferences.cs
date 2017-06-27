using System.Windows.Forms;

namespace DB.UI
{
    public interface IPreferences
    {
        IMainPreferences IMain { get; }

        ISSFPreferences ISSF { get; }

        // void Set(ref Interface inter);
    }

    public partial class ucPreferences : UserControl, IPreferences
    {
        public IMainPreferences IMain
        {
            get
            {
                return main;
            }
        }

        public ISSFPreferences ISSF
        {
            get
            {
                return ssf;
            }
        }

        public ucPreferences()
        {
            InitializeComponent();
        }
    }
}