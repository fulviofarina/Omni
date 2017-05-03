using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class ChannelsDataTable
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] { this.columnAlpha, this.columnf, this.columnReactor, this.columnkth, this.columnkepi , columnIrReqCode, columnpEpi, columnpTh, columnA1, columnA2};
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    if (NonNullables.Contains(e.Column))
                    {
                        ChannelsRow ch = e.Row as ChannelsRow;

                        bool nu = EC.CheckNull(e.Column, e.Row);
                        if (e.Column == this.columnChannelName)
                        {
                            if (nu)
                            {
                            
                                ch.ChannelName = "New Channel";
                            }
                        }
                        else if (e.Column == this.columnpEpi)
                        {
                            if (nu) ch.pEpi = 0.82;
                        }
                        else if (e.Column == this.columnpTh)
                        {
                            if (nu) ch.pTh = 0.964;
                        }
                        else if (e.Column == this.columnA1)
                        {
                            if (nu) ch.A1 = 1;
                        }
                        else if (e.Column == this.columnA2)
                        {
                            if (nu) ch.A2 = 0.06;
                        }
                        return;
                    }
                   
                }
                catch (SystemException ex)
                {
                    e.Row.SetColumnError(e.Column, ex.Message);
                }
            }
        }
    }
}