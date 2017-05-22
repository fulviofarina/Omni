using System.Collections.Generic;
using System.Data;
using Rsx.Dumb; using Rsx;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    public partial class Current : ICurrent
    {

        protected BindingSources bs;

        protected Interface Interface;


        /// <summary>
        /// Current Row
        /// </summary>
        public DataRow Matrix
        {
            get
            {
                return (bs.Matrix.Current as DataRowView)?.Row;
            }
        }
     

        /// <summary>
        /// Current Row
        /// </summary>
        public DataRow SubSample
        {
            get
            {
                return (bs.SubSamples.Current as DataRowView)?.Row;
            }
        }

        public DataRow SubSampleMatrix
        {
            get
            {
                return (bs.SelectedMatrix.Current as DataRowView)?.Row;
            }
        }
        /// <summary>
        /// Current Row
        /// </summary>
        public DataRow Channel
        {
            get
            {
                return (bs.Channels.Current as DataRowView)?.Row;
            }
        }
    

        /// <summary>
        /// Current Rows
        /// </summary>
        public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return Caster.Cast<DataRow>(bs.SubSamples.List as DataView);
            }
        }

        public DataRow Irradiation
        {
            get
            {
                return (bs.Irradiations.Current as DataRowView)?.Row;
            }
        }

        /// <summary>
        /// Current Row
        /// </summary>
        public DataRow Unit
        {
            get
            {
                return (bs.Units.Current as DataRowView)?.Row;
            }
        }

        /// <summary>
        /// Current Row
        /// </summary>
        public IEnumerable<DataRow> Units
        {
            get
            {
                return Caster.Cast<DataRow>(bs.Units.List as DataView);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Current()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Current(ref BindingSources bss, ref Interface interfaces)
        {
            bs = bss;
            Interface = interfaces;
        }
    }
}