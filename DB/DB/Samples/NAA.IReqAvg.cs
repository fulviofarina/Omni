using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

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

        partial class IRequestsAveragesDataTable
        {
            private static Func<LINAA.IRequestsAveragesRow, double> averager = x => { if (x.IsFcNull()) return 0; else return x.Fc; };

            public double AvgOfFCs(string irradiationCode)
            {
                double FC = 0;
                IEnumerable<IRequestsAveragesRow> comparators = this.Where(x => !EC.IsNuDelDetch(x));
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
                if (!Rsx.EC.IsNuDelDetch(s))
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

            //  public System.Collections.Hashtable T2;
            //  public System.Collections.Hashtable T3;
            //  public System.Collections.Hashtable T4;
            //  public System.Collections.Hashtable T5;

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

            public double _T0; //[robando para 1 medición, genetic algorithm
        }
    }
}