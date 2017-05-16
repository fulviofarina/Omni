﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;
using Rsx.Dumb; using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    /// <summary>
    /// PRIVATE FUNCTIONS
    /// </summary>
    public partial class Current
    {
        // private PreferencesRow currentPref;

        // private SSFPrefRow currentSSFPref;

        /// <summary>
        /// Reads the preferences files
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private bool populatePreferences<T>()
        {
            // string prefFolder= Resources.Preferences;
            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);
            //keep this this way, works fine

            //load into table
            bool ok = Tables.ReadTable(path, ref dt);

            return ok;
        }

        protected Func<DataRow, bool> selector
        {
            get
            {
                string WINDOWS_USER = "WindowsUser";
                string label = WINDOWS_USER;
                return p => p.Field<string>(label).CompareTo(WindowsUser) == 0;
            }
        }

     //   protected static string WINDOWS_USER = "WindowsUser";

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
            Interface.IStore.Delete(ref prefes);

            dt.EndLoadData();
            dt.AcceptChanges();
        }

        /// <summary>
        /// finds wether it should use the preferences or the SSf path and table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">  </param>
        /// <param name="path"></param>
        private void findTableAndPath<T>(out DataTable dt, out string path)
        {
            if (typeof(T).Equals(typeof(PreferencesDataTable)))
            {
                path = Interface.IStore.FolderPath + Resources.Preferences + ".xml";
                dt = Interface.IDB.Preferences;
            }
            else
            {
                path = Interface.IStore.FolderPath + Resources.SSFPreferences + ".xml";
                dt = Interface.IDB.SSFPref;
            }
        }

        /// <summary>
        /// Loads the Current Rows with data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void populateCurrentPreferences<T>()
        {
            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);

            DataRow row = dt.AsEnumerable().FirstOrDefault(selector);
            bool add = false;
            if (row == null)
            {
                row = dt.NewRow();
                add = true;
            }
            Type tipo = typeof(T);
            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                // if (this.currentSSFPref == null) {
                PreferencesRow p = row as PreferencesRow;

                //  PreferencesRow p = dt.LoadDataRow(original.ItemArray,false) as PreferencesRow;
                p.WindowsUser = WindowsUser;
                p.Check();

                // } p.Check();
            }
            else
            {
                //                     SSFPrefRow p = dt.LoadDataRow(row.ItemArray, false) as SSFPrefRow;
                SSFPrefRow p = row as SSFPrefRow;

                p.WindowsUser = WindowsUser;
                p.Check();

                //    dt.ImportRow(row);
                // } p.Check();
            }

            if (add)
            {
                dt.LoadDataRow(row.ItemArray,true);
            }
        }

        /// <summary>
        /// I dont know what this does
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefe"></param>
        /*
        private void mergePreferences<T>(ref T prefe)
        {
            Type tipo = typeof(T);

            Interface.IDB.Merge(this, false, MissingSchemaAction.AddWithKey);

            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);

            DataTable destiny = null;

            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                destiny = Interface.IDB.Preferences;
            }
            else
            {
                destiny = Interface.IDB.SSFPref;
            }

            destiny.Clear();
            destiny.Merge(dt, false, MissingSchemaAction.AddWithKey);
            // this.AcceptChanges();
        }
        */

        /// <summary>
        /// Saves preferences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void savePreferences<T>()
        {
            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);
            // if (this.Preferences.Columns.Contains(this.Preferences.DoSolangColumn.ColumnName))
            // this.Preferences.Columns.Remove(this.Preferences.DoSolangColumn); if
            // (this.Preferences.Columns.Contains(this.Preferences.DoMatSSFColumn.ColumnName)) this.Preferences.Columns.Remove(this.Preferences.DoMatSSFColumn);
            dt.EndLoadData();
            dt.AcceptChanges();

            System.IO.File.Delete(path);
            dt.WriteXml(path, System.Data.XmlWriteMode.WriteSchema, true);
        }
    }
}