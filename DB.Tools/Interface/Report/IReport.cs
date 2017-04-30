using System.Messaging;
using Msn;

namespace DB
{
    public interface IReport
    {
        // NotifyIcon Notify { set; get; }
        Pop Msn { get; set; }
        string RestartFile { get; }
        bool CheckRestartFile();
        void AskToRestart();
        void SendToRestartRoutine(string texto);
        void GenerateBugReport();

        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void Msg(string msg, string title, bool ok = true);

        void SpeakLoadingFinished();
        bool CheckMSMQ();
     //   MessageQueue getMessageQueue(string QUEUE_PATH);
        // void Msg(string msg, string title);
        void ReportProgress(int percentage);

        void Speak(string text);

       

        void GenerateUserInfoReport();
    }
}