using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

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
    }
}