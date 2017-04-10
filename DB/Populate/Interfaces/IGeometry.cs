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

        void PopulateUnitsByProject(int irrReqId);

        void PopulateVials();

        void PopulateXCOMList();
    }
}