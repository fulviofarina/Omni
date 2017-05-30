using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class Current
    {
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

        /// <summary>
        /// remove shitty preferences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void cleanNullPreferences<T>()
        {
            Type tipo = typeof(T);

            DataTable dt = null;
            string path = string.Empty;

            findTableAndPath<T>(out dt, out path);

            IEnumerable<DataRow> prefes = null;
            prefes = dt.AsEnumerable().Where(o => string.IsNullOrEmpty(o.Field<string>(WINDOWS_USER)));
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
                path = GetPreferencesPath();
                dt = Interface.IDB.Preferences;
            }
            else
            {
                path = GetSSFPreferencesPath();
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
                if (row.GetType().Equals(typeof(PreferencesRow)))
                {
                    Interface.IReport.GenerateUserInfoReport();
                }
                row = dt.NewRow();

                add = true;
            }
            Type tipo = typeof(T);
            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                PreferencesRow p = row as PreferencesRow;

                p.WindowsUser = WindowsUser;
                p.Check();
            }
            else
            {
                SSFPrefRow p = row as SSFPrefRow;

                p.WindowsUser = WindowsUser;
                p.Check();
            }

            if (add)
            {
                dt.LoadDataRow(row.ItemArray, true);
            }
        }

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
            dt.EndLoadData();
            dt.AcceptChanges();

            System.IO.File.Delete(path);
            dt.WriteXml(path, System.Data.XmlWriteMode.WriteSchema, true);
        }
    }
}