using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{

    public partial class Creator
    {
        private static bool checkBugFile()
        {
            string cmd = Application.StartupPath + Resources.Restarting;
            bool shouldReport = System.IO.File.Exists(cmd);

            if (shouldReport)
            {
                string email = System.IO.File.ReadAllText(cmd);
                Interface.IReport.GenerateReport(restartingOk, string.Empty, string.Empty, Interface.IDB.DataSetName, email);
                System.IO.File.Delete(cmd);
            }
            shouldReport = shouldReport || Interface.IDB.Exceptions.Count != 0;

            //should send bug report?
            if (!shouldReport) return false;

            //yes...
            Interface.IReport.GenerateBugReport();

            return true;
        }
        private static bool populateOverriders()
        {
            string path;
            //override preferences
            path = Interface.IMain.FolderPath + Resources.Preferences + ".xml";
            string developerPath = Application.StartupPath + Resources.DevFiles + Resources.Preferences+".xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.SSFPreferences + ".xml";
            developerPath = Application.StartupPath + Resources.DevFiles + Resources.SSFPreferences + ".xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.WCalc;
            developerPath = Application.StartupPath + Resources.DevFiles  + Resources.WCalc;
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.XCOMEnergies;
            developerPath = Application.StartupPath + Resources.DevFiles + Resources.XCOMEnergies;
            populateReplaceFile(path, developerPath);

            // path = folderPath + Resources.SolCoiFolder;

            bool overriderFound = false;
            try
            {
                //does nothing
                path = Application.StartupPath + Resources.DevFiles + Resources.ResourcesOverrider;
                overriderFound = File.Exists(path);
                //TODO:
                if (overriderFound) File.Delete(path);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            path = Application.StartupPath + Resources.DevFiles + Resources.Features;
            string currentpath = Interface.IMain.FolderPath + Resources.Features;
             bool features = populateReplaceFile(currentpath, path);
            if (features) Help();

            return overriderFound;
        }

        private static void insertSQL(ref ITable dt, ref ITable ita)
        {
            foreach (var i in dt)
            {
                ita.InsertOnSubmit(i);
            }

            ita.Context.SubmitChanges();
        }

     

     
        private static bool  populateReplaceFile(string path, string developerPath)
        {
            try
            {
                //this overwrites the user preferences for the developers ones. in case I need to deploy them new preferences
                if (File.Exists(developerPath))
                {


                    File.Copy(developerPath, path, true);
                    File.Delete(developerPath);
                }
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            return File.Exists(path);
        }
        private static void populateMatSSFResource(bool overriderFound)
        {
            string matssf = Interface.IMain.FolderPath + Resources.SSFFolder;
            bool nossf = !Directory.Exists(matssf);
            if (nossf || overriderFound)
            {
                Directory.CreateDirectory(matssf);
                string resourcePath = Application.StartupPath + Resources.DevFiles 
                    + Resources.SSFResource + ".bak";
                string startexecutePath = Interface.IMain.FolderPath + Resources.SSFFolder;
                string destFile = startexecutePath + Resources.SSFResource + ".CAB";
                unpackResource(resourcePath, destFile, startexecutePath, true);
            }
        }
        private static void populateSolCoiResource(bool overriderFound)
        {
            string solcoi = Interface.IMain.FolderPath + Resources.SolCoiFolder;
            bool nosolcoi = !Directory.Exists(solcoi);

            if (nosolcoi || overriderFound)
            {
                Directory.CreateDirectory(solcoi);
                string startexecutePath = Interface.IMain.FolderPath + Resources.SolCoiFolder;

                string resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.CurvesResource + ".bak";
                string destFile = startexecutePath + Resources.CurvesResource + ".bak";
                unpackResource(resourcePath, destFile, startexecutePath, false);

                resourcePath = Application.StartupPath + Resources.DevFiles 
                    + Resources.SolCoiResource + ".bak";
                destFile = startexecutePath + Resources.SolCoiResource + ".bak";
                unpackResource(resourcePath, destFile, startexecutePath, false);
            }
        }

        private static void unpackResource(string resourcePath, string destFile, string startExecutePath, bool unpack)
        {
            if (File.Exists(resourcePath))
            {
                File.Copy(resourcePath, destFile);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //conservar esto para unzippear
                if (unpack)
                {
                    Dumb.Process(process, startExecutePath, "expand.exe", destFile + " -F:* " + startExecutePath, false, true, 100000);
                    File.Delete(destFile);
                }
            }
        }

    }

}