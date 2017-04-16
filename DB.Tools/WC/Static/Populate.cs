using System;
using System.Collections.Generic;
using System.Linq;

namespace DB.Tools
{
    public partial class WC
    {
        /// <summary>
        /// Finds all spectra related to directory = curentPref.Spectra + project and adds them to
        /// the SubSamples table
        /// </summary>
        /// <param name="project"></param>
        public static ICollection<string> FindSpecSamples(string project, string spectPath)
        {
            HashSet<string> hsamples = new HashSet<string>();

            project = project.ToUpper();

            string directory = spectPath + project;
            string[] arr = null;
            if (System.IO.Directory.Exists(directory))
            {
                // Msg("Searching...", "Searching samples from the Spectra Directory", true);
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directory);
                System.IO.FileInfo[] files = dir.GetFiles("*.cnf", System.IO.SearchOption.AllDirectories);
                Func<System.IO.FileInfo, string> selec = fi =>
                {
                    return fi.Name.ToUpper();
                };
                //deference the function
                arr = files.Select(selec).ToArray();
                files = null;
                Func<string, bool> trimmer = aux =>
                {
                    int ind = aux.IndexOf('.');
                    if (ind > 0) aux = aux.Substring(0, ind).Trim();
                    ind = aux.IndexOf(' ');
                    if (ind > 0) aux = aux.Substring(0, ind).Trim();
                    if (aux.Length > 3)
                    {
                        return hsamples.Add(aux.Substring(0, aux.Length - 3));
                    }
                    return false;
                };
                arr = arr.Where(trimmer).ToArray();
                arr = null;
            }

            return hsamples;
        }

        public static ICollection<string> FindHLSamples(string project)
        {
            string[] arr = null;

            LINAATableAdapters.MeasurementsTableAdapter ta = new LINAATableAdapters.MeasurementsTableAdapter();
            LINAA.MeasurementsDataTable meas = new LINAA.MeasurementsDataTable(false);
            meas.ProjectColumn.Expression = string.Empty;
            meas.ProjectColumn.ReadOnly = true;
            meas.BeginLoadData();
            ta.SetForHL();
            int? proID = ta.GetHLProjectID(project);
            //maybe has sub folders...
            if (proID != null)
            {
                ta.FillByHLProject(meas, (int)proID);

                arr = Rsx.Dumb.HashFrom<string>(meas.SampleColumn).ToArray();
            }

            meas.EndLoadData();

            Rsx.Dumb.FD(ref meas);
            Rsx.Dumb.FD(ref ta);
            return arr;
        }

        public static bool IsHyperLabOK
        {
            get
            {
                bool ok = false;
                LINAATableAdapters.QTA qta = new LINAATableAdapters.QTA();
                try
                {
                    qta.GetMeasurementID("1");
                    ok = true;
                }
                catch (SystemException)
                {
                    //AddException(ex);
                }
                qta.Dispose();
                qta = null;
                return ok;
            }
        }

        #region Nuclear Databases

        public static LINAA.NAADataTable PopulateNAA(bool official)
        {
            LINAA.NAADataTable dt = new LINAA.NAADataTable(false);
            LINAATableAdapters.NAATableAdapter ta = new LINAATableAdapters.NAATableAdapter();

            dt.BeginLoadData();
            dt.Clear();
            if (official) ta.Fill(dt);
            else ta.FillUnOff(dt);
            dt.AcceptChanges();
            dt.EndLoadData();

            ta.Dispose();
            ta = null;

            return dt;
        }

        public static LINAA.k0NAADataTable Populatek0NAA(bool official)
        {
            LINAA.k0NAADataTable dt = new LINAA.k0NAADataTable();
            LINAATableAdapters.k0NAATableAdapter ta = new LINAATableAdapters.k0NAATableAdapter();

            dt.BeginLoadData();
            dt.Clear();
            if (official) ta.Fill(dt);
            else ta.FillUnOff(dt);
            dt.AcceptChanges();
            dt.EndLoadData();

            ta.Dispose();
            ta = null;

            return dt;
        }

        #endregion Nuclear Databases

        #region Local

        public static LINAA.PeaksDataTable PopulatePeaks(int? IrrId, string Sample)
        {
            LINAA.PeaksDataTable newtable = new LINAA.PeaksDataTable(false);

            LINAATableAdapters.PeaksTableAdapter TA = new LINAATableAdapters.PeaksTableAdapter();
            newtable.BeginLoadData();

            TA.FillDataStored(newtable, IrrId, Sample);
            newtable.EndLoadData();
            TA.Dispose();
            TA = null;

            return newtable;
        }

