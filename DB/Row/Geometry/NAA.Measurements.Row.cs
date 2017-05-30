using System;
using System.Linq;
using Rsx.CAM;

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
                if (!Rsx.EC.IsNuDelDetch(SubSamplesRow))
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
        }
    }
}