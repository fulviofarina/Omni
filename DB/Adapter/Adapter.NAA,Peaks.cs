//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
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
    }
}