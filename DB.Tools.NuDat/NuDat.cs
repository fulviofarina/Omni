using System;
using System.IO;
using System.Net;

namespace DB.Tools
{
    public class NuDat
    {
        public static string GetIsotopeUrl(string isoName)
        {
            // Link to Internet!!!
            string isoNr = isoName.Split('-')[1].Replace('m', ' ').Trim().ToUpper();
            string ele = isoName.Split('-')[0].Trim().ToUpper();
            return "http://www.nndc.bnl.gov/nudat2/decaysearchdirect.jsp?nuc=" + isoNr + ele + "&unc=nds";
        }

        public static string GetElementUrl(string element)
        {
            return "http://en.wikipedia.org/wiki/" + element.Trim();
        }

        public static string Query(string isotope)
        {
            string uri = GetIsotopeUrl(isotope);

            string completo = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

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

        public static LINAA.YieldsDataTable HtmlToTable(string htmltext, string isotope)
        {
            string[] res = htmltext.Split('\n');
            bool inDS = false;

            LINAA.YieldsDataTable ydt = new LINAA.YieldsDataTable();

            LINAA.YieldsRow aux = null;
            int i = 0;
            foreach (string e in res)
            {
                if (e.Contains("<p><u>Dataset"))
                {
                    i++;
                }
                if (e.Contains("</table>") && inDS)
                {
                    inDS = false;
                }
                if (e.Contains("<p><u>Gamma") || inDS)
                {
                    inDS = true;
                    if (e.Contains("<td nowrap align=left") && !e.Contains("Parent"))
                    {
                        string comm = e.Substring(e.IndexOf('>') + 1);
                        string field = string.Empty;
                        bool makenull = false;
                        if (aux == null)
                        {
                            aux = ydt.NewYieldsRow();
                        }
                        if (aux.IsCommentNull() && !comm.Contains("&nbsp;"))
                        {
                            int o = comm.IndexOf('<');
                            if (o == 0) aux.Comment = string.Empty;
                            else aux.Comment = comm.Substring(0, o).Trim();
                        }
                        else if (aux.IsEnergyNull())
                        {
                            field = ((LINAA.YieldsDataTable)aux.Table).EnergyColumn.ColumnName;
                        }
                        else if (aux.IsYieldNull())
                        {
                            field = ((LINAA.YieldsDataTable)aux.Table).YieldColumn.ColumnName;
                        }
                        else if (aux.IsDoseNull())
                        {
                            field = ((LINAA.YieldsDataTable)aux.Table).DoseColumn.ColumnName;
                            makenull = true;
                        }

                        if (!string.IsNullOrWhiteSpace(field))
                        {
                            comm = comm.Replace("&nbsp;", null).Replace("<i>", null).Replace("</i>", null).Trim();
                            int o = comm.IndexOf('<');
                            if (o <= 0) aux[field] = 0;
                            else
                            {
                                comm = comm.Substring(0, o);
                                string[] yu = null;
                                if (comm.Contains("%")) yu = comm.Split('%');
                                else if (comm.Contains(" "))
                                {
                                    yu = comm.Split(' ');
                                }
                                if (yu != null)
                                {
                                    aux[field] = Convert.ToDouble(yu[0].Trim());
                                    aux[field + "Unc"] = yu[1].Trim();
                                }
                            }

                            field = string.Empty;
                            if (makenull)
                            {
                                aux.Iso = i.ToString();
                                ydt.AddYieldsRow(aux);
                                aux = null;
                            }
                        }
                    }
                }
            }

            double maxEne = 0;
            foreach (LINAA.YieldsRow y in ydt)
            {
                // double ene = Convert.ToDouble(y.Energy);
                int j = Convert.ToInt32(y.Iso);
                if (i - j == 2) y.Iso = isotope + "m2";
                else if (i - j == 1) y.Iso = isotope + "m";
                else if (i - j == 0) y.Iso = isotope;
            }

            return ydt;
        }
    }
}