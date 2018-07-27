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

     

        public bool Save<T>()
        {
            T table = this.Tables.OfType<T>().FirstOrDefault();

            DataTable dt = table as DataTable;
            if (dt.Rows.Count == 0) return true;

            IEnumerable<DataRow> rows = dt.AsEnumerable();

            bool saved = Save(ref rows);

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

        public bool Save<T>(ref IEnumerable<T> rows)
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
            Save(ref rows);
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

        public bool SaveLocalCopy()
        {
            bool ok = false;
            try
            {
                string LIMSPath = folderPath + Resources.Backups + Resources.Linaa;
                string aux = "." + DateTime.Now.DayOfYear.ToString();
                string LIMSDayPath = LIMSPath.Replace(".xml", aux + ".xml");

                //     CleanPreferences();
                CleanOthers();
                AcceptChanges();
                if (System.IO.File.Exists(LIMSPath))
                {
                    System.IO.File.Copy(LIMSPath, LIMSDayPath, true);
                    System.IO.File.Delete(LIMSPath);
                }
                WriteXml(LIMSPath, XmlWriteMode.WriteSchema);

                ok = true;
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return ok;
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
        public bool SaveRemote(ref IEnumerable<DataTable> tables)
        {
            bool ok = false;

            foreach (System.Data.DataTable t in tables)
            {
                try
                {
                    IEnumerable<DataRow> rows = t.AsEnumerable();
                    Save(ref rows);

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