using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;
using Rsx.Math;

namespace DB.Tools
{
    public partial class WC
    {
        public static double Weight(ref LINAA.IPeakAveragesRow i)
        {
            i.w = 0;
            i.w2 = 0;
            i.wX = 0;
            i.wX2 = 0;
            i.UncSq = 0;
            if (i.Flag == 1)
            {
                i.UncSq = i.SD * i.SD;
                i.w = (1.0 / i.UncSq);
                i.w2 = i.w * i.w;
                i.wX = i.w * i.Asp;
                i.wX2 = i.wX * i.Asp;
            }
            return i.w;
        }

        public static double X(ref LINAA.IPeakAveragesRow i)
        {
            i.X = 0;

            if (i.n >= 1)
            {
                IEnumerable<LINAA.PeaksRow> peaks = i.GetPeaksRows();
                // i.n = peaks.Sum(p => p.Flag);

                double Sumw = peaks.Sum(p => p.w);
                double SumwX = peaks.Sum(p => p.wX);
                if (Sumw != 0)
                {
                    i.X = (SumwX / Sumw);
                }
            }

            return i.X;
        }

        public static double StDev(ref LINAA.IPeakAveragesRow i)
        {
            double aux = 0;

            if (i.n == 1)
            {
                IEnumerable<LINAA.PeaksRow> peaks = i.GetPeaksRows();
                LINAA.PeaksRow p = peaks.FirstOrDefault();
                if (p != null)
                {
                    aux = p.UncSq;
                    aux = Math.Sqrt(aux);
                    i.GActUnc = aux;
                    aux = p.UncSq + Math.Pow(i.k0Unc, 2);
                    aux = Math.Sqrt(aux);
                    i.SD = aux;
                    i.ObsSD = 0.0;
                }
                else i.RowError = "Not child cRow found";
            }
            else if (i.n > 1 && i.X != 0.0)
            {
                IEnumerable<LINAA.PeaksRow> peaks = i.GetPeaksRows();
                double kUnc = 0.0;
                if (i.MD == 0.0) kUnc = Math.Pow(i.k0Unc, 2);
                else if (i.MD == 1) kUnc = Math.Pow(i.k0xUnc, 2);
                else kUnc = Math.Pow(i.k0xUnc, 2) + Math.Pow(i.k0Unc, 2);

                double SumwX2 = peaks.Sum(p => p.wX2);
                double Sumw2 = peaks.Sum(p => p.w2);
                double Sumw = peaks.Sum(p => p.w);
                double SumwX = peaks.Sum(p => p.wX);

                aux = (double)((Sumw * SumwX2) - (SumwX * SumwX));
                aux /= (double)((Sumw * Sumw) - Sumw2);
                aux = 100.0 * Math.Sqrt(aux);
                aux /= i.X;
                i.GActUnc = aux;

                aux = Math.Pow(i.GActUnc, 2) + kUnc;
                aux = Math.Sqrt(aux);
                i.SD = aux;

                aux = (double)((SumwX2 / Sumw));
                aux -= (i.X * i.X);
                aux = 100.0 * Math.Sqrt(aux);
                aux /= i.X;
                i.ObsSD = aux;
            }
            else
            {
                i.SD = 0.0;
                i.ObsSD = 0.0;
                i.GActUnc = 0.0;
            }

            return i.SD;
        }

        public static double StDev(ref LINAA.IRequestsAveragesRow i)
        {
            double aux = 0;

            IEnumerable<LINAA.IPeakAveragesRow> cchilds = i.GetIPeakAveragesRows();

            if (cchilds.Count() == 1)  //when there's only one gamma line for this isotope...
            {
                i.SD = cchilds.First().SD;

                i.ObsSD = cchilds.First().ObsSD;
            }

            if (i.g == 1) //... unless there are several but only 1 gamma line was not rejected
            {
                LINAA.IPeakAveragesRow ip = cchilds.FirstOrDefault(a => a.Flag == 1);
                if (ip != null)
                {
                    i.SD = ip.SD;
                    i.ObsSD = ip.SD;
                }
            }
            else if (i.g > 1 && i.Asp != 0.0)
            {
                double SumwX2 = cchilds.Sum(p => p.wX2);
                double Sumw2 = cchilds.Sum(p => p.w2);
                double Sumw = cchilds.Sum(p => p.w);
                double SumwX = cchilds.Sum(p => p.wX);

                aux = ((Sumw * SumwX2) - (SumwX * SumwX));
                aux /= ((Sumw * Sumw) - Sumw2);

                i.SD = 100.0 * Math.Sqrt(aux) / i.x;

                aux = ((SumwX2 / Sumw));
                aux -= (i.x * i.x);
                i.ObsSD = 100.0 * Math.Sqrt(aux) / i.x;
            }
            else
            {
                i.SD = 0.0;
                i.ObsSD = 0.0;
            }

            double stdev = i.SD;
            i.UncSq = stdev * stdev;

            return stdev;
        }

