using System;
using Rsx.Dumb;
using Rsx;
using System.Collections.Generic;
using System.Data;

namespace DB
{
    public partial class LINAA
    {
      
        public partial class DetectorsAbsorbersRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableDetectorsAbsorbers.Columns)
                {
                    Check(column);
                }
                //   return this.GetColumnsInError().Count() != 0;
            }
            public void Check(DataColumn Column)
            {

                EC.CheckNull(Column, this);
                //  throw new NotImplementedException();
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }
        }

      
    }
}