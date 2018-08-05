using DB.LINAATableAdapters;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected internal static bool removeDuplicates(DataTable table, string UniqueField, string IndexField, ref Action<int> remover)
        {
            bool duplicates = false;

            IList<object> hs = Hash.HashFrom<object>(table.Columns[UniqueField]);

            if (hs.Count != table.Rows.Count) //there are duplicates!!
            {
                IEnumerable<DataRow> rows = null;
                foreach (object s in hs)
                {
                    rows = table.AsEnumerable();
                    rows = rows.Where(d => d.Field<object>(UniqueField).Equals(s));
                    if (rows.Count() > 1)// there are sample duplicates
                    {
                        rows = rows.OrderByDescending(d => d.Field<object>(IndexField)); //most recent is the first, older the last
                        rows = rows.Take(rows.Count() - 1);
                        foreach (DataRow d in rows)
                        {
                            remover.Invoke(d.Field<int>(IndexField));
                        }
                    }
                }

                hs.Clear();
                hs = null;

                duplicates = true;
            }

            return duplicates;
        }

        protected internal void disposeComponent()
        {
            QTA?.Dispose();
            QTA = null;
            tAM?.Connection?.Close();
            tAM?.Dispose();

            tAM = null;
        }

        protected internal void disposeIrradiationAdapters()
        {
            tAM.ChannelsTableAdapter?.Connection.Close();
            tAM.ChannelsTableAdapter?.Dispose();

            tAM.IrradiationRequestsTableAdapter?.Connection.Close();
            tAM.IrradiationRequestsTableAdapter?.Dispose();

            tAM.OrdersTableAdapter?.Connection.Close();
            tAM.OrdersTableAdapter?.Dispose();

            tAM.ProjectsTableAdapter?.Connection.Close();
            tAM.ProjectsTableAdapter?.Dispose();

            tAM.ChannelsTableAdapter = null;
            tAM.IrradiationRequestsTableAdapter = null;
            tAM.OrdersTableAdapter = null;
            tAM.ProjectsTableAdapter = null;
        }

        protected internal void disposeOtherAdapters()
        {
            tAM.ElementsTableAdapter?.Connection.Close();

            tAM.ElementsTableAdapter?.Dispose();

            tAM.SigmasTableAdapter?.Connection.Close();

            tAM.SigmasTableAdapter?.Dispose();

            tAM.SigmasSalTableAdapter?.Connection.Close();
            tAM.SigmasSalTableAdapter?.Dispose();

            tAM.ReactionsTableAdapter?.Connection.Close();
            tAM.ReactionsTableAdapter?.Dispose();

            tAM.NAATableAdapter?.Connection.Close();

            tAM.NAATableAdapter?.Dispose();

            tAM.tStudentTableAdapter?.Connection.Close();
            tAM.tStudentTableAdapter?.Dispose();

            tAM.pValuesTableAdapter?.Connection.Close();
            tAM.pValuesTableAdapter?.Dispose();

            tAM.k0NAATableAdapter?.Connection.Close();
            tAM.k0NAATableAdapter?.Dispose();

            tAM.YieldsTableAdapter?.Connection.Close();
            tAM.YieldsTableAdapter?.Dispose();

            tAM.SchAcqsTableAdapter?.Connection.Close();
            tAM.SchAcqsTableAdapter?.Dispose();

            tAM.YieldsTableAdapter = null;
            tAM.ElementsTableAdapter = null;
            tAM.SigmasTableAdapter = null;
            tAM.SigmasSalTableAdapter = null;
            tAM.ReactionsTableAdapter = null;
            tAM.NAATableAdapter = null;
            tAM.pValuesTableAdapter = null;
            tAM.k0NAATableAdapter = null;
            tAM.SchAcqsTableAdapter = null;
            tAM.tStudentTableAdapter = null;
        }

        protected internal void disposeSampleAdapters()
        {
            tAM.UnitTableAdapter?.Connection.Close();

            tAM.UnitTableAdapter?.Dispose();

            tAM.MonitorsFlagsTableAdapter?.Connection.Close();
            tAM.MonitorsFlagsTableAdapter?.Dispose();

            tAM.MonitorsTableAdapter?.Connection.Close();

            tAM.MonitorsTableAdapter?.Dispose();

            tAM.SamplesTableAdapter?.Connection.Close();
            tAM.SamplesTableAdapter?.Dispose();

            tAM.StandardsTableAdapter?.Connection.Close();
            tAM.StandardsTableAdapter?.Dispose();

            tAM.SubSamplesTableAdapter?.Connection.Close();
            tAM.SubSamplesTableAdapter?.Dispose();

            // tAM.MatSSFTableAdapter = null;
            tAM.UnitTableAdapter = null;
            tAM.MonitorsFlagsTableAdapter = null;
            tAM.MonitorsTableAdapter = null;
            tAM.SamplesTableAdapter = null;
            tAM.StandardsTableAdapter = null;
            tAM.SubSamplesTableAdapter = null;
        }

        protected internal void initializeIrradiationAdapters()
        {
            // tAM.CapsulesTableAdapter = new LINAATableAdapters.CapsulesTableAdapter();
            tAM.ChannelsTableAdapter = new ChannelsTableAdapter();
            adapters.Add(tAM.ChannelsTableAdapter, tAM.ChannelsTableAdapter);
            tAM.IrradiationRequestsTableAdapter = new IrradiationRequestsTableAdapter();
            adapters.Add(tAM.IrradiationRequestsTableAdapter, tAM.IrradiationRequestsTableAdapter);
            tAM.OrdersTableAdapter = new OrdersTableAdapter();
            adapters.Add(tAM.OrdersTableAdapter, tAM.OrdersTableAdapter);
            tAM.ProjectsTableAdapter = new ProjectsTableAdapter();
            adapters.Add(tAM.ProjectsTableAdapter, tAM.ProjectsTableAdapter);
        }

        protected internal void InitializeOtherAdapters()
        {
            tAM.ElementsTableAdapter = new ElementsTableAdapter();
            adapters.Add(tAM.ElementsTableAdapter, tAM.ElementsTableAdapter);
            tAM.SigmasTableAdapter = new SigmasTableAdapter();
            adapters.Add(tAM.SigmasTableAdapter, tAM.SigmasTableAdapter);
            tAM.SigmasSalTableAdapter = new SigmasSalTableAdapter();
            adapters.Add(tAM.SigmasSalTableAdapter, tAM.SigmasSalTableAdapter);
            tAM.ReactionsTableAdapter = new ReactionsTableAdapter();
            adapters.Add(tAM.ReactionsTableAdapter, tAM.ReactionsTableAdapter);
            tAM.NAATableAdapter = new NAATableAdapter();
            adapters.Add(tAM.NAATableAdapter, tAM.NAATableAdapter);
            tAM.pValuesTableAdapter = new pValuesTableAdapter();
            adapters.Add(tAM.pValuesTableAdapter, tAM.pValuesTableAdapter);
            tAM.k0NAATableAdapter = new k0NAATableAdapter();
            adapters.Add(tAM.k0NAATableAdapter, tAM.k0NAATableAdapter);
            tAM.SchAcqsTableAdapter = new SchAcqsTableAdapter();
            adapters.Add(tAM.SchAcqsTableAdapter, tAM.SchAcqsTableAdapter);
            tAM.YieldsTableAdapter = new YieldsTableAdapter();
            adapters.Add(tAM.YieldsTableAdapter, tAM.YieldsTableAdapter);
            tAM.tStudentTableAdapter = new tStudentTableAdapter();
            adapters.Add(tAM.tStudentTableAdapter, tAM.tStudentTableAdapter);
            // tAM.LINESTableAdapter = new LINAATableAdapters.LINESTableAdapter();
            // adapters.Add(tAM.LINESTableAdapter, tAM.LINESTableAdapter); tAM.LINES_FISTableAdapter
            // = new LINAATableAdapters.LINES_FISTableAdapter();
            // adapters.Add(tAM.LINES_FISTableAdapter, tAM.LINES_FISTableAdapter);
        }

        protected internal void initializeSampleAdapters()
        {
            tAM.UnitTableAdapter = new UnitTableAdapter();
            adapters.Add(tAM.UnitTableAdapter, tAM.UnitTableAdapter);
            tAM.MonitorsFlagsTableAdapter = new MonitorsFlagsTableAdapter();
            adapters.Add(tAM.MonitorsFlagsTableAdapter, tAM.MonitorsFlagsTableAdapter);
            tAM.MonitorsTableAdapter = new MonitorsTableAdapter();
            adapters.Add(tAM.MonitorsTableAdapter, tAM.MonitorsTableAdapter);
            tAM.SamplesTableAdapter = new SamplesTableAdapter();
            adapters.Add(tAM.SamplesTableAdapter, tAM.SamplesTableAdapter);
            tAM.StandardsTableAdapter = new StandardsTableAdapter();
            adapters.Add(tAM.StandardsTableAdapter, tAM.StandardsTableAdapter);
            tAM.SubSamplesTableAdapter = new SubSamplesTableAdapter();
            adapters.Add(tAM.SubSamplesTableAdapter, tAM.SubSamplesTableAdapter);

            tAM.SubSamplesTableAdapter.Adapter.AcceptChangesDuringUpdate = true;

            // tAM.SubSamplesTableAdapter.Adapter.FillError += new
            // System.Data.FillErrorEventHandler(Adapter_FillError);
            // tAM.SubSamplesTableAdapter.Adapter.RowUpdating += new System.Data.SqlClient.SqlRowUpdatingEventHandler(Adapter_RowUpdating);
        }

        protected internal void initializeToDoAdapters()
        {
            tAM.ToDoTableAdapter = new ToDoTableAdapter();
            adapters.Add(tAM.ToDoTableAdapter, tAM.ToDoTableAdapter);
        }
    }
}