using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Rsx.Dumb;
using static DB.LINAA;
using System.Diagnostics;

namespace DB.Tools
{
    public partial class XCOM : CalculateBase
    {
        private decimal seconds = 0;

        private bool calculating = false;
        private Stopwatch stopwatch;
        XCOMPrefRow pref;
        IList<MatrixRow> rows;
        Action<string, string, bool, bool> reporter;
        Action<Exception> exceptionAdder;
        bool offline = false;

        public void Calculate(bool? BKG)
        {

        
            double start = pref.StartEnergy;
            double Totalend = pref.EndEnergy;
            double step = pref.Steps;
            bool useList = pref.UseList;
           
            listOfEnergiesBytes = pref.ListOfEnergies;

            if (rows.Count == 0)
            {
                reporter(NOMATRIX_ERROR, NOMATRIX_TITLE, false,false);
                return;
            }

            _resetProgress?.Invoke(0);

            reporter("A total of " + rows.Count + " matrices were selected", "Checking...", true,false);

            bool connection =   CheckForInternetConnection();


            if (!connection)
            {
                reporter(NOINTERNET_ERROR, NOINTERNET_TITLE, false,false);
                return;
            }
            else
            {
                reporter(INTERNET_MSG, INTERNET_TITLE, false,false);
            }

            IsCalculating = true;

            EventHandler runWorker = delegate
            {
                ILoader l = loaders.Values.OfType<ILoader>()?.FirstOrDefault(o => !o.IsBusy);
                l?.RunWorkerAsync();
            };

            while (rows.Count != 0)
            {

                int actionCount = 0;

                MatrixRow m = rows.FirstOrDefault();

                string msg = m.MatrixName + " was ";
                bool ok = true;
                string title = "Preparing...";
             
                try
                {

                    Action<int> report = progress =>
                    {
                        _showProgress?.Invoke(null, EventArgs.Empty);
                        bool finito = reportProgress(progress, m.MatrixName);
                        if (finito)
                        {
                            m.IsBusy = false;
                            m.MatrixDate = DateTime.Now;
                        }
                    };

                    Action callBack =
                        delegate
                        {
                            updateLoaders(runWorker, m.MatrixID);
                            _callBack?.Invoke(m, EventArgs.Empty);

                        };

                      actionCount +=  addToLoaders(ref m, report, callBack, start, Totalend, step, useList);

                }
                catch (SystemException ex)
                {
                    exceptionAdder(ex);
                    ok = false;
                    msg += "not ";
                    title = "Failed!";
                }

                msg += "selected";

                reporter(msg, title, ok, false);

                rows.Remove(m);

                _resetProgress?.Invoke(actionCount);


            }


            reporter("A total of " + loaders.Count + " matrices were prepared", "Starting...", true,false);


            runWorker.Invoke(null, EventArgs.Empty);
            runWorker.Invoke(null, EventArgs.Empty);
            runWorker.Invoke(null, EventArgs.Empty);

         

        }

        private bool reportProgress(int x, string matrixName)
        {
            string msg = matrixName + "\nProgress:\t" + x.ToString() + " %\n";
            string title = "Finished with...";
            bool finito = (x == 100);
            if (!finito)
            {
                title = "Working on...";
            }
            reporter(msg, title, true, false);
            return finito;
        }


        private int addToLoaders(ref MatrixRow m, Action<int> report, Action callBack, double start, double Totalend, double step, bool useList = true)
        {


            m.RowError = string.Empty;
            bool goIn = (!m.HasErrors() && m.ToDo);

            if (!goIn) throw new SystemException("The matrix has errors");

                    IList<Action> actions = generateActions(ref m, start, Totalend, step, offline, useList);

                    if (actions.Count == 0) throw new SystemException("The Actions list for the Matrix is empty");

                    ILoader ld = new Loader();
                    ld.Set(actions, callBack, report);
                    loaders.Add(m.MatrixID, ld);

            return actions.Count;

        }

      