        public static double X(ref LINAA.IRequestsAveragesRow i)
        {
            i.Asp = 0;

            if (i.g >= 1)
            {
                IEnumerable<LINAA.IPeakAveragesRow> cchilds = i.GetIPeakAveragesRows();

                double Sumw = cchilds.Sum(p => p.w);
                double SumwX = cchilds.Sum(p => p.wX);

                // i.g = cchilds.Sum(p => p.Flag);

                if (Sumw != 0.0)
                {
                    i.x = (SumwX / Sumw);
                    i.Asp = i.x;
                }
            }

            return i.x;
        }

        protected internal static double Weight(ref LINAA.PeaksRow p)
        {
            p.w = 0.0;
            p.w2 = 0.0;
            p.wX = 0.0;
            p.wX2 = 0.0;
            p.UncSq = 0;
            if (p.Flag == 1)
            {
                p.UncSq = p.AreaUncertainty * p.AreaUncertainty;
                double ecunc2 = 1.0;
                if (p.MeasurementsRow.Position < 3)
                {
                    ecunc2 = 1.5 * 1.5;
                }
                p.UncSq += ecunc2;
                p.UncSq += 0.2 * 0.2;
                p.w = (1.0 / p.UncSq);
                p.w2 = p.w * p.w;
                p.wX = p.w * p.Y;
                p.wX2 = p.wX * p.Y;
            }
            return p.w;
        }

        public static void Calculate(ref IEnumerable<LINAA.SubSamplesRow> Samples, bool matSSF, bool chilean, bool ReDecay)
        {
            foreach (LINAA.SubSamplesRow s in Samples)
            {
                IEnumerable<LINAA.IRequestsAveragesRow> toCalc = s.GetIRequestsAveragesRows();
                Calculate(ref toCalc, matSSF, chilean);
            }
        }

        public static void Calculate(ref IEnumerable<LINAA.IRequestsAveragesRow> rows, bool matssf, bool chilean)
        {
            rows = EC.NotDeleted<LINAA.IRequestsAveragesRow>(rows);

            if (rows.Count() == 0) return;

            if (matssf && !chilean) GetMatSSF(ref rows);
            else if (chilean && !matssf) GetChilean(ref rows);

            //Finally

            foreach (LINAA.IRequestsAveragesRow r in rows)
            {
                LINAA.SubSamplesRow s = r.SubSamplesRow;
                if (s.IsAlphaNull())
                {
                    s.RowError = "Rate Module Error: Alpha cannot be null";
                    continue;
                }
                if (s.IsfNull())
                {
                    s.RowError = "Rate Module Error: fcannot be null";
                    continue;
                }

                LINAA.IRequestsAveragesRow aux = r;

                Rate(s.Alpha, s.f, ref aux);
                IEnumerable<LINAA.MeasurementsRow> meas = r.SubSamplesRow.GetMeasurementsRows();
                FindDecayTimes(ref meas);
                Temporal(ref meas, ref aux);
            }
        }

