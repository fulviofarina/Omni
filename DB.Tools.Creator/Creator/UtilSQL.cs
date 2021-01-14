using DB.Linq;
using DB.Properties;
using Rsx.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VTools;

namespace DB.Tools
{
    public static partial class Util
    {
        public static class UtilSQL
        {

            /*
        private static Interface Interface = null;

        public static void Set(ref Interface inter)
        {
            Interface = inter;
        }
            */



        public class Strings
            {
                // private static string defaultLocal = "(localdb)\\MSSQLLocalDB";
                public const string CHECKING_SQL_TITLE = "Please wait while testing the SQL connection...";

                public const string ABOUT_TO_POPULATE = "The program will now populate the Database for the first time\n\nWould you like to proceed?\n\nThis might take some time, please be patient\n\n" +
                    "Click NO to avoid the database population";

                public const string ABOUT_TO_POPULATE_TITLE = "Database population starting...";
                public const string ASK_TO_SAVE = "Changes in the database have not been saved yet\n\nDo you want to save the changes on the following tables?\n\n";

                // private static string chamgeConnectionString = "Would you like to change the Connection
                // string?"; private static string changeConnection = "Would you like to modify the
                // Connection string?\n\n";
                public const string CHECKING_SQL = "Checking the database connections";

                public const string HL_SRV = "HyperLab Server";
                public const string LIMS_SRV = "LIMS Server";
            }

         

            public static bool CheckConnectionsRoutine(bool msmq = true, bool lims = true, bool hyperLab = false)
            {
                bool ok = false;
                if (msmq)
                {
                    bool isMsmq = Interface.IReport.CheckMSMQ();
                    if (!isMsmq)
                    {
                        //this needs restart to activate so
                        //it will not give back a OK if queried
                        Interface.IReport.InstallMSMQ();
                    }
                }
                if (lims || hyperLab)
                {
                    //FIRST SQL

                    //this is the OK that matters..
                    //the connection
                    Interface.IReport.Msg("Set up", "Checking SQL Connection...");
                    Application.DoEvents();

                    if (!hyperLab)
                    {
                        ok = SQLPrepare(false);
                    }
                    else //is hyperlab
                    {
                        ok = PrepareSQLForHyperLab();
                        if (lims)
                        {
                            ok = ok && SQLPrepare(true);
                        }
                    }
                }

                Interface.IPreferences.SavePreferences();
                //CHECK RESTART FILE
                Interface.IReport.CheckRestartFile();

                return ok;
            }

            public static bool PrepareSQLForHyperLab()
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
                    Interface.IReport.Msg(Strings.CHECKING_SQL + " " + counter.ToString(), Strings.CHECKING_SQL_TITLE);
                    counter++;

                    // conn.Show();
                    bool updateSQLString = SQLUI.AskToUpdateSQLConnectionString();
                    if (!updateSQLString) break;

                    ok = setHyperLabConnection(ref defaultConnection);

                    if (ok) continue;
                }

                sQLPopulatorFinalize(ref adapterInitializer, ok);

                return ok;
            }

            private static bool setHyperLabConnection(ref string defaultConnection)
            {
                bool ok;
                IucSQLConnection connectionUsrCtrl = new ucSQLConnection();
                //offer user to change database string anyway!!!
                connectionUsrCtrl.Title = Strings.HL_SRV;
                defaultConnection = connectionUsrCtrl.ChangeConnectionString(ref defaultConnection, true, true);
                connectionUsrCtrl.Dispose();

                Interface.IAdapter.SetHyperLabConnection(defaultConnection);
                Interface.IPreferences.CurrentPref.HL = defaultConnection;
                ok = Interface.IAdapter.IsHyperLabConnectionOk;
                return ok;
            }

            /// <summary>
            /// Prepare the needed methods and the worker
            /// </summary>
            /// <param name="populNr"></param>
            /// <returns></returns>
            public static bool SQLPrepare(bool skipMSg = false)
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
                    Interface.IReport.Msg(Strings.CHECKING_SQL + " " + counter.ToString(), Strings.CHECKING_SQL_TITLE);
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
                    if (!skipMSg)
                    {
                        bool updateSQLString = SQLUI.AskToUpdateSQLConnectionString();
                        if (!updateSQLString) break;
                    }

