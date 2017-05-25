using System;
using System.Data;
using Rsx;

namespace DB
{
    public partial class LINAA
    {

        partial class PeaksRow : IRow
        {
            public double UncSq = 0;
            public double w = 0;
            public double w2 = 0;
            public double wX = 0;
            public double wX2 = 0;

            public int ETAInMin = 0;

            public void SetBasic(int k0ID, double energy)
            {
                Ready = false;
                ID = k0ID;
                Energy = energy;
            }

            public void Check(DataColumn Column)
            {
                throw new NotImplementedException();
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                Type t = typeof(T);
                if (!EC.IsNuDelDetch(rowParent as DataRow)) return;
                if (t.Equals(typeof(SubSamplesRow)))
                {
                    SubSamplesRow s = rowParent as SubSamplesRow;
                    IrradiationID = s.IrradiationRequestsID;
                    SampleID = s.SubSamplesID;

                    // peak.Measurement = m.Measurement;

                    // MatrixRow = rowParent as MatrixRow;
                }
                else if (t.Equals(typeof(MeasurementsRow)))
                {
                    MeasurementsRow m = rowParent as MeasurementsRow;
                    MeasurementID = m.MeasurementID;
                    // VialTypeRow = rowParent as VialTypeRow;
                }
              
                else throw new NotImplementedException();

                // throw new NotImplementedException();
            }
        }

      
    }
}