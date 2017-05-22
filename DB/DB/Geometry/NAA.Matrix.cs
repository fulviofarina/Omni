using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        //Requires attention on DataColumn Changing Handlers

        partial class MatrixDataTable
        {
            public Action populateMUESList;
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[]
                        {
                            this.columnMatrixDensity,
                            this.columnMatrixName,
                            this.columnMatrixComposition,
                            this.columnMatrixDate
                        };
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                DataColumn col = e.Column;

                if (!NonNullables.Contains(col)) return;

                LINAA.MatrixRow m = e.Row as LINAA.MatrixRow;
                try
                {
                    m.Check(col);
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
            }

            /// <summary>
            /// Gets a non-repeated list of matrices IDs from wich their mass attenuation
            /// coefficients were stored in the database
            /// </summary>
            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                int col = e.Column.Ordinal;
                object propo = e.ProposedValue; //new value
                object val = e.Row[e.Column]; //old value
                MatrixRow m = (MatrixRow)e.Row;
                try
                {
                    m.Checking(e.Column, propo, val);
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
            }

            /// <summary>
            /// Retabifies the Matrix Composition
            /// </summary>
            private void Tabify()
            {
                foreach (MatrixRow item in this.Rows)
                {
                    IList<string[]> stripped = item.StripComposition(item.MatrixComposition);

                    item.AddOrUpdateComposition(stripped, true);
                }
            }
        }

        partial class MatrixRow : IRow
        {
            private bool renew = false;

            public static void DecomposeFormula(string formula, ref List<string> elements, ref List<string> moles)
            {
                System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("[0-9]");
                string[] result = re.Split(formula);
                foreach (string s in result) if (!s.Equals(string.Empty)) elements.Add(s); // gives elements

                //NUMBERS
                System.Text.RegularExpressions.Regex re2 = new System.Text.RegularExpressions.Regex("[a-z]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = re2.Split(formula);
                foreach (string s in result) if (!s.Equals(string.Empty)) moles.Add(s); // gives moles
            }

            public void Check(DataColumn col)
            {
                LINAA linaa = this.Table.DataSet as LINAA;

                bool nulo = EC.CheckNull(col, this);

                if (col == this.tableMatrix.MatrixDensityColumn)
                {
                    if (renew)
                    {
                        linaa.TAM.MUESTableAdapter.DeleteByMatrixID(MatrixID);

                        linaa.Matrix.populateMUESList?.Invoke();
                    }
                }
                else if (col == this.tableMatrix.MatrixDateColumn)
                {
                    if (nulo || renew)
                    {
                        MatrixDate = DateTime.Now;
                    }
                }
                else if (col == this.tableMatrix.MatrixCompositionColumn)
                {
                    IEnumerable<CompositionsRow> compos = GetCompositionsRows();
                

                    if (compos.Count() == 0 || renew)
                    {
                        if (renew)
                        {
                            linaa.Delete(ref compos);
                            linaa.Save(ref compos);
                            linaa.TAM.MUESTableAdapter.DeleteByMatrixID(MatrixID);
                            linaa.Matrix.populateMUESList?.Invoke();
                        }
                        AddOrUpdateComposition(null, false);
                    }


                    if (compos.Count() != 0)
                    {
                        if (EC.HasErrors(compos))
                        {
                            throw new SystemException("The composition rows have errors");
                        }
                    }


                }
            }

            public void Checking(DataColumn col, object propo, object val)
            {
                renew = false;

                if (DBNull.Value == propo) return; //if null go away
                if (val == DBNull.Value) return; //idem

                //if the density or the composition is changing
                //and the values are different

                if (col == this.tableMatrix.MatrixDensityColumn)
                {
                    double density = (double)propo;
                    double old = (double)val;
                    if (old != density)
                    {
                        renew = true;
                    }
                }
                else if (col == tableMatrix.MatrixCompositionColumn)
                {
                    string newcomposition = (string)propo;
                    string oldcomposition = (string)val;
                    if (oldcomposition.Trim().CompareTo(newcomposition.Trim()) != 0)
                    {
                        renew = true;
                    }
                }
            }

            public void AddOrUpdateComposition(IList<string[]> ls = null, bool code = true)
            {
                if (!code || ls == null) ls = StripComposition(MatrixComposition);
                //to store matrix composition
                string fullComposition = string.Empty;

                //ilst of element and Quantity
                foreach (string[] formCompo in ls)
                {
                    string element;
                    double quantity;
                    double formulaweight;
                    //decompose
                    element = formCompo[0];
                    formulaweight = 0;
                    quantity = Convert.ToDouble(formCompo[1]);

              //      elementQuantity(formCompo, out element, out quantity, out formulaweight);

                    //CODE COMPOSITION
                    if (code)
                    {
                        fullComposition += "#" + element.Trim() + "   (" + quantity + ")   ";
                        continue;
                    }

                    //ADD?
                    LINAA.CompositionsRow c = null; //prepare
                    CompositionsDataTable dt = (this.Table.DataSet as LINAA).Compositions;
                    c = dt.AddCompositionsRow(MatrixID, element, formulaweight, quantity, SubSampleID);
                }

                if (code)
                {
                    // fullComposition = fullComposition.Remove(fullComposition.Length - 1, 1);
                    this.MatrixComposition = fullComposition;
                }
            }

            /*
            public IList<string[]> StripCompositionOld(string composition)
            {
                List<string[]> ls = null;
                if (Rsx.EC.IsNuDelDetch(this)) return ls;
                if (string.IsNullOrEmpty(composition)) return ls;

                string matCompo = composition;
                string[] strArray = null;

                if (matCompo.Contains(',')) strArray = matCompo.Split(',');    ///
                else strArray = new string[] { matCompo };

                ls = new List<string[]>();
                for (int index = 0; index < strArray.Length; index++)
                {
                    string[] strArray2 = strArray[index].Trim().Split('(');
                    string formula = strArray2[0].Replace("#", null).Trim();
                    matCompo = strArray2[1].Replace("%", null).Trim();
                    matCompo = matCompo.Replace(")", null).Trim();
                    string[] formCompo = new string[] { formula, matCompo };
                    ls.Add(formCompo);
                }

                return ls;
            }

            */

            public void SetParent<T>(ref T rowParent)
            {
                throw new NotImplementedException();
            }

            public IList<string[]> StripComposition(string composition)
            {
                IList<string[]> ls = null;
                if (Rsx.EC.IsNuDelDetch(this)) return ls;
                if (string.IsNullOrEmpty(composition)) return ls;

                string matCompo = composition.Trim();

                if (matCompo.Contains(';')) matCompo = matCompo.Replace(';', ')');///

                string[] strArray = null;
                if (matCompo.Contains(')')) strArray = matCompo.Split(')');
                else strArray = new string[] { matCompo };
                strArray = strArray.Where(o => !string.IsNullOrEmpty(o.Trim())).ToArray();

                ls = new List<string[]>();

                for (int index = 0; index < strArray.Length; index++)
                {
                    string[] strArray2 = strArray[index].Trim().Split('(');
                    string formula = strArray2[0].Trim().Replace("#", null);
                    string quantity = strArray2[1].Trim();

                    string[] formCompo = new string[] { formula, quantity };
                    ls.Add(formCompo);
                }

                //STRING WAS DECODED INTO THE LIST ls
                StripMoreComposition(ref ls);

                return ls;
            }

            /// <summary>
            /// Strips the formula into elements and moles
            /// </summary>
            /// <param name="ls"></param>
            public string StripMoreComposition(ref IList<string[]> ls)
            {
                string buffer = string.Empty;
                //matSSF buffer will cointain the snippet for the Matrix Content in MatSSF

                foreach (string[] formulaQuantity in ls)
                {
                    //to auxiliary store elements and moles
                    List<string> elements = new List<string>();
                    List<string> moles = new List<string>();

                    //decomposes Al2O3 into Al 2 O 3  (element and mole)
                    DecomposeFormula(formulaQuantity[0], ref elements, ref moles);

                    //modified formula
                    string modified_formula = string.Empty;
                    for (int z = 0; z < elements.Count; z++)
                    {
                        modified_formula += elements[z] + " ";
                        if (moles.Count != 0) modified_formula += moles[z] + " ";
                    }
                    //Decomposed into Al 2 O 3  100

                    //full MATSSF Input Data for the provided Matrix Information
                    buffer += modified_formula + "\n";
                    buffer += formulaQuantity[1] + "\n";
                }

                return buffer;
            }

          
        }
    }
}