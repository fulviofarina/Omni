using System.Collections.Generic;
using static DB.LINAA;
/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IDetSol
    {
        GeometryRow DefaultGeometry { get; }
        ICollection<string> DetectorsList { get; set; }

        void PopulateCOIList();

        void PopulateDetectorAbsorbers();

        void PopulateDetectorCurves();

        void PopulateDetectorDimensions();

        void PopulateDetectorHolders();
    }
}