using System;
using System.Data;
using System.IO;
using System.Linq;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class Current
    {
        protected static string WINDOWS_USER = "WindowsUser";
        protected static string XML_EXT = ".xml";

        protected internal Func<DataRow, bool> selector
        {
            get
            {
                string label = WINDOWS_USER;
                return p => p.Field<string>(label).CompareTo(WindowsUser) == 0;
            }
        }

        public bool IsSpectraPathOk
        {
            get
            {
                string spec = CurrentPref.Spectra;
                if (string.IsNullOrEmpty(spec)) return false;
                else return Directory.Exists(spec);
            }
        }

        /// <summary>
        /// The current SSF Preferences
        /// </summary>
        public SSFPrefRow CurrentSSFPref
        {
            get
            {
                // currentSSFPref = Interface.IDB.SSFPref.FirstOrDefault(selector) as SSFPrefRow;
                return Interface.IDB.SSFPref.FirstOrDefault(selector) as SSFPrefRow;
            }
        }

        /// <summary>
        /// The current preferences (Main)
        /// </summary>
        public PreferencesRow CurrentPref
        {
            get
            {
                // currentPref = Interface.IDB.Preferences.FirstOrDefault(selector) as PreferencesRow;
                return Interface.IDB.Preferences.FirstOrDefault(selector) as PreferencesRow;
            }
        }
    }
}