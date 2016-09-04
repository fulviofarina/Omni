using System;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class DetectorsAbsorbersDataTable
        {
            private void Data_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                LINAA linaa = this.DataSet as LINAA;
                try
                {
                    Dumb.CheckNull(e.Column, e.Row);
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