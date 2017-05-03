namespace VTools
{
    public interface IucSQLConnection
    {
        string ConnectionString { get; set; }
        string Title { get; set; }
        string Database { get; set; }
    }
}