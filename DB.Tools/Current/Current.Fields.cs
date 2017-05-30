using System.Collections.Generic;
using System.Data;
using Rsx.Dumb;

namespace DB.Tools
{
   

    public partial class Current : ICurrent
    {
        protected internal BS bs;

        protected internal Interface Interface;

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
        /// Current Row
        /// </summary>
        public DataRow Vial
        {
            get
            {
                return (bs.Vial.Current as DataRowView)?.Row;
            }
        }
        /// <summary>
        /// Current Row
        /// </summary>
        public DataRow Rabbit
        {
            get
            {
                return (bs.Rabbit.Current as DataRowView)?.Row;
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
        /// Current Rows
        /// </summary>
        public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return Caster.Cast<DataRow>(bs.SubSamples.List as DataView);
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
    }
}