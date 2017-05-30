//using DB.Interfaces;
using System.Data;
using System.Data.SqlClient;
using DB.LINAATableAdapters;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public void SetConnections(/*string localDB, string developerDB,*/ string defaultConnection)
        {
            //VEEEERY IMPORTANT, SAVES PREFERNCES AND SETTINGS!!!!
            //  Properties.Settings.Default["developerDB"] = developerDB;
            //     Properties.Settings.Default["localDB"] = localDB;
            Properties.Settings.Default["NAAConnectionString"] = defaultConnection;

            Properties.Settings.Default.Save();
        }

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
                    System.Data.ConnectionState st = tAM.Connection.State;

                    if (st == System.Data.ConnectionState.Open)
                    {
                        this.tAM.Connection.Close();
                    }
                    if (st == System.Data.ConnectionState.Closed)
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
            disposePeaksAdapters(ref tAM);
            disposeSampleAdapters(ref tAM);
            disposeOtherAdapters(ref tAM);
            disposeIrradiationAdapters(ref tAM);
            DisposeSolCoinAdapters();
            disposeComponent();
        }

        /// <summary>
        /// Initializes all the DataSet's Table Adapters
        /// </summary>
        public void InitializeAdapters()
        {
            //probando esta linea
            InitializeSolCoinAdapters();
            initializeIrradiationAdapters(ref tAM, ref adapters);
            initializeSampleAdapters(ref tAM, ref adapters);
            initializePeaksAdapters(ref tAM, ref adapters);
            initializeToDoAdapters(ref tAM, ref adapters);
            InitializeOtherAdapters(ref tAM, ref adapters);

            // ChangeConnection = Properties.Settings.Default.LIMSConnectionString; this.TAM.Connection.Close();

            // this.TAM.Connection.Open();

            // this.TAM.Connection.Database. = "LIMS"; this.TAM.Connection.Close();
            // this.TAM.Connection.ConnectionString =
            // DB.Properties.Settings.Default.NAAConnectionString; this.TAM.Connection.Open();
        }

        public void InitializeComponent()
        {
            // ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            this.tAM = new DB.LINAATableAdapters.TableAdapterManager();
            this.qTA = new DB.LINAATableAdapters.QTA();

            this.tAM.UpdateOrder = DB.LINAATableAdapters.TableAdapterManager.UpdateOrderOption.UpdateInsertDelete;
            //  this.EnforceConstraints = false;
            //this.Locale = new System.Globalization.CultureInfo("");

            adapters = new System.Collections.Hashtable();

            // ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }
    }
}