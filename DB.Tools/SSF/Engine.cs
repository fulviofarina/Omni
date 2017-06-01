using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected internal bool bkgCalculation = false;
        public void DoCKS(ref LINAA.UnitRow UNIT)
        {
            //sample geometry dependant values// why recalculate each time? leave them here
            HashSet<double> abs = new HashSet<double>();
            HashSet<string> elements = new HashSet<string>();

            IEnumerable<LINAA.MatSSFRow> rows = UNIT.GetMatSSFRows();

            UNIT.GtCh = 1;
            double surf = (UNIT.SubSamplesRow.Radius) * ((UNIT.SubSamplesRow.Radius) + (UNIT.SubSamplesRow.FillHeight));
            double SDensity = 6.0221415 * 10 * UNIT.SubSamplesRow.Net / surf;
            double Xi = 1e-12 * SDensity * UNIT.kepi;
            Int32 factor = 10000;

            double A2 = 0.06;
            double pUniEpi = 0.82;
            double pUniTh = 0.964;
            double A1 = 1;

            foreach (LINAA.MatSSFRow m in rows)
            {
                try
                {
                    LINAA.SigmasSalRow sal = m.ReactionsRowParent?.SigmasSalRow;
                    LINAA.SigmasRow sepi = m.ReactionsRowParent?.SigmasRowParent;

                    // double kth = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kth; double
                    // kepi = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kepi;
                    if (sal == null || sepi == null) continue;

                    m.SSFCh = 1;
                    if (sal != null)
                    {
                        string element = m.TargetIsotope.Split('-')[0];
                        if (elements.Add(element))
                        {
                            double chTh = 0;

                            chTh = 1000 * sal.sigmaSal / sal.Mat;

                            abs.Add(m.Weight * chTh);
                        }
                    }
                    if (sepi != null)
                    {
                        m.ChEpi = 0;
                        m.ChEpi = 1000 * sepi.sigmaEp / sal.Mat;

                        m.SSFCh = ((A1 - A2) / (Math.Pow(Xi * m.ChEpi * (m.Weight * factor), pUniEpi) + 1.0)) + A2;
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    EC.SetRowError(m, ex);
                }
            }

            double sumTh = abs.Sum() * SDensity * factor;
            //Put the Sum of Thermal absorbers in ChTh of UnitRow
            UNIT.ChTh = sumTh;

            UNIT.GtCh = (A1 / (Math.Pow(UNIT.ChTh * 1e-12 * UNIT.kth, pUniTh) + 1.0));    // and since is inherited by parentRelation...

            abs.Clear();
            elements.Clear();
            abs = null;
            elements = null;
        }

        public bool DoMatSSF(string lecture, ref LINAA.UnitRow UNIT, bool doSSF)
        {
            //read file data
            bool isOk = false;

            IEnumerable<string> array = lecture.Split('\n');
            array = array.Where(o => !o.Equals("\r"));
            array = array.Select(o => o.Trim(null)).AsEnumerable();

            string aux = string.Empty;
            string Gt = string.Empty;
            string Mdens = string.Empty;
            string MCL = string.Empty;
            string EXS = string.Empty;
            string PXS = string.Empty;

            string densityUnit = "[g/cm3]";
            string cmUnit = "[cm]";
            string invcmUnit = "[1/cm]";

            aux = "Material density";
            Tables.SetField(ref Mdens, ref array, aux, densityUnit);
            aux = "G-thermal";
            Tables.SetField(ref Gt, ref array, aux, string.Empty);
            aux = "Mean chord length";
            Tables.SetField(ref MCL, ref array, aux, cmUnit);
            aux = "Escape x.sect.";
            Tables.SetField(ref EXS, ref array, aux, invcmUnit);
            aux = "Potential x.sect.";
            Tables.SetField(ref PXS, ref array, aux, invcmUnit);

            if (!string.IsNullOrEmpty(Gt) && doSSF)
            {
                UNIT.Gt = Dumb.Parse(Gt, 1);
                if (UNIT.SubSamplesRow != null)
                {
                    UNIT.SubSamplesRow.Gthermal = UNIT.Gt;
                }
            }

            fillUnitWith(ref UNIT, Mdens, EXS, MCL, PXS);

            //fill the matssf table
            LINAA.MatSSFDataTable table = fillSSFTableWith(ref UNIT, array.ToList(), doSSF);

            UNIT.SSFTable = Tables.MakeDTBytes(ref table, startupPath);

            array = null;
            isOk = table.Count != 0;

            Dumb.FD(ref table);

            return isOk;
        }
        public void PrepareInputs(bool background = false)
        {
          

            foreach (UnitRow item in units)
            {
                UnitRow UNIT = item;
                PrepareInputs(ref UNIT, background);
            }

      

        }

        public bool PrepareInputs(ref UnitRow UNIT, bool background = false)
        {

            showProgress?.Invoke(null, EventArgs.Empty);

            bool ok = false;
            try
            {
                //UnitRow UNIT = item;
                //update position in BS
                UNIT.Clean();

                if (!background) Interface.IBS.Update<UnitRow>(UNIT);

                ok = CheckInputData(ref UNIT);

                if (ok)
                {
                    //generate file
                    string fulFile = startupPath + UNIT.Name + INPUT_EXT;
                    string buffer =   prepareInputs(ref UNIT);
                 //   bool isOK = false;
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        //delete .in file
                        TextWriter writer = new StreamWriter(fulFile, false); //create fromRow file
                        writer.Write(buffer);
                        writer.Close();
                        writer = null;
                        //here
                        ok = File.Exists(fulFile);
                    }
                   
                    //now check if file generated
                    if (ok)
                    {
                        string inputGeneratedTitle = "Starting calculations...";
                        string inputGeneratedMsg = "Input metadata generated for Sample ";
                        UNIT.IsBusy = true;
                        Interface.IReport.Msg(inputGeneratedMsg + UNIT.Name, inputGeneratedTitle);
                    }
                    else
                    {
                        throw new SystemException(INPUT_NOTGEN + UNIT.Name);
                    }
                }
              
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, "ERROR", false);
            }

            showProgress?.Invoke(null, EventArgs.Empty);

            return ok;
        }


        /// <summary>
        /// Checks if the input data has errors
        /// </summary>
        /// <param name="UNIT"></param>
        /// <returns></returns>
        public bool CheckInputData(ref UnitRow UNIT)
        {
            //CLEAN

            // 1 CHECK ERRORS
            bool hasError = UNIT.HasErrors(); //has Unit errors??
            hasError = UNIT.SubSamplesRow.HasErrors() || hasError; //has Sample Errors?

            string inputDataOK = "Input data is OK for Sample ";

            inputDataOK += UNIT.Name;// inputDataOKTitle;
            string error = "Input data is NOT OK for ";
            if (hasError)
            {
                string inputDataNotOK = error + "Sample " + UNIT.Name;
                UNIT.SetColumnError((UNIT.Table as UnitDataTable).NameColumn, error + "Sample");
            }
            else
            {
                string inputDataOKTitle = "Checking data...";
                Interface.IReport.Msg(inputDataOK, inputDataOKTitle);
            }
            return !hasError;
        }

      
        public void RunProcess()
        {
            //RUN 3
            IsCalculating = true;

            IPreferences ip = Interface.IPreferences;
            bool hide = !(ip.CurrentSSFPref.ShowMatSSF);
            bool doCk = (ip.CurrentSSFPref.DoCK);

            RunProcess(hide);

            showProgress?.Invoke(null, EventArgs.Empty);
            //leave here because RunProcess is public
            if (processTable.Count == 0)
            {
                IsCalculating = false;
            }
        }

        public IEnumerable<string> RunProcess(bool hide=true,  string[] unitsNames=null)
        {
            if (unitsNames==null) unitsNames = units.Select(o => o.Name.Trim()).ToArray();

            IEnumerable<string> exefiles =  GenerateMatSSFEXEFile(ref unitsNames);


            for (int i = 0; i < unitsNames.Count(); i++)
            {
                string item = unitsNames[i];
                string EXE = exefiles.ElementAt(i);
                

                try
                {
                    //if cancelled
                    if (string.IsNullOrEmpty(EXE)) continue;

                    if (!IsCalculating) continue;

                    EventHandler hdl = process_Exited;
                    //  string EXE = exefile + item + "." + Guid.NewGuid() + ".exe";
               
                    RunAProcess(hide, item, EXE, ref hdl);
                }
                catch (Exception ex)
                {
                    string msg = "Problems when running MatSSF for sample " + item;
                    msg += ", having EXE FILE: " + EXE;
                    Interface.IReport.Msg(msg, "MatSSF ERROR!");

                    Exception ex2  = new Exception(msg,ex.InnerException);
                    Interface.IStore.AddException(ex2);
                }
            }

            return exefiles;
        }

        public IEnumerable<string> GenerateMatSSFEXEFile(ref  string[] unitsNames)
        {
            IList<string> exefiles = new List<string>();

            for (int i = 0; i < unitsNames.Count(); i++)
            {
                string item = unitsNames[i];
                try
                {
                    //if cancelled
                    if (!IsCalculating) continue;
                    //otherwise calculate
                   string thisExeFile =    GenerateMatSSFEXEFile(item);
                    exefiles.Add(thisExeFile);

                }
                catch (Exception ex)
                {
                    Interface.IReport.Msg("Problems when cloning code for " + item, "Code Cloning ERROR...");
                    Interface.IStore.AddException(ex);
                }
            }

            return exefiles;
        }

        public string GenerateMatSSFEXEFile(string item)
        {
            string newMatssfEXEFile = EXEFILE 
                + item + "." 
                + Guid.NewGuid().ToString().Substring(0,5)
                + ".exe";

            bool inputExist = File.Exists(startupPath + item + INPUT_EXT);
            if (inputExist)
            {
                //copy .exe file peersonalized for the unit
                File.Copy(startupPath + EXEFILE, startupPath + newMatssfEXEFile, true);
                //runAProcess(hide, item, );
                Interface.IReport.Msg("Code cloning OK", "Code cloned...");
            }
            return newMatssfEXEFile;
        }

        public Process RunAProcess(bool hide, string item, string newMatssfEXEFile, ref EventHandler exitHANDLER)
        {
            if (string.IsNullOrEmpty(newMatssfEXEFile)) return null;
            bool exists = File.Exists(startupPath + newMatssfEXEFile);
            if (!exists) return null;

            Interface.IReport.Msg("MatSSF is running OK for sample " + item, "MatSSF Running...");
            //files in and out
            string[] ioFile = new string[] { item + INPUT_EXT, item + OUTPUT_EXT };

            Process process = IO.Process(cmd, "/c " + newMatssfEXEFile, startupPath, false, hide, null, exitHANDLER);
            //add to table
            processTable.Add((object)process, (object)item);
            //start process
            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            //set input files on Console

            // process.WaitForInputIdle();

            foreach (string content in ioFile)
            {
                process.StandardInput.WriteLine(content);
            }

            return process;
            // process.WaitForExit();
        }

        private void process_Exited(object sender, EventArgs e)
        {
            if (processTable.Count == 0) return;
            Process process = sender as Process;

            string item = (string)processTable[process];

            if (item == null) return;
            if (string.IsNullOrEmpty(item)) return;

            try
            {

                processTable.Remove(process);

                string exeF = process.StartInfo.Arguments;
                int index = exeF.IndexOf(EXEFILE[0]);
                exeF = process.StartInfo.Arguments.Substring(index);

                File.Delete(startupPath + exeF);
               // EventHandler updateToDo = UpdateToDo(item);

                Application.OpenForms[0].Invoke(UpdateToDo(item));

                ////OTHERSS!!!

                Application.OpenForms[0].Invoke(DoMatSSF(item));
         

            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }

            Application.OpenForms[0].Invoke(DoCKS(item));

            Application.OpenForms[0].Invoke(Finalize(item));
        }

        private EventHandler UpdateToDo(string item)
        {
            EventHandler updateToDo = delegate
            {
                UnitRow UNIT = null;
                UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(item) == 0);
                //set DONE
                UNIT.ToDo = false;
                UNIT.LastChanged = DateTime.Now;
                UNIT.LastCalc = DateTime.Now;
                UNIT.IsBusy = false;

                string outFile = UNIT.Name + OUTPUT_EXT;
                string infile = UNIT.Name + INPUT_EXT;

                //SAVE COPY IN BACKUPS TO LATER ZIP
                string pathBase = Interface.IStore.FolderPath
                + Properties.Resources.Backups
                + "\\" + UNIT.SubSamplesRow.IrradiationCode + "\\";


                if (!Directory.Exists(pathBase))
                {
                    Directory.CreateDirectory(pathBase);
                }
                File.Copy(startupPath + outFile, pathBase + outFile, true);
                File.Copy(startupPath + infile, pathBase + infile, true);

                File.Delete(startupPath + infile);

            };
            return updateToDo;
        }

        public EventHandler Finalize(string sampleName)
        {

         

            EventHandler final = delegate
            {
                try
                {

                  //  Application.OpenForms[0].ValidateChildren();

                  if (!IsCalculating) return;


                    UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);

                    string msg = "Finished calculations for sample " + UNIT.Name;
                    Interface.IReport.Msg(msg, "Done");

                    if (processTable.Count == 0)
                    {
                        if (!bkgCalculation)
                        {
                            Interface.IStore.Save<SubSamplesDataTable>();
                            Interface.IStore.Save<UnitDataTable>();

                            Interface.IBS.Update<SubSamplesRow>(UNIT.SubSamplesRow);
                        }

                        IsCalculating = false;
                    }

                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    Interface.IReport.Msg(ex.Message, "ERROR", false);
                }

                showProgress?.Invoke(null, EventArgs.Empty);
             
            };

            return final;
        }

        public EventHandler DoCKS(string sampleName)
        {
            EventHandler chilean = delegate
            {
                if (!IsCalculating) return;

                try
                {
                    LINAA.UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);

                    IPreferences ip = Interface.IPreferences;

                    bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

                    bool doCk = (ip.CurrentSSFPref.DoCK);

                    if (doCk)
                    {
                        DoCKS(ref UNIT);

                        Interface.IReport.Msg("CKS calculations done for Unit " + UNIT.Name, "CKS Calculations");
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    Interface.IReport.Msg(ex.Message, "ERROR", false);
                }

                showProgress?.Invoke(null, EventArgs.Empty);
            };

            return chilean;
        }

        public EventHandler DoMatSSF(string sampleName)
        {
            EventHandler temp = delegate
            {
                if (!IsCalculating) return;

                try
                {
                    bool runOk = false;
                    UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit
                    .FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);

                    string outFile = startupPath+ UNIT.Name + OUTPUT_EXT;
                    string lecture = IO.ReadFile(outFile);
                    File.Delete(outFile);

                    string problemsFor = "Problems Reading MATSSF Output for Unit ";
                    string matssfOk = "MatSSF calculations done for Unit ";
                    string matssfMsgTitle = "MatSSF Calculations";

                    if (string.IsNullOrEmpty(lecture))
                    {
                        throw new SystemException(problemsFor + UNIT.Name + "\n");
                    }

                    bool doSSF = Interface.IPreferences.CurrentSSFPref.DoMatSSF;
                    runOk = DoMatSSF(lecture, ref UNIT, doSSF);

                    if (!runOk)
                    {
                        throw new SystemException(problemsFor + UNIT.Name + "\n");
                    }
                    else
                    {
                     
                       // string infile = UNIT.Name + INPUT_EXT;
                       

                        //SAVE COPY IN BACKUPS TO LATER ZIP
                      //  string pathBase = Interface.IStore.FolderPath
                      //  + Properties.Resources.Backups
                      //  + "\\" + UNIT.SubSamplesRow.IrradiationCode + "\\";
                      //  if (!Directory.Exists(pathBase))
                      //  {
                      //      Directory.CreateDirectory(pathBase);
                      //  }
                      //  File.Copy(startupPath + outFile, pathBase + outFile, true);
                      //  File.Copy(startupPath + infile, pathBase + infile, true);

                    //   File.Delete(startupPath + outFile);
                 //     File.Delete(startupPath + infile);

                  
                        Interface.IReport.Msg(matssfOk + UNIT.Name, matssfMsgTitle);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IReport.Msg(ex.Message, "ERROR", false);
                    Interface.IStore.AddException(ex);
                }

                /// WARNING
                /////////////VOLVER A PONER SI TENGO PROBLEMAS
                // DoMatSSF(sampleName);
                showProgress?.Invoke(null, EventArgs.Empty);
            };

            return temp;
        }
    }
}