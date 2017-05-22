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
   

        public partial class SubSamplesRow : ISampleRow
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

            public UnitRow UnitRow
            {
                get
                {
                    
                    UnitRow u = null;
                    if (GetUnitRows().Count() == 0)
                    {
                        u = (this.Table.DataSet as LINAA).Unit.NewUnitRow();
                        u.ToDo = true;
                        u.LastCalc = DateTime.Now;
                        u.LastChanged = DateTime.Now.AddMinutes(1);
                        u.IrrReqID = IrradiationRequestsID;
                        u.SampleID = SubSamplesID;
                        (this.Table.DataSet as LINAA).Unit.AddUnitRow(u);
                        ChannelsRow c = IrradiationRequestsRow.ChannelsRow;
                        u.SetParent(ref c);
                      //  IEnumerable<UnitRow> us = new List<UnitRow>();
                   //     (us as List<UnitRow>).Add(u);
                      //  (this.Table.DataSet as LINAA).Save<UnitRow>(ref us);
                        //list.Add(u);
                    }
                    else u = this.GetUnitRows().FirstOrDefault();
                    return u;
                }
            }

    
            public void CalculateDensity(bool caldensity)
            {
           
                SetColumnError(tableSubSamples.MatrixDensityColumn, null);

                bool matrixDensNull = this.MatrixRow == null;
                matrixDensNull = matrixDensNull || IsMatrixDensityNull() || MatrixDensity == 0;

             
                    if (this.Net != 0 && Vol != 0 && caldensity)
                    {
                        this.CalcDensity = (this.Net / Vol) * 1e-3;

                        if (matrixDensNull) return;

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
                    else if (caldensity && !matrixDensNull)
                    {
                        this.CalcDensity = this.MatrixRow.MatrixDensity;
                    }
             
            }

            public double CalculateMass()
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

            public void Check(DataColumn column, bool calMass, bool calRad, bool calFh, bool calDensity)
            {
                if (column == this.tableSubSamples.DirectSolcoiColumn)
                {
                    //not used aymore??
                    if (IsDirectSolcoiNull()) DirectSolcoi = true;
                }
                else if (this.tableSubSamples.GeometryNameColumn == column)
                {
                    if (checkGeometry())
                    {
                        //take from geo not vial (first argument)
                        checkFillRad(false);
                    }
                }
                else if (column == this.tableSubSamples.CapsulesIDColumn)
                {
                    if (checkVialCapsule())
                    {
                        // bool rowModified = RowState != DataRowState.Added;
                        checkFillRad(true);
                    }
                }
                else if (column == this.tableSubSamples.CalcDensityColumn)
                {
                    if (IsCalcDensityNull()) CalcDensity = 0;

                    if (calMass) CalculateMass();
                    if (calRad)
                    {
                        Radius = FindRadius();
                    }
                    else if (calFh)
                    {
                        FillHeight = FindFillingHeight();
                    }
                    return;
                }
                else if (this.tableSubSamples.FillHeightColumn == column)
                {
                    // if (!calDensity) CalculateMass();
                    if (calRad)
                    {
                        Radius = FindRadius();
                        return;
                    }
                    Vol = FindVolumen();
                    if (calDensity) CalculateDensity(true);
                    else if (calMass) CalculateMass();
                }
                else if (this.tableSubSamples.RadiusColumn == column)
                {
                    if (calFh)
                    {
                        FillHeight = FindFillingHeight();
                        return;
                    }
                    Vol = FindVolumen();
                    if (calDensity) CalculateDensity(true);
                    else if (calMass) CalculateMass();
                }
                else if (this.tableSubSamples.MatrixIDColumn == column)
                {
                    if (checkLastMatrix())
                    {
                        CalculateDensity(true);
                    }
                }
                else if (column == this.tableSubSamples.Gross1Column)
                {
                    if (Gross2 != Gross1)
                    {
                        Gross2 = Gross1;
                        return;
                    }
                    GrossAvg = (Gross1 + Gross2) * 0.5;
                    Net = GrossAvg - Tare;
                }
                else if (column == this.tableSubSamples.Gross2Column)
                {
                    GrossAvg = (Gross1 + Gross2) * 0.5;
                    Net = GrossAvg - Tare;

                    // EC.CheckNull(e.Column, e.Row);
                }
                else if (column == this.tableSubSamples.NetColumn)
                {
                    // Net = GrossAvg = Tare;
                    if (checkMass())
                    {
                        // Net = GrossAvg = Tare;
                        if (calDensity)
                        {
                            CalculateDensity(calDensity);
                        }
                        if (calRad)
                        {
                            Radius = FindRadius();
                        }
                        else if (calFh)
                        {
                            FillHeight = FindFillingHeight();
                        }
                        //return;
                    }
                }
                else if (column == this.tableSubSamples.ChCapsuleIDColumn)
                {
                    checkRabbit();
                }
                else if (column == this.tableSubSamples.MonitorsIDColumn)
                {
                    if (!checkMonitor()) return;

                    checkStandard();
                }
                else if (column == this.tableSubSamples.StandardsIDColumn)
                {
                    checkStandard();
                }
                else if (column == this.tableSubSamples.SubSampleTypeColumn)
                {
                    if (IsComparatorNull()) Comparator = false;
                }
                else if (column == this.tableSubSamples.IrradiationRequestsIDColumn)
                {
                    if (EC.IsNuDelDetch(IrradiationRequestsRow)) return;

                    IrradiationCode = IrradiationRequestsRow.IrradiationCode;
                }
                else if (column == this.tableSubSamples.ENAAColumn)
                {
                    checkENAA();
                }
                else
                {
                    if (column == this.tableSubSamples.fColumn || column == this.tableSubSamples.AlphaColumn)
                    {
                        checkfOrAlpha();
                        return;
                    }

                    bool inOut = (column == this.tableSubSamples.InReactorColumn);
                    inOut = inOut || (column == this.tableSubSamples.OutReactorColumn);
                    inOut = inOut || column == this.tableSubSamples.IrradiationTotalTimeColumn;
                    if (inOut)
                    {
                        checkTimes();
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
                    decimal r = Decimal.Round(Convert.ToDecimal(FindRadius()), 2);
                    decimal h = Decimal.Round(Convert.ToDecimal(FindFillingHeight()), 2);
                    string estimateRo = "MatSSF estimated it as " + ro + " gr/cm3\n";
                    string estimateFH = "If the Fill Height and Net mass are correct\nthe Radius should be " + r + " mm\n";
                    string estimateR = "Or if the Radius and Net mass are correct\nthe Filling Height should be " + h + " mm\n";
                    this.SetColumnError(col, HighLow + estimateRo + estimateFH + estimateR);
                }

                return seterror;
            }

            public bool CheckUnit()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableSubSamples.NonNullableUnit).Count();

                return count == 0;
            }

            /// <summary>
            /// MoistureContent Content must be in Percentage
            /// </summary>
            /// <returns></returns>
            public double DryMass()
            {
                return (this.Net - (this.Net * this.MoistureContent * 1e-2));  // netto dried mass in miligrams;
            }

            public double FindFillingHeight()
            {
                if (IsCalcDensityNull()) return 0;

                double deno = this.CalcDensity * FindSurface();
                double result = 0;

                if (deno != 0)
                {
                    result = 10 * this.Net * 1e-3 / deno;
                }
                return result;
            }

            public double FindRadius()
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

            public double FindSurface()
            {
                double result = Math.PI * this.Radius * this.Radius * 0.1 * 0.1; //indexer cm
                return result;
            }

            public double FindVolumen()
            {
                return FindSurface() * this.FillHeight * 0.1;
            }

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

         
        }

        public partial class SubSamplesRow
        {
            protected internal bool selected;

            protected internal object tag;

            protected internal void checkFillRad(bool takeFromVial)
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

            protected internal bool checkLastMatrix()
            {

                this.SetColumnError(this.tableSubSamples.MatrixNameColumn, null);

                bool ok = false;

                //has not been associated to a matrix before
                if (EC.IsNuDelDetch(this.MatrixRow))
                {
                    if (!EC.IsNuDelDetch(this.GeometryRow))
                    {
                        //associate to geometry matrix row
                        this.MatrixRow = this.GeometryRow.MatrixRow;
                    }
                }

               
                if (!EC.IsNuDelDetch(this.MatrixRow))
                {
                    MatrixDataTable set = (this.Table.DataSet as LINAA).Matrix;
                    //search template matrix
                    int templateID = 0;
                    bool templateNUll = this.MatrixRow.IsTemplateIDNull();
                    if (templateNUll) templateID = this.MatrixRow.MatrixID; 
                    else templateID = this.MatrixRow.TemplateID;

                    //find in the list of childs Rows
                    MatrixRow m = GetMatrixRows()
             .FirstOrDefault(o => o.MatrixID == templateID);

                    //row is not on the list of childs, find in the Template list 
                    if (m==null)
                    {
                        m = GetMatrixRows()
                 .FirstOrDefault(o => !o.IsTemplateIDNull() && o.TemplateID == templateID);

                    }
                    MatrixRow toClone = this.MatrixRow;
                    //does not exist, so adopt (CLONE) the matrix to the list
                    if (m == null)
                    {
                        m = set.NewMatrixRow();
                        m.SubSampleID = SubSamplesID; //the ID to identify
                        m.MatrixComposition = string.Empty;
                        m.MatrixName = string.Empty;
                        //important, to rellocate the Parent MATRIX
                        m.TemplateID = templateID;
                        set.AddMatrixRow(m);
                    }
                    //it is just updatMatrixNameing content
                    if (m != null)
                    {
                        if (m.MatrixDensity != toClone.MatrixDensity)
                        {
                            m.MatrixDensity = toClone.MatrixDensity;
                        }
                        if (m.MatrixComposition.CompareTo(toClone.MatrixComposition) != 0)
                        {
                            m.MatrixComposition = toClone.MatrixComposition;
                        }
                        if (m.MatrixName.CompareTo(toClone.MatrixName) != 0)
                        {
                            m.MatrixName = toClone.MatrixName;
                        }
                    }
                    //update lastMATRIX ID
                    if (MatrixID != m.MatrixID) MatrixID = m.MatrixID;

                    ok = !this.MatrixRow.HasErrors;
                }
                else
                {
                    // this.SetColumnError(this.tableSubSamples.GeometryNameColumn, "Plase assign a
                    // matrix for this geometry");
                    this.SetColumnError(this.tableSubSamples.MatrixNameColumn, "Plase assign a matrix either directly or through a geometry");
                   
                }

                return ok; 

            }
          
            protected internal void checkENAA()
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

            protected internal void checkfOrAlpha()
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

            protected internal bool checkGeometry()
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

            protected internal bool checkMass()
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

            protected internal bool checkMonitor()
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

            protected internal void checkRabbit()
            {
                SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, null);
                VialTypeRow v = VialTypeRowByChCapsule_SubSamples;
                if (EC.IsNuDelDetch(v))
                {
                    SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, "Assign an irradiation container");
                }
                else
                {
                    //SET THE RABBIT
                    UnitRow u = UnitRow;
                    if (!EC.IsNuDelDetch(u))
                    {
                        u.SetParent(ref v);
                    }
                }
            }

            protected internal bool checkStandard()
            {
                // StandardsRow std = StandardsRow;
                if (EC.IsNuDelDetch(StandardsRow)) return false;

                FC = 1;
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

            protected internal void checkTimes()
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

            protected internal bool checkVialCapsule()
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
                    // if (!EC.IsNuDelDetch(u)) u.Diameter = 2 * rad;
                    return true;
                }
                else if (aaFh && fh != 0)
                {
                    this.FillHeight = fh;
                    // if (!EC.IsNuDelDetch(u)) u.Length = fh;
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
        }

     
    }
}