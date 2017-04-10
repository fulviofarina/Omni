using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using DB.Properties;

//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IMain
    {
        /*
     public bool Read(string file)

     {
         bool read = false;

         try
         {
             this.Clear();

             if (System.IO.File.Exists(file))
             {
                 this.ReadXml(file, XmlReadMode.InferTypedSchema);
             }
             read = true;
         }
         catch (SystemException ex)
         {
             this.AddException(ex);
         }

         return read;
     }
     */

        private string folderPath = string.Empty;

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        public void AddException(Exception ex)
        {
            this.tableExceptions.AddExceptionsRow(ex);
        }

        public void CloneDataSet(ref LINAA set)
        {
            this.InitializeComponent();
            this.Merge(set, false, MissingSchemaAction.Ignore);
            this.PopulateColumnExpresions();
            this.IRequestsAverages.Clear();
            this.IPeakAverages.Clear();

            DataTable table = IRequestsAverages;
            cleanReadOnly(ref table);
            table = IPeakAverages;
            cleanReadOnly(ref table);
        }

        public void Help()
        {
            string path = folderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
        }

        public void PopulateResourceDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //   created = true;
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }
        }

        public void PopulateUserDirectories()
        {
            string path = string.Empty;

            //override preferences
            path = folderPath + Resources.Preferences + ".xml";
            string developerPath = Application.StartupPath + "\\" + Resources.Preferences + "Dev.xml";
            populateReplaceFile(path, developerPath);

            path = folderPath + Resources.SSFPreferences + ".xml";
            developerPath = Application.StartupPath + "\\" + Resources.SSFPreferences + "Dev.xml";
            populateReplaceFile(path, developerPath);

            path = folderPath + Resources.WCalc;
            developerPath = Application.StartupPath + "\\" + Resources.WCalc;
            populateReplaceFile(path, developerPath);

            path = folderPath + Resources.XCOMEnergies;
            developerPath = Application.StartupPath + "\\" + Resources.XCOMEnergies;
            populateReplaceFile(path, developerPath);

            // path = folderPath + Resources.SolCoiFolder;

            bool overriderFound = false;
            try
            {
                //does nothing
                path = Application.StartupPath + "\\" + Resources.ResourcesOverrider;
                overriderFound = File.Exists(path);

                if (overriderFound) File.Delete(path);
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            path = Application.StartupPath + "\\" + Resources.Features;
            string currentpath = folderPath + Resources.Features;
            populateFeaturesDirectory(path, currentpath);

            try
            {
                path = folderPath + Resources.Exceptions;
                populateDirectory(path, overriderFound);

                path = folderPath + Resources.Backups;
                populateDirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            try
            {
                string solcoi = folderPath + Resources.SolCoiFolder;
                bool nosolcoi = !Directory.Exists(solcoi);

                if (nosolcoi || overriderFound)
                {
                    Directory.CreateDirectory(solcoi);
                    string startexecutePath = folderPath + Resources.SolCoiFolder;

                    string resourcePath = Application.StartupPath + "\\" + Resources.CurvesResource + ".bak";
                    string destFile = startexecutePath + Resources.CurvesResource + ".bak";
                    unpackResource(resourcePath, destFile, startexecutePath, false);

                    resourcePath = Application.StartupPath + "\\" + Resources.SolCoiResource + ".bak";
                    destFile = startexecutePath + Resources.SolCoiResource + ".bak";
                    unpackResource(resourcePath, destFile, startexecutePath, false);
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            try
            {
                string matssf = folderPath + Resources.SSFFolder;
                bool nossf = !Directory.Exists(matssf);
                if (nossf || overriderFound)
                {
                    Directory.CreateDirectory(matssf);
                    string resourcePath = Application.StartupPath + "\\" + Resources.SSFResource + ".bak";
                    string startexecutePath = folderPath + Resources.SSFFolder;
                    string destFile = startexecutePath + Resources.SSFResource + ".CAB";
                    unpackResource(resourcePath, destFile, startexecutePath, true);
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }
        }

        public void Read(string filepath)
        {
            LINAA dt = null;
            PreferencesDataTable prefe = null;
            SSFPrefDataTable ssfPrefe = null;

            //  file.EnforceConstraints = false;
            XmlReader reader = null;
            try
            {
                XmlReaderSettings set = new XmlReaderSettings();
                set.CheckCharacters = false;
                set.ConformanceLevel = ConformanceLevel.Auto;
                set.DtdProcessing = DtdProcessing.Ignore;
                set.IgnoreWhitespace = true;
                set.ValidationFlags = XmlSchemaValidationFlags.None;
                set.ValidationType = ValidationType.None;
                reader = XmlReader.Create(filepath, set);

                dt = new LINAA();

                dt.ReadXml(reader, XmlReadMode.IgnoreSchema);

                prefe = new PreferencesDataTable(this.Preferences);

                ssfPrefe = new SSFPrefDataTable(this.SSFPref);

                mergePreferences(ref prefe);

                mergePreferences(ref ssfPrefe);

                this.SavePreferences();
                this.PopulatePreferences();
                // this.PopulateSSFPreferences();
            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            Dumb.FD<PreferencesDataTable>(ref prefe);

            Dumb.FD<SSFPrefDataTable>(ref ssfPrefe);

            Dumb.FD<LINAA>(ref dt);
        }

        public void RestartingRoutine()
        {
            string cmd = Application.StartupPath + Resources.Restarting;
            if (System.IO.File.Exists(cmd))
            {
                //  restarting = true;
                string email = System.IO.File.ReadAllText(cmd);
                System.IO.File.Delete(cmd);
                GenerateReport("Restarting succeeded...", string.Empty, string.Empty, DataSetName, email);
            }
        }

        private static bool removeDuplicates(DataTable table, string UniqueField, string IndexField, ref TAMDeleteMethod remover)
        {
            bool duplicates = false;

            IList<object> hs = Dumb.HashFrom<object>(table.Columns[UniqueField]);

            if (hs.Count != table.Rows.Count) //there are duplicates!!
            {
                IEnumerable<DataRow> rows = null;
                foreach (object s in hs)
                {
                    rows = table.AsEnumerable();
                    rows = rows.Where(d => d.Field<object>(UniqueField).Equals(s));
                    if (rows.Count() > 1)// there are sample duplicates
                    {
                        rows = rows.OrderByDescending(d => d.Field<object>(IndexField)); //most recent is the first, older the last
                        rows = rows.Take(rows.Count() - 1);
                        foreach (DataRow d in rows)
                        {
                            remover.Invoke(d.Field<int>(IndexField));
                        }
                    }
                }

                hs.Clear();
                hs = null;

                duplicates = true;
            }

            return duplicates;
        }

        private void installResources(string solcoi, string matssf)
        {
            //VOLVER A PONER TODO
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
                AddException(ex);//                throw;
            }
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
                AddException(ex);//                throw;
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
                AddException(ex);//                throw;
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
                    Rsx.Dumb.Process(process, startExecutePath, "expand.exe", destFile + " -F:* " + startExecutePath, false, true, 100000);
                    File.Delete(destFile);
                }
            }
        }

        /*
    public void PopulateSubSamples()
    {
    try
    {
       this.tableSubSamples.Clear();
       this.tableSubSamples.BeginLoadData();
       TAM.SubSamplesTableAdapter.DeleteNulls();
       LINAA.SubSamplesDataTable newsamples = new SubSamplesDataTable(false);
       TAM.SubSamplesTableAdapter.Fill(newsamples);
       this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);

       foreach (LINAA.SubSamplesRow s in this.tableSubSamples)
       {
          if (s.IsCapsulesIDNull()) continue;
          if (s.CapsulesRow == null) continue;
          int id = s.CapsulesRow.VialTypeID;
          bool enaa = s.CapsulesRow.ENAA;
         // s.CapsulesID = id;
     //	 s.ENAA = enaa;

          this.tAM.SubSamplesTableAdapter.UpdateCaps(enaa, id, s.SubSamplesID);
       }

       // LINAA.SetAdded(ref old);
       this.tableSubSamples.EndLoadData();
       this.tableSubSamples.AcceptChanges();
    }
    catch (SystemException ex)
    {
       this.AddException(ex);
    }
    }
    */
    }
}