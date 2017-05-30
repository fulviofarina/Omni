﻿using System;


namespace VTools
{
    public interface IucOptions
    {
        //  Action AboutBoxAction { set; }
        Action DatabaseClick { set; }
        Action HelpClick { set; }

   //     void SetDeveloperMode(bool devMode);
        Action ExplorerClick { set; }
        Action PreferencesClick { set; }
        Action AboutBoxAction { set; }
        Action ConnectionBox { set; }
        Action SaveClick { set; }
        void ResetProgress(int max);
        void Set();
        EventHandler ShowProgress { get; }
    }
}