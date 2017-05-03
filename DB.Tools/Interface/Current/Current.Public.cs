using System.Collections.Generic;
using System.Data;
using Rsx;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    public partial class Current
    {

     

        public DataRow Matrix
        {
            get
            {
                return (bs.Matrix.Current as DataRowView)?.Row;
            }
        }

        /// <summary>
        /// Current
        /// </summary>
        public DataRow SubSample
        {
            get
            {
                return (bs.SubSamples.Current as DataRowView)?.Row;
            }
        }

        public DataRow Channel
        {
            get
            {
                return (bs.Channels.Current as DataRowView)?.Row;
            }
        }
    /// <summary>
    /// Current
    /// </summary>
    public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return Dumb.Cast<DataRow>(bs.SubSamples.List as DataView);
            }
        }

        /// <summary>
        /// Current
        /// </summary>
        public DataRow Unit
        {
            get
            {
                return (bs.Units.Current as DataRowView)?.Row;
            }
        }

        /// <summary>
        /// Current
        /// </summary>
        public IEnumerable<DataRow> Units
        {
            get
            {
                return Dumb.Cast<DataRow>(bs.Units.List as DataView);
            }
        }

        public Current()
        {
        }

        public Current(ref BindingSources bss, ref Interface interfaces)
        {
            bs = bss;
            Interface = interfaces;
        }
    }
}