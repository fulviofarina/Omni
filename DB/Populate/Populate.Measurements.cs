using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb;
using Rsx.Math;

//using DB.Interfaces;

namespace DB
{
  

    public partial class LINAA : IMeasurements
    {
     
        public void PopulatePeaksHL(int? id)
        {
            try
            {
                EventData d = new EventData();
                this.SubSamples.CalcParametersHandler?.Invoke(null, d);
                SpecPrefRow r = d.Args[3] as SpecPrefRow;
                //      double minArea = r.minArea;
                //    double maxUnc = (double)d.Args[1];
                // double winA = (double)d.Args[2];
                // double winB = (double)d.Args[3];


                PeaksHLDataTable peakshl = PopulatePeaksHL(id, r.minArea, r.maxUnc);

                PeaksHL.Merge(peakshl);

            }
            catch (System.Exception ex)
            {
                AddException(ex);
            }
        }

        public PeaksHLDataTable PopulatePeaksHL(int? id, double minArea, double maxUnc)
        {
            LINAATableAdapters.PeaksHLTableAdapter pta = new LINAATableAdapters.PeaksHLTableAdapter();
            PeaksHLDataTable phl = new LINAA.PeaksHLDataTable();
            pta.FillByMeasurementID(phl, (int)id, minArea, maxUnc);
            pta.Dispose();

            return phl;
        }
        public MeasurementsDataTable PopulateMeasurementsGeneric(string project, bool merge)
        {
            LINAATableAdapters.MeasurementsTableAdapter mta = new LINAATableAdapters.MeasurementsTableAdapter();
            MeasurementsDataTable meas = new MeasurementsDataTable();
            meas.ProjectColumn.Expression = string.Empty;
            meas.ProjectColumn.ReadOnly = false;

            try
            {
                int? id = (int?)mta.GetHLProjectID(project);
                if (id != null)
                {

                    mta.FillByHLProjectLFC(meas, (int)id);
                    if (meas.Count == 0)
                    {
                        mta.FillByHLProjectNoLFC(meas, (int)id);
                    }

                 
                }

                IEnumerable<MeasurementsRow> measurements = meas;

                setMeasurementTimes(ref measurements);

                if (meas.Count() != 0)
                {
                    Measurements.Merge(meas);
                    Measurements.AcceptChanges();

                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }

            mta.Dispose();
            return meas;
        }
        public MeasurementsRow AddMeasurement(string measName)
        {
            MeasurementsRow meas = FindByMeas(measName);
            if (!EC.IsNuDelDetch(meas)) return meas;

            try
            {
                meas = this.tableMeasurements.NewMeasurementsRow();
                this.tableMeasurements.AddMeasurementsRow(meas);
                meas.SetName(measName);

            }
            catch (SystemException ex)
            {
                //EC.SetRowError(meas, ex);
                AddException(ex);
            }

            return meas;
        }

        public MeasurementsRow FindByMeas(string measName)
        {
            MeasurementsRow meas = null;
            Func<MeasurementsRow, bool> currSel = null;
            string col = this.tableMeasurements.MeasurementColumn.ColumnName;
            currSel = LINAA.SelectorByField<MeasurementsRow>(measName, col);
            meas = this.tableMeasurements.FirstOrDefault(currSel);
            return meas;
        }
        private void setMeasurementTimes(ref IEnumerable<MeasurementsRow> measurements)
        {
            if (measurements.Count() == 0) return;

            SpecPrefRow r = this.Measurements.SpecPrefRow;
            string timeDivider = r.TimeDivider;

            foreach (MeasurementsRow item in measurements)
            {
                try
                {
                    double factor = MyMath.GetTimeFactor(timeDivider);
                    if (factor > 1)
                    {
                        item.CountTime /= factor;
                        item.LiveTime /= factor;
                    }
                }
                catch (Exception ex)
                {
                    this.AddException(ex);
                }
            }
        }

    }
  
}