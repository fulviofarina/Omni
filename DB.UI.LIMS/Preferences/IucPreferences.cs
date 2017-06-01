using System;
using System.Collections;
using DB.Tools;

namespace DB.UI
{
    public interface IucPreferences
    {
     //   EventHandler CheckChanged { set;  }
      //  EventHandler DoChilianChanged { set; }
      //  EventHandler DoMatSSFChanged { set;  }
      //  EventHandler OverriderChanged { set; }
     //   EventHandler RunInBackground { set; }
        void Set(ref Interface inter);

        void SetRoundingBinding(ref Hashtable unitsTable);
    }
}