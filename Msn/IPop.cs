using System.Windows.Forms;

namespace Msn
{
    public interface IPop
    {
        string BufferMessage { get; set; }
        int DisplayInterval { get; set; }

        void MakeForm();
        void Msg(string msg, string title, ToolTipIcon icon);
        void Msg(string msg, string title, bool ok);
        void ReportProgress(int percentage);
        void Speak(string text);
    }
}