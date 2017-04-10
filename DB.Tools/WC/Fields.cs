using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

//using DB.Interfaces;

namespace DB.Tools
{
    public partial class WC
    {
        private string Name;

        private List<BackgroundWorker> workers;
        private List<BackgroundWorker> workersCancelled;

        private ToolStripProgressBar progress = null;
        private Interface Interface = null;

        private LINAA Linaa = null;

        private TreeView tv = null;

        private ToolStripMenuItem cancel = null;

        private IEnumerable<LINAA.SubSamplesRow> selectedSamples = null;

        private SolCoin.IntegrationModes mode;

        private string samplesCol;
        private string composCol;

        private enum R
        {
            SampleMeasStatus = 5,
            RowsDelete = 7,
            SSFSet = 10,
            RowsSave = 11,
            RowsAccept = 12,
            SampleStatus = 13,
            PeaksDelSave = 14,
            MergeTable = 15,

            SolcoiLoaded = 67,
            SolcoiEnded = 69,
            SSFSave = 77,

            SamplePeaksStatus = 95,
            SampleInfere = 97,
            SampleCheck = 98,

            Progress = 100,

            AddException = 18
        };
    }
}