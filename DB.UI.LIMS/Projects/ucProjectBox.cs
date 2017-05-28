using System;
using System.Collections.Generic;
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
     
            get {
              
                return projectbox.Text.Trim().ToUpper() ;
            }
            set
            {

                projectbox.Text = value.Trim().ToUpper();
                updateProjectBox();
                this.KeyUpPressed(this.projectbox, new KeyEventArgs(Keys.Enter));
            }
        }

        private void updateProjectBox()
        {
            string[] projects = Interface.IPopulate.IProjects.ProjectsList.ToArray();
            //  projectbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //  projectbox.AutoCompleteCustomSource.AddRange();
            // projectbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Rsx.Dumb.UIControl.FillABox(projectbox, projects, true, false);
        }


        /// <summary> Refreshed the selected project </summary> </summary> <param name="inter"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;

           // BindingList<IList<string>> Ibls = new BindingList<IList<string>>();
          //  Ibls.Add(Interface.IPopulate.IProjects.ProjectsList);
         
         //   this.projectbox.DataBindings.Add("Text", Interface.IPopulate.IProjects.ProjectsList, "ElementAt");
          //  Rsx.Dumb.UIControl.FillABox(projectbox, Ibls, true, false);
        //       this.projectbox.Items.dd(Ibls);
            // this.projectbox.GotFocus += Projectbox_GotFocus;
     //       this.projectbox.AutoCompleteCustomSource.(IList);
         //   this.projectbox.AutoCompleteSource = AutoCompleteSource.ListItems;
         //   this.projectbox.AutoCompleteMode = AutoCompleteMode.Suggest;
          //  this.projectbox.Items = Interface.IPopulate.IProjects.ProjectsList;

            //   projectbox.Items.AddRange(Interface.IPopulate.IProjects.ProjectsList.ToArray());
            //set if the control should be enabled
            Interface.IBS.EnabledControls = this.projectbox.Enabled;
            Interface.IBS.PropertyChanged += delegate
            {
              
                //it does not fires when GET enbled controls
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
            string ProjectOrOrder = Project;

            bool makeAProject = e.KeyCode == Keys.Enter;

            if (ProjectOrOrder.Equals("DEV"))
            {
                Interface.IPreferences.CurrentPref.AdvancedEditor = true;
                this.projectbox.Text = Interface.IPreferences.CurrentPref.LastIrradiationProject;
                Interface.IPreferences.SavePreferences();
                return;
            }
          


        bool projectAdded =    Interface.IPopulate.LoadProject(makeAProject, ProjectOrOrder);
     
            if (projectAdded)
            {
                updateProjectBox();
            }
            callBack?.Invoke();
        }

     
     

        public ucProjectBox()
        {
            InitializeComponent();
          
         
 
        }
    }
}