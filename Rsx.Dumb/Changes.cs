﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

///FULVIO
namespace Rsx.Dumb
{
  
        public static  class Changes
        {
            public static void AcceptChanges<T>(ref IEnumerable<T> rows)
            {
                if (rows == null || rows.Count() == 0) return;
                IEnumerable<DataRow> rows2 = rows.Cast<DataRow>().Where(o => o != null);
                rows2 = rows2.Where(o => o.RowState != DataRowState.Detached).ToList();
                foreach (DataRow r in rows2) r.AcceptChanges();
                rows2 = null;
            }

        public static IEnumerable<DataTable> GetTablesWithChanges(ref IEnumerable<DataTable> tables)
        {
            Func<DataTable, bool> haschangesFunc = t =>
            {
                bool hasChanges = false;
                IEnumerable<DataRow> rows = t.AsEnumerable();
                IEnumerable<DataRow> rowsWithChanges = Changes.GetRowsWithChanges(rows);
                hasChanges = rowsWithChanges.Count() != 0;
                return hasChanges;
            };

            return tables.Where(haschangesFunc);
        }


        public static bool HasChanges(IEnumerable<DataRow> array)
            {
                if (array == null) return false;
                if (array.Count() == 0) return false;

                DataRow first = array.FirstOrDefault(h => HasChanges(h));
                // DataTable dt = array.FirstOrDefault().Table; //get table DataRow first =
                // array.FirstOrDefault(ChangesSelector(dt)); //get selector for table and
                if (first != null) return true;
                else return false;
            }

            public static bool HasChanges(DataRow r)
            {
                if (r.RowState == DataRowState.Modified) return true;
                if (r.RowState == DataRowState.Added) return true;
                if (r.RowState == DataRowState.Deleted) return true;
                else return false;

                // DataTable dt = r.Table; //get table return ChangesSelector(dt)(r); //get selector for
                // table and
            }

        public static void UpdateDateTimes(ref DataSet data)
        {
            foreach (DataTable dt in data.Tables)
            {
                UpdateDateTimes(dt);
            }

        }

        public static void UpdateDateTimes(DataTable dt)
        {
            IEnumerable<DataRow> changes = dt.Rows.OfType<DataRow>();
            changes = changes.Where(o => o.RowState == DataRowState.Modified);
            foreach (DataRow r in changes)
            {
                IEnumerable<DataColumn> dtes = dt.Columns.OfType<DataColumn>().Where(o => o.DataType == typeof(DateTime) && !o.ReadOnly);
                foreach (DataColumn d in dtes)
                {
                    r.SetField(d, DateTime.Now); //update the dates
                }
            }
        }


        public static IEnumerable<DataRow> GetRowsWithChanges(IEnumerable<DataRow> array)
            {
                if (array.Count() == 0) return array;
                DataTable dt = array.FirstOrDefault().Table;
                Func<DataRow, bool> changessel = ChangesSelector(dt);
                return array.Where<DataRow>(changessel).ToList();
            }

            public static Func<DataRow, bool> ChangesSelector(DataTable dt)
            {
                IEnumerable<int> ords = dt.Columns.OfType<DataColumn>().Select(o => o.Ordinal).ToList();

                Func<DataRow, bool> CurrentModif = i =>
                {
                    if (i.RowState == DataRowState.Deleted) return true;
                    else if (i.RowState == DataRowState.Added) return true;
                    if (!i.HasVersion(DataRowVersion.Current)) return false;
                    if (!i.HasVersion(DataRowVersion.Original)) return false;

                    foreach (int x in ords)
                    {
                        if (HasChanged(i, x)) return true;
                    }
                    return false;
                };
                return CurrentModif;
            }

            public static bool HasChanged(DataRow i, int x)
            {
                object curr = i.Field<object>(x, DataRowVersion.Current);
                object org = i.Field<object>(x, DataRowVersion.Original);
                if (curr == null || org == null) return false;

                if (!curr.Equals(org)) return true;
                else return false;
            }
        } 
    
}