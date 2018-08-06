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

        IList<string> ProjectsList { get; }

        ProjectsRow FindBy(int? IrReqId, int? orderID, bool addIfNull);

        // IList<SubSamplesRow> FindByProject(string project);

        IEnumerable<string> ListOfHLProjects();

        void PopulateProjects();

        void PopulateToDoes();
    }
}