using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA : IDB
    {
        protected List<int> dTWithHandlers = new List<int>();

        protected List<DataColumnChangeEventHandler> handlers = new List<DataColumnChangeEventHandler>();

        public void PopulateColumnExpresions()
        {
            handlersIrradiations();

            handlersGeometries();

            handlersSamples();

            handlersDetSol();

            handlersPreferences();

            setHandlers(true);

            populateSelectedExpression(true);
        }

        /// <summary>
        /// IS THIS USED???
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        protected internal static bool IsTableOk<T>(ref T table)
        {
            DataTable table2 = table as DataTable;
            if (table2 == null) return false;
            if (table2.Rows.Count == 0) return false;
            return true;
        }

        protected internal void cleanReadOnly(ref DataTable table)
        {
            // DataTable table = dt as DataTable;
            foreach (DataColumn column in table.Columns)
            {
                bool ok = column.ReadOnly;
                ok = ok && column.Expression.Equals(string.Empty);
                ok = ok && !table.PrimaryKey.Contains(column);
                if (ok) column.ReadOnly = false;
            }
        }










        protected internal void rowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add && e.Action != DataRowAction.Commit) return;

            // if( e.Action != DataRowAction.Commit) return;
            if (e.Row.RowState == DataRowState.Deleted) return;
            e.Row.ClearErrors();
            dynamic table;
            table = e.Row.Table;
            IEnumerable<DataColumn> cols = e.Row.Table.Columns.OfType<DataColumn>();
            foreach (DataColumn column in cols)
            {
                DataColumnChangeEventArgs args = new DataColumnChangeEventArgs(e.Row, column, e.Row[column]);
                table.DataColumnChanged(sender, args);
            }
        }

        protected internal void setHandlers(bool activate, ref DataTable dt)
        {
            int dtindex = Tables.IndexOf(dt);
            int index = dTWithHandlers.IndexOf(dtindex);
            if (index < 0) return; //not in the list of handlers

            DataColumnChangeEventHandler han = handlers.ElementAt(index);
            if (activate)
            {
                if (dt.Equals(Matrix)) Matrix.ColumnChanging += Matrix.DataColumnChanging;
                else if (dt.Equals(Unit)) Unit.ColumnChanging += Unit.DataColumnChanging;
                else if (dt.Equals(SubSamples)) SubSamples.ColumnChanging += SubSamples.DataColumnChanging;
                dt.ColumnChanged += han;
            }
            else
            {
                if (dt.Equals(Unit)) Unit.ColumnChanging -= Unit.DataColumnChanging;
                else if (dt.Equals(Matrix)) Matrix.ColumnChanging -= Matrix.DataColumnChanging;
                else if (dt.Equals(SubSamples)) SubSamples.ColumnChanging -= SubSamples.DataColumnChanging;
                dt.ColumnChanged -= han;
            }
        }
        protected internal void setRowHandlers(ref DataTable table, bool activate)
        {
            int dtindex = Tables.IndexOf(table);
            int index = dTWithHandlers.IndexOf(dtindex);
            if (index < 0) return; //not in the list of handlers

            if (activate) table.RowChanged += rowChanged;
            else table.RowChanged -= rowChanged;
        }

        protected internal void setHandlers(bool activate)
        {
            for (int i = 0; i < dTWithHandlers.Count; i++)
            {
                int index = dTWithHandlers[i];
                DataTable dt = Tables[index];
                setHandlers(activate, ref dt);
                setRowHandlers(ref dt, activate);
            }
        }
    }
}