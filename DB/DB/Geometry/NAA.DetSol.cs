using System;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {
        protected internal void handlersDetSol()
        {
            handlers.Add(DetectorsAbsorbers.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(DetectorsAbsorbers));
        }

        public partial class DetectorsAbsorbersDataTable
        {
            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                try
                {
                    EC.CheckNull(e.Column, e.Row);
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