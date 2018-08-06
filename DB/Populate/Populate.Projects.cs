//using DB.Interfaces;
using DB.LINAATableAdapters;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public partial class LINAA : IProjects
    {
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
                activeProjectsList = Hash.HashFrom<string>(tableIRequestsAverages.ProjectColumn, Properties.Misc.Cd, string.Empty);

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

        public ProjectsRow FindBy(int? IrReqId, int? orderID, bool addIfNull)
        {
            ProjectsRow pro = null;
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
        //

        /// <summary>
        /// Acomodar para que use un solo TAM
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ListOfHLProjects()
        {

            IEnumerable<string> arr = null;
            MeasurementsDataTable meas = new MeasurementsDataTable();
            try
            {

                this.tAM.MeasurementsTableAdapter.FillDataByHL(meas);
            }
            catch (Exception ex)
            {
                AddException(ex);
              
            }
            arr = meas.Select(o => o.Project.ToUpper()).Distinct();
            Dumb.FD(ref meas);

            return arr;
        }

        public void PopulateProjects()
        {
            try
            {
            
                this.tableProjects.Clear();

                this.tAM.ProjectsTableAdapter.Fill(this.tableProjects);
         
                this.tableProjects.AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}