using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Rsx;

namespace DB.Tools
{
    public partial class SolCoin : IDisposable
    {
        public class IOFiles
        {
            public String COI;
            public String DirectoryPath;
            public String Input;
            public bool OK;

            public String ReferenceEfficiency;

            public String ReferencePTT;

            public String ReferenceSolidAngles;

            public String SolidAngles;

            public bool Gather(String directoryPath, String Geometry, int Position, String Detector, String ReferenceGeometry, int ReferencePosition)
            {
                OK = false;
                DirectoryPath = directoryPath + Geometry + "\\";
                Input = Position + Geometry + Detector + ".SIN";
                SolidAngles = Position + Geometry + Detector + ".SOL";
                COI = Position + Geometry + Detector + ".COI";
                ReferenceEfficiency = "EFF" + ReferencePosition + ReferenceGeometry + Detector + ".DAT";
                ReferencePTT = "PTT" + ReferencePosition + ReferenceGeometry + Detector + ".DAT";
                ReferenceSolidAngles = ReferencePosition + ReferenceGeometry + Detector + ".SOL";
                OK = true;

                IList<string> mainfiles = Directory.GetFiles(directoryPath).Select(o => o.Replace(directoryPath, null).ToUpper()).ToList();

                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                foreach (string f in mainfiles)
                {
                    string infile = directoryPath + f;
                    if (!File.Exists(DirectoryPath + f))
                    {
                        File.Copy(infile, DirectoryPath + f, true);
                        if (f.Contains(".EXE"))
                        {
                            Dumb.Process(new Process(), DirectoryPath, f, "/T:" + DirectoryPath, true, true, 1000000);
                        }
                    }
                }

                if (Geometry.CompareTo(ReferenceGeometry) != 0)
                {
                    string rDir = directoryPath + ReferenceGeometry;
                    if (Directory.Exists(rDir))
                    {
                        string[] reffiles = Directory.GetFiles(rDir).Select(o => o.Replace(directoryPath + ReferenceGeometry + "\\", null)).ToArray();

                        reffiles = reffiles.Where(o => o.Contains(ReferenceGeometry)).ToArray();
                        reffiles = reffiles.Where(o => o.Contains(Detector)).ToArray();
                        reffiles = reffiles.Where(o => !o.Contains("EFF")).ToArray();
                        reffiles = reffiles.Where(o => !o.Contains("PTT")).ToArray();

                        foreach (string f in reffiles)
                        {
                            int dur = 0;
                            string infile = directoryPath + ReferenceGeometry + "\\" + f;
                            string of = DirectoryPath + f;
                            if (File.Exists(of))
                            {
                                DateTime t = File.GetLastWriteTime(of);
                                DateTime i = File.GetLastWriteTime(infile);
                                dur = DateTime.Compare(t, i);
                            }
                            else dur = -1;
                            if (dur < 0) File.Copy(infile, of, true);
                        }
                    }
                }

                return OK;
            }

            // public String FilesPath = String.Empty;
        }
    }
}