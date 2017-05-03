//using DB.Interfaces;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
        protected LINAATableAdapters.QTA qTA;

        /// <summary>
        /// The master Table Adapter Manager of this dataset
        /// </summary>
        protected LINAATableAdapters.TableAdapterManager tAM;

        //  private System.ComponentModel.IContainer components;
        protected System.Exception tAMException = null;

        private delegate int TAMDeleteMethod(int index);

        public string ChangeConnection
        {
            set
            {
                string connection = value;
                IDbConnection con = this.TAM.Connection;
                con.Close();
                con = new SqlConnection(connection);
                this.TAM.Connection = con;
                foreach (dynamic a in adapters.Values)
                {
                    a.Connection.Close();
                    a.Connection = new SqlConnection(connection);
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
    }
}