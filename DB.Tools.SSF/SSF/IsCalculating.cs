using System;
using System.Data;
using System.Linq;
using static DB.LINAA;
using static Rsx.Dumb.CalculateBase.TXT;
namespace DB.Tools
{
    public partial class MatSSF
    {
        private void finalize(ref UnitRow UNIT)
        {
            // if (!_bkgCalculation) {
            // Interface.IBS.CurrentChanged<SubSamplesRow>(UNIT.SubSamplesRow); }

            SubSamplesRow sample = UNIT.SubSamplesRow;
            reportFinished(UNIT.Name, sample.IrradiationCode);

            Interface.IStore.Save(ref UNIT);
            Interface.IStore.Save(ref sample);

            if (_processTable.Count == 0)
            {
                IsCalculating = false;
            }
        }

        private bool reportIfSampleHasErrors(ref UnitRow UNIT, bool hasError)
        {
            // DATA_OK = UNIT.Name;// inputDataOKTitle;

            if (hasError)
            {
                string inputDataNotOK = DATA_NOT_OK + "Sample " + UNIT.Name;
                UNIT.SetColumnError(Interface.IDB.Unit.NameColumn, DATA_NOT_OK + "Sample");
            }
            else
            {
                Interface.IReport.Msg(DATA_OK + UNIT.Name, DATA_OK_TITLE);
            }
            return !hasError;
        }

        public new bool IsCalculating
        {
            get
            {
                return Interface.IBS.IsCalculating;
            }

            set
            {
                Interface.IBS.IsCalculating = value;
                Interface.IBS.EnabledControls = true;

                reportToUser();
            }
        }

        private void reportErrors(int errorCount)
        {
            if (!_bkgCalculation)
            {
                //some missing
                string some = "Some ";
                if (errorCount == _units.Count())
                {
                    some = "All ";
                }
                // some += " were Skipped";
                Interface.IReport.Speak(some + ERROR_SPEAK);
            }
        }

        private void reportFinishedCancelled()
        {
            //user cancelled
            Interface.IReport.Msg(CANCELLED, CANCELLED_TITLE);
            if (!_bkgCalculation) Interface.IReport.Speak(CANCELLED);
            _processTable.Clear();
        }

        private void reportFinishedOK()
        {
            if (!_bkgCalculation)
            {
                //all good finished
                Interface.IReport.Msg(FINISHED_ALL, FINISHED_TITLE);
                Interface.IReport.Speak(FINISHED_ALL);
            }
        }

        private void reportRunning()
        {
            Interface.IReport.Msg(RUNNING, RUNNING_TITLE);
            if (!_bkgCalculation) Interface.IReport.Speak(RUNNING);
        }

        private void reportToUser()
        {
            //false means finished, cancelled or error
            if (!IsCalculating)
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
                    _resetProgress(0);
                }

                _callBack?.Invoke(null, EventArgs.Empty);
            }
            else //everything is runnning now...
            {
                reportRunning();
            }
        }
    }
}