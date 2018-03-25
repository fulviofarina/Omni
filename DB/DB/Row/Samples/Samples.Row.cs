using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        /// <summary>
        /// NOT CLEANED
        /// </summary>
        public partial class SubSamplesRow
        {
            public bool Selected
            {
                get
                {
                    return selected;
                }
                set { selected = value; }
            }

            public object Tag
            {
                get { return tag; }
                set { tag = value; }
            }

            protected internal bool selected;

            protected internal object tag;

            public bool SetOverride(string alpha, string efe, string Geo, string Gt, bool asSamples)
            {
                bool success = false;
                try
                {
                    if (asSamples) this.Comparator = false;
                    if (!efe.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.f = Convert.ToDouble(efe);
                    }
                    if (!alpha.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.Alpha = Convert.ToDouble(alpha);
                    }
                    if (!Gt.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.Gthermal = Convert.ToDouble(Gt);
                    }
                    if (!Geo.ToUpper().Contains(Properties.Misc.Def))
                    {
                        this.GeometryName = Geo;
                    }
                    success = true;
                }
                catch (System.InvalidCastException)
                {
                }

                return success;
            }

            public void SetDetectorPosition(string det, string pos)
            {
                try
                {
                    SetColumnError(this.tableSubSamples.DetectorColumn, null);
                    Detector = det;
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableSubSamples.DetectorColumn, ex.Message);
                }

                try
                {
                    SetColumnError(this.tableSubSamples.PositionColumn, null);

                    Position = Convert.ToInt16(pos);
                }
                catch (SystemException ex)
                {
                    SetColumnError(this.tableSubSamples.PositionColumn, ex.Message);
                }
            }

            public bool ShouldSelectIt(bool CheckForSSF)
            {
                if (this.NeedsMeasurements) return false;
                IEnumerable<LINAA.MeasurementsRow> measurements = GetMeasurementsRows();
                if (base.HasErrors)
                {
                    foreach (LINAA.MeasurementsRow m in measurements) m.Selected = false;
                    return false;
                }
                else
                {
                    foreach (LINAA.MeasurementsRow m in measurements) m.Selected = m.ShouldSelectIt();
                    measurements = LINAA.FindSelected(measurements).ToList();
                    if (measurements.Count() != 0) return true;
                    else
                    {
                        if (CheckForSSF)
                        {
                            if (NeedsSSF) return true;
                            else return false;
                        }
                        else return false;
                    }
                }
            }
        }
    }
}