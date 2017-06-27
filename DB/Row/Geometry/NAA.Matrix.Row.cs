using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        partial class MatrixRow : IRow
        {
            public bool NeedsMUES
            {
                get

                {
                    IEnumerable<MUESRow> mues = GetMUESRows();
                    return mues.Count() == 0;
                }
            }

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
                if ( compos.Count() == 0  || renew)
                {
                 
                       if (renew )
                        {
                            this.tableMatrix.CleanCompositionsHandler?
                                .Invoke(compos, EventArgs.Empty);
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
                  //  this.tableMatrix.MUESRequiredHandler?
                  //      .Invoke(this, EventArgs.Empty);
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