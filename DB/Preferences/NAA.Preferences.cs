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
                if (IsOfflineNull()) Offline = false;

                if (IswindowANull()) windowA = 1.5;
                if (IswindowBNull()) windowB = 0.001;
                if (IsminAreaNull()) minArea = 500;
                if (IsmaxUncNull()) maxUnc = 50;
                if (IsAutoLoadNull()) AutoLoad = true;
                if (IsShowSolangNull()) ShowSolang = false;
                //   if (IsShowSampleDescriptionNull()) ShowSampleDescription = true;
                this.LastAccessDate = DateTime.Now;
                if (IsLastToDoNull()) LastToDo = string.Empty;
                if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                if (IsFillByHLNull()) FillByHL = true;
                if (IsFillBySpectraNull()) FillBySpectra = true;
                if (IsHLNull()) HL = Settings.Default.HLSNMNAAConnectionString;
                else Settings.Default["HLSNMNAAConnectionString"] = HL;
                if (IsLIMSNull()) LIMS = Settings.Default.NAAConnectionString;
                else Settings.Default["NAAConnectionString"] = LIMS;
                if (IsSpectraNull()) Spectra = Settings.Default.SpectraFolder;
                else Settings.Default["SpectraFolder"] = Spectra;
                if (IsSpectraSvrNull()) SpectraSvr = Settings.Default.SpectraServer;
                else Settings.Default["SpectraServer"] = SpectraSvr;

                //    if (IsFolderNull()) Folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                //   else Settings.Default["Folder"] = Folder;

                //    if (IsSolCoiFolderNull()) SolCoiFolder = Resources.SolCoiFolder;
                //   else Settings.Default["SOLCOIFolder"] = SolCoiFolder;

                Settings.Default.Save();
            }
        }

        public partial class SSFPrefRow
        {
            public void Check()
            {
                if (this.IsLastUnitIDNull()) LastUnitID = 0;
                if (this.IsCalcDensityNull()) CalcDensity = true;
                if (this.IsDoCKNull()) DoCK = false;
                if (this.IsDoMatSSFNull()) DoMatSSF = true;
                if (this.IsLoopNull()) Loop = false;
                if (this.IsRoundingNull()) Rounding = "N4";
                //    if (this.IsSQLNull()) SQL = Settings.Default.SSFSQL;
                //  else Settings.Default["SSFSQL"] = SQL;
                if (IsShowOtherNull()) ShowOther = false;

                //   if (IsAutoLoadNull()) AutoLoad = true;

                if (IsShowMatSSFNull()) ShowMatSSF = false;
                if (IsAAFillHeightNull()) AAFillHeight = false;
                if (IsAARadiusNull()) AARadius = true;

                Settings.Default.Save();
            }
        }
    }
}