using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class Preference
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
        private void getSQLPreferenceEvent(object sender, EventData e)
        {
            PreferencesRow pref = CurrentPref;
            object[] args = new object[]
            {
             !pref.Offline
        };

            e.Args = args;
        }
        private void getPreferencesEvent(object sender, EventData e)
        {
            SSFPrefRow pref = CurrentSSFPref;
            object[] args = new object[]
            {
                pref.CalcMass,
            pref.AARadius,
             pref.CalcDensity,
            pref.AAFillHeight
        };

            e.Args = args;
        }

        private void getPreferencesSpectrumEvent(object sender, EventData e)
        {
            PreferencesRow pref = CurrentPref;
            object[] args = new object[]
            {
                pref.minArea,
            pref.maxUnc,
             pref.windowA,
            pref.windowB,
            pref.DetectorIdx,
            pref.PositionIdx,
            pref.MeasIdx
        };

            e.Args = args;
        }

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
            prefes = dt.AsEnumerable()
                .Where(o => string.IsNullOrEmpty(o.Field<string>(WINDOWS_USER)));
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
            Type t = typeof(T);
            if (t.Equals(typeof(PreferencesDataTable)))
            {
                path = GetPreferencesPath();
                dt = Interface.IDB.Preferences;
            }
            else if (t.Equals(typeof(SSFPrefDataTable)))
            {
                path = GetSSFPreferencesPath();
                dt = Interface.IDB.SSFPref;
            }
            else// if (t.Equals(typeof(XCOMPrefDataTable)))
            {
                path = GetXCOMPreferencesPath();
                dt = Interface.IDB.XCOMPref;
            }
         //   else
          //  {

         //   }
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

            DataRow row = dt.AsEnumerable().FirstOrDefault(selector) ;
             bool add = false;

            if (row == null)
            {
                Interface.IReport.GenerateUserInfoReport();

                //row = dt.NewRow();
               
              //  row.SetField<string>("WindowsUser", WindowsUser);
              //  add = true;
            }



          

            Type tipo = typeof(T);




            if (tipo.Equals(typeof(PreferencesDataTable)))
            {
                PreferencesDataTable data = dt as PreferencesDataTable;

                PreferencesRow p = null;

                if (row == null)
                {
                    p = data.NewPreferencesRow();
                    data.AddPreferencesRow(p);
                    p.WindowsUser = WindowsUser;
                 
                    row = p as DataRow;
                }
             //   else p = row as PreferencesRow;
                
            }
            else if (tipo.Equals(typeof(SSFPrefDataTable)))
            {
                SSFPrefDataTable data = dt as SSFPrefDataTable;
                SSFPrefRow p = null;
                if (row == null)
                {
                    p = data.NewSSFPrefRow();
                    p.WindowsUser = WindowsUser;
                    data.AddSSFPrefRow(p);
                    row = p as DataRow;
                }
             //   else p = row as SSFPrefRow;
              
            }
            else if (tipo.Equals(typeof(XCOMPrefDataTable)))
            {
                XCOMPrefDataTable data = dt as XCOMPrefDataTable;
                XCOMPrefRow p = null;
                if (row == null)
                {
                    p = data.NewXCOMPrefRow();
                    p.WindowsUser = WindowsUser;
                    data.AddXCOMPrefRow(p);
          
                    row = p as DataRow;
                }
            //    else p = row as XCOMPrefRow;

               
            }
           

                 (row as IRow).Check();



              if (add)
              {
           //      dt.LoadDataRow(row.ItemArray, true);
              }
            //   IRow r = row as IRow;
            //check
            //     r.Check();
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