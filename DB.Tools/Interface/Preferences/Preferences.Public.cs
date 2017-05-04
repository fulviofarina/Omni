using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    public partial class Current : IPreferences
    {
        /// <summary>
        /// Check if the Spectra directory is OK
        /// </summary>
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

        /// <summary>
        /// Populates the preferences
        /// </summary>
        public void PopulatePreferences()
        {
            try
            {
                bool ok = populatePreferences<PreferencesDataTable>();
                if (ok)
                {
                    //cleaning
                    cleanPreferences<PreferencesDataTable>();    //important
                }
                populateCurrentPreferences<PreferencesDataTable>();
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
            try
            {
                bool ok = populatePreferences<SSFPrefDataTable>();
                if (ok)
                {
                    //cleaning
                    cleanPreferences<SSFPrefDataTable>();    //important
                }
                populateCurrentPreferences<SSFPrefDataTable>();
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// The Windows User
        /// </summary>
        public string WindowsUser
        {
            get
            {
                return WindowsIdentity.GetCurrent().Name.ToUpper();
            }
        }

        /// <summary>
        /// Saves the preferences
        /// </summary>
        public void SavePreferences()
        {
            try
            {
                savePreferences<PreferencesDataTable>();

                savePreferences<SSFPrefDataTable>();
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        /*
        public void MergePreferences()
        {
            PreferencesDataTable  prefe = new PreferencesDataTable();

            SSFPrefDataTable ssfPrefe = new SSFPrefDataTable();

            mergePreferences(ref prefe);

            mergePreferences(ref ssfPrefe);

            // this.SavePreferences();
            this.PopulatePreferences();

            Dumb.FD<PreferencesDataTable>(ref prefe);

            Dumb.FD<SSFPrefDataTable>(ref ssfPrefe);
        }
        */
    }
}