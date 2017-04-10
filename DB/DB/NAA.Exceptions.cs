using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class ExceptionsDataTable
        {
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
        }
    }
}