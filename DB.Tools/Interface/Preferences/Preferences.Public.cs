using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using DB.Properties;
using Rsx;
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
                currentSSFPref = Interface.IDB.SSFPref.FirstOrDefault(selector) as SSFPrefRow;
                return currentSSFPref;
            }
        }

        /// <summary>
        /// The current preferences (Main)
        /// </summary>
        public PreferencesRow CurrentPref
        {
            get
            {
                currentPref = Interface.IDB.Preferences.FirstOrDefault(selector) as PreferencesRow;
                return currentPref;
            }
        }

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
                loadCurrentPreferences<PreferencesDataTable>();
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
            }
            try
            {
                bool ok = populatePreferences<SSFPrefDataTable>();
                if (ok)
                {
                    //cleaning
                    cleanPreferences<SSFPrefDataTable>();    //important
                }
                loadCurrentPreferences<SSFPrefDataTable>();
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
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

        public void SavePreferences()
        {
            try
            {
                savePreferences<PreferencesDataTable>();

                savePreferences<SSFPrefDataTable>();
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
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