namespace DB.Tools
{
    public partial class Interface
    {
        /// <summary>
        /// Adapters interface
        /// </summary>
        public IAdapter IAdapter;

        /// <summary>
        /// binding Sources interface
        /// </summary>
        public BindingSources IBS;

        /// <summary>
        /// Current rows interface
        /// </summary>
        public ICurrent ICurrent;

        /// <summary>
        /// The database tables
        /// </summary>
        public IDB IDB;

        /// <summary>
        /// Main stuff
        /// </summary>
     //   public IStore IStore;

        /// <summary>
        /// Populator interface
        /// </summary>
        public Populate IPopulate;

        /// <summary>
        /// Preferences interface
        /// </summary>
        public IPreferences IPreferences;

        /// <summary>
        /// The reporter interface
        /// </summary>
        public IReport IReport;

        /// <summary>
        /// The Save interface
        /// </summary>
        public IStore IStore;

        /// <summary>
        /// is it used? not sure
        /// </summary>
        protected LINAA dataset = null;

        /// <summary>
        /// Gets the LINAA database
        /// </summary>
        public LINAA Get()
        {
            return dataset;
        }

        /// <summary>
        /// The main object to work with and access all members
        /// </summary>
        public Interface(ref LINAA aux)
        {
            dataset = aux;
      //      IStore = (IStore)aux;
            IAdapter = (IAdapter)aux;
            IStore = (IStore)aux;
            IDB = (IDB)aux;

            //attach interface classes
            Interface inter = this;

            IBS = new BindingSources(ref inter);
            IReport = new Report(ref inter);
            IPopulate = new Populate(ref inter);
            Current current = new Current(ref IBS, ref inter);
            ICurrent = current;
            IPreferences = current;

            //attach interfaces of LINAA (DB)
        }
    }
}