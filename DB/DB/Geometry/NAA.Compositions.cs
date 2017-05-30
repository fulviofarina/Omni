using System;
using System.Collections.Generic;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {
        public partial class CompositionsRow
        {
            public void SetValues(int matrixID, double quantity, string element)
            {
                //   c.Formula = formula;
                MatrixID = matrixID;
                Quantity = quantity;
                //  c.Weight = formulaweight;
                Element = element;
                Unc = 0;
                //c.UncUnit = "%";
                QuantityUnit = "%";
            }
        }
        public partial class CompositionsDataTable
        {

            public CompositionsRow AddCompositionsRow(int matrixID, string element, double formulaweight, double quantity)
            {
                CompositionsRow c = null;
                try
                {
                    IEnumerable<CompositionsRow> pickeable = EC.NotDeleted<CompositionsRow>(this);
                    bool add = false;
                    if (pickeable.Count() != 0)
                    {
                        c = pickeable.FirstOrDefault(o => !o.IsMatrixIDNull() && o.MatrixID == matrixID && o.Element.Equals(element));
                    }
                    if (c == null)
                    {
                        c = NewCompositionsRow();
                        add = true;
                    }

                    c.SetValues(matrixID, quantity, element);
              
               
                    if (add) AddCompositionsRow(c);
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(c, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
                return c;
            }
        }

     
    }
}