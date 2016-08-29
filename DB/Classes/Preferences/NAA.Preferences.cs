using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        partial class PreferencesRow
        {
            private bool usrAnal = false;

            public bool UsrAnal
            {
                get { return usrAnal; }
                set { usrAnal = value; }
            }

            public void Check()
            {
                DoSolang = false;
                DoMatSSF = false;//fix

                if (IswindowANull()) windowA = 1.5;
                if (IswindowBNull()) windowB = 0.001;
                if (IsminAreaNull()) minArea = 500;
                if (IsmaxUncNull()) maxUnc = 50;
                if (IsAutoLoadNull()) AutoLoad = true;
                if (IsShowSolangNull()) ShowSolang = false;
                if (IsShowMatSSFNull()) ShowMatSSF = false;
                if (IsAAFillHeightNull()) AAFillHeight = false;
                if (IsAARadiusNull()) AARadius = true;
                if (IsShowSampleDescriptionNull()) ShowSampleDescription = true;
                if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                if (IsFillByHLNull()) FillByHL = true;
                if (IsFillBySpectraNull()) FillBySpectra = true;
                if (IsHLNull()) HL = DB.Properties.Settings.Default.HLSNMNAAConnectionString;
                else DB.Properties.Settings.Default["HLSNMNAAConnectionString"] = HL;
                if (IsLIMSNull()) LIMS = DB.Properties.Settings.Default.NAAConnectionString;
                else DB.Properties.Settings.Default["NAAConnectionString"] = LIMS;
                if (IsSpectraNull()) Spectra = DB.Properties.Settings.Default.SpectraFolder;
                else DB.Properties.Settings.Default["SpectraFolder"] = Spectra;
                if (IsSpectraSvrNull()) SpectraSvr = DB.Properties.Settings.Default.SpectraServer;
                else DB.Properties.Settings.Default["SpectraServer"] = SpectraSvr;
                DB.Properties.Settings.Default.Save();
            }
        }

     

        partial class ExceptionsDataTable
        {
            public void RemoveDuplicates()
            {
                HashSet<string> hs = new HashSet<string>();
                IEnumerable<LINAA.ExceptionsRow> ordered = this.OrderByDescending(o => o.Date);
                ordered = ordered.TakeWhile(o => !hs.Add(o.StackTrace));
                for (int i = ordered.Count() - 1; i >= 0; i--)
                {
                    LINAA.ExceptionsRow e = ordered.ElementAt(i);
                    e.Delete();
                }

                hs.Clear();
                hs = null;
                this.AcceptChanges();
            }

            public void AddExceptionsRow(Exception ex)
            {
                string target = string.Empty;
                string stack = string.Empty;
                string source = string.Empty;

                if (ex.Source != null) source = ex.Source;
                if (ex.TargetSite != null) target = ex.TargetSite.Name;
                if (ex.StackTrace != null) stack = ex.StackTrace;

                AddExceptionsRow(target, ex.Message, stack, source, DateTime.Now);
            }
        }
    }
}