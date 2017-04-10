using System;
using System.Collections.Generic;
using System.Linq;

//using DB.Interfaces;
using Rsx;

namespace DB.Tools
{
    /// <summary>
    /// PUBLIC
    /// </summary>
    public partial class WC : IWC
    {
        private string alpha;

        private bool asSamples = false;

        private SampleChecker checkNode = null;

        private string f;

        private AsyncCallBack finishMethod = null;

        private string geo;

        private string gt;

        private double maxUnc = 0;

        private double minArea = 0;

        private bool showSolang;

        private bool showSSF;

        private double windowA = 0;

        private double windowB = 0;

        public delegate void AsyncCallBack(object sender);

        public delegate void SampleChecker(ref LINAA.SubSamplesRow sample);

        public IEnumerable<LINAA.SubSamplesRow> SelectedSamples
        {
            get { return selectedSamples; }
            set
            {
                selectedSamples = value.Where(o => !EC.IsNuDelDetch(o)).ToList();
            }
        }

        public bool ShowSolang
        {
            //  get { return showSolang; }
            set { showSolang = value; }
        }

        public bool ShowSSF
        {
            //  get { return showSSF; }
            set { showSSF = value; }
        }

        public void SetExternalMethods(SampleChecker sampleChecker, AsyncCallBack FinishMethod)
        {
            finishMethod = FinishMethod;
            checkNode = sampleChecker;
        }

        public void SetOverriders(string fbox, string alphabox, string gtbox, string geobox, bool asSamplesChecked)
        {
            f = fbox;
            alpha = alphabox;
            gt = gtbox;
            geo = geobox;
            asSamples = asSamplesChecked;
        }

        public void SetPeakSearch(string mArea, string mUnc, string Aw, string Bw)
        {
            minArea = Convert.ToDouble(mArea);
            maxUnc = Convert.ToDouble(mUnc);
            windowA = Convert.ToDouble(Aw);
            windowB = Convert.ToDouble(Bw);
        }
    }
}