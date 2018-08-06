using DB.Tools;
using System;
using System.Linq;
using System.Windows.Forms;
using VTools;

namespace DB.UI
{
    public class ucHyperLabPB : ucGenericCBox
    {
        private Interface Interface;

        public ucHyperLabPB() : base()
        {
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            PopulateListMethod += populate;
            RefreshMethod += refreshor;
            //interesting if this workk without binding
            SetNoBindingSource();
            TextContent = Interface.IPreferences.CurrentPref.LastIrradiationProject;
        }

        private void refreshor(object sender, EventArgs e)
        {
            resetProgress?.Invoke(0);

            string projectTXT = TextContent;

            if (InputProjects.Contains(projectTXT))
            {
                resetProgress?.Invoke(2);

                showProgress?.Invoke(null, EventArgs.Empty);

                Cursor.Current = Cursors.WaitCursor;

                Interface.IBS.EnabledControls = false;

                Interface.IPopulate.IMeasurements.PopulateMeasurementsHyperLab(projectTXT, true);

                Interface.IBS.SelectProjectHL(projectTXT);

                Interface.IPreferences.CurrentPref.LastIrradiationProject = projectTXT;

                Interface.IBS.EnabledControls = true;

                showProgress?.Invoke(null, EventArgs.Empty);

                Cursor.Current = Cursors.Default;
            }
        }

        private void populate(object sender, EventArgs e)
        {
            string[] projects = Interface.IPopulate.IProjects.ListOfHLProjects().ToArray();
            InputProjects = projects;
        }

        private EventHandler showProgress = null;
        private Action<int> resetProgress = null;

        public EventHandler ShowProgress
        {
          

            set
            {
                showProgress = value;
            }
        }

        public Action<int> ResetProgress
        {
         

            set
            {
                resetProgress = value;
            }
        }
    }
}