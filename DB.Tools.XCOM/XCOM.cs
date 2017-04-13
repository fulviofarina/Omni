using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DB.Tools
{
    public class XCOM
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

        public static string MakeEnergiesList(int NrOfEnergies, double StartEnergy, double EndEnergy)
        {
            string str = string.Empty;

            double Energy = 0;
            double step = ((EndEnergy - StartEnergy) / (NrOfEnergies));
            for (int i = 0; i < NrOfEnergies; i++)
            {
                Energy = StartEnergy + (step * i);
                str = str + ((Energy * 0.001)).ToString() + "\n";
                //Energy += step;
            }
            return str;
        }

        public static string QueryXCOM(string MatrixComposition, int NrOfEnergies, double StartEnergy, double EndEnergy)
        {
            string str = MakeCompositionsList(MatrixComposition);
            string str2 = MakeEnergiesList(NrOfEnergies, StartEnergy, EndEnergy);

            string completo = string.Empty;

            if ((str != null) && (str2 != null))
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string s = "Formulae=" + str + "&Energies=" + str2;
                byte[] bytes = encoding.GetBytes(s);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

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
            }

            return completo;
        }

        public static LINAA.MUESDataTable FindMu(double density, ref StreamReader reader)
        {
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

            LINAA.MUESDataTable dt = new LINAA.MUESDataTable();

            String[] temp = null;

            for (int j = num; j <= num2; j++)
            {
                temp = strArray2[j].TrimStart().Split(' ');

                LINAA.MUESRow r = dt.NewMUESRow();

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
                dt.AddMUESRow(r);
            }

            reader.Close();
            reader.Dispose();
            reader = null;

            //photon cross sections in g/cm2 and energies in keV, photon * density = mu (linear attenuation)
            return dt;
        }

        /// <summary>
        /// Obtains the mass attenuation coefficients for total absorbtion without coherent
        /// scattering and compton and returns a table
        /// </summary>
        /// <param name="MatrixComposition"></param>
        /// <param name="NrOfEnergies">     </param>
        /// <param name="StartEnergy">      </param>
        /// <param name="EndEnergy">        </param>
        /// <returns></returns>
        public static System.Data.DataTable FindXCOMBasic(string MatrixComposition, int NrOfEnergies, double StartEnergy, double EndEnergy)
        {
            string str = MakeCompositionsList(MatrixComposition);
            string str2 = MakeEnergiesList(NrOfEnergies, StartEnergy, EndEnergy);

            System.Data.DataTable dt = new System.Data.DataTable();

            if ((str != null) && (str2 != null))
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string s = "Formulae=" + str + "&Energies=" + str2;
                byte[] bytes = encoding.GetBytes(s);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                requestStream.Dispose();
                requestStream = null;

                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                request = null;

                reader.ReadLine();
                reader.ReadLine();
                string[] strArray2 = reader.ReadToEnd().Split(new char[] { '\n' });
                responseStream.Close();
                responseStream.Dispose();
                responseStream = null;
                reader.Close();
                reader.Dispose();
                reader = null;

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

                dt.Columns.Add("Energy", typeof(double));
                dt.Columns.Add("Absorbtion", typeof(double));
                dt.Columns.Add("Compton", typeof(double));

                for (int j = num; j <= num2; j++)
                {
                    temp = strArray2[j].TrimStart().Split(' ');

                    System.Data.DataRow r = dt.NewRow();
                    r[0] = Convert.ToDouble(temp[0]) * 1000;  //gives the energy
                    r[1] = Convert.ToDouble(temp[7]);       //gives the total attenuation without coherent scattering
                    r[2] = Convert.ToDouble(temp[2]);
                    dt.Rows.Add(r);
                }
            }

            return dt;
        }

        /// <summary>
        /// Obtains the mass attenuation coefficients for total absorbtion without coherent
        /// scattering and compton and returns a path toRow the ascii file
        /// </summary>
        /// <param name="MatrixComposition"></param>
        /// <param name="NrOfEnergies">     </param>
        /// <param name="StartEnergy">      </param>
        /// <param name="EndEnergy">        </param>
        /// <returns></returns>
        public static void FindTim(string MatrixComposition, int NrOfEnergies, double StartEnergy, double EndEnergy, string path, bool append)
        {
            string str = MakeCompositionsList(MatrixComposition);
            string str2 = MakeEnergiesList(NrOfEnergies, StartEnergy, EndEnergy);

            System.IO.TextWriter writer = new System.IO.StreamWriter(path, append); //create fromRow file

            if ((str != null) && (str2 != null))
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string s = "Formulae=" + str + "&Energies=" + str2;
                byte[] bytes = encoding.GetBytes(s);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                requestStream.Dispose();
                requestStream = null;

                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                reader.ReadLine();
                reader.ReadLine();
                string[] strArray2 = reader.ReadToEnd().Split(new char[] { '\n' });
                responseStream.Close();
                responseStream.Dispose();
                responseStream = null;

                reader.Close();
                reader.Dispose();
                reader = null;

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

                double energy = 0;
                double absor = 0;
                double compton = 0;

                for (int j = num; j <= num2; j++)
                {
                    temp = strArray2[j].TrimStart().Split(' ');

                    // System.Data.DataRow r = dt.NewRow();
                    energy = Convert.ToDouble(temp[0]) * 1000;
                    absor = Convert.ToDouble(temp[7]);     //total attenuation coefficient without coherent scattering
                    compton = Convert.ToDouble(temp[2]);      //compton or incoherent scattering

                    writer.WriteLine(energy.ToString() + " " + absor + " " + compton);  //gives the energy
                }
            }

            writer.Close();
            writer.Dispose();
            writer = null;
        }

        public static void FindTim(string MatrixCompositionList, string EnergiesList, string path, bool append)
        {
            System.IO.TextWriter writer = new System.IO.StreamWriter(path, append); //create fromRow file

            if ((MatrixCompositionList != null) && (EnergiesList != null))
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string s = "Formulae=" + MatrixCompositionList + "&Energies=" + EnergiesList;
                byte[] bytes = encoding.GetBytes(s);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                requestStream.Dispose();
                requestStream = null;

                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                responseStream.Close();
                responseStream.Dispose();
                responseStream = null;

                reader.ReadLine();
                reader.ReadLine();
                string[] strArray2 = reader.ReadToEnd().Split(new char[] { '\n' });
                reader.Close();
                reader.Dispose();
                reader = null;

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

                double energy = 0;
                double absor = 0;
                double compton = 0;

                for (int j = num; j <= num2; j++)
                {
                    temp = strArray2[j].TrimStart().Split(' ');

                    // System.Data.DataRow r = dt.NewRow();
                    energy = Convert.ToDouble(temp[0]) * 1000;
                    absor = Convert.ToDouble(temp[7]);     //total attenuation coefficient without coherent scattering
                    compton = Convert.ToDouble(temp[2]);      //compton or incoherent scattering

                    writer.WriteLine(energy.ToString() + " " + absor + " " + compton);  //gives the energy
                }
            }

            writer.Close();
            writer.Dispose();
            writer = null;
        }
    }
}