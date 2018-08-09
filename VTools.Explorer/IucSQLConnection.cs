namespace VTools
{
    public interface IucSQLConnection
    {
        string ConnectionString { get; set; }
        string Title { get; set; }
        string Database { get; set; }

        string ChangeConnectionString(ref string defaultConnection, bool skipMsg = false, bool showDialog = true);
        void Dispose();
    }
}