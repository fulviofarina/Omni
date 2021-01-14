using DB.LINAATableAdapters;
using System;
using System.Data;


namespace DB
{
    public partial class LINAA : IAdapter
    {
        /*
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
        */

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
                    checkMainConnection();
                }
                catch (Exception ex)
                {
                    tAMException = ex;
                }

                return tAMException == null;
            }
        }

        public bool IsHyperLabConnectionOk
        {
            get
            {
                tAMException = null;
                try
                {
                    dynamic ta = this.tAM.MeasurementsTableAdapter;
                    ChangeConnection(ref ta, true);
                    checkHyperLabConnection();
                }
                catch (Exception ex)
                {
                    tAMException = ex;
                }

                return tAMException == null;
            }
        }

        private void checkHyperLabConnection()
        {
            ConnectionState st = tAM.MeasurementsTableAdapter.Connection.State;

            if (st == ConnectionState.Open)
            {
                this.tAM.MeasurementsTableAdapter.Connection.Close();
            }
            if (st == ConnectionState.Closed)
            {
                this.tAM.MeasurementsTableAdapter.Connection.Open();
            }
        }

        private void checkMainConnection()
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
            InitializePeaksAdapters(true);
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

        public void SetMainConnection(string defaultConnection)
        {
            DisposeAdapters();
            //VEEEERY IMPORTANT, SAVES PREFERNCES AND SETTINGS!!!!
            //  Properties.Settings.Default["developerDB"] = developerDB;
            //     Properties.Settings.Default["localDB"] = localDB;
            Properties.Settings.Default["NAAConnectionString"] = defaultConnection;

            Properties.Settings.Default.Save();

        //    Properties.Settings.Default.Upgrade();

            InitializeComponent();
            InitializeAdapters(); //why was this after the next code? //check
        }

        public void SetHyperLabConnection(string defaultConnection)
        {
            DisposePeaksAdapters();
            //VEEEERY IMPORTANT, SAVES PREFERNCES AND SETTINGS!!!!
            //  Properties.Settings.Default["developerDB"] = developerDB;
            //     Properties.Settings.Default["localDB"] = localDB;
            Properties.Settings.Default["HLSNMNAAConnectionString"] = defaultConnection;

            Properties.Settings.Default.Save();

            InitializeComponent();
            InitializePeaksAdapters(true); //why was this after the next code? //check
        }
    }
}