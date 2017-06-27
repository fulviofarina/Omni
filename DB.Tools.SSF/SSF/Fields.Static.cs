using System.Collections.Generic;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected internal IList<LINAA.UnitRow> _units = null;

        /// <summary>
        /// Types of channels configurations
        /// </summary>
        protected static string CHILIAN_OK = "CKS calculations done for Sample ";

        protected static string DENSITY_MISMATCH = "Calculated density does not match input density by ";

        protected static char[] FORMULA_SEPARATOR = new char[] { ',', '(', '#', ')' };

        protected static string INPUT_NOTGEN = "Input metadata NOT generated for Sample ";
        protected static string MATSSF_ERROR = "MatSSF ERROR!";
        protected static string MATSSF_OK = "MatSSF is running OK for Sample ";
        protected static string MATSSF_TABLE_SEPARATOR = "------------------------------------------------------------------------";

        protected static string PROBLEMS_MATSSF = "Problems when running MatSSF for Sample ";
        protected static string PROBLEMS_MATSSF_EXTRA = ", having EXE FILE: ";

        protected static string RUNNING_MATSSF_TITLE = "MatSSF Running...";

        protected static string SELECT_SAMPLES = "Select the Samples to calculate by clicking the Color cell under the column 'OK' (set Red)";
    }
}