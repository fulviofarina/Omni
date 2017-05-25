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

        partial class MatrixRow : IRow
        {
            public void DecomposeFormula(string formula, ref List<string> elements, ref List<string> moles)
            {
                System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("[0-9]");
                string[] result = re.Split(formula);
                foreach (string s in result) if (!s.Equals(string.Empty)) elements.Add(s); // gives elements

                //NUMBERS
                System.Text.RegularExpressions.Regex re2 = new System.Text.RegularExpressions.Regex("[a-z]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = re2.Split(formula);
                foreach (string s in result) if (!s.Equals(string.Empty)) moles.Add(s); // gives moles
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                return colsInE.Intersect(this.tableMatrix.NonNullables).Count() != 0;
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

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                throw new NotImplementedException();
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

            public void AddOrUpdateComposition(IList<string[]> ls = null, bool code = true)
            {
                 bool nulo = EC.CheckNull(this.tableMatrix.MatrixCompositionColumn, this);
                if (nulo) return;
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

                    // elementQuantity(formCompo, out element, out quantity, out formulaweight);

                    //CODE COMPOSITION
                    if (code)
                    {
                        fullComposition += "#" + element.Trim() + "   (" + quantity + ")   ";
                        continue;
                    }

                    //ADD?
                    LINAA.CompositionsRow c = null; //prepare
                    CompositionsDataTable dt = (this.Table.DataSet as LINAA).Compositions;
                    c = dt.AddCompositionsRow(MatrixID, element, formulaweight, quantity);

                    if (!EC.IsNuDelDetch(this.SubSamplesRow))
                    {
                        c.SubSampleID = this.SubSamplesRow.SubSamplesID;
                    }
                }

                if (code)
                {
                    // fullComposition = fullComposition.Remove(fullComposition.Length - 1, 1);
                    this.MatrixComposition = fullComposition;
                }
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
        }

        partial class MatrixRow
        {
            protected bool renew = false;

            internal void SetBasic(int subSamplesID, int templateID)
            {
                SubSampleID = subSamplesID; //the ID to identify
                MatrixComposition = string.Empty;
                MatrixName = string.Empty;
                //important, to rellocate the Parent MATRIX
                TemplateID = templateID;
            }

            internal void cloneFromMatrix(ref MatrixRow toClone)
            {
                if (EC.IsNuDelDetch(toClone)) return;
                if (MatrixDensity != toClone.MatrixDensity)
                {
                    MatrixDensity = toClone.MatrixDensity;
                }
                if (MatrixComposition.CompareTo(toClone.MatrixComposition) != 0)
                {
                    MatrixComposition = toClone.MatrixComposition;
                }
                if (MatrixName.CompareTo(toClone.MatrixName) != 0)
                {
                    MatrixName = toClone.MatrixName;
                }
            }
        }
    }
}