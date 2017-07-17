using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using DB.Properties;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    public partial class Preference : IPreferences
    {
        protected internal Interface Interface;
        public Preference(ref Interface interfaces)
        {
            Interface = interfaces;

            // Interface.IDB.SubSamples.AddMatrixHandler += this.addMatrixEvent;
            // Interface.IPopulate
            Interface.IDB.SubSamples.CalcParametersHandler = getPreferencesEvent;
            Interface.IPopulate.ISamples.SpectrumCalcParametersHandler = getPreferencesSpectrumEvent;

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

        public XCOMPrefRow CurrentXCOMPref
        {
            get
            {
                // currentPref = Interface.IDB.Preferences.FirstOrDefault(selector) as PreferencesRow;
                return Interface.IDB.XCOMPref.FirstOrDefault(selector) as XCOMPrefRow;
            }
        }

        public string GetSSFPreferencesPath()
        {
            return Interface.IStore.FolderPath + Resources.SSFPreferences + XML_EXT;
        }

        public string GetPreferencesPath()
        {
            return Interface.IStore.FolderPath + Resources.Preferences + XML_EXT;
        }

        public string GetXCOMPreferencesPath()
        {
            return Interface.IStore.FolderPath + Resources.XCOMPreferences + XML_EXT;
        }

        /*
        public void RejectPreferencesChanges()
        {
            Interface.IPreferences.CurrentPref.RejectChanges();
          // Interface.IPreferences.CurrentSSFPref.RejectChanges();
        }
        public void RejectXCOMChanges()
        {
            Interface.IPreferences.CurrentXCOMPref.RejectChanges();
            // Interface.IPreferences.CurrentSSFPref.RejectChanges();
        }
        public void RejectSSFChanges()
        {
           // Interface.IPreferences.CurrentPref.RejectChanges();
            Interface.IPreferences.CurrentSSFPref.RejectChanges();
        }

        */

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
            try
            {
                bool ok = populatePreferences<XCOMPrefDataTable>();
                if (ok)
                {
                    //cleaning
                    cleanNullPreferences<XCOMPrefDataTable>();    //important
                }
                populateCurrentPreferences<XCOMPrefDataTable>();
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
                savePreferences<XCOMPrefDataTable>();
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
            Interface.IDB.XCOMPref.AcceptChanges();
        }

        public void ReportChanges()
        {
            bool reportChnages = CurrentPref.HasVersion(DataRowVersion.Current);
            reportChnages = reportChnages || CurrentSSFPref.HasVersion(DataRowVersion.Current);
            reportChnages = reportChnages || CurrentXCOMPref.HasVersion(DataRowVersion.Current);
            if (reportChnages)
            {
                Interface.IReport.Msg("A preference/setting was updated", "Preferences updated", true);
            }
            else Interface.IReport.Msg("No changes to the preferences/settings", "No changes", true);
        }

        public void Clear()
        {
            Interface.IPreferences.SavePreferences();


            Interface.IStore.CleanPreferences();
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