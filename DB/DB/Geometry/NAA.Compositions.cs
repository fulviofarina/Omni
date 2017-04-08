using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        public partial class CompositionsDataTable
        {
            /// <summary>
            /// Creates a IList of CompositionsRow from a given MatrixRow
            /// </summary>
            /// <param name="aux">MatrixRow as input</param>
            /// <returns>the IList of Compositions Rows</returns>
            public IList<DB.LINAA.CompositionsRow> AddCompositionRow(ref MatrixRow aux)
            {
                IList<string[]> ls = aux.StripComposition();
                IList<LINAA.CompositionsRow> added = new List<LINAA.CompositionsRow>();
                if (ls != null)
                {
                    AddCompositionRow(aux.MatrixID, ref ls, ref added);
                }
                return added;
            }

            public void AddCompositionRow(int MatrixID, ref IList<string[]> ls, ref IList<LINAA.CompositionsRow> added)
            {
                //no commas, so make table directly....
                bool okreturn = added != null;
                foreach (string[] formCompo in ls)
                {
                    string element = formCompo[0];
                    double formulaweight = 0;
                    double quantity = Convert.ToDouble(formCompo[1]);

                    LINAA.CompositionsRow c = AddCompositionsRow(MatrixID, element, formulaweight, quantity);
                    if (okreturn) added.Add(c);
                }
            }

            private CompositionsRow AddCompositionsRow(int matrixID, string element, double formulaweight, double quantity)
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
                    c.Weight = formulaweight;
                    c.Element = element;
                    c.Unc = 0;
                    c.UncUnit = "%";
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

        //Requires attention on DataColumn Changing Handlers
    }
}