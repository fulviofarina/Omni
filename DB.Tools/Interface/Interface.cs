namespace DB.Tools
{
    public partial class Interface
    {
        private LINAA dataset = null;

        public Interface(ref LINAA aux)
        {
            dataset = aux;

            //attach interface classes
            Interface inter = this;

            IBS = new BindingSources(ref inter);
            IReport = new Report(ref inter);
            IPopulate = new Populate(ref inter);
            ICurrent = new Current(ref IBS, ref inter);
            IPreferences = ICurrent;

            //attach interfaces of LINAA (DB)
            IMain = (IMain)aux;
            IAdapter = (IAdapter)aux;
            IStore = (IStore)aux;
            IDB = (IDB)aux;
        }

        public LINAA Get()
        {
            return dataset;
        }

        public IMain IMain;
        public IAdapter IAdapter;
        public IStore IStore;
        public IDB IDB;

        /// <summary>
        /// other interfaces of this cl
        /// </summary>
        public IPreferences IPreferences;

        public IReport IReport;

        public BindingSources IBS;
        public Populate IPopulate;
        public Current ICurrent;
    }
}