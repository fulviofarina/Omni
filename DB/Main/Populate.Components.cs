namespace DB.LINAATableAdapters
{
  public partial class MeasurementsTableAdapter
  {
    public void SetForHL()
    {
      this.Connection.ConnectionString = DB.Properties.Settings.Default.HLSNMNAAConnectionString;
    }

    public void SetForLIMS()
    {
      this.Connection.ConnectionString = DB.Properties.Settings.Default.NAAConnectionString;
    }
  }
}

namespace DB
{
  public partial class LINAA
  {
    protected internal System.ComponentModel.IContainer components;

    protected internal LINAATableAdapters.TableAdapterManager tAM;
    /// <summary>
    /// The master Table Adapter Manager of this dataset
    /// </summary>

    /// <summary>
    /// Queries of this dataset
    /// </summary>
    public LINAATableAdapters.QTA QTA;

    public void InitializeComponent()
    {
      this.tAM = new DB.LINAATableAdapters.TableAdapterManager();
      this.QTA = new DB.LINAATableAdapters.QTA();

      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
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
      this.tAM.UpdateOrder = DB.LINAATableAdapters.TableAdapterManager.UpdateOrderOption.UpdateInsertDelete;
      this.tAM.VialTypeTableAdapter = null;
      //
      // LINAA
      //
      this.EnforceConstraints = false;
      this.Locale = new System.Globalization.CultureInfo("");
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
    }
  }
}