using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Rsx
{
    public partial class Dumb
    {
        /// <summary>
        /// Reads a data table from an array of bytes and loads the dataTable Destiny with the contents
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">the name of the file to temporaryly create</param>
        /// <param name="auxiliar">array of bytes with datatable content</param>
        /// <param name="DestinyDataTable">table where data should be loaded</param>
        public static void ReadDTBytes<T>(string file, ref byte[] auxiliar, ref T DestinyDataTable)
        {
            Dumb.WriteBytesFile(ref auxiliar, file);
            DataTable toLoad = DestinyDataTable as DataTable;

            try
            {

          //      toLoad.BeginLoadData();

                DataTable table = new DataTable();
                table.ReadXml(file);
            //    FillErrorEventHandler hanlder = fillhandler;
                
                toLoad.Merge( table, true, MissingSchemaAction.AddWithKey);
            //    toLoad.EndLoadData();
                toLoad.AcceptChanges();

                System.IO.File.Delete(file);
            }
            catch (Exception ex)
            {

            }
            //    auxiliar = null;

        }

      

        public static void WriteBytesFile(ref byte[] r, string destFile)
        {
            try
            {
                System.IO.FileStream f = new System.IO.FileStream(destFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                f.Write(r, 0, Convert.ToInt32(r.Length));
                f.Close();
            }
            catch (SystemException ex)
            {
                System.Windows.Forms.MessageBox.Show("El archivo está abierto");
            }
        }

        public static byte[] MakeDTBytes<T, T2>(ref IEnumerable<T> answ, ref T2 adt, string afile)
        {
            IEnumerable<DataRow> rows = answ as IEnumerable<DataRow>;
            DataTable dt = adt as DataTable;

            foreach (DataRow a in rows) dt.LoadDataRow(a.ItemArray, LoadOption.OverwriteChanges);
            dt.WriteXml(afile, XmlWriteMode.WriteSchema, true);
            dt.Clear();
            dt.Dispose();
            byte[] arr = ReadFileBytes(afile);
            System.IO.File.Delete(afile);
            return arr;
        }

        public static byte[] ReadFileBytes(string file)
        {
            System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            int size = Convert.ToInt32(stream.Length);
            Byte[] rtf = new Byte[size];
            stream.Read(rtf, 0, size);
            stream.Close();
            stream.Dispose();
            return rtf;
        }
    }
}
