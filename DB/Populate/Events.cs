using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb;
namespace DB
{
    public partial class LINAA
    {
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
        /*
        protected internal void mUESRequiredEvent(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;
    //        int i = TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
            MUESDataTable mues = GetMUES(ref m, false);
            MUES.Merge(mues);
            //return i != 0;
        }
        */

        protected internal void addCompositionsEvent(object sender, EventArgs e)
        {
            try
            {
                MatrixRow m = sender as MatrixRow;
                CompositionsDataTable dt = null;
                if (m.IsCompositionTableNull())
                {
                    //averiguar argumentos...
                    IEnumerable<CompositionsRow> compos = AddCompositions(ref m, null, false);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";

                    dt = new CompositionsDataTable(false);
                    foreach (CompositionsRow item in compos)
                    {
                        dt.ImportRow(item);
                    }

                    byte[] arr = Rsx.Dumb.Tables.MakeDTBytes(ref dt, path);

                    m.CompositionTable = arr;

                    // Save(ref m);
                }
                else
                {
                    byte[] arr = m.CompositionTable;
                    dt = Compositions;
                    Rsx.Dumb.Tables.ReadDTBytes(ref arr, ref dt);
                }
            }
            catch (SystemException ex)
            {
                AddException(ex);
                //throw;
            }
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
        protected internal void populateSelectedExpression(bool setexpression)
        {
            string expression = string.Empty;
            if (setexpression)
            {
                expression = "Parent(Measurements_Peaks).Selected";
            }
            // PopulatePreferences();
            Peaks.SelectedColumn.Expression = expression;
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

        protected internal void handlersSamples()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Standards));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Monitors));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Unit));


            tableSubSamples.AddMatrixHandler = this.addMatrixEvent;

            this.tableUnit.AddSSFsHandler = addSSFEvent;
            this.tableUnit.CleanSSFsHandler = cleanSSFEvent;

            // tableIRequestsAverages.ChThColumn.Expression = " ISNULL(1000 *
            // Parent(SigmasSal_IRequestsAverages).sigmaSal / Parent(SigmasSal_IRequestsAverages).Mat
            // ,'0')"; tableIRequestsAverages.ChEpiColumn.Expression = " ISNULL(1000 *
            // Parent(Sigmas_IRequestsAverages).sigmaEp / Parent(Sigmas_IRequestsAverages).Mat,'0')
            // "; tableIRequestsAverages.SDensityColumn.Expression = " 6.0221415 * 10 *
            // Parent(SubSamples_IRequestsAverages).DryNet / (
            // Parent(SubSamples_IRequestsAverages).Radius * (
            // Parent(SubSamples_IRequestsAverages).Radius + Parent(SubSamples_IRequestsAverages).FillHeight))";
        }

        public void AddSSFs(ref UnitRow u)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";
            MatSSFDataTable dt = u.GetMatSSFTableNoFile();
            this.tableMatSSF.Merge(dt, false, MissingSchemaAction.AddWithKey);
        }
        protected internal void handlersDetSol()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(DetectorsAbsorbers));
        }

        protected internal void handlersGeometries()
        {
          
            this.tableMatrix.AddCompositionsHandler += addCompositionsEvent;
    //        this.tableMatrix.MUESRequiredHandler += mUESRequiredEvent;
            this.tableMatrix.CleanCompositionsHandler += cleanCompositionsEvent;
            this.tableMatrix.CleanMUESHandler += cleanMUESEvent;

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Matrix));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(VialType));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Geometry));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SubSamples));
        }

        private void cleanMUESEvent(object sender, EventArgs e)
        {
            MatrixRow u = sender as MatrixRow;
            EventData b = e as EventData;
          
            CleanMUES(ref u,(bool)b.Args[0] );

        }
    }
}