using System;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        public partial class DetectorsAbsorbersDataTable
        {
            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
              
                try
                {
                    Dumb.CheckNull(e.Column, e.Row);
                }
                catch (SystemException ex)
                {
                    LINAA linaa = this.DataSet as LINAA;
                    e.Row.SetColumnError(e.Column, ex.Message);
                 //   Dumb.SetRowError(e.Row, e.Column, ex);
                    linaa.AddException(ex);
                }
            }
        }
    }
}