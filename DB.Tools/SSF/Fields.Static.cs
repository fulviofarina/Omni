namespace DB.Tools
{
    public partial class MatSSF
    {
        public static string EXEFILE = "matssf2.exe";

        /// <summary>
        /// The input MatSSF file extension
        /// </summary>
        public static string INPUT_EXT = ".in";

        /// <summary>
        /// The output MatSSF file extension
        /// </summary>
        public static string OUTPUT_EXT = ".out";

        /// <summary>
        /// Types of channels configurations
        /// </summary>
        public static string[] TYPES = new string[] { "0 = Isotropic", "1 = Wire flat", "2 = Foil/wire ch. axis" };

        protected static string CANCELLED = "Self-shielding calculations were cancelled!";
        protected static string CLONING_ERROR_TITLE = "Code Cloning ERROR...";
        protected static string CLONING_OK = "Code cloning OK";

        protected static string CLONING_OK_TITLE = "Code cloned...";
        protected static string CMD = "cmd";
        protected static string DENSITY_MISMATCH = "Calculated density does not match input density by ";
        protected static string ERROR_SPEAK = "Calculations were cancelled because the proper input data is missing.\n"
                 + "Please verify the sample data provided";

        protected static string ERROR_TITLE = "ERROR";
        protected static string ERRORS = "Some samples were not calculated because the proper input data is missing.\n"
            + "Please verify the sample data provided, such as: composition, dimensions and neutron source parameters.";

        protected static string EXE_EXT = ".exe";
        protected static string FINISHED = "Self-shielding calculations finished!";
        protected static string FINISHED_TITLE = "Finished";
        protected static char[] FORMULA_SEPARATOR = new char[] { ',', '(', '#', ')' };
        protected static string INPUT_NOTGEN = "Input metadata NOT generated for Sample ";
        protected static string MATSSF_ERROR = "MatSSF ERROR!";
        protected static string MATSSF_OK = "MatSSF is running OK for Sample ";
        protected static string MATSSF_TABLE_SEPARATOR = "------------------------------------------------------------------------";
        protected static string NOTHING_SELECTED = "Oops, nothing was selected!";
        protected static string PROBLEMS_CLONING = "Problems when cloning code for ";
        protected static string PROBLEMS_MATSSF = "Problems when running MatSSF for Sample ";
        protected static string PROBLEMS_MATSSF_EXTRA = ", having EXE FILE: ";
        protected static string RUNNING = "Process started...";
        protected static string RUNNING_MATSSF_TITLE = "MatSSF Running...";
        protected static string RUNNING_TITLE = "Running...";
        protected static string SELECT_SAMPLES = "Select the Samples to calculate by double-clicking the row header of the sample grid (at the right-hand panel)";
    }
}