using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rsx
{
    public partial class Dumb
    {
        public static String[] ArrayFromColum(DataColumn Column)
        {
            System.Converter<DataRow, String> convi = delegate(DataRow row) { return Convert.ToString(row[Column.ColumnName]); };
            String[] rowValuesForColumn = Array.ConvertAll<DataRow, String>(Column.Table.Select(), convi);
            return rowValuesForColumn;
        }

        /// <summary>
        /// Puts Null Error in the given column value of the given row (row,column)
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public static bool CheckNull(DataColumn column, DataRow row)
        {
            row.SetColumnError(column, null);

            Type t = column.DataType;

            if (t.Equals(typeof(double)))
            {
                if (row.IsNull(column) || Convert.ToDouble(row[column]) == 0)
                {
                    row.SetColumnError(column, "NULL. Not good!");
                }
            }
            else if (t.Equals(typeof(Int32)))
            {
                if (row.IsNull(column) || Convert.ToInt32(row[column]) == 0)
                {
                    row.SetColumnError(column, "NULL. Not good!");
                }
            }
            else if (t.Equals(typeof(String)))
            {
                if (row.IsNull(column) || Convert.ToString(row[column]).CompareTo(String.Empty) == 0)
                {
                    row.SetColumnError(column, "NULL. Not good!");
                }
            }
            else if (t.Equals(typeof(DateTime)))
            {
                if (row.IsNull(column) && !column.ReadOnly)
                {
                    row[column] = DateTime.Now;
                }
            }
            else if (t.Equals(typeof(bool)))
            {
                if (row.IsNull(column) && !column.ReadOnly) row[column] = false;
            }

            string error = row.GetColumnError(column);

            return !String.IsNullOrEmpty(error);
        }

        public static void CheckRowColumn(System.Data.DataRowChangeEventArgs e, DataColumnChangeEventHandler columnChecker)
        {
            if (e.Action == System.Data.DataRowAction.Add || e.Action == System.Data.DataRowAction.Change)
            {
                if (e.Row.RowState != System.Data.DataRowState.Deleted)
                {
                    DataColumn currentcol = null;
                    DataColumnChangeEventArgs args = null;

                    foreach (System.Data.DataColumn col in e.Row.Table.Columns)
                    {
                        currentcol = col;
                        args = new System.Data.DataColumnChangeEventArgs(e.Row, currentcol, e.Row[currentcol]);
                        columnChecker(null, args);
                    }
                }
            }
        }

        public static void CheckRows(DataTable table2, DataColumnChangeEventHandler columnChecker)
        {
            DataRow currentRow = null;
            DataRowChangeEventArgs args = null;

            foreach (DataRow r in table2.Rows)
            {
                currentRow = r;
                if (currentRow.RowState == DataRowState.Unchanged) currentRow.SetModified();
                args = new DataRowChangeEventArgs(currentRow, DataRowAction.Change);

                Dumb.CheckRowColumn(args, columnChecker);
                currentRow.AcceptChanges();
            }
        }

        public static void CheckRows(DataRow[] rows, DataColumnChangeEventHandler columnChecker)
        {
            DataRow currentRow = null;
            DataRowChangeEventArgs args = null;
            foreach (DataRow r in rows)
            {
                currentRow = r;
                args = new DataRowChangeEventArgs(currentRow, DataRowAction.Change);

                if (currentRow.RowState == DataRowState.Unchanged) currentRow.SetModified();
                Dumb.CheckRowColumn(args, columnChecker);
                currentRow.AcceptChanges();
            }
        }

        public static void CleanColumnExpressions(DataTable table)
        {
            IEnumerable<DataColumn> columns = table.Columns.OfType<DataColumn>().Where(c => c.Expression.CompareTo(string.Empty) != 0).ToArray();
            foreach (DataColumn column in columns)
            {
                column.Expression = null;
                column.ReadOnly = false;
            }
        }

        /// <summary>
        /// Returns a cloned "input" table (with values) after the filter "expression" has been applied
        /// </summary>
        /// <param name="input">DataTable to filter and clone</param>
        /// <param name="expression">Expression to use as a filter</param>
        /// <returns>A new DataTable with same schema and values from "input"</returns>
        public static DataTable Clone(DataTable input, string expression)
        {
            DataTable filtered = input.Clone();
            DataView v = input.AsDataView();
            v.RowFilter = expression;
            if (v.Count != 0)
            {
                filtered.Load(v.ToTable().CreateDataReader(), LoadOption.OverwriteChanges);
            }
            return filtered;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.Stream stream = new System.IO.MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Reduces the DataSet "set" removing the strongly-typed DataTables from the input Type array
        /// </summary>
        /// <param name="set">Data set to reduce</param>
        /// <param name="DataTableTypes">input Type array of strongly-typed DataTables to remove from set</param>
        public static void Preserve(System.Data.DataSet set, Type[] DataTableTypes)
        {
            System.Data.DataSet Set = set.Clone();

            bool orgConst = set.EnforceConstraints;
            Set.EnforceConstraints = false;

            foreach (System.Data.DataTable table in Set.Tables)
            {
                System.Data.DataTable toremove = null;

                HashSet<Type> hs = new HashSet<Type>(DataTableTypes.ToArray());

                Type t = table.GetType();

                if (!hs.Contains(t))
                {
                    toremove = set.Tables[table.TableName, table.Namespace];
                }

                if (toremove != null)
                {
                    toremove.ChildRelations.Clear();
                    toremove.ParentRelations.Clear();
                    foreach (System.Data.Constraint constraint in table.Constraints)
                    {
                        if (constraint.GetType().Equals(typeof(System.Data.ForeignKeyConstraint)))
                        {
                            toremove.Constraints.Remove(constraint.ConstraintName);
                        }
                    }
                    toremove.TableName += "...";
                }
            }

            foreach (System.Data.DataTable table in Set.Tables)
            {
                System.Data.DataTable toremove = set.Tables[table.TableName + "...", table.Namespace];
                if (toremove != null)
                {
                    toremove.Dispose();
                    set.Tables.Remove(toremove);
                }
            }

            Set.EnforceConstraints = orgConst;
        }

        public static void SetRowError(DataRow row, DataColumn column, SystemException ex)
        {
            try
            {
                row.SetColumnError(column, ex.Message + "\n\n" + ex.StackTrace);
            }
            catch (SystemException nada)
            {
            }
        }

        public static void SetRowError(DataRow row, SystemException ex)
        {
            try
            {
                row.RowError = ex.Message + "\n\n" + ex.StackTrace;
            }
            catch (SystemException nada)
            {
            }
        }
    }
}