using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Rsx.SQL;

namespace DB.Tools
{
    public partial class Creator
    {
        /// <summary>
        /// Prepare the needed methods and the worker
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool PrepareSQL(ref UserControl connectionUsrControl)
        {
        //    Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg("Set up", "Checking SQL...");
            Application.DoEvents();

            Action adapterInitializer = delegate
           {
               Interface.IAdapter.InitializeComponent();
               Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

           };


            adapterInitializer.Invoke();
         
            bool makeDatabase = false;

            string userDB = Settings.Default.localDB;
            string developerDB = Settings.Default.developerDB;
            string defaultConnection = string.Empty;
            bool ok = Interface.IAdapter.IsMainConnectionOk;
            int counter = 1;
            //No connections
            while (!ok)
            {
                Interface.IReport.Msg(CHECKING_SQL + " " + counter.ToString(), CHECKING_SQL_TITLE);
                counter++;
                //restart server SQL
                ok = SQL.RestartSQLLocalDBServer();

                ok = Interface.IAdapter.IsMainConnectionOk;
                //check connection again after restarting server
                //    ok = Interface.IAdapter.IsMainConnectionOk;
                //restarting the server didn't work, plan B
                if (ok) continue;

                //show no connection Intro
                //could not connect
              //  Cursor.Current = Cursors.Default;
                MessageBox.Show(NO_CONNECTION, NO_CONNECTION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
//
        //        Cursor.Current = Cursors.WaitCursor;

                //provide path to SQL files for deploy (installation)
                string path = Application.StartupPath + DB.Properties.Resources.DevFiles;
                string sqlServerFound = SQLUI.FindSQLOrInstall(path);
                //IMPORTANTE, cambia el string el usuario o el default!
                bool sqlFound = !string.IsNullOrEmpty(sqlServerFound);
                if (sqlFound)
                {
                    SQLUI.ReplaceLocalDBDefaultPath(ref userDB, sqlServerFound);
                    //store later
                }
                //2
                //offer user to change database string anyway!!!
                developerDB = SQLUI.ChangeConnectionString(ref connectionUsrControl, ref userDB);
                //3
                //set local database as default
                defaultConnection = userDB;
                //chequea si ya tiene servidores SQL

                //ask to populate or Not
             //   Cursor.Current = Cursors.Default;

                DialogResult result = MessageBox
                    .Show(ABOUT_TO_POPULATE, ABOUT_TO_POPULATE_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //populate new database...
                if (result == DialogResult.Yes)
                {
                    makeDatabase = true;
                    //set developer database string
                    defaultConnection = developerDB;
                }
                else
                {
                    //just in case...
                    makeDatabase = false;
                    //TODO: populate?
                }
//Cursor.Current = Cursors.WaitCursor;

                Interface.IPreferences.CurrentPref.LIMS = defaultConnection;
                // Interface.IPreferences.SavePreferences();
                if (makeDatabase)
                {
                    makeDatabase = LinqDataContext.PopulateSQL(developerDB, true);
                }

                Interface.IAdapter.DisposeAdapters();

                Interface.IAdapter.SetConnections(defaultConnection);


                adapterInitializer.Invoke();

                // Cursor.Current = Cursors.WaitCursor;
            }

            if (makeDatabase)
            {
                //now populate developer Database first and send data there
                //afterwards, you copy what you need to copy to USER Database...
                //    bool makeDatabase = false;

                //MAE A COPY INTO THE DEVELOPER DB SQL
                //read the backup file
                LoadFromFile();
                Interface.IStore.CleanOthers();
                Interface.IDB.Compositions.Clear();
                Interface.IPopulate.INuclear.CleanSigmas();
                PopulateBasic();
          
                foreach (var item in Interface.IDB.Matrix)
                {
                    item.SetCompositionTableNull();
                    item.SetXCOMTableNull();
                }
                // CleanOthers();
                IEnumerable<DataTable> tables = Interface.Get().Tables.OfType<DataTable>();
                //save
                Interface.IStore.SaveRemote(ref tables);
                Interface.IAdapter.DisposeAdapters();

                ok = cloneDatabase(userDB, developerDB);
                //always delete the developer database if it got stucked next restart...
                SQL.DeleteDatabase(developerDB);
            }

            //SAVE SETTINGS!!!

            Interface.IPreferences.CurrentPref.Check();
            //again restart Adapters...
            adapterInitializer.Invoke();


        //    Cursor.Current = Cursors.Default;
            Interface.IReport.SendToRestartRoutine(Interface.IAdapter.Exception);

            Interface.IPreferences.CurrentPref.IsSQL = ok;

            return ok;
        }

        private static bool cloneDatabase(string localDB, string developerDB)
        {
            bool ok;
          
            //now clone to the USER!!!
            //o do something more selective!
            //DEVELOPER MODE COPY
            ok = LinqDataContext.PopulateSQL(localDB, true, developerDB);
            //again, restore the string to the User STRING
            Interface.IPreferences.PopulatePreferences();
            Interface.IPreferences.CurrentPref.LIMS = localDB;
            return ok;
        }
    }

    public partial class Creator
    {
        // private static string defaultLocal = "(localdb)\\MSSQLLocalDB";
        protected static string CHECKING_SQL_TITLE = "Please wait while testing the SQL connection...";

        protected static string ABOUT_TO_POPULATE = "The program will now populate the Database for the first time\n\nWould you like to proceed?\n\nThis might take some time, please be patient\n\n" +
            "Click NO to avoid the database population";

        protected static string ABOUT_TO_POPULATE_TITLE = "Database population starting...";
        protected static string ASK_TO_SAVE = "Changes in the database have not been saved yet\n\nDo you want to save the changes on the following tables?\n\n";

        // private static string chamgeConnectionString = "Would you like to change the Connection
        // string?"; private static string changeConnection = "Would you like to modify the
        // Connection string?\n\n";
        protected static string CHECKING_SQL = "Checking the database connections";

        protected static string NO_CONNECTION = "The current database connection is not ok.\n\n" +
            "The reasons might be:\n\n" +
               "1) The program database does not exist (first time users)\n" +
            "2) The SQL Server is down/stopped\n" +
            "3) The SQL Server is not installed on this computer\n" +
            "4) The Connection string to the database is wrong.\n\n\n" +
            "This program will attempt to:\n\n" +
            "1) Restart the server\n" +
            "2) Detect other SQL Server instances when present or,\n" +
            "3) Reinstall the SQL Server and the program database.\n\n" +
            "You will have the option to change the connection string (if desired)";

        protected static string NO_CONNECTION_TITLE = "Connection to the database failed";
    }
}