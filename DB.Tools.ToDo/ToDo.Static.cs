﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Rsx.Math;

namespace DB.Tools
{
    //statics
    public partial class ToDo
    {
        protected internal static double Weight(ref LINAA.ToDoResRow p)
        {
            p.w = 0.0;
            p.w2 = 0.0;
            p.wX = 0.0;
            p.wX2 = 0.0;
            if (p.Flag == 1)
            {
                p.w = (1.0 / p._SD_2);
                p.w2 = p.w * p.w;
                p.wX = p.w * (p.X);
                p.wX2 = p.wX * (p.X);
            }
            return p.w;
        }

        protected internal static void SetUncs(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                //artificial not used, just for Delegate compatibility...

                if (X.Bare) //f-Alpha Bare method
                {
                    p.unc = Math.Pow(p.unc, 2) + Math.Pow(p.k0Unc, 2);
                    p.unc = Math.Sqrt(p.unc);
                    p.unc2 = Math.Pow(p.unc2, 2) + Math.Pow(p.k02Unc, 2);
                    p.unc2 = Math.Sqrt(p.unc2);
                }
                else if (X.CdCov) // Alpha Cd-Covered
                {
                    p.unc = Math.Pow(p.unc, 2) + Math.Pow(p.k0Unc, 2);
                    p.unc = Math.Sqrt(p.unc);
                }

                //and now the total!
                //contains all the uncertainty from Asp 1 and 2
                p.SD = Math.Pow(p.unc, 2) + Math.Pow(p.unc2, 2);
                p.SD = Math.Sqrt(p.SD);
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected internal static void SetAsp(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                //  p.use = p.ToDoRow.use;

                LINAA.IPeakAveragesRow monitor = p.IPAvg;
                LINAA.IPeakAveragesRow refe = p.IPAvg2;
                // IRequestsAveragesRow M = p.ToDoAvgRow.IR;
                // IRequestsAveragesRow R = p.ToDoAvgRow.IR2;

                if (monitor.MD != refe.MD && X.QoOrCd)
                {
                    p.use = false;
                    p.RowError = "For this kind of Analysis it's necessary to select measurements were the isotope followed the same decay scheme\ni.e. Use only IVa OR IVb measurements but not combined";
                    return;
                }

                LINAA.IPeakAveragesRow aRow = null;

                Func<LINAA.MeasurementsRow, IEnumerable<LINAA.PeaksRow>> func = m =>
                {
                    return m.GetPeaksRows().Where(d => d.IPeakAveragesRowParent == aRow);
                };

                short MinPos = X.k0BareMinPos;

                Func<LINAA.IPeakAveragesRow, double[]> funcMain = arow =>
                {
                    IEnumerable<LINAA.PeaksRow> peaks = null;
                    IEnumerable<LINAA.MeasurementsRow> meas = null;
                    //Specific Activities
                    double[] a = null;
                    peaks = arow.GetPeaksRows();
                    meas = peaks.Select(c => c.MeasurementsRow);
                    meas = meas.Where(m => m.Position >= MinPos);
                    peaks = meas.SelectMany(func);

                    a = MyMath.WAverageStDeV(peaks, "Y", "AreaUncertainty");

                    return new double[] { a[0], a[1], a[2], peaks.Count() };
                };

                aRow = monitor;
                double[] avg = funcMain(aRow);

                //uncertainty fromRow counting statistics only
                if (monitor.MD == 0)
                {
                    p.Asp = avg[0] / (monitor.k0 * monitor.ppm);
                    //   p.Asp = monitor.X / (monitor.k0 * monitor.ppm);
                    p.k0 = monitor.k0;
                    p.k0Unc = monitor.k0Unc;
                }
                else if (monitor.MD == 1)
                {
                    //   p.Asp = monitor.X / (monitor.k0x * monitor.ppm);

                    p.Asp = avg[0] / (monitor.k0x * monitor.ppm);
                    p.k0 = monitor.k0x;
                    p.k0Unc = monitor.k0xUnc;
                }
                else
                {
                    p.use = false;
                    p.RowError = "For this kind of Analysis it's necessary to select measurements were the isotope followed the same decay scheme\ni.e. Use only IVa OR IVb measurements but not combined";
                }
                //  p.unc = monitor.GActUnc;
                //   p.n = monitor.n;
                p.unc = avg[1];
                p.n = Convert.ToInt32(avg[3]);
                if (p.n == 0)
                {
                    p.use = false;
                    return;
                }

                if (p.n != 0) p.unc /= Math.Sqrt(p.n);

                if (X.CdCov) return; // do not go with second isotope

                aRow = refe; //important, for the function to deference

                avg = funcMain(aRow);

                if (refe.MD == 0)
                {
                    //  p.Asp2 = refe.X / (refe.k0 * refe.ppm);

                    p.Asp2 = avg[0] / (refe.k0 * refe.ppm);
                    p.k02 = refe.k0;
                    p.k02Unc = refe.k0Unc;
                }
                else if (refe.MD == 1)
                {
                    p.Asp2 = avg[0] / (refe.k0x * refe.ppm);
                    p.k02 = refe.k0x;
                    p.k02Unc = refe.k0xUnc;
                }
                else
                {
                    p.use = false;
                    p.RowError = "For this kind of Analysis it's necessary to select measurements were the isotope followed the same decay scheme\ni.e. Use only IVa OR IVb measurements but not combined";
                }

                p.unc2 = avg[1];
                int n = Convert.ToInt32(avg[3]);

                if (n == 0)
                {
                    p.use = false;
                    return;
                }

                if (n != 0) p.unc /= Math.Sqrt(n);

                p.n += n;
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected internal static bool GetResonanceFromAvg(ref LINAA.ToDoAvgRow tavg, bool reference, bool resetEr)
        {
            LINAA.NAARow n = null;
            if (!reference)
            {
                LINAA.IRequestsAveragesRow monitor = tavg.IR;
                n = monitor.NAARow;

                //reset should be false for Qo-determination and other Er determ methods...
                if (!n.IsErNull() && resetEr)
                {
                    tavg.Er = n.Er;
                    if (n.pValuesRow != null) tavg.Er = tavg.Er * Math.Exp(-1 * tavg.alpha * n.pValuesRow.p);
                }
                tavg.Calpha = MyMath.Calpha(tavg.alpha);
                double Qoalpha = MyMath.qoalpha(tavg.alpha, tavg.Qo1, tavg.Er) + tavg.Calpha; //Qalpha calculo g
                tavg.qoalpha = Qoalpha;   // contains alpha
                tavg.qoalpha -= tavg.Calpha;

                tavg._QoEr_a = tavg.Qo1 * Math.Pow(tavg.Er, tavg.alpha);
            }
            else
            {
                //this will calculate the original Er of alpha from database and store on Er2
                //this is because all sub-indices =2 refer to original value from database
                //so i can put artificial values on subindices 1 (Qo1, Er1) for Qo-determination

                //it must be remarked that Qo2 here is the original one because in principle nobody has changed it
                //and no part of te code here does.
                //instead, I use explicitely Er1 and Qo1 for Qo-dertemination

                LINAA.IRequestsAveragesRow refe = tavg.IR2;
                n = refe.NAARow;
                if (!n.IsErNull()) tavg.Er2 = n.Er;
                if (n.pValuesRow != null) tavg.Er2 = tavg.Er2 * Math.Exp(-1 * tavg.alpha * n.pValuesRow.p);

                double Qoalpha = MyMath.qoalpha(tavg.alpha, tavg.Qo2, tavg.Er2) + tavg.Calpha; //Qalpha calculo g
                tavg.qo2alpha = Qoalpha;    // contains alpha
                tavg.qo2alpha -= tavg.Calpha;
                tavg._Qo2Er2_a = tavg.Qo2 * Math.Pow(tavg.Er2, tavg.alpha);
            }

            return true;
        }

        protected internal static string CalcRateAndTemporal(ref LINAA.IRequestsAveragesRow monitor, double alpha0, double f0, bool sameDecay)
        {
            if (sameDecay)
            {
                int vicious = monitor.GetIPeakAveragesRows().Where(p => p.MD == 1).Count();
                if (vicious != 0)
                {
                    return "The isotope has a Dechay Scheme dependence on f and Alpha\nCannot be employed for f-Alpha determination";
                }
            }

            WC.Rate(alpha0, f0, ref monitor);
            //recalculate SDs and check Fc/ppms
            if (monitor.IsAspNull())
            {
                IEnumerable<LINAA.MeasurementsRow> meas = monitor.SubSamplesRow.GetMeasurementsRows();
                meas = meas.Where(m => m.Selected);
                WC.Temporal(ref meas, ref monitor);
                IEnumerable<LINAA.IPeakAveragesRow> avgs = monitor.GetIPeakAveragesRows();
                WC.FindSDs(ref avgs);
                WC.FindSDs(ref monitor);
            }

            return string.Empty;
        }

        protected internal static void GetResonanceFromScratch(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            //so we have everything need for input, create TodoDataRow
            LINAA.IRequestsAveragesRow monitor = t.IR;
            string result = CalcRateAndTemporal(ref monitor, t.alpha, t.f, X.SameDecay);
            if (!result.Equals(string.Empty))
            {
                t.use = false;
                t.RowError = result;
                return;
            }

            t.Er = monitor._Er;
            t.ErUnc = monitor.NAARow.ErUnc;
            t.Qo1 = monitor._Qo;
            //original Qo(0)

            //interins
            t.Calpha = monitor.Calpha;
            t.Cd = monitor._Cd;
            t.Gt = monitor.Gt;
            t.Ge = monitor.Ge;

            t.QoUnc = monitor._QoUnc;
            t.qoalpha = monitor._Qoalpha;   // contains alpha
            t.qoalpha -= monitor.Calpha;

            t._QoEr_a = monitor._Qo * Math.Pow(t.Er, t.alpha);

            if (!X.CdCov)
            {
                LINAA.IRequestsAveragesRow refe = t.IR2;
                result = CalcRateAndTemporal(ref refe, t.alpha, t.f, X.SameDecay);
                if (!result.Equals(string.Empty))
                {
                    t.use = false;
                    t.RowError = result;
                    return;
                }
                t.Er2 = refe._Er;
                t.Er2Unc = refe.NAARow.ErUnc;
                t.Qo2 = refe._Qo;
                t.Cd2 = refe._Cd;
                t.Gt2 = refe.Gt;
                t.Ge2 = refe.Ge;
                t.Qo2Unc = refe._QoUnc;
                t.qo2alpha = refe._Qoalpha;   // contains alpha
                t.qo2alpha -= refe.Calpha;

                t._Qo2Er2_a = refe._Qo * Math.Pow(t.Er2, t.alpha);
            }
        }

        /// <summary>
        /// not used because Im using expressions
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected internal static double X(ref LINAA.ToDoResAvgRow i)
        {
            IEnumerable<LINAA.ToDoResRow> peaks = i.GetToDoResRows();

            i.n = peaks.Sum(p => p.Flag);

            double Sumw = peaks.Sum(p => p.w);
            double SumwX = peaks.Sum(p => p.wX);

            if (i.n >= 1)
            {
                if (Sumw != 0)
                {
                    i.X = (SumwX / Sumw);
                }
            }

            return i.X;
        }

        protected internal static void StDev(ref LINAA.ToDoResAvgRow i)
        {
            IEnumerable<LINAA.ToDoResRow> peaks = i.GetToDoResRows();

            double aux = 0.0;

            if (i.n == 1)
            {
                LINAA.ToDoResRow p = peaks.FirstOrDefault();
                if (p != null)
                {
                    aux = p._SD_2;
                    aux = Math.Sqrt(aux);
                    i.SD = aux;
                    i.ObsSD = 0.0;
                }
                else i.RowError = "Not child Row found";
            }
            else if (i.n > 1 && i.X != 0.0)
            {
                //decimal SumwX2 = peaks.Sum(p => p.wX2);
                //decimal Sumw2 = peaks.Sum(p => p.w2);
                //decimal Sumw = peaks.Sum(p => p.w);
                //decimal SumwX = peaks.Sum(p => p.wX);

                //	aux = (double)((Sumw * SumwX2) - (SumwX * SumwX));
                //aux /= (double)((Sumw * Sumw) - Sumw2);
                aux = 100.0 * Math.Sqrt(i._var);
                aux /= i.X;
                i.SD = aux;

                //aux = (double)((SumwX2 / Sumw));
                //	aux -= (i.X * i.X);
                aux = 100.0 * Math.Sqrt(i._Obsvar);
                aux /= i.X;
                i.ObsSD = aux;
            }
            else
            {
                i.SD = 0.0;
                i.ObsSD = 0.0;
            }
        }

        /// <summary>
        /// not used because I am using expressions
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected internal static double StDevNoExpressions(ref LINAA.ToDoResAvgRow i)
        {
            IEnumerable<LINAA.ToDoResRow> peaks = i.GetToDoResRows();

            double aux = 0;

            if (i.n == 1)
            {
                LINAA.ToDoResRow p = peaks.FirstOrDefault();
                if (p != null)
                {
                    aux = p._SD_2;
                    aux = Math.Sqrt(aux);
                    i.SD = aux;
                    i.ObsSD = 0;
                }
                else i.RowError = "Not child cRow found";
            }
            else if (i.n > 1 && i.X != 0)
            {
                double SumwX2 = peaks.Sum(p => p.wX2);
                double Sumw2 = peaks.Sum(p => p.w2);
                double Sumw = peaks.Sum(p => p.w);
                double SumwX = peaks.Sum(p => p.wX);

                aux = ((Sumw * SumwX2) - (SumwX * SumwX));
                aux /= ((Sumw * Sumw) - Sumw2);
                aux = 100.0 * Math.Sqrt(aux);
                aux /= i.X;
                i.SD = aux;

                aux = ((SumwX2 / Sumw));
                aux -= (i.X * i.X);
                aux = 100.0 * Math.Sqrt(aux);
                aux /= i.X;
                i.ObsSD = aux;
            }
            else
            {
                i.SD = 0;
                i.ObsSD = 0;
            }

            return i.SD;
        }

        public static void Reset<T>(ref T row, ref ToDoArgs args)
        {
            Type tipo = row.GetType();

            if (tipo.Equals(typeof(LINAA.ToDoAvgRow)))
            {
                LINAA.ToDoAvgRow t = row as LINAA.ToDoAvgRow;
                Reset(ref t, ref args);
            }
            else if (tipo.Equals(typeof(LINAA.ToDoAvgUncRow)))
            {
                LINAA.ToDoAvgUncRow t = row as LINAA.ToDoAvgUncRow;
                Reset(ref t, ref args);
            }
            else if (tipo.Equals(typeof(LINAA.ToDoDataRow)))
            {
                LINAA.ToDoDataRow t = row as LINAA.ToDoDataRow;
                Reset(ref t, ref args);
            }
            else throw new SystemException("Not impletemented");
        }

        protected internal static void Reset(ref LINAA.ToDoDataRow t, ref ToDoArgs args)
        {
            t.ClearErrors();
            t.Ai = 0;
            t.Asp = 0;
            t.Asp2 = 0;
            t.Fc = 0;
            t.Fc2 = 0;
            t.RCd = 0;
            t.unc = 0;
            t.unc2 = 0;
            t.use = true;
            t.dk0 = 0;
            t.dk02 = 0;
            //	t.k0 = 0;
            //	t.Energy = 0;
            //	t.k0Unc = 0;
            //	t.k02 = 0;
            //	t.k02Unc = 0;
            //	t.Energy2 = 0;
            if (t.ToDoRow != null) t.use = t.ToDoRow.use;
        }

        protected internal static void Reset(ref LINAA.ToDoAvgUncRow t, ref ToDoArgs args)
        {
            t.ZaEr = 0;
            t.ZaEr2 = 0;
            t.ZaECd = 0;
            t.ZaQo = 0;
            t.ZaQo2 = 0;
            t.ZaAsp = 0;
            t.ZaAsp2 = 0;
            t.Bi = 0;
            t.Vi = 0;
            t.Ji = 0;
            t.Ji2 = 0;
            t.Zfa = 0;
            t.ZfEr = 0;
            t.ZfEr2 = 0;
            t.ZfQo = 0;
            t.ZfQo2 = 0;
            t.ZfAsp = 0;
            t.ClearErrors();
        }

        protected internal static void Reset(ref LINAA.ToDoAvgRow t, ref ToDoArgs args)
        {
            t.ClearErrors();
            t.__qoa_qo2a__1 = 0;
            t.YErrorHigh = 0;
            t.YErrorLow = 0;
            t.n = 0;
            t.fUnc = 0;

            t.Fc = 0;
            t.Fc2 = 0;
            t.dQo = 0;
            t.R0 = 0;
            t.R02 = 0;
            t.qo2alpha = 0;
            t.qoalpha = 0;
            t.Calpha = 0;
            t._QoEr_a = 0;
            t._Qo2Er2_a = 0;
            t.Qo1 = 0;
            t.Qo2 = 0;
            t.QoUnc = 0;
            t.Qo2Unc = 0;
            t.Er = 0;
            t.Er2 = 0;
            t.ErUnc = 0;
            t.Er2Unc = 0;
            t.SD = 0;

            t.Gt = 0;
            t.Ge = 0;
            t.Gt2 = 0;
            t.Ge2 = 0;

            t.f = 0;
            t.alpha = 0;

            t.X = 0;
            t.Y = 0;
            t.YCalc = 0;
            t.YErrorHigh = 0;
            t.YErrorLow = 0;
            t.Res = 0;
            t.RCd = 0;
            t.Ai = 0;
            t.alphaUnc = 0;
            t.Fc = 0;
            t.Fc2 = 0;
            t.Cd = 0;
            t.Cd2 = 1;

            if (t.ToDoRow != null) t.use = t.ToDoRow.use;
        }

        protected static void DoQ0OrCdRatioNormal(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            p.RCd = (p.Asp / p.Asp2);   //normal
        }

        protected static void DoQoOrk0OrCdRatioOptimized(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                IEnumerable<LINAA.ToDoResAvgRow> avgres = p.GetToDoResAvgRows();

                foreach (LINAA.ToDoResAvgRow gc in avgres)
                {
                    LINAA.ToDoResAvgRow aux2 = gc;
                    StDev(ref aux2);
                }
                avgres = avgres.Where(c => !c.HasErrors).ToList();
                if (avgres.Count() == 0)
                {
                    p.RowError = "No Child Rows without ERRORS were found!!! Check the X values for this combination in the Select/Reject Data";
                    p.use = false;
                }
                else
                {
                    avgres = avgres.Where(c => c.use).ToList();
                    LINAA.ToDoResAvgRow Achild = avgres.FirstOrDefault();
                    if (p.use)
                    {
                        if (Achild != null) p.use = true;
                        else p.use = false;
                    }
                    p.n = avgres.Count();
                    if (p.n == 0)
                    {
                        p.RowError = "n is null";
                        p.use = false;
                    }
                    else
                    {
                        double[] WAvgs = new double[] { 0, 0, 0 };
                        WAvgs = MyMath.WAverageStDeV(avgres, "X", "SD", "use", true);
                        p.RCd = WAvgs[0];
                        p.SD = WAvgs[1];
                        if (p.use) p.SD /= Math.Sqrt(p.n);
                        p.ObsSD = WAvgs[2];
                        p.unc = p.SD * 0.5;
                        p.unc2 = p.unc;
                    }
                }
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected static void Dok0DeterminationOneNormal(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                //this executes after Asp function calculated the Asp and Asp2 factors...

                //RCD is not what it normally is here...
                if (p.Asp2 == 0) throw new SystemException("Asp2 is 0. It will go to infinity");

                p.RCd = (p.Asp / p.Asp2) * (p.k0 / p.k02);  //this is ratios of activities withouth k0s!
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected static void Dok0DeterminationTwo(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                LINAA.ToDoAvgRow t = p.ToDoAvgRow;   //the isotope parent

                //for sample
                double R = 0;
                double frac = 0;

                if (t.sample.CompareTo(p.sample) == 0)
                {
                    R = t.R0; //is NAA
                    frac = 1 - Math.Pow(t.RCd * t.Cd2, -1);
                }
                else if (t.sample2.CompareTo(p.sample) == 0)
                {
                    R = t.R02; //is ENAA
                    frac = t.RCd - Math.Pow(t.Cd2, -1);
                }

                //for reference
                double refeR = 0;

                LINAA.ToDoDataRow aux = p;

                LINAA.ToDoAvgRow refeNAA = X.References.FirstOrDefault(o => o.sample.CompareTo(aux.sample2) == 0 && o.Iso.CompareTo(aux.Iso2) == 0);
                LINAA.ToDoAvgRow refeENAA = X.References.FirstOrDefault(o => o.sample2.CompareTo(aux.sample2) == 0 && o.Iso2.CompareTo(aux.Iso2) == 0);

                double fracrefe = 0;

                if (refeNAA != null)
                {
                    fracrefe = 1 - Math.Pow(refeNAA.RCd * refeNAA.Cd2, -1);
                    fracrefe /= refeNAA.Gt;
                    refeR = refeNAA.R0;
                }
                else if (refeENAA != null)
                {
                    fracrefe = refeENAA.RCd - Math.Pow(refeENAA.Cd2, -1);
                    fracrefe /= refeENAA.Gt;
                    refeR = refeENAA.R02;
                }
                //this will be a k0 Value!!!!
                double Rates = (refeR / R); //conventional way

                double Rates2 = frac / fracrefe;				 // f and Qo free way...  //THIS IS THE GOOD ONE

                p.Asp = Rates;		 //for comparision
                p.Asp2 = Rates2;		 //for comparision

                p.Ai = p.RCd * Rates;
                p.AiCd = p.RCd * Rates2;
                p.RCd *= 1e6;

                //there could be a second one base on CD!!!

                p.dk0 = (p.Ai / p.k0) - 1;

                p.dk0 *= 100;
                p.dk0Cd = (p.AiCd / p.k0) - 1;
                p.dk0Cd *= 100;
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected static void DofAlphaBare(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                p.RCd = (p.Asp / p.Asp2);   //trust this, data has been previously filtered to make sure NAA vs ENAA
                p.Ai = (p.RCd - 1) * p.Asp2;
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected static void DofAlphaCdCovered(ref LINAA.ToDoDataRow p, ref ToDoArgs X)
        {
            try
            {
                p.RCd = p.Asp;
            }
            catch (SystemException ex)
            {
                p.use = false;
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        protected static void DofAlphaBare(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            if (t.HasErrors) return;

            //t.Rcd here is just Asp/Asp2 so no physical meaning other than just a ratio...
            //here it will be used as auxiliar
            //since Asp (here) = Asp/(e*k) as stated by DeCorte
            double Qo1 = t.qoalpha + t.Calpha;
            double Qo2 = t.qo2alpha + t.Calpha;

            double qg1 = (Qo1 * t.Ge);
            double qg2 = (Qo2 * t.Ge2);
            double one = (qg1 / t.Gt);
            double two = (qg2 / t.Gt2);
            //t.Ai == Asp2, therefore
            //use this formula, based only on RCd
            //( RCd - 1) * Asp2 ) = Asp - Asp2 because RCd = (Asp/Asp2)
            t.Ai = ((t.RCd - 1) * t.Ai) / (one - two);
            //Determine f
            t.f = (qg1 - (qg2 * t.RCd));
            t.f = t.f / ((t.RCd * t.Gt2) - t.Gt);
            t.f = Math.Abs(t.f);
        }

        /// <summary>
        /// When returns, Qo1 contains alpha and Qo2 is the original one without alpha...
        /// </summary>
        /// <param name="t"></param>
        protected static void DofAlphaCdCovered(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            if (t.HasErrors) return;

            double Qoalpha = t.qoalpha + t.Calpha;
            //  t.Qo2 = t.qoalpha + t.Calpha;
            double qg1 = (Qoalpha * t.Ge);

            t.Ai = t.RCd / (qg1 * t.Cd);
            //take f =0 (Cadmium
            t.f = 0;
        }

        /// <summary>
        /// When returns, Qo1 contains alpha and Qo2 is the original one without alpha...
        /// </summary>
        /// <param name="t"></param>
        protected static void DofAlphaCdRatio(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            if (t.HasErrors) return;

            double Qoalpha = t.qoalpha + t.Calpha;

            double GeCdtoGe = (t.Ge2 / t.Ge);
            //t.Qo2 will contain the Qo1 withouth alpha cuz is the same isotope
            double denom = t.Cd2 * t.RCd * GeCdtoGe;
            denom -= 1.0;
            double aux = denom * Qoalpha;
            double gte = (t.Gt / t.Ge);

            t.Ai = (gte / aux);
            //take f
            t.f = (1 / t.Ai);
        }

        /// <summary>
        /// When returns, Qo1 and Qo2 does not contain alpha, while Ai = Qo(alpha) does. Qo1 contains the new value!!!
        /// </summary>
        /// <param name="t"></param>
        protected static void DoQoDetermination(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            if (t.HasErrors) return;

            t.Qo1 = t.Qo2; //take the original one cuz I wont change Qo2...

            if (!t.ToDoRow._ref)  //dont determine Qo for reference!! this will be the ultimate comparator!!
            {
                //t.Ai is Q0(alpha)
                double GeCdtoGe = (t.Ge2 / t.Ge);
                double denom = t.Cd2 * t.RCd * GeCdtoGe;
                denom -= 1.0;
                double GtGe = (t.Gt / t.Ge);
                t.Ai = GtGe * t.f * Math.Pow(denom, -1);
                //Conversion
                t.qoalpha = t.Ai - t.Calpha;
                // now Q0!!!
                t.Qo1 = t.qoalpha * Math.Pow(t.Er, t.alpha);
                t.Qo1 += 0.429;
            }
            else
            {
                t.f = ((t.Cd2 * t.RCd) - 1) * (t.Ge / t.Gt) * (t.qoalpha + t.Calpha);
                t.Ai = t.qoalpha + t.Calpha;
            }
            //so now compare Ai to the original Qo...
            t.dQo = (t.Qo1 / t.Qo2) - 1; //positive means the new value EXCEEDS the old one in that percentage --> the old one was understimated
            t.dQo *= 100;
        }

        /*
          protected static void Dok0Determination(ref ToDoDataRow p)
          {
              try
              {
                  LINAA.ToDoAvgRow r = (p.Table.DataSet as LINAA).ToDoAvg.FirstOrDefault(o => o.ToDoRow._ref);  //ultimate comparator (AU)
                  //Find Parent Isotope

                  //t is for instance the Al

                  //r is for instance, the first reference...

                  //a TodoData row must have a parent Iso A vs Iso A (CD),
                  //for a Todo Data Al vs Au with a given todoNr
                  //Al must have a Al Iso paren vs Al Iso CD
                  ///

                  LINAA.ToDoAvgRow t = p.ToDoAvgRow;   //the isotope parent

                  // t.Qo1 = t.Qo2;
                  //  r.Qo1 = r.Qo2;

                  //Refresh the total reaction rates
                  //determine reaction rates based on new f and Qo(alpha)
                  double QoalphaMon = t.qoalpha + t.Calpha;
                  double QoalphaRes = r.qoalpha + r.Calpha;

                  t.R0 = (t.Gt * t.f) + (t.Ge * QoalphaMon);
                  t.R02 = t.Cd2 * t.Ge2 * QoalphaMon;

                  r.R0 = (r.Gt * r.f) + (r.Ge * QoalphaRes);
                  r.R02 = r.Cd2 * r.Ge2 * QoalphaRes;

                  //RCD is not what it normally is here...
                  p.RCd = (p.Asp / p.Asp2) * (p.k0 / p.k02);  //this is ratios of activities withouth k0s!

                  //this will be a k0 Value!!!!
                  p.Ai = p.RCd * (r.R0 / t.R0);

                  //there could be a second one base on CD!!!

                  p.dk0 = (p.Ai / p.k0) - 1;
                  p.dk0 *= 100;
              }
              catch (SystemException ex)
              {
                  p.use = false;
                  Rsx.Dumb.SetRowError(p, ex);
              }
          }
           */

        protected static void Dok0Determination(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            try
            {
                if (t.HasErrors) return;

                double QoalphaMon = t.qoalpha + t.Calpha;

                t.R0 = (t.Gt * t.f) + (t.Ge * QoalphaMon);
                t.R02 = t.Cd2 * t.Ge2 * QoalphaMon;
            }
            catch (SystemException ex)
            {
                t.use = false;
                Rsx.Dumb.SetRowError(t, ex);
            }
        }

        public static void CalculateAi(ref LINAA.ToDoAvgRow t, ref ToDoArgs X, double f0, double alpha0)
        {
            try
            {
                t.use = t.ToDoRow.use;
                t.alpha = alpha0;
                t.f = f0;

                t.ClearErrors();

                //if not k0 determination and table was resetted...
                //it is reset after preparing
                //or after the user forced it.

                if (!X.k0 && t.qoalpha == 0)   //this executes always after reseting the values......
                {
                    GetResonanceFromScratch(ref t, ref X);
                }
                else  //k0 determ or... reset was not called, user tryin to use Er1 and Qo1 values perhaps..
                {
                    GetResonanceFromAvg(ref t, false, !X.Qo); //the Qo1 and Er1 values are used instead..
                    GetResonanceFromAvg(ref t, true, !X.Qo);
                    //the reference (the second isotope usually, the denominator) always gets original NAA data!!?
                }

                IEnumerable<LINAA.ToDoDataRow> dats = t.GetToDoDataRows();
                //a reference AvgRow in k0 mode has no CHILDS!!
                //so this is safely not entered for a reference i k0 mode
                //however, the analyte AvgRow can enter, and the functions shall be called...
                //recover the green part if this does not work.
                foreach (LINAA.ToDoDataRow r in dats)
                {
                    LINAA.ToDoDataRow aux = r;
                    //main function!!!
                    X.A.Invoke(ref aux, ref X);
                    /// //   if (!k0) a.Invoke(ref aux);
                    ///   //  else if (!t.ToDoRow._ref) a.Invoke(ref aux);
                }

                X.B.Invoke(ref t, ref X);  //main function for avgs (isotopes)

                if (X.A2 == null) return;  //k0 determination now...

                foreach (LINAA.ToDoDataRow p in dats)
                {
                    LINAA.ToDoDataRow aux = p;
                    //scond main function... k0 determination!!!!
                    X.A2.Invoke(ref aux, ref X);
                }
            }
            catch (SystemException ex)
            {
                t.use = false;
                Rsx.Dumb.SetRowError(t, ex);
            }
        }

        protected static void AvgRCd(ref LINAA.ToDoAvgRow t, ref ToDoArgs X)
        {
            IEnumerable<LINAA.ToDoDataRow> dats = t.GetToDoDataRows();
            dats = dats.Where(o => !o.HasErrors).ToList();
            if (dats.Count() == 0 && !X.k0)    //important that does not enter in k0
            {
                t.RowError = "No Child Rows without ERRORS were found!!! Check the RCd values for this combination in the Raw Data";
                t.use = false;
                return;
            }

            dats = dats.Where(c => c.use).ToList();
            t.n = dats.Count();
            if (t.n == 0) t.use = false;
            else t.use = true;
            //now I only compute the weighted average ONCE because I don't care about the ToDoData-Ais..
            //I discovered I could use their RCds for everything I need!! (I think)
            if (!X.k0)
            {
                //not related to k0

                double[] WAvgs = new double[] { 0, 0, 0 };
                WAvgs = MyMath.WAverageStDeV(dats, "RCd", "SD", "use", true);
                t.RCd = WAvgs[0];
                t.SD = WAvgs[1];
                if (t.use) t.SD /= Math.Sqrt(t.n);
                t.ObsSD = WAvgs[2];
            }

            if (X.Bare)
            {
                t.Ai = dats.Average(p => p.Asp2);	 //for f-Alpha Bare
                t.__qoa_qo2a__1 = t.qoalpha - t.qo2alpha;
                if (t.__qoa_qo2a__1 != 0) t.__qoa_qo2a__1 = (1 / t.__qoa_qo2a__1); //for f-Alpha Bare;
            }
        }

        protected internal static void CalculateXY(ref LINAA.ToDoAvgRow t, ref ToDoArgs args)
        {
            //now t.Ai has the right value to plot...
            t.X = Math.Log(t.Er);
            t.Y = t.Ai * Math.Pow(t.Er, -1 * t.alpha); //do the math trick
            t.Y = Math.Log(t.Y); //take the log
        }

        protected internal static void CheckSD<T>(ref T aRow, ref ToDoArgs args)
        {
            object aux = aRow;
            Type tipo = typeof(T);
            if (tipo.Equals(typeof(LINAA.ToDoAvgRow)))
            {
                LINAA.ToDoAvgRow p = (LINAA.ToDoAvgRow)aux;
                if (p.SD > 10)
                {
                    p.SetColumnError("SD", "Uncertainty higher than 10%");
                }
                if (p.use && !p.HasErrors) p.use = true;
                else if (p.HasErrors) p.use = false;
            }
            else if (tipo.Equals(typeof(LINAA.ToDoDataRow)))
            {
                LINAA.ToDoDataRow p = (LINAA.ToDoDataRow)aux;
                if (p.SD > 10)
                {
                    p.SetColumnError("SD", "Uncertainty higher than 10%");
                }
                if (p.use && !p.HasErrors) p.use = true;
                else if (p.HasErrors) p.use = false;
            }
            else throw new SystemException("CheckSD Notimplemented");
        }

        protected internal static void CheckRCd<T>(ref T aRow, ref ToDoArgs args)
        {
            object aux = aRow;
            Type tipo = typeof(T);
            if (tipo.Equals(typeof(LINAA.ToDoAvgRow)))
            {
                LINAA.ToDoAvgRow p = (LINAA.ToDoAvgRow)aux;
                if (p.IsRCdNull() || Double.IsNaN(p.RCd) || p.RCd == 0) p.SetColumnError("RCd", "RCd is not a double (NaN) or is null");
                if (p.use && !p.HasErrors) p.use = true;
                else if (p.HasErrors) p.use = false;
            }
            else if (tipo.Equals(typeof(LINAA.ToDoDataRow)))
            {
                LINAA.ToDoDataRow p = (LINAA.ToDoDataRow)aux;
                if (p.IsRCdNull() || Double.IsNaN(p.RCd) || p.RCd == 0) p.SetColumnError("RCd", "RCd is not a double (NaN) or is null");
                if (p.use && !p.HasErrors) p.use = true;
                else if (p.HasErrors) p.use = false;
            }
            else throw new SystemException("CheckRCd Notimplemented");
        }
    }
}

namespace DB.Tools
{
    public partial class ToDo
    {
        public static IEnumerable<LINAA.PeaksRow> PropagateSR(ref IEnumerable<LINAA.ToDoResAvgRow> ravgs)
        {
            HashSet<LINAA.PeaksRow> tosave = new HashSet<LINAA.PeaksRow>();
            foreach (LINAA.ToDoResAvgRow ravg in ravgs)
            {
                IEnumerable<LINAA.ToDoResRow> reses = ravg.GetToDoResRows();
                bool use = ravg.use;
                foreach (LINAA.ToDoResRow res in reses)
                {
                    try
                    {
                        LINAA.PeaksRow p1 = res.PeaksRowParentByPeaks_TDRes;
                        LINAA.PeaksRow p2 = res.PeaksRowParentByPeaks_TDRes2;
                        if (use)
                        {
                            p1.ID = Math.Abs(p1.ID);
                            p2.ID = Math.Abs(p2.ID);
                        }
                        else
                        {
                            p1.ID = -1 * Math.Abs(p1.ID);
                            p2.ID = -1 * Math.Abs(p2.ID);
                        }

                        tosave.Add(p1);
                        tosave.Add(p2);
                    }
                    catch (SystemException ex)
                    {
                        Rsx.Dumb.SetRowError(res, ex);
                    }
                }
            }

            return tosave.ToList();
        }

        public static string CalculateFCs(LINAA.ToDoType todoType, ref IEnumerable<LINAA.ToDoDataRow> rows)
        {
            if (rows.Count() == 0) return "CalculateFCs Error (ToDoData): Collection is empty";

            rows = rows.ToList();
            foreach (LINAA.ToDoDataRow p in rows)
            {
                try
                {
                    p.Fc = p.IPAvg.Fc;
                    if (todoType != LINAA.ToDoType.fAlphaCdCovered) // Alpha Cd-Covered
                    {
                        try
                        {
                            p.Fc2 = p.IPAvg2.Fc;
                        }
                        catch (SystemException ex)
                        {
                            p.use = false;
                            p.SetColumnError("Fc2", ex.Message);
                        }
                    }
                }
                catch (SystemException ex)
                {
                    p.use = false;
                    p.SetColumnError("Fc", ex.Message);
                }
            }

            return "CalculateFC (ToDoData) was OK";
        }

        public static string CalculateFCs(LINAA.ToDoType todoType, ref IEnumerable<LINAA.ToDoAvgRow> rows)
        {
            if (rows.Count() == 0) return "CalculateFCs Error (ToDoAvg): Collection is empty";
            rows = rows.ToList();
            foreach (LINAA.ToDoAvgRow t in rows)
            {
                try
                {
                    IEnumerable<LINAA.ToDoDataRow> Allchilds = t.GetToDoDataRows();
                    IEnumerable<LINAA.ToDoDataRow> childs = Allchilds.Where(o => !o.HasErrors);
                    if (childs.Count() == 0) t.use = false;
                    else
                    {
                        t.use = childs.First().use;
                        //   if (t.use)
                        //  {
                        //i'm using the column names because they are common to bot parent and child

                        double[] WAvgs = MyMath.WAverageStDeV(childs, "Fc", "unc", "use", t.use);
                        t.Fc = WAvgs[0];
                        WAvgs = MyMath.WAverageStDeV(childs, "Fc2", "unc2", "use", t.use);
                        t.Fc2 = WAvgs[0];

                        //  }
                    }
                }
                catch (SystemException ex)
                {
                    t.use = false;
                    Rsx.Dumb.SetRowError(t, ex);
                }
            }

            return "CalculateFCs (ToDoAvg) was OK";
        }

        public static double[] GetAvgsfAlpha(LINAA.ToDoType todoType, LINAA.ToDoAvgDataTable table)
        {
            double avgLogEr = 0;
            double avgqo_qo2a_1 = 0;
            double avgQoa_1 = 0;

            if (todoType != LINAA.ToDoType.Q0determination && todoType != LINAA.ToDoType.k0determination)
            {
                //SECOND PART, DETERMINING ERRORS
                string filter = table.useColumn.ColumnName;
                avgLogEr = 0;
                IList<double> IavgLogEr = MyMath.ListFrom(table.XColumn, 1, filter, true);
                avgLogEr = IavgLogEr.Average();
                avgqo_qo2a_1 = 0;
                if (todoType.Equals(LINAA.ToDoType.fAlphaBare))
                {
                    avgqo_qo2a_1 = MyMath.ListFrom(table.__qoa_qo2a__1Column, 1, filter, true).Average();
                }
                avgQoa_1 = 0;
                avgQoa_1 = MyMath.ListFrom(table.qoalphaColumn, -1, filter, true).Average();
            }

            return new double[] { avgLogEr, avgqo_qo2a_1, avgQoa_1 };
        }

        public static void GetUncfAlpha(LINAA.ToDoType tocalculate, double[] args, LINAA.ToDoAvgUncDataTable table)
        {
            double avgLogEr = args[0];
            double avgqo_qo2a_1 = args[1];
            double avgQoa_1 = args[2];

            string filter = table.useColumn.ColumnName;

            IEnumerable<LINAA.ToDoAvgUncRow> rows = table.AsEnumerable();
            if (tocalculate == LINAA.ToDoType.fAlphaBare)
            {
                foreach (LINAA.ToDoAvgUncRow t in rows)
                {
                    try
                    {
                        //  if (t.use)
                        {
                            LINAA.ToDoAvgRow a = t.ToDoAvgRow;
                            t.Bi = ((a.qoalpha * Math.Log(a.Er)) - (a.qo2alpha * Math.Log(a.Er2)));
                            t.Bi = t.Bi * a.__qoa_qo2a__1;
                        }
                    }
                    catch (SystemException ex)
                    {
                        Rsx.Dumb.SetRowError(t, ex);
                    }
                }
                double avgBi = 0;
                avgBi = MyMath.ListFrom(table.BiColumn, 1, table.useColumn.ColumnName, true).Average();
                int used = 0;

                foreach (LINAA.ToDoAvgUncRow r in rows)
                {
                    try
                    {
                        //  if (r.use)
                        {
                            LINAA.ToDoAvgRow a = r.ToDoAvgRow;
                            r.Ji2 = ((a.X - avgLogEr) * (a.__qoa_qo2a__1 - avgqo_qo2a_1));
                            r.Ji = ((a.X - avgLogEr) * (r.Bi - avgBi));
                            used++;
                        }
                    }
                    catch (SystemException ex)
                    {
                        Rsx.Dumb.SetRowError(r, ex);
                    }
                }

                double avgJi = 0;
                double avgJi2 = 0;

                string use = table.useColumn.ColumnName;

                avgJi = MyMath.ListFrom(table.JiColumn, 1, use, true).Average();
                avgJi2 = MyMath.ListFrom(table.Ji2Column, 1, use, true).Average();

                GetUncfAlphaBare(avgJi, avgJi2, used, avgLogEr, ref rows);
                return;
            }
            if (tocalculate == LINAA.ToDoType.fAlphaCdCovered || tocalculate == LINAA.ToDoType.fAlphaCdRatio) //cd-covered
            {
                foreach (LINAA.ToDoAvgUncRow t in rows)
                {
                    try
                    {
                        //  if (t.use)
                        {
                            LINAA.ToDoAvgRow a = t.ToDoAvgRow;
                            t.Vi = ((a.qoalpha / a.Qo1) * Math.Log(a.Er));
                            t.Vi = t.Vi + (((0.26 * a.Calpha) / a.Qo1) * ((1.67 / (a.alpha + 0.5)) - 1));
                        }
                    }
                    catch (SystemException ex)
                    {
                        Rsx.Dumb.SetRowError(t, ex);
                    }
                }

                double avgVi = 0;

                avgVi = MyMath.ListFrom(table.ViColumn, 1, filter, true).Average();
                int used = 0;
                foreach (LINAA.ToDoAvgUncRow r in rows)
                {
                    try
                    {
                        // if (r.use)
                        {
                            LINAA.ToDoAvgRow a = r.ToDoAvgRow;

                            r.Ui = ((a.X - avgLogEr) * (r.Vi - avgVi));
                            r.Wi = ((a.X - avgLogEr) * (Math.Pow(a.qoalpha, -1) - avgQoa_1));
                            used++;
                        }
                    }
                    catch (SystemException ex)
                    {
                        Rsx.Dumb.SetRowError(r, ex);
                    }
                }
                double avgWi = 0;
                double avgUi = 0;
                avgUi = MyMath.ListFrom(table.UiColumn, 1, filter, true).Average();
                avgWi = MyMath.ListFrom(table.WiColumn, 1, filter, true).Average();

                if (tocalculate == LINAA.ToDoType.fAlphaCdCovered)
                {
                    GetUncfAlphaCdCovered(avgLogEr, used, avgWi, avgUi, ref rows);
                }
                else
                {
                    GetUncfAlphaCdRatio(avgLogEr, used, avgWi, avgUi, ref rows);
                }
            }

            return;
        }

        protected internal static void GetUncfAlphaBare(double avgJi, double avgJi2, int used, double avgLogEr, ref IEnumerable<LINAA.ToDoAvgUncRow> rows)
        {
            foreach (LINAA.ToDoAvgUncRow r in rows)
            {
                try
                {
                    // if (r.use)
                    {
                        LINAA.ToDoAvgRow a = r.ToDoAvgRow;
                        r.ZaAsp = 0.434 * (a.f + a.Qo1);
                        r.ZaAsp = r.ZaAsp * Math.Abs((a.X - avgLogEr) * a.__qoa_qo2a__1 * Math.Pow((avgJi * used * a.alpha), -1));

                        r.ZaAsp2 = 0.434 * (a.f + a.Qo2);
                        r.ZaAsp2 = r.ZaAsp2 * Math.Abs(avgJi2 * Math.Pow(a.alpha * avgJi, -1));

                        r.ZaEr = a.qoalpha * Math.Abs(a.alpha); // first ZEr calculation, including uncertainty in Er
                        r.ZaEr = (r.ZaEr * r.ZaAsp) * Math.Pow((a.f + a.Qo1), -1);

                        r.ZaEr2 = a.qo2alpha * Math.Abs(a.alpha); //frist ZEr2 calculation, including uncertainty in Er2
                        r.ZaEr2 = (r.ZaEr2 * r.ZaAsp2) * Math.Pow((a.f + a.Qo2), -1);

                        r.ZaECd = 0;

                        r.ZaQo = (a._QoEr_a * r.ZaAsp) * Math.Pow((a.f + a.Qo1), -1);
                        r.ZaQo2 = (a._Qo2Er2_a * r.ZaAsp2) * Math.Pow((a.f + a.Qo2), -1);

                        r.ZfQo = Math.Abs(a._QoEr_a * (1 + (a.Qo2 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));
                        r.ZfQo2 = Math.Abs(a._Qo2Er2_a * (1 + (a.Qo1 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));

                        r.Zfa = (((a.f + a.Qo1) * a.qo2alpha * Math.Log(a.Er2)) - ((a.f + a.Qo2) * a.qoalpha * Math.Log(a.Er)));
                        r.Zfa = (r.Zfa * Math.Pow(a.Qo1 - a.Qo2, -1)) + (0.60 * a.Calpha * ((1.67 / (a.alpha + 0.5)) - 1));
                        r.Zfa = Math.Abs(r.Zfa * (a.alpha / a.f));

                        r.ZfEr = Math.Abs(a.alpha * a.qoalpha * (1 + (a.Qo2 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));
                        r.ZfEr2 = Math.Abs(a.alpha * a.qo2alpha * (1 + (a.Qo1 / a.f)) * Math.Pow(a.Qo1 - a.Qo2, -1));
                        r.ZfAsp = Math.Abs((a.f + a.Qo1) * (a.f + a.Qo2) * Math.Pow(a.f * (a.Qo1 - a.Qo2), -1));

                        a.alphaUnc = Math.Pow(r.ZaAsp * a.SD * 0.5, 2) + Math.Pow(r.ZaAsp2 * a.SD * 0.5, 2);

                        //fixed accuracy (nuclear data uncertainties)

                        a.alphaUnc += Math.Pow(r.ZaECd * 15, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr * a.ErUnc, 2) + Math.Pow(r.ZaQo * a.QoUnc, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr2 * a.Er2Unc, 2) + Math.Pow(r.ZaQo2 * a.Qo2Unc, 2);
                        a.alphaUnc = Math.Sqrt(a.alphaUnc);

                        a.fUnc = Math.Pow(r.Zfa * a.alphaUnc, 2) + Math.Pow(r.ZfAsp * a.SD, 2);
                        a.fUnc += Math.Pow(r.ZfEr * a.ErUnc, 2) + Math.Pow(r.ZfQo * a.QoUnc, 2);
                        a.fUnc += Math.Pow(r.ZfEr2 * a.Er2Unc, 2) + Math.Pow(r.ZfQo2 * a.Qo2Unc, 2);
                        a.fUnc = Math.Sqrt(a.fUnc);
                    }
                }
                catch (SystemException ex)
                {
                    Rsx.Dumb.SetRowError(r, ex);
                }
            }
        }

        protected internal static void GetUncfAlphaCdCovered(double avgLogEr, int used, double avgWi, double avgUi, ref IEnumerable<LINAA.ToDoAvgUncRow> rows)
        {
            foreach (LINAA.ToDoAvgUncRow r in rows)
            {
                try
                {
                    //  if (r.use)
                    {
                        LINAA.ToDoAvgRow a = r.ToDoAvgRow;
                        r.ZaAsp = 0.434 * Math.Abs((a.X - avgLogEr) * Math.Pow((a.alpha * avgUi * used), -1));
                        r.ZaAsp2 = 0;

                        r.ZaEr = a.qoalpha * Math.Abs(a.alpha); // first ZEr calculation, including uncertainty in Er
                        r.ZaEr = r.ZaEr * r.ZaAsp * Math.Pow(a.qoalpha, -1);
                        r.ZaEr2 = 0;

                        r.ZaECd = 0.434 * a.Calpha * (a.alpha + 0.5); // first ZaECd calculation
                        r.ZaECd = r.ZaECd * Math.Abs(Math.Pow(a.alpha, -1) * (avgWi / avgUi)); //second ZaECd calculation

                        r.ZaQo = a._QoEr_a; //first ZaQo first calculation, including uncertainty in Qo
                        r.ZaQo = r.ZaQo * r.ZaAsp * Math.Pow(a.qoalpha, -1);
                        r.ZaQo2 = 0;

                        r.ZfEr2 = 0;
                        r.ZfQo2 = 0;
                        r.ZfAsp = 0;
                        r.ZfEr = 0;
                        r.ZfQo = 0;

                        a.alphaUnc = Math.Pow(r.ZaAsp * a.SD, 2);

                        //fixed accuracy (nuclear data uncertainties)

                        a.alphaUnc += Math.Pow(r.ZaECd * 15, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr * a.ErUnc, 2) + Math.Pow(r.ZaQo * a.QoUnc, 2);

                        a.alphaUnc = Math.Sqrt(a.alphaUnc);
                    }
                }
                catch (SystemException ex)
                {
                    Rsx.Dumb.SetRowError(r, ex);
                }
            }
        }

        protected internal static void GetUncfAlphaCdRatio(double avgLogEr, int used, double avgWi, double avgUi, ref IEnumerable<LINAA.ToDoAvgUncRow> rows)
        {
            foreach (LINAA.ToDoAvgUncRow r in rows)
            {
                try
                {
                    //if (r.use)
                    {
                        LINAA.ToDoAvgRow a = r.ToDoAvgRow;

                        r.ZaAsp = 0.434 * Math.Pow(a.f, -1) * (a.f + a.Qo1);
                        r.ZaAsp = r.ZaAsp * Math.Abs((a.X - avgLogEr) * Math.Pow((a.alpha * avgUi * used), -1));
                        r.ZaAsp2 = 0;

                        r.ZaEr = a.qoalpha * Math.Abs(a.alpha); // first ZEr calculation, including uncertainty in Er
                        r.ZaEr = (r.ZaEr * r.ZaAsp * Math.Pow(a.qoalpha, -1) * a.f) * Math.Pow((a.f + a.Qo1), -1);
                        r.ZaEr2 = 0;

                        r.ZaECd = 0.434 * a.Calpha * (a.alpha + 0.5); // first ZaECd calculation
                        r.ZaECd = r.ZaECd * Math.Abs(Math.Pow(a.alpha, -1) * (avgWi / avgUi)); //second ZaECd calculation

                        r.ZaQo = a._QoEr_a; //first ZaQo first calculation, including uncertainty in Qo
                        r.ZaQo = (r.ZaQo * r.ZaAsp * Math.Pow(a.qoalpha, -1) * a.f) * Math.Pow((a.f + a.Qo1), -1);
                        r.ZaQo2 = 0;

                        r.Zfa = Math.Abs((a.alpha / a.Qo1) * ((a.qoalpha * Math.Log(a.Er)) + (0.60 * a.Calpha * ((1.67 / (a.alpha + 0.5)) - 1))));
                        r.ZfEr = Math.Abs(a.alpha * (a.qoalpha / a.Qo1));
                        r.ZfQo = Math.Abs(a._QoEr_a / a.Qo1);
                        r.ZfAsp = 1 + (a.Qo1 / a.f);

                        r.ZfEr2 = 0;
                        r.ZfQo2 = 0;

                        a.alphaUnc = Math.Pow(r.ZaAsp * 0.5 * a.SD, 2) + Math.Pow(r.ZaAsp2 * 0.5 * a.SD, 2);
                        //fixed accuracy (nuclear data uncertainties)

                        a.alphaUnc += Math.Pow(r.ZaECd * 15, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr * a.ErUnc, 2) + Math.Pow(r.ZaQo * a.QoUnc, 2);
                        a.alphaUnc += Math.Pow(r.ZaEr2 * a.Er2Unc, 2) + Math.Pow(r.ZaQo2 * a.Qo2Unc, 2);
                        a.alphaUnc = Math.Sqrt(a.alphaUnc);

                        a.fUnc = Math.Pow(r.Zfa * a.alphaUnc, 2) + Math.Pow(r.ZfAsp * a.SD, 2);
                        a.fUnc += Math.Pow(r.ZfEr * a.ErUnc, 2) + Math.Pow(r.ZfQo * a.QoUnc, 2);
                        a.fUnc += Math.Pow(r.ZfEr2 * a.Er2Unc, 2) + Math.Pow(r.ZfQo2 * a.Qo2Unc, 2);
                        a.fUnc = Math.Sqrt(a.fUnc);
                    }
                }
                catch (SystemException ex)
                {
                    Rsx.Dumb.SetRowError(r, ex);
                }
            }
        }

        public static double GetResiduals(ref IEnumerable<LINAA.ToDoAvgRow> rows)
        {
            DateTime start = DateTime.Now;
            foreach (LINAA.ToDoAvgRow r in rows)
            {
                try
                {
                    r.YErrorHigh = r.Y + (r.Y * (r.SD / 100));
                    r.YErrorLow = r.Y - (r.Y * (r.SD / 100));
                }
                catch (SystemException ex)
                {
                    r.use = false;
                    Rsx.Dumb.SetRowError(r, ex);
                }
            }

            return (DateTime.Now - start).TotalSeconds;
        }

        public static string Push(ref LINAA.ToDoAvgDataTable clone, ref IEnumerable<LINAA.ToDoAvgRow> rows, string newColBase, ref DataColumn[] arrOfColToPush)
        {
            rows = rows.ToList();
            if (rows.Count() == 0) return "SetValues Error: Collection is empty";

            try
            {
                //for overriding f and alpha with reference when Qo determination

                foreach (LINAA.ToDoAvgRow avg in rows)
                {
                    foreach (DataColumn col in arrOfColToPush)
                    {
                        LINAA.ToDoAvgRow cl = clone.FirstOrDefault(c => c.ToDoNr == avg.ToDoNr);
                        if (Rsx.Dumb.IsNuDelDetch(cl)) continue;
                        object valueToPush = avg.Field<object>(col.ColumnName);
                        cl.SetField<object>(col.ColumnName + "." + newColBase, valueToPush);
                    }
                }
            }
            catch (SystemException ex)
            {
                string msg = ex.Message;
            }
            return "Push was OK";
        }

        protected internal static void OverridefAlpha(ref LINAA.ToDoAvgRow t, double f0, double alpha)
        {
            t.f = f0;
            t.alpha = alpha;
        }
    }
}