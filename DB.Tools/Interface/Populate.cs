namespace DB.Tools
{
    public class Populate
    {
        private LINAA db;

        public void Preferences()
        {
            db.PopulatePreferences();
            //  I.
        }

        public Populate(ref LINAA aux)
        {
            db = aux;
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