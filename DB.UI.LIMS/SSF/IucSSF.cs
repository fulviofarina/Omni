using DB.Tools;

namespace DB.UI
{
    public interface IucSSF
    {
        void AttachCtrl<T>(ref T pro);
        void Calculate(bool? Bkg =null);
        void Hide();
        void Set(ref Interface inter);
    }
}