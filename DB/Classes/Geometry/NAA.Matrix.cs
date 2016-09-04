using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using DB;

namespace DB
{
    public partial class LINAA
    {
        public partial class CompositionsRow
        {

        }
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
                    IEnumerable<CompositionsRow> pickeable = Dumb.NotDeleted<CompositionsRow>(this);
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
                    Dumb.SetRowError(c, ex);
                }
                return c;
            }
        }

        //Requires attention on DataColumn Changing Handlers

        partial class MatrixRow
        {
            private bool renew = false;

            public bool Renew
            {
                get { return renew; }
                set { renew = value; }
            }

            public IList<string[]> StripComposition()
            {
                System.Collections.Generic.List<string[]> ls = null;
                if (Rsx.Dumb.IsNuDelDetch(this)) return ls;
                if (this.IsMatrixCompositionNull()) return ls;

                string matCompo = this.MatrixComposition;
                string[] strArray = null;

                if (matCompo.Contains(',')) strArray = matCompo.Split(',');    ///
                else strArray = new string[] { matCompo };

                ls = new System.Collections.Generic.List<string[]>();
                for (int index = 0; index < strArray.Length; index++)
                {
                    string[] strArray2 = strArray[index].Trim().Split('(');
                    string formula = strArray2[0].Replace("#", null).Trim();
                    string composition = strArray2[1].Replace("%", null).Trim();
                    composition = composition.Replace(")", null).Trim();
                    string[] formCompo = new string[] { formula, composition };
                    ls.Add(formCompo);
                }

                return ls;
            }
        }

        partial class MatrixDataTable
        {
            public Action PopulateXCom;

            /// <summary>
            /// Gets a non-repeated list of matrices IDs from wich their mass attenuation coefficients were stored in the database
            /// </summary>

            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                MatrixRow m = (MatrixRow)e.Row;
                int col = e.Column.Ordinal;
                object propo = e.ProposedValue; //new value
                object val = e.Row[e.Column]; //old value
                if (propo == null) return; //if null go away
                if (val == null) return; //idem
                try
                {
                    if (col == this.columnMatrixDensity.Ordinal)
                    {
                        double density = (double)propo;
                        double old = (double)val;
                        if (old != density) m.Renew = true;
                    }
                    else if (col == this.MatrixCompositionColumn.Ordinal)
                    {
                        string newcomposition = (string)propo;
                        string oldcomposition = (string)val;
                        if (oldcomposition.CompareTo(newcomposition) != 0) m.Renew = true;
                    }
                }
                catch (SystemException ex)
                {
                    Dumb.SetRowError(e.Row, e.Column, ex);
                }
            }

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                LINAA linaa = this.DataSet as LINAA;

                try
                {
                    LINAA.MatrixRow m = e.Row as LINAA.MatrixRow;

                    if (e.Column == this.columnMatrixName)
                    {
                        Dumb.CheckNull(e.Column, e.Row);
                    }
                    else if (e.Column == this.columnMatrixDensity)
                    {
                        Dumb.CheckNull(e.Column, e.Row);
                        if (m.Renew)
                        {
                            if (m.Renew)
                            {
                                linaa.TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
                            }
                            PopulateXCom();
                            m.Renew = false;
                        }
                        /*
                                                if (listOfMatricesInXCOM == null || m.Renew)
                                                {
                                                    if (m.Renew) linaa.TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
                                                    PopulateXCOMList();
                                                    m.Renew = false;
                                                }

                                                e.Row.SetColumnError(this.XCOMColumn, null);

                                                if (this.listOfMatricesInXCOM.Contains(m.MatrixID))    e.Row[this.XCOMColumn] =  false;
                                                else
                                                {
                                                    e.Row[this.XCOMColumn] = true;
                                                    e.Row.SetColumnError(this.XCOMColumn, "Please click the XCOM button to retrieve the Mass Attenuation coefficients for this matrix\n\nThis is fundamental for Efficiency and COI calculation");
                                                }
                                             */
                    }
                    else if (e.Column == this.columnMatrixComposition)
                    {
                        Dumb.CheckNull(e.Column, e.Row);
                        IEnumerable<LINAA.CompositionsRow> compos = m.GetCompositionsRows();
                        if (compos.Count() != 0)
                        {
                            if (Dumb.HasErrors(compos))
                            {
                                throw new SystemException("The composition rows have errors");
                                //Dumb.SetRowError(e.Row, e.Column, ex);
                            }
                        }
                        if (m.HasErrors) return;
                        if (compos.Count() == 0 || m.Renew)
                        {
                            if (m.Renew)
                            {
                                linaa.Delete(ref compos);
                                linaa.Save(ref compos);
                                linaa.TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
                                PopulateXCom();
                                // Dumb.AcceptChanges(ref compos);
                            }
                            linaa.Compositions.AddCompositionRow(ref m);
                            m.Renew = false;
                        }
                    }
                }
                catch (SystemException ex)
                {
                    Dumb.SetRowError(e.Row, e.Column, ex);
                }
            }
        }
    }
}