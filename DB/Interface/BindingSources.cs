using System.Collections.Generic;
using System.Data;
using Rsx;

namespace DB
{
    /// <summary>
    /// This is a class to attach the binding sources
    /// </summary>
    public class BindingSources
    {
        //binding sources to attach;
        public dynamic SubSamples;

        /// <summary>
        /// not attached yet
        /// </summary>
        public dynamic Monitors;

        public dynamic Units;
        public dynamic Matrix;
        public dynamic Vial;
        public dynamic Geometry;
        public dynamic Rabbit;
        public dynamic Channels;
        public dynamic Irradiations;

        public BindingSources()
        {
        }
    }
}