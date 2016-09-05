using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        #region peaks

        protected void InitializePeaksAdapters()
        {
            this.tAM.MeasurementsTableAdapter = new LINAATableAdapters.MeasurementsTableAdapter();
            this.tAM.PeaksTableAdapter = new LINAATableAdapters.PeaksTableAdapter();
            // this.tAM.IRequestsAveragesTableAdapter = new LINAATableAdapters.IRequestsAveragesTableAdapter();
            // this.tAM.IPeakAveragesTableAdapter = new LINAATableAdapters.IPeakAveragesTableAdapter();
        }

        protected void InitializeToDoAdapters()
        {
            this.tAM.ToDoTableAdapter = new LINAATableAdapters.ToDoTableAdapter();
        }

        protected void DisposePeaksAdapters()
        {
            // this.tAM.IRequestsAveragesTableAdapter.Connection.Close();
            // this.tAM.IPeakAveragesTableAdapter.Connection.Close();

            if (TAM.MeasurementsTableAdapter != null)
            {
                this.tAM.MeasurementsTableAdapter.Connection.Close();
                TAM.MeasurementsTableAdapter.Dispose();
            }
            if (TAM.PeaksTableAdapter != null)
            {
                this.tAM.PeaksTableAdapter.Connection.Close();
                TAM.PeaksTableAdapter.Dispose();
            }
            if (TAM.ToDoTableAdapter != null)
            {
                this.tAM.ToDoTableAdapter.Connection.Close();
                TAM.ToDoTableAdapter.Dispose();
            }
            // if (TAM.IRequestsAveragesTableAdapter != null) TAM.IRequestsAveragesTableAdapter.Dispose();
            //	 if (TAM.IPeakAveragesTableAdapter != null) TAM.IPeakAveragesTableAdapter.Dispose();

            this.tAM.MeasurementsTableAdapter = null;
            this.tAM.PeaksTableAdapter = null;
            this.tAM.ToDoTableAdapter = null;
            //	 this.tAM.IRequestsAveragesTableAdapter = null;
            //	 this.tAM.IPeakAveragesTableAdapter = null;
        }

        #endregion peaks

        #region irradiations

        protected void InitializeIrradiationAdapters()
        {
            //	 this.tAM.CapsulesTableAdapter = new LINAATableAdapters.CapsulesTableAdapter();
            this.tAM.ChannelsTableAdapter = new LINAATableAdapters.ChannelsTableAdapter();
            this.tAM.IrradiationRequestsTableAdapter = new LINAATableAdapters.IrradiationRequestsTableAdapter();
            this.tAM.OrdersTableAdapter = new LINAATableAdapters.OrdersTableAdapter();
            this.tAM.ProjectsTableAdapter = new LINAATableAdapters.ProjectsTableAdapter();
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

        #endregion irradiations

        #region sample

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

        #endregion sample
    }
}