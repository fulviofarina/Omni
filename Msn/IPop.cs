using System.Windows.Forms;

namespace Msn
{
    public interface IPop
    {

        string LastMessage { get;  }
        string CurrentMessage { get;  }
        string LastSpokenMessage { get;  }
        int DisplayInterval { get; set; }

        void MakeForm();
        void Msg(string msg, string title, ToolTipIcon icon, bool accumulate = false);
        void Msg(string msg, string title, bool ok , bool accumulate);
        void ReportProgress(int percentage);
        void Speak(string text);
    }
}