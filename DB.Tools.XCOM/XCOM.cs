using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Rsx.Dumb;
using static DB.LINAA;
namespace DB.Tools
{


    public partial class XCOM : CalculateBase
    {
        protected static string XCOM_ERROR = "Problems comunicating with XCOM";
        protected static string XCOM_TITLE = "XCOM did not answer the query";

        public void Calculate(bool? BKG)
        {
            Interface.IBS.Matrix.EndEdit();
            Interface.IBS.Compositions.EndEdit();

            IsCalculating = true;

            int position = 0;

            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;

            if (pref.Loop)
            {
                Interface.IBS.Matrix.MoveLast();
                position = Interface.IBS.Matrix.Position;
                Interface.IBS.Matrix.MoveFirst();
            }
            else position = Interface.IBS.Matrix.Position;

            _resetProgress?.Invoke(0);

            double Totalend = pref.EndEnergy;
            int NrEnergies = pref.Steps;  // STEP (in keV) for retrieving MUES for each keV

            string notepad = "notepad.exe";

            while (IsCalculating)
            {
                double start = pref.StartEnergy;

                double end = (start + NrEnergies);
                MatrixRow m = Interface.ICurrent.Matrix as MatrixRow;
                string fileName = m.MatrixName.Trim() + ".txt";
                string composition = m.MatrixComposition;
                string filePath = _startupPath + fileName;

                try
                {
                    //finds the MUEs for each 1keV, given start and Totalend energies, by NrEnergies (keV) steps.
                    m.RowError = string.Empty;

                    if (pref.ASCIIOutput)
                    {
                        // the error in Density column because density is not necessary
                        _resetProgress?.Invoke(2);

                        //        string energiesListPath = Interface.IStore.FolderPath + DB.Properties.Resources.XCOMEnergies;
                        byte[] arr = Interface.IPreferences.CurrentXCOMPref.ListOfEnergies;
                        string EnergiesList = string.Empty;
                        //          arr = Encoding.UTF8.GetBytes( DB.Properties.Resources.XCOM);
                        if (!pref.UseList) EnergiesList = Encoding.UTF8.GetString(arr);
                        else EnergiesList = DB.Properties.Resources.XCOM;

                        string compositionList = XCOM.MakeCompositionsList(composition);

                        FindTim(compositionList, EnergiesList, filePath, false);

                        _showProgress?.Invoke(null, EventArgs.Empty);

                        System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                        IO.Process(proceso, _startupPath, notepad, fileName, false, false, 1000);

                        _showProgress?.Invoke(null, EventArgs.Empty);

                    }
                    else
                    {

                        _resetProgress?.Invoke(NrEnergies);

                        bool goIn = (m.NeedsMUES || pref.Force);
                        goIn = !m.HasErrors() && goIn;

                        if (goIn)
                        {
                            string responde = string.Empty;
                            responde = GetMuesFromXCOM(ref start, Totalend, NrEnergies, ref end, composition, m.MatrixDensity, m.MatrixID);
                            if (string.IsNullOrEmpty(responde))
                            {
                                Interface.IReport.Msg(XCOM_ERROR, XCOM_TITLE);
                                break;
                            }
                        }
                    }

                    if (Interface.IBS.Matrix.Position == position) break;
                    else Interface.IBS.Matrix.MoveNext();
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(m, ex);
                    Interface.IStore.AddException(ex);
                }
            }

            IsCalculating = false;

        }

        public object[] GetDataToPlot()
        {
            MatrixRow m = (Interface.ICurrent.Matrix as MatrixRow);

            int matrixID = m.MatrixID;
            string name;
            double density;
            getNameDensity(out name, out density);
            string title = string.Empty;
            bool sql = SQL;
            double eh;//// = pref.EndEnergy;
            double el;// = pref.StartEnergy;
            bool logScale;
            getEnergies(out eh, out el, out logScale);
            bool specific = false;
            MUESDataTable mues = null;
            if (specific) mues = Interface.IPopulate.IGeometry.GetMUES(el, eh, matrixID);
            else mues = Interface.IPopulate.IGeometry.GetMUES(ref m, sql);

