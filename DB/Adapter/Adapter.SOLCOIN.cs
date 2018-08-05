using DB.LINAATableAdapters;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        public void DisposePeaksAdapters()
        {
            TAM.MeasurementsTableAdapter?.Connection.Close();
            TAM.MeasurementsTableAdapter?.Dispose();

            TAM.PeaksTableAdapter?.Connection.Close();
            TAM.PeaksTableAdapter?.Dispose();

            TAM.ToDoTableAdapter?.Connection.Close();
            TAM.ToDoTableAdapter?.Dispose();

            TAM.MeasurementsTableAdapter = null;
            TAM.PeaksTableAdapter = null;
            TAM.ToDoTableAdapter = null;
        }

        public void DisposeSolCoinAdapters()
        {
            tAM?.CompositionsTableAdapter.Connection.Close();
            tAM?.CompositionsTableAdapter.Dispose();

            tAM?.MatrixTableAdapter.Connection.Close();
            tAM?.MatrixTableAdapter.Dispose();

            tAM?.VialTypeTableAdapter.Connection.Close();
            tAM?.VialTypeTableAdapter.Dispose();

            tAM?.GeometryTableAdapter.Connection.Close();
            tAM?.GeometryTableAdapter.Dispose();

            tAM?.MUESTableAdapter.Connection.Close();
            tAM?.MUESTableAdapter.Dispose();

            tAM?.DetectorsAbsorbersTableAdapter.Connection.Close();
            tAM?.DetectorsAbsorbersTableAdapter.Dispose();

            tAM?.DetectorsDimensionsTableAdapter.Connection.Close();
            tAM?.DetectorsDimensionsTableAdapter.Dispose();

            tAM?.HoldersTableAdapter.Connection.Close();
            tAM?.HoldersTableAdapter.Dispose();

            tAM?.DetectorsCurvesTableAdapter.Connection.Close();
            tAM?.DetectorsCurvesTableAdapter.Dispose();

            tAM?.SolangTableAdapter.Connection.Close();
            tAM?.SolangTableAdapter.Dispose();

            tAM?.COINTableAdapter.Connection.Close();
            tAM?.COINTableAdapter.Dispose();

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
            tAM.MeasurementsTableAdapter = new MeasurementsTableAdapter();
            adapters.Add(tAM.MeasurementsTableAdapter, tAM.MeasurementsTableAdapter);
            tAM.PeaksTableAdapter = new PeaksTableAdapter();
            adapters.Add(tAM.PeaksTableAdapter, tAM.PeaksTableAdapter);
            // this.tAM.IRequestsAveragesTableAdapter = new LINAATableAdapters.IRequestsAveragesTableAdapter();
            //this.tAM.IPeakAveragesTableAdapter = new LINAATableAdapters.IPeakAveragesTableAdapter();
        }

        public void InitializeSolCoinAdapters()
        {
            tAM.MatrixTableAdapter = new MatrixTableAdapter();

            adapters.Add(tAM.MatrixTableAdapter, tAM.MatrixTableAdapter);
            tAM.CompositionsTableAdapter = new CompositionsTableAdapter();
            adapters.Add(tAM.CompositionsTableAdapter, tAM.CompositionsTableAdapter);
            tAM.VialTypeTableAdapter = new VialTypeTableAdapter();
            adapters.Add(tAM.VialTypeTableAdapter, tAM.VialTypeTableAdapter);
            tAM.GeometryTableAdapter = new GeometryTableAdapter();
            adapters.Add(tAM.GeometryTableAdapter, tAM.GeometryTableAdapter);
            tAM.MUESTableAdapter = new MUESTableAdapter();
            adapters.Add(tAM.MUESTableAdapter, tAM.MUESTableAdapter);

            tAM.DetectorsAbsorbersTableAdapter = new DetectorsAbsorbersTableAdapter();
            adapters.Add(tAM.DetectorsAbsorbersTableAdapter, tAM.DetectorsAbsorbersTableAdapter);
            tAM.DetectorsDimensionsTableAdapter = new DetectorsDimensionsTableAdapter();
            adapters.Add(tAM.DetectorsDimensionsTableAdapter, tAM.DetectorsDimensionsTableAdapter);
            tAM.HoldersTableAdapter = new HoldersTableAdapter();
            adapters.Add(tAM.HoldersTableAdapter, tAM.HoldersTableAdapter);

            tAM.DetectorsCurvesTableAdapter = new DetectorsCurvesTableAdapter();
            adapters.Add(tAM.DetectorsCurvesTableAdapter, tAM.DetectorsCurvesTableAdapter);
            tAM.SolangTableAdapter = new SolangTableAdapter();
            adapters.Add(tAM.SolangTableAdapter, tAM.SolangTableAdapter);
            tAM.COINTableAdapter = new COINTableAdapter();
            adapters.Add(tAM.COINTableAdapter, tAM.COINTableAdapter);
        }
    }
}