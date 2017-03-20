using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using DB.Properties;

namespace DB
{
    public partial class LINAA
    {
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

        partial class PreferencesRow
        {
            private bool usrAnal = false;

            public bool UsrAnal
            {
                get { return usrAnal; }
                set { usrAnal = value; }
            }

            public void Check()
            {
                // if (IswindowANull()) windowA = 1.5;
                // if (IswindowBNull()) windowB = 0.001;
                // if (IsminAreaNull()) minArea = 500;
                // if (IsmaxUncNull()) maxUnc = 50;
                //    if (IsAutoLoadNull()) AutoLoad = true;
                //    if (IsShowSolangNull()) ShowSolang = false;
                // if (IsShowMatSSFNull()) ShowMatSSF = false;
                //if (IsAAFillHeightNull()) AAFillHeight = false;
                //if (IsAARadiusNull()) AARadius = true;
                //   if (IsShowSampleDescriptionNull()) ShowSampleDescription = true;
                this.LastAccessDate = DateTime.Now;
                if (IsLastToDoNull()) LastToDo = string.Empty;
                if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                //  if (IsFillByHLNull()) FillByHL = true;
                // if (IsFillBySpectraNull()) FillBySpectra = true;
                if (IsHLNull()) HL = Settings.Default.HLSNMNAAConnectionString;
                else Settings.Default["HLSNMNAAConnectionString"] = HL;
                if (IsLIMSNull()) LIMS = Settings.Default.NAAConnectionString;
                else Settings.Default["NAAConnectionString"] = LIMS;
                if (IsSpectraNull()) Spectra = Settings.Default.SpectraFolder;
                else Settings.Default["SpectraFolder"] = Spectra;
                if (IsSpectraSvrNull()) SpectraSvr = Settings.Default.SpectraServer;
                else Settings.Default["SpectraServer"] = SpectraSvr;

                if (IsSolCoiFolderNull()) SolCoiFolder = Resources.SolCoiFolder;
                //   else Settings.Default["SOLCOIFolder"] = SolCoiFolder;

                Settings.Default.Save();
            }
        }

        public partial class SSFPrefRow
        {
            public void Check()
            {
                //  DoSolang = false;
                //  DoMatSSF = false;//fix

                //  if (IsAutoLoadNull()) AutoLoad = true;
                //  if (IsShowSolangNull()) ShowSolang = false;
                //  if (IsShowMatSSFNull()) ShowMatSSF = false;
                // if (IsAAFillHeightNull()) AAFillHeight = false;
                //if (IsAARadiusNull()) AARadius = true;
                if (IsFolderNull()) Folder = Resources.SSFFolder;
                //    else Settings.Default["SSFFolder"] = Folder;

                // ShowSampleDescription = true;
                //   if (IsLastIrradiationProjectNull()) LastIrradiationProject = string.Empty;
                //   if (IsFillByHLNull()) FillByHL = true;
                //  if (IsFillBySpectraNull()) FillBySpectra = true;
                //// if (IsHLNull()) HL = DB.Properties.Settings.Default.HLSNMNAAConnectionString;
                // else DB.Properties.Settings.Default["HLSNMNAAConnectionString"] = HL;

                // if (IsLIMSNull()) LIMS = DB.Properties.Settings.Default.NAAConnectionString;
                //  else DB.Properties.Settings.Default["NAAConnectionString"] = LIMS;
                //  if (IsSpectraNull()) Spectra = DB.Properties.Settings.Default.SpectraFolder;
                //  else DB.Properties.Settings.Default["SpectraFolder"] = Spectra;
                //  if (IsSpectraSvrNull()) SpectraSvr = DB.Properties.Settings.Default.SpectraServer;
                //  else DB.Properties.Settings.Default["SpectraServer"] = SpectraSvr;

                Settings.Default.Save();
            }
        }

        partial class ExceptionsDataTable
        {
            public void RemoveDuplicates()
            {
                HashSet<string> hs = new HashSet<string>();
                IEnumerable<LINAA.ExceptionsRow> ordered = this.OrderByDescending(o => o.Date);
                ordered = ordered.TakeWhile(o => !hs.Add(o.StackTrace));
                for (int i = ordered.Count() - 1; i >= 0; i--)
                {
                    LINAA.ExceptionsRow e = ordered.ElementAt(i);
                    e.Delete();
                }

                hs.Clear();
                hs = null;
                this.AcceptChanges();
            }

            public void AddExceptionsRow(Exception ex)
            {
                string target = string.Empty;
                string stack = string.Empty;
                string source = string.Empty;

                if (ex.Source != null) source = ex.Source;
                if (ex.TargetSite != null) target = ex.TargetSite.Name;
                if (ex.StackTrace != null) stack = ex.StackTrace;

                AddExceptionsRow(target, ex.Message, stack, source, DateTime.Now);
            }
        }
    }
}