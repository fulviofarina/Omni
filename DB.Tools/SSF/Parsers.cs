using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected LINAA.MatSSFDataTable fillSSFTableWith(ref LINAA.UnitRow u, IList<string> fileContent, bool fillSSF)
        {
            string[] content = null;

            string separator = "------------------------------------------------------------------------";
            int sep = fileContent.IndexOf(separator);
            fileContent = fileContent.Skip(sep + 1).ToList();

            LINAA.MatSSFDataTable table = new LINAA.MatSSFDataTable(false);

            foreach (string item in fileContent)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    if (item.CompareTo(separator) == 0) continue;
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

        /// <summary>
        /// Fills the UnitRow with the given physical parameters
        /// </summary>
        /// <param name="Mdens">  density matrix</param>
        /// <param name="Gtermal">thermal SSF</param>
        /// <param name="EXS">    Escape X section</param>
        /// <param name="MCL">    Mean Chord Lenght</param>
        /// <param name="PXS">    Potential X section</param>
        private void fillUnitWith(ref LINAA.UnitRow u, string Mdens, string EXS, string MCL, string PXS)
        {
            try
            {
                double aux2 = 0;

                if (!Mdens.Equals(string.Empty))
                {
                    aux2 = Dumb.Parse(Mdens, 0);
                    double dens1 = Math.Abs(aux2);
                    if (!u.SubSamplesRow.IsCalcDensityNull())
                    {
                        double dens2 = u.SubSamplesRow.CalcDensity;
                        int factor = 10;
                        if (Math.Abs((dens1 / dens2) - 1) * 100 > factor)
                        {
                            EC.SetRowError(u, new SystemException("Calculated density does not match input density by " + factor.ToString()));
                        }
                    }
                    u.SubSamplesRow.CalcDensity = dens1;
                }

                if (!EXS.Equals(string.Empty))
                {
                    aux2 = Dumb.Parse(EXS, 0);

                    u.EXS = aux2 / 10;
                }
                if (!MCL.Equals(string.Empty))
                {
                    aux2 = Dumb.Parse(MCL, 0);

                    u.MCL = aux2 * 10;
                }
                if (!PXS.Equals(string.Empty))
                {
                    aux2 = Dumb.Parse(MCL, 0);

                    u.PXS = aux2 / 10;
                }
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
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
    }
}