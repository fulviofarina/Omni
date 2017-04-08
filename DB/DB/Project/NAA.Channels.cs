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
                        nonNullables = new DataColumn[] { this.columnAlpha, this.columnf, this.columnReactor, this.columnkth, this.columnkepi };
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
                        bool nu = EC.CheckNull(e.Column, e.Row);
                        if (e.Column == this.columnChannelName && nu)
                        {
                            ChannelsRow ch = e.Row as ChannelsRow;
                            ch.ChannelName = "New Channel";
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