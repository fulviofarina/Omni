using DB.LINAATableAdapters;
using Rsx.Dumb;
using Rsx.Math;
using System;
using System.Collections.Generic;
using System.Linq;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IMeasurements
    {
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

        public MeasurementsDataTable PopulateMeasurementsHyperLab(string project, bool merge)
        {
            MeasurementsTableAdapter mta = new MeasurementsTableAdapter();
            dynamic ta = mta;
            ChangeConnection(ref ta, true);
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

                if (meas.Count() != 0)
                {
                    Measurements.Merge(meas);
                 //   Measurements.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }

            mta.Dispose();
            return meas;
        }

        public void PopulatePeaksHL(int? id)
        {
            try
            {
            
                SpecPrefRow r = this.tableMeasurements.SpecPrefRow as SpecPrefRow;
                PeaksHLDataTable peakshl = PopulatePeaksHL(id, r.minArea, r.maxUnc);

                if (peakshl.Count != 0)
                {
                    PeaksHL.Merge(peakshl);
                    PeaksHL.AcceptChanges();
                }
            }
            catch (System.Exception ex)
            {
                AddException(ex);
            }
        }

        public PeaksHLDataTable PopulatePeaksHL(int? id, double minArea, double maxUnc)
        {
            PeaksHLTableAdapter pta = new PeaksHLTableAdapter();
            dynamic ta = pta;
            ChangeConnection(ref ta, true);
            PeaksHLDataTable phl = new PeaksHLDataTable();
            pta.FillByMeasurementID(phl, (int)id);
            pta.Dispose();
            pta = null;

            return phl;
        }

        
        public void CheckMeasurements(ref IEnumerable<MeasurementsRow> measurements)
        {
            if (measurements.Count() == 0) return;
            foreach (MeasurementsRow item in measurements)
            {
                try
                {
                    item.Check();
                    //item.AcceptChanges();
                }
                catch (Exception ex)
                {
                    this.AddException(ex);
                }
            }
        }

       
        /*
public void CheckMeasurements()
{
   IEnumerable<MeasurementsRow> meas = this.tableMeasurements;
   CheckMeasurements(ref meas);
}
*/
    }
}