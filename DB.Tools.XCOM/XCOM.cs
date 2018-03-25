﻿using System;
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

namespace DB.Tools
{
    public partial class XCOM : CalculateBase
    {

      

        public void Calculate(bool? BKG)
        {
            Interface.IBS.Matrix.EndEdit();
            Interface.IBS.Compositions.EndEdit();

            IsCalculating = true;

            _resetProgress?.Invoke(0);


            int position = 0;

            EventHandler runWorker = delegate
            {
                ILoader l = loaders.Values.OfType<ILoader>()?.FirstOrDefault(o => !o.IsBusy);
                l?.RunWorkerAsync();
            };

            if (Interface.IPreferences.CurrentXCOMPref.Loop)
            {
                position = Interface.IBS.Matrix.Count - 1;
                Interface.IBS.Matrix.MoveFirst();
            }
            else position = Interface.IBS.Matrix.Position;

           

            while (calculating)
            {

                try
                {
                    MatrixRow m = Interface.ICurrent.Matrix as MatrixRow;

                    IList<Action> ls =  CalculateUnit(m);

                    if (ls.Count != 0)
                    {

                        _resetProgress?.Invoke(ls.Count);

                        Action<int> report = x =>
                        {
                            _showProgress?.Invoke(null, EventArgs.Empty);
                        };
                        Action callBack = delegate
                        {
                            if (loaders.Contains(m.MatrixID))
                            {
                                loaders.Remove(m.MatrixID);
                            }

                            if (loaders.Count == 0) calculating = false;
                            else runWorker.Invoke(null, EventArgs.Empty);

                            _callBack?.Invoke(m, EventArgs.Empty);
                         
                        };

                        ILoader ld = new Loader();
                        ld.Set(ls, callBack, report);
                        loaders.Add(m.MatrixID, ld);
                    }

                    Application.DoEvents();

                    if (Interface.IBS.Matrix.Position == position) break;
                    else Interface.IBS.Matrix.MoveNext();
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                }
            }

            runWorker.Invoke(null,EventArgs.Empty);
            runWorker.Invoke(null, EventArgs.Empty);
            runWorker.Invoke(null, EventArgs.Empty);

        }

        public IList<Action> CalculateUnit(MatrixRow m)
        {
            //finds the MUEs for each 1keV, given start and Totalend energies, by NrEnergies (keV) steps.
          
            List<Action> ls = new List<Action>();

            m.RowError = string.Empty;
            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;
            bool goIn = !m.HasErrors() || pref.Force;
            if (goIn)
            {
                int i = 0;
                Action action = delegate
                {
                    i = makeFiles(ref m, ref pref);
                };

                Action action3 = delegate
                {
                     makePic(ref m, ref pref);
                };

                Action action2 = delegate
                {
                    MUESDataTable mu = Interface.IPopulate.IGeometry.GetMUES(ref m, SQL);
                    while (i >= 0)
                    {
                        getMUES(m.MatrixDensity, _startupPath, ref mu, m.MatrixID, i);
                        i--;
                    }
                    if (mu.Count == 0) return;
                    Interface.IStore.SaveMUES(ref mu, ref m, SQL);
                };
             
                ls.Add(action);
                ls.Add(action2);
                ls.Add(action3);
            }

            return ls;
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
        }



     
        public void Set(ref Interface inter)
        {
            Interface = inter;
        }

