using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
       public partial class IPeakAveragesDataTable
        {
            public IPeakAveragesRow NewIPeakAveragesRow(int k0Id, ref SubSamplesRow s)
            {
                IPeakAveragesRow ip = this.NewIPeakAveragesRow();
                this.AddIPeakAveragesRow(ip);
                ip.k0ID = k0Id;
                // ip.Radioisotope = iso; ip.Element = sym; ip.Energy = energy;
                if (!EC.IsNuDelDetch(s))
                {
                    ip.Sample = s.SubSampleName;
                    // if ( !s.IsIrradiationCodeNull()) ip.Project = s.IrradiationCode;
                }
                return ip;
            }
        }



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