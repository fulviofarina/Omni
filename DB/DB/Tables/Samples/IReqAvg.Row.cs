using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// Returns two arrays (Y,X) of doubles containing info about the branching factor)
        /// </summary>
        /// <param name="daugther"> </param>
        /// <param name="reference"></param>
        /// <returns></returns>

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

            // public int g = 0;
            public double x = 0;

            // public System.Collections.Hashtable T2; public System.Collections.Hashtable T3; public
            // System.Collections.Hashtable T4; public System.Collections.Hashtable T5;

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