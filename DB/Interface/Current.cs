using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using static DB.LINAA;

namespace DB
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>
    public class Current
    {
        private BindingSources bs;

        public Current(ref BindingSources bsources)
        {
            bs = bsources;
        }

        public DataRow SubSample
        {
            get
            {
                return (bs.SubSamples.Current as DataRowView).Row;
            }
        }

        public DataRow Unit
        {
            get
            {
                return (bs.Units.Current as DataRowView).Row;
            }
        }

        public IEnumerable<DataRow> Units
        {
            get
            {
                return (bs.Units.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }

        public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return (bs.SubSamples.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }
    }
}