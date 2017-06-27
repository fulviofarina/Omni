using System.Collections.Generic;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IProjects
    {
        ICollection<string> ActiveProjectsList { get; }

        ProjectsRow FindBy(int? IrReqId, int? orderID, bool addIfNull);

        IList<string> ProjectsList { get; }

        // IList<SubSamplesRow> FindByProject(string project);

        void PopulateProjects();

        void PopulateToDoes();
        IList<string> ListOfHLProjects();
    }
}