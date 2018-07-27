using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

//using DB.Interfaces;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA : IStore
    {


        private bool cleanMUESPics(ref MatrixRow u)
        {
            string path = this.folderPath + Properties.Resources.XCOMFolder;

            string[] files = System.IO.Directory.GetFiles(path, u.MatrixID.ToString() + ".*");
            bool ok = true;

            foreach (var item in files)
            {
                if (System.IO.File.Exists(item))
                {
                    System.IO.File.Delete(item);
                    ok = ok && !System.IO.File.Exists(item);

                }
            }

            return ok;
        }

        private bool cleanMUES(ref MatrixRow m, bool sql = true)
        {

            //    MUESDataTable mu = new MUESDataTable();
            bool ok = false;
            if (sql)
            {
                TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
            }
            else
            {
                string tempfile = GetMUESFile(ref m);
                if (System.IO.File.Exists(tempfile)) System.IO.File.Delete(tempfile);
            }
            IEnumerable<MUESRow> mues = m.GetMUESRows();
            Delete<MUESRow>(ref mues);
            this.MUES.AcceptChanges();


           // ok =  cleanMUESPics(ref m);


            ok =  GetMUES(ref m, sql).Count == 0;
            return ok;

        }

        public void CleanOthers()
        {
            //Interface.IDB.SSFPref.Clear();
            Exceptions.Clear();
            MatSSF.Clear();

            MUES.Clear();
            MUES.AcceptChanges();
            MatSSF.AcceptChanges();
            Exceptions.AcceptChanges();
        }

        public void Delete<T>(ref IEnumerable<T> rows2)
        {
            if (rows2 == null || rows2.Count() == 0) return;
            //my first reverse loop... seems to work perfectly
            for (int i = rows2.Count() - 1; i >= 0; i--)
            {
                DataRow op = rows2.ElementAt(i) as DataRow;
                try
                {
                    if (!EC.IsNuDelDetch(op))
                    {
                        op.Delete();
                    }
                }
                catch (SystemException ex)
                {
                    this.AddException(ex);
                }
            }
        }

        public bool DeletePeaks(Int32 measID)
        {
            LINAATableAdapters.PeaksTableAdapter peaksTa = new LINAATableAdapters.PeaksTableAdapter();
            bool success = false;
            try
            {
                peaksTa.DeleteByMeasurementID(measID);
                success = true;
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
            peaksTa.Dispose();
            peaksTa = null;
            return success;
        }
    }
}