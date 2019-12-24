using DB.UI;
using DB.Tools;
using System;
using System.Reflection;
using System.Windows.Forms;
using VTools;

namespace µFinder
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool offline = true;

            bool adv = false;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //file with resources
                string LIMSresource = Properties.Resources.LIMS;

                //create aboutBox
                IAboutBox about = new AboutBox();
                about.AssemblyToProvide = Assembly.GetExecutingAssembly();


                LIMSUI.Start(ref about, offline, adv, LIMSresource);

                //set the editor
                bool noDGVControls = false;
                UserControl control = LIMSUI.CreateUI(ControlNames.Matrices, noDGVControls);

                bool showAlready = false;
                Creator.CreateAppForm(Application.ProductName, ref control, showAlready);

                Form toShow = control.ParentForm;

                toShow.WindowState = FormWindowState.Maximized;

                Application.Run(toShow);
            }
            catch (Exception ex)
            {
                
            }
        }

      
    }
}