using System.Windows.Forms;

namespace DB.Tools
{
    public partial class Populate
    {

        protected Interface Interface;


        /// <summary>
        /// this should go outside
        /// </summary>

        public void CreateProject( string ProjectOrOrder)
        {

           // Interface.IBS.SuspendBindings();
           
            //      this.projectbox.Enabled = false;
            DialogResult result = DialogResult.No;

                string text = "This Irradiation Project does not exist.\nCreate new one?";
                Interface.IReport.Msg(text, "Not Found... " + ProjectOrOrder);
                result = MessageBox.Show(text, "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (DialogResult.No == result) return;

                Application.DoEvents();
                Interface.IPopulate.IIrradiations.AddIrradiation(ProjectOrOrder);
                Interface.IStore.Save<LINAA.IrradiationRequestsDataTable>();
                // LIMS.Interface.IReport.Msg("And there was light...", "Creating... " + ProjectOrOrder);
                Interface.IReport.Msg("Project created", "Please add some samples... " + ProjectOrOrder);

              //   Interface.IBS.ResumeBindings();

          
        }
        public void LoadProject(string ProjectOrOrder)
        {
          //    Interface.IBS.SuspendBindings();
            string field = Interface.IDB.IrradiationRequests.IrradiationCodeColumn.ColumnName;
            int position = Interface.IBS.Irradiations.Find(field, ProjectOrOrder);
            Interface.IBS.Irradiations.Position = position;
        //    Interface.IBS.ResumeBindings();


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

        /// <summary>
        /// Creates the Class
        /// </summary>
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