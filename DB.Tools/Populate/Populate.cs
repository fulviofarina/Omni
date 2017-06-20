using System.Windows.Forms;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class Populate
    {

        private void getPreferencesEvent(object sender, EventData e)
        {

            SSFPrefRow pref = Interface.IPreferences.CurrentSSFPref;
            object[] args = new object[]
            {
                pref.CalcMass,
            pref.AARadius,
             pref.CalcDensity,
            pref.AAFillHeight
        };

           
            e.Args = args;
        }

        public bool[] LoadProject( bool enterPressed, string ProjectOrOrder)
        {
            if (string.IsNullOrEmpty(ProjectOrOrder)) return new bool[] { false, false };
            //    ProjectOrOrder = ProjectOrOrder.ToUpper().Trim();
            bool isAProjectOrOrder = Interface.IPopulate.IProjects.ProjectsList.Contains(ProjectOrOrder);
            if (!Interface.IBS.EnabledControls) return new bool[] { false, false };
            //eactivate box because it entered
            bool makeProject = enterPressed && !isAProjectOrOrder;

            if (isAProjectOrOrder || makeProject)
            {
                //disable boxes
                Interface.IBS.EnabledControls = false;
            }
            bool rejected = false;
            if (makeProject)
            {
                //add project and update the flag
                isAProjectOrOrder= Interface.IPopulate.AddProject(ref ProjectOrOrder);
                if (!isAProjectOrOrder) rejected = true;
                if (rejected)
                {
                    Interface.IReport.Msg("The user cancelled the project creation", "Project " + ProjectOrOrder + " not created", false);
                }
          
            }
            if (isAProjectOrOrder)
            {
                //find project
                Interface.IBS.SelectProject(ProjectOrOrder);
                //renable boxes
                Interface.IBS.EnabledControls = true;

                Interface.IBS.SubSamples.MoveLast();//.Position = 0;
                Interface.IBS.Units.MoveLast();//.Position = 0;
            }

            return new bool[] { rejected, isAProjectOrOrder };

        }
        public bool AddProject(ref string ProjectOrOrder)
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
            return result != DialogResult.No;
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


            Interface.IDB.SubSamples.CalcParametersHandler += getPreferencesEvent;
            
        }
    }
}