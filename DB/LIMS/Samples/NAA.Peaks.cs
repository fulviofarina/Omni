using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Math;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// Returns two arrays (Y,X) of doubles containing info about the branching factor)
        /// </summary>
        /// <param name="daugther"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references)
        {
            DataTable table = new DataTable();

            double Ld = 0.693 / daugther.k0NAARow.NAARow.T3;
            double Sd = MyMath.S(Ld, daugther.SubSamplesRow.IrradiationTotalTime);

            LINAA.PeaksRow[] dpeaks = daugther.GetPeaksRows();

            table.Columns.Add("Vs", typeof(string));

            foreach (IPeakAveragesRow reference in references)
            {
                if (reference.Equals(daugther)) continue;
                if (reference.n == 0) continue;

                string energy = reference.Energy.ToString();
                table.Columns.Add("X" + energy, typeof(double));
                table.Columns.Add("Y" + energy, typeof(double));
                table.Columns.Add("EC" + energy, typeof(double));
                table.Columns.Add("Res" + energy, typeof(double));
                table.Columns.Add("YCalc" + energy, typeof(double));

                //lamdas
                double Lref = 0.693 / reference.k0NAARow.NAARow.T2;
                //saturation factor
                double Sref = MyMath.S(Lref, reference.SubSamplesRow.IrradiationTotalTime);
                double DLrefd = Ld - Lref; //delta lamdaes

                LINAA.PeaksRow[] Rpeaks = reference.GetPeaksRows();

                foreach (PeaksRow Rp in Rpeaks)
                {
                    //find same measurement
                    Func<PeaksRow, bool> finder = o => o.Measurement.Equals(Rp.Measurement);

                    LINAA.PeaksRow dp = dpeaks.FirstOrDefault(finder);

                    if (dp == null) continue;

                    //decay factors
                    double Dref = MyMath.D(Lref, Rp.MeasurementsRow.DecayTime / 60);
                    double Dd = MyMath.D(Ld, dp.MeasurementsRow.DecayTime / 60);

                    //counting factors
                    double Cref = MyMath.C(Lref, Rp.MeasurementsRow.CountTime / 60);
                    double Cd = MyMath.C(Ld, dp.MeasurementsRow.CountTime / 60);

                    DataRow result = table.AsEnumerable().FirstOrDefault(o => o.Field<string>("Vs").Equals(Rp.Measurement));

                    if (result == null)
                    {
                        result = table.NewRow();
                        result.SetField<string>("Vs", Rp.Measurement);
                        table.Rows.Add(result);
                    }

                    result.SetField<double>("Y" + energy, (dp.Area) / (Rp.Area));
                    result.SetField<double>("EC" + energy, (dp.Efficiency * dp.COI) / (Rp.Efficiency * Rp.COI));

                    double aux = (Sd * Dd * Cd) / (Sref * Dref * Cref);
                    aux = Ld - (Lref * aux);
                    aux = (aux / DLrefd);

                    result.SetField<double>("X" + energy, aux);
                }

                double[] abR2 = MyMath.CurveFit.Linear.LeastSquaresFit(table.Columns["X" + energy], table.Columns["Y" + energy], table.Columns["Res" + energy], table.Columns["YCalc" + energy]);

                DataRow r = table.AsEnumerable().FirstOrDefault(o => o.Field<string>("Vs").Equals("Fit"));

                if (r == null)
                {
                    r = table.NewRow();
                    table.Rows.Add(r);
                }
                r.SetField<double>("Y" + energy, abR2[0]);
                r.SetField<double>("X" + energy, abR2[1]);
                r.SetField<double>("YCalc" + energy, reference.Energy);
                r.SetField<double>("Res" + energy, abR2[2]);
                r.SetField<double>("EC" + energy, (abR2[0] / abR2[1]));
                r.SetField<string>("Vs", "Fit");
            }

            return table;
        }

        /// <summary>
        /// Calculates the given input samples by iterating aggregate functions on each isotope, its different energies and measurements
        /// </summary>
        /// <param name="Samples"></param>
        /// <param name="matSSF"></param>
        /// <param name="chilean"></param>
        /// <param name="ReEffi"></param>
        /// <param name="ReDecay"></param>
        ///

        public partial class IPeakAveragesRow
        {
            public double UncSq = 0;
            public double w = 0;
            public double w2 = 0;
            public double wX = 0;
            public double wX2 = 0;
        }

        partial class IPeakAveragesDataTable
        {
            public IPeakAveragesRow NewIPeakAveragesRow(Int32 k0Id, ref SubSamplesRow s)
            {
                IPeakAveragesRow ip = this.NewIPeakAveragesRow();
                this.AddIPeakAveragesRow(ip);
                ip.k0ID = k0Id;
                //	ip.Radioisotope = iso;
                //	ip.Element = sym;
                //	ip.Energy = energy;
                if (!Rsx.Dumb.IsNuDelDetch(s))
                {
                    ip.Sample = s.SubSampleName;
                    //   if (  !s.IsIrradiationCodeNull())   ip.Project = s.IrradiationCode;
                }
                return ip;
            }
        }

        partial class IRequestsAveragesDataTable
        {
            private static Func<LINAA.IRequestsAveragesRow, double> averager = x => { if (x.IsFcNull()) return 0; else return x.Fc; };

            public double AvgOfFCs(string irradiationCode)
            {
                double FC = 0;
                IEnumerable<IRequestsAveragesRow> comparators = this.Where(x => !Dumb.IsNuDelDetch(x));
                if (comparators.Count() != 0) comparators = comparators.Where(x => x.Project.CompareTo(irradiationCode) == 0);
                if (comparators.Count() != 0) comparators = comparators.Where(x => x.Comparator);
                if (comparators.Count() != 0) comparators = comparators.Where(x => x.SubSamplesRow.MonitorsRow != null);
                if (comparators.Count() != 0) FC = comparators.Average(averager);
                if (FC == 0) FC = 1;
                return FC;
            }

            public IRequestsAveragesRow NewIRequestsAveragesRow(Int32 NAAID, ref SubSamplesRow s)
            {
                IRequestsAveragesRow irs = this.NewIRequestsAveragesRow();
                this.AddIRequestsAveragesRow(irs);
                irs.NAAID = NAAID;
                //irs.Radioisotope = iso;
                //	irs.Element = sym;
                if (!Rsx.Dumb.IsNuDelDetch(s))
                {
                    irs.Sample = s.SubSampleName;
                    //   irs.Project = s.IrradiationCode;
                }
                return irs;
            }

            /*
            /// <summary>
            /// Returns a IEnumerable of a) IRequestsRows or b) their childs (IPeaksAverages) or c) their grandchilds (the Peaks), depending on T1
            /// </summary>
            /// <typeparam name="T1"></typeparam>
            /// <param name="toFilter">Rows to filter according to target and return data from</param>
            /// <param name="target">target to filter the rows</param>
            /// <returns></returns>
                    public IEnumerable<T1> Where<T1>(IEnumerable<LINAA.IRequestsAveragesRow> toFilter, string Filterfield, object valueFilter)
            {
               //to filter by target

               //filtered list of isotopes sharing same target

               toFilter = toFilter.Where(LINAA.SelectorByField<IRequestsAveragesRow>(valueFilter,Filterfield));

               //now throw array of isotopes or its childs or granchilds... (ipeaks averages and peaks)
               Type tipo = typeof(T1);
               if (tipo.Equals(typeof(LINAA.IRequestsAveragesRow)))    return toFilter.Cast<T1>();
               else
               {
                  System.Collections.Generic.List<T1> output = new List<T1>();
                  foreach (LINAA.IRequestsAveragesRow fil in toFilter)
                  {
                     if (tipo.Equals(typeof(LINAA.IPeakAveragesRow)))
                     {
                        output.AddRange(fil.GetIPeakAveragesRows().Cast<T1>());
                     }
                     else if (tipo.Equals(typeof(LINAA.IPeakAveragesRow)))
                     {
                        IEnumerable<LINAA.IPeakAveragesRow> aux = fil.GetIPeakAveragesRows();
                        foreach (LINAA.IPeakAveragesRow ip in aux)
                        {
                           output.AddRange(ip.GetPeaksRows().Cast<T1>());
                        }
                     }
                  }
                  return output.AsEnumerable<T1>();
               }
            }
             */
        }

        partial class IRequestsAveragesRow
        {
            public IEnumerable<IRequestsAveragesRow> GetSameTarget()
            {
                IEnumerable<IRequestsAveragesRow> companions = null;
                if (IsTargetNull()) return companions;

                Func<IRequestsAveragesRow, bool> sameSample = v =>
                {
                    return (v.SubSamplesRow == this.SubSamplesRow);
                };
                Func<IRequestsAveragesRow, bool> sametarget = v =>
                {
                    return (v.Target.CompareTo(this.Target) == 0);
                };

                companions = this.tableIRequestsAverages.Where(sameSample).Where(sametarget).ToList();

                return companions;
            }

            public void SetGepithermal(double ge)
            {
                IEnumerable<LINAA.IRequestsAveragesRow> isos = this.GetSameTarget();
                foreach (LINAA.IRequestsAveragesRow i in isos) i.Ge = ge;
            }

            public double UncSq = 0;
            public double w = 0;
            public double w2 = 0;
            public double wX = 0;
            public double wX2 = 0;

            //   public int g = 0;
            public double x = 0;

            public System.Collections.Hashtable T2;
            public System.Collections.Hashtable T3;
            public System.Collections.Hashtable T4;
            public System.Collections.Hashtable T5;

            public double _Er = 1.0;
            public double _Qog = 0.0;
            public double _Qom = 0.0;
            public double _Qomg = 0.0;

            public double _Rg = 0;
            public double _Rm = 0;
            public double _Rmg = 0;

            public double Calpha = 0;
            public double _Cd = 1.00;

            public bool? _mg = false; //triple state ;) false, true and null

            public double _Qo;   //for storing the original qo;
            public double _Qoalpha;

            public double _QoUnc;
        }

        partial class PeaksRow
        {
            public double UncSq = 0;
            public double w = 0;
            public double w2 = 0;
            public double wX = 0;
            public double wX2 = 0;

            public int ETAInMin = 0;
        }

        partial class PeaksDataTable
        {
            public LINAA.PeaksRow NewPeaksRow(Int32 k0Id, double energy, ref SubSamplesRow s, ref MeasurementsRow m)
            {
                LINAA.PeaksRow peak = this.NewPeaksRow();
                this.AddPeaksRow(peak);
                //  peak.Selected = true;
                peak.Ready = false;
                peak.ID = k0Id;
                peak.Energy = energy;
                //	peak.Sym = sym;
                //	peak.Iso = iso;

                if (!Rsx.Dumb.IsNuDelDetch(s))
                {
                    peak.IrradiationID = s.IrradiationRequestsID;
                    peak.SampleID = s.SubSamplesID;
                }
                if (!Rsx.Dumb.IsNuDelDetch(m))
                {
                    peak.MeasurementID = m.MeasurementID;
                    //  peak.Measurement = m.Measurement;
                }

                return peak;
            }
        }
    }
}