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

      
    }
}