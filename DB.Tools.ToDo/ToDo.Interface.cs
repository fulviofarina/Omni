using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DB.Tools
{
  public interface IToDo
  {
    decimal[] Average(string field, string fieldSD, string filterCol, ref IEnumerable<DataRow> rows);

    string CalculateAi();

    string CalculateFC();

    string CreateSelectReject(ref DataGridView dgv, ref BindingSource bs);

    void PropagateSR();

    IFitted Fit { get; }
    IEnumerable<LINAA.ToDoRow> IList { get; set; }
    IEnumerable<LINAA.ToDoAvgRow> IAvgs { get; set; }

    string Prepare(short minPos);

    void SelectToDoesGroup();

    void SetParameters(string alpha0box, string f0box, bool opTimize, bool useRefe);

    void SetToDoesGroup();

    DB.LINAA.ToDoType SetToDoType(string ToDoTypeText);

    string Reset();
  }

  public interface IFitted
  {
    decimal AlphaSD { get; set; }
    decimal Alpha { get; set; }
    decimal f { get; set; }
    decimal R2 { get; set; }
    decimal SEAlpha { get; set; }
    decimal SEf { get; set; }
    System.Collections.Generic.IList<double> X { get; set; }
    System.Collections.Generic.IList<double> XLog { get; set; }
    System.Collections.Generic.IList<double> Y { get; set; }
    System.Collections.Generic.IList<double> YCalc { get; set; }
    System.Collections.Generic.IList<double> YErrorHigh { get; set; }
    System.Collections.Generic.IList<double> YErrorLow { get; set; }
    System.Collections.Generic.IList<double> YLog { get; set; }
    System.Collections.Generic.IList<string> Isotopes { get; set; }

    System.Collections.Generic.IList<double> Alphas { get; set; }
    System.Collections.Generic.IList<double> Qo { get; set; }
    System.Collections.Generic.IList<double> Quantity { get; set; }
  }
}