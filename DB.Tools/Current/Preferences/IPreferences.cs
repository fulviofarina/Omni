namespace DB.Tools
{
    public interface IPreferences
    {
        LINAA.PreferencesRow CurrentPref { get; }

        void SavePreferences();
       string  GetPreferencesPath();
        bool IsSpectraPathOk { get; }
        string GetSSFPreferencesPath();
        string WindowsUser { get; }
    //    bool SetConnections(string HLString, string LIMSString, string spectraSrv, string spectraPath);
        void PopulatePreferences();

        LINAA.SSFPrefRow CurrentSSFPref { get; }

        void RejectPreferencesChanges();
        void AcceptChanges();
    }
}