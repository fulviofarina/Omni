//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public LINAATableAdapters.TableAdapterManager TAM
        {
            get { return tAM; }
            set { tAM = value; }
        }

        /// <summary>
        /// Initializes all the DataSet's Table Adapters
        /// </summary>
        public void InitializeAdapters()
        {
            InitializeSolCoinAdapters();
            InitializeIrradiationAdapters();
            InitializeSampleAdapters();
            InitializePeaksAdapters();
            InitializeToDoAdapters();
            InitializeOtherAdapters();
        }

        private System.Exception tAMException = null;

        public bool IsMainConnectionOk
        {
            get
            {
                CheckMainConnection();

                return tAMException == null;
            }
        }

        public string Exception
        {
            get
            {
                return tAMException.Message;
            }
        }

        private void CheckMainConnection()
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
        }

        public void DisposeAdapters()
        {
            if (tAM == null) return;
            DisposePeaksAdapters();
            DisposeSampleAdapters();
            DisposeOtherAdapters();
            DisposeIrradiationAdapters();
            DisposeSolCoinAdapters();
            DisposeMainAdapters();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adapter_RowUpdating(object sender, System.Data.SqlClient.SqlRowUpdatingEventArgs e)
        {
            try
            {
                object o = e.Row;
            }
            catch (System.SystemException ex)
            {
                this.AddException(ex);
            }
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Adapter_FillError(object sender, System.Data.FillErrorEventArgs e)
        {
            try
            {
                object[] o = e.Values;
            }
            catch (System.SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}