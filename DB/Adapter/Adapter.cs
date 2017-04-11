//using DB.Interfaces;
using System.Data.OleDb;
using DB.LINAATableAdapters;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public void DisposeAdapters()
        {
            if (tAM == null) return;
            disposePeaksAdapters(ref tAM);
            disposeSampleAdapters(ref tAM);
            disposeOtherAdapters(ref tAM);
            disposeIrradiationAdapters(ref tAM);
            DisposeSolCoinAdapters();
            disposeMainAdapters();
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

            //   ChangeConnection = Properties.Settings.Default.LIMSConnectionString;
            //  this.TAM.Connection.Close();

            //  this.TAM.Connection.Open();

            //   this.TAM.Connection.Database. = "LIMS";
            //     this.TAM.Connection.Close();
            //      this.TAM.Connection.ConnectionString = DB.Properties.Settings.Default.NAAConnectionString;
            //        this.TAM.Connection.Open();
        }

        public void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            this.tAM = new DB.LINAATableAdapters.TableAdapterManager();
            this.qTA = new DB.LINAATableAdapters.QTA();

            this.tAM.UpdateOrder = DB.LINAATableAdapters.TableAdapterManager.UpdateOrderOption.UpdateInsertDelete;
            //  this.EnforceConstraints = false;
            //this.Locale = new System.Globalization.CultureInfo("");

            adapters = new System.Collections.Hashtable();

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Adapter_FillError(object sender, System.Data.FillErrorEventArgs e)
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

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Adapter_RowUpdating(object sender, System.Data.SqlClient.SqlRowUpdatingEventArgs e)
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
    }
}