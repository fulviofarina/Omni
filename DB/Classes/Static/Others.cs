using System;
using Rsx.CAM;

namespace DB
{
    public partial class LINAA
    {
        public static void SetROIInfo(ref PeaksRow p, ref DetectorX ireader, double minUnc, double bkgCh, double energylow, double energyhigh)
        {
            try
            {
                p.Ready = ireader.GetROI(energylow, energyhigh, bkgCh, minUnc);
                p.Area = ireader.Area;
                p.AreaUncertainty = ireader.AreaUnc;

                if (p.Area != 0)
                {
                    double eta = ((100 * 100) / (minUnc * minUnc));
                    p.ETAInMin = Convert.ToInt32(eta / ((p.Area / (ireader.CountTime / 60))));
                    p.ETA = Rsx.Dumb.ToReadableString(new TimeSpan(0, 0, p.ETAInMin, 0));
                }
            }
            catch (SystemException ex)
            {
                Rsx.Dumb.SetRowError(p, ex);
            }
        }

        private static void InstallResources()
        {
            string resourcePath = Properties.Settings.Default.deployPath + Properties.Resources.ResourcesPath;
            if (System.IO.File.Exists(resourcePath + "setup.exe"))
            {
                Rsx.Dumb.Process(new System.Diagnostics.Process(), resourcePath, resourcePath + "setup.exe", string.Empty, false, true, 100000);
            }
        }
    }
}