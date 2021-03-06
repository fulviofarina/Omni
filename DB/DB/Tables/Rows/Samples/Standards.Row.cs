﻿using Rsx.Dumb;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        partial class StandardsRow : IRow
        {
            public new bool HasErrors()
            {
                return base.HasErrors;
            }

            public void Check()
            {
                foreach (DataColumn column in this.tableStandards.Columns)
                {
                    Check(column);
                }
                // return this.GetColumnsInError().Count() != 0;
            }

            public void Check(DataColumn column)
            {
                bool nulo = EC.CheckNull(column, this);

                if (column == this.tableStandards.MonitorCodeColumn)
                {
                    if (nulo) return;

                    if (GeometryRow == null && MatrixRow != null)
                    {
                        LINAA l = ((LINAA)this.tableStandards.DataSet);
                        GeometryDataTable gdt = l.tableGeometry;
                        GeometryRow g = gdt.NewGeometryRow();
                        g.GeometryName = MonitorCode;
                        gdt.AddGeometryRow(g);
                        g.MatrixID = MatrixID;
                        g.VialTypeID = l.tableVialType.FirstOrDefault(o => o.VialTypeRef.CompareTo("Bare") == 0).VialTypeID;
                        GeometryRow = g;
                    }
                }
            }
        }
    }
}