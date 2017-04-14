using System.Collections;
using DB.Tools;

namespace DB.UI
{
    public interface IucPreferences
    {
        void Set(ref Interface inter);
        void SetRoundingBinding(ref Hashtable unitsTable, ref Hashtable sampleTable);
    }
}