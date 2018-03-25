namespace DB
{
    public partial class LINAA
    {
        public partial class CompositionsRow
        {
            public void SetValues(int matrixID, double quantity, string element)
            {
                // c.Formula = formula;
                MatrixID = matrixID;
                Quantity = quantity;
                // c.Weight = formulaweight;
                Element = element;
                Unc = 0;
                //c.UncUnit = "%";
                QuantityUnit = "%";
            }
        }
    }
}