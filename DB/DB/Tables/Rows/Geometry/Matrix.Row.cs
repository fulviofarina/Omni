using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        public partial class MatrixRow : ICalculableRow
        {
            protected internal bool isBusy = false; // { get; set; }

            public bool IsBusy
            {
                get
                {
                    return isBusy;
                }

                set
                {
                    isBusy = value;

                    if (isBusy)
                    {
                    }
                    else
                    {
                        ToDo = false;
                        SetColumnError(this.tableMatrix.MatrixNameColumn, null);
                    }

                    MatrixDate = DateTime.Now;
                }
            }

            public void SetAsNotCalculated()
            {
                isBusy = false;
                ToDo = true;
                MatrixDate = DateTime.Now;
                CleanMUES();
                // SetColumnError(this.tableMatrix.MatrixNameColumn, "Cancelled");
            }
        }

        public partial class MatrixRow : IRow, ISetteable
        {
            private bool needsMUES = false;

            public bool NeedsMUES
            {
                get

                {
                    IEnumerable<MUESRow> mues = GetMUESRows();
                    int count = mues.Count();
                    needsMUES = count == 0;
                    return needsMUES;
                }
                set
                {
                    needsMUES = value;
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

                // object[] obj = new object[] { this, ToDo };
                if (col == this.tableMatrix.ToDoColumn)
                {
                    if (nulo) ToDo = true;

                    // if (this.GetMUESRows().Count() == 0) ToDo = true;
                }
                else if (col == this.tableMatrix.MatrixDensityColumn)
                {
                    checkMatrixDensity();
                }
                else if (col == this.tableMatrix.MatrixDateColumn)
                {
                    if (nulo) MatrixDate = DateTime.Now;
                }
                else if (col == this.tableMatrix.MatrixCompositionColumn)
                {
                    checkMatrixComposition();
                }
                else if (col == this.tableMatrix.MatrixNameColumn)
                {
                    if (nulo)
                    {
                        MatrixName = "NEW @ " + DateTime.Now.ToShortDateString();
                    }
                }
            }

            private bool compositionRenew = false; // { get; set; }
            private bool densityRenew = false; // { get; set; }

            public void Checking(DataColumnChangeEventArgs e)
            {
                object propo = e.ProposedValue;
                object val = e.Row[e.Column];
                DataColumn col = e.Column;
                // e.Column, e.ProposedValue, e.Row[e.Column]
                if (DBNull.Value == propo) return; //if null go away
                if (val == DBNull.Value) return; //idem

                //if the density or the composition is changing
                //and the values are different

                if (col == this.tableMatrix.MatrixDensityColumn)
                {
                    densityRenew = false;
                    double density = (double)propo;
                    double old = (double)val;
                    if (old != density)
                    {
                        densityRenew = true;
                    }
                }
                else if (col == tableMatrix.MatrixCompositionColumn)
                {
                    compositionRenew = false;
                    string newcomposition = (string)propo;
                    string oldcomposition = (string)val;
                    oldcomposition = oldcomposition.Trim().Replace(" ", null);
                    newcomposition = newcomposition.Trim().Replace(" ", null);
                    oldcomposition = oldcomposition.Replace("\n", null);
                    newcomposition = newcomposition.Replace("\n", null);
                    oldcomposition = oldcomposition.Replace("\t", null);
                    newcomposition = newcomposition.Replace("\t", null);
                    if (oldcomposition.CompareTo(newcomposition) != 0)
                    {
                        compositionRenew = true;
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

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableMatrix.ForbiddenNullCols).Count();// != 0;
                return count != 0;
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

        partial class MatrixRow
        {
            private void setMatrixToClone(ref MatrixRow toClone)
            {
                if (EC.IsNuDelDetch(toClone)) return;
                //if density is null
                if (MatrixDensity != toClone.MatrixDensity)
                {
                    MatrixDensity = toClone.MatrixDensity;
                }
                //if composition is null
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
                //if name is null
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

            // protected internal bool ToDo = false;

            protected internal void setBasic(int? subSamplesID, int? templateID)
            {
                if (subSamplesID != null) SubSampleID = (int)subSamplesID; //the ID to identify
                if (templateID != null) TemplateID = (int)templateID;
                MatrixDate = DateTime.Now;
                MatrixComposition = string.Empty;
                MatrixName = string.Empty;
                //important, to rellocate the Parent MATRIX
            }

            private bool checkMatrixComposition()
            {
                DataColumn col = this.tableMatrix.MatrixCompositionColumn;
                bool nulo = EC.CheckNull(col, this);
                //check compositions
                IEnumerable<CompositionsRow> compos = GetCompositionsRows();
                if (compos.Count() == 0 || compositionRenew)
                {
                    //needs renewal
                    if (compositionRenew)
                    {
                        cleanCompositions(ref compos);
                        compositionRenew = false;
                    }
                    this.tableMatrix.AddCompositionsHandler?.Invoke(this, EventArgs.Empty);
                }
                nulo = nulo || HasCompositionsErrors();
                if (nulo)
                {
                    SetColumnError(col, "This matrix composition has some errors");
                }

                return nulo;
            }

            private void cleanCompositions(ref IEnumerable<CompositionsRow> compos)
            {
                CleanMUES();

                this.tableMatrix.CleanCompositionsHandler?.Invoke(compos, EventArgs.Empty);

                ToDo = true;
                MatrixDate = DateTime.Now;
                if (!EC.IsNuDelDetch(this.SubSamplesRow))
                {
                    this.SubSamplesRow?.UnitRow?.ValueChanged(true);
                }
                this.SetCompositionTableNull();

                // this.tableMatrix.CleanMUESPicturesHandler?.Invoke(this, EventArgs.Empty);
            }

            public void CleanMUES()
            {
                EventData ebento = new EventData();
                this.tableMatrix.CalcParametersHandler?.Invoke(this, ebento);
                this.tableMatrix.CleanMUESHandler?.Invoke(this, ebento);
                //ebento.Args = null;
                //ebento = null;
            }

            private void checkMatrixDensity()
            {
                if (densityRenew)
                {
                    // this.tableMatrix.MUESRequiredHandler? .Invoke(this, EventArgs.Empty);
                    densityRenew = false;
                }
            }
        }
    }
}