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

        

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                    /*
                    return new DataColumn[] {
                            this.windowAColumn,
                            this.windowBColumn,
                            this.minAreaColumn,
                             maxUncColumn

                             };
                             */
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

                if (Column == tableSpecPref.windowAColumn)
                {
                    if (nulo) windowA = 1.5;
                }
                else if (Column == tableSpecPref.windowBColumn)
                {
                    if (nulo) windowB = 0.001;
                }
                else if (Column == tableSpecPref.minAreaColumn)
                {
                    if (nulo) minArea = 200;
                }
                else if (Column == tableSpecPref.maxUncColumn)
                {
                    if (nulo) maxUnc = 50;
                }
                else if (Column == tableSpecPref.ModelMonitorColumn)
                {
                    if (nulo)
                    {
                        ModelMonitor = "PPPPMMMMMDp#";
                    }
                }
                else if (Column == tableSpecPref.ModelSampleColumn)
                {
                    if (nulo)
                    {
                        ModelSample = "PPPPSSDp#";
                        SetIdxLength(false);
                    }
                }
                else if (Column == tableSpecPref.TimeDividerColumn)
                {
                    if (nulo) TimeDivider = "m";
                }
            }

            public void SetIdxLength(bool monitor = false)
            {
              

                    string model = ModelSample;
                    if (monitor) model = ModelMonitor;

                
                    char s = 'S';
                    if (monitor)
                    {
                        s = 'M';
                    }
                    SampleIdx = Convert.ToInt16(model.IndexOf(s));
                    SampleLength = Convert.ToInt16(model.LastIndexOf(s) - SampleIdx);
                    SampleLength++;


                ProjectIdx = Convert.ToInt16(model.IndexOf('P'));
                ProjectLength = Convert.ToInt16(model.LastIndexOf('P') - ProjectIdx);
                ProjectLength++;


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