﻿using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static DB.LINAA;
using static DB.Tools.XCOM.URIS;
namespace DB.Tools
{


 

    public partial class XCOM
    {

        public static class Engine
        {

            private static CultureInfo culture = new CultureInfo("en"); // I'm assuming german here.

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

            /// <summary>
            /// NOT USED?
            /// </summary>
            /// <param name="XCOMResponse"></param>
            /// <param name="elements"></param>
            /// <returns></returns>
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
                double lines = (EndEnergy - StartEnergy);
                lines = lines / (double)step;

                int NrOfEnergies = Convert.ToInt32(Math.Ceiling((lines)));
                return NrOfEnergies;
            }

            public static string MakeEnergiesList(double step, double StartEnergy, int NrOfEnergies)
            {
                string str = string.Empty;

                double Energy = 0;

                for (int i = 0; i < NrOfEnergies; i++)
                {
                    Energy = (StartEnergy + (step * i))*0.001;
                    str = str + (Energy).ToString().Replace(RSX.COLON,RSX.DOT) + "\n";
                }
                return str;
            }

            public static string QueryXCOM(string composition, string energies, string name = "default matrix", bool picture = false)
            {
                byte[] bytes = null;

                if ((composition != null) && (energies != null))
                {
                    string s = "Formulae=" + composition + "&Energies=" + energies;
                    // string s = s2;
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
                    completo = getHTTPQuery(bytes, picture);
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



            public static byte[] ListOfEnergiesBytes = null;

            public static void GetPicture(ref string Response, string tempFile)
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
                string aux = getPicTag(ref Response);
                if (aux.Contains(RSX.ERROR)) return;

                string uriString = URIS.PIC_URI + "_" + aux + RSX.PIC_EXT;
                Uri uri = new Uri(uriString);
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileAsync(uri, tempFile);
                }
                // client.DownloadFile(uri, tempFile); client.Dispose();
            }

            private static string getHTTPQuery(byte[] bytes, bool picture = false)
            {
                string completo;
                // HttpWebRequest request =
                // (HttpWebRequest)WebRequest.Create("https://physics.nist.gov/cgi-bin/Xcom/xcom2");
                // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3");
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

            public static int GetMUESFromNIST(double density, string path, ref MUESDataTable dt, int matrixID, int numberofFiles)
            {
                int added = 0;

                string tempFile = path + matrixID + RSX.DOT + "N" + numberofFiles;
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

                    addMUESRow(density, ref dt, matrixID, ref temp);

                    added++;
                }

                reader.Close();
                reader.Dispose();
                reader = null;

                IO.DeleteIfExists(tempFile);

                return added;
                //photon cross sections in g/cm2 and energies in keV, photon * density = mu (linear attenuation)
            }

            private static void addMUESRow(double density, ref MUESDataTable dt, int matrixID, ref string[] temp)
            {
                MUESRow r = dt.NewMUESRow(); //dt.FirstOrDefault(o => o.Energy ==null);
                r.MatrixID = matrixID;
                setMUESRow(density, ref temp, ref r);
                removeMUESRow(ref dt, r.Energy, matrixID);
                dt.AddMUESRow(r);
            }

            private static void removeMUESRow(ref MUESDataTable dt, double energy, int matrixId)
            {
                MUESRow destiny = dt.FirstOrDefault(o => o.Energy == energy && matrixId == o.MatrixID);
                if (destiny != null)
                {
                    dt.RemoveMUESRow(destiny);
                }
            }

            public static void UseCustomList(out double start, out double Totalend, out double end, out string listOfenergies, out int NrEnergies)
            {
                double factor = 1000;

                listOfenergies = Encoding.UTF8.GetString(ListOfEnergiesBytes);
                string[] energies = listOfenergies.Split('\n');
                NrEnergies = energies.Count();
                start = Convert.ToDouble(energies.First()) * factor;
                string lastEnergy = energies.Last();
                if (NrEnergies > RSX.MAXENERGIES)
                {
                    lastEnergy = energies.ElementAt(RSX.MAXENERGIES - 1);
                }
                end = Convert.ToDouble(lastEnergy) * factor;
                Totalend = end;
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

            private static void setMUESRow(double density, ref string[] temp, ref MUESRow r)
            {

             
              //  double number = Double.Parse("202.667,40", culture);
                long length = temp.LongLength;

                if (length == 8 || length == 10)
                {
                    if (length == 8)
                    {
                        r.Edge = String.Empty;
                    }
                    else if (temp.LongLength == 10)
                    {
                        r.Edge = temp[0] + temp[1];
                    }

                    r.Energy = Double.Parse(temp[length - 8],culture) * 1000;
                    r.Density = density;
                    r.MACS = Double.Parse(temp[length - 7], culture);
                    r.MAIS = Double.Parse(temp[length - 6], culture);
                    r.PE = Double.Parse(temp[length - 5], culture);
                    r.PPNF = Double.Parse(temp[length - 4], culture);
                    r.PPEF = Double.Parse(temp[length - 3], culture);
                    r.MATCS = Double.Parse(temp[length - 2], culture);
                    r.MATNCS = Double.Parse(temp[length - 1], culture);

                }
            }
        }
    }

}