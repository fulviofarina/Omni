using System;
using System.Linq;
using Rsx.CAM;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        public partial class MeasurementsRow
        {
            public bool IsAcquiring = false;

            private object tag;

            public object Tag
            {
                get { return tag; }
                set { tag = value; }
            }

            public void SetCAMData(ref IDetectorX reader)
            {
                IsAcquiring = true;
                Selected = true;

                try
                {
                    SetColumnError(this.tableMeasurements.DTColumn, null);
                    DT = (double)reader.DT;
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableMeasurements.DTColumn, ex.Message);
                }

                try
                {
                    SetColumnError(this.tableMeasurements.CountTimeColumn, null);
                    CountTime = reader.CountTime;
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableMeasurements.CountTimeColumn, ex.Message);
                }
                try
                {
                    SetColumnError(this.tableMeasurements.MeasurementStartColumn, null);
                    MeasurementStart = reader.StartDate;
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableMeasurements.MeasurementStartColumn, ex.Message);
                }
                try
                {
                    SetColumnError(this.tableMeasurements.LiveTimeColumn, null);
                    LiveTime = reader.LiveTime;
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableMeasurements.LiveTimeColumn, ex.Message);
                }
            }

            /// <summary>
            /// Sets the measurement name
            /// </summary>
            /// <param name="measName"></param>
            public void SetName(string measName)
            {
                Measurement = measName;
                Sample = measName.Substring(0, measName.Length - 3);
                string DPN = measName.Replace(Sample, null).Trim();
                MeasurementNr = DPN.Substring(2, 1);
                Position = Convert.ToInt16(DPN.Substring(1, 1));
                Detector = DPN.Substring(0, 1);
                Project = string.Empty;
                if (!EC.IsNuDelDetch(SubSamplesRow))
                {
                    if (!SubSamplesRow.IsIrradiationCodeNull()) Project = SubSamplesRow.IrradiationCode;
                }
            }

            public bool ShouldSelectIt()
            {
                if (HasErrors) return false;
                if (NeedsPeaks) return true;
                else if (NeedsSolang) return true;
                else return false;
            }

            public bool NeedsSolang
            {
                get
                {
                    return (GetPeaksInNeedOf(false, false, this).Count() != 0);
                }
            }

            public bool NeedsPeaks
            {
                get
                {
                    return (this.GetPeaksRows().Count() == 0);
                }
            }

            public void ExtractData(ref SpecPrefRow d)
            {

             //   int fullLenght = 0; // d.DetectorLength + d.PositionLength + d.MeasLength + d.ProjectLength;

                if (d.DetectorIdx >= 0)
                {
                 //   fullLenght += d.DetectorLength;
                    Detector = Measurement.Substring(d.DetectorIdx, d.DetectorLength);
                }
                if (d.PositionIdx >= 0)
                {
                 //   fullLenght += d.PositionLength;
                    string posString = Measurement.Substring(d.PositionIdx, d.PositionLength);
                    if (char.IsDigit(posString[0]))
                    {
                        Position = Convert.ToInt16(posString);
                    }
                    else Position = Convert.ToInt16(Convert.ToInt16(posString) - 49);
                }
                if (d.MeasIdx >= 0)
                {
                  //  fullLenght += d.MeasLength;
                    MeasurementNr = Measurement.Substring(d.MeasIdx, d.MeasLength);
                }
                if (d.ProjectIdx >= 0)
                {
               //     fullLenght += d.ProjectLength;
                  //  Project = Measurement.Substring(d.ProjectIdx, d.ProjectLength);
                }


                if (d.SampleIdx >= 0 )
                {

                    Sample = Measurement.Substring(d.SampleIdx, d.SampleLength);
                }
               // else if (d.MonitorIdx >= 0 && fullLenght + d.MonitorLength == Measurement.Length)
              //  {
                  //  Sample = Measurement.Substring(d.MonitorIdx, d.MonitorLength);
              //  }
              
            }
        }
    }
}