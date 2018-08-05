using Rsx.Dumb;
using System.Data;

namespace DB
{
    public partial class LINAA
    {
        public partial class DetectorsAbsorbersRow : IRow
        {
            public new bool HasErrors()
            {
                return base.HasErrors;
            }

            public void Check()
            {
                foreach (DataColumn column in this.tableDetectorsAbsorbers.Columns)
                {
                    Check(column);
                }
                // return this.GetColumnsInError().Count() != 0;
            }

            public void Check(DataColumn Column)
            {
                EC.CheckNull(Column, this);
                // throw new NotImplementedException();
            }
        }
    }
}