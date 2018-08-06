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

        /// <summary>
        /// Prepare the needed methods and the worker
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool PrepareSQL(ref UserControl connectionUsrControl)
        {

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

                fixMainSQLConnection(ref connectionUsrControl, out makeDatabase, ref userDB, out developerDB, out defaultConnection);

                Interface.IAdapter.DisposeAdapters();

                Interface.IAdapter.SetConnections(defaultConnection);

                adapterInitializer.Invoke();

            }

            if (makeDatabase)
            {
                ok = makeMainSQLDatabase(userDB, developerDB);
            }

            //SAVE SETTINGS!!!

            Interface.IPreferences.CurrentPref.Check();
            //again restart Adapters...
            adapterInitializer.Invoke();

            // Cursor.Current = Cursors.Default;
            Interface.IReport.SendToRestartRoutine(Interface.IAdapter.Exception);

            Interface.IPreferences.CurrentPref.IsSQL = ok;

            return ok;
        }

        private static bool makeMainSQLDatabase(string userDB, string developerDB)
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

        private static void fixMainSQLConnection(ref UserControl connectionUsrControl, out bool makeDatabase, ref string userDB, out string developerDB, out string defaultConnection)
        {
            //show no connection Intro
            //could not connect
            //  Cursor.Current = Cursors.Default;
            MessageBox.Show(NO_CONNECTION, NO_CONNECTION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // Cursor.Current = Cursors.WaitCursor;

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