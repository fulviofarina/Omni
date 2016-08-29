using System.Collections.Generic;
using System.Windows.Forms;

namespace DB.Tools
{
    public interface IWC
    {
        void CalculateMatSSF(bool doSSF, char fluxType);

        void CalculatePeaks(bool monitorsOnly, bool uncsOnly);

        void CalculateSolang(bool DoSolang, bool AlsoCOIS, string IntegrateAs);

        void CancelWorkers();

        void Check(object sender);

        void EffiLoad(bool coin, bool all);

        void Fetch(bool transfer);

        void LoadPeaks(bool deleteOnly, bool transfer);

        void SetOverriders(string fbox, string alphabox, string gtbox, string geobox, bool asSamplesChecked);

        void SetPeakSearch(string mArea, string mUnc, string Aw, string Bw);

        IEnumerable<LINAA.SubSamplesRow> SelectedSamples { get; set; }

        void SetExternalMethods(WC.SampleChecker sampleChecker, WC.AsyncCallBack FinishMethod);

        bool ShowSSF { get; set; }
        bool ShowSolang { get; set; }

        void PopulateIsotopes();

        //  void    Predict(ref IEnumerable<LINAA.SubSamplesRow> samples);
        void RefreshDB(bool official);

        void SelectItems(bool all);

        void SetNodes(ref TreeView tv);
    }
}