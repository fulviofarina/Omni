using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

///FULVIO
namespace Rsx.Dumb
{
    public interface ISetteable
    {
        void SetParent<T>(ref T rowParent, object[] args = null);
    }
    public interface ICalculableRow
    {
        bool IsBusy { set; get; }
        bool ToDo { set; get; }

    }
    public interface IRow
    {
        void Check();

        bool HasErrors();

        void Check(DataColumn Column);
    }
    public interface IColumn
    {
        // void DataColumnChanged(object sender, DataColumnChangeEventArgs e);

        IEnumerable<DataColumn> ForbiddenNullCols
        {
            get;
        }
    }


   

    public class CalculateBase
    {

        public static class TXT
        {

            public static string CANCELLED = "Self-shielding calculations were cancelled!";
            public static string CANCELLED_TITLE = "Cancelled";
            public static string CLONING_ERROR_TITLE = "Code Cloning ERROR...";
            public static string CLONING_OK = "Code cloning OK";
            public static string CLONING_OK_TITLE = "Code cloned...";
            public static string CMD = "cmd";
            public static string DATA_NOT_OK = "Input data is NOT OK for ";
            public static string DATA_OK = "Input data is OK for Sample ";
            public static string DATA_OK_TITLE = "Checking data...";
            public static string DONE = "DONE!";
            public static string ERROR_SPEAK = "Sample calculations were cancelled because the proper input data is missing.\n"
                + "Please verify the sample data provided";

            public static string ERROR_TITLE = "ERROR";
            public static string ERRORS = "Some samples were not calculated because the proper input data is missing.\n"
                + "Please verify the sample data provided, such as: composition, dimensions and neutron source parameters.";

            public static string EXE_EXT = ".exe";
            public static string EXEFILE = "matssf2.exe";

            public static string FINISHED_ALL = "Self-shielding calculations finished!";

            public static string FINISHED_SAMPLE = "Finished calculations for sample ";

            public static string FINISHED_TITLE = "Finished";

            /// <summary>
            /// The input MatSSF file extension
            /// </summary>
            public static string INPUT_EXT = ".in";

            public static string NOTHING_SELECTED = "Oops, nothing was selected!";

            /// <summary>
            /// The output MatSSF file extension
            /// </summary>
            public static string OUTPUT_EXT = ".out";
            public static string PROBLEMS_CLONING = "Problems when cloning code for ";
            public static string RUNNING = "Process started...";
            public static string RUNNING_TITLE = "Running...";

        }
        public bool IsCalculating
        {
            get;
            set;
        }
        public  void Set(string path, EventHandler callBackMethod = null, Action<int> resetProg = null, EventHandler showProg = null)
        {
         
            _startupPath = path;

            _showProgress = showProg;

            _resetProgress = resetProg;

            _callBack = callBackMethod;

            if (_processTable == null) _processTable = new System.Collections.Hashtable();
        }

        protected internal string _cachePath = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\";


        protected internal static System.Collections.Hashtable _processTable;
        protected internal bool _bkgCalculation = false;
        protected internal EventHandler _callBack = null;
        protected internal Action<int> _resetProgress;
        protected internal EventHandler _showProgress;
        protected internal string _startupPath = string.Empty;



        
       
        
        public string StartupPath
        {
            get { return _startupPath; }
            set { _startupPath = value; }
        }
        public CalculateBase()
        {
        }
    }
}