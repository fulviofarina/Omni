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

        protected internal string appPath = Application.StartupPath;

        public string AppPath
        {
            get { return appPath; }
            set { appPath = value; }
        }

        private string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        public void AddException(Exception ex)
        {
            this.tableExceptions.AddExceptionsRow(ex);
        }

        public void PopulateUserDirectories()
        {
            //Documents Folder
            //    this.folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            // bool created = false;

            string path = folderPath + Resources.k0XFolder;

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

            try
            {
                path = folderPath + Resources.Exceptions;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //  created = true;
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            try
            {
                path = folderPath + Resources.Backups;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //  created = true;
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            try
            {
                path = folderPath + Resources.Preferences;
                string developerPath = appPath + Resources.PreferencesDev;
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

            try
            {
                path = folderPath + Resources.SolCoiFolder;
                string overrider = appPath + Resources.ResourcesOverrider;
                bool overriderFound = File.Exists(path);
                if (overriderFound) File.Delete(overrider);
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            try
            {
                string features = appPath + Resources.Features;
                bool feats = File.Exists(features);
                if (feats)
                {
                    string currentpath = folderPath + Resources.Features;
                    File.Copy(features, currentpath, true);
                    File.Delete(features);
                    Help();
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }

            try
            {
                string solcoi = folderPath + Resources.SolCoiFolder;
                string matssf = folderPath + Resources.SSFFolder;
                bool nosolcoi = !Directory.Exists(solcoi);
                bool nossf = !Directory.Exists(matssf);

                if (nosolcoi || nossf) InstallResources();
            }
            catch (SystemException ex)
            {
                AddException(ex);//                throw;
            }
        }

        private static bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref TAMDeleteMethod remover)
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
            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            Dumb.FD<PreferencesDataTable>(ref prefe);

            Dumb.FD<SSFPrefDataTable>(ref ssfPrefe);

            Dumb.FD<LINAA>(ref dt);
        }

        public void Help()
        {
            string path = folderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), appPath, "notepad.exe", path, false, false, 0);
        }

        private delegate int TAMDeleteMethod(int index);
    }
}