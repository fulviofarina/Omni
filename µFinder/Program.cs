using System;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;

namespace µFinder
{
    static class Program
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

                string LIMSresource = Properties.Resources.LIMS;

                //create aboutBox
                Form aboutbox = new AboutBox();

                LIMSUI.Start(ref aboutbox, offline, adv,LIMSresource);

                //set the editor
                bool noDGVControls = false;
                UserControl control = LIMSUI.CreateUI(ControlNames.Matrices, null, noDGVControls);

          
                bool showAlready = false;
                LIMSUI.CreateForm(Application.ProductName, ref control, showAlready);

                Form toShow = control.ParentForm;
    
                toShow.WindowState = FormWindowState.Maximized;

                Application.Run(toShow);

            }
            catch (Exception ex)
            {
           //    Rsx. WriteException(crashFile, ex);

            }

        }

    /*
        public static void PainterTimer()
        {

            //to repaint the form
            System.Timers.Timer painter = new System.Timers.Timer();
            painter.Elapsed += delegate
            {
                painter.Enabled = false;

                Application.OpenForms[0]?.Invalidate();

                painter.Interval = 60 * 10 * 1000; //10 minutes

                GC.Collect();

                painter.Enabled = true;

            };
            painter.Interval = 30000;
            painter.Enabled = true;
        }
      */
    }
}
