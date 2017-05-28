using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IGeometry
    {
         MatrixRow AddNewMatrix();
         VialTypeRow AddNewVial(bool aRabbit);

        GeometryRow DefaultGeometry { get; }

        GeometryRow FindReferenceGeometry(string refName);

        void PopulateCompositions();

        void PopulateGeometry();

        void PopulateMatrix();

        void PopulateUnits();

     

        void PopulateVials();

        void PopulateMUESList();
    }
}