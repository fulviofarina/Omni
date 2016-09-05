using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
      

        partial class ProjectsDataTable
        {
            public ProjectsRow FindBy(int? IrReqId, int? orderID, bool addIfNull)
            {
                LINAA.ProjectsRow pro = null;
                if (IrReqId == null || orderID == null) return pro;
                pro = this.FirstOrDefault(p => p.IrradiationRequestsID == IrReqId && p.OrdersID == orderID);
                if (pro == null && addIfNull)
                {
                    pro = this.NewProjectsRow();
                    pro.IrradiationRequestsID = (int)IrReqId;
                    pro.OrdersID = (int)orderID;
                    this.AddProjectsRow(pro);
                }

                return pro;
            }
        }

    
    }
}