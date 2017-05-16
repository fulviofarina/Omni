using System;
using Msn;

namespace DB
{
    public interface IReport
    {
        Pop Msn { get; set; }
        void ReportToolTip(object sender, EventArgs ev);
        string RestartFile { get; }

        void AskToRestart();

        bool CheckMSMQ();

        bool CheckRestartFile();

        void GenerateBugReport();

        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void GenerateUserInfoReport();

        void Msg(string msg, string title, bool ok=true);

        void ReportProgress(int percentage);

        void SendToRestartRoutine(string texto);

        void Speak(string text);

        void SpeakLoadingFinished();
    }
}