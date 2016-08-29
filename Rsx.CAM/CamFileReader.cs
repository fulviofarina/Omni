using System;
using System.Collections.Generic;
using System.Linq;

namespace Rsx.CAM
{
    public interface IDetectorX
    {
        bool Acquiring { get; set; }
        double Area { get; set; }
        double AreaUnc { get; set; }

        void Clear();

        bool Controlled { get; set; }
        double CountTime { get; set; }
        string DetectorCodeName { get; set; }

        void Dispose();

        decimal DT { get; set; }
        SystemException Exception { get; set; }

        bool GetROI(double energyStart, double energyEnd, double backCh, double limitUnc);

        void GetTimes();

        double Integral { get; set; }

        void IsAcquiring();

        DateTime LastStart { get; set; }
        double LiveTime { get; set; }
        bool Opened { get; set; }
        double PresetTime { get; set; }

        bool Save(string measurementPath);

        string Server { get; set; }

        void Start();

        DateTime StartDate { get; set; }

        void Stop();

        double VDMDelay { get; set; }
        System.IO.FileInfo LastFileInfo { get; set; }
        string LastFileTag { get; set; }
        string SampleTitle { get; set; }
        string User { get; set; }
        string Description1 { get; set; }
        string Description2 { get; set; }
        string Description3 { get; set; }
        string Description4 { get; set; }

        void GetSampleData();

        void SetSampleData(string user, string project, string sample, string measlabel);
    }

    public class DetectorX : Rsx.CAM.IDetectorX
    {
        private string lastFileTag;

        public string LastFileTag
        {
            get
            {
                return lastFileTag;
            }
            set { lastFileTag = value; }
        }

        private bool writable = false;

        public bool Controlled
        {
            get { return writable; }
            set { writable = value; }
        }

        private CAMSRCLib.CamDatasourceClass Reader;
        private CAMSRCLib.CamDatasourceClass Writer;

        private CAMSRCLib.SourceTypes type = CAMSRCLib.SourceTypes.camSpectroscopyDetector;
        private CAMSRCLib.OpenOptions camReadonly = CAMSRCLib.OpenOptions.camReadOnly;
        private CAMSRCLib.OpenOptions camControl = CAMSRCLib.OpenOptions.camReadWrite;

        private System.IO.FileInfo lastFileInfo;

        public System.IO.FileInfo LastFileInfo
        {
            get { return lastFileInfo; }
            set { lastFileInfo = value; }
        }

        private SystemException exception;

        public SystemException Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        private string sampleTitle;

        public string SampleTitle
        {
            get { return sampleTitle; }
            set { sampleTitle = value; }
        }

        private string description1;

        public string Description1
        {
            get { return description1; }
            set { description1 = value; }
        }

        private string description3;

        public string Description3
        {
            get { return description3; }
            set { description3 = value; }
        }

        private string description2;

        public string Description2
        {
            get { return description2; }
            set { description2 = value; }
        }

        private string description4;

        public string Description4
        {
            get { return description4; }
            set { description4 = value; }
        }

