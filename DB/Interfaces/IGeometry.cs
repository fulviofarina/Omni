using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IGeometry
    {
       MatrixRow AddMatrix(int? SubSamplesID = null, int? templateID = null);

        VialTypeRow AddVial(bool aRabbit);

        GeometryRow DefaultGeometry { get; }

        GeometryRow FindReferenceGeometry(string refName);

        void PopulateCompositions();

        void PopulateGeometry();

   //     void PopulateMatrix();
        void PopulateMatrixSQL();

        void PopulateUnits();

        void PopulateVials();

        void PopulateMUESList();

       
   

        void AddCompositions(ref MatrixRow m, ref IEnumerable<CompositionsRow> compos, IList<string[]> ls = null, bool code = true);
      //  void AddMUES(ref MUESDataTable mu, ref MatrixRow m);
        MUESDataTable GetMUES(ref MatrixRow m, bool sql = true);
        MUESDataTable GetMUES(double el, double eh, int matrixID);
    }
}