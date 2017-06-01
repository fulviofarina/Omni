using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected internal static char[] ch = new char[] { ',', '(', '#', ')' };
        protected internal static string cmd = "cmd";
        public static string EXEFILE = "matssf2.exe";
        public static string INPUT_EXT = ".in";
        public static string OUTPUT_EXT = ".out";
        protected internal static string startupPath = string.Empty;

        protected static string ERROR_SPEAK = "Calculations were cancelled because the proper input data is missing.\n"
         + "Please verify the sample data provided";

        protected static string ERRORS = "Calculations were Cancelled because the proper input data is missing.\n"
            + "Please verify the sample data provided, such as: composition, dimensions and neutron source parameters.";

        protected static string FINISHED = "Self-shielding calculations finished!";
        protected static string CANCELLED = "Self-shielding calculations were cancelled!";
        protected static string SELECT_SAMPLES = "Select the Samples to calculate by double-clicking the row header of the sample grid (at the right-hand panel)";

        protected static System.Collections.Hashtable processTable;
        protected static string STARTED = "Process started.\n Please be patient";
        protected EventHandler callBack = null;

        // private bool cancelCalculations = false;
        protected Interface Interface;

        protected Action<int> resetProgress;
        protected EventHandler showProgress;

        protected IList<LINAA.UnitRow> units = null;

        public bool IsCalculating
        {
            get
            {
           
                return Interface.IBS.IsCalculating;
            }

            set
            {
                Interface.IBS.IsCalculating = value;
                Interface.IBS.EnabledControls = true;
                bool valor = value;
                reportToUser(valor);
            }
        }

        private void reportToUser(bool valor)
        {
            //false means finished, cancelled or error
            if (!valor)
            {
                //no processes remaining
                if (processTable.Count == 0)
                {
                    //rows with errors
                    DataRow[] rowsInError = units
                        .Where(o => !CheckInputData(ref o))
                        .ToArray();

                    int errorCount = rowsInError.Count();
                    if (errorCount != 0)
                    {
                        reportErrors(errorCount);
                    }
                    else
                    {
                        reportFinishedOK();
                    }
                }
                else
                {
                    reportFinishedCancelled();
                }

                resetProgress(0);

                //callback
                callBack?.Invoke(null, EventArgs.Empty);

            }
            else //everything is runnning now...
            {
                reportRunning();
            }
        }

        private void reportRunning()
        {
            Interface.IReport.Msg(STARTED, "Running...");
            if (!bkgCalculation) Interface.IReport.Speak(STARTED);
        }

        private void reportFinishedCancelled()
        {
            //user cancelled
            Interface.IReport.Msg(CANCELLED, "Cancelled");
            if (!bkgCalculation) Interface.IReport.Speak(CANCELLED);
            processTable.Clear();
        }

        private void reportFinishedOK()
        {
         
            if (!bkgCalculation)
            {
                //all good finished
                Interface.IReport.Msg(FINISHED, "Finished");
                Interface.IReport.Speak(FINISHED);
            }
        }

        private void reportErrors(int errorCount)
        {
            if (!bkgCalculation)
            {
                //some missing
                string some = "Some of ";
                if (errorCount == units.Count)
                {
                    Interface.IReport.Msg(ERRORS, "All samples were Skipped");
                    some = "All the ";
                }
                else
                {
                    Interface.IReport.Msg(ERRORS, "Some samples were Skipped");
                }
                Interface.IReport.Speak(some + ERROR_SPEAK);
            }
        }
    }
}