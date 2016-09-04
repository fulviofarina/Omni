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
                        nonNullables = new DataColumn[] { this.columnMatrixDensity, this.columnMatrixName, this.columnVialTypeRef };
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                LINAA linaa = this.DataSet as LINAA;
                try
                {
                    VialTypeRow subs = e.Row as VialTypeRow;
                    if (NonNullables.Contains(e.Column))
                    {
                        bool nu = Dumb.CheckNull(e.Column, e.Row);
                        if (e.Column == this.columnVialTypeRef && nu) subs.VialTypeRef = "No Name";
                        return;
                    }
                }
                catch (SystemException ex)
                {
                    Dumb.SetRowError(e.Row, e.Column, ex);
                    linaa.AddException(ex);
                }
            }
        }
    }
}