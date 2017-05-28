using System;
using System.Collections.Generic;
using System.Data;
using DB.Properties;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        public partial class PreferencesDataTable : IColumn
        {
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }
        }

        public partial class SSFPrefDataTable : IColumn
        {
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }
        }

        public partial class SSFPrefRow : IRow
        {
            public void Check()
            {
              
                if (IsCalibrationNull()) Calibration = false;
                if (this.IsDoCKNull()) DoCK = false;
                if (this.IsDoMatSSFNull()) DoMatSSF = true;
                if (this.IsLoopNull()) Loop = true;
                if (this.IsRoundingNull()) Rounding = "N3";
                if (IsShowOtherNull()) ShowOther = false;
                if (IsOverridesNull()) Overrides = false;
                if (IsShowMatSSFNull()) ShowMatSSF = false;

                foreach (DataColumn item in this.tableSSFPref.Columns)
                {
                    Check(item);
                }
            }

            public void Check(DataColumn column)
            {

                if (this.IsCalcMassNull()) CalcMass = false;
                if (IsAAFillHeightNull()) AAFillHeight = false;
                if (IsAARadiusNull()) AARadius = false;
                if (this.IsCalcDensityNull()) CalcDensity = false;

                //    Check();
                if (column == this.tableSSFPref.AAFillHeightColumn)
                {
                   
                    if (AAFillHeight)
                    {
                        AARadius = false;
                        CalcDensity = false;
                        CalcMass = false;
                    }
                    else if (!AARadius && !CalcDensity)
                    {
                        CalcMass = true;
                    }
                }
                else if (column == this.tableSSFPref.AARadiusColumn)
                {
                  

                    if (AARadius)
                    {
                        AAFillHeight = false;
                        CalcDensity = false;
                        CalcMass = false;
                    }
                    else if (!AAFillHeight && !CalcDensity)
                    {
                        CalcMass = true;
                    }
                }
                else if (column == this.tableSSFPref.CalcDensityColumn)
                {
                  

                    if (CalcDensity)
                    {
                        AAFillHeight = false;
                        AARadius = false;
                        CalcMass = false;
                    }
                    else if (!AARadius && !AAFillHeight)
                    {
                        CalcMass = true;
                    }
                }
                else
                {
                
                    EC.CheckNull(column, this);
                }
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
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

                if (IsOfflineNull()) Offline = false;
                if (IsIsSQLNull()) this.IsSQL = false;
                if (IsIsMSMQNull()) this.IsMSMQ = false;
                if (IsAdvancedEditorNull()) this.AdvancedEditor = false;
                if (IswindowANull()) windowA = 1.5;
                if (IswindowBNull()) windowB = 0.001;
                if (IsminAreaNull()) minArea = 500;
                if (IsmaxUncNull()) maxUnc = 50;
                if (IsAutoLoadNull()) AutoLoad = true;
                if (IsShowSolangNull()) ShowSolang = false;
                // if (IsShowSampleDescriptionNull()) ShowSampleDescription = true;
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

                foreach (DataColumn item in this.tablePreferences.Columns)
                {
                    Check(item);
                }

                Settings.Default.Save();
            }


            public void Check(DataColumn Column)
            {

             

                EC.CheckNull(Column, this);
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                // throw new NotImplementedException();
            }
        }

        protected internal void handlersPreferences()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Preferences));
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SSFPref));
        }
    }
}