//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected void InitializeIrradiationAdapters()
        {
            //	 this.tAM.CapsulesTableAdapter = new LINAATableAdapters.CapsulesTableAdapter();
            this.tAM.ChannelsTableAdapter = new LINAATableAdapters.ChannelsTableAdapter();
            adapters.Add(this.tAM.ChannelsTableAdapter, this.tAM.ChannelsTableAdapter);
            this.tAM.IrradiationRequestsTableAdapter = new LINAATableAdapters.IrradiationRequestsTableAdapter();
            adapters.Add(this.tAM.IrradiationRequestsTableAdapter, this.tAM.IrradiationRequestsTableAdapter);
            this.tAM.OrdersTableAdapter = new LINAATableAdapters.OrdersTableAdapter();
            adapters.Add(this.tAM.OrdersTableAdapter, this.tAM.OrdersTableAdapter);
            this.tAM.ProjectsTableAdapter = new LINAATableAdapters.ProjectsTableAdapter();
            adapters.Add(this.tAM.ProjectsTableAdapter, this.tAM.ProjectsTableAdapter);
        }

        protected void DisposeIrradiationAdapters()
        {
            // this.tAM.CapsulesTableAdapter.Connection.Close();

            // if (this.tAM.CapsulesTableAdapter != null) this.tAM.CapsulesTableAdapter.Dispose();
            if (this.tAM.ChannelsTableAdapter != null)
            {
                this.tAM.ChannelsTableAdapter.Connection.Close();
                this.tAM.ChannelsTableAdapter.Dispose();
            }
            if (this.tAM.IrradiationRequestsTableAdapter != null)
            {
                this.tAM.IrradiationRequestsTableAdapter.Connection.Close();
                this.tAM.IrradiationRequestsTableAdapter.Dispose();
            }
            if (this.tAM.OrdersTableAdapter != null)
            {
                this.tAM.OrdersTableAdapter.Connection.Close();
                this.tAM.OrdersTableAdapter.Dispose();
            }
            if (this.tAM.ProjectsTableAdapter != null)
            {
                this.tAM.ProjectsTableAdapter.Connection.Close();
                this.tAM.ProjectsTableAdapter.Dispose();
            }

            // this.tAM.CapsulesTableAdapter = null;
            this.tAM.ChannelsTableAdapter = null;
            this.tAM.IrradiationRequestsTableAdapter = null;
            this.tAM.OrdersTableAdapter = null;
            this.tAM.ProjectsTableAdapter = null;
        }
    }
}