using Rsx.Dumb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static DB.LINAA;
using static DB.Tools.XCOM.XCOMTXT;
using static DB.Tools.XCOM.Engine;

namespace DB.Tools
{
    
    public partial class XCOM 
    {

     

        private bool reportProgress(int x, string matrixName)
        {
            string msg = "Matrix: " + matrixName + "\nProgress:\t" + x.ToString() + " %\n";
            string title = "Finished with...";
            bool finito = (x == 100);
            if (!finito)
            {
                title = "Working on...";
            }
            reporter(msg, title, x, false);
            return finito;
        }

        private void updateLoaders(EventHandler runWorker, int matrixID)
        {
            if (loaders.Contains(matrixID))
            {
                loaders.Remove(matrixID);
            }
            if (loaders.Count == 0 && IsCalculating)
            {
                IsCalculating = false;
            }
            else if (loaders.Count != 0)
            {
                if (IsCalculating)
                {
                    reporter(CALCULATING_MSG + loaders.Count, CALCULATING_TITLE, true, false);
                }
                runWorker.Invoke(null, EventArgs.Empty);
            }
        }
        private int addToLoaders(ref MatrixRow m, Action<int> report, Action callBack, double start, double Totalend, double step, bool accumulate, bool useList = true)
        {
            IList<Action> actions = generateEngineActions(ref m, start, Totalend, step, offline, accumulate, useList);

            if (actions.Count == 0)
            {
                throw new SystemException("The Actions list for the Matrix is empty");
            }
            ILoader ld = new Loader();
            ld.Set(actions, callBack, report);
            loaders.Add(m.MatrixID, ld);

            return actions.Count;
        }
        private Action setMainAction(int matrixID, int numberOfFiles, string listOfenergies, string compositions, string labelName, double start, double end)
        {
            Action action = delegate
            {
                try
                {
                    if (!IsCalculating) return;

                    string Response = string.Empty;
                    string tempFile = string.Empty;

                    Response = QueryXCOM(compositions, listOfenergies, labelName, false);

                    string range = RSX.DOT + RSX.ENUME + numberOfFiles;

                    tempFile = _cachePath + matrixID + range;

                    IO.WriteFileText(tempFile, Response, false);





                    string startdec = String.Format("{0:0.00}", start).Replace(",", RSX.DOT);
                    string enddec = String.Format("{0:0.00}", end).Replace(",", RSX.DOT);


                    range += RSX.DOT + startdec + RSX.DIVIDER + enddec;
                    MakefullPic(compositions, end, start, matrixID, labelName, range);
                }
                catch (SystemException ex)
                {
                    exceptionAdder(ex);
                }
            };
            return action;
        }

        private IList<Action> generateEngineActions(ref MatrixRow matrix, double start, double totalEnd, double step, bool offline, bool accumulate, bool useList = false)
        {
            //finds the MUEs for each 1keV, given start and Totalend energies, by NrEnergies (keV) steps.

            List<Action> ls = new List<Action>();

            MatrixRow m = matrix;

            int numberOfFiles = 0;
            double initialStart = start;

            double delta, end;

            //maximum number of energies per query
            int NrEnergies = GetNumberOfLines(step, start, totalEnd);

            int nrOfQueries = 1;

            delta = 0;
            if (RSX.MAXENERGIES > NrEnergies)
            {
                delta = (NrEnergies * step);
                totalEnd = start + delta;
            }
            else
            {
                nrOfQueries = Convert.ToInt32(Math.Ceiling(((double)NrEnergies / (double)RSX.MAXENERGIES)));
                delta = (RSX.MAXENERGIES * step);
            }
            end = start + delta;

            string listOfenergies, compositions;
            listOfenergies = string.Empty;

            if (useList)
            {
                UseCustomList(out start, out totalEnd, out end, out listOfenergies, out NrEnergies);
            }

            compositions = GetCompositionString(m.MatrixComposition);

            while (nrOfQueries > 0)
            {
                //finishing next round

                if (!useList)
                {
                    if (nrOfQueries - 1 == 0) end = totalEnd + step;

                    int lines = GetNumberOfLines(step, start, end);
                    listOfenergies = MakeEnergiesList(step, start, lines);

                    if (nrOfQueries - 1 == 0) end = totalEnd;
                }

                string labelName = m.MatrixName + ": " + start + " to " + end + " keV";
                Action action = setMainAction(m.MatrixID, numberOfFiles, listOfenergies, compositions, labelName, start, end);

                if (!useList)
                {
                    start = end;
                    end += delta;
                    numberOfFiles++;
                }



                nrOfQueries--;

                ls.Add(action);
            }

            numberOfFiles--;

            Action action2 = delegate
            {
                if (!IsCalculating) return;

                MUESDataTable mu = Interface.IPopulate.IGeometry.GetMUES(ref m, !offline);

                while (numberOfFiles >= 0)
                {
                    GetMUESFromNIST(m.MatrixDensity, _cachePath, ref mu, m.MatrixID, numberOfFiles);
                    numberOfFiles--;
                }

                Interface.IStore.SaveMUES(ref mu, ref m, !offline);
            };

            Action action3 = delegate
            {
                if (!IsCalculating) return;

                int matrixID = m.MatrixID;
                string mstrixName = m.MatrixName;

                double max = 1e8;
                double maxDisplay = max * 1e-6;
                double min = 1;
                string labelName = string.Empty;
                string range = string.Empty;

                labelName = mstrixName + ": " + initialStart.ToString() + " to " + totalEnd.ToString() + " keV";
                range = ".LAST";
                MakefullPic(compositions, totalEnd, initialStart, matrixID, labelName, range);

                labelName = mstrixName + ": " + min.ToString() + " keV to " + maxDisplay.ToString() + " GeV";
                range = ".FULL";
                MakefullPic(compositions, max, min, matrixID, labelName, range);
            };

            ls.Add(action2);
            ls.Add(action3);

            // m.IsBusy = true;

            return ls;
        }

    }


