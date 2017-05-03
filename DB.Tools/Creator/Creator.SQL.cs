using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Linq;
using Rsx.SQL;

namespace DB.Tools
{
    public partial class Creator
    {
   //     private static string defaultLocal = "(localdb)\\MSSQLLocalDB";


        private static string aboutToPopulate = "The program will now populate the Database for the first time\n\nWould you like to proceed?\n\n" +
            "Click NO to avoid the database population";

        private static string aboutToPopulateTitle = "Database population starting...";
      //  private static string chamgeConnectionString = "Would you like to change the Connection string?";
      //  private static string changeConnection = "Would you like to modify the Connection string?\n\n";
        private static string checkingSQL = "Checking the database connections";
        private static string couldNotConnect = "Connection to the database failed";
      //  private static string failureInstall = "\n\nInstallation of SQL LocalDB Failed!!!";

        private static string noConnectionDetected = "The current database connection is not ok.\n\n" +
            "The reasons might be:\n\n" +
               "1) The program database does not exist (first time users)\n\n"+
            "2) The SQL Server is down/stopped\n\n" +
            "3) The SQL Server is not installed on this computer\n\n" +
            "4) The Connection string to the database is wrong.\n\n\n" +
            "This program will attempt to:\n\n" +
            "1) Restart the server\n" +
            "2) Detect other SQL Server instances when present or,\n" +
            "3) Reinstall the SQL Server and the program database.\n\n\n" +
            "You will have the option to change the connection string (if desired)";
     //   private static string deniedTheInstall = "\n\nThe user denied the SQL Express installation";

      //
        // + "Click NO if you want to proceed with the default connection string.\n\nClick Cancel to
        // avoid the population of the Database";
    }

    public partial class Creator
    {
        /// <summary>
        /// Prepare the needed methods and the worker
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool PrepareSQL(ref UserControl connectionUsrControl)
        {
            Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg(checkingSQL, "Please wait while testing the SQL connection...");

            Interface.IAdapter.InitializeComponent();
            Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

            bool makeDatabase = false;

            bool sqlFound = false;

     
            string localDB = Properties.Settings.Default.localDB;

            string developerDB = Properties.Settings.Default.developerDB;

            bool ok = Interface.IAdapter.IsMainConnectionOk;
            //No connections
            while (!ok)
            {
                //restart server SQL
                ok = SQL.RestartSQLLocalDBServer();
                //check connection again after restarting server
                ok = Interface.IAdapter.IsMainConnectionOk;
                //restarting the server didn't work, plan B
                if (ok) continue;

                Cursor.Current = Cursors.Default;
             
                //show no connection Intro
                //could not connect
              //  MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBox.Show(noConnectionDetected, couldNotConnect, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                string path  = Application.StartupPath + DB.Properties.Resources.DevFiles;
                sqlFound = SQLUI.FindSQLInstances(ref localDB, path);

                //IMPORTANTE, así crea cualquier database con DEV delante
                developerDB = SQLUI.ChangeConnectionString(ref connectionUsrControl, ref localDB);

                Interface.IPreferences.CurrentPref.LIMS = localDB;
          
                //should populate the database?
                if (sqlFound)
                {
                    //ask to populate or Not
                    DialogResult result = MessageBox.Show(aboutToPopulate, aboutToPopulateTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //populate new database...
                    if (result == DialogResult.Yes)
                    {
                        makeDatabase = true;
                        //set developer database string
                        Interface.IPreferences.CurrentPref.LIMS = developerDB;
                    }

                    ok = true;
                }

                //VEEEERY IMPORTANT, SAVES PREFERNCES AND SETTINGS!!!!
                Properties.Settings.Default["developerDB"] = developerDB;
                Properties.Settings.Default["localDB"] = localDB;

                Interface.IPreferences.CurrentPref.Check();

                Cursor.Current = Cursors.WaitCursor;
            }

            if (makeDatabase)
            {
                Interface.IAdapter.DisposeAdapters();
                Interface.IAdapter.InitializeComponent();
            }

            Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

            if (makeDatabase)
            {
                //now populate developer Database first and send data there
                //afterwards, you copy what you need to copy to USER Database...
                //    bool makeDatabase = false;
                makeDatabase = LinqDataContext.PopulateSQL(developerDB, true);

            }

            if (makeDatabase)
            {

                //MAE A COPY INTO THE DEVELOPER DB SQL
                Interface.IMain.Read(Interface.IMain.FolderPath + DB.Properties.Resources.Linaa);
                DataSet set = Interface.Get();
                IEnumerable<DataTable> tables = set.Tables.OfType<DataTable>();
                //save
                Interface.IStore.SaveRemote(ref tables, true);
                //again, restore the string to the User STRING
                Interface.IAdapter.DisposeAdapters();
                //now clone to the USER!!!
                //o do something more selective!
                //DEVELOPER MODE COPY
                ok = LinqDataContext.PopulateSQL(localDB, false, developerDB);
                SQL.DeleteDatabase(developerDB);

                Interface.IPreferences.CurrentPref.LIMS = localDB;
                //SAVE AGAIN!!!
                Interface.IPreferences.CurrentPref.Check();
                //again restart Adapters...
            }

            Interface.IPreferences.CurrentPref.IsSQL = ok;

            Interface.IPreferences.SavePreferences();
       

            Cursor.Current = Cursors.Default;

            if (makeDatabase)
            {
                Interface.IReport.SendToRestartRoutine(Interface.IAdapter.Exception);
                Rsx.Dumb.RestartPC();
                Application.ExitThread();
            }

            return ok;
        }

      



    }
}