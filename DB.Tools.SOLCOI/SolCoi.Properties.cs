using System;
using System.Diagnostics;

namespace DB.Tools
{
    public partial class SolCoin : IDisposable
    {
        private bool _disposed = false;
        private bool calculateCOIS;
        private bool calculateSolidAngles;
        private bool cleanEffiCOIS;
        private bool cleanSolidAngles;
        private Process cOINProcess;
        private double cOINProcessTime;
        private DB.LINAA.DetectorsAbsorbersRow detectorAbsorber;
        private LINAA.DetectorsDimensionsRow detectorDimension;
        private LINAA.HoldersRow[] detectorHolder;
        private String detectorName;
        private double[] energies;
        private SystemException exception;
        private IOFiles files;
        private bool finished;
        private LINAA.GeometryRow geometry;
        private IntegrationModes integrationMode;
        private LINAA linaa;
        private LINAA.GeometryRow reference;
        private DateTime sentAt;
        private LINAA Set;
        private Process solangProcess;

        private double solangProcessTime;

        private String solcoinPath;

        private bool storeCOIS;

        private bool storeSolidAngles;

        private object tag;

        public enum IntegrationModes
        {
            AsNonDisk = 20202020,
            AsPointSource = 01242424,
            AsDisk = 01019696,
        };

        public static double[] ClassicEnergies
        {
            get
            {
                double[] classicEnergies = new double[17];
                classicEnergies[0] = 40;
                classicEnergies[1] = 50;
                classicEnergies[2] = 60;
                classicEnergies[3] = 80;
                classicEnergies[4] = 100;
                classicEnergies[5] = 150;
                classicEnergies[6] = 200;
                classicEnergies[7] = 300;
                classicEnergies[8] = 400;
                classicEnergies[9] = 500;
                classicEnergies[10] = 600;
                classicEnergies[11] = 800;
                classicEnergies[12] = 1000;
                classicEnergies[13] = 1500;
                classicEnergies[14] = 2000;
                classicEnergies[15] = 3000;
                classicEnergies[16] = 3500;
                return classicEnergies;
            }

            set
            {
                double[] classicEnergies = new double[17];
                classicEnergies = value;
            }
        }

        public bool CalculateCOIS
        {
            get { return calculateCOIS; }
            set { calculateCOIS = value; }
        }

        public bool CalculateSolidAngles
        {
            get { return calculateSolidAngles; }
            set { calculateSolidAngles = value; }
        }

        public bool CleanEffiCOIS
        {
            get { return cleanEffiCOIS; }
            set { cleanEffiCOIS = value; }
        }

        public bool CleanSolidAngles
        {
            get { return cleanSolidAngles; }
            set { cleanSolidAngles = value; }
        }

        public Process COINProcess
        {
            get { return cOINProcess; }
            set { cOINProcess = value; }
        }

        public double COINProcessTime
        {
            get { return cOINProcessTime; }
            set { cOINProcessTime = value; }
        }

        public LINAA.DetectorsAbsorbersRow DetectorAbsorber
        {
            get { return detectorAbsorber; }
            set { detectorAbsorber = value; }
        }

        public LINAA.DetectorsDimensionsRow DetectorDimension
        {
            get { return detectorDimension; }
            set { detectorDimension = value; }
        }

        public LINAA.HoldersRow[] DetectorHolder
        {
            get { return detectorHolder; }
            set { detectorHolder = value; }
        }

        public String DetectorName
        {
            get { return detectorName; }
            set { detectorName = value; }
        }

        public double[] Energies
        {
            get { return energies; }
            set { energies = value; }
        }

        public SystemException Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        public IOFiles Files
        {
            get { return files; }
            set { files = value; }
        }

        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }

        public LINAA.GeometryRow Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }

        public IntegrationModes IntegrationMode
        {
            get { return integrationMode; }
            set { integrationMode = value; }
        }

        public bool IsDisposed
        {
            get { return _disposed; }
            set { _disposed = value; }
        }

        public LINAA Linaa
        {
            get { return linaa; }
            set { linaa = value; }
        }

        public LINAA.GeometryRow Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        public DateTime SentAt
        {
            get { return sentAt; }
            set { sentAt = value; }
        }

        public Process SolangProcess
        {
            get { return solangProcess; }
            set { solangProcess = value; }
        }

        public double SolangProcessTime
        {
            get { return solangProcessTime; }
            set { solangProcessTime = value; }
        }

        public String SolCoinPath
        {
            get
            {
                solcoinPath = Set.FolderPath + Properties.Resources.SolCoiFolder;
                return solcoinPath;
            }
            set
            {
                solcoinPath = value;
            }
        }

        public bool StoreCOIS
        {
            get { return storeCOIS; }
            set { storeCOIS = value; }
        }

        public bool StoreSolidAngles
        {
            get { return storeSolidAngles; }
            set { storeSolidAngles = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}