        public static LINAA.IPeakAveragesDataTable PopulateIPeaksAverages(string Sample)
        {
            LINAATableAdapters.IPeakAveragesTableAdapter TA = new LINAATableAdapters.IPeakAveragesTableAdapter();

            LINAA.IPeakAveragesDataTable ipnew = new LINAA.IPeakAveragesDataTable(false);
            LINAA.IPeakAveragesDataTable ipnew2 = new LINAA.IPeakAveragesDataTable(false);
            ipnew.BeginLoadData();
            TA.FillDataNew(ipnew, Sample);
            ipnew2.BeginLoadData();
            TA.FillDataStored(ipnew2, Sample);
            ipnew2.EndLoadData();
            ipnew.Merge(ipnew2, false, System.Data.MissingSchemaAction.Ignore);
            ipnew.EndLoadData();

            ipnew2.Clear();
            ipnew2.Dispose();
            ipnew2 = null;

            TA.Dispose();
            TA = null;
            // if (merge) Dumb.MergeTable(ref ipnew);

            return ipnew;
        }

        public static LINAA.IRequestsAveragesDataTable PopulateIRequestsAverages(string Sample)
        {
            LINAATableAdapters.IRequestsAveragesTableAdapter TA = new LINAATableAdapters.IRequestsAveragesTableAdapter();
            LINAA.IRequestsAveragesDataTable irnew = new LINAA.IRequestsAveragesDataTable(false);
            LINAA.IRequestsAveragesDataTable irnew2 = new LINAA.IRequestsAveragesDataTable(false);

            irnew.BeginLoadData();
            TA.FillDataNew(irnew, Sample);  //new ones based on Peaks table
            irnew2.BeginLoadData();
            TA.FillDataStored(irnew2, Sample);     //the ones that al already in the DB
            irnew2.EndLoadData();
            irnew.Merge(irnew2, false, System.Data.MissingSchemaAction.Ignore);
            irnew.EndLoadData();

            irnew2.Clear();
            irnew2.Dispose();
            irnew2 = null;

            TA.Dispose();
            TA = null;

            return irnew;
        }

        #endregion Local

        public static LINAA.MeasurementsDataTable PopulateMeasurements(Int32? IrrReqId)
        {
            LINAA.MeasurementsDataTable meas = new LINAA.MeasurementsDataTable(false);
            LINAATableAdapters.MeasurementsTableAdapter ta = new LINAATableAdapters.MeasurementsTableAdapter();
            meas.BeginLoadData();
            ta.SetForLIMS();
            ta.DeleteNegatives();
            ta.FillByIrrReqId(meas, IrrReqId);
            meas.EndLoadData();
            ta.Dispose();
            ta = null;

            return meas;
        }

        public static LINAA.MatSSFDataTable PopulateMatSSF(int? IrReq)
        {
            // this.tableMatSSF.BeginLoadData();

            LINAA.MatSSFDataTable ssf = new LINAA.MatSSFDataTable(false);
            LINAATableAdapters.MatSSFTableAdapter ta = new LINAATableAdapters.MatSSFTableAdapter();
            ssf.Constraints.Clear();
            ta.FillByIrReqId(ssf, IrReq);
            HashSet<string> key = new HashSet<string>();
            IEnumerable<LINAA.MatSSFRow> rows = ssf.OrderByDescending(m => m.MatSSFID).Where(m => !key.Add(m.SubSamplesID + "," + m.TargetIsotope));
            rows = rows.ToList();
            foreach (LINAA.MatSSFRow m in rows)
            {
                ta.DeleteBy(m.MatSSFID);
            }
            ssf.Clear();

            ta.FillByIrReqId(ssf, IrReq);

            return ssf;
        }

        private static void ContingencySetCOINSolid(bool coin, ref LINAA.GeometryRow reference, ref IEnumerable<LINAA.MeasurementsRow> measforContingency)
        {
            foreach (LINAA.MeasurementsRow m in measforContingency)
            {
                if (coin) ContingencySetCOI(m);
                else ContingencySetEfficiency(m, ref reference, 5);
            }
            measforContingency = null;
        }

        #region Transfer/Filter/Populate Peaks

        public static bool TransferPeaksHL(ref LINAA.MeasurementsRow m, Int32 sampleID, Int32 irReqID, ref LINAA.PeaksHLDataTable peakshl)
        {
            LINAATableAdapters.PeaksTableAdapter peaksTa = new LINAATableAdapters.PeaksTableAdapter();
            bool success = false;
            try
            {
                peaksTa.DeleteByMeasurementID(m.MeasurementID);

                int i = 1;
                foreach (LINAA.PeaksHLRow ph in peakshl)
                {
                    peaksTa.Insert(m.MeasurementID, sampleID, irReqID, ph.Area, ph.AreaUncertainty, ph.Energy, ph.PeaksID, 1, 1, -i, 1, 0);
                    i++;
                }
                peakshl.Clear();
                peakshl.Dispose();
                peakshl = null;

                success = true;
            }
            catch (SystemException ex)
            {
                m.RowError = "TransferPeaks Module Error: " + ex.Message + "\n\n" + ex.StackTrace;
                // this.Linaa.AddException(ex);
            }
            peaksTa.Dispose();
            peaksTa = null;
            return success;
        }

