using System;


namespace VTools
{
    public interface IucOptions
    {
        //  Action AboutBoxAction { set; }
        event EventHandler DatabaseClick;
        event EventHandler HelpClick;

        //     void SetDeveloperMode(bool devMode);
        event EventHandler ExplorerClick;
        event EventHandler PreferencesClick;
        event EventHandler AboutBoxClick;
        event EventHandler ConnectionBox;
        event EventHandler SaveClick;
        void ResetProgress(int max);
        void Set();
        EventHandler ShowProgress { get; }
        bool DisableImportant {  set; }

        event EventHandler DropDownClicked;// { get; set; }
    }
}