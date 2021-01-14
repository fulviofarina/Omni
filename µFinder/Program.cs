using DB.UI;
using DB.Tools;

using System;
using System.Reflection;
using System.Windows.Forms;
using VTools;

namespace uFinder
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //work offline
            bool offline = true;
            // advanced mode
            bool adv = true;

            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

             
             
                //create aboutBox
                IAboutBox about = new AboutBox();
                about.AssemblyToProvide = Assembly.GetExecutingAssembly();

                //file with resources from developer
                Creator.RSX.MainLIMSResource = Properties.Resources.LIMS;

              

                LIMSUI.Start(ref about, offline, adv);

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