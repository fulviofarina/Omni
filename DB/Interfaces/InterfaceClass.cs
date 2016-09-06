namespace DB
{
    public partial class Interface
    {
        private object interf = null;

        public Interface(ref object aux)
        {
            interf = aux;

            IPopulate = new Populate(ref aux);
            IAdapter = (IAdapter)aux;
            IStore = (IStore)aux;
            IReport = (IReport)aux;
            IToDoes = (IToDoes)aux;
            //   ICReport =(ICReport)aux;

            IPreferences = (IPreferences)aux;
        }

        public object Get()
        {
            return interf;
        }

        public IAdapter IAdapter;
        public IStore IStore;
        public IReport IReport;
        //  public ICReport ICReport;

        public ITables ITables;
        public IToDoes IToDoes;
        public IPreferences IPreferences;

        public Populate IPopulate;

        public class Populate
        {
            public Populate(ref object aux)
            {
                IMain = (IMain)aux;
                INuclear = (INuclear)aux;
                IProjects = (IProjects)aux;
                IIrradiations = (IIrradiations)aux;
                IGeometry = (IGeometry)aux;
                IDetSol = (IDetSol)aux;
                IOrders = (IOrders)aux;
                ISamples = (ISamples)aux;
                ISchedAcqs = (ISchedAcqs)aux;
            }

            public ISchedAcqs ISchedAcqs;
            public IMain IMain;
            public INuclear INuclear;
            public IProjects IProjects;
            public IIrradiations IIrradiations;
            public IGeometry IGeometry;
            public IDetSol IDetSol;
            public IOrders IOrders;
            public ISamples ISamples;
        }
    }
}