//using DB.Interfaces;

using System.Collections;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected void disposeMainAdapters()
        {
            QTA?.Dispose();
            QTA = null;

            //if (tAM != null)
            //{
            tAM?.Connection.Close();
            tAM?.Dispose();
            //}
            tAM = null;
        }

        protected static void disposeOtherAdapters(ref LINAATableAdapters.TableAdapterManager tAM)
        {
            //   if (tAM.ElementsTableAdapter != null)
            //   {
            tAM.ElementsTableAdapter?.Connection.Close();

            tAM.ElementsTableAdapter?.Dispose();
            //   }

            //  if (tAM.SigmasTableAdapter != null)
            //   {
            tAM.SigmasTableAdapter?.Connection.Close();

            tAM.SigmasTableAdapter?.Dispose();
            //   }
            //  if (tAM.SigmasSalTableAdapter != null)
            //  {
            tAM.SigmasSalTableAdapter?.Connection.Close();
            tAM.SigmasSalTableAdapter?.Dispose();
            //  }
            //  if (tAM.ReactionsTableAdapter != null)
            //    {
            tAM.ReactionsTableAdapter?.Connection.Close();
            tAM.ReactionsTableAdapter?.Dispose();
            //  }
            //  if (tAM.NAATableAdapter != null)
            //  {
            tAM.NAATableAdapter?.Connection.Close();

            tAM.NAATableAdapter?.Dispose();
            //  }
            //if (tAM.pValuesTableAdapter != null)
            //  {
            tAM.pValuesTableAdapter?.Connection.Close();
            tAM.pValuesTableAdapter?.Dispose();
            //  }
            //  if (tAM.k0NAATableAdapter != null)
            //  {
            tAM.k0NAATableAdapter?.Connection.Close();
            tAM.k0NAATableAdapter?.Dispose();
            //  }
            //  if (tAM.YieldsTableAdapter != null)
            //  {
            tAM.YieldsTableAdapter?.Connection.Close();
            tAM.YieldsTableAdapter?.Dispose();
            // }

            //  if (tAM.SchAcqsTableAdapter != null)
            // {
            //{
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
        }

        private static void disposePeaksAdapters(ref LINAATableAdapters.TableAdapterManager TAM)
        {
            // this.tAM.IRequestsAveragesTableAdapter.Connection.Close();
            // this.tAM.IPeakAveragesTableAdapter.Connection.Close();

            //      if (TAM.MeasurementsTableAdapter != null)
            //{
            TAM.MeasurementsTableAdapter?.Connection.Close();
            TAM.MeasurementsTableAdapter?.Dispose();
            //  }
            //  if (TAM.PeaksTableAdapter != null)
            //  {
            TAM.PeaksTableAdapter?.Connection.Close();
            TAM.PeaksTableAdapter?.Dispose();
            //  }
            //  if (TAM.ToDoTableAdapter != null)
            //  {
            TAM.ToDoTableAdapter?.Connection.Close();
            TAM.ToDoTableAdapter?.Dispose();
            //  }
            // if (TAM.IRequestsAveragesTableAdapter != null) TAM.IRequestsAveragesTableAdapter.Dispose();
            //	 if (TAM.IPeakAveragesTableAdapter != null) TAM.IPeakAveragesTableAdapter.Dispose();

            TAM.MeasurementsTableAdapter = null;
            TAM.PeaksTableAdapter = null;
            TAM.ToDoTableAdapter = null;
            //	 this.tAM.IRequestsAveragesTableAdapter = null;
            //	 this.tAM.IPeakAveragesTableAdapter = null;
        }

        protected static void initializePeaksAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            tAM.MeasurementsTableAdapter = new LINAATableAdapters.MeasurementsTableAdapter();
            adapters.Add(tAM.MeasurementsTableAdapter, tAM.MeasurementsTableAdapter);
            tAM.PeaksTableAdapter = new LINAATableAdapters.PeaksTableAdapter();
            adapters.Add(tAM.PeaksTableAdapter, tAM.PeaksTableAdapter);
            // this.tAM.IRequestsAveragesTableAdapter = new LINAATableAdapters.IRequestsAveragesTableAdapter();
            //this.tAM.IPeakAveragesTableAdapter = new LINAATableAdapters.IPeakAveragesTableAdapter();
        }

        protected static void initializeToDoAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            tAM.ToDoTableAdapter = new LINAATableAdapters.ToDoTableAdapter();
            adapters.Add(tAM.ToDoTableAdapter, tAM.ToDoTableAdapter);
        }

        protected static void InitializeOtherAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
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
        }

        protected static void disposeIrradiationAdapters(ref LINAATableAdapters.TableAdapterManager tAM)
        {
            // tAM.CapsulesTableAdapter.Connection.Close();

            // if (tAM.CapsulesTableAdapter != null) tAM.CapsulesTableAdapter.Dispose();
            //   if (tAM.ChannelsTableAdapter != null)
            //   {
            tAM.ChannelsTableAdapter?.Connection.Close();
            tAM.ChannelsTableAdapter?.Dispose();
            //  }
            //  if (tAM.IrradiationRequestsTableAdapter != null)
            //  {
            tAM.IrradiationRequestsTableAdapter?.Connection.Close();
            tAM.IrradiationRequestsTableAdapter?.Dispose();
            //   }
            //    if (tAM.OrdersTableAdapter != null)
            //    {
            tAM.OrdersTableAdapter?.Connection.Close();
            tAM.OrdersTableAdapter?.Dispose();
            //   }
            //   if (tAM.ProjectsTableAdapter != null)
            //   {
            tAM.ProjectsTableAdapter?.Connection.Close();
            tAM.ProjectsTableAdapter?.Dispose();
            //    }

            // tAM.CapsulesTableAdapter = null;
            tAM.ChannelsTableAdapter = null;
            tAM.IrradiationRequestsTableAdapter = null;
            tAM.OrdersTableAdapter = null;
            tAM.ProjectsTableAdapter = null;
        }

        protected static void initializeIrradiationAdapters(ref LINAATableAdapters.TableAdapterManager tAM, ref Hashtable adapters)
        {
            //	 tAM.CapsulesTableAdapter = new LINAATableAdapters.CapsulesTableAdapter();
            tAM.ChannelsTableAdapter = new LINAATableAdapters.ChannelsTableAdapter();
            adapters.Add(tAM.ChannelsTableAdapter, tAM.ChannelsTableAdapter);
            tAM.IrradiationRequestsTableAdapter = new LINAATableAdapters.IrradiationRequestsTableAdapter();
            adapters.Add(tAM.IrradiationRequestsTableAdapter, tAM.IrradiationRequestsTableAdapter);
            tAM.OrdersTableAdapter = new LINAATableAdapters.OrdersTableAdapter();
            adapters.Add(tAM.OrdersTableAdapter, tAM.OrdersTableAdapter);
            tAM.ProjectsTableAdapter = new LINAATableAdapters.ProjectsTableAdapter();
            adapters.Add(tAM.ProjectsTableAdapter, tAM.ProjectsTableAdapter);
        }
    }
}