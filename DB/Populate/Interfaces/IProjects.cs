using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IProjects
    {
        IList<SubSamplesRow> FindByProject(string project);

        void PopulateToDoes();

        void PopulateProjects();
    }
}