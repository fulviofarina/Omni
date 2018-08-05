using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        public partial class SpecPrefDataTable : IColumn
        {

            private DataColumn[] peakCols = null;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

          

            public IEnumerable<DataColumn> PeakCols
            {
                get
                {
                    if (peakCols == null)
                    {
                        peakCols = new DataColumn[] {
                            this.windowAColumn,
                            this.windowBColumn,
                            this.minAreaColumn,
                             maxUncColumn,
                         //    MeasIdxColumn,PositionIdxColumn,DetectorIdxColumn, SampleIdxColumn,ProjectIdxColumn,
                         //    this.ProjectLengthColumn,MeasLengthColumn,PositionLengthColumn,SampleLengthColumn,DetectorLengthColumn,
                             this.ModelSampleColumn,this.ModelMonitorColumn
                             };
                    }
                    return peakCols;
                }
            }
        }

        partial class SpecPrefRow : IRow
        {
           

            public void Check()
            {

                foreach (DataColumn item in this.tableSpecPref.Columns)
                {
                    Check(item);
                }
            }

            public void Check(DataColumn Column)
            {
                bool nulo = EC.CheckNull(Column, this);

                if (tableSpecPref.PeakCols.Contains(Column))
                {
                    if (IswindowANull()) windowA = 1.5;
                    if (IswindowBNull()) windowB = 0.001;
                    if (IsminAreaNull()) minArea = 500;
                    if (IsmaxUncNull()) maxUnc = 50;

                    if (IsModelSampleNull()) ModelSample = "PPPPSSDp#";
                    if (IsModelMonitorNull()) ModelMonitor = ModelSample.Replace('S','M');

                //    SetIdxLength();
                    /*
                    if (IsDetectorIdxNull()) DetectorIdx = 3;
                    if (IsPositionIdxNull()) PositionIdx = 2;
                    if (IsMeasIdxNull()) MeasIdx = 1;
                    if (IsProjectIdxNull()) ProjectIdx = 0;
                    if (IsSampleIdxNull()) SampleIdx = 1;

                    if (IsDetectorLengthNull()) DetectorLength = 1;
                    if (IsPositionLengthNull()) PositionLength = 1;
                    if (IsMeasLengthNull()) MeasLength = 1;
                    if (IsProjectLengthNull()) ProjectLength = 4;
                    if (IsSampleLengthNull()) SampleLength = 9;
                    */
                }
            }

            public void SetIdxLength(bool monitor = false)
            {
                try
                {

                    string model = ModelSample;
                    if (monitor) model = ModelMonitor;

                    ProjectIdx = Convert.ToInt16(model.IndexOf('P'));
                    ProjectLength = Convert.ToInt16(model.LastIndexOf('P') - ProjectIdx);
                    ProjectLength++;

                    char s = 'S';
                    if (monitor)
                    {
                        s = 'M';
                    }
                    SampleIdx = Convert.ToInt16(model.IndexOf(s));
                    SampleLength = Convert.ToInt16(model.LastIndexOf(s) - SampleIdx);
                    SampleLength++;

                    PositionIdx = Convert.ToInt16(model.IndexOf('p'));
                    PositionLength = Convert.ToInt16(model.LastIndexOf('p') - PositionIdx);
                    PositionLength++;
                    MeasIdx = Convert.ToInt16(model.IndexOf('#'));
                    MeasLength = Convert.ToInt16(model.LastIndexOf('#') - MeasIdx);
                    MeasLength++;
                    DetectorIdx = Convert.ToInt16(model.IndexOf('D'));
                    DetectorLength = Convert.ToInt16(model.LastIndexOf('D') - DetectorIdx);
                    DetectorLength++;
                }

                catch (Exception ex)
                {


                }


            }

            public new bool HasErrors()
            {
                return base.HasErrors;
            }

           
            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                // throw new NotImplementedException();
            }

            
        }

      
    }
}