using VTools;

namespace DB.Tools
{
    public class DBOptions : ucOptions, IOptions
    {
        public DBOptions(int type, bool advanced, bool save, bool restore, bool connections) : base(type)
        {
            Set();
           
            if (save)
            {
                SaveClick += delegate
                {
                    Creator.SaveInFull(true);
                };
            }

            DropDownClicked += delegate
            {
                DisableImportant = advanced;
                bool ssf = type == 0;
                DisableBasic = ssf;
            };
            if (restore)
            {
                RestoreFoldersClick += delegate
                {
                    Creator.CheckDirectories(true);
                };
            }
            if (connections)
            {
                ConnectionBox += delegate
                {
                    Creator.ConnectionsUI();
                };
            }
         
          
        }
    }
}