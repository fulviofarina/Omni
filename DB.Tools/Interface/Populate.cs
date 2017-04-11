using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using DB.Properties;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class Populate
    {
        public void Help()
        {
            string path = Interface.IMain.FolderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
        }

        public void PopulateDirectory(string path)
        {
            string result = string.Empty;
            try
            {
                DirectorySecurity secutiry = new DirectorySecurity(path, AccessControlSections.All);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path, secutiry);
                }
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
            }
        }

        public void PopulateResources(bool overriderFound)
        {
            string path = string.Empty;
            try
            {
                path = Interface.IMain.FolderPath + Resources.Exceptions;
                populateDirectory(path, overriderFound);

                path = Interface.IMain.FolderPath + Resources.Backups;
                populateDirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            try
            {
                populateSolCoiResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            try
            {
                populateMatSSFResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }
        }

        public bool PopulateOverriders()
        {
            string path;
            //override preferences
            path = Interface.IMain.FolderPath + Resources.Preferences + ".xml";
            string developerPath = Application.StartupPath + "\\" + Resources.Preferences + "Dev.xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.SSFPreferences + ".xml";
            developerPath = Application.StartupPath + "\\" + Resources.SSFPreferences + "Dev.xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.WCalc;
            developerPath = Application.StartupPath + "\\" + Resources.WCalc;
            populateReplaceFile(path, developerPath);

            path = Interface.IMain.FolderPath + Resources.XCOMEnergies;
            developerPath = Application.StartupPath + "\\" + Resources.XCOMEnergies;
            populateReplaceFile(path, developerPath);

            // path = folderPath + Resources.SolCoiFolder;

            bool overriderFound = false;
            try
            {
                //does nothing
                path = Application.StartupPath + "\\" + Resources.ResourcesOverrider;
                overriderFound = File.Exists(path);
                //TODO:
                if (overriderFound) File.Delete(path);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            path = Application.StartupPath + "\\" + Resources.Features;
            string currentpath = Interface.IMain.FolderPath + Resources.Features;
            populateFeaturesDirectory(path, currentpath);

            return overriderFound;
        }

        //   public void Preferences()
        //  {
        //   db.PopulatePreferences();
        //  I.
        // }
        private void installResources(string solcoi, string matssf)
        {
            //VOLVER A PONER TODO
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

    public partial class Populate
    {
        public IDetSol IDetSol;
        public IGeometry IGeometry;
        public IIrradiations IIrradiations;

        //  public IExpressions IExpressions;
        public INuclear INuclear;

        public IOrders IOrders;
        public IProjects IProjects;
        public ISamples ISamples;
        public ISchedAcqs ISchedAcqs;
        public IToDoes IToDoes;
        private Interface Interface;

        public Populate(ref Interface inter)
        {
            LINAA aux = inter.Get();
            Interface = inter;

            // IExpressions = (IExpressions)aux;
            INuclear = (INuclear)aux;
            IProjects = (IProjects)aux;
            IIrradiations = (IIrradiations)aux;
            IGeometry = (IGeometry)aux;
            IDetSol = (IDetSol)aux;
            IOrders = (IOrders)aux;
            ISamples = (ISamples)aux;
            ISchedAcqs = (ISchedAcqs)aux;
            IToDoes = (IToDoes)aux;
        }
    }
}