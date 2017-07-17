using System.Linq;

namespace DB.Tools
{
    public partial class Interface
    {
        /// <summary>
        /// Adapters interface
        /// </summary>
        public IAdapter IAdapter;

        /// <summary>
        /// binding Sources interface
        /// </summary>
        public BS IBS;

        /// <summary>
        /// Current rows interface
        /// </summary>
        public ICurrent ICurrent;

        /// <summary>
        /// The database tables
        /// </summary>
        public IDB IDB;

        /// <summary>
        /// Main stuff
        /// </summary>
     // public IStore IStore;

        /// <summary>
        /// Populator interface
        /// </summary>
        public Populate IPopulate;

        /// <summary>
        /// Preferences interface
        /// </summary>
        public IPreferences IPreferences;

        /// <summary>
        /// The reporter interface
        /// </summary>
        public IReport IReport;

        /// <summary>
        /// The Save interface
        /// </summary>
        public IStore IStore;

        /// <summary>
        /// is it used? not sure
        /// </summary>
        protected internal LINAA dataset = null;

        /// <summary>
        /// Gets the LINAA database
        /// </summary>
        public LINAA Get()
        {
            return dataset;
        }

        /// <summary>
        /// The main object to work with and access all members
        /// </summary>
        public Interface(ref LINAA aux)
        {
            dataset = aux;
            // IStore = (IStore)aux;
            IAdapter = (IAdapter)aux;
            IStore = (IStore)aux;
            IDB = (IDB)aux;

            //attach interface classes
            Interface inter = this;

            IBS = new BS(ref inter);
            IReport = new Report(ref inter);
            IPopulate = new Populate(ref inter);
            Current current = new Current(ref IBS, ref inter);
            ICurrent = current;
            Preference pref = new Preference(ref inter);
            IPreferences = pref ;

            //attach interfaces of LINAA (DB)
        }

        /// <summary>
        /// I put this here because I can have broad control on the names list
        /// </summary>
        /// <param name="tableName"></param>
        public void GetDisplayTableName(ref string tableName)
        {
            if (tableName.Contains(IDB.VialType.TableName))
            {
                tableName = "Container";
            }
            else if (tableName.Contains(IDB.Channels.TableName))
            {
                tableName = "Neutron Source";
            }
            else if (tableName.Contains(IDB.Unit.TableName))
            {
                tableName = "neutron source configuration";
            }
            else if (tableName.Contains(IDB.SubSamples.TableName))
            {
                tableName = "geometry";
            }
        }

        public string GetDisplayColumName(string tableName, string[] colsInError)
        {
            //iterate
            for (int i = 0; i < colsInError.Count(); i++)
            {
                string col = colsInError[i];
                if (col.Contains(tableName))
                {
                    colsInError[i] = colsInError[i].Replace(tableName, null);
                }
                if (col.Contains("FillHeight"))
                {
                    colsInError[i] = "Length";
                }
                else if (col.Contains("Radius"))
                {
                    colsInError[i] = "Radius";
                }
                else if (col.Contains("Matrix"))
                {
                    colsInError[i] = "Matrix";
                }
                else if (col.Contains("Name") || col.Contains("Ref"))
                {
                    colsInError[i] = "Label";
                }
                else if (col.Contains("Gross") || col.Contains("Net"))
                {
                    colsInError[i] = "Mass";
                }
            }
            string sep = ", ";
            string result = colsInError.Aggregate((o, next) => o = o + sep + next);
            result = "\t" + result;
            return result;
        }
    }
}