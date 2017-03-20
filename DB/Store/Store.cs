using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IStore
    {
        public IEnumerable<DataTable> GetTablesWithChanges()
        {
            IEnumerable<DataTable> tables = null;
            tables = this.Tables.OfType<DataTable>();
            Func<DataTable, bool> haschangesFunc = t =>
            {
                bool hasChanges = false;
                IEnumerable<DataRow> rows = t.AsEnumerable();
                IEnumerable<DataRow> rowsWithChanges = Dumb.GetRowsWithChanges(rows);
                hasChanges = rowsWithChanges.Count() != 0;
                return hasChanges;
            };

            return tables.Where(haschangesFunc);
        }
    }
}