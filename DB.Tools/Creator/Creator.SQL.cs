using System;
using System.Data.Linq;
using System.Windows.Forms;
using DB.Linq;
using Rsx;

namespace DB.Tools
{
    public partial class Creator
    {
        private static string deniedTheInstall = "\n\nThe user denied the SQL Express installation";
        private static string nocontinueWOSQL = "Cannot continue without a SQL connection";
        private static string shouldInstallSQL = "Would you like to install SQL LocalDB?";
        private static string sqlLocalDB = "SQL LocalDB Installation";
        private static string checkingSQL = "Checking the SQL connections";
        private static string sqlStarted = "Installation of SQL LocalDB started. When finished click OK to restart";
        private static string triedToInstall = "\n\nThe user tried to install SQL Express";
    }

    public partial class Creator
    {
        /// <summary>
        /// Prepare the needed methods and the worker
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool PrepareSQL()
        {
            Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg(checkingSQL, "Please wait...");

            bool ok = true;

            Interface.IAdapter.InitializeComponent();

            Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check


            while (!Interface.IAdapter.IsMainConnectionOk)
            {

                ok = LinqDataContext.RestartSQLServer();

                if (ok) continue;

            
                string title = noConnectionDetected;
                title += Interface.IAdapter.Exception;

                Interface.IReport.SendToRestartRoutine(title);

                Cursor.Current = Cursors.Default;

                //could not connect
                MessageBox.Show(title, couldNotConnect, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //install SQL
                ok = installSQL();

                //restart server
                ok = LinqDataContext.RestartSQLServer();
                //    MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon i = MessageBoxIcon.Information;
                if (!ok) i = MessageBoxIcon.Error;
                MessageBox.Show(sqlStarted, triedToInstall, MessageBoxButtons.OK, i);

                //could not connect
                if (ok)
                {
                    string localDB = DB.Properties.Settings.Default.localDB;
                    ok = LinqDataContext.PopulateSQL(localDB);
                }
                else
                {

                }

                //send this text to a textFile in order to report by email next Reboot
            }
         

            Cursor.Current = Cursors.Default;

            return ok;
        }

        private static bool installSQL()
        {
            //problems withe the connection

            MessageBoxIcon i = MessageBoxIcon.Information;
            //should install SQL?
            DialogResult yesnot = MessageBox.Show(shouldInstallSQL, sqlLocalDB, MessageBoxButtons.YesNo, i);
            if (yesnot == DialogResult.Yes)
            {
                string path = Application.StartupPath;
                LinqDataContext.InstallSQLLocalDB(path);
                return true;
            }
            else
            {
                MessageBox.Show(nocontinueWOSQL, deniedTheInstall, MessageBoxButtons.OK, i);
                return false;
            }
            //try to load the server now
        }
     
}
}