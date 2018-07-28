using System;
using VTools;

namespace DB.Tools
{
    public interface IReport
    {
        IPop Msn { get; }

        void GreetUser();

        void ReportToolTip(object sender, EventArgs ev);

        string RestartFile { get; }

        bool CheckMSMQ();

        bool CheckRestartFile();

        void GenerateBugReport();

        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void GenerateUserInfoReport();

        void Msg(string msg, string title, object ok = null, bool accumulate = false);

        void ReportProgress(int percentage);

        void SendToRestartRoutine(string texto);

        void Speak(string text);

        void SpeakLoadingFinished();

        void InstallMSMQ();
    }
}