using System;
using System.Data;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class GeometryRow : IRow
        {
            public new bool HasErrors()
            {
                return base.HasErrors;
            }
            public void Check()
            {
                foreach (DataColumn column in this.tableGeometry.Columns)
                {
                    Check(column);
                }
                // return this.GetColumnsInError().Count() != 0;
            }

            public void Check(DataColumn col)
            {
                bool nu = EC.CheckNull(col, this);

                if (nu && col == this.tableGeometry.FillHeightColumn)
                {
                    if (EC.IsNuDelDetch(VialTypeRow)) return;
                    VialTypeRow v = VialTypeRow;
                    if (v.IsMaxFillHeightNull()) return;
                    if (v.MaxFillHeight == 0) return;
                    double height = v.MaxFillHeight;
                    FillHeight = height;
                }
                else if (nu && col == this.tableGeometry.RadiusColumn)
                {
                    if (EC.IsNuDelDetch(VialTypeRow)) return;
                    VialTypeRow v = VialTypeRow;
                    if (v.IsInnerRadiusNull()) return;
                    if (v.InnerRadius == 0) return;
                    double rad = v.InnerRadius;
                    Radius = rad;
                }
            }

            /// <summary>
            /// Initializes the GeometryRow members
            /// </summary>
            /// <returns>Vial, Efficiencies and COIs, Solid Angles, DetectorCurves</returns>
            public void Gather(Int16 AtPosition, String detector)
            {
                if (detector.CompareTo(String.Empty) != 0)
                {
                    LINAA linaa = this.tableGeometry.DataSet as LINAA;

                    this.Position = AtPosition;
                    this.Detector = detector;
                    /*
          if (this.Position == 0) linaa.TAM.SolangTableAdapter.FillByGeometryDetector(linaa.Solang, this.GeometryName, this.Detector);
          else linaa.TAM.SolangTableAdapter.FillByGeoDetPos(linaa.Solang, this.GeometryName, this.Detector,this.Position);

            if (this.Position == 0) linaa.TAM.COINTableAdapter.FillByDetectorGeometry(linaa.COIN, this.Detector, this.GeometryName);
          else linaa.TAM.COINTableAdapter.FillByDetectorGeometryPosition(linaa.COIN, this.Detector, this.GeometryName,this.Position);
                      */
                }
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }
        }
    }
}