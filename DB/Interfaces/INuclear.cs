/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface INuclear
    {
        void PopulateElements();

        void PopulatepValues();

        void PopulateReactions();

        void PopulateSigmas();

        void PopulateSigmasSal();

        void PopulateYields();

        void CleanSigmas();
    }
}