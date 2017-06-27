using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        public partial class SSFPrefDataTable : IColumn
        {
            // private EventHandler checkChanged2;

            private DataColumn[] autoCal = null;

            private DataColumn[] doCols = null;

            private DataColumn[] otherCols = null;

            public IEnumerable<DataColumn> AutoCal
            {
                get
                {
                    if (autoCal == null)
                    {
                        autoCal = new DataColumn[] { this.CalcDensityColumn,
                          this.AAFillHeightColumn,
                            this.AARadiusColumn  };
                    }
                    return autoCal;
                }
            }

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

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

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
            protected internal bool isBusy = false;

            public void Check()
            {
                // DoMatSSF = true; DoCK = false;

                foreach (DataColumn item in this.tableSSFPref.Columns)
                {
                    Check(item);
                }
            }

            public void Check(DataColumn column)
            {
                //TODO PORQUE FALTABA ESTA MIERDAAAAAA???
                //ES QUE JAMAS VOY A ENTENDER POR QUE NO VALIDA
                //HASTA QUE ESTA MIERDA ESTE AQUI
                //ES UNA PUTADAAAAAAAAAAAAAA

                //esta mierda coloca los booleanos en falso por defecto
                //dejar así

                ////////////////////////////

                if (this.tableSSFPref.AutoCal.Contains(column))
                {
                    if (isBusy) return;
                    checkAutoCalculation(column);
                }
                else if (column == this.tableSSFPref.CalcMassColumn)
                {
                    bool nulo = EC.CheckNull(column, this);
                    isBusy = true;
                    if (CalcMass)
                    {
                        AAFillHeight = false;
                        AARadius = false;
                        CalcDensity = false;
                    }
                    else
                    {
                        if (!CalcDensity && !AAFillHeight && !AARadius)
                        {
                            CalcDensity = true;
                        }
                    }
                    isBusy = false;
                }
                else if (tableSSFPref.DoCols.Contains(column))
                {
                    checkDoColumns(column);
                }
                else if (tableSSFPref.OtherCols.Contains(column))
                {
                    checkOtherColumns(column);
                }
                else //if (column == this.tableSSFPref.OverridesColumn)
                {
                }

                bool nulow = EC.CheckNull(column, this);
            }

            public new bool HasErrors()
            {
                // throw new NotImplementedException();
                return base.HasErrors;
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }

            // bool isBusy = false;
            /*
            private void checkAutoCalculation(DataColumn column)
            {
                // if (isBusy) return;

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

                // isBusy = true;
                if (AAFillHeight)
                {
                    // CalcMass = false;

                    if (CalcDensity) CalcDensity = false;

                    if (AARadius) AARadius = false;

                    // AARadius = false; CalcDensity = false; CalcMass = false;
                }
                else if (AARadius)
                {
                  // CalcMass = false;
                    if (AAFillHeight) AAFillHeight = false;

                    if (CalcDensity) CalcDensity = false;

                    // AAFillHeight = false; CalcDensity = false; CalcMass = false;
                }
                else  if (CalcDensity)
                {
                    // CalcMass = false;
                    if (AAFillHeight) AAFillHeight = false;
                    if (AARadius) AARadius = false;
                }
                else if (!AAFillHeight && !CalcDensity && !AARadius)
                {
                    CalcMass = true;
                }
                else if (!AARadius && !CalcDensity)
                {
                    CalcMass = true;
                }
                else if (!AARadius && !AAFillHeight)
                {
                    CalcMass = true;
                }
                // isBusy = false;

                // this.tableSSFPref.SampleChanged?.Invoke(null, EventArgs.Empty);
            }

            */

            private void checkAutoCalculation(DataColumn column)
            {
                // if (isBusy) return;

                if (IsAAFillHeightNull())
                {
                    AAFillHeight = false;
                    // return;
                }
                if (IsAARadiusNull())
                {
                    AARadius = false;
                    // return;
                }
                if (IsCalcDensityNull())
                {
                    CalcDensity = true;
                    // return;
                }

                if (column == this.tableSSFPref.AAFillHeightColumn)
                {
                    // isBusy = true;
                    if (AAFillHeight)
                    {
                        CalcMass = false;
                        AARadius = false;
                        CalcDensity = false;

                        // AARadius = false; CalcDensity = false; CalcMass = false;
                    }
                    else if (!AARadius && !CalcDensity)
                    {
                        CalcMass = true;
                    }
                    // isBusy = false;
                }
                else if (column == this.tableSSFPref.AARadiusColumn)
                {
                    // isBusy = true;

                    if (AARadius)
                    {
                        CalcMass = false;
                        AAFillHeight = false;
                        CalcDensity = false;

                        // AAFillHeight = false; CalcDensity = false; CalcMass = false;
                    }
                    else if (!AAFillHeight && !CalcDensity)
                    {
                        CalcMass = true;
                    }
                    // isBusy = false;
                }
                else if (column == this.tableSSFPref.CalcDensityColumn)
                {
                    // isBusy = true;
                    if (CalcDensity)
                    {
                        CalcMass = false;
                        AAFillHeight = false;
                        AARadius = false;
                    }
                    else if (!AARadius && !AAFillHeight)
                    {
                        CalcMass = true;
                    }
                    // isBusy = false;
                }

                // this.tableSSFPref.SampleChanged?.Invoke(null, EventArgs.Empty);
            }

            private void checkDoColumns(DataColumn column)
            {
                // bool nulo = EC.CheckNull(column, this);

                if (column == this.tableSSFPref.DoCKColumn)
                {
                    if (IsDoCKNull()) DoCK = true;
                    // this.tableSSFPref.DoChilianChanged?.Invoke(null, EventArgs.Empty);
                }
                else if (column == this.tableSSFPref.DoMatSSFColumn)
                {
                    if (IsDoMatSSFNull()) DoMatSSF = true;
                    // this.tableSSFPref.DoMatSSFChanged?.Invoke(null, EventArgs.Empty);
                }
                else if (column == this.tableSSFPref.LoopColumn)
                {
                    if (IsLoopNull()) Loop = true;
                }
            }

            private void checkOtherColumns(DataColumn column)
            {
                bool nulo = EC.CheckNull(column, this);
                ///EC automatically sets stuff to null
                if (column == this.tableSSFPref.RoundingColumn)
                {
                    if (nulo) Rounding = "N3";
                }
                else if (column == this.tableSSFPref.CalibrationColumn)
                {
                    /// if (nulo) Calibration = false;
                }
                else if (column == this.tableSSFPref.ShowOtherColumn)
                {
                    /// if (nulo) ShowOther = false;
                }
                else if (column == this.tableSSFPref.ShowMatSSFColumn)
                {
                    /// if (nulo) ShowMatSSF = false;
                }
            }
        }
    }
}