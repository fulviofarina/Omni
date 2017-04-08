using System.Collections.Generic;
using System.Data;
using Rsx;

namespace DB
{
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