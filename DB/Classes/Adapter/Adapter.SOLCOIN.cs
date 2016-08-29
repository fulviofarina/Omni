using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public void InitializeSolCoinAdapters()
        {
            this.tAM.MatrixTableAdapter = new LINAATableAdapters.MatrixTableAdapter();
            this.tAM.CompositionsTableAdapter = new LINAATableAdapters.CompositionsTableAdapter();
            this.tAM.VialTypeTableAdapter = new LINAATableAdapters.VialTypeTableAdapter();
            this.tAM.GeometryTableAdapter = new LINAATableAdapters.GeometryTableAdapter();
            this.tAM.MUESTableAdapter = new LINAATableAdapters.MUESTableAdapter();
            this.tAM.DetectorsAbsorbersTableAdapter = new LINAATableAdapters.DetectorsAbsorbersTableAdapter();
            this.tAM.DetectorsDimensionsTableAdapter = new LINAATableAdapters.DetectorsDimensionsTableAdapter();
            this.tAM.HoldersTableAdapter = new LINAATableAdapters.HoldersTableAdapter();
            this.tAM.DetectorsCurvesTableAdapter = new LINAATableAdapters.DetectorsCurvesTableAdapter();
            this.tAM.SolangTableAdapter = new LINAATableAdapters.SolangTableAdapter();

            this.tAM.COINTableAdapter = new LINAATableAdapters.COINTableAdapter();
        }

        public void DisposeSolCoinAdapters()
        {
            if (this.tAM.CompositionsTableAdapter != null)
            {
                this.tAM.CompositionsTableAdapter.Connection.Close();
                this.tAM.CompositionsTableAdapter.Dispose();
            }
            if (this.tAM.MatrixTableAdapter != null)
            {
                this.tAM.MatrixTableAdapter.Connection.Close();
                this.tAM.MatrixTableAdapter.Dispose();
            }
            if (this.tAM.VialTypeTableAdapter != null)
            {
                this.tAM.VialTypeTableAdapter.Connection.Close();
                this.tAM.VialTypeTableAdapter.Dispose();
            }
            if (this.tAM.GeometryTableAdapter != null)
            {
                this.tAM.GeometryTableAdapter.Connection.Close();
                this.tAM.GeometryTableAdapter.Dispose();
            }
            if (this.tAM.MUESTableAdapter != null)
            {
                this.tAM.MUESTableAdapter.Connection.Close();
                this.tAM.MUESTableAdapter.Dispose();
            }
            if (this.tAM.DetectorsAbsorbersTableAdapter != null)
            {
                this.tAM.DetectorsAbsorbersTableAdapter.Connection.Close();
                this.tAM.DetectorsAbsorbersTableAdapter.Dispose();
            }
            if (this.tAM.DetectorsDimensionsTableAdapter != null)
            {
                this.tAM.DetectorsDimensionsTableAdapter.Connection.Close();
                this.tAM.DetectorsDimensionsTableAdapter.Dispose();
            }
            if (this.tAM.HoldersTableAdapter != null)
            {
                this.tAM.HoldersTableAdapter.Connection.Close();
                this.tAM.HoldersTableAdapter.Dispose();
            }
            if (this.tAM.DetectorsCurvesTableAdapter != null)
            {
                this.tAM.DetectorsCurvesTableAdapter.Connection.Close();
                this.tAM.DetectorsCurvesTableAdapter.Dispose();
            }
            if (this.tAM.SolangTableAdapter != null)
            {
                this.tAM.SolangTableAdapter.Connection.Close();
                this.tAM.SolangTableAdapter.Dispose();
            }

            if (this.tAM.COINTableAdapter != null)
            {
                this.tAM.COINTableAdapter.Connection.Close();
                this.tAM.COINTableAdapter.Dispose();
            }
            this.tAM.CompositionsTableAdapter = null;

            this.tAM.MatrixTableAdapter = null;
            this.tAM.VialTypeTableAdapter = null;
            this.tAM.GeometryTableAdapter = null;
            this.tAM.MUESTableAdapter = null;
            this.tAM.DetectorsAbsorbersTableAdapter = null;
            this.tAM.DetectorsDimensionsTableAdapter = null;
            this.tAM.HoldersTableAdapter = null;
            this.tAM.DetectorsCurvesTableAdapter = null;
            this.tAM.SolangTableAdapter = null;

            this.tAM.COINTableAdapter = null;
        }
    }
}