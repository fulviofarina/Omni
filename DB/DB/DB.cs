using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA : IDB
    {
        /// <summary>
        /// IS THIS USED???
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        protected static bool IsTableOk<T>(ref T table)
        {
            DataTable table2 = table as DataTable;
            if (table2 == null) return false;
            if (table2.Rows.Count == 0) return false;
            return true;
        }

        public void CloneDataSet(ref LINAA set)
        {
            this.InitializeComponent();
            this.Merge(set, false, MissingSchemaAction.Ignore);
            this.PopulateColumnExpresions();
            this.IRequestsAverages.Clear();
            this.IPeakAverages.Clear();

            DataTable table = IRequestsAverages;
            cleanReadOnly(ref table);
            table = IPeakAverages;
            cleanReadOnly(ref table);
        }

        partial class ExceptionsDataTable
        {
            public void RemoveDuplicates()
            {
                HashSet<string> hs = new HashSet<string>();
                IEnumerable<LINAA.ExceptionsRow> ordered = this.OrderByDescending(o => o.Date);
                ordered = ordered.TakeWhile(o => !hs.Add(o.StackTrace));
                for (int i = ordered.Count() - 1; i >= 0; i--)
                {
                    LINAA.ExceptionsRow e = ordered.ElementAt(i);
                    e.Delete();
                }

                hs.Clear();
                hs = null;
                this.AcceptChanges();
            }

            public void AddExceptionsRow(Exception ex)
            {
                string target = string.Empty;
                string stack = string.Empty;
                string source = string.Empty;

                if (ex.Source != null) source = ex.Source;
                if (ex.TargetSite != null) target = ex.TargetSite.Name;
                if (ex.StackTrace != null) stack = ex.StackTrace;

                AddExceptionsRow(target, ex.Message, stack, source, DateTime.Now);
            }
        }
    }
}