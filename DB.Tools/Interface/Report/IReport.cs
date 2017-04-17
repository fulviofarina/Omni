using Msn;

namespace DB
{
    public interface IReport
    {
        // NotifyIcon Notify { set; get; }
        Pop Msn { get; set; }

        void AskToRestart();

        void GenerateBugReport();

        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void Msg(string msg, string title, bool ok = true);

        void ReportFinished();

        // void Msg(string msg, string title);
        void ReportProgress(int percentage);

        void Speak(string text);

        void Test();

        void UserInfo();
    }
}