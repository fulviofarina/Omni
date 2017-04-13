using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

 

    public partial class Current
    {
        public Current(ref BindingSources bss, ref Interface interfaces)
        {
            bs = bss;
            Interface = interfaces;
        }

        /// <summary>
        /// Current
        /// </summary>
        public DataRow SubSample
        {
            get
            {
                return (bs.SubSamples.Current as DataRowView).Row;
            }
        }

        /// <summary>
        /// Current
        /// </summary>
        public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return (bs.SubSamples.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }

        /// <summary>
        /// Current
        /// </summary>
        public DataRow Unit
        {
            get
            {
                return (bs.Units.Current as DataRowView).Row;
            }
        }

        /// <summary>
        /// Current
        /// </summary>
        public IEnumerable<DataRow> Units
        {
            get
            {
                return (bs.Units.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }
    }
}