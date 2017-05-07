using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    /// <summary>
    /// IMP{ORTATAAAAAANTE HACER ESTO FINO IMPLEMENTAR
    /// </summary>
    internal interface IColumn
    {
        void DataColumnChanged(object sender, DataColumnChangeEventArgs e);

        IEnumerable<DataColumn> NonNullables
        {
            get;
        }
    }

    internal interface IRow
    {
        void Check(DataColumn Column);
        void SetParent<T>(ref T rowParent);
    }
}

namespace DB
{
    public partial class LINAA
    {
        partial class ChannelsRow : IRow
        {

            public void SetParent<T>(ref T row)
            {
                throw new NotImplementedException();
            }
            public void Check(DataColumn Column)
            {
                bool nu = EC.CheckNull(Column, this);

                if (Column == this.tableChannels.ChannelNameColumn)
                {
                    if (nu)
                    {
                        ChannelName = "New Channel";
                    }
                }
                else if (Column == this.tableChannels.pEpiColumn)
                {
                    if (nu) pEpi = 0.82;
                }
                else if (Column == this.tableChannels.pEpiColumn)
                {
                    if (nu) pTh = 0.964;
                }
                else if (Column == this.tableChannels.A1Column)
                {
                    if (nu) A1 = 1;
                }
                else if (Column == this.tableChannels.A2Column)
                {
                    if (nu) A2 = 0.06;
                }
                else if (Column == this.tableChannels.FluxTypeColumn)
                {
                    if (nu) FluxType = 0.ToString();
                 
                        if (EC.CheckNull(this.tableChannels.WGtColumn, this) || this.tableChannels.overriders)
                        {
                            if (FluxType.Contains(2.ToString()))
                            {
                                WGt = 0.67;
                                //   BellFactor = 1.16;
                            }
                            else if (FluxType.Contains(1.ToString()))
                            {
                                WGt = 0.93;
                                //  BellFactor = 1.30;
                            }
                            else
                            {
                                WGt = 1;
                                // BellFactor = 1.16;
                            }
                        }

                        if (EC.CheckNull(this.tableChannels.BellFactorColumn, this) || this.tableChannels.overriders)
                        {
                            if (FluxType.Contains(2.ToString()))
                            {
                                //  WGt = 0.67;
                                BellFactor = 1.16;
                            }
                            else if (FluxType.Contains(1.ToString()))
                            {
                                //   WGt = 0.93;
                                BellFactor = 1.30;
                            }
                            else
                            {
                                //   WGt = 1;
                                BellFactor = 1.16;
                            }
                        }
                    
                }
                    
                
                else if (Column == this.tableChannels.WGtColumn)
                {
                    if (nu) WGt = 1;
                }
                else if (Column == this.tableChannels.BellFactorColumn)
                {
                    if (nu) BellFactor = 1.16;
                }
                else if (Column == this.tableChannels.nFactorColumn)
                {
                    if (nu) nFactor = 0.5;
                }
            }
        }

        partial class ChannelsDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables;
            public bool overriders
            {
                //TODO: windows user instead
                get
                {
                    // LINAA set = this.DataSet as LINAA;
                    return (this.DataSet as LINAA).SSFPref.FirstOrDefault().Overrides;
                }
            }
            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] { this.columnAlpha, this.columnf,
                            this.columnReactor, this.columnkth, this.columnkepi ,
                            columnIrReqCode,this.FluxTypeColumn,
                            columnpEpi, columnpTh, columnA1, columnA2,
                        columnBellFactor, columnWGt, columnnFactor};
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                ChannelsRow ch = e.Row as ChannelsRow;

                try
                {
                    if (NonNullables.Contains(e.Column))
                    {
                        ch.Check(e.Column);
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