using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTools;
using System.Windows.Forms;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

using System.Data;

using System.IO;

using DB.Properties;
using DB.UI;
using Rsx.Dumb;


using VTools;
namespace DB.Tools
{

   public  class ucDBOptions : ucOptions, IOptions
    {
        public ucDBOptions(int type, bool advanced):base(type)
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
                //   Creator.SaveInFull(true);
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
