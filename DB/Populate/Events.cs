using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rsx;

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
            //     s = m.SubSamplesRow;
            //  Save(ref s);

        }
        protected internal void mUESRequiredEvent(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;

            int i = TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
            PopulateMUESList();

            //return i != 0;
        }

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

          //     Save(ref m);
            }
            else
            {
                byte[] arr = m.CompositionTable;
                dt = Compositions;
                Rsx.Dumb.Tables.ReadDTBytesNoFile(ref arr, ref dt);

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
            cleanCompositions(ref compos);
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

        protected internal void handlersDetSol()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(DetectorsAbsorbers));
        }

        protected internal void handlersGeometries()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Matrix));

            this.tableMatrix.AddCompositionsHandler += addCompositionsEvent;
            this.tableMatrix.MUESRequiredHandler += mUESRequiredEvent;
            this.tableMatrix.CleanCompositionsHandler += cleanCompositionsEvent;

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(VialType));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Geometry));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(SubSamples));
        }



    }
   


}
