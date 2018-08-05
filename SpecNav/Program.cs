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
         
          
                UserControl hl = LIMSUI.CreateUI(ControlNames.SpecNavigator);

                Bitmap bt = DB.Properties.Resources.Matrices;
                Form form = DBForm.CreateForm(ref bt);
                form.Size = hl.Size;
                form.Controls.Add(hl);
                form.Opacity = 100;
                form.Text = "A spectrum browser for HyperLab users by F. Farina Arboccò";
                form.WindowState = FormWindowState.Maximized;

          

                Application.Run(form);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }

        }


      
    }
}
