using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// Cleaned
        /// </summary>
        public partial class SubSamplesRow 
        {
            protected double perCentDiff = 0.1;

            protected string _projectNr
            {
                get
                {
                    // this.SubSampleName;
                    return Regex.Replace(this.IrradiationCode, "[a-z]", String.Empty, RegexOptions.IgnoreCase);
                }
            }
            internal bool CheckGeometry()
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

            internal bool CheckMass()
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
                bool ok = false;
                if ((diff / Gross1) >= perCentDiff)
                {
                    SetColumnError(col, "Difference between weights is higher than 0.1%\nPlease check");
                }
                else if ((diff / Gross2) >= perCentDiff)
                {
                    SetColumnError(col, "Difference between weights is higher than 0.1%\nPlease check");
                }
                else ok = true;
                return ok;
            }

            internal bool CheckVialCapsule()
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

            internal void GetDensity(bool caldensity)
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

            internal string GetDescriptionFromMonitor()
            {
                string description = string.Empty;
                if (EC.IsNuDelDetch(MonitorsRow)) return description;
                bool namenull = MonitorsRow.IsMonNameNull();
                if (!namenull) description = MonitorsRow.MonName;
                return description;
            }

            internal string GetDescriptionFromStandard()
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
            internal double GetDryMass()
            {
                return (this.Net - (this.Net * this.MoistureContent * 1e-2));  // netto dried mass in miligrams;
            }

            internal double GetFillHeight()
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

            internal double GetGrossMass()
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

            internal string GetIrradiationCode()
            {
                if (EC.IsNuDelDetch(IrradiationRequestsRow)) return string.Empty;

                if (IrradiationRequestsRow.IsIrradiationCodeNull()) return string.Empty;
                return IrradiationRequestsRow.IrradiationCode;
            }

            internal MatrixRow GetMatrixByMatrixID(int templateID)
            {
                //find in the list of childs Rows
                return GetMatrixRows()
         .FirstOrDefault(o => o.MatrixID == templateID);
            }

            internal MatrixRow GetMatrixByTemplateID(int templateID)
            {
                //find child from template
                return GetMatrixRows()
         .FirstOrDefault(o => !o.IsTemplateIDNull() && o.TemplateID == templateID);
            }
            internal string GetMonitorNameFromSampleName()
            {
                string newName = string.Empty;
                if (string.IsNullOrEmpty(SubSampleName)) return newName;
                newName = SubSampleName.Trim().ToUpper();
                //     string _projectNr = Regex.Replace(IrradiationCode, "[a-z]", String.Empty, RegexOptions.IgnoreCase);
                //take default vlues
                string code = IrradiationCode.Replace(_projectNr, null);
                if (!EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow))
                {
                    code = IrradiationRequestsRow.ChannelsRow.IrReqCode.Trim().ToUpper();
                }
                string projectNoCode = IrradiationCode.Replace(code, null);
                return newName.Replace(projectNoCode, null);
            }

            internal string GetName(int _lastSampleNr)
            {
                string name = string.Empty;
                if (_lastSampleNr >= 10) name = _projectNr + _lastSampleNr.ToString();
                else name = _projectNr + "0" + _lastSampleNr.ToString();
                return name;
            }

            internal string GetNameFromMonitor()
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

            internal double GetRadius()
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

            internal double GetSurface()
            {
                double result = Math.PI * this.Radius * this.Radius * 0.1 * 0.1; //indexer cm
                return result;
            }

            internal double GetTotalMinutes()
            {
                double aux2 = 0;
                if (!IsOutReactorNull() && !IsInReactorNull()) aux2 = (OutReactor - InReactor).TotalMinutes;

                return aux2;
            }

            internal double GetVolume()
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

            internal void SetCreationDate()
            {
                SubSampleCreationDate = DateTime.Now;
            }

            internal bool SetDescriptionFromMonitor()
            {
                SubSampleDescription = GetDescriptionFromMonitor();

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
                if (innull) SetColumnError(inCol, "Set an Irradiation Start date/time");
                else SetColumnError(inCol, null);

                DataColumn outCol = this.tableSubSamples.OutReactorColumn;
                //out
                if (outnull) SetColumnError(outCol, "Set an Irradiation End date/time");
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

            internal void SetIrradiationRequestID(int? IrrReqID)
            {
                if (IrrReqID != null) IrradiationRequestsID = (int)IrrReqID;
            }

            internal void SetIrradiationTime(double totalMins)
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
                    else SetIrradiationDates();
                }
            }

            internal bool SetLastMatrix()
            {
                this.SetColumnError(this.tableSubSamples.MatrixNameColumn, null);

                if (EC.IsNuDelDetch(this.MatrixRow)) SetMatrixFromGeometry();

                if (!EC.IsNuDelDetch(this.MatrixRow))
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
                        MatrixDataTable set = db.Matrix;
                        m = set.NewMatrixRow();
                        set.AddMatrixRow(m);
                        m.SetBasic(SubSamplesID, templateID);
                    }
                    //it is just updatMatrixNameing content
                    if (m != null)
                    {
                        MatrixRow toClone = this.MatrixRow;
                        m.cloneFromMatrix(ref toClone);
                    }
                    //update lastMATRIX ID
                    if (MatrixID != m.MatrixID) MatrixID = m.MatrixID;

                  //  ok = !this.MatrixRow.HasErrors();
                }
                else
                {
                    this.SetColumnError(this.tableSubSamples.MatrixNameColumn, "Plase assign a matrix either directly or through a geometry");
                }

                return !EC.IsNuDelDetch(this.MatrixRow);
            }

            internal bool SetMassFromMonitor(bool force = true)
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

            internal bool SetMatrixFromGeometry()
            {
                if (EC.IsNuDelDetch(this.GeometryRow)) return false;
                //associate to geometry matrix row
                this.MatrixRow = this.GeometryRow.MatrixRow;
                return MatrixRow!=null;
            }
            internal bool SetMatrixFromStandard()
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

            internal bool SetMVialFromGeometry()
            {
                if (EC.IsNuDelDetch(this.GeometryRow)) return false;
                this.VialTypeRow = this.GeometryRow.VialTypeRow;
                return VialTypeRow!=null;
            }
            internal bool SetName(ref int _lastSampleNr)
            {
                if (EC.IsNuDelDetch(MonitorsRow) && IsSubSampleNameNull())
                {
                    this.SubSampleName = this.GetName(_lastSampleNr);
                    _lastSampleNr++;
                }
                return !string.IsNullOrEmpty(SubSampleName);
            }

            internal bool SetName(string sampleName)
            {
                if (!string.IsNullOrEmpty(sampleName))
                {
                    SubSampleName = sampleName;
                    return true;
                }
                else return false;
            }
            internal bool SetNameFromMonitor()
            {
                // if (EC.IsNuDelDetch(MonitorsRow)) return false;
                string result = GetNameFromMonitor();
                bool ok = !string.IsNullOrEmpty(result);
                if (ok) SubSampleName = result;
                return ok;
            }

            internal bool SetRabbit()
            {
                SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, null);
                VialTypeRow v = ChCapsuleRow;
                if (EC.IsNuDelDetch(v))
                {
                    bool ok = true;
                    if (EC.IsNuDelDetch(IrradiationRequestsRow) ) return false;
                    if (EC.IsNuDelDetch(IrradiationRequestsRow.ChannelsRow)) return false;
                
                    v = IrradiationRequestsRow.ChannelsRow.VialTypeRow;
                    if (EC.IsNuDelDetch(v)) ok =false;
                    if (!ok) SetColumnError(this.tableSubSamples.ChCapsuleIDColumn, "Assign an irradiation container");
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
            internal void SetRadiusFillHeight(bool takeFromVial)
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

            internal void SetRadiusFillHeight(bool calRad, bool calFh)
            {
                if (calRad) Radius = GetRadius();
                else if (calFh) FillHeight = GetFillHeight();
            }

            internal bool SetStandard()
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