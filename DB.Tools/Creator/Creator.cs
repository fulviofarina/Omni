using System;
using System.IO;
using System.Windows.Forms;
using DB.Properties;
using Rsx.Dumb; using Rsx;
using Rsx.Generic;

namespace DB.Tools
{
    public partial class Creator
    {
        private static Interface Interface = null;

        private static Action lastCallBack = null;
        private static Action mainCallBack = null;

        // private static int toPopulate = 0;

        private static Loader worker = null;
       /// <summary>
        /// disposes the worker that loads the data
        /// </summary>
    }

    public partial class Creator
    {
        /// <summary>
        /// populates MatSSF resources only
        /// </summary>
        private static void populateMatSSFResource(bool overriderFound)
        {
            string matssf = Interface.IStore.FolderPath + Resources.SSFFolder;
            bool nossf = !Directory.Exists(matssf);
            if (nossf || overriderFound)
            {
                Directory.CreateDirectory(matssf);
                string resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.SSFResource + ".bak";
                string startexecutePath = Interface.IStore.FolderPath + Resources.SSFFolder;
                string destFile = startexecutePath + Resources.SSFResource + ".CAB";
                IO.UnpackCABFile(resourcePath, destFile, startexecutePath, true);

             //   destFile = startexecutePath + "MATSSF_XSR.ZIP";
             //   resourcePath = destFile;

               // IO.UnpackCABFile(resourcePath, destFile, startexecutePath, true);
            }
        }

        private static bool populateOverriders()
        {
            string path;
            //override preferences
            path = Interface.IStore.FolderPath + Resources.Preferences + ".xml";
            string developerPath = Application.StartupPath + Resources.DevFiles + Resources.Preferences + ".xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IStore.FolderPath + Resources.SSFPreferences + ".xml";
            developerPath = Application.StartupPath + Resources.DevFiles + Resources.SSFPreferences + ".xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IStore.FolderPath + Resources.WCalc;
            developerPath = Application.StartupPath + Resources.DevFiles + Resources.WCalc;
            populateReplaceFile(path, developerPath);

            path = Interface.IStore.FolderPath + Resources.XCOMEnergies;
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
                Interface.IStore.AddException(ex);//                throw;
            }

            path = Application.StartupPath + Resources.DevFiles + Resources.Features;
            string currentpath = Interface.IStore.FolderPath + Resources.Features;
            bool features = populateReplaceFile(currentpath, path);
            if (features) Help();

            return overriderFound;
        }

        private static bool populateReplaceFile(string path, string developerPath)
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
                Interface.IStore.AddException(ex);//                throw;
            }

            return File.Exists(path);
        }

        /// <summary>
        /// populates Solcoi resources only
        /// </summary>
        private static void populateSolCoiResource(bool overriderFound)
        {
            string solcoi = Interface.IStore.FolderPath + Resources.SolCoiFolder;
            bool nosolcoi = !Directory.Exists(solcoi);

            if (nosolcoi || overriderFound)
            {
                Directory.CreateDirectory(solcoi);
                string startexecutePath = Interface.IStore.FolderPath + Resources.SolCoiFolder;

                string resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.CurvesResource + ".bak";
                string destFile = startexecutePath + Resources.CurvesResource + ".bak";
                IO.UnpackCABFile(resourcePath, destFile, startexecutePath, false);

                resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.SolCoiResource + ".bak";
                destFile = startexecutePath + Resources.SolCoiResource + ".bak";
                IO.UnpackCABFile(resourcePath, destFile, startexecutePath, false);
            }
        }

       
    }
}