using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {

   


    

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