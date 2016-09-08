using System.Collections.Generic;
using System.Linq;

//using DB.Interfaces;
using System.IO;
using Rsx;
using System.Data;
using System.Data.Linq;
using DB.Properties;
namespace DB
{
    public partial class LINAA : IPreferences
    {
        private PreferencesRow currentPref;

        public PreferencesRow CurrentPref
        {
            get
            {
                return currentPref;
            }
            set { currentPref = value; }
        }
        private SSFPrefRow currentSSFPref;

        public SSFPrefRow CurrentSSFPref
        {
            get
            {
                return currentSSFPref;
            }
            set { currentSSFPref = value; }
        }


     


        public void PopulatePreferences()
        {

            
          //  string prefFolder=   Resources.Preferences;
            string path = folderPath + Resources.Preferences;
           
            DataTable dt = this.Preferences;
            //keep this this way, works fine

            //load
            bool ok = Dumb.ReadTable(path, ref dt);

            if (ok)
            {
                //cleaning
                cleanPreferences<PreferencesDataTable>();    //important
            }

            loadCurrentPreferences<PreferencesRow>();


            //keep this this way, works fine
             dt = this.SSFPref;
            string pathSSFPref = folderPath + Resources.SSFPreferences;
            //load
            ok = Dumb.ReadTable(pathSSFPref, ref dt);

            //cleaning
            if (ok)
            {
                cleanPreferences<SSFPrefDataTable>();
            }

            loadCurrentPreferences<SSFPrefRow>();


            //find the current preference


        }
    }
}