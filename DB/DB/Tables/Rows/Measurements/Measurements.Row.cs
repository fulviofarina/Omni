using Rsx.CAM;
using System;
using System.Data;
using System.Linq;
using Rsx.Dumb;
using Rsx.Math;

namespace DB
{
    public partial class LINAA
    {
        public partial class MeasurementsRow : IRow
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
            /*
            public void SetName(string measName)
            {
                Measurement = measName;

                Sample = measName.Substring(0, measName.Length - 3);
                string DPN = measName.Replace(Sample, null).Trim();
                MeasurementNr = DPN.Substring(2, 1);
                Position = Convert.ToInt16(DPN.Substring(1, 1));
                Detector = DPN.Substring(0, 1);
                Project = string.Empty;
            }
            */

            public bool ShouldSelectIt()
            {
                if (HasErrors()) return false;
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

            internal void SetName(string measName)
            {
                if (!string.IsNullOrEmpty(measName))
                {
                    Measurement = measName;
                }
            }

            private void setLabels()
            {
                SpecPrefRow d = this.tableMeasurements.SpecPrefRow;
                if (d == null) return;

                SetDetectorNull();
                SetPositionNull();
                SetMeasurementNrNull();
                SetSampleNull();

                bool isMonitor = !(Measurement.Length == d.ModelSample.Length);
                d.SetLabellingParameters(isMonitor);
           

                if (d.DetectorIdx >= 0)
                {
                    Detector = Measurement.Substring(d.DetectorIdx, d.DetectorLength);
                }
            
                if (d.PositionIdx >= 0)
                {
                    string posString = Measurement.Substring(d.PositionIdx, d.PositionLength);
                    if (char.IsDigit(posString[0]))
                    {
                        Position = Convert.ToInt16(posString);
                    }
                    else Position = Convert.ToInt16(Convert.ToInt16(posString) - 49);
                }
              
                if (d.MeasIdx >= 0)
                {
                    MeasurementNr = Measurement.Substring(d.MeasIdx, d.MeasLength);
                }

                if (d.ProjectIdx >= 0)
                {
                    // Project = Measurement.Substring(d.ProjectIdx, d.ProjectLength);
                }
               
                if (d.SampleIdx >= 0)
                {
                    Sample = Measurement.Substring(d.SampleIdx, d.SampleLength);
                }
            }

            private void checkTimes()
            {
                if (countTimeRaw == null) return;
                SpecPrefRow r = this.tableMeasurements.SpecPrefRow;
                if (r == null) return;
             
                double factor = MyMath.GetTimeFactor(r.TimeDivider);
          
                    double ct = (double)this.countTimeRaw / factor;
                    if (ct != CountTime) CountTime = ct;
                   
                    double lt = (double)this.liveTimeRaw / factor;
                    if (lt != LiveTime) LiveTime = lt;

              
            }

            public void Check()
            {
                this.ClearErrors();

                foreach (DataColumn item in this.tableMeasurements.Columns)
                {
                    Check(item);
                }
            }

           private double? countTimeRaw = null;
            private double? liveTimeRaw = null;

            public new bool HasErrors()
            {
                return base.HasErrors;
            }

            public void Check(DataColumn Column)
            {
                //   if (!this.tableMeasurements.ForbiddenNullCols.Contains(Column)) return;

             
                if (Column == this.tableMeasurements.MeasurementColumn)
                {
                  
                        setLabels();
                   
                }
                else if (Column == this.tableMeasurements.CountTimeColumn || (Column == this.tableMeasurements.LiveTimeColumn))
                {
                    if (countTimeRaw == null)
                    {
                        countTimeRaw = CountTime;
                        liveTimeRaw = LiveTime;
                    }
                    else checkTimes();
                  
                }
           
            }
        }
    }
}