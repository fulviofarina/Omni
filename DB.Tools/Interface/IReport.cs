using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DB.LINAATableAdapters;
using Msn;

namespace DB
{
    public interface IReport
    {
        NotifyIcon Notify { set; get; }
        Pop Msn { get; set; }

        void GenerateBugReport();
        void SendToRestartRoutine(string texto);
        bool RestartingRoutine();

      

        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void Msg(string msg, string title, bool ok);

        void Msg(string msg, string title);

        void ReportFinished();

        void ReportProgress(int percentage);

        void Speak(string text);
    }
}