using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Windows.Forms;

namespace Rsx.SQL
{
   
    /// <summary>
    /// Provides the Methods for SQL Population that are  more suited for User Screen Instructions
    /// </summary>
    public partial class SQLUI
    {
        protected static string chamgeConnectionString = "Change connection string?";

        protected static string changeConnection = "Would you like to mannually modify the current database connection string?\n\n"
            + "This is not necessary unless a remote server redirection is desired";

        protected static string defaultLocal = "(localdb)\\MSSQLLocalDB";
        protected static string deniedTheInstall = "\n\nThe user denied the SQL Server (LocalDB) installation";
        protected static string failureInstall = "\n\nInstallation of SQL LocalDB Failed!!!";
        protected static string nocontinueWOSQL = "Cannot continue without a SQL Server connection";
        protected static string okInstall = "\n\nInstallation of SQL LocalDB ran OK";
        protected static string restartAfterSQLFAIL = "Installation of SQL LocalDB did not work";
        protected static string restartAfterSQLOK = "Installation of the SQL Server went ok. Will attempt to populate the Database now...";
        protected static string shouldInstallSQL = "Would you like to install the SQL Server (LocalDB)?";
        protected static string sqlLocalDB = "SQL LocalDB Installation";
        protected static string sqlStarted = "Installation of the SQL Server (LocalDB) will execute now.\n\nWhen finished please click OK to continue";
        protected static string willInstall = "\n\nInstallation of the SQL Server LocalDB starting...";
    }
    public partial class SQLUI
    {
        /// <summary>
        /// Shows the UI to change the connection String localDB and returns a copy to an equivalent
        /// DB qith Dev name
        /// </summary>
        /// <param name="connectionControl"></param>
        /// <param name="localDB">          </param>
        /// <returns></returns>
        public static string ChangeConnectionString(ref UserControl connectionUsrControl, ref string localDB)
        {
            //make dynamic to access the 2 elements, whic foloows...
            dynamic connectionControl = connectionUsrControl;
            //check if database creation is needed
            //ask the USER NOW if he agrees wih the following Connection String
            connectionControl.Title = "LIMS Server";
            connectionControl.ConnectionString = localDB;

            DialogResult result = MessageBox.Show(chamgeConnectionString, changeConnection, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                Form form = new Form();

                form.StartPosition = FormStartPosition.CenterScreen;
                form.Text = "Check the Connection String";
                form.TopMost = true;

                form.AutoSizeMode = AutoSizeMode.GrowOnly;
                form.AutoSize = true;
                form.Size = new System.Drawing.Size(connectionControl.Width, connectionControl.Height);
                form.Controls.Add(connectionControl);

                form.ShowDialog();
                localDB = connectionControl.ConnectionString;
            }

            string catalogCmd = "Catalog=";

            //return a copy of the name for for Developer purposes
            return localDB.Replace(catalogCmd, catalogCmd + "Dev");

            // return localDB;
        }

        public static bool FindSQLInstances(ref string localDB, string developerFolder)
        {
            bool sqlFound = false;

            List<string> ls = null;
            ls = SQL.GetLocalSqlServerInstancesByCallingSqlWmi32();
            ls.AddRange(SQL.GetLocalSqlServerInstancesByCallingSqlWmi64());

            bool ok = false;
            MessageBoxButtons btn = MessageBoxButtons.OK;
            //No instances detected??
            if (ls.Count == 0)
            {
                MessageBoxIcon i = MessageBoxIcon.Question;
                MessageBox.Show(sqlStarted, willInstall, btn, i);
                //Install SQL

                ok = InstallSQL(developerFolder);

                i = MessageBoxIcon.Information;
                if (!ok) i = MessageBoxIcon.Error;

                //create SQL database
                if (ok)
                {
                    //installed LocalDB ok... go ahead and make default database
                    MessageBox.Show(restartAfterSQLOK, okInstall, btn, i);
                    //make database for first time!! (at default place
                    sqlFound = true;
                }
                else
                {
                    //could not install  LocalDB now WHAT??
                    //should work offline then...
                    MessageBox.Show(restartAfterSQLFAIL, failureInstall, btn, i);
                    sqlFound = false;

                    //TODO: poner algo que hacer si quieres ir offline
                }
            }
            else //there are other instances of SQL!!
            {
                sqlFound = true;
                //changed the database CONNECTION!!!
                //from default
                //to the server that you actually detected

                if (localDB.Contains(defaultLocal))
                {
                    localDB = localDB.Replace(defaultLocal, ls[0]);
                }

                //now store the new developerDB path!!!! VERY IMPORTANT
            }

            return sqlFound;
        }

        public static bool InstallSQL(string path)
        {
            bool ok = false;

            //should install SQL?
            DialogResult yesnot = MessageBox.Show(shouldInstallSQL, sqlLocalDB, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (yesnot == DialogResult.Yes)
            {
                // string path =;
                SQL.InstallSQL(path);
                ok = true;
            }
            else
            {
                MessageBox.Show(nocontinueWOSQL, deniedTheInstall, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // ok = false;
            }

            return ok;
        }
    }
}