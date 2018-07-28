using VTools;

namespace DB.Tools
{
    public class DBOptions : ucOptions, IOptions
    {
        public static IOptions GetOptions(int type, bool advanced)
        {
            IOptions options = new DBOptions(type, advanced);

            /*
            options.Set();

            options.SaveClick += delegate
            {
                Creator.SaveInFull(true);
            };

            options.DropDownClicked += delegate
            {
                bool ok = Interface.IPreferences
                .CurrentPref.AdvancedEditor;
                options.DisableImportant = ok;
                bool ssf = type == 0;
                options.DisableBasic = ssf;
            };
            options.RestoreFoldersClick += delegate
            {
                Creator.CheckDirectories(true);
             // Creator.SaveInFull(true);
            };

            options.ConnectionBox += delegate
            {
                Creator.ConnectionsUI();
            };
            //NOW ADD IT
            Creator.UserControls.Add(options);
            */
            return options;
        }

        public DBOptions(int type, bool advanced) : base(type)
        {
            Set();

            SaveClick += delegate
            {
                Creator.SaveInFull(true);
            };

            DropDownClicked += delegate
            {
                DisableImportant = advanced;
                bool ssf = type == 0;
                DisableBasic = ssf;
            };
            RestoreFoldersClick += delegate
            {
                Creator.CheckDirectories(true);
                // Creator.SaveInFull(true);
            };

            ConnectionBox += delegate
            {
                Creator.ConnectionsUI();
            };
            //NOW ADD IT
            Creator.UserControls.Add(this);
        }
    }
}