using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
      

        partial class MonitorsDataTable : IColumn
        {
            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    MonitorsRow m = e.Row as MonitorsRow;

                    m.Check(e.Column);
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