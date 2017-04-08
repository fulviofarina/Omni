//using DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using DB.Properties;
using Rsx;

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

        private void savePreferences<T>()
        {
            try
            {
                DataTable dt = null;

                string path = string.Empty;

                if (typeof(T).Equals(typeof(PreferencesDataTable)))
                {
                    path = this.folderPath + Resources.Preferences;
                    dt = this.Preferences;
                }
                else
                {
                    path = this.folderPath + Resources.SSFPreferences;
                    dt = this.SSFPref;
                }
                // if (this.Preferences.Columns.Contains(this.Preferences.DoSolangColumn.ColumnName)) this.Preferences.Columns.Remove(this.Preferences.DoSolangColumn);
                //  if (this.Preferences.Columns.Contains(this.Preferences.DoMatSSFColumn.ColumnName)) this.Preferences.Columns.Remove(this.Preferences.DoMatSSFColumn);
                dt.EndLoadData();
                dt.AcceptChanges();

                System.IO.File.Delete(path);
                dt.WriteXml(path, System.Data.XmlWriteMode.WriteSchema, true);
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        /// <summary>
        /// I dont know what this does
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefe"></param>
        private void mergePreferences<T>(ref T prefe)
        {
            Type tipo = typeof(T);

            this.Merge(this, false, MissingSchemaAction.AddWithKey);
            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                PreferencesDataTable table = prefe as PreferencesDataTable;
                this.Preferences.Clear();
                this.Preferences.Merge(table, false, MissingSchemaAction.AddWithKey);
            }
            else
            {
                SSFPrefDataTable table = prefe as SSFPrefDataTable;
                this.SSFPref.Clear();
                this.SSFPref.Merge(table, false, MissingSchemaAction.AddWithKey);
            }

            // this.AcceptChanges();
        }

        /// <summary>
        /// Loads the CurrentSSFPrefRow
        /// </summary>

        private void loadCurrentPreferences<T>()
        {
            string WinUser = WindowsIdentity.GetCurrent().Name.ToUpper();

            Type tipo = typeof(T);
            if (tipo.Equals(typeof(PreferencesRow)))
            {
                this.currentPref = tablePreferences.FirstOrDefault(p => p.WindowsUser.CompareTo(WinUser) == 0);
                if (this.currentPref == null)
                {
                    this.currentPref = this.Preferences.NewPreferencesRow();
                    this.Preferences.AddPreferencesRow(this.currentPref);
                    this.currentPref.WindowsUser = WinUser;
                }
                this.currentPref.Check();
            }
            else
            {
                this.currentSSFPref = tableSSFPref.FirstOrDefault(p => p.WindowsUser.CompareTo(WinUser) == 0);
                if (this.currentSSFPref == null)
                {
                    this.currentSSFPref = this.SSFPref.NewSSFPrefRow();
                    this.SSFPref.AddSSFPrefRow(this.currentSSFPref);
                    this.currentSSFPref.WindowsUser = WinUser;
                }
                if (currentSSFPref.IsFolderNull()) currentSSFPref.Folder = Resources.SSFFolder;
            }
        }

        private void cleanPreferences<T>()
        {
            Type tipo = typeof(T);
            DataTable table = null;
            IEnumerable<DataRow> prefes = null;

            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                LINAA.PreferencesDataTable dt = this.tablePreferences;
                prefes = dt.Where(o => string.IsNullOrEmpty(o.WindowsUser));

                table = dt;
            }
            else
            {
                LINAA.SSFPrefDataTable dt = this.tableSSFPref;
                prefes = dt.AsEnumerable().Where(o => string.IsNullOrEmpty(o.WindowsUser));

                table = dt;
            }

            this.Delete(ref prefes);
            table.EndLoadData();
            table.AcceptChanges();
        }

        public void PopulatePreferences()
        {
            PopulatePreferences<LINAA.PreferencesDataTable>();

            PopulatePreferences<LINAA.SSFPrefDataTable>();
        }

        public void PopulatePreferences<T>()
        {
            //  string prefFolder=   Resources.Preferences;
            string path = string.Empty;

            DataTable dt = null;
            if (typeof(T).Equals(typeof(LINAA.PreferencesDataTable)))
            {
                dt = this.Preferences;
                path = folderPath + Resources.Preferences;
            }
            else
            {
                dt = this.SSFPref;
                path = folderPath + Resources.SSFPreferences;
            }
            //keep this this way, works fine

            //load
            bool ok = Dumb.ReadTable(path, ref dt);

            if (ok)
            {
                //cleaning
                cleanPreferences<T>();    //important
            }

            loadCurrentPreferences<T>();
        }
    }
}