        private void updateLoaders( EventHandler runWorker,  int matrixID)
        {

                if (loaders.Contains(matrixID))
                {
                    loaders.Remove(matrixID);
                }
            if (loaders.Count == 0 && IsCalculating)
            {
                IsCalculating = false;

            }
            else if (loaders.Count != 0)
            {
                if (IsCalculating)
                {
                    reporter(CALCULATING_MSG + loaders.Count, CALCULATING_TITLE, true,false);
                }
                runWorker.Invoke(null, EventArgs.Empty);
            }
    

        }

    

        public IList<Action> generateActions(ref MatrixRow matrix, double start, double totalEnd, double step, bool offline, bool useList =false)
        {
            //finds the MUEs for each 1keV, given start and Totalend energies, by NrEnergies (keV) steps.

            List<Action> ls = new List<Action>();


            MatrixRow m = matrix;

            int numberOfFiles = 0;
            double initialStart = start;

            double delta, end;

            string listOfenergies, compositions;



            setValuesForQuery(m, ref start, ref totalEnd, step, useList, out delta, out end, out listOfenergies);

            compositions = GetCompositionString(m.MatrixComposition);

            while (end <= totalEnd)
            {

                if (!useList)
                {
                    int lines = GetNumberOfLines(step, start, end);
                    listOfenergies = MakeEnergiesList(step, start, lines);
                }
                string labelName = m.MatrixName + ": " + start + " to " + end + " keV";
                Action action = setMainAction(m.MatrixID, numberOfFiles, listOfenergies, compositions, labelName, start, end);

                start += delta;
                end += delta;

                numberOfFiles++;

                ls.Add(action);

            }

            numberOfFiles--;

            Action action2 = delegate
            {
                MUESDataTable mu = Interface.IPopulate.IGeometry.GetMUES(ref m, !offline);
                while (numberOfFiles >= 0)
                {
                    getMUESFromNIST(m.MatrixDensity, _startupPath, ref mu, m.MatrixID, numberOfFiles);
                    numberOfFiles--;
                }
                Interface.IStore.SaveMUES(ref mu, ref m, !offline);
            };

            Action action3 = delegate
            {
                double max = 1e7;

                double min = 1;
                string labelName = m.MatrixName + ": " + min.ToString() + " to " + (max).ToString() + " keV";
                listOfenergies = MakeEnergiesList(max - min, min, 2);

                string Response = QueryXCOM(compositions, listOfenergies, labelName, true);
                if (string.IsNullOrEmpty(Response)) return;

                string tempFile = _startupPath + m.MatrixID + ".FULL RANGE" + PictureExtension;
                getPicture(ref Response, tempFile);

            };


            ls.Add(action3);
            ls.Add(action2);

            m.IsBusy = true;


            return ls;
        }

    

        private void getPicture(ref string Response, string tempFile)
        {
            File.Delete(tempFile);
            string aux = getPicTag(ref Response);
            if (aux.Contains("Error")) return;

            string uriString = "https://physics.nist.gov/PhysRefData/Xcom/tmp/graph" + "_" + aux + PictureExtension;
            Uri uri = new Uri(uriString);
            WebClient client = new WebClient();
            client.DownloadFile(uri, tempFile);
            client.Dispose();
        }

        private Action setMainAction(int matrixID, int numberOfFiles, string listOfenergies, string compositions, string labelName, double start, double end)
        {
            Action action = delegate
            {

                try
                {
                    string Response = string.Empty;
                    string  tempFile = string.Empty;
                   
                    Response = QueryXCOM(compositions, listOfenergies,labelName,false);
                    tempFile = _startupPath + matrixID + punto +"N" + numberOfFiles + punto;
                    File.WriteAllText(tempFile , Response);

                    Response = QueryXCOM(compositions, listOfenergies, labelName, true);
                    getPicture(ref Response, tempFile +start.ToString()+" - "+end.ToString()+ PictureExtension);


                }
                catch (SystemException ex)
                {

                    exceptionAdder(ex);
                }



            };
            return action;
        }



        public bool SQL
        {
            get
            {
                return !(bool)Interface?.IPreferences.CurrentPref.Offline;
            }
        }

    

        public XCOM() : base()
        {

            stopwatch = new Stopwatch();

        }



     
        public void Set(ref Interface inter)
        {
            Interface = inter;
        }

    

