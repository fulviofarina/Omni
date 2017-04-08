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
                if (Rsx.EC.IsNuDelDetch(this)) return ls;
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
                            this.columnMatrixComposition
                        };
                    }
                    return nonNullables;
                }
            }

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
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
            }

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                DataColumn col = e.Column;

                if (!NonNullables.Contains(col)) return;

                LINAA linaa = this.DataSet as LINAA;

                LINAA.MatrixRow m = e.Row as LINAA.MatrixRow;

                try
                {
                    EC.CheckNull(col, e.Row);

                    if (col == this.columnMatrixDensity)
                    {
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
                    else if (col == this.columnMatrixComposition)
                    {
                        // EC.CheckNull(col, e.Row);
                        IEnumerable<LINAA.CompositionsRow> compos = m.GetCompositionsRows();
                        if (compos.Count() != 0)
                        {
                            if (EC.HasErrors(compos))
                            {
                                throw new SystemException("The composition rows have errors");
                                //EC.SetRowError(e.Row, e.Column, ex);
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
                                // EC.AcceptChanges(ref compos);
                            }
                            linaa.Compositions.AddCompositionRow(ref m);
                            m.Renew = false;
                        }
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
                }
            }
        }
    }
}