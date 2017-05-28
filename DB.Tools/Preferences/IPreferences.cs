namespace DB.Tools
{
    public interface IPreferences
    {
        LINAA.PreferencesRow CurrentPref { get; }

        void SavePreferences();

        bool IsSpectraPathOk { get; }

        string WindowsUser { get; }

        void PopulatePreferences();

        LINAA.SSFPrefRow CurrentSSFPref { get; }

        void RejectChanges();
    }
}