        public static void Rate(double alpha, double f, ref LINAA.IRequestsAveragesRow ir)
        {
            LINAA.IRequestsAveragesDataTable table = ir.Table as LINAA.IRequestsAveragesDataTable;

            try
            {
                //auxiliars (as functions of alpha)
                ir._Er = 1.0;
                ir._Qog = 0.0;  //ground state
                ir._Qom = 0.0;   //meta state
                ir._Qomg = 0.0;  //meta + ground state

                ir._QoUnc = 0.0;

                //auxiliars
                ir._Rg = 0.0; //rate of ground state
                ir._Rm = 0.0; //rate of meta state
                ir._Rmg = 0.0; //rate of meta+ground state

                ir._Qo = 0.0;
                ir._Qoalpha = 0.0;
                ir.Calpha = 0.0;
                ir._Cd = 1.0;    //by default
                ir.R1 = 0.0;   //for ground, ground+meta , when isotope needs alternate rate
                ir.R0 = 0.0;   //for ground  or simple decay scheme

                if (ir.NAARow == null)
                {
                    ir.RowError = "NAA data was not found. Check this Isotope/Element exist in the database";
                    return;
                }

                LINAA.NAARow n = ir.NAARow;

                if (!n.IsErNull()) ir._Er = n.Er;
                if (n.pValuesRow != null) ir._Er = ir._Er * Math.Exp(-1 * alpha * n.pValuesRow.p);
                ir.Calpha = MyMath.Calpha(alpha);
                if (ir.SubSamplesRow.ENAA)
                {
                    ir._Cd = n.Cd;
                    f = 0.0;
                }

                double Gtxf = ir.Gt * f;
                double CdxGe = ir._Cd * ir.Ge;

                if (!n.IsQogNull() && n.Qog != 0.0)
                {
                    ir._Qog = MyMath.qoalpha(alpha, n.Qog, ir._Er) + ir.Calpha; //Qalpha calculo g
                    ir._Rg = Gtxf + (CdxGe * ir._Qog);
                }
                if (!n.IsQomNull() && n.Qom != 0.0)
                {
                    ir._Qom = MyMath.qoalpha(alpha, n.Qom, ir._Er) + ir.Calpha; //Qalpha calculo m
                    ir._Rm = Gtxf + (CdxGe * ir._Qom);
                }
                if (!n.IsQomgNull() && n.Qomg != 0.0)
                {
                    ir._Qomg = MyMath.qoalpha(alpha, n.Qomg, ir._Er) + ir.Calpha; //Qalpha calculo g+m
                    ir._Rmg = Gtxf + (CdxGe * ir._Qomg);
                }

                ir._mg = false;
                ir.R0 = ir._Rg; //by default use Qog for majority of isotopes, then, modify accordingly...
                ir._Qo = n.Qog; //withouth alpha
                ir._Qoalpha = ir._Qog;
                ir._QoUnc = n.QogUnc;
                //Case I
                if (n.MD == 1)
                {
                    char m = ir.Radioisotope.Last();
                    if (m.CompareTo('m') == 0)
                    {
                        ir._mg = null;
                        ir._Qo = n.Qom;
                        ir._Qoalpha = ir._Qom;
                        ir._QoUnc = n.QomUnc;
                        ir.R0 = ir._Rm; //is a metaestable, therefore use Qom
                    }
                }
                //Case IVa/b/c/d
                else if (n.MD == 4 || n.MD == 5 || n.MD == 6 || n.MD == 7 || n.MD == 9)
                {
                    //this 7 must be VII/c, which is not in the library (see my thesis), check this!

                    ir._mg = true;
                    ir._Qo = n.Qomg;
                    ir._Qoalpha = ir._Qomg;
                    ir._QoUnc = n.QomgUnc;
                    ir.R0 = ir._Rmg;    //when IVb (or Vb/c/d) (or VIIa)
                    if (n.MD != 6)
                    {
                        ir.R1 = ir._Rg;  //  R1 = _Rg for IVa/d (or Va or VIIa)!
                                         //peaks will later have md = 0 for IVb	 (or Vb/c/d)
                                         // and md = 1 for IVa/d (or Va), since IVb (or Vb/c/d) is the natural case (D2 =0)
                    }
                }
                else if (n.MD == 8) ir.R1 = ir._Rg;

                if (!ir.Radioisotope.ToUpper().Contains("NP-239") && !ir.Radioisotope.ToUpper().Contains("U-239") && ir.Element.ToUpper().Equals("U"))
                {
                    ir._Qo = n.Qog;
                    ir._QoUnc = n.QogUnc;

                    ir._Qoalpha = ir._Qog;

                    ir.R1 = ir._Rg; //fission reaction rate
                    ir.R0 = ir._Rg;
                }

                if (Double.IsNaN(ir.R0))
                {
                    ir.SetColumnError(table.R0Column, "Activation Rate Module Problem: the Activation Rate is NaN\n");
                }
                if (Double.IsNaN(ir.R1))
                {
                    ir.SetColumnError(table.R1Column, "Activation Rate Module Problem: the Activation Rate is NaN\n");
                }
            }
            catch (SystemException ex)
            {
                EC.SetRowError(ir, table.R0Column, ex);
            }
        }

        public static void Temporal(ref IEnumerable<LINAA.MeasurementsRow> meas, ref LINAA.IRequestsAveragesRow ir)
        {
            if (ir.NAARow == null)
            {
                ir.RowError = "NAA data was not found. Check this Isotope/Element exist in the database";
                return;
            }
            if (ir.SubSamplesRow == null)
            {
                ir.RowError = "Sample data was not found. This isotope has no sample associated with it";
                return;
            }

            if (ir.SubSamplesRow.IsIrradiationTotalTimeNull())
            {
                ir.RowError = "Sample irradiation total time was not found. Please fill in the irradiation time";
                return;
            }

            foreach (LINAA.MeasurementsRow m in meas)
            {
                IEnumerable<LINAA.PeaksRow> peaks = m.GetPeaksRows();
                peaks = peaks.Intersect(ir.GetPeaksRows()).ToList();

                double irrMin = ir.SubSamplesRow.IrradiationTotalTime;
                double secsInMin = 60.0;
                double dec = (m.DecayTime / secsInMin);
                double countMin = (m.CountTime / secsInMin);

                Temporal(ref ir, ref peaks, irrMin, dec, countMin);
            }
        }

