using System.Collections.Generic;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IToDoes
    {
        IList<string> ToDoesList { get; }

        void PopulateToDoes();
    }
}