﻿using System;
using System.Data;
using System.Linq;
using Rsx;

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
                // return this.GetColumnsInError().Count() != 0;
            }

            public void Check(DataColumn Column)
            {
                bool nu = EC.CheckNull(Column, this);
                if (nu && Column == this.tableVialType.VialTypeRefColumn)
                {
                        VialTypeRef = "New @ " + DateTime.Now.ToLocalTime();
                }
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableVialType.ForbiddenNullCols).Count();
                return count != 0;
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }
        }
    }
}