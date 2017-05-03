using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

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
            // this.PopulateColumnExpresions()
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

            // this.notify;
        }

        public void Read(string filepath)
        {
            LINAA dt = null;

            // file.EnforceConstraints = false;
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

                //MergePreferences();
                // this.PopulateSSFPreferences();
            }
            catch (Exception ex)
            {
                this.AddException(ex);
            }

            Dumb.FD<LINAA>(ref dt);
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
         // s.CapsulesID = id; s.ENAA = enaa;

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