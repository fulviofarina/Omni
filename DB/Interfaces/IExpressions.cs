/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IExpressions
    {
        void PopulateColumnExpresions();

        void PopulateSelectedExpression(bool setexpression);

        //    bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref DB.LINAA.TAMDeleteMethod remover);
    }
}