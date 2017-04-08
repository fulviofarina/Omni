using System.Collections.Generic;
using System.Data;
using Rsx;

namespace DB
{
    public partial class Interface
    {
        private LINAA interf = null;

        public Interface(ref LINAA aux)
        {
            interf = aux;
            IBS = new BindingSources();
            ICurrent = new Current(ref IBS);
            IPopulate = new Populate(ref aux);
            IMain = (IMain)aux;
            IAdapter = (IAdapter)aux;
            IStore = (IStore)aux;
            IReport = (IReport)aux;
            IDB = (IDB)aux;

            IPreferences = (IPreferences)aux;
        }

        public LINAA Get()
        {
            return interf;
        }

        public IMain IMain;
        public IAdapter IAdapter;
        public IStore IStore;
        public IReport IReport;
        public IDB IDB;
        public IPreferences IPreferences;

        public BindingSources IBS;
        public Populate IPopulate;
        public Current ICurrent;
    }

    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public class BindingSources
    {
        //binding sources to attach;
        public dynamic SubSamples;

        /// <summary>
        /// not attached yet
        /// </summary>
        public dynamic Monitors;

        public dynamic Units;
        public dynamic Matrix;
        public dynamic Vial;
        public dynamic Geometry;
        public dynamic Rabbit;
        public dynamic Channels;
        public dynamic Irradiations;

        public BindingSources()
        {
        }
    }

    public class Current
    {
        private BindingSources bs;

        public Current(ref BindingSources bsources)
        {
            bs = bsources;
        }

        public IEnumerable<LINAA.SubSamplesRow> SubSamples
        {
            get
            {
                return Dumb.Cast<LINAA.SubSamplesRow>(bs.SubSamples.List as DataView);
            }
        }
    }

    public class Populate
    {
        public Populate(ref LINAA aux)
        {
            IExpressions = (IExpressions)aux;
            INuclear = (INuclear)aux;
            IProjects = (IProjects)aux;
            IIrradiations = (IIrradiations)aux;
            IGeometry = (IGeometry)aux;
            IDetSol = (IDetSol)aux;
            IOrders = (IOrders)aux;
            ISamples = (ISamples)aux;
            ISchedAcqs = (ISchedAcqs)aux;
            IToDoes = (IToDoes)aux;
        }

        public ISchedAcqs ISchedAcqs;
        public IExpressions IExpressions;
        public INuclear INuclear;
        public IProjects IProjects;
        public IIrradiations IIrradiations;
        public IGeometry IGeometry;
        public IDetSol IDetSol;
        public IOrders IOrders;
        public ISamples ISamples;
        public IToDoes IToDoes;
    }
}