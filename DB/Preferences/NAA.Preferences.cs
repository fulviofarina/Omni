using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using DB.Properties;

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
                // if (IswindowANull()) windowA = 1.5;
                // if (IswindowBNull()) windowB = 0.001;
                // if (IsminAreaNull()) minArea = 500;
                // if (IsmaxUncNull()) maxUnc = 50;
                //    if (IsAutoLoadNull()) AutoLoad = true;
                //    if (IsShowSolangNull()) ShowSolang = false;
                // if (IsShowMatSSFNull()) ShowMatSSF = false;
                //if (IsAAFillHeightNull()) AAFillHeight = false;
                //if (IsAARadiusNull()) AARadius = true;
                //   if (IsShowSampleDescriptionNull()) ShowSampleDescription = true;
                this.LastAccessDate = DateTime.Now;
                if (IsLastToDoNull()) LastToDo = string.Empty;
                if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                //  if (IsFillByHLNull()) FillByHL = true;
                // if (IsFillBySpectraNull()) FillBySpectra = true;
                if (IsHLNull()) HL = Settings.Default.HLSNMNAAConnectionString;
                else Settings.Default["HLSNMNAAConnectionString"] = HL;
                if (IsLIMSNull()) LIMS = Settings.Default.NAAConnectionString;
                else Settings.Default["NAAConnectionString"] = LIMS;
                if (IsSpectraNull()) Spectra = Settings.Default.SpectraFolder;
                else Settings.Default["SpectraFolder"] = Spectra;
                if (IsSpectraSvrNull()) SpectraSvr = Settings.Default.SpectraServer;
                else Settings.Default["SpectraServer"] = SpectraSvr;

                if (IsSolCoiFolderNull()) SolCoiFolder = Resources.SolCoiFolder;
                //   else Settings.Default["SOLCOIFolder"] = SolCoiFolder;

                Settings.Default.Save();
            }
        }

        public partial class SSFPrefRow
        {
            public void Check()
            {
                //  DoSolang = false;
                //  DoMatSSF = false;//fix

                //  if (IsAutoLoadNull()) AutoLoad = true;
                //  if (IsShowSolangNull()) ShowSolang = false;
                //  if (IsShowMatSSFNull()) ShowMatSSF = false;
                // if (IsAAFillHeightNull()) AAFillHeight = false;
                //if (IsAARadiusNull()) AARadius = true;
                if (IsFolderNull()) Folder = Resources.SSFFolder;
                //    else Settings.Default["SSFFolder"] = Folder;

                // ShowSampleDescription = true;
                //   if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                //   if (IsFillByHLNull()) FillByHL = true;
                //  if (IsFillBySpectraNull()) FillBySpectra = true;
                //// if (IsHLNull()) HL = DB.Properties.Settings.Default.HLSNMNAAConnectionString;
                // else DB.Properties.Settings.Default["HLSNMNAAConnectionString"] = HL;

                // if (IsLIMSNull()) LIMS = DB.Properties.Settings.Default.NAAConnectionString;
                //  else DB.Properties.Settings.Default["NAAConnectionString"] = LIMS;
                //  if (IsSpectraNull()) Spectra = DB.Properties.Settings.Default.SpectraFolder;
                //  else DB.Properties.Settings.Default["SpectraFolder"] = Spectra;
                //  if (IsSpectraSvrNull()) SpectraSvr = DB.Properties.Settings.Default.SpectraServer;
                //  else DB.Properties.Settings.Default["SpectraServer"] = SpectraSvr;

                Settings.Default.Save();
            }
        }
    }
}