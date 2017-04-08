namespace DB.LINAATableAdapters
{
    public partial class MeasurementsTableAdapter
    {
        public void SetForHL()
        {
            this.Connection.ConnectionString = DB.Properties.Settings.Default.HLSNMNAAConnectionString;
        }

        public void SetForLIMS()
        {
            this.Connection.ConnectionString = DB.Properties.Settings.Default.NAAConnectionString;
        }
    }
}