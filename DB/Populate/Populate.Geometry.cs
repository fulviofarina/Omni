﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB.Properties;
using Rsx.Dumb;
using System.Text;

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
                if (matrices == null) matrices = this.Matrix.AsEnumerable();

                foreach (MatrixRow item in matrices)
                {
                    IList<string[]> stripped = RegEx.StripComposition(item.MatrixComposition);
                    LINAA.MatrixRow m = item;
                    AddCompositions(ref m, stripped, true);
                }

            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
        }



        private void addCompositions(ref MatrixRow m, IList<string[]> listOfFormulasAndCompositions, bool reCode, ref IList<CompositionsRow> compos)
        {
            bool nulo = EC.CheckNull(this.tableMatrix.MatrixCompositionColumn, m);
            if (nulo)
            {
                compos = new List<CompositionsRow>();
                return;
            }
            if (!reCode || listOfFormulasAndCompositions == null)
            {
                listOfFormulasAndCompositions = RegEx.StripComposition(m.MatrixComposition);
                //to store matrix composition
            }

            // .. IList<CompositionsRow> compos = new List<CompositionsRow>();
            string fullComposition = string.Empty;
            //ilst of element and Quantity
            foreach (string[] formCompo in listOfFormulasAndCompositions)
            {
                try
                {
                    //decompose
                    string element = formCompo[0].Trim();
                    // double formulaweight = 0;
                    double quantity = Convert.ToDouble(formCompo[1].Trim());
                    // elementQuantity(formCompo, out element, out quantity, out formulaweight);
                    //CODE COMPOSITION
                    if (reCode)
                    {
                        fullComposition += "#" + element.Trim() + "   (" + quantity.ToString() + ")   ";
                        continue;
                    }

                    CompositionsRow c = addComposition(element, quantity, ref m);
                    compos.Add(c);
                }
                catch (SystemException ex)
                {
                    AddException(ex);
                }
            }
            if (reCode) m.MatrixComposition = fullComposition;
        }

        private CompositionsRow addComposition(string element, double quantity, ref MatrixRow m)
        {
            CompositionsRow c = m.FindComposition(element);
            if (c == null)
            {
                c = this.Compositions.NewCompositionsRow();
                Compositions.AddCompositionsRow(c);
            }
            else quantity += c.Quantity; //add new quantity

            c.SetValues(m.MatrixID, quantity, element);

            if (!EC.IsNuDelDetch(m.SubSamplesRow))
            {
                c.SubSampleID = m.SubSamplesRow.SubSamplesID;
            }

            return c;
        }


        public void CleanCompositions(ref IEnumerable<CompositionsRow> compos)
        {
            Delete(ref compos);
            this.Compositions.AcceptChanges();
            // return Save(ref compos);
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

        public MatrixRow AddMatrix(int? SubSamplesID=null, int? templateID=null)
        {
            MatrixRow m = null;
            try
            {
                m = Matrix.NewMatrixRow() as MatrixRow;
                Matrix.AddMatrixRow(m);
            //    return v;
                m.setBasic(SubSamplesID, templateID);
            }
            catch (SystemException ex)
            {
                AddException(ex);
            }
            return m;
        }

       

        public VialTypeRow AddVial(bool aRabbit)
        {
            VialTypeRow v = null;
            try
            {
                v = VialType.NewVialTypeRow() as VialTypeRow;
                VialType.AddVialTypeRow(v);
                if (aRabbit) v.IsRabbit = true;
                else v.IsRabbit = false;
               
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
                //   if (m.IsXCOMTableNull()) return mu;
                //     byte[] arr = m.XCOMTable;
                //   string tempfile = ASCIIEncoding.ASCII.GetString(arr);
                string tempfile = GetMUESFile(ref m);
                if (!System.IO.File.Exists(tempfile)) return mu;

                byte[] arr = System.IO.File.ReadAllBytes(tempfile);

                Rsx.Dumb.Tables.ReadDTBytes(ref arr, ref mu);
            }
            return mu;
        }

        public string GetMUESFile(ref MatrixRow m)
        {
            string tempfile = folderPath + Resources.XCOMFolder;
            tempfile += m.MatrixID;
            return tempfile;
        }

        public bool CleanMUES(ref MatrixRow m, bool sql = true)
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

            ok = GetMUES(ref m, sql).Count ==0;
            return ok;
           
        }


        public MUESDataTable GetMUES(double el, double eh, int matrixID)
        {
            return TAM.MUESTableAdapter.GetDataByMatrixIDAndEnergy(el, eh, matrixID);
        }
    }
}