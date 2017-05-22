﻿using System;
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

            if (!Gt.Equals(string.Empty) && doSSF)
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


        public void CheckAndClean(ref UnitRow UNIT)
        {
            //CLEAN

            UNIT.Clean();
            // 1 CHECK ERRORS
            bool isOK = UNIT.CheckErrors(); //has Unit errors??
            isOK = UNIT.SubSamplesRow.CheckUnit() && isOK; //has Sample Errors?

            string inputDataOK = "Input row data is OK for Sample ";

            inputDataOK += UNIT.Name;// inputDataOKTitle;
            if (isOK)
            {
                string inputDataOKTitle = "Checking data...";
                Interface.IReport.Msg(inputDataOK, inputDataOKTitle);
            }
            else
            {
                string inputDataNotOK = "Input row data is NOT OK for Sample " + UNIT.Name;

                throw new SystemException(inputDataNotOK);
            }
        }

        public void GenerateAnInput(ref LINAA.UnitRow UNIT)
        {
            //2
            string unitName = UNIT.Name;
            //delete .out file

            //generate input file
            bool defaultValues = !Interface.IPreferences.CurrentSSFPref.Overrides;
            bool isOK = false;

            string buffer = string.Empty;

            IEnumerable<MatrixRow> matrices = UNIT.SubSamplesRow.GetMatrixRows();
            foreach (MatrixRow mat in matrices)
            {
                //suck into buffer th matrix composition of each matrix
                string composition = mat.MatrixComposition;
                IList<string[]> ls = mat.StripComposition(composition);
                buffer += mat.StripMoreComposition(ref ls);
                // buffer += " ";
            }

            string config = getChannelCfg(defaultValues, ref UNIT);

            double lenfgt = UNIT.SubSamplesRow.FillHeight;
            double diamet = UNIT.SubSamplesRow.Radius * 2;
            double mass = UNIT.SubSamplesRow.Net;

            string inputNOTGeneratedMsg = "Input metadata NOT generated for Sample ";

            if (diamet != 0 && lenfgt != 0 && !config.Equals(String.Empty))
            {
                buffer += "\n";
                buffer += mass + "\n" + diamet + "\n" + lenfgt + "\n"
                    + config;
                buffer += "\n";
                buffer += "\n";

                string fulFile = startupPath + unitName + inPutExt;
                //delete .in file
                System.IO.TextWriter writer = new System.IO.StreamWriter(fulFile, false); //create fromRow file
                writer.Write(buffer);
                writer.Close();
                writer = null;
                isOK = System.IO.File.Exists(fulFile);

                if (isOK)
                {
                    string inputGeneratedTitle = "Starting calculations...";
                    string inputGeneratedMsg = "Input metadata generated for Sample ";

                    Interface.IReport.Msg(inputGeneratedMsg + unitName, inputGeneratedTitle);
                }
                else
                {
                    throw new SystemException(inputNOTGeneratedMsg + unitName);
                }
            }
            else throw new SystemException(inputNOTGeneratedMsg + unitName);
        }

        public void RunProcess()
        {
            //RUN 3
            IPreferences ip = Interface.IPreferences;
            bool hide = !(ip.CurrentSSFPref.ShowMatSSF);
            bool doCk = (ip.CurrentSSFPref.DoCK);

            string msg = "Calculations started.\n Please be patient";
            Interface.IReport.Msg(msg, "Running...");
            Interface.IReport.Speak(msg);

            string[] unitsNames = units.Select(o => o.Name).ToArray();
            foreach (string item in unitsNames)
            {
                try
                {
                    string newMatssfEXEFile = exefile + item + ".exe";
                    System.IO.File.Copy(startupPath + exefile, startupPath + newMatssfEXEFile, true);
                    //runAProcess(hide, item, );
                    Interface.IReport.Msg("Code cloning OK", "Code cloned...");
                }
                catch (Exception ex)
                {
                    // runOk = false;
                    Interface.IReport.Msg("Problems when cloning the code for " + item, "Code Cloning ERROR...");
                    Interface.IStore.AddException(ex);
                }
            }

            foreach (string item in unitsNames)
            {
                try
                {
                    RunAProcess(hide, item, exefile + item + ".exe");
                    Interface.IReport.Msg("MatSSF execution OK", "MatSSF Code executed...");
                }
                catch (Exception ex)
                {
                    // runOk = false;
                    Interface.IReport.Msg("Problems when running the MatSSF code for " + item, "MatSSF ERROR...");
                    Interface.IStore.AddException(ex);
                }
            }
        }

        public void RunAProcess(bool hide, string item, string newMatssfEXEFile)
        {
            //files in and out
            string[] ioFile = new string[] { item + inPutExt, item + outPutExt };
            EventHandler processExits = delegate
            {
                Application.OpenForms[0].Invoke(DoMatSSF(item));
                Application.OpenForms[0].Invoke(DoCKS(item));
                Application.OpenForms[0].Invoke(ReportFinished(item));
            };
            System.Diagnostics.Process process = IO.Process(cmd, "/c " + newMatssfEXEFile, startupPath, false, hide, null, processExits);
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

        public EventHandler ReportFinished(string sampleName)
        {
            EventHandler final = delegate
            {
                try
                {
                    LINAA.UnitRow UNIT = null;
                    UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);

                    Interface.IStore.Save<LINAA.SubSamplesDataTable>();
                    Interface.IStore.Save<LINAA.UnitDataTable>();

                    Interface.IBS.Update<LINAA.UnitRow>(UNIT);

                    string msg = "Finished for Unit " + UNIT.Name;
                    Interface.IReport.Msg(msg, "Done");
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

                // Application.DoEvents();
                DoMatSSF(sampleName);
                showProgress?.Invoke(null, EventArgs.Empty);
            };

            return temp;
        }
    }
}