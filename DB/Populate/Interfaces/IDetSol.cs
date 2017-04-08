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
        void PopulateCOIList();

        void PopulateDetectorCurves();

        void PopulateDetectorAbsorbers();

        void PopulateDetectorDimensions();

        void PopulateDetectorHolders();
    }
}