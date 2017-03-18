//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected void InitializeSampleAdapters()
        {
            this.tAM.MatSSFTableAdapter = new LINAATableAdapters.MatSSFTableAdapter();
            this.tAM.UnitTableAdapter = new LINAATableAdapters.UnitTableAdapter();

            this.tAM.MonitorsFlagsTableAdapter = new LINAATableAdapters.MonitorsFlagsTableAdapter();
            this.tAM.MonitorsTableAdapter = new LINAATableAdapters.MonitorsTableAdapter();
            this.tAM.SamplesTableAdapter = new LINAATableAdapters.SamplesTableAdapter();
            this.tAM.StandardsTableAdapter = new LINAATableAdapters.StandardsTableAdapter();
            this.tAM.SubSamplesTableAdapter = new LINAATableAdapters.SubSamplesTableAdapter();
            this.tAM.SubSamplesTableAdapter.Adapter.AcceptChangesDuringUpdate = true;
            // this.tAM.SubSamplesTableAdapter.Adapter.FillError += new System.Data.FillErrorEventHandler(Adapter_FillError);
            // this.tAM.SubSamplesTableAdapter.Adapter.RowUpdating += new System.Data.SqlClient.SqlRowUpdatingEventHandler(Adapter_RowUpdating);
        }

        protected void DisposeSampleAdapters()
        {
            if (this.tAM.UnitTableAdapter != null)
            {
                this.tAM.UnitTableAdapter.Connection.Close();

                this.tAM.UnitTableAdapter.Dispose();
            }

            if (this.tAM.MatSSFTableAdapter != null)
            {
                this.tAM.MatSSFTableAdapter.Connection.Close();

                this.tAM.MatSSFTableAdapter.Dispose();
            }
            if (this.tAM.MonitorsFlagsTableAdapter != null)
            {
                this.tAM.MonitorsFlagsTableAdapter.Connection.Close();
                this.tAM.MonitorsFlagsTableAdapter.Dispose();
            }
            if (this.tAM.MonitorsTableAdapter != null)
            {
                this.tAM.MonitorsTableAdapter.Connection.Close();

                this.tAM.MonitorsTableAdapter.Dispose();
            }
            if (this.tAM.SamplesTableAdapter != null)
            {
                this.tAM.SamplesTableAdapter.Connection.Close();
                this.tAM.SamplesTableAdapter.Dispose();
            }
            if (this.tAM.StandardsTableAdapter != null)
            {
                this.tAM.StandardsTableAdapter.Connection.Close();
                this.tAM.StandardsTableAdapter.Dispose();
            }
            if (this.tAM.SubSamplesTableAdapter != null)
            {
                this.tAM.SubSamplesTableAdapter.Connection.Close();
                this.tAM.SubSamplesTableAdapter.Dispose();
            }

            this.tAM.MatSSFTableAdapter = null;
            this.tAM.UnitTableAdapter = null;
            this.tAM.MonitorsFlagsTableAdapter = null;
            this.tAM.MonitorsTableAdapter = null;
            this.tAM.SamplesTableAdapter = null;
            this.tAM.StandardsTableAdapter = null;
            this.tAM.SubSamplesTableAdapter = null;
        }
    }
}