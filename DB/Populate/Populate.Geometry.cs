using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IGeometry
    {
      //  public Action populateMUESList;

        public GeometryRow DefaultGeometry
        {
            get
            {
                LINAA.GeometryRow defaultGeometry = null;
                defaultGeometry = FindReferenceGeometry("REF");
                if (defaultGeometry == null)
                {
                    string msg = "The Reference Geometry was not found! Please make sure a Geometry named 'REF' exist in the database";
                    throw new System.Exception(msg);
                }

                return defaultGeometry;
            }
        }

        public void AddCompositions()
        {
            foreach (MatrixRow item in this.Matrix.Rows)
            {
                IList<string[]> stripped = item.StripComposition(item.MatrixComposition);
                LINAA.MatrixRow m = item;
                AddCompositions(ref m, stripped, true);
            }
        }

        public IEnumerable<CompositionsRow> AddCompositions(ref MatrixRow m, IList<string[]> listOfFormulasAndCompositions = null, bool code = true)
        {
            IList<CompositionsRow> compos = new List<CompositionsRow>();
            bool nulo = EC.CheckNull(this.tableMatrix.MatrixCompositionColumn, m);
            if (nulo) return compos;
            if (!code || listOfFormulasAndCompositions == null)
            {
                listOfFormulasAndCompositions = m.StripComposition(m.MatrixComposition);
                //to store matrix composition
            }
            string fullComposition = string.Empty;
            //ilst of element and Quantity
            foreach (string[] formCompo in listOfFormulasAndCompositions)
            {
                //decompose
                string element = formCompo[0];
                double formulaweight = 0;
                double quantity = Convert.ToDouble(formCompo[1]);
                // elementQuantity(formCompo, out element, out quantity, out formulaweight);
                //CODE COMPOSITION
                if (code)
                {
                    fullComposition += "#" + element.Trim() + "   (" + quantity.ToString() + ")   ";
                    continue;
                }
                CompositionsRow c = AddCompositions(ref m, element, quantity, formulaweight);
                compos.Add(c);
            }
            if (code) m.MatrixComposition = fullComposition;
            return compos.AsEnumerable();
        }

        public CompositionsRow AddCompositions(ref LINAA.MatrixRow mea, string element, double quantity, double formulaweight)
        {
            CompositionsRow c = null;
            try
            {
                LINAA.MatrixRow m = mea;
                c = m.FindComposition(element);
                if (c == null)
                {
                    c = this.Compositions.NewCompositionsRow();
                    Compositions.AddCompositionsRow(c);
                }

                c.SetValues(m.MatrixID, quantity, element);

                if (c.RowState == System.Data.DataRowState.Detached)
                {
                }

                if (!EC.IsNuDelDetch(m.SubSamplesRow))
                {
                    c.SubSampleID = m.SubSamplesRow.SubSamplesID;
                }
            }
            catch (SystemException ex)
            {
                EC.SetRowError(c, ex);
                AddException(ex);
            }
            return c;
        }

        public MatrixRow AddMatrix()
        {
            MatrixRow v = null;//Interface.IDB.Matrix.NewMatrixRow();
            v = Matrix.NewMatrixRow() as MatrixRow;
            Matrix.AddMatrixRow(v);
            return v;
        }

        public VialTypeRow AddVial(bool aRabbit)
        {
            VialTypeRow v = null;
            v = VialType.NewVialTypeRow() as VialTypeRow;
            VialType.AddVialTypeRow(v);
            if (aRabbit) v.IsRabbit = true;
            else v.IsRabbit = false;
            return v;
        }

        public bool CleanCompositions(ref IEnumerable<CompositionsRow> compos)
        {
            Delete(ref compos);
            return Save(ref compos);
        }

        public GeometryRow FindReferenceGeometry(string refName)
        {
            GeometryRow reference = null;
            reference = this.tableGeometry.FirstOrDefault(LINAA.SelectorGeometryBy(refName));
            return reference;
        }

        public Action[] PMMatrix()
        {
            Action[] populatorArray = null;

            populatorArray = new Action[]   {
            PopulateCompositions ,
        PopulateMatrix,
        // PopulateMUESList,
         PopulateVials,
            PopulateGeometry};

            return populatorArray;
        }

        public void PopulateCompositions()
        {
            try
            {
                this.tableCompositions.BeginLoadData();
                this.tableCompositions.Clear();
                this.tAM.CompositionsTableAdapter.Fill(this.tableCompositions);
                this.tableCompositions.EndLoadData();
                this.tableCompositions.AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateGeometry()
        {
            try
            {
                this.tableGeometry.BeginLoadData();
                this.tableGeometry.Clear();
                this.TAM.GeometryTableAdapter.Fill(this.tableGeometry);
                this.tableGeometry.AcceptChanges();
                this.tableGeometry.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
        public void PopulateMatrix()
        {
            try
            {
                this.tableMatrix.BeginLoadData();
                this.tableMatrix.Clear();
                PopulateMUESList();
                this.TAM.MatrixTableAdapter.Fill(this.tableMatrix);

                this.tableMatrix.EndLoadData();
                this.tableMatrix.AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateMUESList()
        {
            MUESDataTable mues = this.TAM.MUESTableAdapter.GetListOfMatrices();
            MUES.Clear();
            MUES.Merge(mues);
            MUES.AcceptChanges();
        }

        public void PopulateUnits()
        {
            try
            {
                this.tableUnit.BeginLoadData();
                this.tableUnit.Clear();
                this.TAM.UnitTableAdapter.Fill(this.tableUnit);
                this.tableUnit.AcceptChanges();
                this.tableUnit.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        public void PopulateVials()
        {
            try
            {
                this.tableVialType.BeginLoadData();
                this.tableVialType.Clear();
                this.TAM.VialTypeTableAdapter.Fill(this.tableVialType);
                this.tableVialType.AcceptChanges();
                this.tableVialType.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }

        protected internal void addCompositionsEvent(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;
            //averiguar argumentos...
            AddCompositions(ref m, null, false);
        }

        protected internal void cleanCompositionsEvent(object sender, EventArgs e)
        {
            // MatrixRow m = sender as MatrixRow;
            IEnumerable<CompositionsRow> compos = sender as IEnumerable<CompositionsRow>;
            CleanCompositions(ref compos);
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
        protected internal void mUESRequiredEvent(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;

            int i = TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
            PopulateMUESList();

            //return i != 0;
        }
    }
}