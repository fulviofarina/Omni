//using DB.Interfaces;
using System.Collections;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected Hashtable adapters;

        /// <summary>
        /// Queries of this dataset
        /// </summary>
        protected LINAATableAdapters.QTA qTA;

        /// <summary>
        /// The master Table Adapter Manager of this dataset
        /// </summary>
        protected LINAATableAdapters.TableAdapterManager tAM;

        // private System.ComponentModel.IContainer components;
        protected System.Exception tAMException = null;

        public delegate int TAMDeleteMethod(int index);

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        protected internal void adapter_FillError(object sender, System.Data.FillErrorEventArgs e)
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
        /// <param name="e">     </param>
        protected internal void adapter_RowUpdating(object sender, System.Data.SqlClient.SqlRowUpdatingEventArgs e)
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

        /*
      // cRV

      // tAM
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
      // this.tAM.IPeakAveragesTableAdapter = null; this.tAM.IRequestsAveragesTableAdapter = null;
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
    }
}