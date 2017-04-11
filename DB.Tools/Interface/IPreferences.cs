namespace DB
{
    public interface IPreferences
    {
        LINAA.PreferencesRow CurrentPref { get; }

        void SavePreferences();

        void PopulatePreferences();

        LINAA.SSFPrefRow CurrentSSFPref { get; }
    }
}