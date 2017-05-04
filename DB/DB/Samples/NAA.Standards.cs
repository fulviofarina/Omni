using System;
using System.Data;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class StandardsDataTable
        {
            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    StandardsRow std = (StandardsRow)e.Row;
                    if (e.Column == this.MonitorCodeColumn)
                    {
                        bool nulo = EC.CheckNull(e.Column, e.Row);

                        if (nulo) return;

                        if (std.GeometryRow == null && std.MatrixRow != null)
                        {
                            LINAA l = ((LINAA)this.DataSet);
                            GeometryDataTable gdt = l.tableGeometry;
                            GeometryRow g = gdt.NewGeometryRow();
                            g.GeometryName = std.MonitorCode;
                            gdt.AddGeometryRow(g);
                            g.MatrixID = std.MatrixID;
                            g.VialTypeID = l.tableVialType.FirstOrDefault(o => o.VialTypeRef.CompareTo("Bare") == 0).VialTypeID;
                            std.GeometryRow = g;
                        }
                    }
                    else if (e.Column == this.stdNameColumn) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.stdProducerColumn) EC.CheckNull(e.Column, e.Row);
                    //  else if (e.Column == this.stdElementColumn) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.MatrixNameColumn) EC.CheckNull(e.Column, e.Row);
                    else if (e.Column == this.stdUncColumn) EC.CheckNull(e.Column, e.Row);
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }
    }
}