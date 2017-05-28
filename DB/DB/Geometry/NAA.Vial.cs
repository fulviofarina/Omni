﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx.Dumb; using Rsx;

namespace DB
{
    public partial class LINAA
    {

        public partial class VialTypeRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableVialType.Columns)
                {
                    Check(column);
                }
                //   return this.GetColumnsInError().Count() != 0;
            }
            public void Check(DataColumn Column)
            {
                bool nu = EC.CheckNull(Column, this);
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                return colsInE.Intersect(this.tableVialType.ForbiddenNullCols).Count() != 0;
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }
        }
        partial class VialTypeDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnMatrixDensity,
                            this.columnMatrixName,
                            this.columnVialTypeRef ,
                        this.columnMaxFillHeight,
                        this.InnerRadiusColumn};
                    }
                    return nonNullables;
                }
            }

            
          
        }
    }
}