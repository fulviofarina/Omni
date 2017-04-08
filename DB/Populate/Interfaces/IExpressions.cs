using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IExpressions
    {
        void PopulateSelectedExpression(bool setexpression);

        void PopulateColumnExpresions();

        //    bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref DB.LINAA.TAMDeleteMethod remover);
    }
}