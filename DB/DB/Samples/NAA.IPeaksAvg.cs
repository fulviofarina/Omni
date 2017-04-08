using System;

namespace DB
{
    public partial class LINAA
    {
        public partial class IPeakAveragesRow
        {
            public double UncSq = 0;
            public double w = 0;
            public double w2 = 0;
            public double wX = 0;
            public double wX2 = 0;
        }

        partial class IPeakAveragesDataTable
        {
            public IPeakAveragesRow NewIPeakAveragesRow(Int32 k0Id, ref SubSamplesRow s)
            {
                IPeakAveragesRow ip = this.NewIPeakAveragesRow();
                this.AddIPeakAveragesRow(ip);
                ip.k0ID = k0Id;
                //	ip.Radioisotope = iso;
                //	ip.Element = sym;
                //	ip.Energy = energy;
                if (!Rsx.EC.IsNuDelDetch(s))
                {
                    ip.Sample = s.SubSampleName;
                    //   if (  !s.IsIrradiationCodeNull())   ip.Project = s.IrradiationCode;
                }
                return ip;
            }
        }
    }
}