            DataColumn ene = mues.EnergyColumn;
            DataColumn mu = mues.MATNCSColumn;

            return new object[] { ene, mu,logScale,title };
        }

        public void getEnergies(out double eh, out double el, out bool logScale)
        {
            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;

            eh = pref.EndEnergy;
            el = pref.StartEnergy;

            logScale = pref.LogGraph;

        }
        public void getNameDensity(out string name, out double density)
        {
            MatrixRow m = (Interface.ICurrent.Matrix as MatrixRow);
            XCOMPrefRow pref = Interface.IPreferences.CurrentXCOMPref;
            name = m.MatrixName;
            density = m.MatrixDensity;
            int matrixID = m.MatrixID;

        }

        public bool SQL
        { 
            get
            {
            return !(bool)Interface?.IPreferences.CurrentPref.Offline;
            }
}
        protected internal string GetMuesFromXCOM(ref double start, double Totalend, int NrEnergies, ref double end, string composition, double density, int matrixID)
        {
            string headerWithContent = string.Empty;

            try
            {
                string path = _startupPath + matrixID + ".";
                string ext = ".txt";

                int i = 0;
                string Response = string.Empty;


                MatrixRow m = (Interface.ICurrent.Matrix as MatrixRow);

                MUESDataTable mu = new MUESDataTable();

                while (IsCalculating)
                {
                    double START = start;
                    double END = end;

                    Response = QueryXCOM(composition, NrEnergies, START, END);
                    if (i == 0) headerWithContent = Response; //keep the header of the response

                    string tempFile = path + i + ext;
              

                      _callBack?.Invoke(new object[] { tempFile, Response }, EventArgs.Empty);
                    StreamReader reader = new StreamReader(tempFile);

                    FindMu(density, ref reader, ref mu, matrixID);

             
                    _showProgress?.Invoke(null, EventArgs.Empty);

             


                    if (end >= Totalend) break;

                    start += NrEnergies;
                    end = start + NrEnergies;
                    i++;
                }


                Interface.IDB.MUES.Merge(mu);

                Action action = delegate
                {
                    if (SQL)
                    {
                        Interface.IStore.InsertMUES(ref mu, m.MatrixID);
                    }
                    byte[] arr = Rsx.Dumb.Tables.MakeDTBytes(ref mu);
                    m.XCOMTable = arr;
                };
                Action callBack = delegate
                {

                };

                ILoader ld = new Loader();
                ld.Set(action, callBack, null);
                ld.RunWorkerAsync();

            

            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }

            return headerWithContent;
        }



        protected internal  Interface Interface;
        public XCOM() : base()
        {

        }
        public void Set(ref Interface inter)
        {
            Interface = inter;

        }

        bool calculating = false;
     //   private System.Web webBrowser;

        public new bool IsCalculating
        {
      
            get
            {
                return calculating;
            }
            set
            {
                calculating = value;
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
            IList<string[]> str0 = RegEx.StripComposition(MatrixComposition);
            //STRING WAS DECODED INTO THE LIST ls
            // string str = RegEx.StripMoreComposition(ref str0);
            //    str = str.Replace('\n',' ');
            string str = string.Empty;
            foreach (string[] str5 in str0)
            {
                str += str5[0] + " " + str5[1] + "\n";
            }
            // string str1 = MakeCompositionsList(MatrixComposition);
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

        public static void FindMu(double density, ref StreamReader reader, ref LINAA.MUESDataTable dt, int matrixID)
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

            String[] temp = null;

            for (int j = num; j <= num2; j++)
            {
                temp = strArray2[j].TrimStart().Split(' ');
                LINAA.MUESRow r = dt.NewMUESRow();
                r.MatrixID = matrixID;
                SetMUESRow(density, temp, ref r);
                dt.AddMUESRow(r);
            }

            reader.Close();
            reader.Dispose();
            reader = null;

            //photon cross sections in g/cm2 and energies in keV, photon * density = mu (linear attenuation)
        }

        private static void SetMUESRow(double density, string[] temp, ref LINAA.MUESRow r)
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