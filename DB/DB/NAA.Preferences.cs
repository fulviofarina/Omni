using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
    

        public partial class SSFPrefDataTable : IColumn
        {
          

            public EventHandler SampleChanged;

            //   private EventHandler checkChanged2;
            public EventHandler DoChilianChanged;
         

            public EventHandler OverriderChanged;
           
        



            public EventHandler DoMatSSFChanged;
           



            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }
            private DataColumn[] autoCal = null;
            public IEnumerable<DataColumn> AutoCal
            {
                get
                {
                    if (autoCal ==null)
                    {
                        autoCal = new DataColumn[] { this.CalcDensityColumn,
                            this.CalcMassColumn, this.AAFillHeightColumn,
                            this.AARadiusColumn  };
                    }
                    return autoCal;
                }
            }
            private DataColumn[] doCols = null;
            public IEnumerable<DataColumn> DoCols
            {
                get
                {
                    if (doCols == null)
                    {
                        doCols = new DataColumn[] { this.DoCKColumn,
                            this.DoMatSSFColumn, this.LoopColumn
                             };
                    }
                    return doCols;
                }
            }
            private DataColumn[] otherCols = null;
            public IEnumerable<DataColumn> OtherCols
            {
                get
                {
                    if (otherCols == null)
                    {
                        otherCols = new DataColumn[] { this.CalibrationColumn,
                            this.RoundingColumn, this.ShowOtherColumn,
                             ShowMatSSFColumn
                             };
                    }
                    return otherCols;
                }
            }

        }

        public partial class SSFPrefRow : IRow
        {
            public void Check()
            {

            //    DoMatSSF = true;
             //   DoCK = false;

                foreach (DataColumn item in this.tableSSFPref.Columns)
                {
                    Check(item);
                }
            }

            public void Check(DataColumn column)
            {


              

                if (this.tableSSFPref.AutoCal.Contains(column))
                {
                    checkAutoCalculation(column);
                  
                }
                else if (tableSSFPref.DoCols.Contains(column))
                {
                    checkDoColumns( column);
                }
                else if (tableSSFPref.OtherCols.Contains(column))
                {
                    checkOtherColumns( column);
                }
                else if (column == this.tableSSFPref.OverridesColumn)
                {
                    if (IsOverridesNull()) Overrides = false;
                    this.tableSSFPref.OverriderChanged?.Invoke(null, EventArgs.Empty);
                }
                else EC.CheckNull(column, this);
            }

            private void checkOtherColumns(DataColumn column)
            {
                if (IsCalibrationNull()) Calibration = false;
                if (IsRoundingNull()) Rounding = "N3";
                if (IsShowOtherNull()) ShowOther = false;
            
                if (IsShowMatSSFNull()) ShowMatSSF = false;
            }

            private void checkDoColumns(DataColumn column)
            {
                if (column == this.tableSSFPref.DoCKColumn)
                {
                      if (this.IsDoCKNull())      DoCK = false;
                    this.tableSSFPref.DoChilianChanged?.Invoke(null, EventArgs.Empty);
                }
                else if (column == this.tableSSFPref.DoMatSSFColumn)
                {
                     if (this.IsDoMatSSFNull()) DoMatSSF = true;
                    this.tableSSFPref.DoMatSSFChanged?.Invoke(null, EventArgs.Empty);
                }
                else if (column == this.tableSSFPref.LoopColumn)
                {
                    if (this.IsLoopNull()) Loop = true;
                }
            }

            private void checkAutoCalculation(DataColumn column)
            {

                if (this.IsCalcMassNull())
                {
                    CalcMass = false;
                    return;
                }
                if (IsAAFillHeightNull())
                {
                    AAFillHeight = false;
                    return;
                }
                if (IsAARadiusNull())
                {
                    AARadius = false;
                    return;
                }
                if (this.IsCalcDensityNull())
                {
                    CalcDensity = true;
                    return;
                }

             


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

                this.tableSSFPref.SampleChanged?.Invoke(null, EventArgs.Empty);


            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }

             public new bool HasErrors()
            {
                // throw new NotImplementedException();
                return base.HasErrors;
            }
        }
        public partial class PreferencesDataTable : IColumn
        {

            public EventHandler RunInBackground;


            private DataColumn[] otherCols = null;
            public IEnumerable<DataColumn> OtherCols
            {
                get
                {
                    if (otherCols == null)
                    {
                        otherCols = new DataColumn[] {
                     
                            ShowSolangColumn,
                            AutoLoadColumn,
                       //     RunInBackgroundColumn
                             };
                    }
                    return otherCols;
                }
            }

            private DataColumn[] fillCols = null;
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


            private DataColumn[] peakCols = null;
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
                             maxUncColumn
                             };
                    }
                    return peakCols;
                }
            }

            private DataColumn[] mainCols = null;
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

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
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

            public void Check(DataColumn Column)
            {
              


            //    try
            //    {

                    if (tablePreferences.OtherCols.Contains(Column))
                    {


                        if (IsShowSolangNull()) ShowSolang = false;
                        if (IsAutoLoadNull()) AutoLoad = true;
                        //still experimental? almost there
                     

                    }
                    else if (Column == this.tablePreferences.RunInBackgroundColumn)
                {
                    if (IsRunInBackgroundNull()) RunInBackground = false;
                    this.tablePreferences.RunInBackground?.Invoke(null, EventArgs.Empty);
                }
                    if (tablePreferences.FillCols.Contains(Column))
                    {
                        if (IsLastToDoNull()) LastToDo = string.Empty;
                        if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                        if (IsFillByHLNull()) FillByHL = true;
                        if (IsFillBySpectraNull()) FillBySpectra = true;
                        if (IsShowSampleDescriptionNull()) ShowSampleDescription = true;




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

                    }
                    else
                {
                    EC.CheckNull(Column, this);
                }

           //     }
            //    catch (Exception ex)
             //   {

                  
             //   }
             



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

            public new bool HasErrors()
            {
                return base.HasErrors;
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