using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class VialTypeDataTable
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
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

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                DataColumn col = e.Column;
                VialTypeRow subs = e.Row as VialTypeRow;
                try
                {
                    if (NonNullables.Contains(col))
                    {
                        bool nu = EC.CheckNull(e.Column, e.Row);
                        if (col == this.columnVialTypeRef && nu) subs.VialTypeRef = "No Name";
                    }
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }
    }
}