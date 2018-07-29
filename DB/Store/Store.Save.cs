using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;

//using DB.Interfaces;
using Rsx.Dumb;
using System.IO;

namespace DB
{
    public partial class LINAA : IStore
    {

        /*
        public void SaveMatrices(bool offile)
        {
            if (offile)
            {
                SaveLocalCopy();
            }
            else
            {
                IEnumerable<DataTable> tables = new DataTable[] { Matrix, Compositions };
                SaveTables(ref tables);
            }
        }
        */
        public bool SaveMUES(ref MUESDataTable mu, ref MatrixRow m, bool sql = true)
        {



            if (sql)
            {
                IEnumerable<MUESRow> mues = m?.GetMUESRows();
                Delete(ref mues);
                saveMUES_SQL(ref mu, ref m);
            }

            bool filesOk = saveMUES_File(ref mu, m);
            return filesOk;
        }

     

        public bool SaveTable<T>()
        {
            T table = this.Tables.OfType<T>().FirstOrDefault();

            DataTable dt = table as DataTable;
            if (dt.Rows.Count == 0) return true;

            IEnumerable<DataRow> rows = dt.AsEnumerable();

            bool saved = SaveRows(ref rows);

            return saved;
        }

        public bool Save(string file)
        {
            bool saved = false;

            try
            {
                System.IO.File.Delete(file);
                this.WriteXml(file, XmlWriteMode.WriteSchema);
                saved = true;
                //Msg("Database has been updated to a file!\n\n" + file, "Saved");
            }
            catch (SystemException ex)
            {
                // Msg(ex.StackTrace, ex.Message);
                AddException(ex);
            }

            return saved;
        }

        public bool SaveRows<T>(ref IEnumerable<T> rows)
        {
            if (rows == null) return false;

          
            rows = rows.ToList(); //solidify any query
            if (rows.Count() == 0) return false;

            Type t = rows.First().GetType();

            DataTable dt = (rows.First() as DataRow).Table;

            if (useHandlers)
            {
                // setHandlers(false, ref dt);
            }
            // setRowHandlers(ref dt, false);

            dt.BeginLoadData();

            save(ref rows);

            if (useHandlers)
            {
                // setHandlers(true, ref dt); useHandlers = false;
            }
            else
            {
                // useHandlers = true; Save(ref rows);
            }

            // setRowHandlers(ref dt, true);

            dt.EndLoadData();

            return true;
        }


        public void Save<T>(ref T row)
        {
            // this.BeginEndLoadData(true);
            List<T> list = new List<T>();
            list.Add(row);
            IEnumerable<T> rows = list.ToArray();
            SaveRows(ref rows);
            // this.BeginEndLoadData(false);
        }

        public string SaveExceptions()
        {
            string path = string.Empty;
            try
            {
                this.Exceptions.RemoveDuplicates();
                if (this.Exceptions.Rows.Count != 0)
                {
                    long now = DateTime.Now.ToFileTimeUtc();
                    path = folderPath + DB.Properties.Resources.Exceptions + "Exceptions." + now + ".xml";
                    this.Exceptions.WriteXml(path, XmlWriteMode.WriteSchema, false);
                }
                this.Exceptions.Clear();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
                path = string.Empty;
            }

            return path;
        }


        private string extension = ".xml";


        public bool SaveLocalCopy(string LIMSPath = "")
        {
            bool ok = false;

            try
            {

                //    string LIMSPath = string.Empty;
                if (string.IsNullOrEmpty(LIMSPath))
                {
                    LIMSPath = folderPath + Resources.Backups + Resources.Linaa;
                    makeBackup(LIMSPath);
                }
                //     CleanPreferences();
                CleanOthers();
                AcceptChanges();

                WriteXml(LIMSPath, XmlWriteMode.WriteSchema);

                ok = true;
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return ok;
        }

        private void makeBackup(string LIMSPath)
        {
            string aux = "." + DateTime.Now.DayOfYear.ToString();
            string LIMSDayPath = LIMSPath.Replace(extension, aux + extension);

            if (System.IO.File.Exists(LIMSPath))
            {
                System.IO.File.Copy(LIMSPath, LIMSDayPath, true);
                System.IO.File.Delete(LIMSPath);
            }
          
        }

        /*
public bool ReadLocalCopy()
{
   bool ok = false;

   try
   {

       if (System.IO.File.Exists(LIMSPath))
       {
           ReadXml(LIMSPath, XmlReadMode.ReadSchema);
       }


       ok = true;
   }
   catch (SystemException ex)
   {
       AddException(ex);
   }
   return ok;
}
*/
        public bool SaveTables(ref IEnumerable<DataTable> tables)
        {
            bool ok = false;

            foreach (System.Data.DataTable t in tables)
            {
                try
                {
                    IEnumerable<DataRow> rows = t.AsEnumerable();
                    SaveRows(ref rows);

                    ok = true;
                }
                catch (SystemException ex)
                {
                    AddException(ex);
                }
            }

            return ok;
        }


    }

 
}