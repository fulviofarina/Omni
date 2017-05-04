using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rsx.SQL
{
    /// <summary>
    /// Provides the Methods for SQL Population that are more suited for User Screen Instructions
    /// </summary>
    public partial class SQLUI
    {
        protected static string CONNECTION_CHANGE = "Would you like to manually modify the current database connection string?\n\n"
            + "This is not necessary unless a remote server redirection is desired";

        protected static string CONNECTION_CHANGE_TITLE = "Change connection string?";
        protected static string LOCALDB_DEFAULT_PATH = "(localdb)\\MSSQLLocalDB";
        protected static string SQL_DENIED_INSTALL = "\n\nThe user denied the SQL Server (LocalDB) installation";
        protected static string SQL_DENIED_INSTALL_TITLE = "Cannot continue without a SQL Server connection";
        protected static string SQL_INSTALL_ASK = "Would you like to install the SQL Server (LocalDB)?";
        protected static string SQL_INSTALL_ASK_TITLE = "SQL LocalDB Installation";
        protected static string SQL_INSTALL_FAILURE = "\n\nInstallation of SQL LocalDB Failed!!!";
        protected static string SQL_INSTALL_FAILURE_TITLE = "Installation of SQL LocalDB did not work";
        protected static string SQL_INSTALL_OK = "\n\nInstallation of SQL LocalDB ran OK";
        protected static string SQL_INSTALL_OK_TITLE = "Installation of the SQL Server went ok. Will attempt to populate the Database now...";
        protected static string SQL_INSTALL_STARTED = "Installation of the SQL Server (LocalDB) will execute now.\n\nWhen finished please click OK to continue";
        protected static string SQL_INSTALL_STARTED_TITLE = "\n\nInstallation of the SQL Server LocalDB starting...";

       
    }

   
}