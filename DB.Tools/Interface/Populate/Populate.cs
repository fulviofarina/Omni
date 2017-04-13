using System;
using System.Data.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Rsx;

namespace DB.Tools
{
    public partial class Populate
    {
      
        private static string sqlStarted = "Installation of SQL LocalDB started. When finished click OK to restart";
        private static string sqlPack32 = "localdbx32.msi";
        private static string sqlPack64 = "localdbx64.msi";
        private static string triedToInstall = "\n\nThe user tried to install SQL Express";
        private static string deniedTheInstall = "\n\nThe user denied the SQL Express installation";
        private static string nocontinueWOSQL = "Cannot continue without a SQL connection";
        private static string sqlDBEXE = "SqlLocalDB.exe";
     
        private static string shouldInstallSQL = "Would you like to install SQL LocalDB?";
        private static string sqlLocalDB = "SQL LocalDB Installation";
    }
  

    public partial class Populate 
    {

        private Interface Interface;

        private static void insertSQL(ref ITable dt, ref ITable ita)
        {
            foreach (var i in dt)
            {
                ita.InsertOnSubmit(i);
            }

            ita.Context.SubmitChanges();
        }

        private void populateDirectory(string path, bool overrider)
        {
            try
            {
                if (!Directory.Exists(path) || overrider)
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }
        }

        private void populateFeaturesDirectory(string features, string currentpath)
        {
            try
            {
                bool feats = File.Exists(features);
                if (feats)
                {
                    File.Copy(features, currentpath, true);
                    File.Delete(features);
                    Help();
                }
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }
        }
        private void populateReplaceFile(string path, string developerPath)
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
        }
        private void populateMatSSFResource(bool overriderFound)
        {
            string matssf = Interface.IMain.FolderPath + Resources.SSFFolder;
            bool nossf = !Directory.Exists(matssf);
            if (nossf || overriderFound)
            {
                Directory.CreateDirectory(matssf);
                string resourcePath = Application.StartupPath + "\\" + Resources.SSFResource + ".bak";
                string startexecutePath = Interface.IMain.FolderPath + Resources.SSFFolder;
                string destFile = startexecutePath + Resources.SSFResource + ".CAB";
                unpackResource(resourcePath, destFile, startexecutePath, true);
            }
        }
        private void populateSolCoiResource(bool overriderFound)
        {
            string solcoi = Interface.IMain.FolderPath + Resources.SolCoiFolder;
            bool nosolcoi = !Directory.Exists(solcoi);

            if (nosolcoi || overriderFound)
            {
                Directory.CreateDirectory(solcoi);
                string startexecutePath = Interface.IMain.FolderPath + Resources.SolCoiFolder;

                string resourcePath = Application.StartupPath + "\\" + Resources.CurvesResource + ".bak";
                string destFile = startexecutePath + Resources.CurvesResource + ".bak";
                unpackResource(resourcePath, destFile, startexecutePath, false);

                resourcePath = Application.StartupPath + "\\" + Resources.SolCoiResource + ".bak";
                destFile = startexecutePath + Resources.SolCoiResource + ".bak";
                unpackResource(resourcePath, destFile, startexecutePath, false);
            }
        }
      
        private void unpackResource(string resourcePath, string destFile, string startExecutePath, bool unpack)
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