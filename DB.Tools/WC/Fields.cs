using System.Collections.Generic;
using System.Windows.Forms;

//using DB.Interfaces;

namespace DB.Tools
{
  public partial class WC
  {
    private string Name;

    private List<System.ComponentModel.BackgroundWorker> workers;
    private List<System.ComponentModel.BackgroundWorker> workersCancelled;

    private ToolStripProgressBar progress = null;
    private Interface Interface = null;

    private LINAA Linaa = null;

    private TreeView tv = null;

    private System.Windows.Forms.ToolStripMenuItem cancel = null;

    private IEnumerable<LINAA.SubSamplesRow> selectedSamples = null;

    private SolCoin.IntegrationModes mode;

    private double minArea = 0;
    private double maxUnc = 0;
    private double windowA = 0;
    private double windowB = 0;

    private string f;
    private string alpha;
    private string gt;
    private string geo;
    private bool asSamples = false;

    private AsyncCallBack finishMethod = null;
    private SampleChecker checkNode = null;

    private bool showSolang;
    private string samplesCol;
    private string composCol;
    private bool showSSF;

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