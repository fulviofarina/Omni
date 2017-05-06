using System;
using DB.Properties;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {

        protected internal void handlersPreferences()
        {
            handlers.Add(Preferences.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Preferences));
            handlers.Add(SSFPref.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SSFPref));
        }


        public partial class SSFPrefRow
        {
            public void Check()
            {
            //    if (this.IsLastUnitIDNull()) LastUnitID = -1;
                if (this.IsCalcDensityNull()) CalcDensity = true;
                if (this.IsCalcMassNull()) CalcMass = false;
                if (IsAAFillHeightNull()) AAFillHeight = false;
                if (IsAARadiusNull()) AARadius = false;

                if (this.IsDoCKNull()) DoCK = false;
                if (this.IsDoMatSSFNull()) DoMatSSF = true;
                if (this.IsLoopNull()) Loop = true;
                if (this.IsRoundingNull()) Rounding = "N3";
                //    if (this.IsSQLNull()) SQL = Settings.Default.SSFSQL;
                //  else Settings.Default["SSFSQL"] = SQL;
                if (IsShowOtherNull()) ShowOther = false;
                if (IsOverridesNull()) Overrides = false;
                //   if (IsAutoLoadNull()) AutoLoad = true;

                if (IsShowMatSSFNull()) ShowMatSSF = false;

        //        this.AcceptChanges();

                Settings.Default.Save();
            }
        }

        public partial class PreferencesDataTable
        {
            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
           //     PreferencesRow p = e.Row as PreferencesRow;
                try
                {
                    EC.CheckNull(e.Column, e.Row);
                }
                catch (Exception ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
                //  throw new NotImplementedException();
            }
        }

        public partial class SSFPrefDataTable
        {
            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                SSFPrefRow p = e.Row as SSFPrefRow;

                try
                {
                    if (e.Column == this.AAFillHeightColumn)
                    {
                        if (p.AAFillHeight)
                        {
                            p.AARadius = false;
                            p.CalcDensity = false;
                            p.CalcMass = false;

                        }
                        else if (!p.AARadius && !p.CalcDensity)
                        {
                            p.CalcMass = true;

                        }

                    }
                    else if (e.Column == this.AARadiusColumn)
                    {
                        if (p.AARadius)
                        {
                            p.AAFillHeight = false;
                            p.CalcDensity = false;
                            p.CalcMass = false;

                        }
                        else if (!p.AAFillHeight && !p.CalcDensity)
                        {
                            p.CalcMass = true;

                        }

                    }
                    else if (e.Column == this.CalcDensityColumn)
                    {
                        if (p.CalcDensity)
                        {
                            p.AAFillHeight = false;
                            p.AARadius = false;
                            p.CalcMass = false;
                        }
                        else if (!p.AARadius && !p.AAFillHeight)
                        {
                            p.CalcMass = true;

                        }
                    }
                    else EC.CheckNull(e.Column, e.Row);
                   
                }
                catch (Exception ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
            }
        }

        partial class PreferencesRow
        {
            private bool usrAnal = false;
            /// <summary>
            /// Should fix this
            /// </summary>
            public bool UsrAnal
            {
                //TODO: change this
                get { return usrAnal; }
                set { usrAnal = value; }
            }

            public void Check()
            {
                if (IsOfflineNull()) Offline = false;
                if (IsIsSQLNull()) this.IsSQL = false;
                if (IsIsMSMQNull()) this.IsMSMQ = false;
                if (IsAdvancedEditorNull()) this.AdvancedEditor = true;
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
                if (IsLIMSNull()) LIMS = Settings.Default.localDB;
               ////// //reponer
                Settings.Default["NAAConnectionString"] = LIMS;
                if (IsSpectraNull()) Spectra = Settings.Default.SpectraFolder;
                else Settings.Default["SpectraFolder"] = Spectra;
                if (IsSpectraSvrNull()) SpectraSvr = Settings.Default.SpectraServer;
                else Settings.Default["SpectraServer"] = SpectraSvr;

                Settings.Default.Save();
                //    if (IsFolderNull()) Folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                //   else Settings.Default["Folder"] = Folder;

                //    if (IsSolCoiFolderNull()) SolCoiFolder = Resources.SolCoiFolder;
                //   else Settings.Default["SOLCOIFolder"] = SolCoiFolder;
            }
        }
    }
}