    public partial class XCOM: XCOMBase
    {
        public XCOM() : base()
        {
           

        }
        public void Calculate(bool? BKG)
        {
            double start = pref.StartEnergy;
            double Totalend = pref.EndEnergy;
            double step = pref.Steps;
            bool useList = pref.UseList;
            bool accumulate = pref.AccumulateResults;

            ListOfEnergiesBytes = pref.ListOfEnergies;

            if (rows.Count == 0)
            {
                reporter(NOMATRIX_ERROR, NOMATRIX_TITLE, false, false);
                return;
            }

            _resetProgress?.Invoke(0);

            reporter("A total of " + rows.Count + " matrices were selected", "Checking...", true, false);

            bool connection = CheckForInternetConnection();

            if (!connection)
            {
                reporter(NOINTERNET_ERROR, NOINTERNET_TITLE, false, false);
                return;
            }
            else
            {
                reporter(INTERNET_MSG, INTERNET_TITLE, false, false);
            }

            IsCalculating = true;

            EventHandler runWorker = delegate
            {
                ILoader l = loaders.Values.OfType<ILoader>()?.FirstOrDefault(o => !o.IsBusy);
                l?.RunWorkerAsync();
            };

            int contador = rows.Count;
            int actionCount = 0;

            while (contador != 0)
            {
                MatrixRow m = rows[contador - 1];

                string msg = m.MatrixName + " was ";
                bool ok = true;
                string title = "Preparing...";

                try
                {
                    if (!accumulate)
                    {
                        m.CleanMUES();
                    }

                    Action<int> reportTheProgress = progress =>
                    {
                        if (!IsCalculating) return;

                        _showProgress?.Invoke(null, EventArgs.Empty);

                        bool finito = false;
                        finito = reportProgress(progress, m.MatrixName);

                        m.IsBusy = !finito;
                    };

                    Action updateLoadersAndCallBack =
                        delegate
                        {
                            // if (!IsCalculating) return;

                            updateLoaders(runWorker, m.MatrixID);
                            _callBack?.Invoke(m, EventArgs.Empty);
                        };

                    m.RowError = string.Empty;
                    bool goIn = (!m.HasErrors() && m.ToDo);

                    if (goIn)
                    {
                        actionCount += addToLoaders(ref m, reportTheProgress, updateLoadersAndCallBack, start, Totalend, step, accumulate, useList);
                    }
                    else
                    {
                        rows.Remove(m);
                        throw new SystemException("The matrix has errors");
                    }
                }
                catch (SystemException ex)
                {
                    exceptionAdder(ex);
                    ok = false;
                    msg += "not ";
                    title = "Failed!";
                }

                msg += "selected";

                reporter(msg, title, ok, false);

                contador--;
            }

            _resetProgress?.Invoke(actionCount);

            reporter("A total of " + loaders.Count + " matrices were prepared", "Starting...", true, false);

            runWorker.Invoke(null, EventArgs.Empty);
            runWorker.Invoke(null, EventArgs.Empty);
            runWorker.Invoke(null, EventArgs.Empty);
        }
     
        public void MakefullPic(string compositions, double max, double min, int matrixID, string labelName, string range)
        {
            string listOfenergies = string.Empty;
            listOfenergies = Engine.MakeEnergiesList(max - min, min, 2);

            string Response = Engine.QueryXCOM(compositions, listOfenergies, labelName, true);
            if (string.IsNullOrEmpty(Response)) return;

            string tempFile = _startupPath + matrixID + range + RSX.PIC_EXT;
            Engine.GetPicture(ref Response, tempFile);

            // File.WriteAllText(tempFile+HTMLExtension, Response);
        }
       

    }

    
}