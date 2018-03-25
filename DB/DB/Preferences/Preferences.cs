using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        public partial class PreferencesDataTable : IColumn
        {
            private DataColumn[] fillCols = null;
            private DataColumn[] mainCols = null;
            private DataColumn[] otherCols = null;

            private DataColumn[] peakCols = null;

            public IEnumerable<DataColumn> FillCols
            {
                get
                {
                    if (fillCols == null)
                    {
                        fillCols = new DataColumn[] {
                            this.LastToDoColumn,
                            this.LastIrradiationProjectColumn,
                            this.FillByHLColumn,
                             FillBySpectraColumn,
                             ShowSampleDescriptionColumn
                             };
                    }
                    return fillCols;
                }
            }

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

            public IEnumerable<DataColumn> MainCols
            {
                get
                {
                    if (mainCols == null)
                    {
                        mainCols = new DataColumn[] {
                            this.OfflineColumn,
                            this.IsMSMQColumn,
                            this.IsSQLColumn,
                             AdvancedEditorColumn
                             };
                    }
                    return mainCols;
                }
            }

            public IEnumerable<DataColumn> OtherCols
            {
                get
                {
                    if (otherCols == null)
                    {
                        otherCols = new DataColumn[] {
                            ShowSolangColumn,
                            AutoLoadColumn,
                       // RunInBackgroundColumn
                             };
                    }
                    return otherCols;
                }
            }

            public IEnumerable<DataColumn> PeakCols
            {
                get
                {
                    if (peakCols == null)
                    {
                        peakCols = new DataColumn[] {
                            this.windowAColumn,
                            this.windowBColumn,
                            this.minAreaColumn,
                             maxUncColumn,
                             MeasIdxColumn,PositionIdxColumn,DetectorIdxColumn
                             };
                    }
                    return peakCols;
                }
            }
        }

        partial class PreferencesRow : IRow
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
                this.LastAccessDate = DateTime.Now;

                foreach (DataColumn item in this.tablePreferences.Columns)
                {
                    Check(item);
                }

                checkConnections();
            }

            public void Check(DataColumn Column)
            {
                bool nulo = EC.CheckNull(Column, this);

                if (tablePreferences.OtherCols.Contains(Column))
                {
                    if (Column == this.tablePreferences.ShowSolangColumn)
                    {
                        if (nulo) ShowSolang = false;
                    }
                    else if (Column == this.tablePreferences.AutoLoadColumn)
                    {
                        if (nulo) AutoLoad = true;
                    }
                }
                else if (Column == this.tablePreferences.RunInBackgroundColumn)
                {
                    if (nulo) RunInBackground = false;
                    // this.tablePreferences.RunInBackground?.Invoke(null, EventArgs.Empty);
                }
                if (tablePreferences.FillCols.Contains(Column))
                {
                    if (Column == this.tablePreferences.LastToDoColumn)
                    {
                        // if (nulo ) LastToDo = string.Empty;
                    }
                    else if (Column == this.tablePreferences.FillByHLColumn)
                    {
                        if (nulo) FillByHL = true;
                    }
                    else if (Column == this.tablePreferences.FillBySpectraColumn)
                    {
                        if (nulo) FillBySpectra = true;
                    }
                    else if (Column == this.tablePreferences.LastIrradiationProjectColumn)
                    {
                        if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                    }
                    else if (Column == this.tablePreferences.ShowSampleDescriptionColumn)
                    {
                        if (nulo) ShowSampleDescription = true;
                    }
                }
                else if (tablePreferences.MainCols.Contains(Column))
                {
                    if (IsOfflineNull()) Offline = false;
                    if (IsIsSQLNull()) this.IsSQL = false;
                    if (IsIsMSMQNull()) this.IsMSMQ = false;
                    if (IsAdvancedEditorNull()) this.AdvancedEditor = false;
                }
                else if (tablePreferences.PeakCols.Contains(Column))
                {
                    if (IswindowANull()) windowA = 1.5;
                    if (IswindowBNull()) windowB = 0.001;
                    if (IsminAreaNull()) minArea = 500;
                    if (IsmaxUncNull()) maxUnc = 50;
                    if (IsDetectorIdxNull()) DetectorIdx = 3;
                    if (IsPositionIdxNull()) PositionIdx = 2;
                    if (IsMeasIdxNull()) MeasIdx = 1;
                }
            }

            public new bool HasErrors()
            {
                return base.HasErrors;
            }

            public void SetConnections(string hLString, string lIMSString, string spectraSrv, string spectraPath)
            {
                HL = hLString;
                LIMS = lIMSString;
                SpectraSvr = spectraSrv;
                string spectra = spectraPath;
                if (!spectra[spectra.Length - 1].ToString().Equals("\\")) spectra += "\\";
                Spectra = spectra;
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                // throw new NotImplementedException();
            }

            private void checkConnections()
            {
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
            }
        }

        protected internal void handlersPreferences()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Preferences));
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SSFPref));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(XCOMPref));
        }
    }
}