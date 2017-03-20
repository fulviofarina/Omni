using System.Collections.Generic;
using System.Data;
using System.Linq;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IDB
    {
        private void cleanReadOnly(ref DataTable table)
        {
            //    DataTable table = dt as DataTable;
            foreach (DataColumn column in table.Columns)
            {
                bool ok = column.ReadOnly;
                ok = ok && column.Expression.Equals(string.Empty);
                ok = ok && !table.PrimaryKey.Contains(column);
                if (ok) column.ReadOnly = false;
            }
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

        public void PopulateColumnExpresions()
        {
            handlers = new List<DataColumnChangeEventHandler>();
            dTWithHandlers = new List<int>();

            handlers.Add(Channels.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(Channels));
            handlers.Add(IrradiationRequests.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(IrradiationRequests));
            handlers.Add(Matrix.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(Matrix));
            handlers.Add(VialType.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(VialType));
            handlers.Add(Geometry.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(Geometry));
            handlers.Add(SubSamples.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(SubSamples));
            handlers.Add(Standards.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(Standards));
            handlers.Add(Monitors.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(Monitors));
            handlers.Add(DetectorsAbsorbers.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(DetectorsAbsorbers));
            handlers.Add(Unit.DataColumnChanged);
            dTWithHandlers.Add(this.Tables.IndexOf(Unit));

            Handlers(true);

            PopulateSelectedExpression(true);

            //  tableIRequestsAverages.ChThColumn.Expression = " ISNULL(1000 * Parent(SigmasSal_IRequestsAverages).sigmaSal / Parent(SigmasSal_IRequestsAverages).Mat ,'0')";
            //  tableIRequestsAverages.ChEpiColumn.Expression = " ISNULL(1000 * Parent(Sigmas_IRequestsAverages).sigmaEp / Parent(Sigmas_IRequestsAverages).Mat,'0') ";
            //   tableIRequestsAverages.SDensityColumn.Expression = "  6.0221415 * 10 * Parent(SubSamples_IRequestsAverages).DryNet / ( Parent(SubSamples_IRequestsAverages).Radius * ( Parent(SubSamples_IRequestsAverages).Radius + Parent(SubSamples_IRequestsAverages).FillHeight))";
        }

        public void PopulateSelectedExpression(bool setexpression)
        {
            string expression = string.Empty;
            if (setexpression)
            {
                expression = "Parent(Measurements_Peaks).Selected";
            }

            tablePeaks.SelectedColumn.Expression = expression;
        }

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

        protected internal void Handlers(bool activate, ref DataTable dt)
        {
            int dtindex = this.Tables.IndexOf(dt);
            int index = dTWithHandlers.IndexOf(dtindex);
            if (index < 0) return; //not in the list of handlers

            DataColumnChangeEventHandler han = handlers.ElementAt(index);
            if (activate)
            {
                if (dt.Equals(this.tableMatrix)) tableMatrix.ColumnChanging += tableMatrix.DataColumnChanging;
                dt.ColumnChanged += han;
            }
            else
            {
                if (dt.Equals(this.tableMatrix)) tableMatrix.ColumnChanging -= tableMatrix.DataColumnChanging;
                dt.ColumnChanged -= han;
            }
        }

        private void Handlers(bool activate)
        {
            for (int i = 0; i < dTWithHandlers.Count; i++)
            {
                int index = dTWithHandlers[i];
                DataTable dt = this.Tables[index];
                Handlers(activate, ref dt);
                RowHandlers(ref dt, activate);
            }
        }

        protected internal void RowHandlers(ref DataTable table, bool activate)
        {
            int dtindex = this.Tables.IndexOf(table);
            int index = dTWithHandlers.IndexOf(dtindex);
            if (index < 0) return; //not in the list of handlers

            if (activate) table.RowChanged += this.RowChanged;
            else table.RowChanged -= this.RowChanged;
        }

        protected internal void RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add && e.Action != DataRowAction.Commit) return;
            //	if( e.Action != DataRowAction.Commit) return;

            e.Row.ClearErrors();
            dynamic table;
            table = e.Row.Table;
            IEnumerable<DataColumn> cols = e.Row.Table.Columns.OfType<DataColumn>().ToArray();
            foreach (DataColumn column in cols)
            {
                table.DataColumnChanged(sender, new DataColumnChangeEventArgs(e.Row, column, e.Row[column]));
            }
        }

        private List<DataColumnChangeEventHandler> handlers;

        private List<int> dTWithHandlers;
    }
}