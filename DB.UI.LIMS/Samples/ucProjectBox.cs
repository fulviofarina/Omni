using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Reports;
using Rsx;
using DB.Tools;

namespace DB.UI
{
    public partial class ucProjectBox : UserControl
    {
        public ucProjectBox()
        {
            InitializeComponent();
        }

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

        //   private
        public Int32 IrrReqID = 0;

        /// <summary>
        /// Refreshed the selected project
        /// </summary>
        public void RefreshSubSamples()
        {
            if (projectbox.Text.CompareTo(string.Empty) == 0) return;

            string project = projectbox.Text.ToUpper().Trim();

            if (!Interface.IPopulate.IProjects.ProjectsList.Contains(project)) return;
            Interface.IPreferences.CurrentPref.LastIrradiationProject = project;
            LINAA.IrradiationRequestsRow Irradiation = null;
            Irradiation = Interface.IDB.IrradiationRequests.FindByIrradiationCode(project);
            IrrReqID = Irradiation.IrradiationRequestsID;

            //   if (!this.offline)
            //   {
            Interface.IPopulate.ISamples.PopulateSubSamples(IrrReqID);
            Interface.IPopulate.IGeometry.PopulateUnitsByProject(IrrReqID);
            //  }

            string filter = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName + " = '" + IrrReqID + "'";
            string sort = Interface.IDB.SubSamples.SubSampleNameColumn + " asc";

            Interface.IStore.SavePreferences();

            callBack(filter, sort);
        }

        private Action<string, string> callBack;

        /// </summary>
        /// <param name="inter"></param>
        public void Set(ref Interface inter, Action<string, string> CallBack)
        {
            Interface = inter;

            callBack = CallBack;

            projectbox.Items.AddRange(Interface.IPopulate.IProjects.ProjectsList.ToArray());

            this.projectbox.TextChanged += delegate
            {
                RefreshSubSamples();// this.projectbox_Click;
            };
        }
    }
}