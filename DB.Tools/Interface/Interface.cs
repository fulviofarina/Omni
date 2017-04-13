namespace DB.Tools
{
    public partial class Interface
    {
        /// <summary>
        /// is it used?
        /// </summary>
        private LINAA dataset = null;

        /// <summary>
        /// The main object to work with and access all members
        /// </summary>
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

        /// <summary>
        /// Gets the LINAA database
        /// </summary>
        public LINAA Get()
        {
            return dataset;
        }

        /// <summary>
        /// Main stuff
        /// </summary>
        public IMain IMain;
        /// <summary>
        /// For the adapters
        /// </summary>
        public IAdapter IAdapter;
        /// <summary>
        /// The Save interface
        /// </summary>
        public IStore IStore;
        /// <summary>
        /// The database tables
        /// </summary>
        public IDB IDB;

        /// <summary>
        /// other interfaces of this cl
        /// </summary>
        public IPreferences IPreferences;

        /// <summary>
        /// The reporter class
        /// </summary>
        public IReport IReport;

        /// <summary>
        /// For the binding sources
        /// </summary>
        public BindingSources IBS;
        /// <summary>
        /// The populator, by section
        /// </summary>
        public Populate IPopulate;
        /// <summary>
        /// Current DataRows
        /// </summary>
        public Current ICurrent;
    }
}