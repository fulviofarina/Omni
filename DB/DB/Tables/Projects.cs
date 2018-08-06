using Rsx.Dumb;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        partial class ChannelsDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables;

            public bool defaultValue
            {
                //TODO: windows user instead
                get
                {
                    // LINAA set = this.DataSet as LINAA;
                    return !(this.DataSet as LINAA).SSFPref.FirstOrDefault().Overrides;
                }
            }

            public IEnumerable<DataColumn> ForbiddenNullCols
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
        }

        partial class IrradiationRequestsDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] { this.columnChannelName, this.columnIrradiationCode, this.columnNumber, this.columnIrradiationStartDateTime };
                    }
                    return nonNullables;
                }
            }
        }

        protected internal void handlersIrradiations()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Channels));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(IrradiationRequests));
        }

        partial class OrdersDataTable
        {
        }

        partial class ProjectsDataTable
        {
        }
    }
}