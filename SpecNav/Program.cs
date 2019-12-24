using DB.Tools;
using DB.UI;
using System;
using System.Reflection;
using System.Windows.Forms;
using VTools;

namespace SpecNav
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool offline = false;
            bool adv = false;
            bool lims = false;
            bool hyperLab = true;
            bool mmqs = false;
            bool noDGVCtrl = true;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                IAboutBox about = new AboutBox();
                about.AssemblyToProvide = Assembly.GetExecutingAssembly();

                LIMSUI.Start(ref about, offline, adv, string.Empty);

                UtilSQL.CheckConnectionsRoutine(mmqs, lims, hyperLab);

                UserControl hl = LIMSUI.CreateUI(ControlNames.SpecNavigator, noDGVCtrl);

                bool showAlready = false;
                Creator.CreateAppForm(Application.ProductName, ref hl, showAlready);

                Form toShow = hl.ParentForm;

                toShow.WindowState = FormWindowState.Maximized;

                Application.Run(toShow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }
        }
    }
}