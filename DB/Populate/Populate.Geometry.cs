using System;
using System.Linq;

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

        public GeometryRow FindReferenceGeometry(string refName)
        {
            GeometryRow reference = null;
            reference = this.tableGeometry.FirstOrDefault(LINAA.SelectorGeometryBy(refName));
            return reference;
        }

        public void PopulateCompositions()
        {
            //  string path = folderPath + DB.Properties.Resources.Backups + "Compositions.xml" ;
            // if (!System.IO.File.Exists(path)) return;

            try
            {
                this.tableCompositions.BeginLoadData();
                this.tableCompositions.Clear();
                this.tAM.CompositionsTableAdapter.Fill(this.tableCompositions);

                //      this.tableCompositions.ReadXml(path);
                /*
                 IEnumerable<CompositionsRow> compos = this.tableCompositions.Where(o => o.IsMatrixIDNull());
                 if (compos.Count() != 0)
                 {
                     this.Delete(ref compos);
                     this.Save<CompositionsDataTable>();
                 }

                 compos = tableCompositions.Where(o => o.IsElementNull());
                 if (compos.Count() != 0)
                 {
                     this.Delete(ref compos);
                     this.Save<CompositionsDataTable>();
                 }
                   compos = null;
                      */
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

        //
        public void PopulateMatrix()
        {
            try
            {
                this.tableMatrix.BeginLoadData();
                this.tableMatrix.Clear();
                this.tableMatrix.populateMUESList = PopulateMUESList;
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

        public void PopulateUnits()
        {
            try
            {
                this.tableUnit.BeginLoadData();
                this.tableUnit.Clear();
                this.TAM.UnitTableAdapter.Fill(this.tableUnit);
                this.tableUnit.AcceptChanges();
                this.tableUnit.EndLoadData();
                //    Hashtable bindings = Dumb.BS.ArrayOfBindings(ref bs, "N4");
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

        public void PopulateMUESList()
        {
            MUESDataTable mues = this.TAM.MUESTableAdapter.GetListOfMatrices();
            MUES.Clear();
            MUES.Merge(mues);
            MUES.AcceptChanges();
        }
    }
}