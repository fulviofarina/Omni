using System.Data;

namespace DB.UI
{
    public interface ISampleColumn
    {
        string BindingPreferenceField { get; set; }
        DataRow BindingPreferenceRow { get; set; }
    }

    public interface IUnitSSFColumn
    {
        string BindableAsteriskField { get; set; }
        string BindingPreferenceField { get; set; }
        DataRow BindingPreferenceRow { get; set; }
        string BindingRoundingField { get; set; }
        string OriginalHeaderText { get; set; }
        int SSFCellType { get; set; }

        void PaintHeader();

        void SetEnable();

        void SetRounding();
    }
}