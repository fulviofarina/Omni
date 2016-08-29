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

                toload = MakeForm();
               
              //  toload.WindowState = FormWindowState.Maximized;
                Application.Run(toload);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Program Error: " + ex.Message);
            }

          


        }

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
        private static Form MakeForm()
        {

            DB.LINAA db = null;
            DB.Msn msn = new DB.Msn();
            NotifyIcon con = null;
            string result = DB.Tools.Creator.Build(ref db, ref con, ref msn);

            //   DB.Tools.Creator.CallBack = this.CallBack;
            //  DB.Tools.Creator.LastCallBack = this.LastCallBack;

            if (!string.IsNullOrEmpty(result))
            {
                //    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //   Connections_Click(null, EventArgs.Empty);
            }
            else DB.Tools.Creator.Load(ref db, 0);

           

            DB.UI.ucMatSSF uc = new DB.UI.ucMatSSF(ref db, false);

            //   DB.UI.Auxiliar aux = new DB.UI.Auxiliar();

            return uc.ParentForm;
        }

      
    }
}