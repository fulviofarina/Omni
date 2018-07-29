using Rsx.Dumb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class XCOM : CalculateBase
    {
        protected internal Interface Interface;
        private bool calculating = false;
        private Action<Exception> exceptionAdder;
        private Hashtable loaders = new Hashtable();
        private bool offline = false;
        private XCOMPrefRow pref;
        private Action<string, string, object, bool> reporter;
        private IList<MatrixRow> rows;
        private decimal seconds = 0;
        private Stopwatch stopwatch;
        public Action<Exception> ExceptionAdder
        {
            get
            {
                return exceptionAdder;
            }

            set
            {
                exceptionAdder = value;
            }
        }

        public new bool IsCalculating
        {
            get
            {
                return calculating;
            }
            set
            {
                calculating = value;

                if (!calculating)
                {
                    CheckCompletedOrCancelled();
                }
                else
                {
                    loaders.Clear();
                    stopwatch.Start();
                    seconds = 0;
                }
            }
        }

        public bool Offline
        {
            get
            {
                return offline;
            }

            set
            {
                offline = value;
            }
        }

        public XCOMPrefRow Preferences
        {
            set
            {
                pref = value;
            }
        }

        public Action<string, string, object, bool> Reporter
        {
            set
            {
                reporter = value;
            }
        }

        public IList<MatrixRow> Rows
        {
            get
            {
                return rows;
            }

            set
            {
                rows = value;
            }
        }

     
        public void Calculate(bool? BKG)
        {
            double start = pref.StartEnergy;
            double Totalend = pref.EndEnergy;
            double step = pref.Steps;
            bool useList = pref.UseList;
            bool accumulate = pref.AccumulateResults;

            listOfEnergiesBytes = pref.ListOfEnergies;

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

                    Action<int> report = progress =>
                    {

                        if (!IsCalculating) return;

                        _showProgress?.Invoke(null, EventArgs.Empty);


                         bool finito = false;
                         finito = reportProgress(progress, m.MatrixName);
                     
                         m.IsBusy = !finito;
                     
                    };

                    Action callBack =
                        delegate
                        {
                           // if (!IsCalculating) return;

                            updateLoaders(runWorker, m.MatrixID);
                            _callBack?.Invoke(m, EventArgs.Empty);
                        };
             
                    actionCount += addToLoaders(ref m, report, callBack, start, Totalend, step,accumulate, useList);
                   
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

        public void CheckCompletedOrCancelled()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                seconds = Decimal.Round(Convert.ToDecimal(stopwatch.Elapsed.TotalSeconds), 0);
            }
            string log = string.Empty;
            bool ok = true;
            string title = "Completed!";
            if (loaders.Count == 0)
            {
                log = "Everything completed in ";
            }
            else
            {
                cancelLoaders();
                ok = false;
                loaders.Clear();
                title = "Cancelled!";
                log = "Computations cancelled after ";
            }
            log += seconds + " seconds";

            reporter(log, title, ok, false);
        }

        private void cancelLoaders()
        {
            foreach (int i in loaders.Keys)
            {
                ILoader item = (ILoader)loaders[i];
                item.CancelLoader();

                MatrixRow toCancel = rows.Where(o => o.IsBusy).FirstOrDefault(o => o.MatrixID == i);
                toCancel.SetAsNotCalculated();


            }
        }

        public IList<Action> generateActions(ref MatrixRow matrix, double start, double totalEnd, double step, bool offline, bool accumulate, bool useList = false)
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
            if (maxEnergies > NrEnergies)
            {
                delta = (NrEnergies * step);
                totalEnd = start + delta;
            }
            else
            {
                nrOfQueries = Convert.ToInt32(Math.Ceiling(((double)NrEnergies / (double)maxEnergies)));
                delta = (maxEnergies * step);
            }
            end = start + delta;




            string listOfenergies, compositions;
            listOfenergies = string.Empty;

            if (useList)
            {
                useCustomList(out start, out totalEnd, out end, out listOfenergies, out NrEnergies);
            }

            compositions = GetCompositionString(m.MatrixComposition);


            while (nrOfQueries>0)
            {

                //finishing next round
            


                if (!useList)
                {
                    if (nrOfQueries - 1 == 0) end = totalEnd + step;

                    int lines = GetNumberOfLines(step, start, end);
                    listOfenergies = MakeEnergiesList(step, start, lines);

                    if (nrOfQueries - 1 == 0) end = totalEnd ;

                }

                string labelName = m.MatrixName + ": " + start + " to " + end + " keV";
                Action action = setMainAction(m.MatrixID, numberOfFiles, listOfenergies, compositions, labelName, start, end);


                if (!useList)
                {
                    start = end;
                    end += delta;
                    numberOfFiles++;
                }

                /*
                //si sobrepasó totalEnd
                if (end >= totalEnd)
                {
                    end = totalEnd + step;
                }
                */

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
                    getMUESFromNIST(m.MatrixDensity, _startupPath, ref mu, m.MatrixID, numberOfFiles);
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
                makefullPic(compositions, totalEnd, initialStart, matrixID, labelName, range);



                labelName = mstrixName + ": " + min.ToString() + " keV to " + maxDisplay.ToString() + " GeV";
                 range = ".FULL";
                 makefullPic( compositions, max, min, matrixID, labelName, range);

            

            };


            ls.Add(action2);
            ls.Add(action3);
          

      //      m.IsBusy = true;

            return ls;
        }

        private void makefullPic(string compositions, double max, double min, int matrixID, string labelName, string range)
        {
            string listOfenergies = string.Empty;
            listOfenergies = MakeEnergiesList(max - min, min, 2);

            string Response = QueryXCOM(compositions, listOfenergies, labelName, true);
            if (string.IsNullOrEmpty(Response)) return;

            string tempFile = _startupPath + matrixID + range+ PictureExtension;
            getPicture(ref Response, tempFile);

       //     File.WriteAllText(tempFile+HTMLExtension, Response);

        }

        public void Set(ref Interface inter)
        {
            Interface = inter;
        }

        private int addToLoaders(ref MatrixRow m, Action<int> report, Action callBack, double start, double Totalend, double step, bool accumulate, bool useList = true)
        {
            m.RowError = string.Empty;
            bool goIn = (!m.HasErrors() && m.ToDo);

            if (!goIn) throw new SystemException("The matrix has errors");

            IList<Action> actions = generateActions(ref m, start, Totalend, step, offline, accumulate, useList);

            if (actions.Count == 0) throw new SystemException("The Actions list for the Matrix is empty");

            ILoader ld = new Loader();
            ld.Set(actions, callBack, report);
            loaders.Add(m.MatrixID, ld);

            return actions.Count;
        }

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

                    string range = punto + "N" + numberOfFiles ;

                    tempFile = _startupPath + matrixID +  range ;

                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                    File.WriteAllText(tempFile, Response);


                    range += punto + start.ToString() + " - " + end.ToString();
                    makefullPic(compositions, end,start , matrixID, labelName, range);


                }
                catch (SystemException ex)
                {
                    exceptionAdder(ex);
                }
            };
            return action;
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
        public XCOM() : base()
        {
            stopwatch = new Stopwatch();
        }
        // private string png = ".png";
    }
}