using System.Windows.Forms;

namespace DB.Tools
{
    public partial class Populate
    {

    

        public bool LoadProject( bool makeProject, string ProjectOrOrder)
        {
            if (string.IsNullOrEmpty(ProjectOrOrder)) return false;
            //    ProjectOrOrder = ProjectOrOrder.ToUpper().Trim();
            bool isAProjectOrOrder = Interface.IPopulate.IProjects.ProjectsList.Contains(ProjectOrOrder);
            if (!Interface.IBS.EnabledControls) return false;
            //eactivate box because it entered
            makeProject = makeProject && !isAProjectOrOrder;

            if (isAProjectOrOrder || makeProject)
            {
                Interface.IBS.EnabledControls = false;
            }
            if (makeProject)
            {
               
                Interface.IPopulate.addProject(ref ProjectOrOrder);
                isAProjectOrOrder = true;
            }
            if (isAProjectOrOrder)
            {
             //   Interface.IBS.EnabledControls = true;
                Interface.IBS.SelectProject(ProjectOrOrder);
                
            }
            Interface.IBS.EnabledControls = true;

            Interface.IBS.SubSamples.MoveLast();//.Position = 0;
            Interface.IBS.Units.MoveLast();//.Position = 0;

            return makeProject;

        }
        private void addProject(ref string ProjectOrOrder)
        {
            DialogResult result = DialogResult.No;
            string text = "This Project does not exist yet (not found).\nWould you like to create a new one?";
            Interface.IReport.Msg(text, "Not Found... " + ProjectOrOrder);
            result = MessageBox.Show(text, "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        //    Application.DoEvents();
            if (DialogResult.No == result)
            {
                ProjectOrOrder = "NULL";
            }
            else
            {
             
                Interface.IPopulate.IIrradiations.AddNewIrradiation(ProjectOrOrder);
                Interface.IStore.Save<LINAA.IrradiationRequestsDataTable>();
                // LIMS.Interface.IReport.Msg("And there was light...", "Creating... " + ProjectOrOrder);
                text = "Project created.\n\nPlease add some samples to it... ";
                string created = "Project " + ProjectOrOrder + " created";
                MessageBox.Show(text, created, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Interface.IReport.Msg(created, text );
            }

        }

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