        public new bool IsCalculating
        {
            get
            {
                return calculating;
            }
            set
            {
                calculating = value;

                if (!calculating)
                {
                 
                    CheckCompletedOrCancelled();
                }
                else
                {
                    loaders.Clear();
                    stopwatch.Start();
                    seconds = 0;
                }
            }
        }

     
        public void CheckCompletedOrCancelled()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                seconds = Decimal.Round(Convert.ToDecimal(stopwatch.Elapsed.TotalSeconds), 0);
            }
            string log = string.Empty;
            bool ok = true;
            string title = "Completed!";
            if (loaders.Count == 0)
            {
                log = "Everything completed in ";
            }
            else
            {
                foreach (ILoader item in loaders.Values)
                {
                    item.CancelLoader();
                }
                ok = false;
                loaders.Clear();
                title = "Cancelled!";
                log = "Computations cancelled after ";
            }
            log += seconds + " seconds";

            reporter(log, title, ok, false);
        }

        public Action<string, string, bool, bool> Reporter
        {
           

            set
            {
                reporter = value;
            }
        }

        public XCOMPrefRow Preferences
        {
            set
            {
                pref = value;
            }
        }

        public IList<MatrixRow> Rows
        {
            get
            {
                return rows;
            }

            set
            {
                rows = value;
            }
        }

        public Action<Exception> ExceptionAdder
        {
            get
            {
                return exceptionAdder;
            }

            set
            {
                exceptionAdder = value;
            }
        }

        public bool Offline
        {
            get
            {
                return offline;
            }

            set
            {
                offline = value;
            }
        }
    }

    public partial class XCOM 
    {
        public static int maxEnergies = 75;
        protected static string punto = ".";

        public static string PictureExtension = ".png";
        //  protected static ASCIIEncoding encoding = new ASCIIEncoding();

        protected static string CALCULATING_MSG = "Number of calculations pending ";
        protected static string CALCULATING_TITLE = "Still calculating...";

        protected static string DONE_MSG = "Number of calculations pending ";
        protected static string DONE_TITLE = "All finished";

        public static Uri XCOMTestUri = new Uri("https://physics.nist.gov/");
        public static Uri XCOMUri = new Uri("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
        public static Uri XCOMUriPic = new Uri("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3");


        protected static string XCOM_ERROR = "Problems comunicating with XCOM";
        protected static string XCOM_TITLE = "XCOM did not answer the query";

        protected static string NOMATRIX_ERROR = "No matrices where selected";
        protected static string NOMATRIX_TITLE = "Nothing to do...";

        protected static string INTERNET_MSG = "The Internet connection is valid";
        protected static string INTERNET_TITLE = "Server available!";

        protected static string NOINTERNET_ERROR = "No Internet connection found\nThis program requires a valid Internet connection";
        protected static string NOINTERNET_TITLE = "Server not available!";

        public static bool CheckForInternetConnection()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new WebClient())
                using (client.OpenRead(XCOMTestUri))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static IList<string[]> ExtractComposition(String XCOMResponse, ref LINAA.ElementsDataTable elements)
        {
            StringSplitOptions o = StringSplitOptions.RemoveEmptyEntries;
            string[] Ncontent = XCOMResponse.Split(new string[] { "<p" }, o)[1].Split('\n');
            // Ncontent[0] is trash NContent[last] is trash

            IList<string[]> ls = new List<string[]>();

            for (int i = 1; i < Ncontent.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(Ncontent[i])) continue;
                if (Ncontent[i].Contains('<')) continue;

                string[] ZCont = Ncontent[i].Split('\t');
                string Z = ZCont[0].Split('=')[1].Trim();
                string cont = ZCont[1].Replace(": ", null).Trim();

                int z = Convert.ToInt32(Z);

                ElementsRow ele = elements.OfType<ElementsRow>().FirstOrDefault(a => a.Z == z);

                if (ele != null)
                {
                    double content = Convert.ToDouble(cont);
                    content *= 100;
                    cont = content.ToString();
                    ls.Add(new string[] { ele.Element, cont });
                }
            }
            return ls;
        }

        /*
        public static string MakeCompositionsList(string MatrixComposition)
        {
            string str = string.Empty;
            if (MatrixComposition != null)
            {
                List<string> list = new List<string>();
                string[] strArray = new string[40];
                char[] chArray = new char[] { ',', '(' };
                strArray = MatrixComposition.Split(new char[] { chArray[0] });
                int index = 0;
                for (index = 0; index < strArray.Length; index++)
                {
                    string[] strArray2 = strArray[index].Split(chArray[1]);
                    string formula = strArray2[0].Replace("#", null);
                    string composition = strArray2[1].Replace("%", null);
                    composition = composition.Replace(")", null);
                    formula = formula.Trim();
                    composition = composition.Trim();
                    string item = formula + " " + composition;
                    list.Add(item);
                }
                foreach (string str5 in list)
                {
                    str += str5 + "\n";
                }
            }
            return str;
        }
        */

        public static int GetNumberOfLines(double step, double StartEnergy, double EndEnergy)
        {
            double lines = (EndEnergy - StartEnergy) / (step);

            int NrOfEnergies = Convert.ToInt32(Math.Ceiling((lines)));
            return NrOfEnergies;
        }

        public static string MakeEnergiesList(double step, double StartEnergy, int NrOfEnergies)
        {
            string str = string.Empty;

            double Energy = 0;

            for (int i = 0; i < NrOfEnergies; i++)
            {
                Energy = StartEnergy + (step * i);
                str = str + ((Energy * 0.001)).ToString() + "\n";
            }
            return str;
        }

     

        public static string QueryXCOM(string composition, string energies,string name = "default matrix", bool picture = false)
        {
            byte[] bytes = null;
        
            if ((composition != null) && (energies != null))
            {
                string s = "Formulae=" + composition + "&Energies=" + energies;
                //   string s = s2;
                if (picture)
                {
                    s += "&Name =" + name + "&Graph1=on";
                    s += "&Graph2=on" + "&Graph3=on" + "&Graph4=on" + "&Graph5=on" + "&Graph6=on" + "&Graph7=on";
                    s += "&NumAdd=1" + "&Output=off";// + "&Graph4=on" + "&Graph5=on" + "&Graph6=on" + "&Graph7=on";
                  
                }
                ASCIIEncoding encoding = new ASCIIEncoding();

                bytes = encoding.GetBytes(s);

               
                //bytes2 = encoding.GetBytes(s2);
            }

            string completo = string.Empty;
          

            if (bytes != null)
            {
                completo = getHTTPQuery(bytes,picture);
            }

            return completo;
        }

        public static string GetCompositionString(string MatrixComposition)
        {
            IList<string[]> str0 = RegEx.StripComposition(MatrixComposition);
            string str = string.Empty;
            foreach (string[] str5 in str0)
            {
                str += str5[0] + " " + str5[1] + "\n";
            }

            return str;
        }

   
      //  protected static string ext = ".txt";
    //    protected static string ext2 = ".v2.xml";

        private static string getHTTPQuery(byte[] bytes, bool picture = false)
        {
            string completo;
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://physics.nist.gov/cgi-bin/Xcom/xcom2");
            //   HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3");
            HttpWebRequest request = null;
            if (picture) request = (HttpWebRequest)WebRequest.Create(XCOMUriPic);
            else request = (HttpWebRequest)WebRequest.Create(XCOMUri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            //request.ProtocolVersion = HttpVersion.Version10;
            //  request.ProtocolVersion = HttpVersion.Version11;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            requestStream.Dispose();
            requestStream = null;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            completo = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            reader = null;

            responseStream.Close();
            responseStream.Dispose();
            responseStream = null;

            response = null;
            request = null;
            return completo;
        }
  
        private static int getMUESFromNIST(double density, string path, ref MUESDataTable dt, int matrixID, int subsection)
        {

            int added = 0;

            string tempFile = path + matrixID + punto + "N" + subsection+ punto;
            if (!File.Exists(tempFile)) return added;

            StreamReader reader = new StreamReader(tempFile);

            reader.ReadLine();
            reader.ReadLine();
            string[] strArray2 = reader.ReadToEnd().Split(new char[] { '\n' });
            

            int num = 0;
            int num2 = 1;
            for (int i = 0; i < strArray2.LongLength; i++)
            {
                if (strArray2[i].Equals(string.Empty)) //find initial row
                {
                    num = i;
                    num2 = 0;
                }
                else if ((num2 == 0) && strArray2[i].Contains("</pre>")) //find end row
                {
                    num2 = i;
                    break;
                }
            }
            num++; //this is the initial row
            num2--; //this is the final row

            String[] temp = null;

            for (int j = num; j <= num2; j++)
            {
                temp = strArray2[j].TrimStart().Split(' ');
                MUESRow r = dt.NewMUESRow(); //dt.FirstOrDefault(o => o.Energy ==null);
                r.MatrixID = matrixID;
                setMUESRow(density, temp, ref r);

                MUESRow destiny = dt.FirstOrDefault(o => o.Energy == r.Energy);
                if (destiny != null)
                {
                    dt.RemoveMUESRow(destiny);
                }
                dt.AddMUESRow(r);
                added++;
            }

            reader.Close();
            reader.Dispose();
            reader = null;

            File.Delete(tempFile);

            return added;
            //photon cross sections in g/cm2 and energies in keV, photon * density = mu (linear attenuation)
        }
     

        private static string getPicTag(ref string Response)
        {
            string aux = string.Empty;
            string[] split = Response.Split('\n');
            for (int j = 0; j < split.Length; j++)
            {
                aux = split[j];
                if (aux.Contains("<td><img src="))
                {
                    aux = aux.Split('.').First();
                    aux = aux.Split('_').Last();
                    break;
                }

            }

            return aux;
        }

        private static void setMUESRow(double density, string[] temp, ref MUESRow r)
        {
            if (temp.LongLength == 8)
            {
                r.Energy = Convert.ToDouble(temp[0]) * 1000;
                r.Density = density;
                r.MACS = Convert.ToDouble(temp[1]);
                r.MAIS = Convert.ToDouble(temp[2]);
                r.PE = Convert.ToDouble(temp[3]);
                r.PPNF = Convert.ToDouble(temp[4]);
                r.PPEF = Convert.ToDouble(temp[5]);
                r.MATCS = Convert.ToDouble(temp[6]);
                r.MATNCS = Convert.ToDouble(temp[7]);
                r.Edge = String.Empty;
            }
            else if (temp.LongLength == 10)
            {
                r.Energy = Convert.ToDouble(temp[2]) * 1000;
                r.Density = density;
                r.MACS = Convert.ToDouble(temp[3]);

                r.MAIS = Convert.ToDouble(temp[4]);
                r.PE = Convert.ToDouble(temp[5]);
                r.PPNF = Convert.ToDouble(temp[6]);
                r.PPEF = Convert.ToDouble(temp[7]);
                r.MATCS = Convert.ToDouble(temp[8]);
                r.MATNCS = Convert.ToDouble(temp[9]);
                r.Edge = temp[0] + temp[1];
            }
        }

    }

    public partial class XCOM
    {

        private Hashtable loaders = new Hashtable();

        protected internal Interface Interface;
        //   private string png = ".png";


        byte[] listOfEnergiesBytes=null;

        private void setValuesForQuery(MatrixRow m, ref double start, ref double Totalend, double step, bool useList, out double delta, out double end, out string listOfenergies)
        {
            int NrEnergies = GetNumberOfLines(step, start, Totalend);
            //maximum number of energies per query


            delta = 0;
            if (maxEnergies > NrEnergies)
            {
                delta = (NrEnergies * step);

            }
            else delta = (maxEnergies * step);
            //increment in energy
            //   double delta = (maxEnergies * step);
            //    delta = (maxEnergies * step);
            //end energy
            end = start + delta;

            listOfenergies = string.Empty;
            if (useList)
            {
                double factor = 1000;
             
                listOfenergies = Encoding.UTF8.GetString(listOfEnergiesBytes);
                string[] energies = listOfenergies.Split('\n');
                NrEnergies = energies.Count();
                start = Convert.ToDouble(energies.First()) * factor;
                string lastEnergy = energies.Last();
                if (NrEnergies > maxEnergies)
                {
                    lastEnergy = energies.ElementAt(maxEnergies - 1);
                }
                end = Convert.ToDouble(lastEnergy) * factor;
                Totalend = end;
            }
            // arr = Encoding.UTF8.GetBytes( DB.Properties.Resources.XCOM);

        }

     


    }
}