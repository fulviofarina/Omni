using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rsx;
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


        /// <summary>
        /// Does everything
        /// </summary>
        /// <param name="background"></param>
        public void RunAll(bool background = false)
        {
            try
            {
                _bkgCalculation = background;

                //select
                SelectSamples(background);
                //calculate

                if (_units.Count == 0)
                {
                    if (!_bkgCalculation)
                    {
                        MessageBox.Show(SELECT_SAMPLES, NOTHING_SELECTED, MessageBoxButtons.OK);
                    }
                }
                else
                {
                    string[] inFiles =   PrepareInputs(background).ToArray();
                    RunProcess(ref inFiles);
                }
              
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
            doChMethod(ref UNIT);
        }

        public EventHandler DoChMethod(int sampleID)
        {
            EventHandler chilean = delegate
            {
                if (!IsCalculating) return;
                try
                {
                    doChMethod(sampleID);
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

        public EventHandler DoMatSSF(int sampleID, string outPutFile)
        {
            EventHandler temp = delegate
            {
                try
                {
                    if (!IsCalculating) return;
                    doMatSSFMethod(sampleID, outPutFile);
                }
                catch (SystemException ex)
                {
                    Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
                    Interface.IStore.AddException(ex);
                }
              
                _showProgress?.Invoke(null, EventArgs.Empty);
            };

            return temp;
        }

        private EventHandler finalize(int sampleID)
        {
            EventHandler final = delegate
            {
                try
                {
                    if (!IsCalculating) return;

                    UnitRow UNIT = null;
                    UNIT = Interface.IPopulate.ISamples.GetUnitBySampleID(sampleID);
                    finalize(ref UNIT);

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


        int numberofSteps = 7;


        /// <summary>
        /// Prepares de Input Files (Step 2)
        /// </summary>
        /// <param name="background"></param>
        /// <returns></returns>
        public IEnumerable<string> PrepareInputs(bool background = false)
        {
            //loop through all samples to work to
            IList<string> ioFiles = new List<string>();

            foreach (UnitRow item in _units)
            {
                
                UnitRow UNIT = item;
             
                string ioFile = string.Empty;
                try
                {
                    ioFile = prepareInputs(ref UNIT, background);

                    if (string.IsNullOrEmpty(ioFile)) continue;

                    _resetProgress?.Invoke(numberofSteps);

                     UNIT.IsBusy = true;

                    //add
                     ioFiles.Add(ioFile);

                    _showProgress?.Invoke(null, EventArgs.Empty);
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    Interface.IReport.Msg(ex.Message, ERROR_TITLE, false);
                }
             
               
            }



            return ioFiles;//.Where(o=> !string.IsNullOrEmpty(o));
        }

       

        public Process runProcess(bool hide, ref string[] arrayOfUnitFiles)
        {
            Process proceso = null;
          
            string newMatssfEXEFile = arrayOfUnitFiles[1];

            if (string.IsNullOrEmpty(newMatssfEXEFile)) return proceso;
            bool exists = File.Exists(_startupPath + newMatssfEXEFile);
            if (!exists) return proceso;


            string item = arrayOfUnitFiles[0];

            string msg = PROBLEMS_MATSSF + item;
            msg += PROBLEMS_MATSSF_EXTRA + newMatssfEXEFile;


            try
            {
                proceso = runProcess(ref arrayOfUnitFiles, hide, process_Exited);

                if (proceso != null)
                {
                    
                    _showProgress?.Invoke(null, EventArgs.Empty);
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

        /// <summary>
        /// Runs the processes (Step 3)
        /// </summary>
        /// <param name="inputFiles"></param>
        public void RunProcess(ref string[] inputFiles)
        {
            IPreferences ip = Interface.IPreferences;
            bool hide = !(ip.CurrentSSFPref.ShowMatSSF);
            //RUN 3
            runProcess( ref inputFiles, hide);

            if (_processTable.Count == 0)
            {
                IsCalculating = false;
            }
            else IsCalculating = true;
        }

        private IEnumerable<string> runProcess(ref string[] l_inpuMatSSFtFiles, bool l_hide = true)
        {
            string[] unitsNames = _units.Select(o => o.Name.Trim()).ToArray();
            int[] SubSamplesID = _units.Where(o => !o.IsSampleIDNull() )
                .Select( o=> o.SampleID)
                .ToArray();

            string[] exefiles = GenerateMatSSFEXEFile(ref unitsNames, ref l_inpuMatSSFtFiles).ToArray();

            if (exefiles.Count() == 0) return exefiles;

            IList<string> finalFilesProcessRunning = new List<string>();

            for (int i = 0; i < unitsNames.Count(); i++)
            {
                string item = unitsNames[i];
                int sampleID = SubSamplesID[i];

                string theEXEFILE = exefiles.FirstOrDefault(o => o.Contains(item));
                if (theEXEFILE == null) continue;
                if (string.IsNullOrEmpty(theEXEFILE)) continue;


                string input = l_inpuMatSSFtFiles.FirstOrDefault(o => o.Contains(item));
                if (input == null) continue;
                if (string.IsNullOrEmpty(input)) continue;


                string[] ioFile = new string[] {
                    item,
                    theEXEFILE,
                    input,
                    input.Replace(INPUT_EXT, OUTPUT_EXT),
                sampleID.ToString()};

                Process proceso =   runProcess(l_hide, ref ioFile);

                //tells if it is running
                theEXEFILE = findEXEFile(ref proceso);

                //add to list
                finalFilesProcessRunning.Add(theEXEFILE);
            }

            return finalFilesProcessRunning;
        }

        /// <summary>
        /// Selects Samples (Step 1)
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
            _units = _units.Where(o => !EC.IsNuDelDetch(o) &&  o.ToDo && !o.IsBusy).ToList();


        //    _resetProgress?.Invoke(3);


        }

        public void Set(ref Interface inter, EventHandler callBackMethod = null, Action<int> resetProg = null, EventHandler showProg = null)
        {
            Interface = inter;
            _showProgress = showProg;

            _resetProgress = resetProg;
         
            _callBack = callBackMethod;

            if (_processTable == null) _processTable = new System.Collections.Hashtable();
        }

        public MatSSF()
        {
        }
    }
}