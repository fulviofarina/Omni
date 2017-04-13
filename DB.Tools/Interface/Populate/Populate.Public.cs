using System;
using System.Data.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Rsx;

namespace DB.Tools
{
   
    public partial class Populate
    {

      

        public IDetSol IDetSol;
        public IGeometry IGeometry;
        public IIrradiations IIrradiations;

        // public IExpressions IExpressions;
        public INuclear INuclear;

        public IOrders IOrders;
        public IProjects IProjects;
        public ISamples ISamples;
        public ISchedAcqs ISchedAcqs;
        public IToDoes IToDoes;
       

        public Populate(ref Interface inter)
        {
            LINAA aux = inter.Get();
            Interface = inter;

            // IExpressions = (IExpressions)aux;
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


    }

  
}