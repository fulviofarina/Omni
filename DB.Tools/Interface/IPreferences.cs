namespace DB
{
    public interface IPreferences
    {
        LINAA.PreferencesRow CurrentPref { get; }

        void SavePreferences();

        bool IsSpectraPathOk { get; }

        void PopulatePreferences();

        LINAA.SSFPrefRow CurrentSSFPref { get; }
    }
}