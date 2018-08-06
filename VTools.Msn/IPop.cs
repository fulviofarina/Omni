using System.Windows.Forms;

namespace VTools
{
    public interface IPop
    {
        string LastMessage { get; }
        string CurrentMessage { get; }
        string LastSpokenMessage { get; }
        int DisplayInterval { get; set; }
        Form ParentForm { get; }

        void FillLog(string msg);

        void MakeForm();

        // void Msg(string msg, string title, ToolTipIcon icon, bool accumulate = false);
        void Msg(string msg, string title, object ok = null, bool accumulate = false);

        void ReportProgress(int percentage);

        void Speak(string text);
    }
}