﻿using System;
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

  

    /// <summary>
    /// PRIVATE FUNCTIONS
    /// </summary>
    public partial class Current
    {
        private PreferencesRow currentPref;

        private SSFPrefRow currentSSFPref;

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
            bool ok = Dumb.ReadTable(path, ref dt);

            return ok;
        }

        private Func<DataRow, bool> selector
        {
            get
            {
                string label = "WindowsUser";
                return p => p.Field<string>(label).CompareTo(WindowsUser) == 0;
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
                path = Interface.IMain.FolderPath + Resources.Preferences + ".xml";
                dt = Interface.IDB.Preferences;
            }
            else
            {
                path = Interface.IMain.FolderPath + Resources.SSFPreferences + ".xml";
                dt = Interface.IDB.SSFPref;
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

            DataRow row = dt.AsEnumerable().FirstOrDefault(selector);
            if (row == null) row = dt.NewRow();

            Type tipo = typeof(T);
            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                // if (this.currentSSFPref == null) {
                PreferencesRow original = row as PreferencesRow;
            
                PreferencesRow p = dt.LoadDataRow(original.ItemArray, true) as PreferencesRow;
                p.WindowsUser = WindowsUser;
                p.Check();
                // }
                //    p.Check();
            }
            else
            {
                SSFPrefRow p = dt.LoadDataRow(row.ItemArray, true) as SSFPrefRow;
                SSFPrefRow original = row as SSFPrefRow;
               
                p.WindowsUser = WindowsUser;
                p.Check();

                // }
                // p.Check();
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