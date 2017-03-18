using System;

namespace DB
{
  public partial class LINAA
  {
    partial class ToDoAvgDataTable
    {
      public LINAA.ToDoAvgRow AddToDoAvgRow(ref LINAA.ToDoRow todo)
      {
        LINAA.ToDoAvgRow t = NewToDoAvgRow();
        AddToDoAvgRow(t);
        try
        {
          t.ToDoNr = todo.ToDoNr;
          t.IR = todo.IRAvgRow;
          t.IR2 = todo.IRAvgRow2;
        }
        catch (SystemException ex)
        {
          t.use = false;
          Rsx.EC.SetRowError(t, ex);
        }
        return t;
      }

      public System.Data.DataColumn[] NAA
      {
        get
        {
          System.Data.DataColumn[] arr;
          arr = new System.Data.DataColumn[] { this.sampleColumn, this.IsoColumn };
          return arr;
        }
      }

      public System.Data.DataColumn[] ENAA
      {
        get
        {
          System.Data.DataColumn[] arr;
          arr = new System.Data.DataColumn[] { this.sample2Column, this.Iso2Column };
          return arr;
        }
      }
    }

    public partial class ToDoAvgUncDataTable
    {
      public LINAA.ToDoAvgUncRow AddToDoAvgUncRow(int ToDoNr)
      {
        LINAA.ToDoAvgUncRow tu = NewToDoAvgUncRow();
        AddToDoAvgUncRow(tu);
        try
        {
          tu.ToDoNr = ToDoNr;
        }
        catch (SystemException ex)
        {
          Rsx.EC.SetRowError(tu, ex);
        }
        return tu;
      }
    }
  }
}