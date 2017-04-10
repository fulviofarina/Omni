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
    public partial class LINAA // : IPreferences
    {
        /*
        private PreferencesRow currentPref;

        private SSFPrefRow currentSSFPref;

        public PreferencesRow CurrentPref
        {
            get
            {
                return currentPref;
            }
            set { currentPref = value; }
        }

        public SSFPrefRow CurrentSSFPref
        {
            get
            {
                return currentSSFPref;
            }
            set { currentSSFPref = value; }
        }
        */

        /// <summary>
        /// Reads the preferences files
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool populatePreferences<T>()
        {
            //  string prefFolder=   Resources.Preferences;
            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);
            //keep this this way, works fine

            //load into table
            bool ok = Dumb.ReadTable(path, ref dt);

            return ok;
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
                this.AddException(ex);
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
                this.AddException(ex);
            }
        }

        /// <summary>
        /// remove shitty preferences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void cleanPreferences<T>()
        {
            Type tipo = typeof(T);

            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);

            IEnumerable<DataRow> prefes = null;
            prefes = dt.AsEnumerable().Where(o => string.IsNullOrEmpty(o.Field<string>("WindowsUser")));
            this.Delete(ref prefes);

            dt.EndLoadData();
            dt.AcceptChanges();
        }

        /// <summary>
        /// finds wether it should use the preferences or the SSf path and table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        private void findTableAndPath<T>(out DataTable dt, out string path)
        {
            if (typeof(T).Equals(typeof(PreferencesDataTable)))
            {
                path = this.folderPath + Resources.Preferences + ".xml";
                dt = this.Preferences;
            }
            else
            {
                path = this.folderPath + Resources.SSFPreferences + ".xml";
                dt = this.SSFPref;
            }
        }

        /// <summary>
        /// Loads the Current Rows with data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void loadCurrentPreferences<T>()
        {
            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);
            string windowsUser = WindowsIdentity.GetCurrent().Name.ToUpper();
            Func<DataRow, bool> selector = p =>
            {
                return p.Field<string>("WindowsUser").CompareTo(windowsUser) == 0;
            };

            DataRow row = dt.AsEnumerable().FirstOrDefault(selector);
            if (row == null) row = dt.NewRow();

            Type tipo = typeof(T);
            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                // if (this.currentSSFPref == null)
                //  {
                PreferencesRow p = dt.LoadDataRow(row.ItemArray, true) as PreferencesRow;
                p.WindowsUser = windowsUser;
                //  }
                p.Check();
            }
            else
            {
                SSFPrefRow p = dt.LoadDataRow(row.ItemArray, true) as SSFPrefRow;
                p.WindowsUser = windowsUser;

                // }
                p.Check();
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

            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);

            DataTable destiny = null;

            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                destiny = this.Preferences;
            }
            else
            {
                destiny = this.SSFPref;
            }

            destiny.Clear();
            destiny.Merge(dt, false, MissingSchemaAction.AddWithKey);
            // this.AcceptChanges();
        }

        /// <summary>
        /// Saves preferences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void savePreferences<T>()
        {
            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);
            // if (this.Preferences.Columns.Contains(this.Preferences.DoSolangColumn.ColumnName)) this.Preferences.Columns.Remove(this.Preferences.DoSolangColumn);
            //  if (this.Preferences.Columns.Contains(this.Preferences.DoMatSSFColumn.ColumnName)) this.Preferences.Columns.Remove(this.Preferences.DoMatSSFColumn);
            dt.EndLoadData();
            dt.AcceptChanges();

            System.IO.File.Delete(path);
            dt.WriteXml(path, System.Data.XmlWriteMode.WriteSchema, true);
        }
    }
}