using System;

namespace DB
{
    public partial class LINAA
    {
        partial class PeaksRow
        {
            public double UncSq = 0;
            public double w = 0;
            public double w2 = 0;
            public double wX = 0;
            public double wX2 = 0;

            public int ETAInMin = 0;
        }

        partial class PeaksDataTable
        {
            public LINAA.PeaksRow NewPeaksRow(Int32 k0Id, double energy, ref SubSamplesRow s, ref MeasurementsRow m)
            {
                LINAA.PeaksRow peak = this.NewPeaksRow();
                this.AddPeaksRow(peak);
                //  peak.Selected = true;
                peak.Ready = false;
                peak.ID = k0Id;
                peak.Energy = energy;
                //	peak.Sym = sym;
                //	peak.Iso = iso;

                if (!Rsx.EC.IsNuDelDetch(s))
                {
                    peak.IrradiationID = s.IrradiationRequestsID;
                    peak.SampleID = s.SubSamplesID;
                }
                if (!Rsx.EC.IsNuDelDetch(m))
                {
                    peak.MeasurementID = m.MeasurementID;
                    //  peak.Measurement = m.Measurement;
                }

                return peak;
            }


        }
    }
}