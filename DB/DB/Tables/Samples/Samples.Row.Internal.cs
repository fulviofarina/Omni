using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// CLEANED
        /// </summary>
        public partial class SubSamplesRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableSubSamples.Columns)
                {
                    Check(column);
                }
            }

            public void Check(DataColumn column)
            {
                if (this.tableSubSamples.SimpleNonNullable.Contains(column))
                {
                    EC.CheckNull(column, this);
                }
                else
                if (this.tableSubSamples.GeometriesNonNullables.Contains(column))
                {
                    checkGeometryColumns(column);
                }
                else if (this.tableSubSamples.StandardsNonNullables.Contains(column))
                {
                    checkStandardsColumns(column);
                }
                else if (this.tableSubSamples.NonNullableUnit.Contains(column))
                {
                    checkUnitsColumns(column);
                }
                else if (this.tableSubSamples.IrradiationNonNullable.Contains(column))
                {
                    checkIrradiationsColumns(column);
                }
                else if (this.tableSubSamples.TimesNonNullable.Contains(column))
                {
                    SetIrradiationDateErrors();
                    double totalMins = GetTotalMinutes();
                    //total time
                    SetIrradiationTime(totalMins);
                }
            }

            private void checkStandardsColumns(DataColumn column)
            {
                if (column == this.tableSubSamples.MonitorsIDColumn)
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
            }

            private void checkGeometryColumns(DataColumn column)
            {
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
            }

         //   protected internal EventData ebento = new EventData();

            private void checkUnitsColumns(DataColumn column)
            {
                bool calMass, calRad, calFh, calculateDensity;
                //TODO: Mejorar esto
                EventData ebento = new EventData();
                // ebento = null;
                this.tableSubSamples.CalcParametersHandler?.Invoke(this, ebento);
                // SSFPrefRow pref = db.SSFPref.FirstOrDefault();
                calMass = (bool)ebento.Args[0];// pref.CalcMass;
                calRad = (bool)ebento.Args[1]; //  pref.AARadius;
                calculateDensity = (bool)ebento.Args[2];// pref.CalcDensity;
                calFh = (bool)ebento.Args[3];// pref.AAFillHeight;

                ebento = null;

                EC.CheckNull(column, this);
                if (column == this.tableSubSamples.CalcDensityColumn)
                {
                    if (IsCalcDensityNull()) CalcDensity = 0;
                    if (calMass) GetGrossMass();
                    SetRadiusFillHeight(calRad, calFh);
                }
                else if (column == this.tableSubSamples.NetColumn)
                {
                    if (CheckMass())
                    {
                        if (calculateDensity) GetDensity(calculateDensity);
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
                    SetGeometryValues(calMass, calculateDensity);
                }
                else if (this.tableSubSamples.RadiusColumn == column)
                {
                    if (calFh)
                    {
                        FillHeight = GetFillHeight();
                        return;
                    }
                    SetGeometryValues(calMass, calculateDensity);
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
                else if (this.tableSubSamples.MatrixIDColumn == column)
                {
                    if (SetLastMatrix())
                    {
                        GetDensity(true);
                    }
                }
            }

            private void checkIrradiationsColumns(DataColumn column)
            {
                if (column == this.tableSubSamples.ChCapsuleIDColumn)
                {
                    SetRabbit();
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

            public new bool HasErrors()
            {
                int count = GetBasicColumnsInErrorNames().Count();
                return (count != 0);
            }

            public IEnumerable<string> GetBasicColumnsInErrorNames()
            {
                IEnumerable<DataColumn> colsInE = this.GetColumnsInError();
                // colsInE = colsInE.Intersect(this.tableSubSamples.NonNullableUnit);
                colsInE = colsInE.Intersect(this.tableSubSamples.NonNullBasicUnits);
                return colsInE.Select(o => o.ColumnName);
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
        /// Cleaned
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

            protected internal double perCentDiff = 0.1;

            protected internal string _projectNr
            {
                get
                {
                    // this.SubSampleName;
                    return Regex.Replace(this.IrradiationCode, "[a-z]", String.Empty, RegexOptions.IgnoreCase);
                }
            }

            protected static string ASSIGN_MATRIX = "Plase assign a matrix either directly or through a geometry";
            protected static string THE_CALC_DENSITY = "The calculated density (in gr/cm3)";
            protected static string ASSIGN_IRR_START = "Set an Irradiation Start date/time";
            protected static string ASSIGN_IRR_END = "Set an Irradiation End date/time";
            protected static string IRR_TIME_NEGATIVE = "Irradiation Time was found tow be negative\nThe Irradiation End date/time is less than the Start date/time!";
            protected static string IRR_TIME_ISNULL = "Irradiation Time is 0 min\nThe Irradiation End date/time is equal to the Start date/time";

            protected static string ASSIGN_VIAL = "Assign a vial/capsule";

            protected static string MASS_DIFFERENCE_HIGH = "Difference between weights is higher than 0.1%\nPlease check";

            protected static string ASSIGN_NET = "NULL. Assign the Net Mass (in miligrams) through Gross and Tare Mass";

            protected static string ASSIGN_GEOMETRY = "Please assign a valid geometry to this sample";

            protected static string SET_IRR_TIMES = "Set an Irradiation Time (in minutes) or an Irradiation Start and End date/times";
            protected static string ASSIGN_RABBIT = "Assign an irradiation container";

            public bool CheckGeometry()
            {
                DataColumn geoCol = this.tableSubSamples.GeometryNameColumn;
                this.SetColumnError(geoCol, null);
                if (EC.IsNuDelDetch(GeometryRow))
                {
                    this.SetColumnError(geoCol, ASSIGN_GEOMETRY);
                    return false;
                }
                return true;
            }

            public bool CheckMass()
            {
                bool one = EC.CheckNull(this.tableSubSamples.Gross1Column, this);
                bool two = EC.CheckNull(this.tableSubSamples.Gross2Column, this);
                DataColumn col = this.tableSubSamples.GrossAvgColumn;
                SetColumnError(col, null);
                if (one || two)
                {
                    SetColumnError(col, ASSIGN_NET);
                    return false;
                }

                double diff = Math.Abs(Gross1 - Gross2) * 100;
                bool ok = false;
                if ((diff / Gross1) >= perCentDiff)
                {
                    SetColumnError(col, MASS_DIFFERENCE_HIGH);
                }
                else if ((diff / Gross2) >= perCentDiff)
                {
                    SetColumnError(col, MASS_DIFFERENCE_HIGH);
                }
                else ok = true;
                return ok;
            }

            public bool CheckVialCapsule()
            {
                this.SetColumnError(this.tableSubSamples.CapsuleNameColumn, null);
                VialTypeRow v = this.VialTypeRow;
                if (EC.IsNuDelDetch(v))
                {
                    this.SetColumnError(tableSubSamples.CapsuleNameColumn, ASSIGN_VIAL);
                    return false;
                }
                return true;
            }

            public void GetDensity(bool caldensity)
            {
                SetColumnError(tableSubSamples.MatrixDensityColumn, null);

                bool matrixDensNull = this.MatrixRow == null;
                matrixDensNull = matrixDensNull || IsMatrixDensityNull() || MatrixDensity == 0;

                if (this.Net != 0 && Vol != 0 && caldensity)
                {
                    this.CalcDensity = (this.Net / Vol) * 1e-3;

                    if (matrixDensNull) return;

                    string HighLow = THE_CALC_DENSITY;
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
                else if (caldensity && !matrixDensNull)
                {
                    this.CalcDensity = this.MatrixRow.MatrixDensity;
                }
            }

            public string GetDescriptionFromMonitor()
            {
                string description = string.Empty;
                if (EC.IsNuDelDetch(MonitorsRow)) return description;
                bool namenull = MonitorsRow.IsMonNameNull();
                if (!namenull) description = MonitorsRow.MonName;
                return description;
            }

            public string GetDescriptionFromStandard()
            {
                string description = string.Empty;
                if (!StandardsRow.IsstdNameNull())
                {
                    description = StandardsRow.stdName;
                }
                return description;
            }

            /// <summary>
            /// MoistureContent Content must be in Percentage
            /// </summary>
            /// <returns></returns>
            public double GetDryMass()
            {
                return (this.Net - (this.Net * this.MoistureContent * 1e-2));  // netto dried mass in miligrams;
            }

            public double GetFillHeight()
            {
                if (IsCalcDensityNull()) return 0;

                double deno = this.CalcDensity * GetSurface();
                double result = 0;

                if (deno != 0)
                {
                    result = 10 * this.Net * 1e-3 / deno;
                }
                return result;
            }

            public double GetGrossMass()
            {
                // UnitRow u = this.UnitRow;
                double mass = Vol * this.CalcDensity * 1e3;
                // if (!EC.IsNuDelDetch(u)) {
                if (this.CalcDensity != 0 && Vol != 0)
                {
                    this.Gross1 = mass;
                    this.Gross2 = mass;
                }
                // }
                return mass;
            }

            public string GetIrradiationCode()
            {
                if (EC.IsNuDelDetch(IrradiationRequestsRow)) return string.Empty;

                if (IrradiationRequestsRow.IsIrradiationCodeNull()) return string.Empty;
                return IrradiationRequestsRow.IrradiationCode;
            }

            public MatrixRow GetMatrixByMatrixID(int templateID)
            {
                //find in the list of childs Rows
                return GetMatrixRows()
         .FirstOrDefault(o => o.MatrixID == templateID);
            }

            public MatrixRow GetMatrixByTemplateID(int templateID)
            {
                //find child from template
                return GetMatrixRows()
         .FirstOrDefault(o => !o.IsTemplateIDNull() && o.TemplateID == templateID);
            }

            public string GetMonitorNameFromSampleName()
            {
                string newName = string.Empty;
                if (string.IsNullOrEmpty(SubSampleName)) return newName;
                newName = SubSampleName.Trim().ToUpper();
                //     string _projectNr = Regex.Replace(IrradiationCode, "[a-z]", String.Empty, RegexOptions.IgnoreCase);
                //take default vlues
                string code = string.Empty;
                if (!string.IsNullOrEmpty(_projectNr)) code = IrradiationCode.Replace(_projectNr, null);
                if (!EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow))
                {
                    code = IrradiationRequestsRow.ChannelsRow.IrReqCode.Trim().ToUpper();
                }
                string projectNoCode = string.Empty;
                if (!string.IsNullOrEmpty(code)) projectNoCode = IrradiationCode.Replace(code, null);
                if (!string.IsNullOrEmpty(projectNoCode)) newName = newName.Replace(projectNoCode, null);
                return newName;
            }

            public string GetName(int _lastSampleNr)
            {
                string name = string.Empty;
                string zero = "0";
                if (_lastSampleNr >= 10) name = _projectNr + _lastSampleNr.ToString();
                else name = _projectNr + zero + _lastSampleNr.ToString();
                return name;
            }

            public string GetNameFromMonitor()
            {
                string monName = string.Empty;
                if (EC.IsNuDelDetch(MonitorsRow)) return monName;
                bool namenull = MonitorsRow.IsMonNameNull();
                if (!namenull)
                {
                    monName = _projectNr + MonitorsRow.MonName;
                }
                return monName;
            }

            public double GetRadius()
            {
                if (IsCalcDensityNull()) return 0;

                double deno = (Math.PI * this.CalcDensity * this.FillHeight * 0.1);
                double result = 0;

                if (deno != 0)
                {
                    result = 10 * Math.Sqrt(this.Net * 1e-3 / deno);
                }
                return result;
            }

            public double GetSurface()
            {
                double result = Math.PI * this.Radius * this.Radius * 0.1 * 0.1; //indexer cm
                return result;
            }

            public double GetTotalMinutes()
            {
                double aux2 = 0;
                if (!IsOutReactorNull() && !IsInReactorNull()) aux2 = (OutReactor - InReactor).TotalMinutes;

                return aux2;
            }

            public double GetVolume()
            {
                return GetSurface() * this.FillHeight * 0.1;
            }

            internal bool SetAlpha()
            {
                if (EC.IsNuDelDetch(IrradiationRequestsRow)) return false;
                if (EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow)) return false;

                if (IsAlphaNull() || Alpha == 0)
                {
                    Alpha = IrradiationRequestsRow.ChannelsRow.Alpha;
                    return true;
                }
                return false;
            }

            public void SetCreationDate()
            {
                SubSampleCreationDate = DateTime.Now;
            }

            public bool SetDescriptionFromMonitor()
            {
                string description = GetDescriptionFromMonitor();
                if (!string.IsNullOrEmpty(description))
                {
                    SubSampleDescription = description;
                }
                return !string.IsNullOrEmpty(SubSampleDescription);
            }

            internal bool SetDescriptionFromStandard()
            {
                bool ok = false;
                if (EC.IsNuDelDetch(StandardsRow)) return ok;
                string description = GetDescriptionFromStandard();
                SubSampleDescription = description;
                return !string.IsNullOrEmpty(SubSampleDescription);
            }

            internal void SetENAA()
            {
                if (IsENAANull()) ENAA = false;
                if (!EC.IsNuDelDetch(IrradiationRequestsRow))
                {
                    string code = GetIrradiationCode();
                    if (ENAA)
                    {
                        code += Properties.Misc.Cd;
                        f = 0.0;
                    }
                    //restore
                    else f = -1;
                    IrradiationCode = code;
                }
            }

            internal bool Setf()
            {
                if (EC.IsNuDelDetch(IrradiationRequestsRow)) return false;
                if (EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow)) return false;
                if (IsfNull() || f < 0)
                {
                    f = IrradiationRequestsRow.ChannelsRow.f;
                    return true;
                }
                return false;
            }

            internal bool SetGeometryFromMonitor()
            {
                bool ok = false;
                if (EC.IsNuDelDetch(MonitorsRow)) return ok;
                ok = EC.IsNuDelDetch(GeometryRow);
                ok = ok && !EC.IsNuDelDetch(MonitorsRow.GeometryRow);
                if (ok)
                {
                    GeometryRow = MonitorsRow.GeometryRow;
                }
                return ok;
            }

            internal void SetGeometryValues(bool calMass, bool calDensity)
            {
                Vol = GetVolume();
                if (calDensity) GetDensity(true);
                else if (calMass) GetGrossMass();
            }

            internal void SetGrossAndNet()
            {
                GrossAvg = (Gross1 + Gross2) * 0.5;
                Net = GrossAvg - Tare;
            }

            internal bool SetIrradiationCode()
            {
                // if (EC.IsNuDelDetch(IrradiationRequestsRow)) return false;
                IrradiationCode = GetIrradiationCode();
                return !string.IsNullOrEmpty(IrradiationCode);
            }

            internal void SetIrradiationDateErrors()
            {
                bool innull = IsInReactorNull();

                bool outnull = IsOutReactorNull();

                //in
                DataColumn inCol = this.tableSubSamples.InReactorColumn;
                if (innull) SetColumnError(inCol, ASSIGN_IRR_START);
                else SetColumnError(inCol, null);

                DataColumn outCol = this.tableSubSamples.OutReactorColumn;
                //out
                if (outnull) SetColumnError(outCol, ASSIGN_IRR_END);
                else SetColumnError(outCol, null);
            }

            internal void SetIrradiationDates()
            {
                bool innull = IsInReactorNull();
                bool outnull = IsOutReactorNull();

                if (!innull)
                {
                    DateTime aux = InReactor.AddSeconds(IrradiationTotalTime * 60);
                    if (outnull || aux != OutReactor) OutReactor = aux;
                }
                else if (!outnull)
                {
                    DateTime aux = OutReactor.AddSeconds(-1 * IrradiationTotalTime * 60);
                    if (aux != InReactor) InReactor = aux;
                }
            }

            public void SetIrradiationTime(double totalMins)
            {
                DataColumn tCol = this.tableSubSamples.IrradiationTotalTimeColumn;

                bool tirnull = IsIrradiationTotalTimeNull();
                if (tirnull && totalMins != 0)
                {
                    IrradiationTotalTime = totalMins;
                    // return;
                }
                else if (tirnull && totalMins == 0)
                {
                    SetColumnError(tCol, SET_IRR_TIMES);
                }
                else
                {
                    SetColumnError(tCol, null);
                    if (IrradiationTotalTime < 0)
                    {
                        SetColumnError(tCol, IRR_TIME_NEGATIVE);
                    }
                    else if (IrradiationTotalTime == 0)
                    {
                        SetColumnError(tCol, IRR_TIME_ISNULL);
                    }
                    else SetIrradiationDates();
                }
            }

            public bool SetLastMatrix()
            {
                this.SetColumnError(this.tableSubSamples.MatrixNameColumn, null);
                this.SetColumnError(this.tableSubSamples.MatrixIDColumn, null);

                if (EC.IsNuDelDetch(this.MatrixRow)) SetMatrixFromGeometry();

                if (!EC.IsNuDelDetch(this.MatrixRow))
                {
                    checkIfCloneNeeded();
                }
                else
                {
                    this.SetColumnError(this.tableSubSamples.MatrixIDColumn, ASSIGN_MATRIX);
                    this.SetColumnError(this.tableSubSamples.MatrixNameColumn, ASSIGN_MATRIX);
                }

                return !EC.IsNuDelDetch(this.MatrixRow);
            }

            private void checkIfCloneNeeded()
            {
                //search template matrix
                int templateID = 0;
                bool templateNUll = this.MatrixRow.IsTemplateIDNull();
                if (templateNUll) templateID = this.MatrixRow.MatrixID;
                else templateID = this.MatrixRow.TemplateID;
                MatrixRow m = GetMatrixByMatrixID(templateID);
                //row is not on the list of childs, find in the Template list
                if (m == null) m = GetMatrixByTemplateID(templateID);

                //does not exist, so adopt (CLONE) the matrix to the list
                if (m == null)
                {
                    MatrixRow toClone = this.MatrixRow;
                    object[] args = new object[] { m, SubSamplesID, templateID, toClone, this };
                    EventData data = new EventData(args);
                    this.tableSubSamples.AddMatrixHandler?.Invoke(args, data);
                    m = data.Args[0] as MatrixRow;
                }
                if (IsMatrixIDNull() || MatrixID != m.MatrixID) MatrixID = m.MatrixID;
            }

            public bool SetMassFromMonitor(bool force = true)
            {
                if (EC.IsNuDelDetch(MonitorsRow)) return false;

                bool takeMonMass = false;
                DateTime lastweight = MonitorsRow.LastMassDate;

                int diffDates = Math.Abs((lastweight - SubSampleCreationDate).Days);
                if (diffDates >= 0) takeMonMass = diffDates <= 45;
                takeMonMass = takeMonMass || force;
                if (takeMonMass) SubSampleCreationDate = lastweight;
                if (Gross1 == 0 || takeMonMass)
                {
                    if (MonitorsRow.MonGrossMass1 != 0) Gross1 = Tare + MonitorsRow.MonGrossMass1;
                }
                if (Gross2 == 0 || takeMonMass)
                {
                    if (MonitorsRow.MonGrossMass2 != 0) Gross2 = Tare + MonitorsRow.MonGrossMass2;
                }
                return takeMonMass;
            }

            public bool SetMatrixFromGeometry()
            {
                if (EC.IsNuDelDetch(this.GeometryRow)) return false;
                //associate to geometry matrix row
                this.MatrixRow = this.GeometryRow.MatrixRow;
                return MatrixRow != null;
            }

            public bool SetMatrixFromStandard()
            {
                bool ok = false;
                if (EC.IsNuDelDetch(StandardsRow)) return ok;
                if (EC.IsNuDelDetch(StandardsRow.MatrixRow)) return ok;
                int standardMatrixID = StandardsRow.MatrixRow.MatrixID;
                if (EC.IsNuDelDetch(MatrixRow) || MatrixID != standardMatrixID)
                {
                    MatrixRow = StandardsRow.MatrixRow;
                }
                return EC.IsNuDelDetch(MatrixRow);
            }

            public bool SetMVialFromGeometry()
            {
                if (EC.IsNuDelDetch(this.GeometryRow)) return false;
                this.VialTypeRow = this.GeometryRow.VialTypeRow;
                return VialTypeRow != null;
            }

            public bool SetName(ref int _lastSampleNr)
            {
                if (EC.IsNuDelDetch(MonitorsRow) && IsSubSampleNameNull())
                {
                    this.SubSampleName = this.GetName(_lastSampleNr);
                    _lastSampleNr++;
                }
                return !string.IsNullOrEmpty(SubSampleName);
            }

            public bool SetName(string sampleName)
            {
                if (!string.IsNullOrEmpty(sampleName))
                {
                    SubSampleName = sampleName;
                    return true;
                }
                else return false;
            }

            public bool SetNameFromMonitor()
            {
                // if (EC.IsNuDelDetch(MonitorsRow)) return false;
                string result = GetNameFromMonitor();
                bool ok = !string.IsNullOrEmpty(result);
                if (ok) SubSampleName = result;
                return ok;
            }

            public bool SetRabbit()
            {
                SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, null);
                VialTypeRow v = ChCapsuleRow;
                if (EC.IsNuDelDetch(v))
                {
                    bool ok = true;
                    if (EC.IsNuDelDetch(IrradiationRequestsRow)) return false;
                    if (EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow)) return false;

                    v = IrradiationRequestsRow.ChannelsRow.VialTypeRow;
                    if (EC.IsNuDelDetch(v)) ok = false;
                    if (!ok) SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, ASSIGN_RABBIT);
                    else
                    {
                        ChCapsuleID = v.VialTypeID;
                    }
                }
                else if (!EC.IsNuDelDetch(UnitRow))
                {
                    UnitRow.SetParent(ref v);
                }
                return !EC.IsNuDelDetch(ChCapsuleRow);
            }

            /// <summary>
            /// Takes from vial otherwise takes from Geometry.Vial
            /// </summary>
            /// <param name="takeFromVial"></param>
            public void SetRadiusFillHeight(bool takeFromVial)
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

                if ((fh != 0))
                {
                    this.FillHeight = fh;
                }
                if ((rad != 0))
                {
                    this.Radius = rad;
                }
            }

            public void SetRadiusFillHeight(bool calRad, bool calFh)
            {
                if (calRad) Radius = GetRadius();
                else if (calFh) FillHeight = GetFillHeight();
            }

            public bool SetStandard()
            {
                FC = 1;
                Comparator = true;
                Elements = string.Empty;
                bool ok = SetMatrixFromStandard();
                ok = ok && SetDescriptionFromStandard();
                return ok;
            }
        }
    }
}