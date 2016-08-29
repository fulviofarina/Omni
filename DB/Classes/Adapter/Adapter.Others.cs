using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected void InitializeOtherAdapters()
        {
            this.tAM.ElementsTableAdapter = new LINAATableAdapters.ElementsTableAdapter();
            this.tAM.SigmasTableAdapter = new LINAATableAdapters.SigmasTableAdapter();
            this.tAM.SigmasSalTableAdapter = new LINAATableAdapters.SigmasSalTableAdapter();
            this.tAM.ReactionsTableAdapter = new LINAATableAdapters.ReactionsTableAdapter();
            this.tAM.NAATableAdapter = new LINAATableAdapters.NAATableAdapter();
            this.tAM.pValuesTableAdapter = new LINAATableAdapters.pValuesTableAdapter();
            this.tAM.k0NAATableAdapter = new LINAATableAdapters.k0NAATableAdapter();
            this.tAM.SchAcqsTableAdapter = new LINAATableAdapters.SchAcqsTableAdapter();
            this.tAM.YieldsTableAdapter = new LINAATableAdapters.YieldsTableAdapter();
        }

        protected void DisposeMainAdapters()
        {
            if (this.QTA != null) this.QTA.Dispose();
            this.QTA = null;

            if (this.tAM != null)
            {
                if (this.tAM.Connection != null) this.tAM.Connection.Close();
                this.tAM.Dispose();
            }
            this.tAM = null;
        }

        protected void DisposeOtherAdapters()
        {
            if (TAM.ElementsTableAdapter != null)
            {
                this.tAM.ElementsTableAdapter.Connection.Close();

                TAM.ElementsTableAdapter.Dispose();
            }

            if (this.tAM.SigmasTableAdapter != null)
            {
                this.tAM.SigmasTableAdapter.Connection.Close();

                this.tAM.SigmasTableAdapter.Dispose();
            }
            if (TAM.SigmasSalTableAdapter != null)
            {
                this.tAM.SigmasSalTableAdapter.Connection.Close();
                TAM.SigmasSalTableAdapter.Dispose();
            }
            if (this.tAM.ReactionsTableAdapter != null)
            {
                this.tAM.ReactionsTableAdapter.Connection.Close();
                this.tAM.ReactionsTableAdapter.Dispose();
            }
            if (TAM.NAATableAdapter != null)
            {
                this.tAM.NAATableAdapter.Connection.Close();

                TAM.NAATableAdapter.Dispose();
            }
            if (this.tAM.pValuesTableAdapter != null)
            {
                this.tAM.pValuesTableAdapter.Connection.Close();
                this.tAM.pValuesTableAdapter.Dispose();
            }
            if (this.tAM.k0NAATableAdapter != null)
            {
                this.tAM.k0NAATableAdapter.Connection.Close();
                this.tAM.k0NAATableAdapter.Dispose();
            }
            if (this.tAM.YieldsTableAdapter != null)
            {
                this.tAM.YieldsTableAdapter.Connection.Close();
                this.tAM.YieldsTableAdapter.Dispose();
            }

            if (this.tAM.SchAcqsTableAdapter != null) { this.tAM.SchAcqsTableAdapter.Dispose(); }

            this.tAM.YieldsTableAdapter = null;
            this.tAM.ElementsTableAdapter = null;
            this.tAM.SigmasTableAdapter = null;
            this.tAM.SigmasSalTableAdapter = null;
            this.tAM.ReactionsTableAdapter = null;
            this.tAM.NAATableAdapter = null;
            this.tAM.pValuesTableAdapter = null;
            this.tAM.k0NAATableAdapter = null;
            this.tAM.SchAcqsTableAdapter = null;
        }
    }
}