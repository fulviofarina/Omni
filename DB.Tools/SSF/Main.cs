using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        /// <summary>
        /// The MatSSF startup path
        /// </summary>
        public static string StartupPath
        {
            get { return _startupPath; }
            set { _startupPath = value; }
        }

        public IList<UnitRow> Units
        {
            get
            {
                return _units;
            }
        }

        public void Calculate(bool background = false)
        {
            _bkgCalculation = background;

            //actual position
            //  Cursor.Current = Cursors.WaitCursor;
            try
            {
                // Interface.IBS.IsCalculating = true;

                if (!_bkgCalculation) Creator.SaveInFull(true);

                _resetProgress?.Invoke(3);

                int position = Interface.IBS.SubSamples.Position;
                // int count = SelectUnits();
                SelectSamples(background);

                //1
                if (_units.Count == 0)
                {
                    if (!_bkgCalculation)
                    {
                        MessageBox.Show(SELECT_SAMPLES, NOTHING_SELECTED, MessageBoxButtons.OK);
                    }
                    // IsCalculating = false;
                }
                else
                {
                    //loop through all samples to work to
                    int numberofSteps = 5;

                    _resetProgress?.Invoke(2 + (_units.Count * numberofSteps));

                   string[] inFiles =   PrepareInputs(background).ToArray();

                    if (!background) Interface.IBS.SubSamples.Position = position;

                    RunProcess(ref inFiles);
                }

                _callBack?.Invoke(null, EventArgs.Empty);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
            }
        }

        /// <summary>
        /// Checks if the input data has errors
        /// </summary>
        /// <param name="UNIT"></param>
        /// <returns></returns>
    

        public void DoChMethod(ref LINAA.UnitRow UNIT)
        {
            doChMethod(UNIT.Name);
        }

        public EventHandler DoChMethod(string sampleName)
        {
            EventHandler chilean = delegate
            {
                if (!IsCalculating) return;
                try
                {
                    doChMethod(sampleName);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
                }

                //
                _showProgress?.Invoke(null, EventArgs.Empty);
            };

            return chilean;
        }

        public bool DoMatSSF(string lecture, ref LINAA.UnitRow UNIT, bool doSSF)
        {
            return doMatSSFMethod(lecture, ref UNIT, doSSF);
        }

        public EventHandler DoMatSSF(string sampleName, string outPutFile)
        {
            EventHandler temp = delegate
            {
                if (!IsCalculating) return;

                try
                {
                    doMatSSFMethod(sampleName, outPutFile);
                }
                catch (SystemException ex)
                {
                    Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
                    Interface.IStore.AddException(ex);
                }

                //
                _showProgress?.Invoke(null, EventArgs.Empty);
            };

            return temp;
        }

        public EventHandler Finalize(string sampleName)
        {
            EventHandler final = delegate
            {
                try
                {
                    if (!IsCalculating) return;

                    UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);


                    reportFinished(sampleName);

                    if (!_bkgCalculation)
                    {
                        Interface.IBS.Update<SubSamplesRow>(UNIT.SubSamplesRow);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
                }

                _showProgress?.Invoke(null, EventArgs.Empty);
            };

            return final;
        }

        public IEnumerable<string> GenerateMatSSFEXEFile(ref string[] unitsNames, ref string[] inFiles)
        {
            IList<string> exefiles = new List<string>();

            for (int i = 0; i < unitsNames.Count(); i++)
            {
                string item = unitsNames[i];
                try
                {
                    //if not found in the list of input files, dont add it
                    string INfile = inFiles.FirstOrDefault(o => o.Contains(item));

                    if (INfile == null) continue;
                    if (string.IsNullOrEmpty(INfile)) continue;
                    //otherwise calculate
                    string thisExeFile = generateMatSSFEXEFile(INfile);
                  //  return newMatssfEXEFile;

                   // string thisExeFile = GenerateMatSSFEXEFile(item);
                    exefiles.Add(thisExeFile);
                }
                catch (Exception ex)
                {
                    Interface.IReport.Msg(PROBLEMS_CLONING + item, CLONING_ERROR_TITLE);
                    Interface.IStore.AddException(ex);
                }
            }

            return exefiles;
        }

      

        public IEnumerable<string> PrepareInputs(bool background = false)
        {

            IList<string> ioFiles = new List<string>();

            foreach (UnitRow item in _units)
            {
                
                UnitRow UNIT = item;
                string ioFile =   PrepareInputs(ref UNIT, background);

                if (string.IsNullOrEmpty(ioFile)) continue;
                ioFiles.Add(ioFile);
               
            }

            return ioFiles;
        }

        public string PrepareInputs(ref UnitRow UNIT, bool background = false)
        {
            _showProgress?.Invoke(null, EventArgs.Empty);


            string inputFile = string.Empty;
            try
            {
                 inputFile = prepareInputs(ref UNIT, background);
              //  ok = true;
             
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
            }

            _showProgress?.Invoke(null, EventArgs.Empty);

            return inputFile;
        }

        public Process RunProcess(bool hide, ref string[] ioFile)
        {
            Process proceso = null;

          
            string newMatssfEXEFile = ioFile[1];

            if (string.IsNullOrEmpty(newMatssfEXEFile)) return proceso;
            bool exists = File.Exists(_startupPath + newMatssfEXEFile);
            if (!exists) return proceso;

            string item = ioFile[0];

            string msg = PROBLEMS_MATSSF + item;
            msg += PROBLEMS_MATSSF_EXTRA + newMatssfEXEFile;
            try
            {
                proceso = runProcess(ref ioFile, hide, process_Exited);

                if (proceso != null)
                {
                    Interface.IReport.Msg(MATSSF_OK + item, RUNNING_MATSSF_TITLE);
                }
                else throw new Exception(msg);
                // process.WaitForExit();
            }
            catch (Exception ex)
            {
                Interface.IReport.Msg(msg, MATSSF_ERROR);
              
                Interface.IStore.AddException(ex);
            }
            return proceso;
        }

        public void RunProcess(ref string[] inFiles)
        {
            IPreferences ip = Interface.IPreferences;
            bool hide = !(ip.CurrentSSFPref.ShowMatSSF);
            bool doCk = (ip.CurrentSSFPref.DoCK);

            //RUN 3
            RunProcess( ref inFiles, hide);

            _showProgress?.Invoke(null, EventArgs.Empty);

            //leave here because RunProcess is public
            if (_processTable.Count == 0)
            {
                IsCalculating = false;
            }
            else IsCalculating = true;
        }

        public IEnumerable<string> RunProcess(ref string[] infIles, bool hide = true)
        {
            string[] unitsNames = _units.Select(o => o.Name.Trim()).ToArray();

            string[] exefiles = GenerateMatSSFEXEFile(ref unitsNames, ref infIles).ToArray();

            if (exefiles.Count() == 0) return exefiles;

            IList<string> finalFilesProcessRunning = new List<string>();

            for (int i = 0; i < unitsNames.Count(); i++)
            {
                string item = unitsNames[i];


                string theEXEFILE = exefiles.FirstOrDefault(o => o.Contains(item));
                if (theEXEFILE == null) continue;
                if (string.IsNullOrEmpty(theEXEFILE)) continue;


                string input = infIles.FirstOrDefault(o => o.Contains(item));
                if (input == null) continue;
                if (string.IsNullOrEmpty(input)) continue;


                string[] ioFile = new string[] {item,
                    theEXEFILE,
                    input,
                    input.Replace(INPUT_EXT, OUTPUT_EXT) };

                Process proceso =   RunProcess(hide, ref ioFile);

                //tells if it is running
                theEXEFILE = findEXEFile(ref proceso);

                //add to list
                finalFilesProcessRunning.Add(theEXEFILE);
            }

            return finalFilesProcessRunning;
        }

        /// <summary>
        /// Prepares de Input Files
        /// </summary>

        public void SelectSamples(bool background = false)
        {
            //should loop all?
            bool shoulLoop = Interface.IPreferences.CurrentSSFPref.Loop;

            //if batch evaluation or backround bacth work
            if (shoulLoop || background)
            {
                //take ALL SAMPLES (BACKROUND RUN)
                if (background)
                {
                    _units = Interface.IDB.Unit.OfType<UnitRow>()
                        .ToList();
                }
                //take the ones in the binding source
                else
                {
                    _units = Interface.ICurrent.Units.OfType<LINAA.UnitRow>()
                        .ToList();
                }
            }
            else //no loop, take current
            {
                //take only current (binding source)
                _units = new List<UnitRow>();
                UnitRow u = Interface.ICurrent.Unit as UnitRow;
                _units.Add(u);
            }
            //filter now by non busy and TODO
            _units = _units.Where(o => o.ToDo && !o.IsBusy).ToList();
        }

        public void Set(ref Interface inter, EventHandler callBackMethod = null, Action<int> resetProg = null, EventHandler showProg = null)
        {
            Interface = inter;
            _showProgress = delegate
            {
                if (!_bkgCalculation)
                {
                    showProg?.Invoke(null, EventArgs.Empty);
                }
            };
            _resetProgress = delegate (int i)
            {
                if (!_bkgCalculation)
                {
                    resetProg?.Invoke(i);
                }
            };
            _callBack = callBackMethod;

            if (_processTable == null) _processTable = new System.Collections.Hashtable();
        }

        public MatSSF()
        {
        }
    }
}