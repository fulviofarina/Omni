using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        public partial class IrradiationRequestsRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableIrradiationRequests.Columns)
                {
                    Check(column);
                }
                // return this.GetColumnsInError().Count() != 0;
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                return colsInE.Intersect(this.tableIrradiationRequests.ForbiddenNullCols)
                    .Count() != 0;
            }

            protected internal LINAA db
            {
                get
                {
                    return this.Table.DataSet as LINAA;
                }
            }

            protected string _projectNr
            {
                get
                {
                    return Regex.Replace(IrradiationCode, "[a-z]", String.Empty, RegexOptions.IgnoreCase);
                }
            }

            public void Check(DataColumn column)
            {
                bool nu = EC.CheckNull(column, this);
                if (column == this.tableIrradiationRequests.IrradiationCodeColumn && !nu)
                {
                    SetChannel();
                }
            }

            internal ChannelsRow GetChannel()
            {
                ChannelsRow ch = null;
                if (!_projectNr.Equals(string.Empty))
                {
                    string code = IrradiationCode.Replace(_projectNr, null);
                    ch = db.Channels.FirstOrDefault(o => o.IrReqCode.ToUpper().CompareTo(code) == 0);
                }

                return ch;
            }

            internal string GetIrradiationCode()
            {
                string code = string.Empty;
                if (!IsIrradiationCodeNull()) return IrradiationCode.Trim().ToUpper();
                else return code;
            }

            internal void SetChannel()
            {
                if (EC.IsNuDelDetch(ChannelsRow))
                {
                    ChannelsRow = GetChannel();
                }
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                //throw new NotImplementedException();
            }
        }
    }
}