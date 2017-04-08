﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class SubSamplesDataTable
        {
            /// <summary>
            /// Finds the SampleRow with given sample name, or adds it if not found and specifically requested, using the IrrReqID given
            /// </summary>
            /// <param name="sampleName">name of sample to find</param>
            /// <param name="AddifNull">true for adding the row if not found</param>
            /// <param name="IrrReqID">irradiation request id to set for the sample only if added</param>
            /// <returns>A non-null SampleRow if AddIfNull is true, otherwise can be null</returns>
            public SubSamplesRow FindBySample(string sampleName, bool AddifNull, int? IrrReqID)
            {
                SubSamplesRow sample = this.FindBySample(sampleName);
                if (sample == null)
                {
                    sample = this.NewSubSamplesRow();
                    if (IrrReqID != null) sample.IrradiationRequestsID = (int)IrrReqID;
                    sample.SubSampleName = sampleName;
                    sample.SubSampleCreationDate = DateTime.Now;
                    this.AddSubSamplesRow(sample);
                }
                return sample;
            }

            /// <summary>
            ///  Finds the SampleRow with given sample name
            /// </summary>
            /// <param name="sampleName">name of sample to find</param>
            /// <returns>A SampleRow or null</returns>
            public SubSamplesRow FindBySample(string sampleName)
            {
                string field = this.SubSampleNameColumn.ColumnName;
                string fieldVal = sampleName.Trim().ToUpper();
                return this.FirstOrDefault(LINAA.SelectorByField<SubSamplesRow>(fieldVal, field));
            }

            public IEnumerable<SubSamplesRow> FindByIrReqID(int? IrReqID)
            {
                IEnumerable<SubSamplesRow> old = null;
                string IrReqField = this.IrradiationRequestsIDColumn.ColumnName;
                old = this.Where(LINAA.SelectorByField<SubSamplesRow>(IrReqID, IrReqField));
                return old.ToList();
            }

            public IList<SubSamplesRow> FindByProject(string project)
            {
                IList<SubSamplesRow> old = null;
                string cd = DB.Properties.Misc.Cd;
                string IrReqField = this.IrradiationCodeColumn.ColumnName;
                project = project.Replace(cd, null);
                old = this.Where(LINAA.SelectorByField<SubSamplesRow>(project, IrReqField)).ToList();
                IEnumerable<SubSamplesRow> oldCD = this.Where(LINAA.SelectorByField<SubSamplesRow>(project + cd, IrReqField));

                old = old.Union(oldCD).ToList();

                return old;
            }

            public bool calFh
            {
                get
                {
                    return (this.DataSet as LINAA).currentPref.AAFillHeight;
                }
            }

            public bool calRad
            {
                get
                {
                    return (this.DataSet as LINAA).currentPref.AARadius;
                }
            }

            public bool calDensity
            {
                get
                {
                    return (this.DataSet as LINAA).currentSSFPref.CalcDensity;
                }
            }

            private DataColumn[] nonNullable;
            /*
            //  private DataColumn[] geometric;
            private DataColumn[] masses;

            public DataColumn[] Masses
            {
                get
                {
                    if (masses == null)
                    {
                        masses = new DataColumn[] {
                     columnGross1,columnGross2 ,columnGrossAvg };
                    }

                    return masses;
                }
            }
            */

            public DataColumn[] NonNullable
            {
                get
                {
                    if (nonNullable == null)
                    {
                        nonNullable = new DataColumn[]{columnSubSampleName,
                     columnSubSampleCreationDate,columnSubSampleDescription,
                     columnConcentration, columnCapsuleName, columnMatrixName};
                    }

                    return nonNullable;
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    //e.Row.SetColumnError(e.Column, null);
                    LINAA.SubSamplesRow subs = e.Row as LINAA.SubSamplesRow; //cast

                    if (NonNullable.Contains(e.Column)) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.DirectSolcoiColumn)
                    {
                        //not used aymore??
                        if (subs.IsDirectSolcoiNull()) subs.DirectSolcoi = true;
                    }
                    else if (columnGeometryName == (e.Column))
                    {
                        if (subs.CheckGeometry())
                        {
                            //if the row was modified go ahead
                            //take from geo not vial (first argument)
                            //   bool rowModified = subs.RowState != DataRowState.Added;

                            subs.CheckFillRad(false);
                        }
                    }
                    else if (e.Column == this.CapsulesIDColumn)
                    {
                        if (subs.CheckVialCapsule())
                        {
                            //if the row was modified go ahead
                            //take from vial not geo (first argument)
                            //     bool rowModified = subs.RowState != DataRowState.Added;
                            subs.CheckFillRad(true);
                        }
                    }
                    else if (e.Column == this.CalcDensityColumn)
                    {
                        if (!calDensity) subs.CalculateMass();
                    }
                    else if (columnFillHeight == e.Column || columnRadius == e.Column)
                    {
                        subs.Vol = subs.FindVolumen();
                    }
                    else if (columnVol == e.Column)
                    {
                        if (calDensity) subs.CalculateDensity(true, false);
                        else subs.CalculateMass();
                    }
                    else if (columnMatrixID == (e.Column))
                    {
                        if (subs.CheckMatrix())
                        {
                            subs.CalculateDensity(true, true);
                        }
                    }
                    else if (e.Column == columnGross1)
                    {
                        if (subs.Gross2 != subs.Gross1)
                        {
                            subs.Gross2 = subs.Gross1;
                            return;
                        }
                        subs.GrossAvg = (subs.Gross1 + subs.Gross2) * 0.5;
                        subs.Net = subs.GrossAvg - subs.Tare;
                    }
                    else if (e.Column == columnGross2)
                    {
                        subs.GrossAvg = (subs.Gross1 + subs.Gross2) * 0.5;
                        subs.Net = subs.GrossAvg - subs.Tare;

                        //   EC.CheckNull(e.Column, e.Row);
                    }
                    else if (e.Column == columnNet)
                    {
                        //   subs.Net = subs.GrossAvg = subs.Tare;
                        if (subs.CheckMass())
                        {
                            //      subs.Net = subs.GrossAvg = subs.Tare;
                            subs.CalculateDensity(calDensity, false);
                        }
                    }
                    else if (e.Column == this.ChCapsuleIDColumn)
                    {
                        subs.CheckRabbit();
                    }
                    else if (e.Column == this.columnMonitorsID)
                    {
                        if (!subs.CheckMonitor()) return;

                        subs.CheckStandard();
                    }
                    else if (e.Column == this.columnStandardsID)
                    {
                        subs.CheckStandard();
                    }
                    else if (e.Column == this.columnSubSampleType)
                    {
                        if (subs.IsComparatorNull()) subs.Comparator = false;
                    }
                    else if (e.Column == this.columnIrradiationRequestsID)
                    {
                        if (EC.IsNuDelDetch(subs.IrradiationRequestsRow)) return;
                        if (subs.IsIrradiationCodeNull())
                        {
                            subs.IrradiationCode = subs.IrradiationRequestsRow.IrradiationCode;
                        }
                    }
                    else if (e.Column == this.ENAAColumn) subs.CheckENAA();
                    else
                    {
                        if (e.Column == this.columnf || e.Column == this.columnAlpha)
                        {
                            subs.CheckfOrAlpha();
                            return;
                        }
                        bool inOut = (e.Column == this.columnInReactor);
                        inOut = inOut || (e.Column == this.columnOutReactor);
                        inOut = inOut || e.Column == this.IrradiationTotalTimeColumn;
                        if (inOut)
                        {
                            subs.CheckTimes();
                        }
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
            }

            public bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples)
            {
                foreach (LINAA.SubSamplesRow row in this)
                {
                    row.Override(Alpha, f, Geo, Gt, asSamples);
                }
                return this.HasErrors;
            }
        }

        partial class SubSamplesRow
        {
            public UnitRow UnitRow
            {
                get
                {
                    return this.GetUnitRows().AsEnumerable().FirstOrDefault();
                }
            }

            public void CheckFillRad(bool takeFromVial)
            {
                UnitRow u = UnitRow;

                double rad, fh = 0;
                bool noradius = false;
                bool nofillheight = false;
                nofillheight = EC.CheckNull(this.tableSubSamples.FillHeightColumn, this);
                noradius = EC.CheckNull(this.tableSubSamples.RadiusColumn, this);

                if (!takeFromVial)
                {
                    fh = this.GeometryRow.FillHeight;

                    rad = this.GeometryRow.Radius;
                }
                else
                {
                    fh = this.VialTypeRow.MaxFillHeight;

                    rad = this.VialTypeRow.InnerRadius;
                }

                if ((nofillheight && fh != 0))
                {
                    this.FillHeight = fh;
                }
                if ((noradius && rad != 0))
                {
                    this.Radius = rad;
                }
            }

            public bool CheckGeometry()
            {
                DataColumn geoCol = this.tableSubSamples.GeometryNameColumn;
                this.SetColumnError(geoCol, null);
                if (EC.IsNuDelDetch(GeometryRow))
                {
                    this.SetColumnError(geoCol, "Please assign a valid geometry to this sample");
                    return false;
                }
                return true;
            }

            public void CalculateDensity(bool caldensity, bool forceContent)
            {
                UnitRow u = this.UnitRow;
                SetColumnError(tableSubSamples.MatrixDensityColumn, null);

                if (!EC.IsNuDelDetch(u))
                {
                    if (forceContent) u.Content = this.MatrixRow.MatrixComposition;
                    if (this.Net != 0 && Vol != 0 && caldensity)
                    {
                        this.CalcDensity = this.Net / Vol;
                        string HighLow = "The calculated density (in gr/cm3)";
                        double ratio = (this.MatrixDensity / this.CalcDensity) * 100;
                        int pent = 3;
                        bool seterror = false;
                        if (ratio >= 100 + pent)
                        {
                            HighLow += "exceeds more than " + pent + "% the Matrix density shown\n\n";
                            seterror = true;
                        }
                        else if (ratio <= 100 - pent)
                        {
                            HighLow += "is " + pent + "% lower than the Matrix density shown\n\n";
                            seterror = true;
                        }
                        if (seterror)
                        {
                            SetColumnError(tableSubSamples.MatrixDensityColumn, HighLow);
                        }
                    }
                    else if (caldensity) this.CalcDensity = this.MatrixRow.MatrixDensity;
                }
            }

            public void CalculateMass()
            {
                UnitRow u = this.UnitRow;

                if (!EC.IsNuDelDetch(u))
                {
                    if (this.CalcDensity != 0 && Vol != 0)
                    {
                        this.Gross1 = Vol * this.CalcDensity;
                        this.Gross2 = Vol * this.CalcDensity;
                    }
                }
            }

            public bool CheckMatrix()
            {
                this.SetColumnError(this.tableSubSamples.MatrixNameColumn, null);

                if (EC.IsNuDelDetch(this.MatrixRow))
                {
                    MatrixRow m = null;

                    if (!EC.IsNuDelDetch(this.GeometryRow))
                    {
                        m = this.GeometryRow.MatrixRow;
                    }
                    if (!EC.IsNuDelDetch(m))
                    {
                        this.MatrixID = m.MatrixID;
                        return true;
                    }
                    else
                    {
                        this.SetColumnError(this.tableSubSamples.GeometryNameColumn, "Plase assign a matrix for this geometry");
                        this.SetColumnError(this.tableSubSamples.MatrixNameColumn, "Plase assign a matrix either directly or through a geometry");

                        return false;
                    }
                }
                else return true;
            }

            public bool CheckVialCapsule()
            {
                this.SetColumnError(this.tableSubSamples.CapsuleNameColumn, null);
                VialTypeRow v = this.VialTypeRow;
                if (EC.IsNuDelDetch(v))
                {
                    this.SetColumnError(tableSubSamples.CapsuleNameColumn, "Assign a vial/capsule");
                    return false;
                }
                return true;
            }

            public void CheckRabbit()
            {
                SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, null);
                VialTypeRow v = VialTypeRowByChCapsule_SubSamples;
                if (EC.IsNuDelDetch(v))
                {
                    SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, "Assign an irradiation container");
                }
                else
                {
                    UnitRow u = UnitRow;
                    if (!EC.IsNuDelDetch(u))
                    {
                        u.SetRabbitContainer(ref v);
                    }
                }
            }

            public void CheckENAA()
            {
                if (IsENAANull()) ENAA = false;
                if (!EC.IsNuDelDetch(IrradiationRequestsRow))
                {
                    string code = IrradiationRequestsRow.IrradiationCode;
                    if (ENAA)
                    {
                        IrradiationCode = code + Properties.Misc.Cd;
                        f = 0.0;
                    }
                    else
                    {
                        IrradiationCode = code;
                        f = -1.0;
                    }
                }
            }

            public void CheckfOrAlpha()
            {
                if (EC.IsNuDelDetch(IrradiationRequestsRow)) return;
                if (EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow)) return;
                if (IsfNull() || f < 0)
                {
                    f = IrradiationRequestsRow.ChannelsRow.f;
                }

                if (IsAlphaNull())
                {
                    Alpha = IrradiationRequestsRow.ChannelsRow.Alpha;
                }
            }

            public bool CheckMass()
            {
                bool one = EC.CheckNull(this.tableSubSamples.Gross1Column, this);
                bool two = EC.CheckNull(this.tableSubSamples.Gross2Column, this);
                DataColumn col = this.tableSubSamples.GrossAvgColumn;
                SetColumnError(col, null);
                if (one || two)
                {
                    SetColumnError(col, "NULL. Assign the Net Mass (in miligrams) through Gross and Tare Mass");
                    return false;
                }

                double diff = Math.Abs(Gross1 - Gross2) * 100;
                double pent = 0.1;

                if ((diff / Gross1) >= pent)
                {
                    SetColumnError(col, "Difference between weights is higher than 0.1%\nPlease check");
                    return false;
                }
                else if ((diff / Gross2) >= pent)
                {
                    SetColumnError(col, "Difference between weights is higher than 0.1%\nPlease check");
                    return false;
                }
                return true;
            }

            private object tag;

            public object Tag
            {
                get { return tag; }
                set { tag = value; }
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
                if (HasErrors)
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

            public bool CheckMonitor()
            {
                if (EC.IsNuDelDetch(MonitorsRow)) return false;

                bool namenull = MonitorsRow.IsMonNameNull();
                if (!namenull)
                {
                    string _projectNr = Regex.Replace(IrradiationCode, "[a-z]", String.Empty, RegexOptions.IgnoreCase);
                    SubSampleName = _projectNr + MonitorsRow.MonName;
                    SubSampleDescription = MonitorsRow.MonName;
                }
                bool takeMonMass = false;

                DateTime lastweight = MonitorsRow.LastMassDate;

                int diffDates = Math.Abs((lastweight - SubSampleCreationDate).Days);
                if (diffDates >= 0) takeMonMass = diffDates <= 45;
                if (takeMonMass) SubSampleCreationDate = lastweight;
                if (Gross1 == 0 || takeMonMass)
                {
                    if (MonitorsRow.MonGrossMass1 != 0) Gross1 = Tare + MonitorsRow.MonGrossMass1;
                }
                if (Gross2 == 0 || takeMonMass)
                {
                    if (MonitorsRow.MonGrossMass2 != 0) Gross2 = Tare + MonitorsRow.MonGrossMass2;
                }
                if (GeometryRow == null && MonitorsRow.GeometryRow != null)
                {
                    GeometryRow = MonitorsRow.GeometryRow;
                }

                return true;
            }

            public bool CheckStandard()
            {
                // StandardsRow std = StandardsRow;
                if (EC.IsNuDelDetch(StandardsRow)) return false;

                Concentration = 1;
                Comparator = true;
                Elements = string.Empty;
                if (EC.IsNuDelDetch(StandardsRow.MatrixRow)) return false;
                if (EC.IsNuDelDetch(MatrixRow) || MatrixID != StandardsRow.MatrixRow.MatrixID)
                {
                    MatrixRow = StandardsRow.MatrixRow;
                }
                if (IsSubSampleDescriptionNull() && !StandardsRow.IsstdNameNull())
                {
                    SubSampleDescription = StandardsRow.stdName;
                }
                return true;
            }

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

            /// <summary>
            // MEJORARTE ESTO ESTA MAL USAR UNITS
            /// </summary>
            public bool NeedsSSF
            {
                get
                {
                    return (this.GetMatSSFRows().Count() == 0);
                }
            }

            public void CheckTimes()
            {
                bool innull = IsInReactorNull();
                bool tirnull = IsIrradiationTotalTimeNull();
                bool outnull = IsOutReactorNull();

                double aux2 = 0;
                if (!outnull && !innull) aux2 = (OutReactor - InReactor).TotalMinutes;

                //in
                DataColumn inCol = this.tableSubSamples.InReactorColumn;
                if (innull) SetColumnError(inCol, "Set an Irradiation Start date/time");
                else SetColumnError(inCol, null);

                DataColumn outCol = this.tableSubSamples.OutReactorColumn;
                //out
                if (outnull) SetColumnError(outCol, "Set an Irradiation End date/time");
                else SetColumnError(outCol, null);

                //total time
                DataColumn tCol = this.tableSubSamples.IrradiationTotalTimeColumn;

                if (tirnull && aux2 != 0)
                {
                    IrradiationTotalTime = aux2;
                    return;
                }
                else if (tirnull && aux2 == 0)
                {
                    SetColumnError(tCol, "Set an Irradiation Time (in minutes) or an Irradiation Start and End date/times");
                }
                else
                {
                    SetColumnError(tCol, null);
                    if (IrradiationTotalTime < 0)
                    {
                        SetColumnError(tCol, "Irradiation Time was found tow be negative\nThe Irradiation End date/time is less than the Start date/time!");
                    }
                    else if (IrradiationTotalTime == 0)
                    {
                        SetColumnError(tCol, "Irradiation Time is 0 min\nThe Irradiation End date/time is equal to the Start date/time!");
                    }
                    else
                    {
                        if (!innull)
                        {
                            DateTime aux = InReactor.AddSeconds(IrradiationTotalTime * 60);
                            if (outnull || aux != OutReactor) OutReactor = aux;
                            return;
                        }
                        else if (!outnull)
                        {
                            DateTime aux = OutReactor.AddSeconds(-1 * IrradiationTotalTime * 60);
                            InReactor = aux;
                            return;
                        }
                    }
                }
            }

            /*
            public bool CalculateDensityOld()
            {
                UnitRow u = this.UnitRow;

                if (!EC.IsNuDelDetch(u))
                {
                    //find density
                    double ro = 0;

                    decimal density = Decimal.Round(Convert.ToDecimal(ro), 3);
                    string estimateRo = "The calculated density = " + density;

                    DataColumn matDenCol = this.tableSubSamples.MatrixDensityColumn;
                    if (nomatrixdensity)
                    {
                        this.SetColumnError(matDenCol, "There is no default density value assigned to this matrix, a comparison is not possible.\n" + estimateRo + " might not be the correct one\nPlease verify this");
                        return false;
                    }
                }
                string HighLow;
                bool seterror = checkDensityValues(ro, out HighLow);

                if (!seterror) return !nomatrixdensity;

                double rad, fh = 0;
                rad = this.FindRadius();
                fh = this.FindFillingHeight();

                if (aaRad && rad != 0)
                {
                    this.Radius = rad;
                    //  if (!EC.IsNuDelDetch(u)) u.Diameter = 2 * rad;
                    return true;
                }
                else if (aaFh && fh != 0)
                {
                    this.FillHeight = fh;
                    //  if (!EC.IsNuDelDetch(u)) u.Length = fh;
                    return true;
                }

                if (aaFh == false && aaRad == false) return false;

                decimal radius = Decimal.Round(Convert.ToDecimal(rad), 3);
                decimal fil = Decimal.Round(Convert.ToDecimal(fh), 3);
                string estimateR = "If the Radius and Net mass are correct\nthe Filling Height should be " + fil + " mm\n";
                string estimateFH = "Or if the Fill Height and Net mass are correct\nthe Radius should be " + radius + " mm\n";
                this.SetColumnError(matDenCol, estimateRo + HighLow + estimateFH + estimateR);
                return true;
            }
            */

            /// <summary>
            /////////////////////////////////////////////////////////////////
            /// </summary>
            /// <returns></returns>

            public double FindVolumen()
            {
                return FindSurface() * this.FillHeight * 0.1;
            }

            public double FindSurface()
            {
                return Math.PI * this.Radius * this.Radius * 0.1 * 0.1;
            }

            public double FindRadius()
            {
                UnitRow u = GetUnitRows().AsEnumerable().FirstOrDefault();
                double ro = this.MatrixDensity;
                if (u != null) ro = this.CalcDensity;
                return 10 * Math.Sqrt((this.DryMass() * 1e-3 / (Math.PI * ro * this.FillHeight * 0.1)));
            }

            public double FindFillingHeight()
            {
                return (10 * this.DryMass() * 1e-3 / (this.MatrixDensity * FindSurface()));
            }

            /// <summary>
            /// MoistureContent Content must be in Percentage
            /// </summary>
            /// <returns></returns>
            public double DryMass()
            {
                return (this.Net - (this.Net * this.MoistureContent * 1e-2));  // netto dried mass in miligrams;
            }

            //////////////////////////////////////////////////////////////

            /// <summary>
            ///
            /// </summary>
            /// <param name="alpha"></param>
            /// <param name="efe"></param>
            /// <param name="Geo"></param>
            /// <param name="Gt"></param>
            /// <param name="asSamples"></param>
            /// <returns></returns>

            public bool Override(string alpha, string efe, string Geo, string Gt, bool asSamples)
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

            public bool CheckGthermal()
            {
                DataColumn col = this.tableSubSamples.MatrixDensityColumn;
                string chk = "\nCheck the filling height, radius net mass or matrix composition\n";
                string rofrom = "The density from MatSSF";
                if (MatSSFDensity == 0)
                {
                    this.SetColumnError(col, "The density from MatSSF is NULL!" + chk);
                    return false;
                }
                bool seterror = false;
                string HighLow = string.Empty;
                int pent = 7;
                double ratio = (this.MatrixDensity / MatSSFDensity) * 100;
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
                    decimal ro = Decimal.Round(Convert.ToDecimal(MatSSFDensity), 2);
                    decimal r = Decimal.Round(Convert.ToDecimal(FindRadius()), 2);
                    decimal h = Decimal.Round(Convert.ToDecimal(FindFillingHeight()), 2);
                    string estimateRo = "MatSSF estimated it as " + ro + " gr/cm3\n";
                    string estimateFH = "If the Fill Height and Net mass are correct\nthe Radius should be " + r + " mm\n";
                    string estimateR = "Or if the Radius and Net mass are correct\nthe Filling Height should be " + h + " mm\n";
                    this.SetColumnError(col, HighLow + estimateRo + estimateFH + estimateR);
                }

                return seterror;
            }

            private double matSSFDensity = 0;

            public double MatSSFDensity
            {
                get { return matSSFDensity; }
                set { matSSFDensity = value; }
            }

            private bool selected;

            public bool Selected
            {
                get
                {
                    return selected;
                }
                set { selected = value; }
            }
        }
    }
}