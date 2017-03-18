using System;
using System.Linq;

namespace DB
{
  public partial class LINAA
  {
    partial class OrdersDataTable
    {
      public Int32? FindOrderID(String LabOrderRef)
      {
        Int32? OrderID = null;

        string Orders = this.LabOrderRefColumn.ColumnName;
        Func<OrdersRow, bool> finder = LINAA.SelectorByField<OrdersRow>(LabOrderRef, Orders);
        LINAA.OrdersRow r = this.FirstOrDefault(finder);
        if (r != null) OrderID = r.OrdersID;

        return OrderID;
      }
    }
  }
}