using System;
using System.Collections.Generic;

//using DB.Interfaces;
using Rsx.Dumb;
using Rsx;
using System.Linq;

namespace DB
{
    public partial class LINAA : IProjects
    {


        public ProjectsRow FindBy(int? IrReqId, int? orderID, bool addIfNull)
        {
            LINAA.ProjectsRow pro = null;
            if (IrReqId == null || orderID == null) return pro;
            pro = this.tableProjects.FirstOrDefault(p => p.IrradiationRequestsID == IrReqId && p.OrdersID == orderID);
            if (pro == null && addIfNull)
            {
                pro = this.tableProjects.NewProjectsRow();
                pro.IrradiationRequestsID = (int)IrReqId;
                pro.OrdersID = (int)orderID;
                this.tableProjects.AddProjectsRow(pro);
            }

            return pro;
        }


        protected ICollection<string> activeProjectsList;
        protected IList<string> projectsList;

        /// <summary>
        /// Gets a non-repeated list of Lab-Order References in the database
        /// </summary>
        public ICollection<string> ActiveProjectsList
        {
            get
            {
                if (activeProjectsList != null) activeProjectsList.Clear();
                activeProjectsList = Hash.HashFrom<string>(tableIRequestsAverages.ProjectColumn, DB.Properties.Misc.Cd, string.Empty);

                return activeProjectsList;
            }
        }

        /// <summary>
        /// Gets a non-repeated list of Irradiation Requests in the database
        /// </summary>
        public IList<string> ProjectsList
        {
            get
            {
                if (projectsList != null) projectsList.Clear();
                projectsList = Hash.HashFrom<string>(this.tableIrradiationRequests.IrradiationCodeColumn);
                return projectsList;
            }
        }

        //

        public void PopulateProjects()
        {
            try
            {
                //Handlers(this.tableSubSamples, false);

                //	Dumb.CleanColumnExpressions(Measurements);
                this.tableProjects.Clear();

                this.TAM.ProjectsTableAdapter.Fill(this.tableProjects);
                //Handlers(this.tableSubSamples, true);
                this.tableProjects.AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}