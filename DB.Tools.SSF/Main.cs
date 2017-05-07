using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Rsx.Dumb; using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        public static bool RUN(string[] units, bool hide = true)
        {
            string workDir = StartupPath;
            //  workDir += "\\Temp";
            string scriptFile = "ssf";
            string basepath = workDir + "\\" + scriptFile;

            foreach (string item in units)
            {
                //basepath = "/c " + basepath + ".bat";
                string cmd = "cmd";
                string[] ioFile = new string[] { item, item + ".txt" };
                IO.Process(cmd, "/c matssf2.exe", workDir, ioFile);
            }
            return true;
        }
  /*
   *      public static bool RUN(string[] units,  bool hide = true)
        {
            string content = string.Empty;
          //  string matSSFEXE = "matssf2.exe";
            int counter = 1;
            string terminate = "\n";
            counter = 1;
            foreach (string item in units)
            {
                content += "dim a" + counter.ToString() + terminate;
                content += "a" +counter.ToString() +" = \"" +
                    item + "{ENTER}"+ item + ".txt{ENTER}" + "\"" + terminate;

                counter++;
            }
content+= "Set WshShell = Wscript.CreateObject(\"Wscript.Shell\")"+ terminate;
            content += "Set U = CreateObject(\"Shell.Application\")" + terminate;
        //    content += "Set WshShell = Wscript.CreateObject("Wscript.Shell")
content += "U.ShellExecute \"matssf2.exe\", \"\", \"\", \"runas\", 1"+terminate;
            counter = 1;

            foreach (string item in units)
            {

                content += "WshShell.SendKeys a" + counter.ToString() + terminate;
                counter++;
            }

            string workDir = StartupPath;
          //  workDir += "\\Temp";
            string scriptFile = "ssf";
            string basepath = workDir + "\\"+ scriptFile ;

            File.WriteAllText(basepath+ ".vbs", content);

           
             //now execute the VB scripts 1 and 2 for Container and Server MSMQ installation

            content = "cmd /c ssf.vbs";
            File.WriteAllText(basepath+ ".bat", content);

   

         
         
            return true;

        }

     */


        /// <summary>
        /// This is the table for the epithermal self-shielding factors
        /// </summary>
        public static LINAA.MatSSFDataTable Table = null;

        /// <summary>
        /// This is the main row with the data
        /// </summary>
        public static LINAA.UnitRow UNIT = null;

        private static string exefile = "matssf2.exe";
        private static string inputFile = "MATSSF_INP.TXT";
        private static string outputFile = "MATSSF_LST.TXT";
        private static string startupPath = string.Empty;

        /// <summary>
        /// The input MatSSF file
        /// </summary>
        public static string InputFile
        {
            set
            {
                inputFile = value;
            }
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
            set
            {
                outputFile = value;
            }
            get
            {
                return outputFile;
            }
        }

        /// <summary>
        /// The MatSSF startup path
        /// </summary>
        public static string StartupPath
        {
            get { return startupPath; }
            set { startupPath = value; }
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
                    EC.SetRowError(m, ex);
                }
            }

            double sumTh = abs.Sum() * SDensity * factor;
            //Put the Sum of Thermal absorbers in ChTh of UnitRow
            UNIT.ChTh = sumTh;

            UNIT.GtCh = (A1 / (Math.Pow(UNIT.ChTh * 1e-12 * UNIT.kth, pUniTh) + 1.0));    // and since is inherited by parentRelation...

            abs.Clear();
            elements.Clear();
        }

        /// <summary>
        /// Generates the INPUT File for MatSSF
        /// </summary>
        public static bool INPUT(bool defaultVal)
        {
            bool success = false;
            IList<string[]> ls = UNIT.SubSamplesRow.MatrixRow.StripComposition(UNIT.SubSamplesRow.MatrixRow.MatrixComposition);
            string buffer = UNIT.SubSamplesRow.MatrixRow.StripMoreComposition(ref ls);
            string config = getChannelCfg(defaultVal);

            double lenfgt = UNIT.SubSamplesRow.FillHeight;
            double diamet = UNIT.SubSamplesRow.Radius * 2;
            if (diamet != 0 && lenfgt != 0 && !config.Equals(String.Empty))
            {
                buffer += "\n";
                buffer += UNIT.SubSamplesRow.Net + "\n" + diamet + "\n" + lenfgt + "\n" 
                    + config;
                      
                buffer += "\n";
                buffer += "\n";
            }
            else return success;
            // string fileInput = ;

            string fulFile = startupPath + inputFile;

            System.IO.File.Delete(startupPath + outputFile + ".txt");
            System.IO.File.Delete(fulFile); //delete fromRow

            success = writeFile(buffer, fulFile);

            return success;
        }

        /// <summary>
        /// Link the Unit Row to the Parent Matrix, Vial, Channel, Rabbit
        /// </summary>
     

        /// <summary>
        /// Generates the OUTPUT File for MatSSF
        /// </summary>
        public static bool   OUTPUT(string unitName)
        {
            string File = startupPath + unitName + ".txt";
      

            //read file data
            string lecture =IO.ReadFile(File);
            IEnumerable<string> array = lecture.Split('\n');
            array = array.Where(o => !o.Equals("\r"));
            array = array.Select(o => o.Trim(null)).AsEnumerable();
            //fill the unit data
            fillUnitWithText(ref array);
            //fill the matssf table
            LINAA.MatSSFDataTable table = UNIT.FillSSFTable(array.ToList());

            UNIT.SSFTable =   Tables.MakeDTBytes(ref table, startupPath);

            array = null;
            bool isOk = table.Count != 0;

            Dumb.FD(ref table);
            return isOk;
        }
        /*
        /// <summary>
        /// Reads the MatSSF datatable from an xml file
        /// </summary>
        public static bool ReadXML()
        {
            Table.Clear();
            if (UNIT.IsSSFTableNull()) return false;
            //file to save as
            string file = startupPath + Guid.NewGuid().ToString() + ".xml";
            //get bytes
            byte[] bites = UNIT.SSFTable;

            //write to file
            IO.WriteFileBytes(ref bites, file);
            //read from file
            Table.ReadXml(file);
            //delete file
            File.Delete(file);

            return true;
        }
        */
        /*
        /// <summary>
        /// Runs the MatSSF program
        /// </summary>
        public static bool RUN(bool Hide)
        {
            Process process = new Process();

            File.Delete(startupPath + outputFile); //delete output

            IO.Process(process, startupPath, exefile, String.Empty, Hide, true, 100000);

            return process.HasExited;
        }
        */
        /*
        /// <summary>
        /// Writes the MatSSF datatable to an xml file and assigns it to the row object
        /// </summary>
        public static bool WriteXML()
        {
            string file = startupPath + Guid.NewGuid().ToString() + ".xml";

            File.Delete(file);

     
            //write to file
            Table.WriteXml(file, XmlWriteMode.IgnoreSchema);

            //read bytes
            byte[] bites =IO.ReadFileBytes(file);

            //asign
           

            //delete file
            File.Delete(file);

            return true;
        }
        */
    }
}