using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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



        protected internal string folderPath;

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
            this.folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            bool created = false;

            string path = folderPath + DB.Properties.Resources.k0XFolder;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                created = true;
            }

            path = folderPath + DB.Properties.Resources.Exceptions;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                created = true;
            }
            path = folderPath + DB.Properties.Resources.Backups;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                created = true;
            }

            path = folderPath + Properties.Resources.Preferences;
            string developerPath = System.Windows.Forms.Application.StartupPath + Properties.Resources.PreferencesDev;
            //this overwrites the user preferences for the developers ones. in case I need to deploy them new preferences
            if (System.IO.File.Exists(developerPath))
            {
                System.IO.File.Copy(developerPath, path, true);
                System.IO.File.Delete(developerPath);
            }

            path = folderPath + DB.Properties.Resources.SolCoiFolder;
            string overrider = System.Windows.Forms.Application.StartupPath + DB.Properties.Resources.ResourcesOverrider;
            bool overriderFound = System.IO.File.Exists(path);

            string features = System.Windows.Forms.Application.StartupPath + DB.Properties.Resources.Features;

            if (System.IO.File.Exists(features))
            {
                System.IO.File.Copy(features, folderPath + DB.Properties.Resources.Features, true);
                System.IO.File.Delete(features);
                Help();
            }

            string solcoi = folderPath + DB.Properties.Resources.SolCoiFolder;
            string matssf = folderPath + DB.Properties.Resources.MatSSFFolder;

            if (!System.IO.Directory.Exists(solcoi) || !System.IO.Directory.Exists(matssf) || overriderFound)
            {
                InstallResources();
                if (overriderFound) System.IO.File.Delete(overrider);
            }
        }

        public bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref TAMDeleteMethod remover)
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
            LINAA file = null;
            PreferencesDataTable prefe = null;
            //  file.EnforceConstraints = false;
            System.Xml.XmlReader reader = null;
            try
            {
                System.Xml.XmlReaderSettings set = new System.Xml.XmlReaderSettings();
                set.CheckCharacters = false;
                set.ConformanceLevel = System.Xml.ConformanceLevel.Auto;
                set.DtdProcessing = System.Xml.DtdProcessing.Ignore;
                set.IgnoreWhitespace = true;
                set.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
                set.ValidationType = System.Xml.ValidationType.None;
                reader = System.Xml.XmlReader.Create(filepath, set);

                file = new LINAA();
                /*
                Type[] arrayToPreserve = new Type[]{
                        typeof(IrradiationRequestsDataTable)
                     typeof(CapsulesDataTable),
                    typeof(SubSamplesDataTable),
                      typeof(MeasurementsDataTable),
                        typeof(PeaksDataTable),
                          typeof(IPeakAveragesDataTable),
                                      typeof(IRequestsAveragesDataTable)};

                Dumb.Preserve(file, arrayToPreserve);
                 * */
                file.ReadXml(reader, XmlReadMode.IgnoreSchema);
                //   this.Peaks.Clear();
                //  this.IPeakAverages.Clear();
                //  this.IRequestsAverages.Clear();

                prefe = new PreferencesDataTable(this.Preferences);

                this.Merge(file, false, MissingSchemaAction.AddWithKey);
                this.Preferences.Clear();
                this.Preferences.Merge(prefe, false, MissingSchemaAction.AddWithKey);
                this.SavePreferences();
                this.PopulatePreferences();
                this.AcceptChanges();

                /*
               ucSamples s = new ucSamples(this.Linaa);
               s.MBox = this.Main.Box;
               this.Main.userControls.Add(s);
               this.Main.userControls.Add(s.ucSS);
               s.ucSS.Offline = true;
               s.ucSS.Samples = this.Linaa.SubSamples.AsEnumerable();
               s.Populate(true,OFD.SafeFileName.Replace(".xml",string.Empty));
               s.ucSS.Offline = false;
               s.TV.BackColor = System.Drawing.Color.Honeydew;
               s.NewForm();
                  */
                //	  this.Main.Analysis_Click(sender, e);
            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            if (prefe != null)
            {
                prefe.Clear();
                prefe.Dispose();
                prefe = null;
            }
            if (file != null)
            {
                file.Clear();
                file.Dispose();
                file = null;
            }
        }

        public void Help()
        {
            string path = folderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), System.Windows.Forms.Application.StartupPath, "notepad.exe", path, false, false, 0);
        }

        public delegate int TAMDeleteMethod(int index);
    }
}