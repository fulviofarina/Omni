using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Rsx;

namespace DB.Tools
{
    public partial class MatSSF
    {
        private static string exefile = "matssf.exe";
        private static string startupPath;
        private static string inputFile = "MATSSF_INP.TXT";
        private static string outputFile = "MATSSF_LST.TXT";

        public static string StartupPath
        {
            get { return startupPath; }
            set { startupPath = value; }
        }

        public static string InputFile
        {
            get
            {
                return inputFile;
            }

            set
            {
                inputFile = value;
            }
        }

        public static LINAA.UnitRow UNIT = null;
        public static LINAA.MatSSFDataTable Table = null;
        public static LINAA.SubSamplesRow Sample = null;

        public static bool WriteXML()
        {
            string file = startupPath + UNIT.Name;
            File.Delete(file);
            Table.WriteXml(file);

            byte[] bites = Dumb.ReadFileBytes(file);

            UNIT.SSFTable = bites;

            File.Delete(file);

            return true;
        }

        public static bool ReadXML()
        {
            //   System.IO.File.Delete(file);
            //
            Table.Clear();

            if (UNIT.IsSSFTableNull()) return false;

            string file = startupPath + UNIT.Name;
            byte[] bites = UNIT.SSFTable;
            Dumb.WriteBytesFile(ref bites, file);
            Table.ReadXml(file);
            //  UNIT.SSFTable = bites;
            File.Delete(file);

            return true;
        }

        public static bool INPUT()
        {
            bool success = false;
            string buffer = getDecomposedMatrix();
            string config = getChannelCfg();

            double lenfgt = UNIT.SubSamplesRow.FillHeight;
            double diamet = UNIT.SubSamplesRow.Radius * 2;
            if (diamet != 0 && lenfgt != 0 && !config.Equals(String.Empty))
            {
                buffer += "\n";
                buffer += UNIT.Mass + "\n" + diamet + "\n" + lenfgt + "\n" + config;
                buffer += "\n";
                buffer += "\n";
            }
            else return success;
            // string fileInput = ;

            string fulFile = startupPath + inputFile;

            System.IO.File.Delete(fulFile); //delete fromRow

            success = writeFile(buffer, fulFile);

            return success;
        }

        public static void CHILEAN()
        {
            //sample geometry dependant values// why recalculate each time? leave them here
            HashSet<double> abs = new HashSet<double>();
            HashSet<string> elements = new HashSet<string>();

            IEnumerable<LINAA.MatSSFRow> rows = UNIT.GetMatSSFRows();

            UNIT.GtCh = 1;
            double surf = (UNIT.SubSamplesRow.Radius) * ((UNIT.SubSamplesRow.Radius) + (UNIT.SubSamplesRow.FillHeight));
            double SDensity = 6.0221415 * 10 * UNIT.Mass / surf;
            double Xi = 1e-12 * SDensity * UNIT.kepi;
            Int32 factor = 10000;

            foreach (LINAA.MatSSFRow m in rows)
            {
                try
                {
                    LINAA.SigmasSalRow sal = m.ReactionsRowParent.SigmasSalRow;
                    LINAA.SigmasRow sepi = m.ReactionsRowParent.SigmasRowParent;

                    //     double kth = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kth;
                    //    double kepi = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kepi;

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

                        m.SSFCh = (0.94 / (Math.Pow(Xi * m.ChEpi * (m.Weight * factor), 0.82) + 1.0)) + 0.06;
                    }
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(m, ex);
                }
            }

            double sumTh = abs.Sum() * SDensity * factor;
            //Put the Sum of Thermal absorbers in ChTh of UnitRow
            UNIT.ChTh = sumTh;

            UNIT.GtCh = (1.0 / (Math.Pow(UNIT.ChTh * 1e-12 * UNIT.kth, 0.964) + 1.0));    // and since is inherited by parentRelation...

            abs.Clear();
            elements.Clear();
        }

        public static String OUTPUT()
        {
            string File = startupPath + outputFile;

            if (!System.IO.File.Exists(File)) return File;

            string lecture = Dumb.ReadFile(File);
            IEnumerable<string> array = lecture.Split('\n');
            array = array.Where(o => !o.Equals("\r"));
            array = array.Select(o => o.Trim(null)).AsEnumerable();

            UNIT.FillWith(ref array);

            fillTable(array.ToList());

            array = null;

            if (Sample != null) Sample.MatSSFDensity = UNIT.Density;

            Table.GtDensity[1] = UNIT.Density.ToString();
            if (Sample != null) Sample.Gthermal = UNIT.Gt;

            Table.GtDensity[0] = UNIT.Gt.ToString();

            return File;
        }

        public static bool RUN(bool Hide)
        {
            Process process = new Process();

            File.Delete(startupPath + outputFile); //delete output

            Dumb.Process(process, startupPath, exefile, String.Empty, Hide, true, 100000);

            return process.HasExited;
        }
    }
}