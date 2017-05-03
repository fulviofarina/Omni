using System;
using System.Collections;
using DB.Tools;

namespace DB.UI
{
    public interface IucPreferences
    {
        EventHandler CheckChanged {  set; }

        void Set(ref Interface inter);
        void SetRoundingBinding(ref Hashtable unitsTable, ref Hashtable sampleTable);
    }
}