        public static LINAA.PeaksHLDataTable PopulatePeaksHL(ref LINAA.MeasurementsRow m, double minArea, double maxUnc)
        {
            LINAA.PeaksHLDataTable peakshl = null;

            LINAATableAdapters.PeaksHLTableAdapter peaksHlta = null;
            try
            {
                peaksHlta = new LINAATableAdapters.PeaksHLTableAdapter();
                peakshl = peaksHlta.GetDataByMeasurementID(m.MeasurementID, minArea, maxUnc);
                if (peakshl.Rows.Count == 0)
                {
                    m.RowError = "PopulatePeaksHL Module Error: No peaks were found in HyperLab for this measurement.\n";
                    m.RowError += "This means that I did not find a Spectrum Deconvolution associated to it\n";
                }
            }
            catch (SystemException ex)
            {
                m.RowError = "PopulatePeaksHL Module Error: " + ex.Message + "\n\n" + ex.StackTrace;
                // this.Linaa.AddException(ex);
            }
            if (peaksHlta != null)
            {
                peaksHlta.Dispose();
                peaksHlta = null;
            }
            return peakshl;
        }

        protected static void FilterPeaks(ref LINAA.MeasurementsRow m, double windowA, double windowB, double minArea, double maxUnc, ref IList<string> elementsIfAny)
        {
            LINAATableAdapters.PeaksTableAdapter peaksTa = null;
            LINAA.PeaksDataTable newpeaks = null;

            try
            {
                peaksTa = new LINAATableAdapters.PeaksTableAdapter();
                newpeaks = new LINAA.PeaksDataTable(false);
                peaksTa.FillDatak0NAA(newpeaks, windowA, windowB, minArea, maxUnc, m.MeasurementID); //get new peaks
                if (newpeaks.Rows.Count == 0)
                {
                    m.RowError += "LoadPeaks Module Error: No peaks were found for the given Peak Search options (filters) or\nNo peaks were associated with the k0-NAA Library\n";
                    goto Finish;
                }

                IEnumerable<LINAA.PeaksRow> oldpeaks = m.GetPeaksRows().ToList();
                foreach (LINAA.PeaksRow op in oldpeaks)
                {
                    // int positiveID = Math.Abs(op.ID);
                    LINAA.PeaksRow n = newpeaks.FirstOrDefault(p => Math.Abs(p.ID) == Math.Abs(op.ID));
                    if (n == null) continue;
                    n.ID = op.ID;
                    if (!op.IsMDNull()) n.MD = op.MD;
                    // if (!op.IsEfficiencyNull()) n.Efficiency = op.Efficiency;
                    //if (!op.IsCOINull()) n.COI = op.COI;
                }
                oldpeaks = null;

                peaksTa.DeleteByMeasurementID(m.MeasurementID);

                //filter elements
                if (elementsIfAny != null)
                {
                    foreach (string element in elementsIfAny)
                    {
                        IEnumerable<LINAA.PeaksRow> filtered = newpeaks.Where(p => p.Sym.ToUpper().CompareTo(element.ToUpper()) == 0);
                        foreach (LINAA.PeaksRow p in filtered)
                        {
                            peaksTa.Insert(p.MeasurementID, p.SampleID, p.IrradiationID, p.Area, p.AreaUncertainty, p.Energy, p.PeaksID, p.Efficiency, p.COI, p.ID, p.T0, p.MD);
                        }
                    }
                }
                else
                {
                    foreach (LINAA.PeaksRow p in newpeaks)
                    {
                        peaksTa.Insert(p.MeasurementID, p.SampleID, p.IrradiationID, p.Area, p.AreaUncertainty, p.Energy, p.PeaksID, p.Efficiency, p.COI, p.ID, p.T0, p.MD);
                    }
                }
            }
            catch (SystemException ex)
            {
                m.RowError = "FilterPeaks Module Error: " + ex.Message + "\n\n" + ex.StackTrace;
                // this.Linaa.AddException(ex);
            }

            Finish:

            if (peaksTa != null)
            {
                peaksTa.Dispose();
                peaksTa = null;
            }
            if (newpeaks != null)
            {
                newpeaks.Clear();
                newpeaks.Dispose();
                newpeaks = null;
            }
        }

        #endregion Transfer/Filter/Populate Peaks
    }
}