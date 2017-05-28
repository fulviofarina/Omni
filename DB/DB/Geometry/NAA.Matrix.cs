using System;
using System.Collections.Generic;
using System.Data;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        //Requires attention on DataColumn Changing Handlers

        partial class MatrixDataTable : IColumn
        {
            public Action populateMUESList;
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> ForbiddenNullCols
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
    }
}