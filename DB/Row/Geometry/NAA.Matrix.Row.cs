using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class MatrixRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableMatrix.Columns)
                {
                    Check(column);
                }
            }

            public void Check(DataColumn col)
            {
                bool nulo = EC.CheckNull(col, this);

                object[] obj = new object[] { this, renew };

                if (col == this.tableMatrix.MatrixDensityColumn)
                {
                    checkMatrixDensity();
                }
                else if (col == this.tableMatrix.MatrixDateColumn)
                {
                    if (nulo) MatrixDate = DateTime.Now;
                }
                else if (col == this.tableMatrix.MatrixCompositionColumn)
                {
                    nulo = checkMatrixComposition();
                }
                else if (col == this.tableMatrix.MatrixNameColumn)
                {
                    if (nulo)
                    {
                        MatrixName = "New @ " + DateTime.Now.ToLocalTime();
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
                Type tipo = typeof(T);
                if (tipo.Equals(typeof(MatrixRow)))
                {
                    MatrixRow toClone = rowParent as MatrixRow;
                    setMatrixToClone(ref toClone);
                }
            }

            private void setMatrixToClone(ref MatrixRow toClone)
            {
                if (EC.IsNuDelDetch(toClone)) return;
                if (MatrixDensity != toClone.MatrixDensity)
                {
                    MatrixDensity = toClone.MatrixDensity;
                }
                if (!toClone.IsMatrixCompositionNull())
                {
                    if (!IsMatrixCompositionNull())
                    {
                        if (MatrixComposition.CompareTo(toClone.MatrixComposition) != 0)
                        {
                            MatrixComposition = toClone.MatrixComposition;
                        }
                    }
                    else MatrixComposition = toClone.MatrixComposition;
                }
                if (!toClone.IsMatrixNameNull())
                {
                    if (!IsMatrixNameNull())
                    {
                        if (MatrixName.CompareTo(toClone.MatrixName) != 0)
                        {
                            MatrixName = toClone.MatrixName;
                        }
                    }
                    else MatrixName = toClone.MatrixName;
                }
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableMatrix.ForbiddenNullCols).Count();// != 0;
                return count != 0;
            }
            public bool HasCompositionsErrors()
            {
                IEnumerable<CompositionsRow> compos = GetCompositionsRows();
                if (compos.Count() != 0)
                {
                    return EC.HasErrors(compos);
                }
                return true;
            }

        }
        partial class MatrixRow : IRow
        {
            protected internal bool renew = false;
            public void DecomposeFormula(string formula, ref List<string> elements, ref List<string> moles)
            {
                Regex re = new Regex("[0-9]");
                string[] result = re.Split(formula);
                foreach (string s in result) if (!s.Equals(string.Empty)) elements.Add(s); // gives elements

                //NUMBERS
                Regex re2 = new Regex("[a-z]", RegexOptions.IgnoreCase);
                result = re2.Split(formula);
                foreach (string s in result) if (!s.Equals(string.Empty)) moles.Add(s); // gives moles
            }

            public CompositionsRow FindComposition(string element)
            {
                CompositionsRow c = null;
                IEnumerable<CompositionsRow> pickeable = GetCompositionsRows();
                pickeable = EC.NotDeleted<CompositionsRow>(pickeable);
                // bool add = false;
                if (pickeable.Count() != 0)
                {
                    c = pickeable.FirstOrDefault(o => o.Element.Equals(element));
                }
                return c;
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

        
            internal void setBasic(int subSamplesID, int templateID)
            {
                SubSampleID = subSamplesID; //the ID to identify
                MatrixComposition = string.Empty;
                MatrixName = string.Empty;
                //important, to rellocate the Parent MATRIX
                TemplateID = templateID;
            }

            protected internal bool checkMatrixComposition()
            {
                DataColumn col = this.tableMatrix.MatrixCompositionColumn;

                bool nulo = EC.CheckNull(col, this);

                IEnumerable<CompositionsRow> compos = GetCompositionsRows();
                if (compos.Count() == 0 || renew)
                {
                    if (renew)
                    {
                        this.tableMatrix.CleanCompositionsHandler?
                            .Invoke(compos, EventArgs.Empty);
                       
                        this.tableMatrix.MUESRequiredHandler?
                            .Invoke(this, EventArgs.Empty);
                        
                        updateUnit();
                        this.SetCompositionTableNull();
                        renew = false;
                    }
                    this.tableMatrix.AddCompositionsHandler?
                        .Invoke(this, EventArgs.Empty);
                }
                nulo = nulo || HasCompositionsErrors();
                if (nulo)
                {
                    SetColumnError(col, "This matrix composition has some errors");
                }

                return nulo;
            }

            protected internal void checkMatrixDensity()
            {
                if (renew)
                {
                    this.tableMatrix.MUESRequiredHandler?
                        .Invoke(this, EventArgs.Empty);
                    renew = false;
                }
            }

            protected internal void updateUnit()
            {
                MatrixDate = DateTime.Now;
                if (!EC.IsNuDelDetch(this.SubSamplesRow))
                {
                    this.SubSamplesRow?.UnitRow?.ValueChanged(true);
                }
            }
        }
    }
}