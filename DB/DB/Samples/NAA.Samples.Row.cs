using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// CLEANED
        /// </summary>
        public partial class SubSamplesRow: IRow
        {
            public LINAA db { get


                {
                    return (this.Table.DataSet as LINAA);
                }

            }

            public void Check()
            {
                foreach (DataColumn column  in this.tableSubSamples.Columns)
                {
                    Check(column);
                }
             //   return this.GetColumnsInError().Count() != 0;
            }
            public void Check(DataColumn column)
            {

                if (this.tableSubSamples.SimpleNonNullable.Contains(column))
                {
                    EC.CheckNull(column, this);
                    return;
                }

                bool calMass, calRad, calFh, calDensity;
                //TODO: Mejorar esto
                SSFPrefRow pref = db.SSFPref.FirstOrDefault();
                calMass = pref.CalcMass;
                calRad = pref.AARadius;
                calDensity = pref.CalcDensity;
                calFh = pref.AAFillHeight;

                if (column == this.tableSubSamples.DirectSolcoiColumn)
                {
                    //not used aymore??
                    if (IsDirectSolcoiNull()) DirectSolcoi = true;
                }
                else if (this.tableSubSamples.GeometryNameColumn == column)
                {
                    if (CheckGeometry())
                    {
                        //take from geo not vial (first argument)
                        SetRadiusFillHeight(false);
                    }
                }
                else if (column == this.tableSubSamples.CapsulesIDColumn)
                {
                    if (CheckVialCapsule())
                    {
                        // bool rowModified = RowState != DataRowState.Added;
                        SetRadiusFillHeight(true);
                    }
                }
                else if (column == this.tableSubSamples.CalcDensityColumn)
                {
                    if (IsCalcDensityNull()) CalcDensity = 0;
                    if (calMass) GetGrossMass();
                    SetRadiusFillHeight(calRad, calFh);
                }
                else if (column == this.tableSubSamples.NetColumn)
                {
                    if (CheckMass())
                    {
                        if (calDensity) GetDensity(calDensity);
                        SetRadiusFillHeight(calRad, calFh);
                    }
                }
                else if (this.tableSubSamples.FillHeightColumn == column)
                {
                    if (calRad)
                    {
                        Radius = GetRadius();
                        return;
                    }
                    SetGeometryValues(calMass, calDensity);
                }
                else if (this.tableSubSamples.RadiusColumn == column)
                {
                    if (calFh)
                    {
                        FillHeight = GetFillHeight();
                        return;
                    }
                    SetGeometryValues(calMass, calDensity);
                }
                else if (this.tableSubSamples.MatrixIDColumn == column)
                {
                    if (SetLastMatrix())
                    {
                        GetDensity(true);
                    }
                }
                else if (column == this.tableSubSamples.Gross1Column)
                {
                    if (Gross2 != Gross1)
                    {
                        Gross2 = Gross1;
                        return;
                    }
                    SetGrossAndNet();
                }
                else if (column == this.tableSubSamples.Gross2Column)
                {
                    SetGrossAndNet();
                }
                else if (column == this.tableSubSamples.ChCapsuleIDColumn)
                {
                    SetRabbit();
                }
                else if (column == this.tableSubSamples.MonitorsIDColumn)
                {
                    SetNameFromMonitor();
                    SetDescriptionFromMonitor();
                    SetMassFromMonitor(false);
                    SetGeometryFromMonitor();

                    SetStandard();
                }
                else if (column == this.tableSubSamples.StandardsIDColumn)
                {
                    SetStandard();
                }
                else if (column == this.tableSubSamples.SubSampleTypeColumn)
                {
                    if (IsComparatorNull()) Comparator = false;
                }
                else if (column == this.tableSubSamples.IrradiationRequestsIDColumn)
                {
                    SetIrradiationCode();
                }
                else if (column == this.tableSubSamples.ENAAColumn)
                {
                    SetENAA();
                }
                else if (column == this.tableSubSamples.fColumn || column == this.tableSubSamples.AlphaColumn)
                {
                    Setf();
                    SetAlpha();
                }
                else
                {
                    bool enter = (column == this.tableSubSamples.InReactorColumn);
                    enter = enter || (column == this.tableSubSamples.OutReactorColumn);
                    enter = enter || column == this.tableSubSamples.IrradiationTotalTimeColumn;
                    if (enter)
                    {
                        SetIrradiationDateErrors();
                        double totalMins = GetTotalMinutes();
                        //total time
                        SetIrradiationTime(totalMins);
                    }
                }
            }

            public bool CheckGthermal()
            {
                DataColumn col = this.tableSubSamples.MatrixDensityColumn;
                string chk = "\nCheck the filling height, radius net mass or matrix composition\n";
                string rofrom = "The density from MatSSF";
                if (this.CalcDensity == 0)
                {
                    this.SetColumnError(col, "The density from MatSSF is NULL!" + chk);
                    return false;
                }
                bool seterror = false;
                string HighLow = string.Empty;
                int pent = 7;
                double ratio = (this.MatrixDensity / CalcDensity) * 100;
                if (ratio >= 100 + pent)
                {
                    HighLow = rofrom + " exceeds more than " + pent + "% the density shown" + chk;
                    seterror = true;
                }
                else if (ratio <= 100 - pent)
                {
                    HighLow = rofrom + " is " + pent + "% lower than the density shown" + chk;
                    seterror = true;
                }

                if (seterror)
                {
                    decimal ro = Decimal.Round(Convert.ToDecimal(CalcDensity), 2);
                    decimal r = Decimal.Round(Convert.ToDecimal(GetRadius()), 2);
                    decimal h = Decimal.Round(Convert.ToDecimal(GetFillHeight()), 2);
                    string estimateRo = "MatSSF estimated it as " + ro + " gr/cm3\n";
                    string estimateFH = "If the Fill Height and Net mass are correct\nthe Radius should be " + r + " mm\n";
                    string estimateR = "Or if the Radius and Net mass are correct\nthe Filling Height should be " + h + " mm\n";
                    this.SetColumnError(col, HighLow + estimateRo + estimateFH + estimateR);
                }

                return seterror;
            }

            public  bool HasBasicErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                return colsInE.Intersect(this.tableSubSamples.NonNullableUnit).Count() != 0;
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                Type t = typeof(T);
                if (EC.IsNuDelDetch(rowParent as DataRow)) return;
                if (t.Equals(typeof(MonitorsRow)))
                {
                    MonitorsRow = rowParent as MonitorsRow;
                }
                               
                else if (t.Equals(typeof(IrradiationRequestsRow)))
                {
                    IrradiationRequestsRow ir = rowParent as IrradiationRequestsRow;
                    IrradiationRequestsRow = ir;
                }
                else throw new NotImplementedException();
            }

           
        }

       /// <summary>
       /// NOT CLEANED
       /// </summary>
        public partial class SubSamplesRow  
        {
          
            public bool NeedsMeasurements
            {
                get
                {
                    return (this.GetMeasurementsRows().Count() == 0);
                }
            }

            public bool NeedsPeaks
            {
                get
                {
                    return (this.GetIRequestsAveragesRows().Count() == 0);
                }
            }

            public bool NeedsSSF
            {
                get
                {
                    return (this.UnitRow.GetMatSSFRows().Count() == 0);
                }
            }

            public bool Selected
            {
                get
                {
                    return selected;
                }
                set { selected = value; }
            }

            public object Tag
            {
                get { return tag; }
                set { tag = value; }
            }

            protected internal bool selected;

            protected internal object tag;
          

            public bool SetOverride(string alpha, string efe, string Geo, string Gt, bool asSamples)
            {
                bool success = false;
                try
                {
                    if (asSamples) this.Comparator = false;
                    if (!efe.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.f = Convert.ToDouble(efe);
                    }
                    if (!alpha.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.Alpha = Convert.ToDouble(alpha);
                    }
                    if (!Gt.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.Gthermal = Convert.ToDouble(Gt);
                    }
                    if (!Geo.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.GeometryName = Geo;
                    }
                    success = true;
                }
                catch (System.InvalidCastException)
                {
                }

                return success;
            }

            public void SetDetectorPosition(string det, string pos)
            {
                try
                {
                    SetColumnError(this.tableSubSamples.DetectorColumn, null);
                    Detector = det;
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableSubSamples.DetectorColumn, ex.Message);
                }

                try
                {
                    SetColumnError(this.tableSubSamples.PositionColumn, null);

                    Position = Convert.ToInt16(pos);
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableSubSamples.PositionColumn, ex.Message);
                }
            }

            public bool ShouldSelectIt(bool CheckForSSF)
            {
                if (this.NeedsMeasurements) return false;
                IEnumerable<LINAA.MeasurementsRow> measurements = GetMeasurementsRows();
                if (base.HasErrors)
                {
                    foreach (LINAA.MeasurementsRow m in measurements) m.Selected = false;
                    return false;
                }
                else
                {
                    foreach (LINAA.MeasurementsRow m in measurements) m.Selected = m.ShouldSelectIt();
                    measurements = LINAA.FindSelected(measurements).ToList();
                    if (measurements.Count() != 0) return true;
                    else
                    {
                        if (CheckForSSF)
                        {
                            if (NeedsSSF) return true;
                            else return false;
                        }
                        else return false;
                    }
                }
            }

        }
   

     
    }
}