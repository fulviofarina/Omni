using System.Collections.Generic;
using System.Linq;
//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IPreferences
    {
        protected internal LINAA.PreferencesRow currentPref;

        public LINAA.PreferencesRow CurrentPref
        {
            get
            {
                return currentPref;
            }
            set { currentPref = value; }
        }

        public void PopulatePreferences()
        {
            string path = folderPath + Properties.Resources.Preferences;

            //keep this this way, works fine
            if (System.IO.File.Exists(path)) //user preferences found...
            {
                this.Preferences.BeginLoadData();
                System.IO.FileInfo info = new System.IO.FileInfo(path);
                if (info.Length < 204800)
                {
                    this.Preferences.ReadXml(folderPath + Properties.Resources.Preferences);
                }
                else System.IO.File.Delete(path);
                IEnumerable<PreferencesRow> prefes = this.tablePreferences.Where(o => string.IsNullOrEmpty(o.WindowsUser));
                this.Delete(ref prefes);
                this.Preferences.EndLoadData();
                this.Preferences.AcceptChanges();       //important
            }

            string windowsUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper();
            this.currentPref = tablePreferences.FirstOrDefault(p => p.WindowsUser.CompareTo(windowsUser) == 0);
            if (this.currentPref == null)
            {
                this.currentPref = this.Preferences.NewPreferencesRow();
                this.Preferences.AddPreferencesRow(this.currentPref);
                this.currentPref.WindowsUser = windowsUser;
            }
            this.currentPref.Check();
        }
    }
}