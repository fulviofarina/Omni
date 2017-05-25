using System;
using System.Collections.Generic;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {
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
                    //   c.Formula = formula;
                    c.MatrixID = matrixID;
                    c.Quantity = quantity;
                  //  c.Weight = formulaweight;
                    c.Element = element;
                    c.Unc = 0;
                    //c.UncUnit = "%";
                    c.QuantityUnit = "%";
               
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