        public static void Temporal(ref LINAA.IRequestsAveragesRow ir, ref IEnumerable<LINAA.PeaksRow> peaks, double irrMin, double dec, double countMin)
        {
            LINAA.NAARow n = ir.NAARow;

            /*
            if (ir.T2 == null) ir.T2 = new System.Collections.Hashtable();
            else ir.T2.Clear();
            if (ir.T3 == null) ir.T3 = new System.Collections.Hashtable();
            else ir.T3.Clear();
            if (ir.T4 == null) ir.T4 = new System.Collections.Hashtable();
            else ir.T4.Clear();
            if (ir.T5 == null) ir.T5 = new System.Collections.Hashtable();
            else ir.T5.Clear();
            */
            double L2 = 0.0;
            double L3 = 0.0;
            double L4 = 0.0;
            double L5 = 0.0;

            double ln2 = Math.Log(2.0);

            if (n.T2 != 0.0) L2 = (ln2 / n.T2);        //auxiliar
            if (n.T3 != 0.0) L3 = (ln2 / n.T3);                  //auxiliar
            if (n.T4 != 0.0) L4 = (ln2 / n.T4);                            //auxiliar

            L5 = (ln2 / 43.2); //remove this, for In-117

            double l42 = L4 - L2;
            double l32 = L3 - L2;
            double l43 = L4 - L3;
            double l53 = L5 - L3;
            double l52 = L5 - L2;

            double l54 = L5 - L4;

            double S2 = MyMath.S(L2, irrMin);
            double S3 = MyMath.S(L3, irrMin);
            double S4 = MyMath.S(L4, irrMin);
            double S5 = MyMath.S(L5, irrMin);

            int decayMod = n.MD;

            double _T2 = S2 * MyMath.D(L2, dec) * MyMath.C(L2, countMin);        //auxiliar
            double _T3 = S3 * MyMath.D(L3, dec) * MyMath.C(L3, countMin);                  //auxiliar
            double _T4 = S4 * MyMath.D(L4, dec) * MyMath.C(L4, countMin);                            //auxiliar
            double _T5 = S5 * MyMath.D(L5, dec) * MyMath.C(L5, countMin);

            //auxiliar

            /*
              ir.T2.Add(m.MeasurementID, _T2);
              ir.T3.Add(m.MeasurementID, _T3);
              ir.T4.Add(m.MeasurementID, _T4);
              ir.T5.Add(m.MeasurementID, _T5);
              */

            // if (n.MD == 11 && n.Iso.CompareTo("In-117m") == 0) n.MD = 7; //For In-117m, scheme
            // VII/a, fix this later

            // if (n.MD == 11 && n.Iso.CompareTo("In-117m") == 0) { decayMod = 7; //For In-117m,
            // scheme VII/a, fix this later }
            double a = 0.0;
            double b = 0.0;
            double c = 0.0;
            short md = 0; //decay mode for peaks
            double gamma = 0; //another auxiliar for IV/a/d
            // double d = 0.0;
            double TFinal = 0.0;

            #region Temporal Decay Chemes

            switch (decayMod)
            {
                #region Case I or MD=1 --> Normal/Direct Activation

                case 1:

                    TFinal = _T2;
                    /*
                    foreach (LINAA.PeaksRow p in peaks)
                    {
                        p.T0 = ir.T0;
                    }*/
                    break;

                #endregion Case I or MD=1 --> Normal/Direct Activation

                #region Case IIa/b/c/d or MD =2 --> Measurement of Daughter

                case 2:
                    {
                        // F2 = 0.88 and F1 = gammaMo/gammaTc at 140keV = 0.594 so F1/F2 = 0.0675 as
                        // given by DeCorte! All other MD=2 isotopes have F1=0
                        a = 0.0;
                        b = 0.0;
                        c = 0.0;
                        a = ((L3 * _T2) - (L2 * _T3)) / l32;
                        b = (n.F1 / n.F2) * _T2;

                        TFinal = a + b;

                        /*
                        foreach (LINAA.PeaksRow p in peaks)
                        {
                            p.T0 = ir.T0;
                        }*/
                    }
                    break;

                #endregion Case IIa/b/c/d or MD =2 --> Measurement of Daughter

                #region Case IIIa/b/c or MD=3 --> i.e. Measurement of Daughter coming from Parent and metastable

                case 3:
                    {
                        //or IIIa like Nb-95
                        // IIIa is general case
                        // i.e. Nb-95 from Zr95 and Nb-95m
                        //IIIb implies F24 = F4 =0; //no isotope in library follows this scheme
                        //IIIc implies F24 = F4 = 0
                        a = 0.0;
                        b = 0.0;
                        c = 0.0;
                        double f234 = (n.F4 / n.F2 * n.F3);  // (F24/F2*F3)
                        a = _T2 * (L4 / l42) * ((L3 / l32) + f234);
                        b = _T3 * (L2 * L4) / (l43 * l32);
                        c = _T4 * (L2 / l42) * ((L3 / l43) - f234);

                        TFinal = (a + c) - b;

                        /*
                        foreach (LINAA.PeaksRow p in peaks)
                        {
                            p.T0 = ir.T0;
                        }*/
                    }
                    break;

                #endregion Case IIIa/b/c or MD=3 --> i.e. Measurement of Daughter coming from Parent and metastable

                #region Case IVa/b/d or MD=4

                case 4:
                    {
                        //For the case IVa
                        //n.F1 = (sigma_m/sigma_g )
                        //Therefore --> n.F2*n.F1 = F2(sigma_m/sigma_g) as given by DeCorte

                        // 2 (Parent) has decayed...
                        if ((dec / n.T2) > 20) md = 0;
                        else md = 1;

                        a = 0.0;
                        b = 0.0;
                        c = 0.0;

                        //For IVd, we have: Co-60 measured while Co-60m is alive
                        //We need in this special case (gamma_m/(F2*gamma_g)) = gamma
                        // For 1173 keV -->  gamma = 0
                        //For 1332.5 keV --> gamma = 2.4e-3

                        //this should be next to c, but the Qm and Qog for IVb are sometimes not complete
                        a = (n.F2 * n.F1);
                        if (a != 0.0 && ir._Rm != 0.0 && ir._Rg != 0.0)
                        {
                            a = a * (ir._Rm / ir._Rg); //Fraction meta-to-ground
                        }
                        b = ((L3 * _T2) - (L2 * _T3)) / l32;  //expression between parenthesis
                        c = _T3;

                        //continua abajo
                    }
                    break;

                #endregion Case IVa/b/d or MD=4

                #region Case Va/b/c/d or MD=5

                case 5:
                    {
                        //For the case Va
                        //n.F1 = (sigma_m/sigma_g )
                        //Therefore --> n.F2*n.F1 = F2(sigma_m/sigma_g) as given by DeCorte
                        a = 0.0;
                        b = 0.0;
                        c = 0.0;

                        // short md = 0; 2 (Parent) has decayed...
                        if ((dec / n.T2) >= 20 && (dec / n.T4) >= 20) md = 0;
                        else md = 1;

                        //this should be next to c, but the Qm and Qog for V are sometimes not complete
                        a = (n.F2 * n.F1);
                        if (a != 0 && ir._Rm != 0 && ir._Rg != 0) a = a * (ir._Rm / ir._Rg); //Fraction meta-to-ground
                        b = (_T2 * (L4 * L3) / (l42 * l32)) - (_T3 * (L4 * L2) / (l43 * l32)) + (_T4 * (L2 * L3) / (l42 * l43));
                        c = ((L4 * _T3) - (L3 * _T4)) / l43;

                        if (md == 0) a = 0; //this means... Vc !!! only T3 survives and typicallly T4 =0

                        TFinal = (a * b) + c;

                        //Finally
                        /*
                        foreach (LINAA.PeaksRow p in peaks)
                        {
                            p.MD = md;
                            p.T0 = ir.T0;
                        }
                        */
                    }
                    break;

                #endregion Case Va/b/c/d or MD=5

                #region Case VI or MD=6

                case 6:
                    {
                        // short md = 0; 2 (Parent) has decayed...
                        if ((dec / n.T2) >= 20 && (dec / n.T3) >= 20) md = 0;
                        else md = 1;

                        if (md == 0) TFinal = _T4;
                        else TFinal = 1;
                    }
                    break;

                #endregion Case VI or MD=6

                #region Case VII or MD=7

                case 7:
                    {
                        //For the case Va
                        //n.F1 = (sigma_m/sigma_g )
                        //Therefore --> n.F2*n.F1 = F2(sigma_m/sigma_g) as given by DeCorte
                        a = 0.0;
                        b = 0.0;
                        c = 0.0;
                        // short md = 0; 2 (Parent) has decayed...
                        if ((dec / n.T2) >= 20 && (dec / n.T3) >= 20) md = 0;
                        else md = 1;

                        double mg = (ir._Rm / ir._Rg);
                        //this should be next to c, but the Qm and Qog are sometimes not complete
                        a = (n.F2 * n.F1);
                        a = a * mg; //Fraction meta-to-ground

                        b = (_T2 * (L4 * L3) / (l42 * l32));
                        b = b - (_T3 * (L4 * L2) / (l43 * l32));
                        b = b + (_T4 * (L2 * L3) / (l42 * l43));

                        //F4 es F24
                        c = (n.F4 * n.F1 / n.F3);
                        c = c * mg * ((L4 * _T2) - (L2 * _T4)) / l42;
                        c = c + ((L4 * _T3) - (L3 * _T4)) / l43;

                        if (md == 0) //Case VII/c? surely, but check
                        {
                            a = 1.0;
                            c = 0;
                            b = _T4;
                        }

                        TFinal = (a * b) + c;

                        /*
                        foreach (LINAA.PeaksRow p in peaks)
                        {
                            p.MD = md;
                            p.T0 = ir.T0;
                        }
                        */
                    }
                    break;

                #endregion Case VII or MD=7

                #region Case VIII or MD=8

                case 8:
                    {
                        //For the case Va
                        //n.F1 = (sigma_m/sigma_g )
                        //Therefore --> n.F2*n.F1 = F2(sigma_m/sigma_g) as given by DeCorte
                        a = 0.0;
                        b = 0.0;
                        c = 0.0;

                        // short md = 1;

                        // 2 (Parent) has decayed... if ((dec / n.T2) >= 20 && (dec / n.T3) >= 20) md
                        // = 0; else md = 1;

                        double mg = (ir._Rm / ir._Rg);
                        //this should be next to c, but the Qm and Qog are sometimes not complete
                        a = (n.F1);
                        a = a * mg; //Fraction meta-to-ground
                        a = a / n.F3;

                        double f4 = 0.471; // In-117 is the only one with this scheme
                                           // f4 here is F4, while in the database F4 plays always the role of F24, since In-117 is the
                                           //only isotope that needs F4.
                        double f25 = 0; // I dont know, it seems it is not 0 but we dont know
                        double f35 = 0;    // I dont know, it seems it is not 0 but we dont know

                        double delta = f25 / (n.F4 * f4);
                        double delta2 = f35 / (n.F3 * f4);

                        b = (_T2 * L5 / l52) * ((L4 / l42) + (delta));
                        b = b - (_T4 * (L5 * L2) / (l54 * l42));
                        b = b + ((_T5 * L2 / l52) * ((L4 / l54) - (delta)));

                        c = (_T3 * L5 / l53) * ((L4 / l43) + (delta2));
                        c = c - (_T4 * (L5 * L3) / (l54 * l43));
                        c = c + ((_T5 * L3 / l53) * ((L4 / l54) - (delta2)));

                        c = (c / n.F4);
                        /*
                        b = (_T2 * (L4 * L3) / (l42 * l32));
                        b = b - (_T3 * (L4 * L2) / (l43 * l32));
                        b = b + (_T4 * (L2 * L3) / (l42 * l43));
                        */

                        //F4 es F24

                        /*
                       if (md == 0) //Case VIII/c does not exist
                       {
                           a = 1.0;
                           c = 0;
                           b = _T4; //check this
                       }
                       */

                        TFinal = (a * b) + c;

                        /*
                        foreach (LINAA.PeaksRow p in peaks)
                        {
                            p.MD = md;
                            p.T0 = ir.T0;
                        }*/
                    }
                    break;

                #endregion Case VIII or MD=8

                #region Case  or MD=9

                case 9:
                    {
                        //For the case Va
                        //n.F1 = (sigma_m/sigma_g )
                        //Therefore --> n.F2*n.F1 = F2(sigma_m/sigma_g) as given by DeCorte
                        a = 0.0;
                        b = 0.0;
                        c = 0.0;
                        // short md = 0; 2 (Parent) has decayed...
                        if ((dec / n.T2) >= 20 && (dec / n.T3) >= 20) md = 0;
                        else md = 1;

                        double mg = (ir._Rm / ir._Rg);
                        //this should be next to c, but the Qm and Qog for V are sometimes not complete
                        a = (n.F2 * n.F1);
                        a = a * mg; //Fraction meta-to-ground

                        b = (_T2 * (L4 * L3) / (l42 * l32));
                        b = b - (_T3 * (L4 * L2) / (l43 * l32));
                        b = b + (_T4 * (L2 * L3) / (l42 * l43));

                        c = (n.F4 * n.F1);
                        c = c * mg * ((L4 * _T2) - (L2 * _T4)) / l42;
                        c = c + n.F3 * ((L4 * _T3) - (L3 * _T4)) / l43 + _T4;

                        if (md == 0)
                        {
                            a = 1.0;
                            c = 0;
                            b = _T4;
                        }

                        TFinal = (a * b) + c;
                        /*
                        foreach (LINAA.PeaksRow p in peaks)
                        {
                            p.MD = md;
                            p.T0 = ir.T0;
                        }
                        */
                    }
                    break;

                    #endregion Case  or MD=9
            }

            foreach (LINAA.PeaksRow p in peaks) p.MD = md;

            if (decayMod != 4)
            {
                foreach (LINAA.PeaksRow p in peaks)
                {
                    p.T0 = TFinal;
                }
            }
            else
            {
                foreach (LINAA.PeaksRow p in peaks)
                {
                    // p.MD = md;

                    if (md == 1)  //  IVa/d
                    {
                        if (p.Iso.CompareTo("Co-60") == 0)
                        {
                            //if (p.Energy == 1172.3) gamma = n.F3;
                            if (p.Energy == 1332.5) gamma = n.F4;
                        }
                        else if (p.Iso.CompareTo("Rh-104") == 0)
                        {
                            if (p.Energy == 555.8) gamma = n.F4;
                        }
                    }
                    else a = 0;
                    TFinal = (a * (b + (gamma * _T2))) + c;
                    //Finally
                    p.T0 = TFinal;
                }
            }

            #endregion Temporal Decay Chemes

            foreach (LINAA.PeaksRow p in peaks)
            {
                LINAA.PeaksRow aus = p;
                CheckT0(ref aus);
            }

            ir._T0 = TFinal;
        }

