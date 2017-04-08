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
}