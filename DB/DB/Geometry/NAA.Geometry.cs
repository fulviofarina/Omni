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
            handlers.Add(Matrix.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Matrix));
            handlers.Add(VialType.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(VialType));
            handlers.Add(Geometry.DataColumnChanged);

            dTWithHandlers.Add(Tables.IndexOf(Geometry));
            handlers.Add(SubSamples.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SubSamples));
        }


        partial class GeometryDataTable
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
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

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                DataColumn col = e.Column;

                if (!NonNullables.Contains(col)) return;

                //  LINAA linaa = this.DataSet as LINAA;
                LINAA.GeometryRow g = e.Row as LINAA.GeometryRow;

                try
                {
                    bool nu = EC.CheckNull(col, e.Row);
                    if (col == this.columnGeometryName && nu)
                    {
                        g.GeometryName = "No Name";
                    }
                    else if (nu && col == this.columnFillHeight || col == this.columnRadius)
                    {
                        VialTypeRow v = g.VialTypeRow;
                        if (v == null) return;
                        if (col == this.columnFillHeight)
                        {
                            if (v.IsMaxFillHeightNull()) return;
                            if (v.MaxFillHeight == 0) return;
                            g.FillHeight = v.MaxFillHeight;
                        }
                        else
                        {
                            if (!EC.CheckNull(col, e.Row)) return;
                            if (v.IsInnerRadiusNull()) return;
                            if (v.InnerRadius == 0) return;
                            g.Radius = v.InnerRadius;
                        }
                    }
                    return;
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