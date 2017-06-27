namespace DB.Tools
{
    public interface IPreferences
    {
        LINAA.PreferencesRow CurrentPref { get; }

        LINAA.SSFPrefRow CurrentSSFPref { get; }

        LINAA.XCOMPrefRow CurrentXCOMPref { get; }

        bool IsSpectraPathOk { get; }

        string WindowsUser { get; }

        void AcceptChanges();

        string GetPreferencesPath();

        string GetSSFPreferencesPath();

        string GetXCOMPreferencesPath();

        void PopulatePreferences();

        void SavePreferences();

        void ReportChanges();
        void Clear();
    }
}