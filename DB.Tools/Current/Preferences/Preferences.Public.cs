using System;
using System.Security.Principal;
using DB.Properties;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    public partial class Current : IPreferences
    {

       

        public string GetSSFPreferencesPath()
        {
            return Interface.IStore.FolderPath + Resources.SSFPreferences + XML_EXT;
        }

        public string GetPreferencesPath()
        {
            return Interface.IStore.FolderPath + Resources.Preferences + XML_EXT;
        }

        public void RejectPreferencesChanges()
        {
            Interface.IPreferences.CurrentPref.RejectChanges();
          //  Interface.IPreferences.CurrentSSFPref.RejectChanges();
        }
        public void RejectSSFChanges()
        {
           // Interface.IPreferences.CurrentPref.RejectChanges();
            Interface.IPreferences.CurrentSSFPref.RejectChanges();
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
                    cleanNullPreferences<PreferencesDataTable>();    //important
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
                    cleanNullPreferences<SSFPrefDataTable>();    //important
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

        public void AcceptChanges()
        {
            Interface.IDB.Preferences.AcceptChanges();
            Interface.IDB.SSFPref.AcceptChanges();
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