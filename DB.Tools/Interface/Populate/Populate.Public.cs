using System;
using System.Data.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Rsx;

namespace DB.Tools
{
   
    public partial class Populate
    {
        public void LoadProject(string ProjectOrOrder, ref int IrrReqID)
        {
            LINAA.IrradiationRequestsRow Irradiation = null;
            Irradiation = Interface.IDB.IrradiationRequests.FindByIrradiationCode(ProjectOrOrder);

            if (Irradiation == null) return;
            if (IrrReqID == Irradiation.IrradiationRequestsID) return;

            IrrReqID = Irradiation.IrradiationRequestsID;

            Interface.IBS.SubSamples.SuspendBinding();
            Interface.IBS.Units.SuspendBinding();

            Interface.IPopulate.ISamples.PopulateSubSamples(IrrReqID);
            Interface.IPopulate.ISamples.PopulateUnitsByProject(IrrReqID);


            Interface.IPreferences.CurrentPref.LastIrradiationProject = ProjectOrOrder;
            Interface.IPreferences.SavePreferences();

            string filter = Interface.IDB.SubSamples.IrradiationRequestsIDColumn.ColumnName + " = '" + IrrReqID + "'";
            string sort = Interface.IDB.SubSamples.SubSampleNameColumn + " asc";

            Interface.IBS.SubSamples.Filter = filter;
            Interface.IBS.SubSamples.Sort = sort;

            filter = Interface.IDB.Unit.IrrReqIDColumn.ColumnName + " = '" + IrrReqID + "'";
            sort = Interface.IDB.Unit.NameColumn.ColumnName + " asc";

            Interface.IBS.Units.Filter = filter;
            Interface.IBS.Units.Sort = sort;

            Interface.IBS.Units.ResumeBinding();
            Interface.IBS.SubSamples.ResumeBinding();


        }

        /// <summary>
        /// this could be outside
        /// </summary>
        /// <param name="projectOrOrder"></param>
        public void MakeAProjectOrOrder(string projectOrOrder)
        {
            DialogResult result = DialogResult.No;

            string text = "This Irradiation Project does not exist.\nCreate new one?";
            Interface.IReport.Msg(text, "Not Found... " + projectOrOrder);
            result = MessageBox.Show(text, "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (DialogResult.No == result) return;

            Application.DoEvents();
            Interface.IPopulate.IIrradiations.AddIrradiation(projectOrOrder);
            Interface.IStore.Save<LINAA.IrradiationRequestsDataTable>();
            //   LIMS.Interface.IReport.Msg("And there was light...", "Creating... " + ProjectOrOrder);
            Interface.IReport.Msg("Project created", "Please add some samples... " + projectOrOrder);
        }


        public IDetSol IDetSol;
        public IGeometry IGeometry;
        public IIrradiations IIrradiations;

        // public IExpressions IExpressions;
        public INuclear INuclear;

        public IOrders IOrders;
        public IProjects IProjects;
        public ISamples ISamples;
        public ISchedAcqs ISchedAcqs;
        public IToDoes IToDoes;
       

        public Populate(ref Interface inter)
        {
            LINAA aux = inter.Get();
            Interface = inter;

            // IExpressions = (IExpressions)aux;
            INuclear = (INuclear)aux;
            IProjects = (IProjects)aux;
            IIrradiations = (IIrradiations)aux;
            IGeometry = (IGeometry)aux;
            IDetSol = (IDetSol)aux;
            IOrders = (IOrders)aux;
            ISamples = (ISamples)aux;
            ISchedAcqs = (ISchedAcqs)aux;
            IToDoes = (IToDoes)aux;

    
        }


    }

  
}