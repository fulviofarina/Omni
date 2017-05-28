using System;
using System.ComponentModel;
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

    
        /// <summary>
        /// NOT EMPLOYED BUT COULD BE
        /// </summary>
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
            set {

                projectbox.Text = value;
                this.KeyUpPressed(this.projectbox, new KeyEventArgs(Keys.Enter));
            }
        }


        /// <summary> Refreshed the selected project </summary> </summary> <param name="inter"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;

            projectbox.Items.AddRange(Interface.IPopulate.IProjects.ProjectsList.ToArray());
            //set if the control should be enabled
            Interface.IBS.EnabledControls = this.projectbox.Enabled;
            Interface.IBS.PropertyChanged += delegate
            {
                this.projectbox.Enabled = Interface.IBS.EnabledControls;
            };

            this.projectlabel.Click += delegate
             {
                 hideChildControl?.Invoke();
             };
            this.projectbox.KeyUp += KeyUpPressed;
        }
        /// <summary>
        /// This could be outside since it does not depend on anything
        /// </summary>
        /// <param name="ProjectOrOrder"></param>
        private void KeyUpPressed(object sender, KeyEventArgs e)
        {
            int keyValue = e.KeyValue;
            bool noEnter = e.KeyCode != Keys.Enter;
            noEnter = noEnter && (keyValue < 47 || keyValue > 105);
            if (noEnter) return;
            string ProjectOrOrder = projectbox.Text;

            bool makeAProject = e.KeyCode == Keys.Enter;
            Interface.IPopulate.LoadProject(makeAProject, ProjectOrOrder);
     
            callBack?.Invoke();
        }

     
     

        public ucProjectBox()
        {
            InitializeComponent();
          
         
 
        }
    }
}