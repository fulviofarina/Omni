using System;
using System.Data;
using System.Linq;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {

        string error = "Input data is NOT OK for ";
        string inputDataOK = "Input data is OK for Sample ";

        private bool reportIfSampleHasErrors(ref UnitRow UNIT, bool hasError)
        {
        
            inputDataOK += UNIT.Name;// inputDataOKTitle;

            if (hasError)
            {
           
                string inputDataNotOK = error + "Sample " + UNIT.Name;
                UNIT.SetColumnError(Interface.IDB.Unit.NameColumn, error + "Sample");
            }
            else
            {
                string inputDataOKTitle = "Checking data...";
                Interface.IReport.Msg(inputDataOK, inputDataOKTitle);
            }
            return !hasError;
        }

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

        private void reportErrors(int errorCount)
        {
            if (!_bkgCalculation)
            {
                //some missing
                string some = "Some of ";
                if (errorCount == _units.Count)
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

        private void reportFinishedCancelled()
        {
            //user cancelled
            Interface.IReport.Msg(CANCELLED, "Cancelled");
            if (!_bkgCalculation) Interface.IReport.Speak(CANCELLED);
            _processTable.Clear();
        }

        private void reportFinishedOK()
        {
            if (!_bkgCalculation)
            {
                //all good finished
                Interface.IReport.Msg(FINISHED, FINISHED_TITLE);
                Interface.IReport.Speak(FINISHED);
            }
        }

        private void reportRunning()
        {
            Interface.IReport.Msg(RUNNING, RUNNING_TITLE);
            if (!_bkgCalculation) Interface.IReport.Speak(RUNNING);
        }

        private void reportToUser(bool valor)
        {
            //false means finished, cancelled or error
            if (!valor)
            {
                //no processes remaining
                if (_processTable.Count == 0)
                {
                    //rows with errors
                    DataRow[] rowsInError = _units
                        .Where(o => !checkInputData(ref o))
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

                _resetProgress(0);

                //callback
                _callBack?.Invoke(null, EventArgs.Empty);
            }
            else //everything is runnning now...
            {
                reportRunning();
            }
        }
    }
}