using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        public static IEnumerable<PeaksRow> GetPeaksInNeedOf<T>(bool all, bool coin, T sampleOrMeas)
        {
            IEnumerable<LINAA.PeaksRow> aux = null;

            Type tipo = typeof(T);

            if (tipo.Equals(typeof(LINAA.MeasurementsRow))) aux = (sampleOrMeas as LINAA.MeasurementsRow).GetPeaksRows();
            else if (tipo.Equals(typeof(LINAA.SubSamplesRow))) aux = (sampleOrMeas as LINAA.SubSamplesRow).GetPeaksRows();
            else throw new SystemException("GetPeaksInNeedOf Not implemented");

            if (!all && coin) return aux.Where(o => o.COI == 1);
            else if (!all) return aux.Where(o => o.Efficiency == 1);
            else return aux;
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

        /// <summary>
        /// Projects a referenced IEnumerable of DataRow into a new one with only distinct rows according to a given filter field and returns those eliminated
        /// </summary>
        /// <param name="peaks1"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(ref IEnumerable<T> peaks1, string field)
        {
            HashSet<string> hashy = new HashSet<string>();

            IEnumerable<System.Data.DataRow> eliminated = peaks1.OfType<System.Data.DataRow>().ToList();	//all
            IEnumerable<System.Data.DataRow> passed = eliminated.Where(p => hashy.Add(p[field].ToString())).ToList();   //passed the filter

            IEnumerable<T> aux = eliminated.Except(passed).OfType<T>().ToList();
            eliminated = null;
            peaks1 = passed.OfType<T>().ToList();
            passed = null;
            hashy.Clear();
            hashy = null;
            return aux;
        }

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

        public static IList<string[]> StripComposition(ref MatrixRow Matrix)
        {
            System.Collections.Generic.List<string[]> ls = null;
            if (Rsx.Dumb.IsNuDelDetch(Matrix)) return ls;
            if (Matrix.IsMatrixCompositionNull()) return ls;

            string matCompo = Matrix.MatrixComposition;
            string[] strArray = null;
            if (matCompo.Contains(',')) strArray = matCompo.Split(',');    ///
            else strArray = new string[] { matCompo };

            ls = new System.Collections.Generic.List<string[]>();
            for (int index = 0; index < strArray.Length; index++)
            {
                string[] strArray2 = strArray[index].Trim().Split('(');
                string formula = strArray2[0].Replace("#", null).Trim();
                string composition = strArray2[1].Replace("%", null).Trim();
                composition = composition.Replace(")", null).Trim();
                string[] formCompo = new string[] { formula, composition };
                ls.Add(formCompo);
            }

            return ls;
        }
    }
}