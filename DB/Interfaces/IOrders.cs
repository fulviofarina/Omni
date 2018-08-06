using System.Collections.Generic;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IOrders
    {
        ICollection<string> OrdersList { get; }

        // Int32? FindOrderID(String LabOrderRef)
        int? FindOrderID(string LabOrdRef);

        void PopulateOrders();
    }
}