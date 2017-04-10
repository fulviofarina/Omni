namespace DB
{
    public interface IPreferences
    {
        LINAA.PreferencesRow CurrentPref { get; }

        LINAA.SSFPrefRow CurrentSSFPref { get; }
    }
}