using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IReport
    {
        NotifyIcon Notify { set; get; }

        void GenerateBugReport();

        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void Msg(string msg, string title, bool ok);

        void Msg(string msg, string title);

        void ReportFinished();

        void ReportProgress(int percentage);

        void Speak(string text);
    }
}