                    setLIMSConnectionString(ref userDB, out developerDB, out defaultConnection, skipMSg);

                    DialogResult result = DialogResult.Yes;
                    if (!skipMSg)
                    {
                        result = MessageBox.Show(Strings.ABOUT_TO_POPULATE, Strings.ABOUT_TO_POPULATE_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    }
                    //populate new database...
                    if (result == DialogResult.Yes)
                    {
                        fillDatabase = true;
                        //set developer database string
                        defaultConnection = developerDB;
                    }
                    else
                    {
                        //just in case...
                        fillDatabase = false;
                        //TODO: populate?
                    }
                    //Cursor.Current = Cursors.WaitCursor;

                    Interface.IPreferences.CurrentPref.LIMS = defaultConnection;
                    // Interface.IPreferences.SavePreferences();
                    if (fillDatabase)
                    {
                        //populate developer
                        fillDatabase = LinqDataContext.PopulateSQL(developerDB, true);
                    }

                    Interface.IAdapter.SetMainConnection(defaultConnection);

                    //adapterInitializer.Invoke();
                }

                if (fillDatabase)
                {
                    ok = sQLDatabaseClone(userDB, developerDB);
                }

                //SAVE SETTINGS!!!
                sQLPopulatorFinalize(ref adapterInitializer, ok);

                return ok;
            }

            private static void sQLPopulatorFinalize(ref Action adapterInitializer, bool ok)
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

            private static bool sQLDatabaseClone(string userDB, string developerDB)
            {
                bool ok = false;
                //now populate developer Database first and send data there
                //afterwards, you copy what you need to copy to USER Database...
                //    bool makeDatabase = false;

                //MAE A COPY INTO THE DEVELOPER DB SQL
                //read the backup file
                Interface.IStore.ReadDefaultLIMS();
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

                //now clone to the USER!!!
                //o do something more selective!
                //DEVELOPER MODE COPY
                ok = LinqDataContext.PopulateSQL(userDB, true, developerDB);
                //again, restore the string to the User STRING
                Interface.IPreferences.PopulatePreferences();
                Interface.IPreferences.CurrentPref.LIMS = userDB;

                //always delete the developer database if it got stucked next restart...
                SQL.DeleteDatabase(developerDB);
                return ok;
            }

            private static void setLIMSConnectionString(ref string userDB, out string developerDB, out string defaultConnection, bool skipMsg)
            {
                //provide path to SQL files for deploy (installation)
                string path = Interface.IStore.DevPath;
                string sqlServerFound = SQLUI.FindSQLOrInstall(path, skipMsg);
                //IMPORTANTE, cambia el string el usuario o el default!
                bool sqlFound = !string.IsNullOrEmpty(sqlServerFound);
                if (sqlFound)
                {
                    SQL.ReplaceLocalDBDefaultPath(ref userDB, sqlServerFound);
                    //store later
                }
                //2
                //offer user to change database string anyway!!!
                IucSQLConnection connectionUsrControl = new ucSQLConnection();

                connectionUsrControl.Title = Strings.LIMS_SRV;
                string connection = connectionUsrControl.ChangeConnectionString(ref userDB, skipMsg, !skipMsg);
                connectionUsrControl.Dispose();

                developerDB = SQL.ReplaceStringForDeveloper(connection);
                //return a copy of the name for for Developer purposes
                //3
                //set local database as default
                defaultConnection = userDB;
                //chequea si ya tiene servidores SQL
            }

            public static void ConnectionsUI()
            {
                LINAA.PreferencesRow prefe = Interface.IPreferences.CurrentPref;
                Action<SystemException> addException = Interface.IStore.AddException;
                Action savemethod = Interface.IPreferences.SavePreferences;
                Action undoMethod = Interface.IPreferences.CurrentPref.RejectChanges;

                Connections.ConnectionsUI(ref prefe, ref addException, ref savemethod, ref undoMethod);
            }
        }
   }
}