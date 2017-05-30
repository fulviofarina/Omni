using System.Collections.Generic;
using System.Data;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IColumn
    {
        // void DataColumnChanged(object sender, DataColumnChangeEventArgs e);

        IEnumerable<DataColumn> ForbiddenNullCols
        {
            get;
        }
    }
}