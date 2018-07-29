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

            bool offline = false;
            bool adv = true;


            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form aboutbox = new AboutBox();

                LIMSUI.Start(ref aboutbox, offline, adv,string.Empty);
         
                bool ok = Creator.CheckConnections(true, true);
         
          
                UserControl hl = LIMSUI.CreateUI(ControlNames.SpecNavigator);

                Bitmap bt = DB.Properties.Resources.Matrices;
                Form form = DBForm.CreateForm(ref bt);
                form.Size = hl.Size;
                form.Controls.Add(hl);
                form.Opacity = 100;
                form.Text = "A data browser for HyperLab users, by F. Farina Arboccò";
       

               // LIMSUI.Interface.IPreferences.CurrentPref.AdvancedEditor = true;

               // PainterTimer();

                Application.Run(form);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
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
