using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB
{
    public partial class LINAA
    {
        protected internal void addCompositionsEvent(object sender, EventArgs e)
        {
            try
            {
                MatrixRow m = sender as MatrixRow;
               
                if (m.IsCompositionTableNull())
                {
                    //averiguar argumentos...
                    IEnumerable<CompositionsRow> compos = null;
                    AddCompositions(ref m, ref compos, null, false);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\";

                    CompositionsDataTable dt =  new CompositionsDataTable(false);
                    foreach (CompositionsRow item in compos)
                    {
                        dt.ImportRow(item);
                    }
                    byte[] arr = Rsx.Dumb.Tables.MakeDTBytes(ref dt, path);
                    m.CompositionTable = arr;

                }
                else
                {
                    byte[] arr = m.CompositionTable;
                    CompositionsDataTable dt = null;
                    dt = Compositions;
                    Rsx.Dumb.Tables.ReadDTBytes(ref arr, ref dt);
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
        }

        protected internal void addMatrixEvent(object sender, EventData e)
        {
            object[] args = e.Args as object[];

            MatrixRow m = e.Args[0] as MatrixRow;
            int SubSamplesID = (int)e.Args[1];
            int templatesID = (int)e.Args[2];
            MatrixRow toClone = e.Args[3] as MatrixRow;
            SubSamplesRow s = e.Args[4] as SubSamplesRow;
            //add matrix
            m = AddMatrix(SubSamplesID, templatesID);
            m.SetParent(ref toClone);
            //save matrix and sample
            Save(ref m);

            //update Inedx after save!
            s.MatrixID = m.MatrixID;

            e.Args[0] = m;
            // s = m.SubSamplesRow; Save(ref s);
        }


        protected internal void cleanCompositionsEvent(object sender, EventArgs e)
        {
            // MatrixRow m = sender as MatrixRow;
            IEnumerable<CompositionsRow> compos = sender as IEnumerable<CompositionsRow>;
            CleanCompositions(ref compos);
        }
    }

    public partial class LINAA
    {
        public void AddSSFs(ref UnitRow u)
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";
            MatSSFDataTable dt = u.GetMatSSFTableNoFile();
            this.tableMatSSF.Merge(dt, false, MissingSchemaAction.AddWithKey);
        }

        protected internal void addSSFEvent(object sender, EventArgs e)
        {
            UnitRow u = sender as UnitRow;
            AddSSFs(ref u);
        }

        protected internal void cleanSSFEvent(object sender, EventArgs e)
        {
            IEnumerable<MatSSFRow> ssfs = sender as IEnumerable<MatSSFRow>;
            CleanSSFs(ref ssfs);
        }


        private void cleanMUESEvent(object sender, EventArgs e)
        {
            MatrixRow u = sender as MatrixRow;
            EventData b = e as EventData;
            XCOMPrefRow xcomPref = b.Args[1] as XCOMPrefRow;
            if (xcomPref.AccumulateResults) return;
            PreferencesRow pref = b.Args[0] as PreferencesRow;

            cleanMUES(ref u, !pref.Offline);
            cleanMUESPics(ref u);
            // b.Args = null;
        }
    }
}