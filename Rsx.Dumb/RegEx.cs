using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

///FULVIO
namespace Rsx.Dumb
{
  


    public static partial class RegEx
    {
        public static void DecomposeFormula(string formula, ref List<string> elements, ref List<string> moles)
        {
            Regex re = new Regex("[0-9]");
            string[] result = re.Split(formula);
            foreach (string s in result) if (!s.Equals(string.Empty)) elements.Add(s); // gives elements

            //NUMBERS
            Regex re2 = new Regex("[a-z]", RegexOptions.IgnoreCase);
            result = re2.Split(formula);
            foreach (string s in result) if (!s.Equals(string.Empty)) moles.Add(s); // gives moles
        }

        public static IList<string[]> StripComposition(string composition)
        {
            IList<string[]> ls = null;
            // if (Rsx.EC.IsNuDelDetch(this)) return ls;
            if (string.IsNullOrEmpty(composition)) return ls;

            string matCompo = composition.Trim();

            //   if (matCompo.Contains(';')) matCompo = matCompo.Replace(';', ')');///

            string[] strArray = null;
            if (matCompo.Contains('\t')) matCompo = matCompo.Replace('\t', ' ');
            if (matCompo.Contains('\n')) matCompo = matCompo.Replace('\n', ' ');
            if (matCompo.Contains('#')) strArray = matCompo.Split('#');
            else if (matCompo.Contains(';')) strArray = matCompo.Split(';');
            //  else if (matCompo.Contains('\n')) strArray = matCompo.Split('\n');
            else strArray = new string[] { matCompo };
            strArray = strArray.Select(o => o.Trim()).Where(o => !string.IsNullOrEmpty(o)).ToArray();

            ls = new List<string[]>();

            for (int index = 0; index < strArray.Length; index++)
            {
                string[] strArray2 = strArray[index].Trim().Split(' ').Where(o => !string.IsNullOrEmpty(o)).ToArray();
                //      string formula = strArray2[0].Trim().Replace("#", null);
                string formula = strArray2[0].Trim();
                string quantity = strArray2[1].Trim();

                string[] formCompo = new string[] { formula, quantity };
                ls.Add(formCompo);
            }

            return ls;
        }

        /// <summary>
        /// Strips the formula into elements and moles
        /// </summary>
        /// <param name="ls"></param>
        public static string StripMoreComposition(ref IList<string[]> ls)
        {
            string buffer = string.Empty;
            //matSSF buffer will cointain the snippet for the Matrix Content in MatSSF

            foreach (string[] formulaQuantity in ls)
            {
                //to auxiliary store elements and moles
                List<string> elements = new List<string>();
                List<string> moles = new List<string>();

                //decomposes Al2O3 into Al 2 O 3  (element and mole)
                DecomposeFormula(formulaQuantity[0], ref elements, ref moles);

                //modified formula
                string modified_formula = string.Empty;
                for (int z = 0; z < elements.Count; z++)
                {
                    modified_formula += elements[z] + " ";
                    if (moles.Count != 0) modified_formula += moles[z] + " ";
                }
                //Decomposed into Al 2 O 3  100

                //full MATSSF Input Data for the provided Matrix Information
                buffer += modified_formula + "\n";
                buffer += formulaQuantity[1] + "\n";
            }

            return buffer;
        }
    }

  
}