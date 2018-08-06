using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA : IExpressions
    {
        protected internal void handlersDetSol()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(DetectorsAbsorbers));
        }

        protected internal void handlersGeometries()
        {
            this.tableMatrix.AddCompositionsHandler += addCompositionsEvent;
            // this.tableMatrix.MUESRequiredHandler += mUESRequiredEvent;
            this.tableMatrix.CleanCompositionsHandler += cleanCompositionsEvent;
            this.tableMatrix.CleanMUESHandler += cleanMUESEvent;
            // this.tableMatrix.CleanMUESPicturesHandler += cleanMUESPics;

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Matrix));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(VialType));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Geometry));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SubSamples));
        }

        protected internal void handlersSamples()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Standards));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Monitors));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Unit));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Measurements));

            tableSubSamples.AddMatrixHandler = this.addMatrixEvent;

            this.tableUnit.AddSSFsHandler = addSSFEvent;
            this.tableUnit.CleanSSFsHandler = cleanSSFEvent;

            // tableIRequestsAverages.ChThColumn.Expression = " ISNULL(1000 *
            // Parent(SigmasSal_IRequestsAverages).sigmaSal / Parent(SigmasSal_IRequestsAverages).Mat
            // ,'0')"; tableIRequestsAverages.ChEpiColumn.Expression = " ISNULL(1000 *
            // Parent(Sigmas_IRequestsAverages).sigmaEp / Parent(Sigmas_IRequestsAverages).Mat,'0')
            // "; tableIRequestsAverages.SDensityColumn.Expression = " 6.0221415 * 10 *
            // Parent(SubSamples_IRequestsAverages).DryNet / (
            // Parent(SubSamples_IRequestsAverages).Radius * (
            // Parent(SubSamples_IRequestsAverages).Radius + Parent(SubSamples_IRequestsAverages).FillHeight))";
        }

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
        /// Is this used?
        /// </summary>
        /// <param name="setexpression"></param>
        private void populateSelectedExpression(bool setexpression)
        {
            string expression = string.Empty;
            if (setexpression)
            {
                expression = "Parent(Measurements_Peaks).Selected";
            }
            // PopulatePreferences();
            Peaks.SelectedColumn.Expression = expression;
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

        public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                DataColumn col = e.Column;
                IColumn table = e.Row.Table as IColumn;

                if (table.ForbiddenNullCols != null)
                {
                    if (!table.ForbiddenNullCols.Contains(col)) return;
                }

                IRow m = e.Row as IRow;
                m.Check(col);
            }
            catch (SystemException ex)
            {
                EC.SetRowError(e.Row, e.Column, ex);
                AddException(ex);
            }
        }

        protected internal void rowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add && e.Action != DataRowAction.Commit) return;

            // if( e.Action != DataRowAction.Commit) return;
            if (EC.IsNuDelDetch(e.Row)) return;
            // if (e.Row.RowState == DataRowState.Deleted) return;
            e.Row.ClearErrors();
            // dynamic table; table = e.Row.Table.DataSet;
            IEnumerable<DataColumn> cols = e.Row.Table.Columns.OfType<DataColumn>();
            foreach (DataColumn column in cols)
            {
                DataColumnChangeEventArgs args = new DataColumnChangeEventArgs(e.Row, column, e.Row[column]);
                DataColumnChanged(sender, args);
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
                else if (dt.Equals(XCOMPref)) XCOMPref.ColumnChanging += XCOMPref.DataColumnChanging;
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

        protected List<int> dTWithHandlers = new List<int>();

        protected List<DataColumnChangeEventHandler> handlers = new List<DataColumnChangeEventHandler>();
    }
}