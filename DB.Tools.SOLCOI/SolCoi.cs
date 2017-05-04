using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Rsx.Dumb; using Rsx;

namespace DB.Tools
{
    public partial class SolCoin : IDisposable
    {
        private Type[] DataTablesToPreserve = new Type[] {
                typeof(LINAA.DetectorsAbsorbersDataTable),
                typeof(LINAA.DetectorsDimensionsDataTable),
                typeof(LINAA.HoldersDataTable),
                typeof(LINAA.VialTypeDataTable),
                typeof(LINAA.GeometryDataTable),
                typeof(LINAA.MatrixDataTable),
                typeof(LINAA.SolangDataTable),
                typeof(LINAA.COINDataTable),
                typeof(LINAA.MUESDataTable),
                        typeof(LINAA.CompositionsDataTable),
                typeof(LINAA.DetectorsCurvesDataTable),
           typeof(LINAA.ExceptionsDataTable)};

        public static void KillHanged()
        {
            IList<System.Diagnostics.Process> processes = System.Diagnostics.Process.GetProcesses();
            IList<System.Diagnostics.Process> processes2;

            processes2 = processes.Where(p => p.ProcessName.ToUpper().Contains("SOLCO")).ToList();
            foreach (System.Diagnostics.Process p in processes2)
            {
                p.Kill();
                p.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        public bool EndIt()
        {
            bool ok = false;
            try
            {
                if (Exception != null)
                {
                    Set.AddException(Exception);
                }
                else if (SolangProcess != null)
                {
                    if (SolangProcess.ExitCode == 0) ok = Finished;
                }
                Dispose();
            }
            catch (SystemException ex)
            {
                Set.AddException(ex);
            }
            return ok;
        }

        public bool Gather(string geometryName, Int16 AtPosition, string _detectorName, string referenceGeometryName, Int16 referenceAtPosition)
        {
            this.linaa.Clear();
            this.linaa.EnforceConstraints = false;
            exception = null;

            bool ok = GatherMatComp();
            if (!ok) return false;

            ok = GatherDetectorData(_detectorName);
            if (!ok) return false;

            ok = GatherGeo(geometryName, AtPosition, ref geometry, _detectorName);
            if (!ok) return false;

            ok = GatherGeo(referenceGeometryName, referenceAtPosition, ref reference, _detectorName);
            if (!ok) return false;

            Files = new IOFiles();
            ok = Files.Gather(SolCoinPath, geometry.GeometryName, geometry.Position, _detectorName, reference.GeometryName, reference.Position);

            return ok;
        }

        public bool GatherGeo(string geo, short atpos, ref LINAA.GeometryRow geom, string setDet)
        {
            bool lo = true;

            if (geo == null || geo.Equals(string.Empty))
            {
                exception = new SystemException("Geometry Name is null or empty - Gather Method");
                return false;
            }

            bool succes = false;

            try
            {
                geom = this.Set.Geometry.Where(g => g.GeometryName.CompareTo(geo) == 0).FirstOrDefault();

                this.linaa.VialType.LoadDataRow(geom.VialTypeRow.ItemArray, lo);
                geom = this.linaa.Geometry.LoadDataRow(geom.ItemArray, lo) as LINAA.GeometryRow;
            }
            catch (SystemException ex)
            {
                exception = ex;
            }

            if (EC.IsNuDelDetch(geom))
            {
                exception = new SystemException("Gather of Geometry failed! - Gather Method", exception);
                return false;
            }
            LINAA.VialTypeRow vial = geom.VialTypeRow;

            if (EC.IsNuDelDetch(vial))
            {
                exception = new SystemException("Gather of Geometry Vial failed! - Gather Method", exception);
                return false;
            }

            geom.Gather(atpos, setDet);

            succes = !geom.HasErrors;

            if (!succes)
            {
                exception = new SystemException("The Geometry Row has Row Errors! - Gather Method", exception);
                return false;
            }

            return succes;
        }

        public bool GatherMatComp()
        {
            this.linaa.Compositions.Merge(this.Set.Compositions, false, MissingSchemaAction.Add);
            this.linaa.Compositions.AcceptChanges();

            if (linaa.Compositions.Rows.Count == 0)
            {
                exception = new SystemException("Gather of Compositions Failed! - Gather Method");
                return false;
            }
            this.linaa.Matrix.Merge(this.Set.Matrix, false, MissingSchemaAction.Add);
            this.linaa.Matrix.AcceptChanges();

            if (linaa.Matrix.Rows.Count == 0)
            {
                exception = new SystemException("Gather of Matrices Failed! - Gather Method");
                return false;
            }
            return true;
        }

        public bool PrepareSampleUnit(string[] PosGeoDetFillRad, double[] energies, bool calcSolid, bool calcCois)
        {
            Gather(PosGeoDetFillRad[1], Convert.ToInt16(PosGeoDetFillRad[0]), PosGeoDetFillRad[2], "REF", 5);
            if (Exception != null) return false;

            CalculateSolidAngles = calcSolid;
            CleanSolidAngles = calcSolid;
            StoreSolidAngles = calcSolid;

            CalculateCOIS = calcCois;
            StoreCOIS = calcCois;
            CleanEffiCOIS = calcCois;

            Energies = energies;

            //SPECIFIC GEOMETRY OR DIRECT SOLCOI!
            if (!Geometry.GeometryName.ToUpper().Contains("REF"))
            {
                Geometry.FillHeight = Convert.ToDouble(PosGeoDetFillRad[3]);
                Geometry.Radius = Convert.ToDouble(PosGeoDetFillRad[4]);
                Geometry.GeometryName += PosGeoDetFillRad[5];
                Files.Input = Files.Input.Replace(".SIN", PosGeoDetFillRad[5] + ".SIN");
                Files.SolidAngles = Files.SolidAngles.Replace(".SOL", PosGeoDetFillRad[5] + ".SOL");
                Files.COI = Files.COI.Replace(".COI", PosGeoDetFillRad[5] + ".COI");
                integrationMode = IntegrationModes.AsNonDisk;
            }
            else IntegrationMode = IntegrationModes.AsPointSource; //if reference, don't waste time, always AsPointSource
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these operations, as well as in your
            // methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    this.detectorAbsorber = null;
                    this.detectorDimension = null;
                    this.detectorHolder = null;
                    this.reference = null;
                    this.geometry = null;
                    this.files = null;
                    this.exception = null;
                    this.solcoinPath = null;

                    this.linaa.Clear();
                    foreach (DataTable t in linaa.Tables)
                    {
                        t.Clear();
                        t.Dispose();
                    }

                    this.linaa.DisposeSolCoinAdapters();
                    this.linaa.Dispose();
                    this.linaa = null;

                    if (solangProcess != null)
                    {
                        if (!solangProcess.HasExited)
                        {
                            solangProcess.Kill();
                            solangProcess.Close();
                        }
                        solangProcess.Dispose();
                        solangProcess = null;
                    }
                    if (cOINProcess != null)
                    {
                        if (!cOINProcess.HasExited)
                        {
                            cOINProcess.Kill();
                            cOINProcess.Close();
                        }
                        cOINProcess.Dispose();
                        cOINProcess = null;
                    }

                    this.energies = null;
                    this.Set = null;

                    if (this.tag != null)
                    {
                        IDisposable dis = this.tag as IDisposable;
                        dis.Dispose();
                        this.tag = null;
                    }
                }

                // Indicate that the instance has been disposed.
                _disposed = true;
            }
        }

