using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected internal static char[] ch = new char[] { ',', '(', '#', ')' };
        protected internal static string cmd = "cmd";
        protected internal static string exefile = "matssf2.exe";
        protected internal static string inPutExt = ".in";
        protected internal static string outPutExt = ".out";
        protected internal static string startupPath = string.Empty;
        protected static string ERRORS = "The self-shielding calculations were cancelled because the proper input data is missing.\n"
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

                if (!value)
                {

                    //no processes remaining
                    if (processTable.Count == 0)
                    {
                        //rows with errors
              
                        DataRow[] rowsInError = units.Where(o => !CheckInputData(o)).ToArray();
                        int errorCount = rowsInError.Count();
                        if (errorCount!=0)
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

                            Interface.IReport.Speak(some + ERRORS);
                        }
                        else
                        {
                            //all good finished
                            Interface.IReport.Msg(FINISHED, "Finished");
                            Interface.IReport.Speak(FINISHED);
                        }
                    }
                    else
                    {
                        //user cancelled
                        Interface.IReport.Msg(CANCELLED, "Cancelled");
                        Interface.IReport.Speak(CANCELLED);
                        processTable.Clear();
                    }
                    resetProgress(0);
                }
                else
                {
                    Interface.IReport.Msg(STARTED, "Running...");
                    Interface.IReport.Speak(STARTED);
                }
            }
        }

    
    }
}