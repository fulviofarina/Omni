using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
      
            
        partial class IrradiationRequestsDataTable
        {
            public int? FindIrrReqID(String project)
            {
                int? IrrReqID = null;

                LINAA.IrradiationRequestsRow irr = this.FindByIrradiationCode(project);
                if (irr != null) IrrReqID = irr.IrradiationRequestsID;

                return IrrReqID;
            }

            public IrradiationRequestsRow FindByIrradiationCode(string project)
            {
                project = project.ToUpper();
                return this.FirstOrDefault(i => i.IrradiationCode.CompareTo(project) == 0);
            }

            private IEnumerable<DataColumn> nonNullables;

            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] { this.columnChannelName, this.columnIrradiationCode, this.columnNumber, this.columnIrradiationStartDateTime };
                    }
                    return nonNullables;
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    IrradiationRequestsRow r = e.Row as IrradiationRequestsRow;
                    if (NonNullables.Contains(e.Column))
                    {
                        bool nu = Dumb.CheckNull(e.Column, e.Row);
                        if (e.Column == this.columnIrradiationCode && !nu)
                        {
                            r.Number = Convert.ToInt32(r.IrradiationCode.Substring(r.IrradiationCode.Length - 2));
                            if (r.ChannelsRow == null)
                            {
                                string _projectNr = System.Text.RegularExpressions.Regex.Replace(r.IrradiationCode, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (!_projectNr.Equals(string.Empty))
                                {
                                    string code = r.IrradiationCode.Replace(_projectNr, null);
                                    r.ChannelsRow = (this.DataSet as LINAA).Channels.FirstOrDefault(o => o.IrReqCode.ToUpper().CompareTo(code) == 0);
                                }
                            }
                        }
                        // return;
                    }
                }
                catch (SystemException ex)
                {
                    e.Row.SetColumnError(e.Column, ex.Message);
                }
            }
        }
    }
}