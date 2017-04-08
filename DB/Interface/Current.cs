using System.Collections.Generic;
using System.Data;
using Rsx;

namespace DB
{
    public class Current
    {
        private BindingSources bs;

        public Current(ref BindingSources bsources)
        {
            bs = bsources;
        }

        public IEnumerable<LINAA.SubSamplesRow> SubSamples
        {
            get
            {
                return Dumb.Cast<LINAA.SubSamplesRow>(bs.SubSamples.List as DataView);
            }
        }
    }
}