using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;



namespace SSF
{
    internal static class Program
    {
        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form aboutbox = new AboutBox();

                LIMSUI.Start(ref aboutbox, false, false, string.Empty);

                bool ok = Creator.CheckConnections(true,true);
                if (ok) Creator.LoadMethods(0);
                else throw new Exception("Could not start loading the database");
            
                //EventHandler firstCallBack;
                Form toShow =   LIMSUI.CreateSSFApplication();

                Creator.Run();

            

              //  PainterTimer();

                Application.Run(toShow);

            }
            catch (Exception ex)
            {
                LIMSUI.Interface.IStore.AddException(ex);
                LIMSUI.Interface.IStore.SaveExceptions();
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }
    
        }

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




    }
}