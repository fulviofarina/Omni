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
    public partial class ucSubSamples : UserControl
    {
        private string filter;

        private string sort;

        private CReport Icrepo = null;
        private Interface Interface;

        private ucSSF ucssf;

        private object daddy;

        public object Daddy
        {
            get { return daddy; }
            set { daddy = value; }
        }

        private ucSSContent uccontent;

        public ucSSContent ucContent
        {
            get
            {
                return uccontent;
            }
            set
            {
                this.splitContainer1.Panel2.Controls.Clear();
                uccontent = value;
                this.splitContainer1.Panel2.Controls.Add(uccontent);
            }
        }

        public ucSSF ucSSF
        {
            get
            {
                return ucssf;
            }
            set
            {
                this.splitContainer1.Panel2.Controls.Clear();
                ucssf = value;
                this.splitContainer1.Panel2.Controls.Add(ucssf);
            }
        }

        public ucSubSamples()
        {
            InitializeComponent();

            //these two functions work flawlessly for selecting the rowHeader in the
            //dgv and for updating of child row positions
            this.BS.CurrentChanged += BS_CurrentChanged;
        }

        /// <summary>
        /// Changes the view with another form
        /// </summary>
        public void ChangeView()
        {
            if (this.ParentForm == null) return;

            if (this.ParentForm.Visible)
            {
                if (!projectbox.Offline && this.SaveItem.Enabled)
                {
                    this.SaveItem.PerformClick();
                }
            }
            else
            {
                this.Link(this.filter, this.sort);
                this.SaveItem.Enabled = false;
            }
            this.ParentForm.Visible = !this.ParentForm.Visible;
        }

        /// <summary>
        /// delinks to the binding source
        /// </summary>
        public void DeLink()
        {
            this.filter = this.BS.Filter;
            this.sort = this.BS.Sort;
            Dumb.DeLinkBS(ref this.BS);
            ucContent.DeLink();
        }

        /// <summary>
        /// Hides the content of the PANEL 2 containers
        /// </summary>
        public bool HideSamplesContent
        {
            set
            {
                this.splitContainer1.Panel2Collapsed = value;
                this.splitContainer2.Panel2Collapsed = value;
            }
        }

        /// <summary>
        /// links to the binding source
        /// </summary>
        public void Link(string Filter, string Sort)
        {
            if (!string.IsNullOrEmpty(Filter)) this.filter = Filter;
            if (!string.IsNullOrEmpty(Sort)) this.sort = Sort;
            Dumb.LinkBS(ref this.BS, this.Linaa.SubSamples, this.filter, this.sort);
            if (this.ParentForm != null) this.ParentForm.Text = projectbox.Project + " - Samples";
            ucContent.Link(Filter, Sort);
        }

        public void Predict(object sender, EventArgs e)
        {
            List<LINAA.SubSamplesRow> samplesToPredict = new List<LINAA.SubSamplesRow>();

            HashSet<int> indexes = new HashSet<int>();

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (!indexes.Add(cell.RowIndex)) continue;

                var r = dataGridView1.Rows[cell.RowIndex];
                var vr = r.DataBoundItem as DataRowView;
                LINAA.SubSamplesRow s = vr.Row as LINAA.SubSamplesRow;

                samplesToPredict.Add(s);
            }

         //   samplesToPredict = samplesToPredict.ToArray();

            //   LINAA newLina = this.Linaa.Clone() as DB.LINAA;
            //newLina.CloneDataSet(ref Linaa);

            ToolStripProgressBar bar = new ToolStripProgressBar();
            ToolStripMenuItem can = new ToolStripMenuItem();

            IWC w = new WC("Predict", ref bar, ref can, ref Linaa);

            DataTable dtk0 = WC.Populatek0NAA(true);
            DataTable dtNAA = WC.PopulateNAA(true);

            Dumb.MergeTable(ref dtk0, ref Linaa);
            Dumb.MergeTable(ref dtNAA, ref Linaa);

            w.SelectedSamples = samplesToPredict;//.ToList();

            w.Predict();
            ucPredict uc = new ucPredict(ref Linaa);
        }

        public void PredictOLDVERSION(object sender, EventArgs e)
        {
            List<LINAA.SubSamplesRow> samplesToPredict = new List<LINAA.SubSamplesRow>();

            HashSet<int> indexes = new HashSet<int>();

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (!indexes.Add(cell.RowIndex)) continue;

                DataGridViewRow r = dataGridView1.Rows[cell.RowIndex];

                DataRowView vr = r.DataBoundItem as DataRowView;

                LINAA.SubSamplesRow s = vr.Row as LINAA.SubSamplesRow;

                samplesToPredict.Add(s);
            }

            IEnumerable<LINAA.SubSamplesRow> send = samplesToPredict.ToArray();

            LINAA newLina = this.Linaa.Clone() as DB.LINAA;
            newLina.CloneDataSet(ref Linaa);

            ToolStripProgressBar bar = new ToolStripProgressBar();
            ToolStripMenuItem can = new ToolStripMenuItem();

            IWC w = new WC("Predict", ref bar, ref can, ref newLina);

            DataTable dtk0 = WC.Populatek0NAA(true);
            DataTable dtNAA = WC.PopulateNAA(true);

            Dumb.MergeTable(ref dtk0, ref newLina);
            Dumb.MergeTable(ref dtNAA, ref newLina);

            w.SelectedSamples = send.ToList();

            w.Predict();
            ucPredict uc = new ucPredict(ref newLina);
        }

        /// <summary>
        /// to do when a sample is added
        /// </summary>
        /// <param name="row"></param>
        public void RowAdded(ref DataRow row)
        {
            this.BS.SuspendBinding();

            IEnumerable<LINAA.SubSamplesRow> samples = Dumb.Cast<LINAA.SubSamplesRow>(this.BS.List as DataView);
            LINAA.IrradiationRequestsRow irr = projectbox.Irradiation;

            try
            {
                IList<LINAA.SubSamplesRow> list = samples.ToList();
                list.Add((LINAA.SubSamplesRow)row);
                samples = list;

                Interface.IPopulate.ISamples
                    .SetLabels(ref samples, projectbox.Project.ToUpper());
                Interface.IPopulate.ISamples
                    .SetIrradiatioRequest(ref samples, ref irr);

                Interface.IPopulate.ISamples
                    .SetUnits(ref samples);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            this.BS.ResumeBinding();

            this.BS.EndEdit();

            Link(this.filter, this.sort);

            string subSIDCol = this.Linaa.SubSamples.SubSamplesIDColumn.ColumnName;
            int ind = this.BS.Find(subSIDCol, samples.Last().SubSamplesID);
            this.BS.Position = ind;
        }

        /// <summary>
        /// set interface and basics
        /// </summary>
        /// <param name="inter"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            Dumb.FD(ref Linaa);
            this.Linaa = Interface.Get();

            projectbox.Set(ref inter, Link);

            this.BS.Sort = this.Linaa.SubSamples.SubSampleNameColumn.ColumnName + " asc";
        }

        /// <summary>
        /// several sample share the irradiation times
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShareTirr(object sender, EventArgs e)
        {
            LINAA.SubSamplesRow current = Dumb.Cast<LINAA.SubSamplesRow>(BS.Current as DataRowView);

            if (current.IsInReactorNull() || current.IsIrradiationTotalTimeNull())
            {
                MessageBox.Show("Please fill in the irradiations date/times for this sample in order to propagate it to the others");
                return;
            }
            IEnumerable<LINAA.SubSamplesRow> others = Dumb.Cast<LINAA.SubSamplesRow>(BS.List as DataView);
            others = others.Where(o => o != current).ToList();
            foreach (LINAA.SubSamplesRow s in others)
            {
                if (s.Equals(current)) continue;
                s.InReactor = current.InReactor;
                s.IrradiationTotalTime = current.IrradiationTotalTime;
            }
        }

        /// <summary>
        /// When the BS changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_CurrentChanged(object sender, EventArgs e)
        {
            DataRow r = (BS.Current as DataRowView).Row;
            if (r == null) return;
            //if not usercontrol attached...
            if (ucssf == null) return;

            LINAA.SubSamplesRow s = r as LINAA.SubSamplesRow;
            //get unit
            LINAA.UnitRow u = s.GetUnitRows().AsEnumerable().FirstOrDefault();
            if (u != null) ucssf.Set(ref u);
        }

        /*
        private void itemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            // ucSSF
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            DataRow r = (dgv.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;

            updateUnitPosition(ref r);
        }
        */

        /// <summary>
        /// something about a crystal report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportBtton_Click(object sender, EventArgs e)
        {
            if (Icrepo == null) Icrepo = new CReport(this.Linaa as DataSet);
            Icrepo.LoadACrystalReport(this.projectbox.Project, CReport.ReporTypes.ProjectReport);
        }
    }
}