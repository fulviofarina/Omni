using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Reports;
using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucProjectBox : UserControl
    {
        public ucProjectBox()
        {
            InitializeComponent();
        }


        Action hideChildControl;



        private Interface Interface;
        private bool offline = false;

        public bool Offline
        {
            get { return offline; }
            set { offline = value; }
        }

        public bool CanSelectProject
        {
            set { projectbox.Enabled = value; }
        }

        public string Project
        {
            get { return projectbox.Text; }
            set { projectbox.Text = value; }
        }

        public Action HideChildControl
        {
       

            set
            {
                hideChildControl = value;
            }
        }

        public void Refresher()
        {
            
             
                    this.KeyUpPressed(this.projectbox, new KeyEventArgs(Keys.Enter));
         
            
        }

        //   private
        public Int32 IrrReqID = 0;

        /// <summary>
        /// Refreshed the selected project
        /// </summary>
     

        private Action callBack;

        /// </summary>
        /// <param name="inter"></param>
        public void Set(ref Interface inter, Action CallBack)
        {
            Interface = inter;

            callBack = CallBack;

            projectbox.Items.AddRange(Interface.IPopulate.IProjects.ProjectsList.ToArray());

            this.projectbox.KeyUp += KeyUpPressed;


        }

     //   EventHandler refresher;



        private  void KeyUpPressed(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue < 47 || e.KeyValue > 105) && e.KeyCode != Keys.Enter) return;

            string ProjectOrOrder = projectbox.Text;

            if (ProjectOrOrder.CompareTo(string.Empty) == 0) return;

            ProjectOrOrder = ProjectOrOrder.ToUpper().Trim();

            bool  isAProjectOrOrder = Interface.IPopulate.IProjects.ProjectsList.Contains(ProjectOrOrder);

            if (!isAProjectOrOrder && e.KeyCode == Keys.Enter)
            {
                Interface.IPopulate.MakeAProjectOrOrder(ProjectOrOrder);

            }

            Interface.IPopulate.LoadProject(ProjectOrOrder,ref  IrrReqID);


            callBack?.Invoke();
        }


        /// <summary>
        /// This could be outside since it does not depend on anything
        /// </summary>
        /// <param name="ProjectOrOrder"></param>
    

        private void projectlabel_Click(object sender, EventArgs e)
        {
            hideChildControl?.Invoke();
        }
    }
}