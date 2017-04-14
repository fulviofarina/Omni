using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IGeometry
    {
        GeometryRow DefaultGeometry { get; }

        GeometryRow FindReferenceGeometry(string refName);

        void PopulateCompositions();

        void PopulateGeometry();

        void PopulateMatrix();

        void PopulateUnits();

     

        void PopulateVials();

        void PopulateXCOMList();
    }
}