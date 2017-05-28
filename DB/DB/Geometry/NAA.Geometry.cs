using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {

        protected internal void handlersGeometries()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Matrix));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(VialType));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Geometry));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SubSamples));
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

      
    }
}