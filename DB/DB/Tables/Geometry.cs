using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB
{
    public partial class LINAA
    {
        public partial class DetectorsAbsorbersDataTable : IColumn
        {
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }
        }

        partial class GeometryDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    // IEnumerable<DataColumn> nonNullables;

                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[]
                        {
                            this.columnFillHeight,
                            this.columnRadius,
                            this.columnGeometryName,
                            this.columnMatrixDensity,
                            this.columnMatrixName,
                            this.columnCreationDateTime,
                            this.columnVialTypeRef
                        };
                    }
                    return nonNullables;
                }
            }
        }

        partial class MatrixDataTable : IColumn

        {
            public EventHandler<EventData> CalcParametersHandler;
            public EventHandler CleanMUESHandler;
            public EventHandler CleanCompositionsHandler;

            // public EventHandler MUESRequiredHandler;
            public EventHandler AddCompositionsHandler;

            /*
               public  new DataColumn MatrixNameColumn
                   {
                   get
                   {
                       this.columnMatrixName.ColumnName = "Prueba";
                       return this.columnMatrixName;
                   }
                   set

                   {
                       this.columnMatrixName = value;
                   }
                   }
               */
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[]
                        {
                           this.columnToDo,
                            this.columnMatrixName,
                            this.columnMatrixComposition,
                            this.columnMatrixDate
                        };
                    }
                    return nonNullables;
                }
            }

            // public EventHandler CleanMUESPicturesHandler;

            /// <summary>
            /// Gets a non-repeated list of matrices IDs from wich their mass attenuation
            /// coefficients were stored in the database
            /// </summary>
            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                MatrixRow m = (MatrixRow)e.Row;
                try
                {
                    m.Checking(e);
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
        }

        //Requires attention on DataColumn Changing Handlers
        partial class VialTypeDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnMatrixDensity,
                            this.columnMatrixName,
                            this.columnVialTypeRef ,
                        this.columnMaxFillHeight,
                        this.InnerRadiusColumn};
                    }
                    return nonNullables;
                }
            }
        }
    }
}