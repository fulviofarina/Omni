using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Reports;
using Rsx;

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

        public LINAA.IrradiationRequestsRow Irradiation = null;

        public bool CanSelectProject
        {
            set { projectbox.Enabled = value; }
        }

        public string Project
        {
            get { return projectbox.Text; }
            set { projectbox.Text = value; }
        }

        private Int32 irrReqID = 0;

        /// <summary>
        /// Refreshed the selected project
        /// </summary>
        public void RefreshSubSamples()
        {
            if (projectbox.Text.CompareTo(string.Empty) == 0) return;

            string project = projectbox.Text.ToUpper().Trim();

            if (!Interface.IDB.ProjectsList.Contains(project)) return;

            Irradiation = Interface.IDB.IrradiationRequests.FindByIrradiationCode(project);
            irrReqID = Irradiation.IrradiationRequestsID;

            if (!this.offline)
            {
                Interface.IPopulate.ISamples.PopulateSubSamples(irrReqID);
            }

            string filter = Interface.IDB.IrradiationRequests.IrradiationRequestsIDColumn.ColumnName + " = '" + irrReqID + "'";
            string sort = Interface.IDB.SubSamples.SubSampleNameColumn + " asc";

            callBack(filter, sort);
        }

        private Action<string, string> callBack;

        /// </summary>
        /// <param name="inter"></param>
        public void Set(ref Interface inter, Action<string, string> CallBack)
        {
            Interface = inter;

            this.AAFillHeight.Checked = Interface.IPreferences.CurrentPref.AAFillHeight;
            this.AARadius.Checked = Interface.IPreferences.CurrentPref.AARadius;

            callBack = CallBack;

            projectbox.Items.AddRange(Interface.IDB.ProjectsList.ToArray());

            this.projectbox.TextChanged += delegate
            {
                RefreshSubSamples();// this.projectbox_Click;
            };
        }

        private void AutoAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(AAFillHeight))
            {
                if (AAFillHeight.Checked == true) AARadius.Checked = false;
            }
            else if (AARadius.Checked == true) AAFillHeight.Checked = false;

            Interface.IPreferences.CurrentPref.AAFillHeight = AAFillHeight.Checked;
            Interface.IPreferences.CurrentPref.AARadius = AARadius.Checked;

            Interface.IStore.Save<LINAA.PreferencesDataTable>();
        }
    }
}