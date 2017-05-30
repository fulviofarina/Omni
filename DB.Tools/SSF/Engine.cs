using System;
using System.Collections.Generic;
using System.Data;
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

        public void PrepareInputs(ref UnitRow UNIT, bool background = false)
        {
            try
            {
                //UnitRow UNIT = item;
                //update position in BS
                UNIT.Clean();

                if (!background) Interface.IBS.Update<UnitRow>(UNIT);

                showProgress?.Invoke(null, EventArgs.Empty);

                bool ok = CheckInputData(ref UNIT);

                if (ok)
                {
                    prepareInputs(ref UNIT);
                }
                showProgress?.Invoke(null, EventArgs.Empty);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, "ERROR", false);
            }
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

            string[] unitsNames = units.Select(o => o.Name.Trim()).ToArray();
            for (int i = 0; i < unitsNames.Count(); i++)
            {
                string item = unitsNames[i];

                try
                {
                    //if cancelled
                    if (!IsCalculating) continue;

                    //otherwise calculate
                    string newMatssfEXEFile = exefile + item + ".exe";
                    if (File.Exists(startupPath + item + inPutExt))
                    {
                        System.IO.File.Copy(startupPath + exefile, startupPath + newMatssfEXEFile, true);
                        //runAProcess(hide, item, );
                        Interface.IReport.Msg("Code cloning OK", "Code cloned...");
                    }
                    else
                    {
                        //remove from list
                        // units = units.Where(o => o.Name.CompareTo(item) != 0).ToList();
                    }
                }
                catch (Exception ex)
                {
                    // runOk = false;
                    Interface.IReport.Msg("Problems when cloning code for " + item, "Code Cloning ERROR...");
                    Interface.IStore.AddException(ex);
                }
            }

            //refresh
            //       unitsNames = units.Select(o => o.Name).ToArray();

            foreach (string item in unitsNames)
            {
                try
                {
                    //if cancelled
                    if (!IsCalculating) continue;
                    EventHandler hdl = process_Exited;
                    string EXE = exefile + item + ".exe";
                    RunAProcess(hide, item, EXE, ref hdl);
                }
                catch (Exception ex)
                {
                    // runOk = false;
                    Interface.IReport.Msg("Problems when running MatSSF for sample " + item, "MatSSF ERROR!");
                    Interface.IStore.AddException(ex);
                }
            }

            showProgress?.Invoke(null, EventArgs.Empty);
            //leave here because RunProcess is public
            if (processTable.Count == 0)
            {
                IsCalculating = false;
            }
        }

        public void RunAProcess(bool hide, string item, string newMatssfEXEFile, ref EventHandler exitHANDLER)
        {
            if (!File.Exists(startupPath + newMatssfEXEFile)) return;

            Interface.IReport.Msg("MatSSF is running OK for sample " + item, "MatSSF Running...");
            //files in and out
            string[] ioFile = new string[] { item + inPutExt, item + outPutExt };

            System.Diagnostics.Process process = IO.Process(cmd, "/c " + newMatssfEXEFile, startupPath, false, hide, null, exitHANDLER);
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

            // process.WaitForExit();
        }

        private void process_Exited(object sender, EventArgs e)
        {
            string item = (string)processTable[sender as System.Diagnostics.Process];
            processTable.Remove(sender as System.Diagnostics.Process);
            // processTable((object)process, (object)item);
            Application.OpenForms[0].Invoke(DoMatSSF(item));
            Application.OpenForms[0].Invoke(DoCKS(item));
            Application.OpenForms[0].Invoke(Finalize(item));
        }

        public EventHandler Finalize(string sampleName)
        {
            EventHandler final = delegate
            {
                if (!IsCalculating) return;

                try
                {
                    LINAA.UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);

                    Interface.IStore.Save<LINAA.SubSamplesDataTable>();
                    Interface.IStore.Save<LINAA.UnitDataTable>();

                    if (!bkgCalculation)  Interface.IBS.Update<LINAA.SubSamplesRow>(UNIT.SubSamplesRow);

                    //set DONE
                    UNIT.ValueChanged(false);
                    //what is this? check
                    UNIT.SubSamplesRow.Selected = false;


                    string msg = "Finished calculations for sample " + UNIT.Name;
                    Interface.IReport.Msg(msg, "Done");


                    if (processTable.Count == 0)
                    {
                        IsCalculating = false;
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    Interface.IReport.Msg(ex.Message, "ERROR", false);
                }

                showProgress?.Invoke(null, EventArgs.Empty);

                //callback
                callBack?.Invoke(null, EventArgs.Empty);
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
                    LINAA.UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit
                    .FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);

                    string file = startupPath + sampleName + outPutExt;
                    string lecture = IO.ReadFile(file);
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
                        string outFile = UNIT.Name + outPutExt;
                        string infile = UNIT.Name + inPutExt;
                        string exeF = startupPath + exefile + UNIT.Name + ".exe";

                        //SAVE COPY IN BACKUPS TO LATER ZIP
                        string pathBase = Interface.IStore.FolderPath
                        + Properties.Resources.Backups
                        + "\\" + UNIT.SubSamplesRow.IrradiationCode + "\\";
                        if (!Directory.Exists(pathBase))
                        {
                            Directory.CreateDirectory(pathBase);
                        }
                        System.IO.File.Copy(startupPath + outFile, pathBase + outFile, true);
                        System.IO.File.Copy(startupPath + infile, pathBase + infile, true);

                        System.IO.File.Delete(startupPath + outFile);
                        System.IO.File.Delete(startupPath + infile);
                        System.IO.File.Delete(exeF);

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