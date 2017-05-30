using System.Collections.Generic;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IDetSol
    {
        LINAA.GeometryRow DefaultGeometry { get; }
        ICollection<string> DetectorsList { get; set; }

        void PopulateCOIList();

        void PopulateDetectorAbsorbers();

        void PopulateDetectorCurves();

        void PopulateDetectorDimensions();

        void PopulateDetectorHolders();
    }
}