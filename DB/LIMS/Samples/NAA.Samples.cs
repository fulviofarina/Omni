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
                string Sname = this.SubSampleNameColumn.ColumnName;
                return this.FirstOrDefault(LINAA.SelectorByField<SubSamplesRow>(sampleName.Trim().ToUpper(), Sname));
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

            private bool AAFh
            {
                get
                {
                    return (this.DataSet as LINAA).currentPref.AAFillHeight;
                }
            }

            private bool AARad
            {
                get
                {
                    return (this.DataSet as LINAA).currentPref.AARadius;
                }
            }

            private DataColumn[] nonNullable;

            private DataColumn[] geometric;

            public DataColumn[] Geometric
            {
                get
                {
                    if (geometric == null)
                    {
                        geometric = new DataColumn[] { columnFillHeight, columnRadius,
                     columnGeometryName, columnMatrixID,
                     columnGross1,columnGross2 ,columnGrossAvg };
                    }

                    return geometric;
                }
            }

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
                    else if (Geometric.Contains(e.Column))
                    {
                        if (!subs.CheckGeometry()) return;
                        if (!subs.CheckMatrix()) return;
                        if (!subs.CheckMass()) return;
                        if (!subs.CheckDensity(this.AAFh, this.AARad)) return;
                    }
                    else if (e.Column == this.columnMonitorsID)
                    {
                        subs.CheckMonitor();
                    }
                    else if (e.Column == this.columnStandardsID)
                    {
                        StandardsRow std = subs.StandardsRow;
                        if (!subs.CheckStandard(ref std)) return;
                        if (subs.IsSubSampleDescriptionNull() && !subs.StandardsRow.IsstdNameNull())
                        {
                            subs.SubSampleDescription = subs.StandardsRow.stdName;
                        }
                    }
                    else if (e.Column == this.columnSubSampleType)
                    {
                        if (subs.IsComparatorNull()) subs.Comparator = false;
                    }
                    else if (e.Column == this.columnIrradiationRequestsID)
                    {
                        //  if (subs.IsSubSampleCreationDateNull()) subs.SubSampleCreationDate = DateTime.Now;

                        if (EC.IsNuDelDetch(subs.IrradiationRequestsRow)) return;
                        if (subs.IsIrradiationCodeNull())
                        {
                            subs.IrradiationCode = subs.IrradiationRequestsRow.IrradiationCode;
                        }
                    }
                    else if (e.Column == this.columnInReactor || e.Column == this.columnOutReactor || e.Column == this.IrradiationTotalTimeColumn)
                    {
                        subs.CheckTimes();
                    }
                    else if (e.Column == this.CapsulesIDColumn)
                    {
                        subs.SetColumnError(this.CapsuleNameColumn, null);
                        if (EC.IsNuDelDetch(subs.VialTypeRow)) subs.SetColumnError(this.CapsuleNameColumn, "Assign an irradiation capsule");
                    }
                    else if (e.Column == this.ENAAColumn) subs.CheckENAA();
                    else if (e.Column == this.columnf || e.Column == this.columnAlpha) subs.CheckfOrAlpha();
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
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
            // creates a MatSSF unit
            /*
            public UnitRow NewUnit ()
            {
                UnitRow u = null;
                LINAA set =  ((LINAA)(this.tableSubSamples.DataSet));
               UnitDataTable dt = set.tableUnit;
               u = dt.LastOrDefault(o => o.SampleID == this.SubSamplesID);
               if (u == null) u = dt.NewUnitRow();
               u.SampleID = this.SubSamplesID;

               dt.AddUnitRow(u);

               return u;
            }
             * */

            public void CheckENAA()
            {
                if (IsENAANull()) ENAA = false;
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
                if ((diff / Gross2) >= pent)
                {
                    SetColumnError(col, "Difference between weights is higher than 0.1%\nPlease check");
                    return false;
                }
                return true;
            }

            public void CheckMonitor()
            {
                if (EC.IsNuDelDetch(MonitorsRow)) return;

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

                StandardsRow std = MonitorsRow.StandardsRow;

                if (!CheckStandard(ref std)) return;
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

            public bool CheckStandard(ref StandardsRow std)
            {
                if (EC.IsNuDelDetch(std)) return false;

                Concentration = 1;
                Comparator = true;
                Elements = string.Empty;
                if (EC.IsNuDelDetch(std.MatrixRow)) return false;
                if (EC.IsNuDelDetch(MatrixRow) || MatrixID != std.MatrixRow.MatrixID)
                {
                    MatrixRow = std.MatrixRow;
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
                //else if (IrradiationTotalTime != aux2 && aux2 != 0)
                //	{
                //  IrradiationTotalTime = aux2;
                //  return;
                //	}
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

            public bool CheckDensity(bool aaFh, bool aaRad)
            {
                if (nofillheight && this.GeometryRow.FillHeight != 0)
                {
                    this.FillHeight = this.GeometryRow.FillHeight;
                    return false;
                }
                bool noradius = EC.CheckNull(this.tableSubSamples.RadiusColumn, this);
                if (noradius && this.GeometryRow.Radius != 0)
                {
                    this.Radius = this.GeometryRow.Radius;
                    return false;
                }

                DataColumn matDenCol = this.tableSubSamples.MatrixDensityColumn;
                double ro = this.FindDensity();

                decimal density = Decimal.Round(Convert.ToDecimal(ro), 3);
                string estimateRo = "The calculated density = " + density;

                if (nomatrixdensity)
                {
                    this.SetColumnError(matDenCol, "There is no default density value assigned to this matrix, a comparison is not possible.\n" + estimateRo + " might not be the correct one\nPlease verify this");
                    return false;
                }

                bool seterror = false;
                string HighLow = string.Empty;
                double ratio = (this.MatrixDensity / ro) * 100;
                int pent = 3;

                if (ratio >= 100 + pent)
                {
                    HighLow = " gr/cm3 exceeds more than " + pent + "% the density shown\n\n";
                    seterror = true;
                }
                else if (ratio <= 100 - pent)
                {
                    HighLow = " gr/cm3 is " + pent + "% lower than the density shown\n\n";
                    seterror = true;
                }

                if (!seterror) return !nomatrixdensity;

                double radius = this.FindRadius();
                double fh = this.FindFillingHeight();

                if (aaRad && radius != 0)
                {
                    this.Radius = radius;
                    return true;
                }
                else if (aaFh && fh != 0)
                {
                    this.FillHeight = fh;
                    return true;
                }

                if (aaFh == false && aaRad == false) return false;

                decimal rad = Decimal.Round(Convert.ToDecimal(radius), 3);
                decimal fil = Decimal.Round(Convert.ToDecimal(fh), 3);
                string estimateR = "If the Radius and Net mass are correct\nthe Filling Height should be " + fil + " mm\n";
                string estimateFH = "Or if the Fill Height and Net mass are correct\nthe Radius should be " + rad + " mm\n";
                this.SetColumnError(matDenCol, estimateRo + HighLow + estimateFH + estimateR);
                return false;
            }

            public bool CheckMatrix()
            {
                MatrixRow m = this.GeometryRow.MatrixRow;
                //bool diffMat = (this.MatrixRow!=m ); //different matrix than the geometry... restore it?
                if (EC.IsNuDelDetch(this.MatrixRow))
                {
                    if (!EC.IsNuDelDetch(m))
                    {
                        this.MatrixID = this.GeometryRow.MatrixID;
                        return true;
                    }
                    else
                    {
                        this.SetColumnError(this.tableSubSamples.GeometryNameColumn, "Plase assign a matrix for this geometry");
                        return false;
                    }
                }
                else return true;
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
                else return true;
            }

            private bool noradius
            {
                get
                {
                    return EC.CheckNull(this.tableSubSamples.RadiusColumn, this);
                }
            }

            private bool nomatrixdensity
            {
                get
                {
                    return EC.CheckNull(this.tableSubSamples.MatrixDensityColumn, this);
                }
            }

            private bool nofillheight
            {
                get
                {
                    return EC.CheckNull(this.tableSubSamples.FillHeightColumn, this);
                }
            }

            #region Others

            public double FindVolumen()
            {
                return FindSurface() * this.FillHeight * 0.1;
            }

            public double FindSurface()
            {
                return Math.PI * this.Radius * this.Radius * 0.1 * 0.1;
            }

            public double FindDensity()
            {
                return (this.DryMass() * 1e-3 / FindVolumen());
            }

            public double FindRadius()
            {
                return 10 * Math.Sqrt((this.DryMass() * 1e-3 / (Math.PI * this.MatrixDensity * this.FillHeight * 0.1)));
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

            #endregion Others

            #region Properties

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

            #endregion Properties
        }

        partial class MonitorsDataTable
        {
            public MonitorsRow FindByMonName(string MonName)
            {
                string field = this.MonNameColumn.ColumnName;
                MonitorsRow old = this.FirstOrDefault(LINAA.SelectorByField<MonitorsRow>(MonName.Trim().ToUpper(), field));
                return old;
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    MonitorsRow m = e.Row as MonitorsRow;

                    if (e.Column == this.MonGrossMass1Column || e.Column == this.MonGrossMass2Column)
                    {
                        bool one = EC.CheckNull(this.MonGrossMass1Column, e.Row);
                        bool two = EC.CheckNull(this.MonGrossMass2Column, e.Row);

                        if (one || two) return;

                        if (one || two) return;

                        double diff = Math.Abs(m.MonGrossMass1 - m.MonGrossMass2) * 100;

                        double pent = 0.03;

                        m.SetColumnError(this.MonGrossMassAvgColumn, null);
                        if ((diff / m.MonGrossMass1) > pent)
                        {
                            m.SetColumnError(this.MonGrossMassAvgColumn, "Difference between weights is higher than 0.03%\nPlease check");
                        }
                        else if ((diff / m.MonGrossMass2) > pent)
                        {
                            m.SetColumnError(this.MonGrossMassAvgColumn, "Difference between weights is higher than 0.03%\nPlease check");
                        }
                    }
                    else if (e.Column == this.MonNameColumn)
                    {
                        if (EC.CheckNull(this.MonNameColumn, m)) return;

                        if (Dumb.IsLower(m.MonName.Substring(1)))
                        {
                            m.MonName = m.MonName.ToUpper();
                        }
                        else m.MonitorCode = m.MonName.Substring(0, m.MonName.Length - 3);
                    }
                    else if (e.Column == this.columnGeometryName)
                    {
                        if (EC.CheckNull(e.Column, e.Row))
                        {
                            if (!m.IsMonitorCodeNull()) m.GeometryName = m.MonitorCode.ToUpper();
                        }
                        else if (Dumb.IsLower(m.GeometryName.Substring(1)))
                        {
                            m.GeometryName = m.GeometryName.ToUpper();
                        }
                    }
                    else if (e.Column == this.LastIrradiationDateColumn)
                    {
                        if (m.IsLastIrradiationDateNull()) m.LastIrradiationDate = new DateTime(1999, 1, 1);
                        m.NumberOfDays = (DateTime.Today - m.LastIrradiationDate).Days;
                    }
                    else if (e.Column == this.LastMassDateColumn)
                    {
                        if (m.IsLastMassDateNull()) m.LastMassDate = new DateTime(1999, 1, 1);
                        m.Difference = (DateTime.Today - m.LastMassDate).Days;
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }

        partial class StandardsDataTable
        {
            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    StandardsRow std = (StandardsRow)e.Row;
                    if (e.Column == this.MonitorCodeColumn)
                    {
                        bool nulo = EC.CheckNull(e.Column, e.Row);

                        if (nulo) return;

                        if (std.GeometryRow == null && std.MatrixRow != null)
                        {
                            LINAA l = ((LINAA)this.DataSet);
                            GeometryDataTable gdt = l.tableGeometry;
                            GeometryRow g = gdt.NewGeometryRow();
                            g.GeometryName = std.MonitorCode;
                            gdt.AddGeometryRow(g);
                            g.MatrixID = std.MatrixID;
                            g.VialTypeID = l.tableVialType.FirstOrDefault(o => o.VialTypeRef.CompareTo("Bare") == 0).VialTypeID;
                            std.GeometryRow = g;
                        }
                    }
                    else if (e.Column == this.stdNameColumn) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.stdProducerColumn) EC.CheckNull(e.Column, e.Row);
                    //  else if (e.Column == this.stdElementColumn) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.MatrixNameColumn) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.stdUncColumn) EC.CheckNull(e.Column, e.Row);
                }
                catch (SystemException ex)
                {
                    e.Row.RowError += ex.TargetSite.Name + "\t" + ex.Message + "\n";
                }
            }
        }
    }
}