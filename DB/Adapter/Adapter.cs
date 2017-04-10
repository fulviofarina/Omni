//using DB.Interfaces;
using System.Data.OleDb;
using DB.LINAATableAdapters;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        /*
      //
      // cRV
      //

      //
      // tAM
      //
      this.tAM.AcquisitionsTableAdapter = null;
      this.tAM.BackupDataSetBeforeUpdate = false;
      this.tAM.BlanksTableAdapter = null;
      // this.tAM.CapsulesTableAdapter = null;
      this.tAM.ChannelsTableAdapter = null;
      this.tAM.COINTableAdapter = null;
      this.tAM.Connection = null;
      this.tAM.ContactPersonsTableAdapter = null;
      this.tAM.CustomerTableAdapter = null;
      this.tAM.DetectorsAbsorbersTableAdapter = null;
      this.tAM.DetectorsCurvesTableAdapter = null;
      this.tAM.DetectorsDimensionsTableAdapter = null;
      this.tAM.ElementsTableAdapter = null;
      this.tAM.GeometryTableAdapter = null;
      this.tAM.HoldersTableAdapter = null;
      //	 this.tAM.IPeakAveragesTableAdapter = null;
      //	 this.tAM.IRequestsAveragesTableAdapter = null;
      this.tAM.IrradiationRequestsTableAdapter = null;
      this.tAM.k0NAATableAdapter = null;
      this.tAM.MatrixTableAdapter = null;
      this.tAM.MatSSFTableAdapter = null;
      this.tAM.MeasurementsTableAdapter = null;
      this.tAM.MonitorsFlagsTableAdapter = null;
      this.tAM.MonitorsTableAdapter = null;
      this.tAM.MUESTableAdapter = null;
      this.tAM.NAATableAdapter = null;
      this.tAM.OrdersTableAdapter = null;
      this.tAM.PeaksTableAdapter = null;
      this.tAM.ProjectsTableAdapter = null;
      this.tAM.pValuesTableAdapter = null;
      this.tAM.ReactionsTableAdapter = null;
      this.tAM.RefMaterialsTableAdapter = null;
      this.tAM.SamplesTableAdapter = null;
      this.tAM.SchAcqsTableAdapter = null;
      this.tAM.SigmasSalTableAdapter = null;
      this.tAM.SigmasTableAdapter = null;
      this.tAM.SolangTableAdapter = null;
      this.tAM.StandardsTableAdapter = null;
      this.tAM.SubSamplesTableAdapter = null;
      this.tAM.ToDoTableAdapter = null;
      this.tAM.UnitTableAdapter = null;
      this.tAM.VialTypeTableAdapter = null;

      */

        /// <summary>
        /// Queries of this dataset
        /// </summary>
        private LINAATableAdapters.QTA qTA;

        /// <summary>
        /// The master Table Adapter Manager of this dataset
        /// </summary>
        private LINAATableAdapters.TableAdapterManager tAM;

        //  private System.ComponentModel.IContainer components;
        private System.Exception tAMException = null;

        private delegate int TAMDeleteMethod(int index);

        public string ChangeConnection
        {
            set
            {
                string connection = value;
                System.Data.IDbConnection con = this.TAM.Connection;
                con.Close();
                con = new System.Data.SqlClient.SqlConnection(connection);
                this.TAM.Connection = con;
                foreach (dynamic a in adapters.Values)
                {
                    a.Connection.Close();
                    a.Connection = new System.Data.SqlClient.SqlConnection(connection);
                }
                con.Open();
            }
        }

        public string Exception
        {
            get
            {
                return tAMException.Message;
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

        public LINAATableAdapters.TableAdapterManager TAM
        {
            get { return tAM; }
            set { tAM = value; }
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
        /// Initializes all the DataSet's Table Adapters
        /// </summary>
        public void InitializeAdapters()
        {
            //probando esta linea
            InitializeSolCoinAdapters();
            InitializeIrradiationAdapters();
            InitializeSampleAdapters();
            InitializePeaksAdapters();
            InitializeToDoAdapters();
            InitializeOtherAdapters();

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
            this.EnforceConstraints = false;
            this.Locale = new System.Globalization.CultureInfo("");

            adapters = new System.Collections.Hashtable();

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
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
    }
}