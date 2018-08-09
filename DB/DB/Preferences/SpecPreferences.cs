using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB
{
    public partial class LINAA
    {
        public partial class SpecPrefDataTable : IColumn
        {
            private IEnumerable<DataColumn> forbiddenNullCols;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if
                        (forbiddenNullCols == null)
                    {
                        forbiddenNullCols = new DataColumn[] {
                            windowAColumn,
                            windowBColumn,
                            minAreaColumn,
                             maxUncColumn,
                        ModelMonitorColumn,
                        TimeDividerColumn,
                        ModelSampleColumn, RoundingColumn };
                    }
                    return forbiddenNullCols;
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
                // if (!this.tableSpecPref.ForbiddenNullCols.Contains(Column)) return;

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
                    SetLabellingParameters(true);
                }
                else if (Column == tableSpecPref.ModelSampleColumn)
                {
                    if (nulo)
                    {
                        ModelSample = "PPPPSSDp#";
                        // SetLabellingParameters(false);
                    }
                    SetLabellingParameters(false);
                }
                else if (Column == tableSpecPref.TimeDividerColumn)
                {
                    if (nulo) TimeDivider = "m";

                    //the user selected a good time divider (s, m, y,d,h)
                    bool ok = Caster.IsTimeDividerOk(TimeDivider);
                    if (!ok) TimeDivider = "m";
                }
                else if (Column == tableSpecPref.RoundingColumn)
                {
                    if (nulo) Rounding = "N3";

                 
                }
            }

            public new bool HasErrors()
            {
                return base.HasErrors;
            }

            public void SetLabellingParameters(bool monitor = false)
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
        }
    }
}