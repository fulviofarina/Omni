﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;
using static Rsx.Dumb.CalculateBase.TXT;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        private static bool checkIfSampleHasErrors(ref UnitRow UNIT)
        {
            //CLEAN

            // 1 CHECK ERRORS
            bool hasError = UNIT.HasErrors(); //has Unit errors??
            bool hasSampleError = UNIT.SubSamplesRow.HasErrors();
            //has Sample Errors?
            return hasSampleError || hasError;
        }

        private static Process runProcess(string path, ref string[] arrayOfUnitFiles, bool hide, EventHandler exitHANDLER)
        {
            string newMatssfEXEFile = arrayOfUnitFiles[1]; // the second item

            Process process = IO.Process(CMD, path, "/c " + newMatssfEXEFile, false, hide, null, exitHANDLER);
            //add to table
            _processTable.Add((object)process, (object)arrayOfUnitFiles);
            //start process
            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            //set input files on Console

            // process.WaitForInputIdle();
            process.StandardInput.WriteLine(arrayOfUnitFiles[2]);
            process.StandardInput.WriteLine(arrayOfUnitFiles[3]);

            return process;
        }

        private void doChMethod(ref UnitRow UNIT)
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

        private void doChMethod(int sampleID)
        {
            UnitRow UNIT = Interface.IPopulate.ISamples.GetUnitBySampleID(sampleID);

            IPreferences ip = Interface.IPreferences;

            bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

            bool doCk = (ip.CurrentSSFPref.DoCK);

            if (doCk)
            {
                doChMethod(ref UNIT);

                Interface.IReport.Msg(CHILIAN_OK + UNIT.Name, "CKS Calculations");
            }
        }

        private bool doMatSSFMethod(string lecture, ref UnitRow UNIT, bool doSSF)
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

            UNIT.SSFTable = Tables.MakeDTBytes(ref table, _startupPath);

            array = null;
            isOk = table.Count != 0;

            Dumb.FD(ref table);

            return isOk;
        }

        private void doMatSSFMethod(int sampleID, string OutputFile)
        {
            bool runOk = false;
            UnitRow UNIT = Interface.IPopulate.ISamples.GetUnitBySampleID(sampleID);

            string outFile = _startupPath + OutputFile;
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
                Interface.IReport.Msg(matssfOk + UNIT.Name, matssfMsgTitle);
            }
        }

        private string prepareInputs(ref UnitRow UNIT, bool background)
        {
            bool ok;
            //UnitRow UNIT = item;
            //update position in BS
            //  UNIT.Clean();
            //CLEAN!!!
            //
            //
            //    if (!background) Interface.IBS.CurrentChanged<UnitRow>(UNIT);

            ok = checkInputData(ref UNIT);

            string fulFile = string.Empty;
            if (ok)
            {
                //generate file
                fulFile = getGUID(UNIT.Name) + INPUT_EXT;
                string buffer = prepareInputs(ref UNIT);
                // bool isOK = false;
                if (!string.IsNullOrEmpty(buffer))
                {
                    //delete .in file
                    TextWriter writer = new StreamWriter(_startupPath + fulFile, false); //create fromRow file
                    writer.Write(buffer);
                    writer.Close();
                    writer = null;
                    //here
                    ok = File.Exists(_startupPath + fulFile);
                }

                //now check if file generated
                if (ok)
                {
                    string inputGeneratedTitle = "Starting calculations...";
                    string inputGeneratedMsg = "Input metadata generated for Sample ";

                    Interface.IReport.Msg(inputGeneratedMsg + UNIT.Name, inputGeneratedTitle);
                }
                else
                {
                    throw new SystemException(INPUT_NOTGEN + UNIT.Name);
                }
            }

            return fulFile;
        }

        private void process_Exited(object sender, EventArgs e)
        {
            if (_processTable.Count == 0) return;
            Process process = sender as Process;

            string[] itemIOFileANDEXE = (string[])_processTable[process];
            string item = itemIOFileANDEXE[0];
            string exeFile = itemIOFileANDEXE[1];
            string outFile = itemIOFileANDEXE[3];
            string infile = itemIOFileANDEXE[2];
            int sampleID = Convert.ToInt32(itemIOFileANDEXE[4]);

            _processTable.Remove(process);

            try
            {
                //delete files and move
                Application.OpenForms[0].Invoke(updateFiles(sampleID, infile, outFile, exeFile));

                //find results Matssf
                Application.OpenForms[0].Invoke(DoMatSSF(sampleID, outFile));

                //find chilian
                Application.OpenForms[0].Invoke(DoChMethod(sampleID));

                ////done!
                EventHandler updateToDo = delegate
                {
                    UnitRow UNIT = null;
                    UNIT = Interface.IPopulate.ISamples.GetUnitBySampleID(sampleID);
                    UNIT.IsBusy = false;

                    _showProgress?.Invoke(null, EventArgs.Empty);
                };
                Application.OpenForms[0].Invoke(updateToDo);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
            finally
            {
                //save and report
                Application.OpenForms[0].Invoke(finalize(sampleID));
            }
        }

        private void reportFinished(string sampleName, string project)
        {
            string msg = FINISHED_SAMPLE + sampleName + " (" + project + ")";
            Interface.IReport.Msg(msg, DONE);
        }

        private EventHandler updateFiles(int subSampleID, string infile, string outFile, string exeFile)
        {
            EventHandler updateToDo = delegate
            {
                UnitRow UNIT = null;
                UNIT = Interface.IPopulate.ISamples.GetUnitBySampleID(subSampleID);
                //SAVE COPY IN BACKUPS TO LATER ZIP
                string pathBase = Interface.IStore.FolderPath
                + Properties.Resources.Backups
                + "\\" + UNIT.SubSamplesRow.IrradiationCode + "\\";
                if (!Directory.Exists(pathBase))
                {
                    Directory.CreateDirectory(pathBase);
                }
                File.Copy(_startupPath + outFile, pathBase + outFile, true);
                File.Copy(_startupPath + infile, pathBase + infile, true);
                File.Delete(_startupPath + infile);
                File.Delete(_startupPath + exeFile);

                _showProgress?.Invoke(null, EventArgs.Empty);
            };
            return updateToDo;
        }
    }
}