        private string user;

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Gets the info inside the Sample Info of the detector
        /// </summary>
        public void GetSampleData()
        {
            OpenRead();

            if (opened)
            {
                try
                {
                    sampleTitle = (string)Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_STITLE);
                    user = (string)Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SCOLLNAME);
                    description1 = (string)Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC1);
                    description2 = (string)Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC2);
                    description3 = (string)Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC3);
                    description4 = (string)Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC4);
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseRead();
        }

        public void SetSampleData(string user, string project, string sample, string measlabel)
        {
            OpenWrite();
            // bool exists = false;

            if (writable)
            {
                try
                {
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_STITLE, 0, 0, detectorCodeName);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SCOLLNAME, 0, 0, user);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_STYPE, 0, 0, string.Empty);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SIDENT, 0, 0, string.Empty);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC1, 0, 0, detectorCodeName);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC2, 0, 0, project);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC3, 0, 0, sample);
                    this.Writer.set_Parameter(CanberraDataAccessLib.ParamCodes.CAM_T_SDESC4, 0, 0, measlabel);
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseWrite();
        }

        private double vDMDelay = 0;

        public double VDMDelay
        {
            get
            {
                this.vDMDelay = (lastStart - startDate).TotalSeconds;

                return vDMDelay;
            }
            set { vDMDelay = value; }
        }

        private bool opened = false;

        public bool Opened
        {
            get { return opened; }
            set { opened = value; }
        }

        private bool acquiring = false;

        public bool Acquiring
        {
            get { return acquiring; }
            set { acquiring = value; }
        }

        private double area;

        public double Area
        {
            get { return area; }
            set { area = value; }
        }

        private double areaUnc;

        public double AreaUnc
        {
            get { return areaUnc; }
            set { areaUnc = value; }
        }

        private double integral;

        public double Integral
        {
            get { return integral; }
            set { integral = value; }
        }

        private double countTime;

        public double CountTime
        {
            get { return countTime; }
            set { countTime = value; }
        }

        private double liveTime;

        public double LiveTime
        {
            get { return liveTime; }
            set { liveTime = value; }
        }

        private double dT;

        public decimal DT
        {
            get
            {
                this.dT = 0;
                if (countTime != 0)
                {
                    this.dT = (1 - (liveTime / countTime));
                    this.dT = dT * 100;
                }
                return Decimal.Round(Convert.ToDecimal(dT), 1);
            }
            set { dT = (double)value; }
        }

        private string detectorCodeName = string.Empty;

        public string DetectorCodeName
        {
            get { return detectorCodeName; }
            set { detectorCodeName = value; }
        }

        private double presetTime;

        public double PresetTime
        {
            get { return presetTime; }
            set { presetTime = value; }
        }

        private string server;

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime lastStart;

        public DateTime LastStart
        {
            get { return lastStart; }
            set { lastStart = value; }
        }

        public static void NewLink(ref Rsx.CAM.IDetectorX link)
        {
            if (link != null) link.Dispose();
            link = null;
            link = new DetectorX();
        }

        public DetectorX()
        {
            try
            {
                Reader = new CAMSRCLib.CamDatasourceClass();
                Writer = new CAMSRCLib.CamDatasourceClass();
            }
            catch (SystemException EX)
            {
                exception = EX;
            }
        }

        public void Dispose()
        {
            CloseRead();
            CloseWrite();
            Reader = null;
            Writer = null;
        }

        public static void KillVDM(string AServer)
        {
            if (System.Environment.MachineName.ToUpper().CompareTo(AServer) == 0) return;
            IEnumerable<System.Diagnostics.Process> processes = System.Diagnostics.Process.GetProcesses();
            System.Diagnostics.Process VDM = processes.Where(p => p.ProcessName.Contains("winvdm")).FirstOrDefault();
            if (VDM != null) VDM.Kill();
        }

        public void GetTimes()
        {
            OpenRead();
            this.startDate = DateTime.MinValue;
            this.liveTime = 0;
            this.countTime = 0;
            this.presetTime = 0;

            if (opened)
            {
                try
                {
                    this.countTime = Convert.ToDouble(Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_X_EREAL));
                    lastStart = DateTime.Now.Subtract(new TimeSpan(0, 0, Convert.ToInt32(this.countTime)));
                    this.liveTime = Convert.ToDouble(Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_X_ELIVE));
                    this.presetTime = Convert.ToDouble(Reader.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_X_PREAL));
                    this.startDate = Convert.ToDateTime(Reader.get_Parameter(CanberraDeviceAccessLib.ParamCodes.CAM_X_ASTIME));
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseRead();
        }

        public bool GetROI(double energyStart, double energyEnd, double backCh, double limitUnc)
        {
            bool success = false;

            CAMSRCLib.CamDatasourceClass ReaderAux = new CAMSRCLib.CamDatasourceClass();

            exception = null;
            try
            {
                ReaderAux.OpenEx(detectorCodeName, this.camReadonly, type, this.server);

                /*
             object o = device.get_Information( CAMSRCLib.InformationType.camName);
             object time =  device.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_X_EREAL, 0, 0);
             object roi = device.get_Parameter(CanberraDeviceAccessLib.ParamCodes.CAM_L_SHOWROIS, 0, 0);
             object f = o;
                 o = device.get_Parameter(CanberraDeviceAccessLib.ParamCodes.CAM_L_EXPROIS, 0, 0);

                 */
            }
            catch (SystemException ex)
            {
                exception = ex;
            }
            try
            {
                object[] argsIn = new object[] { energyStart, energyEnd, backCh, 1, true };
                object result = ReaderAux.Evaluate.Evaluate(CAMSRCLib.EvaluateOptions.camRoiInformation, argsIn);
                double[] args = result as double[];
                if (args[2] < limitUnc) success = true;
                this.area = args[1];
                this.integral = args[0];
                this.areaUnc = args[2];
                this.countTime = (double)ReaderAux.get_Parameter(CanberraDataAccessLib.ParamCodes.CAM_X_EREAL, 0, 0);
            }
            catch (SystemException ex)
            {
                exception = ex;
            }
            try
            {
                ReaderAux.Close();
            }
            catch (SystemException ex)
            {
                exception = ex;
            }

            ReaderAux = null;

            return success;
        }

        public void IsAcquiring()
        {
            OpenRead();

            this.acquiring = false;
            if (opened)
            {
                try
                {
                    int status = Convert.ToInt16(Reader.Status.ToString());
                    //stoped and cleared
                    // if (status == 2080)
                    //started (acquiring)	//2052 when acquiring?
                    //status == 2084 when I take control?
                    if (status == 2052 || status == 2084) this.acquiring = true;
                }
                catch (SystemException x)
                {
                    exception = x;
                }
            }

            CloseRead();
        }

        private void CloseRead()
        {
            // if (!opened) return;

            try
            {
                Reader.Close();
                opened = false;
            }
            catch (SystemException x)
            {
                exception = x;
            }
        }

        private void OpenRead()
        {
            exception = null;
            opened = false;
            try
            {
                Reader.OpenEx(detectorCodeName, this.camReadonly, type, this.server);
                opened = true;
            }
            catch (SystemException x)
            {
                exception = x;
            }
        }

        private void CloseWrite()
        {
            // if (!writable) return;
            try
            {
                Writer.Close();
                writable = false;
            }
            catch (SystemException x)
            {
                exception = x;
            }
        }

        private void OpenWrite()
        {
            exception = null;
            writable = false;
            try
            {
                Writer.OpenEx(detectorCodeName, this.camControl, type, this.server);
                writable = true;
            }
            catch (SystemException ex)
            {
                exception = ex;
            }
        }

        public bool Save(string measurementPath)
        {
            OpenWrite();
            bool exists = false;

            if (writable)
            {
                try
                {
                    Writer.Save(measurementPath, true);
                    exists = System.IO.File.Exists(measurementPath);
                    lastFileInfo = null;
                    lastFileTag = string.Empty;

                    if (exists)
                    {
                        lastFileInfo = new System.IO.FileInfo(measurementPath);
                        System.IO.FileInfo f = this.lastFileInfo;
                        lastFileTag = "Saved on: " + f.LastWriteTime + "\nSize: " + f.Length + "\nPath: " + f.FullName;

                        long size = lastFileInfo.Length;
                        if (size < 50000)
                        {
                            throw new SystemException("The size of the spectra file " + measurementPath + " is less than 50Kb (" + size + " bytes), which does not look good.\nPlease verify the VDM is not blocked, by closing and reopening (Genie and then the VDM) on the server computer.\nPlease do it while this program is turned off.");
                        }
                    }
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseWrite();

            return exists;
        }

        public void Start()
        {
            OpenWrite();

            if (writable)
            {
                try
                {
                    Writer.Device.ExecuteCommand(CAMSRCLib.DeviceCommands.camStart);
                    acquiring = true;
                    lastStart = DateTime.Now;
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseWrite();
        }

        public void Clear()
        {
            OpenWrite();

            if (writable)
            {
                try
                {
                    Writer.Device.ExecuteCommand(CAMSRCLib.DeviceCommands.camClear);
                    if (acquiring) lastStart = DateTime.Now;
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseWrite();
        }

        public void Stop()
        {
            OpenWrite();
            if (writable)
            {
                try
                {
                    Writer.Device.ExecuteCommand(CAMSRCLib.DeviceCommands.camStop);
                    acquiring = false;
                }
                catch (SystemException ex)
                {
                    exception = ex;
                }
            }
            CloseWrite();
        }
    }

    public class CamFileReader
    {
        private CanberraDataAccessLib.DataAccess dataAccess;
        private string fileName = string.Empty;

        // Aquisition parameters
        private string _aSTIME; // Acquisition start time

        private double _pLIVE; // Preset live time
        private double _pREAL; // Preset real time
        private double _eLIVE; // Elapsed live time
        private double _eREAL; // Elapsed real time

        private string _detectorName; // detector name

        public CamFileReader()
        {
            this.dataAccess = new CanberraDataAccessLib.DataAccess();
        }

        public bool GetData(string fileName)
        {
            bool succes = false;
            this._exception = null;

            this.fileName = fileName;
            try
            {
                dataAccess.Open(this.fileName, CanberraDataAccessLib.OpenMode.dReadOnly, 8192);
            }
            catch (Exception ex)
            {
                //_exception = ex; //nothing should be here because always generates exception
            }
            try
            {
                this._aSTIME = dataAccess.get_Param(CanberraDataAccessLib.ParamCodes.CAM_X_ASTIME, 0, 0).ToString();
                this._pLIVE = Convert.ToDouble(dataAccess.get_Param(CanberraDataAccessLib.ParamCodes.CAM_X_PLIVE));
                this._pREAL = Convert.ToDouble(dataAccess.get_Param(CanberraDataAccessLib.ParamCodes.CAM_X_PREAL));
                this._eLIVE = Convert.ToDouble(dataAccess.get_Param(CanberraDataAccessLib.ParamCodes.CAM_X_ELIVE));
                this._eREAL = Convert.ToDouble(dataAccess.get_Param(CanberraDataAccessLib.ParamCodes.CAM_X_EREAL));
                this._detectorName = dataAccess.get_Param(CanberraDataAccessLib.ParamCodes.CAM_T_DETNAME).ToString();

                succes = true;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }

            try
            {
                dataAccess.Close(CanberraDataAccessLib.CloseMode.dNoUpdate);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }

            return succes;
        }

        private Exception _exception;

        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }

        public string ASTIME
        {
            get { return _aSTIME; }
        }

        public double PLIVE
        {
            get { return _pLIVE; }
        }

        public double PREAL
        {
            get { return _pREAL; }
        }

        public double EREAL
        {
            get { return _eREAL; }
        }

        public double ELIVE
        {
            get { return _eLIVE; }
        }

        public string DetectorName
        {
            get { return _detectorName; }
        }
    }

    public class Linx
    {
        private Canberra.DSA3K.DataTypes.Communications.IDevice device = null;

        public Linx()
        {
            //	  Canberra.DSA3K.DataTypes.Communications.DeviceFactory.CreateInstance(Canberra.DSA3K.DataTypes.Communications.DeviceFactory.DeviceInterface);

            object o = Canberra.DSA3K.DataTypes.Communications.DeviceFactory.CreateInstance(Canberra.DSA3K.DataTypes.Communications.DeviceFactory.DeviceInterface.IDevice);

            device = (Canberra.DSA3K.DataTypes.Communications.IDevice)o;

            string local = "10.0.3.122";

            string linx = "192.168.114.001";
            try
            {
                device.Open(local, linx);
            }
            catch (Canberra.DSA3K.DataTypes.DeviceException ex)
            {
                string exce = ex.Description;
            }
            bool opened = device.IsOpen;

            object real = device.GetProperty("Real Time");

            string retext = real.ToString();
        }

        public static String GetLocalAddress()
        {
            System.Net.IPHostEntry Entry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            String LocalAddr = "";
            int AdapterCnt = 0;
            foreach (System.Net.IPAddress Address in Entry.AddressList)
            {
                //Skip loopbacks
                if (System.Net.IPAddress.IsLoopback(Address)) continue;

                AdapterCnt++;
                if (0 == LocalAddr.Length) { LocalAddr = Address.ToString(); }
            }

            //Let .NET figure the 'best' adapter to use instead of using
            //the first adapter found
            if (AdapterCnt > 1) return "";

            //We found 1 adapter so return it's address
            return LocalAddr;
        }
    }
}