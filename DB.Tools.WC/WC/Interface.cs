using System.Collections.Generic;
using System.Windows.Forms;

namespace DB.Tools
{
    public interface IWC
    {
        IEnumerable<LINAA.SubSamplesRow> SelectedSamples { get; set; }

        bool ShowSolang { set; }

        bool ShowSSF { set; }

        void CalculateMatSSF(bool doSSF, char fluxType);

        void CalculatePeaks(bool monitorsOnly, bool uncsOnly);

        void CalculateSolang(bool DoSolang, bool AlsoCOIS, string IntegrateAs);

        void CancelWorkers();

        void Check(object sender);

        void EffiLoad(bool coin, bool all);

        void Fetch(bool transfer);

        void LoadPeaks(bool deleteOnly, bool transfer);

        void PopulateIsotopes();

        void Predict();

        void RefreshDB(bool official);

        void SelectItems(bool all);

        void SetExternalMethods(WC.SampleChecker sampleChecker, WC.AsyncCallBack FinishMethod);

        void SetNodes(ref TreeView tv);

        void SetOverriders(string fbox, string alphabox, string gtbox, string geobox, bool asSamplesChecked);

        void SetPeakSearch(string mArea, string mUnc, string Aw, string Bw);
    }
}