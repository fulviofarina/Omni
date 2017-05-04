using System.Data.Linq;

using Rsx.SQL;

namespace DB.Linq
{
    public partial class LinqDataContext
    {
        public static void CloneSQLDatabase(ref DataContext original, ref DataContext destiny)
        {
            ITable dt = original.GetTable<Unit>();
            ITable ita = destiny.GetTable(typeof(Unit));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Matrix>();
            ita = destiny.GetTable(typeof(Matrix));
            SQL.InsertDataTable(ref dt, ref ita);
            dt = original.GetTable<Composition>();
            ita = destiny.GetTable(typeof(Composition));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Reaction>();
            ita = destiny.GetTable(typeof(Reaction));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Sigma>();
            ita = destiny.GetTable(typeof(Sigma));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<SigmasSal>();
            ita = destiny.GetTable(typeof(SigmasSal));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Element>();
            ita = destiny.GetTable(typeof(Element));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<MonitorsFlag>();
            ita = destiny.GetTable(typeof(MonitorsFlag));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Yield>();
            ita = destiny.GetTable(typeof(Yield));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<tStudent>();
            ita = destiny.GetTable(typeof(tStudent));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<LINE>();
            ita = destiny.GetTable(typeof(LINE));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<LINES_FI>();
            ita = destiny.GetTable(typeof(LINES_FI));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<DetectorsAbsorber>();
            ita = destiny.GetTable(typeof(DetectorsAbsorber));
            SQL.InsertDataTable(ref dt, ref ita);
            dt = original.GetTable<DetectorsCurve>();
            ita = destiny.GetTable(typeof(DetectorsCurve));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<DetectorsDimension>();
            ita = destiny.GetTable(typeof(DetectorsDimension));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Holder>();
            ita = destiny.GetTable(typeof(Holder));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<VialType>();
            ita = destiny.GetTable(typeof(VialType));
            SQL.InsertDataTable(ref dt, ref ita);
            dt = original.GetTable<Standard>();
            ita = destiny.GetTable(typeof(Standard));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Monitor>();
            ita = destiny.GetTable(typeof(Monitor));
            SQL.InsertDataTable(ref dt, ref ita);
            dt = original.GetTable<Channel>();
            ita = destiny.GetTable(typeof(Channel));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<Geometry>();
            ita = destiny.GetTable(typeof(Geometry));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<NAA>();
            ita = destiny.GetTable(typeof(NAA));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<k0NAA>();
            ita = destiny.GetTable(typeof(k0NAA));
            SQL.InsertDataTable(ref dt, ref ita);
        }

        public static bool PopulateSQL(string localDBPath, bool cleanSheet = false, string backupString = "")
        {
            //create the database if it does not exist
            bool exist = false;
            // LinqDataContext destin
            if (string.IsNullOrEmpty(localDBPath)) return exist;
            if (cleanSheet)
            {
                exist = SQL.DeleteDatabase(localDBPath);
            }

            // bool exist = false;
            DataContext destiny = new LinqDataContext(localDBPath);

            exist = destiny.DatabaseExists();
            if (!exist) destiny.CreateDatabase();
            exist = destiny.DatabaseExists();
            if (!exist) return exist;

            if (string.IsNullOrEmpty(backupString)) return exist;
            DataContext original = new LinqDataContext(backupString);
            exist = original.DatabaseExists();
            CloneSQLDatabase(ref original, ref destiny);

            original.Connection.Close();
            destiny.Connection.Close();
            return exist;
        }
    }
}