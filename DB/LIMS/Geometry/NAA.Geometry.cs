using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class GeometryDataTable
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                   // IEnumerable<DataColumn> nonNullables;

                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[]
                        {
                            this.columnFillHeight,
                            this.columnRadius,
                            this.columnGeometryName,
                            this.columnMatrixDensity,
                            this.columnMatrixName,
                            this.columnCreationDateTime,
                            this.columnVialTypeRef
                        };
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                DataColumn col = e.Column;

                if (!NonNullables.Contains(col)) return;

              //  LINAA linaa = this.DataSet as LINAA;
                LINAA.GeometryRow g = e.Row as LINAA.GeometryRow;

                try
                {
                      
                        bool nu = EC.CheckNull(col, e.Row);
                        if (col == this.columnGeometryName && nu)
                        {
                            g.GeometryName = "No Name";
                        }
                        else if (nu && col == this.columnFillHeight || col == this.columnRadius)
                        {
                            VialTypeRow v = g.VialTypeRow;
                            if (v == null) return;
                            if (col == this.columnFillHeight)
                            {
                                if (v.IsMaxFillHeightNull()) return;
                                if (v.MaxFillHeight == 0) return;
                                g.FillHeight = v.MaxFillHeight;
                            }
                            else
                            {
                                if (!EC.CheckNull(col, e.Row)) return;
                                if (v.IsInnerRadiusNull()) return;
                                if (v.InnerRadius == 0) return;
                                g.Radius = v.InnerRadius;
                            }
                        }
                        return;
                    
                }
                catch (SystemException ex)
                {
                    LINAA linaa = this.DataSet as LINAA;
                    EC.SetRowError(e.Row, col, ex);
                    linaa.AddException(ex);
                }
            }
        }

        partial class GeometryRow
        {
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
        }
    }
}