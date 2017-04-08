using System;
using System.Linq;
using Rsx;
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

        public partial class MeasurementsDataTable
        {
            public LINAA.MeasurementsRow FindByMeasurementName(string measName, bool addIfnull)
            {
                LINAA.MeasurementsRow meas = null;
                Func<MeasurementsRow, bool> currSel = null;
                currSel = LINAA.SelectorByField<MeasurementsRow>(measName, this.MeasurementColumn.ColumnName);
                meas = this.FirstOrDefault(currSel);
                if (Rsx.EC.IsNuDelDetch(meas) && addIfnull) meas = this.AddMeasurementsRow(measName);

                return meas;
            }

            private LINAA.MeasurementsRow AddMeasurementsRow(string measName)
            {
                LINAA.MeasurementsRow meas = this.NewMeasurementsRow();
                this.AddMeasurementsRow(meas);

                try
                {
                    meas.Measurement = measName;
                    meas.Sample = measName.Substring(0, measName.Length - 3);
                    string DPN = measName.Replace(meas.Sample, null).Trim();
                    meas.MeasurementNr = DPN.Substring(2, 1);
                    meas.Position = Convert.ToInt16(DPN.Substring(1, 1));
                    meas.Detector = DPN.Substring(0, 1);
                    meas.Project = string.Empty;
                    if (!Rsx.EC.IsNuDelDetch(meas.SubSamplesRow))
                    {
                        if (!meas.SubSamplesRow.IsIrradiationCodeNull()) meas.Project = meas.SubSamplesRow.IrradiationCode;
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(meas, ex);
                }
                return meas;
            }
        }
    }
}