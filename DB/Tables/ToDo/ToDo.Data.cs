using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;

namespace DB
{
  public partial class LINAA
  {
    partial class ToDoDataDataTable
    {
      public ToDoDataRow AddToDoDataRow(ref ToDoRow todo, ref IPeakAveragesRow mon, ref IPeakAveragesRow refe)
      {
        ToDoDataRow p = NewToDoDataRow();
        AddToDoDataRow(p);

        try
        {
          //so we have everything need for input, create TodoDataRow

          p.ToDoNr = todo.ToDoNr;
          p.IPAvg = mon;
          p.IPAvg2 = refe;
          p.use = p.ToDoRow.use;    //important
        }
        catch (SystemException ex)
        {
          p.use = false;
          EC.SetRowError(p, ex);
        }

        return p;
      }

      private System.Data.DataColumn[] arr;

      public System.Data.DataColumn[] NAA
      {
        get
        {
          arr = new System.Data.DataColumn[] { this.sample2Column, this.Iso2Column };
          return arr;
        }
      }
    }

    public partial class ToDoDataTable
    {
      public System.Data.DataColumn[] ColsToFilter
      {
        get
        {
          return new System.Data.DataColumn[] { projectColumn, sampleColumn, IsoColumn, refColumn, useColumn };
        }
      }

      public void AddToDoRow(string ToDoLabel, bool Alike, ref IEnumerable<LINAA.IRequestsAveragesRow> ir, ref IEnumerable<LINAA.IRequestsAveragesRow> ir2)
      {
        IList<LINAA.IRequestsAveragesRow[]> list = null;

        LINAA.Comparer<LINAA.IRequestsAveragesRow> comparer = LINAA.Comparers.Alike;
        if (!Alike) comparer = LINAA.Comparers.NotLike;
        list = LINAA.Intersect(ref ir, ref ir2, comparer);

        IList<ToDoRow> subList = this.Where(o => o.label.CompareTo(ToDoLabel) == 0).ToList();
        if (subList.Count == 0) subList = this.ToList();

        foreach (LINAA.IRequestsAveragesRow[] i in list)
        {
          LINAA.ToDoRow n = subList.FirstOrDefault(o => o.IRAvgRow == i[0] && o.IRAvgRow2 == i[1]);
          if (n == null)
          {
            n = NewToDoRow();
            n.IRAvgRow = i[0];
            n.IRAvgRow2 = i[1];
            n.groupDes = null;
            n.groupNr = 0;
            n.label = ToDoLabel;
            n._ref = false;
            n.use = false;
            n.done = false;
            AddToDoRow(n);
          }
        }
        list = null;
        comparer = null;
      }
    }
  }
}