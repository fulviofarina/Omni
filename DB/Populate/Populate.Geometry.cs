using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;
using Rsx.Dumb;

//using DB.Interfaces;

namespace DB
{
    public partial class LINAA : IGeometry
    {
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

        public void AddCompositions(IEnumerable<MatrixRow> matrices = null)
        {
            try
            {
                matrices = addCompositions(ref matrices);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
        }

        public IEnumerable<CompositionsRow> AddCompositions(ref MatrixRow m, IList<string[]> listOfFormulasAndCompositions = null, bool code = true)
        {
            IList<CompositionsRow> compos = new List<CompositionsRow>();
            try
            {
                addCompositions(ref m, listOfFormulasAndCompositions, code, ref compos);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return compos;
        }

        public MatrixRow AddMatrix(int SubSamplesID, int templateID)
        {
            MatrixRow m = null;
            try
            {
                m = addMatrix();
                m.setBasic(SubSamplesID, templateID);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return m;
        }

        public MatrixRow AddMatrix()
        {
            MatrixRow v = null;//Interface.IDB.Matrix.NewMatrixRow();
            v = addMatrix();
            return v;
        }

        public VialTypeRow AddVial(bool aRabbit)
        {
            VialTypeRow v = null;
            try
            {
                v = addVial(aRabbit);
            }
            catch (SystemException ex)
            {
                // EC.SetRowError(v, ex);
                AddException(ex);
            }
            return v;
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
        // PopulateCompositions ,
        PopulateMatrixSQL,
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
       
        public void PopulateMatrixSQL()
        {
            try
            {
                // this.tableMatrix.BeginLoadData();
                this.tableMatrix.Clear();
                this.tableMatrix.AcceptChanges();
                //  PopulateMUESList();

                MatrixDataTable m = new MatrixDataTable();
            
                    tAM.MatrixTableAdapter.ClearBeforeFill = true;
                    tAM.MatrixTableAdapter.Fill(m);
          
                this.tableMatrix.Merge(m, false, System.Data.MissingSchemaAction.AddWithKey);
                // this.tableMatrix.EndLoadData(); Save(ref this.tableMatrix); this.tableMatrix.AcceptChanges();
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

     

   

    

        public MUESDataTable GetMUES(ref MatrixRow m, bool sql = true)
        {
        
            MUESDataTable mu = new MUESDataTable();

            if (sql)
            {
                TAM.MUESTableAdapter.FillByMatrixID(mu,m.MatrixID);
            }
            else
            {

                if (m.IsXCOMTableNull()) return mu;
                byte[] arr = m.XCOMTable;
                Rsx.Dumb.Tables.ReadDTBytes(ref arr, ref mu);
            }
            return mu;
        }

      

        public MUESDataTable GetMUES(double el, double eh, int matrixID)
        {
            return TAM.MUESTableAdapter.GetDataByMatrixIDAndEnergy(el, eh, matrixID);
        }
    }
}