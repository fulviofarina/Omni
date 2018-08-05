using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        public partial class MeasurementsDataTable : IColumn
        {
            public EventHandler<EventData> CalcParametersHandler;
            private IEnumerable<DataColumn> nonNullables = null;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[]{ MeasurementColumn,
                     LiveTimeColumn,CountTimeColumn };
                    }

                    return nonNullables;
                }
            }

            private EventData eventData = new EventData();

            public SpecPrefRow SpecPrefRow
            {
                get
                {
                    CalcParametersHandler?.Invoke(null, eventData);
                    return eventData.Args[3] as SpecPrefRow;
                }
                set { }
            }
        }

     
    }
}