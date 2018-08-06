namespace DB.Tools
{
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

        protected static string NO_CONNECTION = "The default database connection is not properly configured.\n\n" +
            "The reasons might be:\n\n" +
               "- You are a first-time user\n\n" +
            "- The database does not exist\n\n" +
            "- The SQL Server is down/stopped or not installed\n\n" +
            // "3) The SQL Server is not installed\n\n" +
            "- The connection parameters are wrong.\n\n\n" +
            "This program will attempt to:\n\n" +
            //"Perform basic connection routines,\n"+
            "Restart the server,\n\n" +
            "Detect other SQL Server instances (when present),\n\n" +
            "Reinstall the SQL Server and database (when applicable and after confirmation),\n\n" +
            "Offer you to change the connection parameters\n\n\n";

        protected static string NO_CONNECTION_TITLE = "SQL Server Connection Wizard by F. Farina Arboccò";
    }
}