        private bool GatherDetectorData(string det)
        {
            if (det == null || det.Equals(string.Empty))
            {
                exception = new SystemException("Detector Name is null or empty - Gather Method");
                return false;
            }
            detectorName = det;

            try
            {
                LoadOption lo = LoadOption.OverwriteChanges;

                detectorDimension = this.Set.DetectorsDimensions.Where(d => d.Detector.CompareTo(det) == 0).FirstOrDefault();
                this.linaa.DetectorsAbsorbers.LoadDataRow(detectorDimension.DetectorsAbsorbersRow.ItemArray, lo);

                this.linaa.Holders.Merge(detectorDimension.GetHoldersRows().CopyToDataTable(), false, MissingSchemaAction.Add);
                this.linaa.Holders.AcceptChanges();

                //finally
                DataRow aux = this.linaa.DetectorsDimensions.LoadDataRow(detectorDimension.ItemArray, lo);
                detectorDimension = aux as LINAA.DetectorsDimensionsRow;
            }
            catch
                  (SystemException ex)
            {
                exception = ex;
            }

            if (EC.IsNuDelDetch(detectorDimension))
            {
                exception = new SystemException("Gather of DetDimension failed! - Gather Method", exception);
                return false;
            }
            detectorAbsorber = detectorDimension.DetectorsAbsorbersRow;
            if (EC.IsNuDelDetch(detectorAbsorber))
            {
                exception = new SystemException("Gather of Detector Absorbers failed! - Gather Method", exception);
                return false;
            }

            detectorHolder = detectorDimension.GetHoldersRows();
            if (detectorHolder.Count() == 0)
            {
                exception = new SystemException("Gather of Detector Holder Failed! - Gather Method", exception);
                return false;
            }

            return true;
        }

        private void Initialize(ref LINAA set)
        {
            Set = set;
            linaa = Set.Clone() as LINAA;
            linaa.InitializeComponent();
            linaa.InitializeSolCoinAdapters();
            // Rsx.Dumb.Preserve(linaa, DataTablesToPreserve);
        }

        ~SolCoin()
        {
            Dispose(false);
        }

        public SolCoin()
        {
            LINAA s = new LINAA();
            s.InitializeComponent();
            IEnumerable<Action> populMethod = s.PMTwo();
            foreach (Action a in populMethod) a.Invoke();
            // Populate(ref s); Rsx.Dumb.Preserve(s, DataTablesToPreserve);
            Initialize(ref s);
        }

        public SolCoin(ref LINAA set)
        {
            Initialize(ref set);
        }
    }
}