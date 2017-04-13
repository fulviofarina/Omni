﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        private static string exefile = "matssf.exe";
        private static string startupPath = string.Empty;
        private static string inputFile = "MATSSF_INP.TXT";
        private static string outputFile = "MATSSF_LST.TXT";

        /// <summary>
        /// The MatSSF startup path
        /// </summary>
        public static string StartupPath
        {
            get { return startupPath; }
            set { startupPath = value; }
        }

        /// <summary>
        /// The input MatSSF file
        /// </summary>
        public static string InputFile
        {
            get
            {
                return inputFile;
            }
        }

        /// <summary>
        /// The output MatSSF file
        /// </summary>
        public static string OutputFile
        {
            get
            {
                return outputFile;
            }
        }

        /// <summary>
        /// This is the main row with the data
        /// </summary>
        public static LINAA.UnitRow UNIT = null;
        /// <summary>
        /// This is the table for the epithermal self-shielding factors
        /// </summary>
        public static LINAA.MatSSFDataTable Table = null;

        /// <summary>
        /// Is it used? should not
        /// </summary>
        public static LINAA.SubSamplesRow Sample = null;

        /// <summary>
        /// Writes the MatSSF datatable to an xml file and assigns it to the row object
        /// </summary>
        public static bool WriteXML()
        {
            string file = startupPath + UNIT.Name;
            File.Delete(file);
            //write to file
            Table.WriteXml(file);

            //read bytes
            byte[] bites = Dumb.ReadFileBytes(file);

            //asign
            UNIT.SSFTable = bites;

            //delete file
            File.Delete(file);

            return true;
        }

        /// <summary>
        /// Link the Unit Row to the Parent Matrix, Vial, Channel, Rabbit
        /// </summary>
        public static void LinkToParent(ref DataRow row)
        {
            bool isChannel = row.GetType().Equals(typeof(ChannelsRow));
            bool isMatrix = row.GetType().Equals(typeof(MatrixRow));
            if (isChannel)
            {
                ChannelsRow c = row as ChannelsRow;
                UNIT.SetChannel(ref c);
            }
            else if (!isMatrix)
            {
                LINAA.VialTypeRow v = row as VialTypeRow;
                if (!v.IsRabbit) UNIT.SubSamplesRow.VialTypeRow = v;
                else UNIT.SubSamplesRow.VialTypeRowByChCapsule_SubSamples = v;
            }
            else
            {
                MatrixRow m = row as MatrixRow;
                UNIT.SubSamplesRow.MatrixID = m.MatrixID;
            }
        }

        /// <summary>
        /// Reads the MatSSF datatable from an xml file 
        /// </summary>
        public static bool ReadXML()
        {
            // System.IO.File.Delete(file);
            Table.Clear();

            if (UNIT.IsSSFTableNull()) return false;
            //file to save as
            string file = startupPath + UNIT.Name;
            //get bytes
            byte[] bites = UNIT.SSFTable;
            //write to file
            Dumb.WriteBytesFile(ref bites, file);
            //read from file
            Table.ReadXml(file);
            //delete file
            File.Delete(file);

            return true;
        }

        /// <summary>
        /// Generates the INPUT File for MatSSF
        /// </summary>
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
                buffer += UNIT.SubSamplesRow.Net + "\n" + diamet + "\n" + lenfgt + "\n" + config;
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

        /// <summary>
        /// Calculates according to the Chilean method
        /// </summary>
        public static void CHILEAN()
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

            foreach (LINAA.MatSSFRow m in rows)
            {
                try
                {
                    LINAA.SigmasSalRow sal = m.ReactionsRowParent.SigmasSalRow;
                    LINAA.SigmasRow sepi = m.ReactionsRowParent.SigmasRowParent;

                    // double kth = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kth; double
                    // kepi = ir.SubSamplesRow.IrradiationRequestsRow.ChannelsRow.kepi;

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

        /// <summary>
        /// Generates the OUTPUT File for MatSSF
        /// </summary>
        public static String OUTPUT()
        {
            string File = startupPath + outputFile;

            if (!System.IO.File.Exists(File)) return File;


            //read file data
            string lecture = Dumb.ReadFile(File);
            IEnumerable<string> array = lecture.Split('\n');
            array = array.Where(o => !o.Equals("\r"));
            array = array.Select(o => o.Trim(null)).AsEnumerable();
            //fill the unit data
            UNIT.FillWith(ref array);
            //fill the matssf table
            fillTable(array.ToList());
            array = null;



            //fill shit I should delete!!!!
            if (Sample != null) Sample.MatSSFDensity = UNIT.SubSamplesRow.CalcDensity;
            Table.GtDensity[1] = UNIT.SubSamplesRow.CalcDensity.ToString();
            if (Sample != null) Sample.Gthermal = UNIT.Gt;
            Table.GtDensity[0] = UNIT.Gt.ToString();

            return File;
        }

        /// <summary>
        /// Runs the MatSSF program
        /// </summary>
        public static bool RUN(bool Hide)
        {
            Process process = new Process();

            File.Delete(startupPath + outputFile); //delete output

            Dumb.Process(process, startupPath, exefile, String.Empty, Hide, true, 100000);

            return process.HasExited;
        }
    }
}