        public static void FindDecayTimes(ref IEnumerable<LINAA.MeasurementsRow> meas)
        {
            foreach (LINAA.MeasurementsRow m in meas)
            {
                m.DecayTime = 0;
                if (!m.SubSamplesRow.IsOutReactorNull())
                {
                    m.DecayTime = (m.MeasurementStart - m.SubSamplesRow.OutReactor).TotalSeconds;
                }
            }
        }

        protected static void GetMatSSF(ref IEnumerable<LINAA.IRequestsAveragesRow> rows)
        {
            foreach (LINAA.IRequestsAveragesRow r in rows)
            {
                try
                {
                    r.Ge = 1.0;
                    if (r.MatSSFRowParent != null)
                    {
                        r.Ge = Math.Abs(r.MatSSFRowParent.SSF);
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(r, ex);
                }
            }
        }

        protected static void GetChilean(ref IEnumerable<LINAA.IRequestsAveragesRow> rows)
        {
            //sample geometry dependant values// why recalculate each time? leave them here
            HashSet<double> abs = new HashSet<double>();
            HashSet<string> elements = new HashSet<string>();

            foreach (LINAA.IRequestsAveragesRow ir in rows)
            {
                try
                {
                    ir.Ge = 1;
                    ir.SubSamplesRow.Gthermal = 1;

                    LINAA.NAARow n = ir.NAARow;
                    double surf = ir.SubSamplesRow.Radius * (ir.SubSamplesRow.Radius + ir.SubSamplesRow.FillHeight);
                    ir.SDensity = 6.0221415 * 10 * ir.SubSamplesRow.DryNet / surf;
                    double kth = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kth;
                    double kepi = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kepi;

                    if (n.ReactionsRowParent.SigmasSalRow != null && n.ReactionsRowParent.SigmasRowParent != null)
                    {
                        ir.ChTh = 0;
                        ir.ChTh = 1000 * n.ReactionsRowParent.SigmasSalRow.sigmaSal / n.ReactionsRowParent.SigmasSalRow.Mat;

                        if (elements.Add(ir.Sample + ir.Element))
                        {
                            abs.Add(ir.ppm * ir.ChTh);
                            double sumTh = abs.Sum() * ir.SDensity;
                            ir.ChTh = sumTh;
                            ir.SubSamplesRow.Gthermal = (1.0 / (Math.Pow(sumTh * 1e-12 * kth, 0.964) + 1.0));    // and since is inherited by parentRelation...
                        }

                        ir.ChEpi = 0;
                        ir.ChEpi = 1000 * n.ReactionsRowParent.SigmasRowParent.sigmaEp / n.ReactionsRowParent.SigmasSalRow.Mat;
                        double Xi = ir.ppm * 1e-12 * ir.ChEpi * ir.SDensity * kepi;
                        ir.Ge = (0.94 / (Math.Pow(Xi, 0.82) + 1.0)) + 0.06;
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(ir, ex);
                }
            }
            abs.Clear();
            elements.Clear();
        }

        /// <summary>
        /// Calculates the Temporal Decay Scheme of the given isotope Must be called after Rate() (compulsory)
        /// </summary>
        /// <returns></returns>

        public static void FindSDs<T>(ref T row)
        {
            Type tipo = typeof(T);

            if (tipo.Equals(typeof(LINAA.IRequestsAveragesRow)))
            {
                LINAA.IRequestsAveragesRow ir = row as LINAA.IRequestsAveragesRow;
                FindSDs(ref ir);
            }
            else if (tipo.Equals(typeof(LINAA.IPeakAveragesRow)))
            {
                LINAA.IPeakAveragesRow ip = row as LINAA.IPeakAveragesRow;
                FindSDs(ref ip);
            }
        }

        public static void FindSDs<T>(ref IEnumerable<T> row)
        {
            Type tipo = typeof(T);

            if (tipo.Equals(typeof(LINAA.IRequestsAveragesRow)))
            {
                IEnumerable<LINAA.IRequestsAveragesRow> rows = row as IEnumerable<LINAA.IRequestsAveragesRow>;
                rows = EC.NotDeleted<LINAA.IRequestsAveragesRow>(rows);

                if (rows.Count() == 0) return;

                System.Data.DataTable itable = rows.FirstOrDefault().Table;
                itable.BeginLoadData();

                //after restoring the computing expressions do the SD determination
                foreach (LINAA.IRequestsAveragesRow r in rows)
                {
                    try
                    {
                        IEnumerable<LINAA.IPeakAveragesRow> ipavg = r.GetIPeakAveragesRows();
                        FindSDs(ref ipavg);
                        LINAA.IRequestsAveragesRow aux = r;
                        FindSDs(ref aux);
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(r, ex);
                    }
                }

                itable.EndLoadData();
            }
            else if (tipo.Equals(typeof(LINAA.IPeakAveragesRow)))
            {
                IEnumerable<LINAA.IPeakAveragesRow> ipavg = row as IEnumerable<LINAA.IPeakAveragesRow>;
                ipavg = EC.NotDeleted<LINAA.IPeakAveragesRow>(ipavg);
                if (ipavg.Count() == 0) return;
                //after restoring the computing expressions do the SD determination
                foreach (LINAA.IPeakAveragesRow ip in ipavg)
                {
                    try
                    {
                        IEnumerable<LINAA.PeaksRow> peaks = ip.GetPeaksRows();
                        FindSDs(ref peaks);

                        LINAA.IPeakAveragesRow aux = ip;
                        FindSDs(ref aux);
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(ip, ex);
                    }
                }
                ipavg = null;
            }
            else if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
            {
                IEnumerable<LINAA.SubSamplesRow> Samples = row as IEnumerable<LINAA.SubSamplesRow>;
                foreach (LINAA.SubSamplesRow s in Samples)
                {
                    IEnumerable<LINAA.IRequestsAveragesRow> toCalc = s.GetIRequestsAveragesRows();
                    FindSDs(ref toCalc);
                }
            }
            else if (tipo.Equals(typeof(LINAA.PeaksRow)))
            {
                IEnumerable<LINAA.PeaksRow> peaks = row as IEnumerable<LINAA.PeaksRow>;
                peaks = EC.NotDeleted<LINAA.PeaksRow>(peaks);
                if (peaks.Count() == 0) return;

                foreach (LINAA.PeaksRow p in peaks)
                {
                    try
                    {
                        LINAA.PeaksRow a = p;
                        CheckFcOrPPM(ref a);
                        Weight(ref a);
                    }
                    catch (SystemException ex)
                    {
                        EC.SetRowError(p, ex);
                    }
                }
            }
        }

        protected static void FindSDs(ref LINAA.IPeakAveragesRow ir)
        {
            // IPeakAveragesDataTable table = ir.Table as IPeakAveragesDataTable;

            try
            {
                ir.ClearErrors();

                X(ref ir);    //executes hidden code

                if (ir.HasErrors)
                {
                    ir.SD = 0.0;
                    ir.ObsSD = 0.0;
                    ir.GActUnc = 0.0;
                    return;
                }

                StDev(ref ir);

                Weight(ref ir); //executes hidden code
            }
            catch (SystemException ex)
            {
                EC.SetRowError(ir, ex);
            }
        }

        protected static void FindSDs(ref LINAA.IRequestsAveragesRow ir)
        {
            try
            {
                ir.ClearErrors();

                X(ref ir);

                if (ir.HasErrors)
                {
                    ir.SD = 0.0;
                    ir.ObsSD = 0.0;
                    return;
                }

                StDev(ref ir);
            }
            catch (SystemException ex)
            {
                EC.SetRowError(ir, ex);
            }
        }

        protected internal static void CheckT0(ref LINAA.PeaksRow p)
        {
            LINAA.PeaksDataTable table = p.Table as LINAA.PeaksDataTable;
            try
            {
                if (!MyMath.IsGoodDouble(p.T0) || p.T0 == 0.0 || p.T0 == 1.0)
                {
                    if (p.T0 != 1.0) p.T0 = 1.0;
                    p.ID = -1 * Math.Abs(p.ID);
                    p.SetColumnError(table.T0Column, "Temporal Module Problem (Decay Scheme): the Temporal Factor is NaN\n");
                }
            }
            catch (SystemException ex)
            {
                p.ID = -1 * Math.Abs(p.ID);
                EC.SetRowError(p, table.T0Column, ex);
            }
        }

        protected internal static void CheckFcOrPPM(ref LINAA.PeaksRow p)
        {
            LINAA.PeaksDataTable table = p.Table as LINAA.PeaksDataTable;
            try
            {
                if (!MyMath.IsGoodDouble(p.Fc))
                {
                    p.ID = -1 * Math.Abs(p.ID);
                    p.SetColumnError(table.FcColumn, "PPM or Fc Calculation Module Problem: Calculated Fc is not a valid number");
                }

                if (!MyMath.IsGoodDouble(p.ppm))
                {
                    p.ID = -1 * Math.Abs(p.ID);
                    p.SetColumnError(table.ppmColumn, "PPM or Fc Calculation Module Problem: Calculated ppm is not a valid number");
                }
            }
            catch (SystemException ex)
            {
                p.ID = -1 * Math.Abs(p.ID);
                EC.SetRowError(p, table.FcColumn, ex);
                EC.SetRowError(p, table.ppmColumn, ex);
            }
        }

        protected internal static void CheckEffi(ref LINAA.PeaksRow p)
        {
            LINAA.PeaksDataTable table = p.Table as LINAA.PeaksDataTable;
            bool negate = true;

            string error = string.Empty;
            try
            {
                if (!MyMath.IsGoodDouble(p.Efficiency) || p.Efficiency < 1e-8)
                {
                    if (p.Efficiency != 1) p.Efficiency = 1;
                    error = "Efficiency problem: the Efficiency is NaN\n";
                }
                else negate = false;
            }
            catch (SystemException ex)
            {
                p.Efficiency = 1.0;
                error = ex.Message + "\n";
            }

            if (negate)
            {
                p.ID = -1 * Math.Abs(p.ID);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                string olderror = p.GetColumnError(table.EfficiencyColumn);
                p.SetColumnError(table.EfficiencyColumn, olderror + error);
            }
        }
    }
}