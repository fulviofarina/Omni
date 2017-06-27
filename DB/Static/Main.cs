using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// Projects a referenced IEnumerable of DataRow into a new one with only distinct rows
        /// according to a given filter field and returns those eliminated
        /// </summary>
        /// <param name="peaks1"></param>
        /// <param name="field"> </param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(ref IEnumerable<T> peaks1, string field)
        {
            HashSet<string> hashy = new HashSet<string>();

            IEnumerable<DataRow> eliminated = peaks1.OfType<DataRow>().ToList();  //all
            IEnumerable<DataRow> passed = eliminated.Where(p => hashy.Add(p[field].ToString())).ToList();   //passed the filter

            IEnumerable<T> aux = eliminated.Except(passed).OfType<T>().ToList();
            eliminated = null;
            peaks1 = passed.OfType<T>().ToList();
            passed = null;
            hashy.Clear();
            hashy = null;
            return aux;
        }

        public static double AverageOfFCs(string irradiationCode, ref IEnumerable<IRequestsAveragesRow> comparators)
        {
            double FC = 0;
            if (comparators.Count() != 0) comparators = comparators.Where(HasProjectName(irradiationCode));
            if (comparators.Count() != 0) comparators = comparators.Where(IsComparator());
            // if (comparators.Count() != 0) comparators = comparators.Where(x => !EC.IsNuDelDetch(x.SubSamplesRow.MonitorsRow));
            if (comparators.Count() != 0) FC = comparators.Average(FCAverager);
            if (FC == 0) FC = 1;
            return FC;
        }

        public static IEnumerable<T> FindSelected<T>(IEnumerable<T> rows)
        {
            Type t = typeof(T);

            if (t.Equals(typeof(SubSamplesRow)))
            {
                return rows.OfType<SubSamplesRow>().Where(s => s.Selected).Cast<T>();
            }
            else if (t.Equals(typeof(MeasurementsRow)))
            {
                return rows.OfType<MeasurementsRow>().Where(s => s.Selected).Cast<T>();
            }
            else if (t.Equals(typeof(PeaksRow)))
            {
                return rows.OfType<PeaksRow>().Where(s => s.Selected).Cast<T>();
            }
            else throw new SystemException("FindSelected not Implemented");
        }

        public static Func<LINAA.IRequestsAveragesRow, double> FCAverager = x => { if (x.IsFcNull()) return 0; else return x.Fc; };

        /// <summary>
        /// Get Peaks in need of calculations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="all">         </param>
        /// <param name="coin">        </param>
        /// <param name="sampleOrMeas"></param>
        /// <returns></returns>
        public static IEnumerable<PeaksRow> GetPeaksInNeedOf<T>(bool all, bool coin, T sampleOrMeas)
        {
            IEnumerable<PeaksRow> aux = null;

            Type tipo = typeof(T);

            if (tipo.Equals(typeof(MeasurementsRow))) aux = (sampleOrMeas as MeasurementsRow).GetPeaksRows();
            else if (tipo.Equals(typeof(SubSamplesRow))) aux = (sampleOrMeas as SubSamplesRow).GetPeaksRows();
            else throw new SystemException("GetPeaksInNeedOf Not implemented");

            if (!all && coin) return aux.Where(o => o.COI == 1);
            else if (!all) return aux.Where(o => o.Efficiency == 1);
            else return aux;
        }

        /*
        public static void SetAdded<T>(ref IEnumerable<T> old)
        {
            if (!typeof(T).BaseType.Equals(typeof(DataRow))) return;
            IEnumerable<DataRow> added = old.OfType<DataRow>().Where(r => r.RowState == DataRowState.Unchanged).ToList();
            foreach (DataRow r in added)
            {
                    r.SetModified();
            }
        }
         */

        public static IEnumerable<T> GetRowsAdded<T>(IEnumerable<T> table)
        {
            return table.OfType<DataRow>().Where(o => o.RowState == System.Data.DataRowState.Added).OfType<T>().ToList();
        }

        public static IList<T[]> Intersect<T>(ref IEnumerable<T> rows1, ref IEnumerable<T> rows2, Comparer<T> comparer)
        {
            List<T[]> ls = new List<T[]>();
            rows1 = rows1.ToList();
            foreach (T p in rows1)
            {
                IEnumerable<T> aux2 = null;
                if (comparer != null) aux2 = rows2.Where(p2 => comparer(p, p2));
                aux2 = aux2.ToList();
                foreach (T p2 in aux2)
                {
                    T[] arr = new T[2] { p, p2 };
                    ls.Add(arr);
                }
            }
            ls.TrimExcess();
            return ls;
        }
    }
}