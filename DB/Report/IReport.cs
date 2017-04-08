using System;
using System.Collections.Generic;
using System.Data;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IReport
    {
        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void GenerateBugReport();

        void Speak(string text);

        void Msg(string msg, string title, bool ok);

        void Msg(string msg, string title);

        void AddException(Exception ex);

        LINAA.ExceptionsDataTable Exceptions { get; }

        void ReportProgress(int percentage);
    }
}