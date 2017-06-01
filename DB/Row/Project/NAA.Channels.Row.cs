using System;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    /// <summary>
    /// IMP{ORTATAAAAAANTE HACER ESTO FINO IMPLEMENTAR
    /// </summary>
}

namespace DB
{
    public partial class LINAA
    {
        partial class ChannelsRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableChannels.Columns)
                {
                    Check(column);
                }
                // return this.GetColumnsInError().Count() != 0;
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                return colsInE.Intersect(this.tableChannels.ForbiddenNullCols)
                    .Count() != 0;
            }

            public void SetParent<T>(ref T row, object[] args = null)
            {
                // throw new NotImplementedException();
            }

            public void Check(DataColumn Column)
            {
                bool nu = EC.CheckNull(Column, this);

                if (Column == this.tableChannels.ChannelNameColumn)
                {
                    if (nu)
                    {
                        ChannelName = "New @ " + DateTime.Now.ToLocalTime();
                    }
                }
                else if (Column == this.tableChannels.pEpiColumn)
                {
                    if (nu) pEpi = 0.82;
                }
                else if (Column == this.tableChannels.pThColumn)
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

                    if (EC.CheckNull(this.tableChannels.WGtColumn, this) || this.tableChannels.defaultValue)
                    {
                        if (FluxType.Contains(2.ToString()))
                        {
                            WGt = 0.67;
                            // BellFactor = 1.16;
                        }
                        else if (FluxType.Contains(1.ToString()))
                        {
                            WGt = 0.93;
                            // BellFactor = 1.30;
                        }
                        else
                        {
                            WGt = 1;
                            // BellFactor = 1.16;
                        }
                    }

                    if (EC.CheckNull(this.tableChannels.BellFactorColumn, this) || this.tableChannels.defaultValue)
                    {
                        if (FluxType.Contains(2.ToString()))
                        {
                            // WGt = 0.67;
                            BellFactor = 1.16;
                        }
                        else if (FluxType.Contains(1.ToString()))
                        {
                            // WGt = 0.93;
                            BellFactor = 1.30;
                        }
                        else
                        {
                            // WGt = 1;
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
    }
}