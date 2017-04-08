using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IDetSol
    {
        ICollection<string> DetectorsList { get; set; }
        LINAA.GeometryRow DefaultGeometry { get; }

        void PopulateCOIList();

        void PopulateDetectorCurves();

        void PopulateDetectorAbsorbers();

        void PopulateDetectorDimensions();

        void PopulateDetectorHolders();
    }
}