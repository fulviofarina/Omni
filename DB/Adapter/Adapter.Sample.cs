//using DB.Interfaces;

using System.Collections;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected static void disposeSampleAdapters(ref LINAATableAdapters.TableAdapterManager tAM)
        {
            //    if (tAM.UnitTableAdapter != null)
            {
                tAM.UnitTableAdapter?.Connection.Close();

                tAM.UnitTableAdapter?.Dispose();
            }

            //    if (tAM.MatSSFTableAdapter != null)
            {
                tAM.MatSSFTableAdapter?.Connection.Close();

                tAM.MatSSFTableAdapter?.Dispose();
            }
            //   if (tAM.MonitorsFlagsTableAdapter != null)
            {
                tAM.MonitorsFlagsTableAdapter?.Connection.Close();
                tAM.MonitorsFlagsTableAdapter?.Dispose();
            }
            //  if (tAM.MonitorsTableAdapter != null)
            {
                tAM.MonitorsTableAdapter?.Connection.Close();

                tAM.MonitorsTableAdapter?.Dispose();
            }
            //  if (tAM.SamplesTableAdapter != null)
            {
                tAM.SamplesTableAdapter?.Connection.Close();
                tAM.SamplesTableAdapter?.Dispose();
            }
            //  if (tAM.StandardsTableAdapter != null)
            {
                tAM.StandardsTableAdapter?.Connection.Close();
                tAM.StandardsTableAdapter?.Dispose();
            }
            //   if (tAM.SubSamplesTableAdapter != null)
            {
                tAM.SubSamplesTableAdapter?.Connection.Close();
                tAM.SubSamplesTableAdapter?.Dispose();
            }

            tAM.MatSSFTableAdapter = null;
            tAM.UnitTableAdapter = null;
            tAM.MonitorsFlagsTableAdapter = null;
            tAM.MonitorsTableAdapter = null;
            tAM.SamplesTableAdapter = null;
            tAM.StandardsTableAdapter = null;
            tAM.SubSamplesTableAdapter = null;
        }

        protected static void initializeSampleAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            tAM.MatSSFTableAdapter = new LINAATableAdapters.MatSSFTableAdapter();
            adapters.Add(tAM.MatSSFTableAdapter, tAM.MatSSFTableAdapter);
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

            // tAM.SubSamplesTableAdapter.Adapter.FillError += new System.Data.FillErrorEventHandler(Adapter_FillError);
            // tAM.SubSamplesTableAdapter.Adapter.RowUpdating += new System.Data.SqlClient.SqlRowUpdatingEventHandler(Adapter_RowUpdating);
        }
    }
}