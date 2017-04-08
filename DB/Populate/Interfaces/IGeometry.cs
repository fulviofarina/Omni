using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IGeometry
    {
        void PopulateXCOMList();

        GeometryRow DefaultGeometry { get; }

        GeometryRow FindReferenceGeometry(string refName);

        void PopulateCompositions();

        void PopulateMatrix();

        void PopulateVials();

        void PopulateUnits();

        void PopulateUnitsByProject(int irrReqId);

        void PopulateGeometry();
    }
}