//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected void InitializeSampleAdapters()
        {
            this.tAM.MatSSFTableAdapter = new LINAATableAdapters.MatSSFTableAdapter();
            adapters.Add(this.tAM.MatSSFTableAdapter, this.tAM.MatSSFTableAdapter);
            this.tAM.UnitTableAdapter = new LINAATableAdapters.UnitTableAdapter();
            adapters.Add(this.tAM.UnitTableAdapter, this.tAM.UnitTableAdapter);
            this.tAM.MonitorsFlagsTableAdapter = new LINAATableAdapters.MonitorsFlagsTableAdapter();
            adapters.Add(this.tAM.MonitorsFlagsTableAdapter, this.tAM.MonitorsFlagsTableAdapter);
            this.tAM.MonitorsTableAdapter = new LINAATableAdapters.MonitorsTableAdapter();
            adapters.Add(this.tAM.MonitorsTableAdapter, this.tAM.MonitorsTableAdapter);
            this.tAM.SamplesTableAdapter = new LINAATableAdapters.SamplesTableAdapter();
            adapters.Add(this.tAM.SamplesTableAdapter, this.tAM.SamplesTableAdapter);
            this.tAM.StandardsTableAdapter = new LINAATableAdapters.StandardsTableAdapter();
            adapters.Add(this.tAM.StandardsTableAdapter, this.tAM.StandardsTableAdapter);
            this.tAM.SubSamplesTableAdapter = new LINAATableAdapters.SubSamplesTableAdapter();
            adapters.Add(this.tAM.SubSamplesTableAdapter, this.tAM.SubSamplesTableAdapter);

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