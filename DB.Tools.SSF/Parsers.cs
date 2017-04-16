using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;

namespace DB.Tools
{
    public partial class MatSSF
    {
        /// <summary>
        /// Fills the UnitRow with data from an array extracted from the OUTPUT MatSSF File
        /// </summary>
        /// <param name="array">Output file extracted array</param>
        public static void fillUnitWithText(ref IEnumerable<string> array)
        {
            string aux = string.Empty;
            string Gt = string.Empty;
            string Mdens = string.Empty;
            string MCL = string.Empty;
            string EXS = string.Empty;
            string PXS = string.Empty;

            try
            {
                string densityUnit = "[g/cm3]";
                string cmUnit = "[cm]";
                string invcmUnit = "[1/cm]";

                aux = "Material density";
                Dumb.SetField(ref Mdens, ref array, aux, densityUnit);
                aux = "G-thermal";
                Dumb.SetField(ref Gt, ref array, aux, string.Empty);
                aux = "Mean chord length";
                Dumb.SetField(ref MCL, ref array, aux, cmUnit);
                aux = "Escape x.sect.";
                Dumb.SetField(ref EXS, ref array, aux, invcmUnit);
                aux = "Potential x.sect.";
                Dumb.SetField(ref PXS, ref array, aux, invcmUnit);
            }
            catch (SystemException ex)
            {
                //  (this.Table.DataSet as LINAA).AddException(ex);

            }

            UNIT.FillWith(Mdens, Gt, EXS, MCL, PXS);
        }


        private static char[] ch = new char[] { ',', '(', '#', ')' };
        /// <summary>
        /// Types of channels configurations
        /// </summary>
        public static string[] Types = new string[] { "0 = Isotropic", "1 = Wire flat", "2 = Foil/wire ch. axis" };

        private static string getChannelCfg()
        {
            string chCfg = string.Empty;

            chCfg = UNIT.ChCfg[0] + "," + UNIT.ChDiameter + "," + UNIT.ChLength;

            return chCfg;
        }

        private static string getDecomposedMatrix()
        {
            string buffer = string.Empty;
            if (UNIT.Content == null) return buffer;
            if (string.IsNullOrWhiteSpace(UNIT.Content)) return buffer;

            string[] compositions = null;

            compositions = UNIT.Content.Split(ch[0]);

            int i = 0;
            int z = 0;

            for (i = 0; i < compositions.Length; i++)
            {
                string[] formula_weight = compositions[i].Split(ch[1]); // split the formula-weight because of '('
                string formula = formula_weight[0].Replace(ch[2].ToString(), null); // formula no spaces
                string weightpercent = formula_weight[1].Replace("%" + ch[3], null); //weight percent
                formula = formula.Trim();
                weightpercent = weightpercent.Trim();

                string modified_formula = string.Empty;

                //TEXT
                List<string> elements = new List<string>();
                List<string> moles = new List<string>();

                decomposeFormula(formula, ref elements, ref moles);

                for (z = 0; z < elements.Count; z++)
                {
                    modified_formula = modified_formula + elements[z] + " ";
                    if (moles.Count != 0) modified_formula = modified_formula + moles[z] + " ";
                }

                buffer += modified_formula + "\n";
                buffer += weightpercent + "\n";
            }
            return buffer;
        }

        private static bool writeFile(string buffer, string fileInput)
        {
            System.IO.TextWriter writer = new System.IO.StreamWriter(fileInput, false); //create fromRow file
            writer.Write(buffer);
            writer.Close();
            writer = null;
            return System.IO.File.Exists(fileInput);
        }

        private static void fillTable(IList<string> list)
        {
            string[] content = null;

            string separator = "------------------------------------------------------------------------";
            int sep = list.IndexOf(separator);
            list = list.Skip(sep + 1).ToList();
            IEnumerable<LINAA.MatSSFRow> ssfs = UNIT.GetMatSSFRows();


            foreach (LINAA.MatSSFRow m in ssfs)
            {
                m.Delete();
            }

            foreach (string item in list)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;

                try
                {
                    content = item.Substring(10).Trim().Split(' ');
                    content = content.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
                    // Z, the element and A data
                    string[] ZEl = item.Substring(0, 10).Split('-');
                    ZEl[2] = ZEl[2].Trim();
                    Int16 A = Convert.ToInt16(ZEl[2]);
                    //interested only in the isotopes
                    if (A > 0)
                    {
                        ZEl[0] = ZEl[0].Trim();// Z
                        ZEl[1] = ZEl[1].Trim(); // Element

                        setMatSSFRow( content, ZEl);
                    }
                }
                catch (SystemException ex)
                {
                }
            }
        }

        private static void decomposeFormula(string formula, ref List<string> elements, ref List<string> moles)
        {
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("[0-9]");

            foreach (string s in re.Split(formula)) if (!s.Equals(string.Empty)) elements.Add(s); // gives elements

            //NUMBERS
            System.Text.RegularExpressions.Regex re2 = new System.Text.RegularExpressions.Regex("[a-z]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (string s in re2.Split(formula)) if (!s.Equals(string.Empty)) moles.Add(s); // gives moles
        }

        private static void setMatSSFRow( string[] content, string[] ZEl)
        {
            LINAA.MatSSFRow m = null;
            string radioisotope = ZEl[1] + "-" + (Convert.ToInt32(ZEl[2]) + 1).ToString();
            string targetIsotope = ZEl[1] + "-" + ZEl[2];

            //find
            LINAA.SubSamplesRow sample = UNIT.SubSamplesRow;

            m = Table.FirstOrDefault(o => o.UnitID == UNIT.UnitID && targetIsotope.Equals(o.TargetIsotope));
            if (m==null && sample != null)
            {
                int sampleID = sample.SubSamplesID;
                m = Table.FirstOrDefault(o => o.SubSamplesID == sampleID && targetIsotope.Equals(o.TargetIsotope));
            }

            if (m == null)
            {
                m = Table.NewMatSSFRow();
                m.UnitID = UNIT.UnitID;

                Table.AddMatSSFRow(m);

                if (sample != null) m.SubSamplesID = sample.SubSamplesID;
                else m.SubSamplesID = 0;
            }

            m.RadioIsotope = radioisotope;
            m.TargetIsotope = targetIsotope;
            m.SSF = -1;

            m.GFast = -1;
            m.SigB = Convert.ToDouble(content[3]);
            m.Weight = Convert.ToDouble(content[1]);
            m.ND = Convert.ToDouble(content[2]);
            if (content.Length == 6)
            {
                m.SSF = Convert.ToDouble(content[4]);
                m.GFast = Convert.ToDouble(content[5]);
            }
            else if (content.Length == 5) m.GFast = Convert.ToDouble(content[4]);
        }
    }
}