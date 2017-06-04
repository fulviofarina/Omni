using System;
using System.Collections;
using DB.Tools;

namespace DB.UI
{
    public interface IucPreferences
    {
      //  event EventHandler SampleChanged;
        event EventHandler DoChilianChanged;
        event EventHandler DoMatSSFChanged;
        event EventHandler OverriderChanged;
        event EventHandler RunInBackground;
        event EventHandler CalcRadiusChanged;
       event EventHandler CalcLengthChanged;
        event EventHandler CalcDensityChanged;
        event EventHandler CalcMassChanged;

        void Set(ref Interface inter);

        void SetRoundingBinding(ref Hashtable unitsTable);
    }
}