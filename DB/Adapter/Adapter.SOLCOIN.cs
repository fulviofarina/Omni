//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public void DisposeSolCoinAdapters()
        {
            // if (tAM?.CompositionsTableAdapter != null)
            {
                tAM?.CompositionsTableAdapter.Connection.Close();
                tAM?.CompositionsTableAdapter.Dispose();
            }
            // if (tAM?.MatrixTableAdapter != null)
            {
                tAM?.MatrixTableAdapter.Connection.Close();
                tAM?.MatrixTableAdapter.Dispose();
            }
            // if (tAM?.VialTypeTableAdapter != null)
            {
                tAM?.VialTypeTableAdapter.Connection.Close();
                tAM?.VialTypeTableAdapter.Dispose();
            }
            // if (tAM?.GeometryTableAdapter != null)
            {
                tAM?.GeometryTableAdapter.Connection.Close();
                tAM?.GeometryTableAdapter.Dispose();
            }
            // if (tAM?.MUESTableAdapter != null)
            {
                tAM?.MUESTableAdapter.Connection.Close();
                tAM?.MUESTableAdapter.Dispose();
            }
            // if (tAM?.DetectorsAbsorbersTableAdapter != null)
            {
                tAM?.DetectorsAbsorbersTableAdapter.Connection.Close();
                tAM?.DetectorsAbsorbersTableAdapter.Dispose();
            }
            // if (tAM?.DetectorsDimensionsTableAdapter != null)
            {
                tAM?.DetectorsDimensionsTableAdapter.Connection.Close();
                tAM?.DetectorsDimensionsTableAdapter.Dispose();
            }
            // if (tAM?.HoldersTableAdapter != null)
            {
                tAM?.HoldersTableAdapter.Connection.Close();
                tAM?.HoldersTableAdapter.Dispose();
            }
            // if (tAM?.DetectorsCurvesTableAdapter != null)
            {
                tAM?.DetectorsCurvesTableAdapter.Connection.Close();
                tAM?.DetectorsCurvesTableAdapter.Dispose();
            }
            // if (tAM?.SolangTableAdapter != null)
            {
                tAM?.SolangTableAdapter.Connection.Close();
                tAM?.SolangTableAdapter.Dispose();
            }

            // if (tAM?.COINTableAdapter != null)
            {
                tAM?.COINTableAdapter.Connection.Close();
                tAM?.COINTableAdapter.Dispose();
            }

            tAM.CompositionsTableAdapter = null;

            tAM.MatrixTableAdapter = null;
            tAM.VialTypeTableAdapter = null;
            tAM.GeometryTableAdapter = null;
            tAM.MUESTableAdapter = null;
            tAM.DetectorsAbsorbersTableAdapter = null;
            tAM.DetectorsDimensionsTableAdapter = null;
            tAM.HoldersTableAdapter = null;
            tAM.DetectorsCurvesTableAdapter = null;
            tAM.SolangTableAdapter = null;

            tAM.COINTableAdapter = null;
        }
        public void InitializePeaksAdapters()
        {
            tAM.MeasurementsTableAdapter = new LINAATableAdapters.MeasurementsTableAdapter();
            adapters.Add(tAM.MeasurementsTableAdapter, tAM.MeasurementsTableAdapter);
            tAM.PeaksTableAdapter = new LINAATableAdapters.PeaksTableAdapter();
            adapters.Add(tAM.PeaksTableAdapter, tAM.PeaksTableAdapter);
            // this.tAM.IRequestsAveragesTableAdapter = new LINAATableAdapters.IRequestsAveragesTableAdapter();
            //this.tAM.IPeakAveragesTableAdapter = new LINAATableAdapters.IPeakAveragesTableAdapter();
        }
        public void InitializeSolCoinAdapters()
        {
            tAM.MatrixTableAdapter = new LINAATableAdapters.MatrixTableAdapter();

            adapters.Add(tAM.MatrixTableAdapter, tAM.MatrixTableAdapter);
            tAM.CompositionsTableAdapter = new LINAATableAdapters.CompositionsTableAdapter();
            adapters.Add(tAM.CompositionsTableAdapter, tAM.CompositionsTableAdapter);
            tAM.VialTypeTableAdapter = new LINAATableAdapters.VialTypeTableAdapter();
            adapters.Add(tAM.VialTypeTableAdapter, tAM.VialTypeTableAdapter);
            tAM.GeometryTableAdapter = new LINAATableAdapters.GeometryTableAdapter();
            adapters.Add(tAM.GeometryTableAdapter, tAM.GeometryTableAdapter);
            tAM.MUESTableAdapter = new LINAATableAdapters.MUESTableAdapter();
            adapters.Add(tAM.MUESTableAdapter, tAM.MUESTableAdapter);
            tAM.DetectorsAbsorbersTableAdapter = new LINAATableAdapters.DetectorsAbsorbersTableAdapter();
            adapters.Add(tAM.DetectorsAbsorbersTableAdapter, tAM.DetectorsAbsorbersTableAdapter);
            tAM.DetectorsDimensionsTableAdapter = new LINAATableAdapters.DetectorsDimensionsTableAdapter();
            adapters.Add(tAM.DetectorsDimensionsTableAdapter, tAM.DetectorsDimensionsTableAdapter);
            tAM.HoldersTableAdapter = new LINAATableAdapters.HoldersTableAdapter();
            adapters.Add(tAM.HoldersTableAdapter, tAM.HoldersTableAdapter);

            tAM.DetectorsCurvesTableAdapter = new LINAATableAdapters.DetectorsCurvesTableAdapter();
            adapters.Add(tAM.DetectorsCurvesTableAdapter, tAM.DetectorsCurvesTableAdapter);
            tAM.SolangTableAdapter = new LINAATableAdapters.SolangTableAdapter();
            adapters.Add(tAM.SolangTableAdapter, tAM.SolangTableAdapter);
            tAM.COINTableAdapter = new LINAATableAdapters.COINTableAdapter();
            adapters.Add(tAM.COINTableAdapter, tAM.COINTableAdapter);
        }
    }
}