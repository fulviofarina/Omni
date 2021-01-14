using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Rsx.Dumb;
using static DB.LINAA;
using static Rsx.Dumb.CalculateBase.TXT;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected static string findEXEFile(ref Process process)
        {
            string exeF = process.StartInfo.Arguments;
            int index = exeF.IndexOf(EXEFILE[0]);
            exeF = process.StartInfo.Arguments.Substring(index);
            return exeF;
        }

        protected bool checkInputData(ref UnitRow UNIT)
        {
            bool hasError = checkIfSampleHasErrors(ref UNIT);

            bool isOK = reportIfSampleHasErrors(ref UNIT, hasError);

            return isOK;
        }

        protected string generateMatSSFEXEFile(string INFile)
        {
            bool inputExist = File.Exists(_startupPath + INFile);
            string newMatssfEXEFile = string.Empty;
            if (inputExist)
            {
                newMatssfEXEFile = INFile.Replace(INPUT_EXT, EXE_EXT);
                //copy .exe file peersonalized for the unit
                string copyIn = _startupPath + EXEFILE;
                string copyOut = _startupPath + newMatssfEXEFile;
                File.Copy(copyIn, copyOut, true);
                //runAProcess(hide, item, );
                Interface.IReport.Msg(CLONING_OK, CLONING_OK_TITLE);
            }

            return newMatssfEXEFile;
        }

        protected static string getGUID(string item)
        {
            return "m" + item + "." + Guid.NewGuid().ToString().Substring(0, 5);
        }

        protected LINAA.MatSSFDataTable fillSSFTableWith(ref LINAA.UnitRow u, IList<string> fileContent, bool fillSSF)
        {
            string[] content = null;

            int sep = fileContent.IndexOf(MATSSF_TABLE_SEPARATOR);
            fileContent = fileContent.Skip(sep + 1).ToList();

            LINAA.MatSSFDataTable table = new LINAA.MatSSFDataTable(false);

            foreach (string item in fileContent)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    if (item.CompareTo(MATSSF_TABLE_SEPARATOR) == 0) continue;
                    content = item.Substring(10).Trim().Split(' ');
                    content = content.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
                    // Z, the element and A data
                    string[] ZEl = item.Substring(0, 10).Split('-');
                    if (ZEl.Count() < 2) continue;
                    ZEl[2] = ZEl[2].Trim();
                    if (string.IsNullOrEmpty(ZEl[2])) continue;
                    Int16 A = Convert.ToInt16(ZEl[2]);
                    //interested only in the isotopes
                    if (A > 0)
                    {
                        ZEl[0] = ZEl[0].Trim();// Z
                        ZEl[1] = ZEl[1].Trim(); // Element

                        LINAA.MatSSFRow m = table.NewMatSSFRow();
                        m.UnitID = u.UnitID;
                        m.SubSamplesID = u.SampleID;
                        fillSSRowFWith(ref m, content, ZEl, fillSSF);
                        table.AddMatSSFRow(m);
                    }
                }
                catch (SystemException ex)
                {
                    Interface.IStore.AddException(ex);
                    EC.SetRowError(u, ex);
                }
            }

            return table;
        }

        protected void fillSSRowFWith(ref MatSSFRow mat, string[] content, string[] ZEl, bool fillSSF)
        {
            string radioisotope = ZEl[1] + "-" + (Convert.ToInt32(ZEl[2]) + 1).ToString();
            string targetIsotope = ZEl[1] + "-" + ZEl[2];

            mat.RadioIsotope = radioisotope;
            mat.TargetIsotope = targetIsotope;
            // m.SSF = -1;

            // m.GFast = -1;
            mat.SigB = Convert.ToDouble(content[3]);
            mat.Weight = Convert.ToDouble(content[1]);
            mat.ND = Convert.ToDouble(content[2]);
            if (content.Length == 6 && fillSSF)
            {
                mat.SSF = Convert.ToDouble(content[4]);
                mat.GFast = Convert.ToDouble(content[5]);
            }
            else if (content.Length == 5 && fillSSF) mat.GFast = Convert.ToDouble(content[4]);
        }

        protected string getChannelCfg(bool defaultVal, ref LINAA.UnitRow UNIT)
        {
            string chCfg = string.Empty;

            chCfg = UNIT.ChCfg[0] + "\n" + (2 * UNIT.ChRadius * 0.1) + "\n" + (UNIT.ChLength * 0.1) + "\n";
            if (!defaultVal)
            {
                chCfg += UNIT.BellFactor;
                // chCfg += UNIT.nFactor + "\n"; chCfg += UNIT.WGt + "\n";
            }
            chCfg += "\n";
            if (!defaultVal)
            {
                // chCfg += UNIT.BellFactor + "\n";
                chCfg += UNIT.nFactor;
                // chCfg += UNIT.WGt + "\n";
            }
            chCfg += "\n";
            if (!defaultVal)
            {
                // chCfg += UNIT.BellFactor + "\n"; chCfg += UNIT.nFactor + "\n";
                chCfg += UNIT.WGt;
            }
            chCfg += "\n";
            return chCfg;
        }

        /// <summary>
        /// Fills the UnitRow with the given physical parameters
        /// </summary>
        /// <param name="Mdens">  density matrix</param>
        /// <param name="Gtermal">thermal SSF</param>
        /// <param name="EXS">    Escape X section</param>
        /// <param name="MCL">    Mean Chord Lenght</param>
        /// <param name="PXS">    Potential X section</param>
        protected void fillUnitWith(ref LINAA.UnitRow u, string Mdens, string EXS, string MCL, string PXS)
        {
            try
            {
                // double aux2 = 0;

                if (!Mdens.Equals(string.Empty))
                {
                    double densityCode = Dumb.Parse(Mdens, 0);
                    if (!u.SubSamplesRow.IsCalcDensityNull())
                    {
                        double dens2 = (double)Decimal.Round(Convert.ToDecimal(u.SubSamplesRow.CalcDensity), 4);
                        int factor = 10; // in %
                        double relError = (densityCode / dens2) - 1;
                        double diffFactor = Math.Abs(relError * 100);
                        if (diffFactor > factor)
                        {
                            string mistmatch = DENSITY_MISMATCH + factor.ToString();
                            string col = Interface.IDB.SubSamples.CalcDensityColumn.ColumnName;
                            u.SubSamplesRow
                                .SetColumnError(col, mistmatch);
                        }
                    }
                    u.SubSamplesRow.CalcDensity = densityCode;
                }

                if (!EXS.Equals(string.Empty))
                {
                    double aux2 = Dumb.Parse(EXS, 0);

                    u.EXS = aux2 / 10;
                }
                if (!MCL.Equals(string.Empty))
                {
                    double aux2 = Dumb.Parse(MCL, 0);

                    u.MCL = aux2 * 10;
                }
                if (!PXS.Equals(string.Empty))
                {
                    double aux2 = Dumb.Parse(MCL, 0);

                    u.PXS = aux2 / 10;
                }
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected string prepareInputs(ref LINAA.UnitRow UNIT)
        {
            //2
            string unitName = UNIT.Name;
            //delete .out file

            //generate input file
            bool defaultValues = !Interface.IPreferences.CurrentSSFPref.Overrides;

            string buffer = string.Empty;

            IEnumerable<MatrixRow> matrices = UNIT.SubSamplesRow.GetMatrixRows();
            foreach (MatrixRow mat in matrices)
            {
                //suck into buffer th matrix composition of each matrix
                string composition = mat.MatrixComposition;
                IList<string[]> ls = RegEx.StripComposition(composition);
                buffer += RegEx.StripMoreComposition(ref ls);
                // buffer += " ";
            }

            string config = getChannelCfg(defaultValues, ref UNIT);

            double lenfgt = UNIT.SubSamplesRow.FillHeight;
            double diamet = UNIT.SubSamplesRow.Radius * 2;
            double mass = UNIT.SubSamplesRow.Net;

            if (diamet != 0 && lenfgt != 0 && !string.IsNullOrEmpty(config))
            {
                buffer += "\n";
                buffer += mass + "\n" + diamet + "\n" + lenfgt + "\n"
                    + config;
                buffer += "\n";
                buffer += "\n";
            }
            else throw new SystemException(INPUT_NOTGEN + unitName);
            return buffer;
        }
    }
}