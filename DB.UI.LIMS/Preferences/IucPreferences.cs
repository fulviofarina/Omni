using System;
using System.Collections;
using DB.Tools;

namespace DB.UI
{
    public interface IucPreferences
    {
        EventHandler CheckChanged { set; get; }
        EventHandler CheckChanged2 { set; get; }
        EventHandler CheckChanged3 { set; get; }

        void Set(ref Interface inter);

        void SetRoundingBinding(ref Hashtable unitsTable);
    }
}