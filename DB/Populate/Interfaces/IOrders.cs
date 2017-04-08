using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IOrders
    {
        void PopulateOrders();

        Int32? FindOrderID(string LabOrdRef);
    }
}