using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;

namespace SpecNav
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

                Form aboutbox = new AboutBox();

                LIMSUI.Start(ref aboutbox, offline, adv,string.Empty);
         
          
                UserControl hl = LIMSUI.CreateUI(ControlNames.SpecNavigator, true);


                bool showAlready = false;
                LIMSUI.CreateForm(Application.ProductName, ref hl, showAlready);

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
