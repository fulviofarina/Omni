using Rsx.SQL;
using System.Data.Linq;

namespace DB.Linq
{
    public partial class LinqDataContext
    {
        public static void CloneSQLDatabase(ref DataContext original, ref DataContext destiny)
        {
            ITable dt = original.GetTable<Unit>();
            ITable ita = destiny.GetTable(typeof(Unit));

            SQL.InsertDataTable(ref dt, ref ita, "Unit");

            dt = original.GetTable<ContactPerson>();
            ita = destiny.GetTable(typeof(ContactPerson));
            SQL.InsertDataTable(ref dt, ref ita, "ContactPersons");

            dt = original.GetTable<Customer>();
            ita = destiny.GetTable(typeof(Customer));
            SQL.InsertDataTable(ref dt, ref ita, "Customer");

            dt = original.GetTable<Company>();
            ita = destiny.GetTable(typeof(Company));
            SQL.InsertDataTable(ref dt, ref ita, "Companies");

            dt = original.GetTable<Channel>();
            ita = destiny.GetTable(typeof(Channel));
            SQL.InsertDataTable(ref dt, ref ita, "Channels");

            dt = original.GetTable<ElementsCorrectionFactor>();
            ita = destiny.GetTable(typeof(ElementsCorrectionFactor));
            SQL.InsertDataTable(ref dt, ref ita, "ElementsCorrectionFactors");

            dt = original.GetTable<Matrix>();
            ita = destiny.GetTable(typeof(Matrix));
            SQL.InsertDataTable(ref dt, ref ita, "Matrix");

            dt = original.GetTable<Composition>();
            ita = destiny.GetTable(typeof(Composition));
            SQL.InsertDataTable(ref dt, ref ita, "Compositions");

            dt = original.GetTable<MUE>();
            ita = destiny.GetTable(typeof(MUE));
            SQL.InsertDataTable(ref dt, ref ita, "MUES");

            dt = original.GetTable<Sample>();
            ita = destiny.GetTable(typeof(Sample));
            SQL.InsertDataTable(ref dt, ref ita, "Samples");

            dt = original.GetTable<SubSample>();
            ita = destiny.GetTable(typeof(SubSample));
            SQL.InsertDataTable(ref dt, ref ita, "SubSamples");

            dt = original.GetTable<Standard>();
            ita = destiny.GetTable(typeof(Standard));
            SQL.InsertDataTable(ref dt, ref ita, "Standards");

            dt = original.GetTable<Monitor>();
            ita = destiny.GetTable(typeof(Monitor));
            SQL.InsertDataTable(ref dt, ref ita, "Monitors");

            dt = original.GetTable<MonitorsFlag>();
            ita = destiny.GetTable(typeof(MonitorsFlag));
            SQL.InsertDataTable(ref dt, ref ita, "MonitorsFlags");

            dt = original.GetTable<RefMaterial>();
            ita = destiny.GetTable(typeof(RefMaterial));
            SQL.InsertDataTable(ref dt, ref ita, "RefMaterials");

            dt = original.GetTable<Blank>();
            ita = destiny.GetTable(typeof(Blank));
            SQL.InsertDataTable(ref dt, ref ita, "Blanks");

            dt = original.GetTable<Result>();
            ita = destiny.GetTable(typeof(Result));
            SQL.InsertDataTable(ref dt, ref ita, "Results");

            dt = original.GetTable<DetectorsAbsorber>();
            ita = destiny.GetTable(typeof(DetectorsAbsorber));
            SQL.InsertDataTable(ref dt, ref ita, "DetectorsAbsorbers");

            dt = original.GetTable<DetectorsCurve>();
            ita = destiny.GetTable(typeof(DetectorsCurve));
            SQL.InsertDataTable(ref dt, ref ita, "DetectorsCurves");

            dt = original.GetTable<DetectorsDimension>();
            ita = destiny.GetTable(typeof(DetectorsDimension));
            SQL.InsertDataTable(ref dt, ref ita, "DetectorsDimensions");

            dt = original.GetTable<Holder>();
            ita = destiny.GetTable(typeof(Holder));
            SQL.InsertDataTable(ref dt, ref ita, "Holders");

            dt = original.GetTable<Solang>();
            ita = destiny.GetTable(typeof(Solang));
            SQL.InsertDataTable(ref dt, ref ita, "Solang");

            dt = original.GetTable<COIN>();
            ita = destiny.GetTable(typeof(COIN));
            SQL.InsertDataTable(ref dt, ref ita, "COIN");

            dt = original.GetTable<SchAcq>();
            ita = destiny.GetTable(typeof(SchAcq));
            SQL.InsertDataTable(ref dt, ref ita, "SchAcqs");

            dt = original.GetTable<VialType>();
            ita = destiny.GetTable(typeof(VialType));
            SQL.InsertDataTable(ref dt, ref ita, "VialTypes");

            dt = original.GetTable<Geometry>();
            ita = destiny.GetTable(typeof(Geometry));
            SQL.InsertDataTable(ref dt, ref ita, "Geometry");

            dt = original.GetTable<NAA>();
            ita = destiny.GetTable(typeof(NAA));
            SQL.InsertDataTable(ref dt, ref ita, "NAA");

            dt = original.GetTable<k0NAA>();
            ita = destiny.GetTable(typeof(k0NAA));
            SQL.InsertDataTable(ref dt, ref ita, "k0NAA");

            dt = original.GetTable<Yield>();
            ita = destiny.GetTable(typeof(Yield));
            SQL.InsertDataTable(ref dt, ref ita, "Yields");

            dt = original.GetTable<tStudent>();
            ita = destiny.GetTable(typeof(tStudent));
            SQL.InsertDataTable(ref dt, ref ita, "tStudent");
            /*
            dt = original.GetTable<LINE>();
            ita = destiny.GetTable(typeof(LINE));
            SQL.InsertDataTable(ref dt, ref ita);

            dt = original.GetTable<LINES_FI>();
            ita = destiny.GetTable(typeof(LINES_FI));
            SQL.InsertDataTable(ref dt, ref ita);
            */
            dt = original.GetTable<pValue>();
            ita = destiny.GetTable(typeof(pValue));
            SQL.InsertDataTable(ref dt, ref ita, "pValues");

            dt = original.GetTable<Reaction>();
            ita = destiny.GetTable(typeof(Reaction));
            SQL.InsertDataTable(ref dt, ref ita, "Reactions");

            dt = original.GetTable<Sigma>();
            ita = destiny.GetTable(typeof(Sigma));
            SQL.InsertDataTable(ref dt, ref ita, "Sigma");

            dt = original.GetTable<SigmasSal>();
            ita = destiny.GetTable(typeof(SigmasSal));
            SQL.InsertDataTable(ref dt, ref ita, "SigmaSal");

            dt = original.GetTable<Element>();
            ita = destiny.GetTable(typeof(Element));
            SQL.InsertDataTable(ref dt, ref ita, "Elements");

            dt = original.GetTable<Order>();
            ita = destiny.GetTable(typeof(Order));
            SQL.InsertDataTable(ref dt, ref ita, "Orders");

            dt = original.GetTable<OrderStatus>();
            ita = destiny.GetTable(typeof(OrderStatus));
            SQL.InsertDataTable(ref dt, ref ita, "OrderStatus");

            dt = original.GetTable<Project>();
            ita = destiny.GetTable(typeof(Project));
            SQL.InsertDataTable(ref dt, ref ita, "Projects");

            dt = original.GetTable<IrradiationRequest>();
            ita = destiny.GetTable(typeof(IrradiationRequest));
            SQL.InsertDataTable(ref dt, ref ita, "IrradiationRequests");

            dt = original.GetTable<IRequestsAverage>();
            ita = destiny.GetTable(typeof(IRequestsAverage));
            SQL.InsertDataTable(ref dt, ref ita, "IRequestsAverages");

            dt = original.GetTable<IPeakAverage>();
            ita = destiny.GetTable(typeof(IPeakAverage));
            SQL.InsertDataTable(ref dt, ref ita, "IPeakAverages");

            dt = original.GetTable<Peak>();
            ita = destiny.GetTable(typeof(Peak));
            SQL.InsertDataTable(ref dt, ref ita, "Peaks");

            dt = original.GetTable<Measurement>();
            ita = destiny.GetTable(typeof(Measurement));
            SQL.InsertDataTable(ref dt, ref ita, "Measurements");

            dt = original.GetTable<ToDo>();
            ita = destiny.GetTable(typeof(ToDo));
            SQL.InsertDataTable(ref dt, ref ita, "ToDo");

            dt = original.GetTable<ToDoResult>();
            ita = destiny.GetTable(typeof(ToDoResult));
            SQL.InsertDataTable(ref dt, ref ita, "ToDoResults");

            // destiny.ExecuteCommand("SET IDENTITY_INSERT ? OFF");

            // Irradiation;
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