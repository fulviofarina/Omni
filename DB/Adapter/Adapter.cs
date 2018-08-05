using DB.LINAATableAdapters;
using System.Data;
using System.Data.SqlClient;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public string ChangeConnection
        {
            set
            {
                string connection = value;
                IDbConnection con = this.TAM.Connection;
                con.Close();
                this.qTA.Dispose();
                this.qTA = new QTA();
                // con = new SqlConnection(connection);
                this.TAM.Connection = new SqlConnection(connection);

                foreach (dynamic a in adapters.Values)
                {
                    a.Connection.Close();
                    a.Connection = new SqlConnection(connection);
                }
                this.TAM.Connection.Open();
            }
        }

        public string Exception
        {
            get
            {
                return tAMException?.Message;
            }
        }

        public bool IsMainConnectionOk
        {
            get
            {
                tAMException = null;
                try
                {
                    ConnectionState st = tAM.Connection.State;

                    if (st == ConnectionState.Open)
                    {
                        this.tAM.Connection.Close();
                    }
                    if (st == ConnectionState.Closed)
                    {
                        this.tAM.Connection.Open();
                    }
                }
                catch (System.Exception ex)
                {
                    tAMException = ex;
                }

                return tAMException == null;
            }
        }

        public QTA QTA
        {
            get
            {
                return qTA;
            }

            set
            {
                qTA = value;
            }
        }

        public TableAdapterManager TAM
        {
            get { return tAM; }
            set { tAM = value; }
        }

        public void DisposeAdapters()
        {
            if (tAM == null) return;
            DisposePeaksAdapters();
            disposeSampleAdapters();
            disposeOtherAdapters();
            disposeIrradiationAdapters();
            DisposeSolCoinAdapters();
            disposeComponent();
        }

        /// <summary>
        /// Initializes all the DataSet's Table Adapters
        /// </summary>
        public void InitializeAdapters()
        {
            InitializeSolCoinAdapters();
            initializeIrradiationAdapters();
            initializeSampleAdapters();
            InitializePeaksAdapters();
            initializeToDoAdapters();
            InitializeOtherAdapters();
        }

        public void InitializeComponent()
        {
            this.tAM = new TableAdapterManager();
            this.qTA = new QTA();

            this.tAM.UpdateOrder = TableAdapterManager.UpdateOrderOption.UpdateInsertDelete;

            adapters = new System.Collections.Hashtable();
        }

        public void SetConnections(/*string localDB, string developerDB,*/ string defaultConnection)
        {
            //VEEEERY IMPORTANT, SAVES PREFERNCES AND SETTINGS!!!!
            //  Properties.Settings.Default["developerDB"] = developerDB;
            //     Properties.Settings.Default["localDB"] = localDB;
            Properties.Settings.Default["NAAConnectionString"] = defaultConnection;

            Properties.Settings.Default.Save();
        }
    }
}