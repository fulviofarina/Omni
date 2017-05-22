//using DB.Interfaces;

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA : IAdapter
    {



        protected internal static bool removeDuplicates(DataTable table, string UniqueField, string IndexField, ref TAMDeleteMethod remover)
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



        protected internal static void disposeIrradiationAdapters(ref LINAATableAdapters.TableAdapterManager tAM)
        {
            // tAM.CapsulesTableAdapter.Connection.Close();

            // if (tAM.CapsulesTableAdapter != null) tAM.CapsulesTableAdapter.Dispose(); if
            // (tAM.ChannelsTableAdapter != null) {
            tAM.ChannelsTableAdapter?.Connection.Close();
            tAM.ChannelsTableAdapter?.Dispose();
            // } if (tAM.IrradiationRequestsTableAdapter != null) {
            tAM.IrradiationRequestsTableAdapter?.Connection.Close();
            tAM.IrradiationRequestsTableAdapter?.Dispose();
            // } if (tAM.OrdersTableAdapter != null) {
            tAM.OrdersTableAdapter?.Connection.Close();
            tAM.OrdersTableAdapter?.Dispose();
            // } if (tAM.ProjectsTableAdapter != null) {
            tAM.ProjectsTableAdapter?.Connection.Close();
            tAM.ProjectsTableAdapter?.Dispose();
            // }

            // tAM.CapsulesTableAdapter = null;
            tAM.ChannelsTableAdapter = null;
            tAM.IrradiationRequestsTableAdapter = null;
            tAM.OrdersTableAdapter = null;
            tAM.ProjectsTableAdapter = null;
        }

        protected internal static void disposeOtherAdapters(ref LINAATableAdapters.TableAdapterManager tAM)
        {
            // if (tAM.ElementsTableAdapter != null) {
            tAM.ElementsTableAdapter?.Connection.Close();

            tAM.ElementsTableAdapter?.Dispose();
            // }

            // if (tAM.SigmasTableAdapter != null) {
            tAM.SigmasTableAdapter?.Connection.Close();

            tAM.SigmasTableAdapter?.Dispose();
            // } if (tAM.SigmasSalTableAdapter != null) {
            tAM.SigmasSalTableAdapter?.Connection.Close();
            tAM.SigmasSalTableAdapter?.Dispose();
            // } if (tAM.ReactionsTableAdapter != null) {
            tAM.ReactionsTableAdapter?.Connection.Close();
            tAM.ReactionsTableAdapter?.Dispose();
            // } if (tAM.NAATableAdapter != null) {
            tAM.NAATableAdapter?.Connection.Close();

            tAM.NAATableAdapter?.Dispose();

            tAM.tStudentTableAdapter?.Connection.Close();
            tAM.tStudentTableAdapter?.Dispose();

          //  tAM.LINES_FISTableAdapter?.Connection.Close();
          //  tAM.LINES_FISTableAdapter?.Dispose();

         //   tAM.LINESTableAdapter?.Connection.Close();
         //   tAM.LINESTableAdapter?.Dispose();

            //  }
            //if (tAM.pValuesTableAdapter != null)
            //  {
            tAM.pValuesTableAdapter?.Connection.Close();
            tAM.pValuesTableAdapter?.Dispose();
            // } if (tAM.k0NAATableAdapter != null) {
            tAM.k0NAATableAdapter?.Connection.Close();
            tAM.k0NAATableAdapter?.Dispose();
            // } if (tAM.YieldsTableAdapter != null) {
            tAM.YieldsTableAdapter?.Connection.Close();
            tAM.YieldsTableAdapter?.Dispose();
            // }

            //  if (tAM.SchAcqsTableAdapter != null)
            // {
            //{
            tAM.SchAcqsTableAdapter?.Connection.Close();
            tAM.SchAcqsTableAdapter?.Dispose();
            //}
            // }
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

        protected internal static void disposePeaksAdapters(ref LINAATableAdapters.TableAdapterManager TAM)
        {
            // this.tAM.IRequestsAveragesTableAdapter.Connection.Close(); this.tAM.IPeakAveragesTableAdapter.Connection.Close();

            //      if (TAM.MeasurementsTableAdapter != null)
            //{
            TAM.MeasurementsTableAdapter?.Connection.Close();
            TAM.MeasurementsTableAdapter?.Dispose();
            // } if (TAM.PeaksTableAdapter != null) {
            TAM.PeaksTableAdapter?.Connection.Close();
            TAM.PeaksTableAdapter?.Dispose();
            // } if (TAM.ToDoTableAdapter != null) {
            TAM.ToDoTableAdapter?.Connection.Close();
            TAM.ToDoTableAdapter?.Dispose();
            // } if (TAM.IRequestsAveragesTableAdapter != null)
            // TAM.IRequestsAveragesTableAdapter.Dispose(); if (TAM.IPeakAveragesTableAdapter !=
            // null) TAM.IPeakAveragesTableAdapter.Dispose();

            TAM.MeasurementsTableAdapter = null;
            TAM.PeaksTableAdapter = null;
            TAM.ToDoTableAdapter = null;
            // this.tAM.IRequestsAveragesTableAdapter = null; this.tAM.IPeakAveragesTableAdapter = null;
        }

        protected internal static void disposeSampleAdapters(ref LINAATableAdapters.TableAdapterManager tAM)
        {
            // if (tAM.UnitTableAdapter != null)
            {
                tAM.UnitTableAdapter?.Connection.Close();

                tAM.UnitTableAdapter?.Dispose();
            }

            // if (tAM.MatSSFTableAdapter != null)
       //     {
        //        tAM.MatSSFTableAdapter?.Connection.Close();

          //      tAM.MatSSFTableAdapter?.Dispose();
         //   }
            // if (tAM.MonitorsFlagsTableAdapter != null)
            {
                tAM.MonitorsFlagsTableAdapter?.Connection.Close();
                tAM.MonitorsFlagsTableAdapter?.Dispose();
            }
            // if (tAM.MonitorsTableAdapter != null)
            {
                tAM.MonitorsTableAdapter?.Connection.Close();

                tAM.MonitorsTableAdapter?.Dispose();
            }
            // if (tAM.SamplesTableAdapter != null)
            {
                tAM.SamplesTableAdapter?.Connection.Close();
                tAM.SamplesTableAdapter?.Dispose();
            }
            // if (tAM.StandardsTableAdapter != null)
            {
                tAM.StandardsTableAdapter?.Connection.Close();
                tAM.StandardsTableAdapter?.Dispose();
            }
            // if (tAM.SubSamplesTableAdapter != null)
            {
                tAM.SubSamplesTableAdapter?.Connection.Close();
                tAM.SubSamplesTableAdapter?.Dispose();
            }

        //    tAM.MatSSFTableAdapter = null;
            tAM.UnitTableAdapter = null;
            tAM.MonitorsFlagsTableAdapter = null;
            tAM.MonitorsTableAdapter = null;
            tAM.SamplesTableAdapter = null;
            tAM.StandardsTableAdapter = null;
            tAM.SubSamplesTableAdapter = null;
        }

        protected internal static void initializeIrradiationAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            // tAM.CapsulesTableAdapter = new LINAATableAdapters.CapsulesTableAdapter();
            tAM.ChannelsTableAdapter = new LINAATableAdapters.ChannelsTableAdapter();
            adapters.Add(tAM.ChannelsTableAdapter, tAM.ChannelsTableAdapter);
            tAM.IrradiationRequestsTableAdapter = new LINAATableAdapters.IrradiationRequestsTableAdapter();
            adapters.Add(tAM.IrradiationRequestsTableAdapter, tAM.IrradiationRequestsTableAdapter);
            tAM.OrdersTableAdapter = new LINAATableAdapters.OrdersTableAdapter();
            adapters.Add(tAM.OrdersTableAdapter, tAM.OrdersTableAdapter);
            tAM.ProjectsTableAdapter = new LINAATableAdapters.ProjectsTableAdapter();
            adapters.Add(tAM.ProjectsTableAdapter, tAM.ProjectsTableAdapter);
        }

        protected internal static void InitializeOtherAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            tAM.ElementsTableAdapter = new LINAATableAdapters.ElementsTableAdapter();
            adapters.Add(tAM.ElementsTableAdapter, tAM.ElementsTableAdapter);
            tAM.SigmasTableAdapter = new LINAATableAdapters.SigmasTableAdapter();
            adapters.Add(tAM.SigmasTableAdapter, tAM.SigmasTableAdapter);
            tAM.SigmasSalTableAdapter = new LINAATableAdapters.SigmasSalTableAdapter();
            adapters.Add(tAM.SigmasSalTableAdapter, tAM.SigmasSalTableAdapter);
            tAM.ReactionsTableAdapter = new LINAATableAdapters.ReactionsTableAdapter();
            adapters.Add(tAM.ReactionsTableAdapter, tAM.ReactionsTableAdapter);
            tAM.NAATableAdapter = new LINAATableAdapters.NAATableAdapter();
            adapters.Add(tAM.NAATableAdapter, tAM.NAATableAdapter);
            tAM.pValuesTableAdapter = new LINAATableAdapters.pValuesTableAdapter();
            adapters.Add(tAM.pValuesTableAdapter, tAM.pValuesTableAdapter);
            tAM.k0NAATableAdapter = new LINAATableAdapters.k0NAATableAdapter();
            adapters.Add(tAM.k0NAATableAdapter, tAM.k0NAATableAdapter);
            tAM.SchAcqsTableAdapter = new LINAATableAdapters.SchAcqsTableAdapter();
            adapters.Add(tAM.SchAcqsTableAdapter, tAM.SchAcqsTableAdapter);
            tAM.YieldsTableAdapter = new LINAATableAdapters.YieldsTableAdapter();
            adapters.Add(tAM.YieldsTableAdapter, tAM.YieldsTableAdapter);
            tAM.tStudentTableAdapter = new LINAATableAdapters.tStudentTableAdapter();
            adapters.Add(tAM.tStudentTableAdapter, tAM.tStudentTableAdapter);
        //    tAM.LINESTableAdapter = new LINAATableAdapters.LINESTableAdapter();
          //  adapters.Add(tAM.LINESTableAdapter, tAM.LINESTableAdapter);
           // tAM.LINES_FISTableAdapter = new LINAATableAdapters.LINES_FISTableAdapter();
          //  adapters.Add(tAM.LINES_FISTableAdapter, tAM.LINES_FISTableAdapter);
        }

        protected internal static void initializePeaksAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            tAM.MeasurementsTableAdapter = new LINAATableAdapters.MeasurementsTableAdapter();
            adapters.Add(tAM.MeasurementsTableAdapter, tAM.MeasurementsTableAdapter);
            tAM.PeaksTableAdapter = new LINAATableAdapters.PeaksTableAdapter();
            adapters.Add(tAM.PeaksTableAdapter, tAM.PeaksTableAdapter);
            // this.tAM.IRequestsAveragesTableAdapter = new LINAATableAdapters.IRequestsAveragesTableAdapter();
            //this.tAM.IPeakAveragesTableAdapter = new LINAATableAdapters.IPeakAveragesTableAdapter();
        }

        protected internal static void initializeSampleAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
        //    tAM.MatSSFTableAdapter = new LINAATableAdapters.MatSSFTableAdapter();
        //    adapters.Add(tAM.MatSSFTableAdapter, tAM.MatSSFTableAdapter);
            tAM.UnitTableAdapter = new LINAATableAdapters.UnitTableAdapter();
            adapters.Add(tAM.UnitTableAdapter, tAM.UnitTableAdapter);
            tAM.MonitorsFlagsTableAdapter = new LINAATableAdapters.MonitorsFlagsTableAdapter();
            adapters.Add(tAM.MonitorsFlagsTableAdapter, tAM.MonitorsFlagsTableAdapter);
            tAM.MonitorsTableAdapter = new LINAATableAdapters.MonitorsTableAdapter();
            adapters.Add(tAM.MonitorsTableAdapter, tAM.MonitorsTableAdapter);
            tAM.SamplesTableAdapter = new LINAATableAdapters.SamplesTableAdapter();
            adapters.Add(tAM.SamplesTableAdapter, tAM.SamplesTableAdapter);
            tAM.StandardsTableAdapter = new LINAATableAdapters.StandardsTableAdapter();
            adapters.Add(tAM.StandardsTableAdapter, tAM.StandardsTableAdapter);
            tAM.SubSamplesTableAdapter = new LINAATableAdapters.SubSamplesTableAdapter();
            adapters.Add(tAM.SubSamplesTableAdapter, tAM.SubSamplesTableAdapter);

            tAM.SubSamplesTableAdapter.Adapter.AcceptChangesDuringUpdate = true;

            // tAM.SubSamplesTableAdapter.Adapter.FillError += new
            // System.Data.FillErrorEventHandler(Adapter_FillError);
            // tAM.SubSamplesTableAdapter.Adapter.RowUpdating += new System.Data.SqlClient.SqlRowUpdatingEventHandler(Adapter_RowUpdating);
        }

        protected internal static void initializeToDoAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            tAM.ToDoTableAdapter = new LINAATableAdapters.ToDoTableAdapter();
            adapters.Add(tAM.ToDoTableAdapter, tAM.ToDoTableAdapter);
        }

        protected internal void disposeComponent()
        {
            QTA?.Dispose();
            QTA = null;

            //if (tAM != null)
            //{
            tAM?.Connection?.Close();
            tAM?.Dispose();
            //}
            tAM = null;
        }
    }
}