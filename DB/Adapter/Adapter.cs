//using DB.Interfaces;

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

        private delegate int TAMDeleteMethod(int index);

        private System.ComponentModel.IContainer components;

        /// <summary>
        /// The master Table Adapter Manager of this dataset
        /// </summary>
        private LINAATableAdapters.TableAdapterManager tAM;

        private System.Exception tAMException = null;

        /// <summary>
        /// Queries of this dataset
        /// </summary>
        public LINAATableAdapters.QTA QTA;

        public void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            this.tAM = new DB.LINAATableAdapters.TableAdapterManager();
            this.QTA = new DB.LINAATableAdapters.QTA();

            this.tAM.UpdateOrder = DB.LINAATableAdapters.TableAdapterManager.UpdateOrderOption.UpdateInsertDelete;
            this.EnforceConstraints = false;
            this.Locale = new System.Globalization.CultureInfo("");

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
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
            InitializeSolCoinAdapters();
            InitializeIrradiationAdapters();
            InitializeSampleAdapters();
            InitializePeaksAdapters();
            InitializeToDoAdapters();
            InitializeOtherAdapters();
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