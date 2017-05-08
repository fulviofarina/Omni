using System;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;

namespace DB.UI
{
    public partial class ucProjectBox : UserControl
    {
        private Action callBack;

        private Action hideChildControl;

        private Interface Interface;

        private bool offline = false;

        public Action CallBack
        {
            get
            {
                return callBack;
            }

            set
            {
                callBack = value;
            }
        }

        public bool CanSelectProject
        {
            set { projectbox.Enabled = value; }
        }

        public Action HideChildControl
        {
            set
            {
                hideChildControl = value;
            }
        }

        public bool Offline
        {
            get { return offline; }
            set { offline = value; }
        }

        public string Project
        {
            get { return projectbox.Text; }
            set { projectbox.Text = value; }
        }

        public void Refresher()
        {
            this.KeyUpPressed(this.projectbox, new KeyEventArgs(Keys.Enter));
        }

        /// <summary>
        /// Refreshed the selected project
        /// </summary>
        /// </summary> <param name="inter"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            projectbox.Items.AddRange(Interface.IPopulate.IProjects.ProjectsList.ToArray());

            this.projectbox.KeyUp += KeyUpPressed;
        }

        /// <summary>
        /// This could be outside since it does not depend on anything
        /// </summary>
        /// <param name="ProjectOrOrder"></param>
        private void KeyUpPressed(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue < 47 || e.KeyValue > 105) && e.KeyCode != Keys.Enter) return;

            string ProjectOrOrder = projectbox.Text;

            if (ProjectOrOrder.CompareTo(string.Empty) == 0) return;

            ProjectOrOrder = ProjectOrOrder.ToUpper().Trim();

            bool isAProjectOrOrder = Interface.IPopulate.IProjects.ProjectsList.Contains(ProjectOrOrder);

            if (this.projectbox.Enabled == false) return;

            bool makeProject = e.KeyCode == Keys.Enter;
            //eactivate box because it entered
            makeProject = makeProject && !isAProjectOrOrder;

            if (isAProjectOrOrder || makeProject) this.projectbox.Enabled = false;

            if (makeProject)
            {
             
                Interface.IPopulate.CreateProject( ProjectOrOrder);
                isAProjectOrOrder = true;
            }

            if (isAProjectOrOrder)
            {
              
                Interface.IPopulate.LoadProject(ProjectOrOrder);
            }
            this.projectbox.Enabled = true;

            callBack?.Invoke();
        }

        private void projectlabel_Click(object sender, EventArgs e)
        {
            hideChildControl?.Invoke();
        }

        public ucProjectBox()
        {
            InitializeComponent();
        }
   
    }
}