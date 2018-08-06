using DB.Linq;
using DB.Properties;
using Rsx.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DB.Tools
{
    public partial class Creator
    {
        public static string MainLIMSResource { get; set; }


        public static bool PrepareSQLForHyperLab(ref VTools.IucSQLConnection conn)
        {
          
            string defaultConnection = Settings.Default.HLSNMNAAConnectionString;

            Action adapterInitializer = delegate
            {
                Interface.IAdapter.InitializeComponent();
                Interface.IAdapter.InitializePeaksAdapters(true); //why was this after the next code? //check
            };

            adapterInitializer.Invoke();

            bool ok = false;
            ok = Interface.IAdapter.IsHyperLabConnectionOk;

            int counter = 1;


            //No connections
            while (!ok)
            {
                Interface.IReport.Msg(CHECKING_SQL + " " + counter.ToString(), CHECKING_SQL_TITLE);
                counter++;

                //    conn.Show();
                MessageBox.Show(NO_CONNECTION, NO_CONNECTION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //offer user to change database string anyway!!!
                conn.Title = "HyperLab Server";
                defaultConnection = conn.ChangeConnectionString( ref defaultConnection, true,true);


                Interface.IAdapter.SetHyperLabConnection(defaultConnection);

                Interface.IPreferences.CurrentPref.HL = defaultConnection;

                ok = Interface.IAdapter.IsHyperLabConnectionOk;
           
                if (ok) continue;


            }

            finalizeSQLPopulator(ref adapterInitializer, ok);


            return ok;
        }

        /// <summary>
        /// Prepare the needed methods and the worker
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool PrepareSQL(ref VTools.IucSQLConnection connectionUsrControl, bool skipMSg = false)
        {

            Action adapterInitializer = delegate
            {
                Interface.IAdapter.InitializeComponent();
                Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check
            };

            adapterInitializer.Invoke();





            bool ok = Interface.IAdapter.IsMainConnectionOk;


            string userDB = Settings.Default.localDB;
            string developerDB = Settings.Default.developerDB;


            bool fillDatabase = false;
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

                string defaultConnection = string.Empty;
                //show no connection Intro
                //could not connect
               if (!skipMSg)  MessageBox.Show(NO_CONNECTION, NO_CONNECTION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                populateDeveloperSQLDatabase(ref connectionUsrControl, out fillDatabase, ref userDB, out developerDB, out defaultConnection, skipMSg);



                Interface.IAdapter.SetMainConnection(defaultConnection);

                //adapterInitializer.Invoke();

            }

            if (fillDatabase)
            {
                ok = clonetoSQLDatabase(userDB, developerDB);
            }

            //SAVE SETTINGS!!!
            finalizeSQLPopulator(ref adapterInitializer, ok);
          
            return ok;
        }

        private static void finalizeSQLPopulator(ref Action adapterInitializer, bool ok)
        {
            Interface.IPreferences.CurrentPref.Check();
            //     Interface.IPreferences.SavePreferences();
            //again restart Adapters...
            //  
            adapterInitializer.Invoke();
            // Cursor.Current = Cursors.Default;
            Interface.IReport.SendToRestartRoutine(Interface.IAdapter.Exception);

            Interface.IPreferences.CurrentPref.IsSQL = ok;
        }

        private static bool clonetoSQLDatabase(string userDB, string developerDB)
        {
            bool ok = false;
            //now populate developer Database first and send data there
            //afterwards, you copy what you need to copy to USER Database...
            //    bool makeDatabase = false;

            //MAE A COPY INTO THE DEVELOPER DB SQL
            //read the backup file
            LoadFromFile();
            Interface.IStore.CleanOthers();
            Interface.IDB.Compositions.Clear();
            Interface.IPopulate.INuclear.CleanSigmas();
            Interface.IPreferences.PopulatePreferences();

            foreach (var item in Interface.IDB.Matrix)
            {
                item.SetCompositionTableNull();
                // item.SetXCOMTableNull();
            }
            // CleanOthers();
            IEnumerable<DataTable> tables = Interface.Get().Tables.OfType<DataTable>();
            //save
            Interface.IStore.SaveTables(ref tables);
            Interface.IAdapter.DisposeAdapters();

            ok = cloneDatabase(userDB, developerDB);
            //always delete the developer database if it got stucked next restart...
            SQL.DeleteDatabase(developerDB);
            return ok;
        }

        private static void populateDeveloperSQLDatabase(ref VTools.IucSQLConnection connectionUsrControl, out bool makeDatabase, ref string userDB, out string developerDB, out string defaultConnection, bool skipMsg )
        {
        

            //provide path to SQL files for deploy (installation)
            string path = Application.StartupPath + Resources.DevFiles;
            string sqlServerFound = SQLUI.FindSQLOrInstall(path, skipMsg);
            //IMPORTANTE, cambia el string el usuario o el default!
            bool sqlFound = !string.IsNullOrEmpty(sqlServerFound);
            if (sqlFound)
            {
                SQLUI.ReplaceLocalDBDefaultPath(ref userDB, sqlServerFound);
                //store later
            }
            //2
            //offer user to change database string anyway!!!
            connectionUsrControl.Title = "LIMS Server";
            developerDB = connectionUsrControl.ChangeConnectionString(ref userDB,skipMsg, !skipMsg);

            developerDB= SQL.ReplaceStringForDeveloper(developerDB);
            //return a copy of the name for for Developer purposes
        
            //3
            //set local database as default
            defaultConnection = userDB;
            //chequea si ya tiene servidores SQL

            //ask to populate or Not
            //   Cursor.Current = Cursors.Default;

            DialogResult result = DialogResult.Yes;
            if (!skipMsg)
            {
                result = MessageBox.Show(ABOUT_TO_POPULATE, ABOUT_TO_POPULATE_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
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
        }

        public static void ConnectionsUI()
        {
            LINAA.PreferencesRow prefe = Interface.IPreferences.CurrentPref;
            Action<SystemException> addException = Interface.IStore.AddException;
            Action savemethod = Interface.IPreferences.SavePreferences;
            Action undoMethod = Interface.IPreferences.CurrentPref.RejectChanges;

            Connections.ConnectionsUI(ref prefe, ref addException, ref savemethod, ref undoMethod);
        }

        protected static bool cloneDatabase(string localDB, string developerDB)
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
}