        private bool calculating = false;

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
                    //check if cancelled
                    if (loaders.Count!=0)
                    {
                        //    IEnumerable<ILoader> lds = loaders.Values.OfType<ILoader>();
                        //   lds = lds.Where(o => !o.IsBusy);
                    }
                }
                loaders.Clear();
            }
        }
    }

    public partial class XCOM 
    {
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

                LINAA.ElementsRow ele = elements.OfType<LINAA.ElementsRow>().FirstOrDefault(a => a.Z == z);

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

        public static Uri XCOMUri = new Uri("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
        public static Uri XCOMUriPic = new Uri("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3");
        public static string QueryXCOM(string composition, string energies,string name = "default matrix", bool picture = false)
        {
            byte[] bytes = null;
        
            if ((composition != null) && (energies != null))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ASCIIEncoding encoding = new ASCIIEncoding();
                string s = "Formulae=" + composition + "&Energies=" + energies;
                //   string s = s2;
                if (picture)
                {
                    s += "&Name =" + name + "&Graph1=on";
                    s += "&Graph2=on" + "&Graph3=on" + "&Graph4=on" + "&Graph5=on" + "&Graph6=on" + "&Graph7=on";
                    s += "&NumAdd=1" + "&Output=off";// + "&Graph4=on" + "&Graph5=on" + "&Graph6=on" + "&Graph7=on";
                  
                }
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

        protected static string XCOM_ERROR = "Problems comunicating with XCOM";
        protected static string XCOM_TITLE = "XCOM did not answer the query";
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

        private static void getMUES(double density, string path, ref LINAA.MUESDataTable dt, int matrixID, int subsection)
        {

            string tempFile = path + matrixID + "." + subsection;
            if (!File.Exists(tempFile)) return;

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
                LINAA.MUESRow r = dt.NewMUESRow(); //dt.FirstOrDefault(o => o.Energy ==null);
                r.MatrixID = matrixID;
                setMUESRow(density, temp, ref r);

                LINAA.MUESRow destiny = dt.FirstOrDefault(o => o.Energy == r.Energy);
                if (destiny != null)
                {
                    dt.RemoveMUESRow(destiny);
                }
                dt.AddMUESRow(r);
            }

            reader.Close();
            reader.Dispose();
            reader = null;

            File.Delete(tempFile);
            //photon cross sections in g/cm2 and energies in keV, photon * density = mu (linear attenuation)
        }

        private static void setMUESRow(double density, string[] temp, ref LINAA.MUESRow r)
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
        private string png = ".png";
       
        private void makePic(ref MatrixRow m, ref XCOMPrefRow pref)
        {
            int i = 0;

            double start = pref.StartEnergy;
            double Totalend = pref.EndEnergy;

            string compositions = GetCompositionString(m.MatrixComposition);

          //  start = pref.StartEnergy;
            string   listOfenergies = MakeEnergiesList(Totalend - start, start, 2);

            string  Response = QueryXCOM(compositions, listOfenergies, m.MatrixName, true);

            if (string.IsNullOrEmpty(Response)) return;
            string aux = getPicTag(ref Response);

            if (aux.Contains("Error")) return;
           
            string     tempFile = _startupPath + m.MatrixID + png;

            string uriString = "https://physics.nist.gov/PhysRefData/Xcom/tmp/graph" + "_" + aux + png;
            Uri uri = new Uri(uriString);


            WebClient client = new WebClient();
                client.DownloadFile(uri, tempFile);
                client.Dispose();
                // Or you can get the file content without saving it:
                //     string htmlCode = client.DownloadString("http://yoursite.com/page.html");
                //...
           

          //  return File.ReadAllBytes(tempFile);
            
        }

        private int makeFiles(ref MatrixRow m, ref XCOMPrefRow pref)
        {
            int i = 0;

            double start = pref.StartEnergy;
            double Totalend = pref.EndEnergy;
            double step = pref.Steps;
            int NrEnergies = GetNumberOfLines(step, start, Totalend);
            //maximum number of energies per query
            int maxEnergies = 75;
            //increment in energy
            double delta = (maxEnergies * step);
            //end energy
            double end = start + delta;

            string listOfenergies = string.Empty;

            string compositions = GetCompositionString(m.MatrixComposition);

            // arr = Encoding.UTF8.GetBytes( DB.Properties.Resources.XCOM);
            if (pref.UseList)
            {
                byte[] arr = Interface.IPreferences.CurrentXCOMPref.ListOfEnergies;
                listOfenergies = Encoding.UTF8.GetString(arr);
                string[] energies = listOfenergies.Split('\n');
                NrEnergies = energies.Count();
                start = Convert.ToDouble(energies.First()) * 1000;
                end = Convert.ToDouble(energies.Last()) * 1000;
                Totalend = end;
            }
            string Response = string.Empty;
            string tempFile = string.Empty;

            while (end <= Totalend)
            {

                try
                {

                    if (!pref.UseList)
                    {
                        int lines = GetNumberOfLines(step, start, end);
                        listOfenergies = MakeEnergiesList(step, start, lines);
                    }

                    Response = string.Empty;
                    Response = QueryXCOM(compositions, listOfenergies);

                    tempFile = _startupPath + m.MatrixID + "." + i ;
                    File.WriteAllText(tempFile, Response);

              

                string text = "Working on: " + m.MatrixName + "\n";
                text += "Lines = " + NrEnergies + "\n";
                text += "Start (keV) = " + start + "\t";
                text += "End (keV) = " + end + "\n";
                string title = "Busy";
                if (string.IsNullOrEmpty(Response))
                {
                    title = "FAILED CONNECTION\n";
                }
                Interface.IReport.Msg(text, title);

                }
                catch (SystemException ex)
                {

                    Interface.IStore.AddException(ex);
                }

                start += delta;
                end += delta;

                i++;
            }

          


            return i;
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


    }
}