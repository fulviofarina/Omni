using System;
using System.Collections.Generic;
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
        private static string sqlStarted = "Installation of SQL LocalDB will execute now.\n\nWhen finished please click OK to continue";
        private static string restartAfterSQLFAIL = "Installation of SQL LocalDB did not work";
        private static string restartAfterSQLOK = "Installation of SQL ok. Will try to restart the connection now...";

        private static string willInstall = "\n\nInstallation of SQL LocalDB starting...";

        private static string okInstall = "\n\nInstallation of SQL LocalDB ran OK";
        private static string failureInstall = "\n\nInstallation of SQL LocalDB Failed!!!";

        private static string couldNotConnect = "Could not connect to LIMS DataBase";
        private static string noConnectionDetected = "Please check the LIMS Database connection.\n" +
            "The Server might be down, not installed, misconfigured or just offline.\n\n";


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

            Interface.IReport.Msg(checkingSQL, "Please wait while testing the SQL connection...");
         

            Interface.IAdapter.InitializeComponent();

            bool ok = Interface.IAdapter.IsMainConnectionOk;

            //No connections
            while (!ok)
            {

                //restart server SQL
                ok = LinqDataContext.RestartSQLServer();

                //check connection again after restarting server
                ok = Interface.IAdapter.IsMainConnectionOk;

                //restarting the server didn't work, plan B
                if (ok) continue;

                Cursor.Current = Cursors.Default;
                //show no connection Intro
                //could not connect
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBox.Show(noConnectionDetected, couldNotConnect, btn, MessageBoxIcon.Error);
                
                //make dictionary (plan B)
             //   SQL.MakeDictionary();
            
                //make a list of SQL instances
                List<string> ls = null;
                ls = Rsx.SQL.GetLocalSqlServerInstancesByCallingSqlWmi32();
                ls.AddRange(Rsx.SQL.GetLocalSqlServerInstancesByCallingSqlWmi64());

                string localDB = DB.Properties.Settings.Default.localDB;
                bool makeDatabase = false;
                //No instances detected??
                if (ls.Count == 0)
                {
                 
                    MessageBoxIcon i = MessageBoxIcon.Information;
                    MessageBox.Show(sqlStarted, willInstall, btn, i);
                    //Install SQL
                    ok = installSQL();

                     i = MessageBoxIcon.Information;
                    if (!ok) i = MessageBoxIcon.Error;
               
                    //create SQL database
                    if (ok)
                    {
                        //installed LocalDB ok... go ahead and make default database
                        MessageBox.Show(restartAfterSQLOK, okInstall, btn, i);
                        //make database for first time!! (at default place
                        makeDatabase = true;
                    }
                    else
                    {
                        //could not install  LocalDB now WHAT??
                        //should work offline then...
                        MessageBox.Show(restartAfterSQLFAIL, failureInstall, btn, i);
                        Interface.IReport.SendToRestartRoutine(Interface.IAdapter.Exception);

                        //TODO: poner algo que hacer si quieres ir offline
                    }

                }
             else //there are other instances of SQL!!
                {
                    makeDatabase = true;
                    //changed the database CONNECTION!!!
                    if (ls.Count==1)
                    {
                        string defaultLocal = "(localdb)\\MSSQLLocalDB";
                        localDB = localDB.Replace(defaultLocal, ls[0]);
                     //   localDB+= ";Integrated Security=True";
                    }
                    else
                    {
                        //the user should pick
                    }

                }
             
           
                //check if database creation is needed
                if (makeDatabase)
                {
                    //save the new connevtion string
                    Interface.IPreferences.CurrentPref.LIMS = localDB;
                    //populate new database...
                    ok = LinqDataContext.PopulateSQL(localDB);
                    //important to save connection to the Settings Class
                    Interface.IPreferences.CurrentPref.Check();
                }
                Cursor.Current = Cursors.WaitCursor;
            }


           if (ok) Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

            Interface.IPreferences.CurrentPref.IsSQL = ok;

            Cursor.Current = Cursors.Default;

            return ok;
        }

        private static bool installSQL()
        {

            bool ok;


        
            //install SQL
            //problems withe the connection

            MessageBoxIcon i = MessageBoxIcon.Information;
            //should install SQL?
            DialogResult yesnot = MessageBox.Show(shouldInstallSQL, sqlLocalDB, MessageBoxButtons.YesNo, i);
            if (yesnot == DialogResult.Yes)
            {
                string path = Application.StartupPath + DB.Properties.Resources.DevFiles;
                LinqDataContext.InstallSQLLocalDB(path);
                ok = true;
            }
            else
            {
                MessageBox.Show(nocontinueWOSQL, deniedTheInstall, MessageBoxButtons.OK, i);
                ok = false;
            }

      
       

            return ok;
        }

     
     
}
}