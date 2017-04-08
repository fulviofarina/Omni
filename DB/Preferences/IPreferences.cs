using System;
using System.Collections.Generic;
using System.Data;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IPreferences
    {
        void PopulatePreferences();

        //     void PopulateSSFPreferences();

        LINAA.PreferencesRow CurrentPref { get; set; }

        LINAA.SSFPrefRow CurrentSSFPref { get; set; }

        //   void PopulateSSFPreferences();
    }
}