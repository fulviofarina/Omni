using System;
using System.Windows.Forms;

namespace NSS
{
    internal static class Program
    {
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
                Form toload = null;

                toload = DB.UI.ucSSF.Start();

                //  toload.WindowState = FormWindowState.Maximized;
                Application.Run(toload);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Program Error: " + ex.Message + "\n\n"+  ex.StackTrace);
            }
        }

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
   
    }
}