using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using DB.Reports;

namespace DB.UI
{
    public interface ISubSamples
    {
        void DeLink();

        LINAA.IrradiationRequestsRow Irradiation { get; set; }

        void Link();

        bool Offline { get; set; }
        string Project { get; set; }

        void RefreshSubSamples();

        // System.Collections.Generic.IEnumerable<DB.LINAA.SubSamplesRow> Samples { get; set; }
        void Dispose();

        Form ParentForm { get; }
        bool CanSelectProject { set; }

        bool ShouldPaint(object sender, DataGridViewCellPaintingEventArgs e);

        void PaintCells(object sender, DataGridViewCellPaintingEventArgs e);

        object Daddy { get; set; }

        void ChangeView();

        void RowAdded(ref DataRow row);

        void ShareTirr(object sender, EventArgs e);

        void Predict(object sender, EventArgs e);
    }

    public partial class ucSubSamples : UserControl, ISubSamples
    {

        private CReport Icrepo = null;
        public ucSubSamples()
        {
            offline = false;
            InitializeComponent();

            this.Linaa.Dispose();
            this.Linaa = null;
            this.Linaa = LIMS.Linaa;

            this.AAFillHeight.Checked = this.Linaa.CurrentPref.AAFillHeight;
            this.AARadius.Checked = this.Linaa.CurrentPref.AARadius;

            projectbox.Items.AddRange(this.Linaa.ProjectsList.ToArray());
            this.projectbox.TextChanged -= this.projectbox_Click;
            this.projectbox.TextChanged += this.projectbox_Click;


          

        }

        private object daddy;

        public object Daddy
        {
            get { return daddy; }
            set { daddy = value; }
        }

        private Int32 irrReqID = 0;

        public bool CanSelectProject
        {
            set { projectbox.Enabled = value; }
        }

        public string Project
        {
            get { return projectbox.Text; }
            set { projectbox.Text = value; }
        }

        private bool offline;

        public bool Offline
        {
            get { return offline; }
            set { offline = value; }
        }

        private string filter;
        private DateTime minDate = DateTime.Now;
        private string sort;

        private LINAA.IrradiationRequestsRow irRow = null;

        public LINAA.IrradiationRequestsRow Irradiation
        {
            get { return irRow; }
            set { irRow = value; }
        }

        public void Link()
        {
            Dumb.LinkBS(ref this.BS, this.Linaa.SubSamples, this.filter, this.sort);
            if (this.ParentForm != null) this.ParentForm.Text = projectbox.Text + " - Samples";
        }

        public void ChangeView()
        {
            if (this.ParentForm == null) return;

            if (this.ParentForm.Visible)
            {
                if (!Offline && this.SaveItem.Enabled)
                {
                    this.SaveItem.PerformClick();
                }
            }
            else
            {
                this.Link();
                this.SaveItem.Enabled = false;
            }
            this.ParentForm.Visible = !this.ParentForm.Visible;
        }

        public void DeLink()
        {
            this.filter = this.BS.Filter;
            this.sort = this.BS.Sort;
            Dumb.DeLinkBS(ref this.BS);
        }

        public void RefreshSubSamples()
        {
            if (projectbox.Text.CompareTo(string.Empty) == 0) return;

            string project = projectbox.Text.ToUpper().Trim();

            if (!this.Linaa.ProjectsList.Contains(project)) return;

            irRow = this.Linaa.IrradiationRequests.FindByIrradiationCode(project);
            irrReqID = irRow.IrradiationRequestsID;

            if (!this.offline)
            {
                this.Linaa.PopulateSubSamples(irrReqID);
            }

            filter = this.Linaa.SubSamples.IrradiationRequestsIDColumn.ColumnName + " = '" + irrReqID + "'";
            sort = this.Linaa.SubSamples.SubSampleNameColumn + " asc";

            Link();
        }

        private void projectbox_Click(object sender, EventArgs e)
        {
            RefreshSubSamples();

            //SaveItem.PerformClick();
        }

        /// <summary>
        /// Painter method
        /// </summary>
        /// <param name="sampleCellArgs"></param>
        public void PaintCells(object sender, DataGridViewCellPaintingEventArgs sampleCellArgs)
        {
            int ind = sampleCellArgs.ColumnIndex;
            if (ind == 0) SampleCellPaint(ref sampleCellArgs);
            else if (ind == this.InReactorColumn.Index) IrradiationCellPaint(ref sampleCellArgs);
        }

        /// <summary>
        /// Paint method for irradiation cell
        /// </summary>
        /// <param name="sampleCellArgs"></param>
        protected void IrradiationCellPaint(ref DataGridViewCellPaintingEventArgs sampleCellArgs)
        {
            DateTime currentVal;
            bool parsed = DateTime.TryParse(sampleCellArgs.Value.ToString(), out currentVal);
            if (parsed)
            {
                if (currentVal < minDate)
                {
                    //minDate = Samples.Where(s => !s.HasErrors).Where(s => !s.IsInReactorNull()).Min(c => c.InReactor);
                    this.InReactorColumn.Tag = currentVal;
                    this.OutReactorColumn.Tag = currentVal;
                }
            }
        }

        /// <summary>
        /// Paint method for sample cell
        /// </summary>
        /// <param name="sampleCellArgs"></param>
        protected void SampleCellPaint(ref DataGridViewCellPaintingEventArgs sampleCellArgs)
        {
            //   if (sampleCellArgs.CellStyle.BackColor != Color.FromName("Window")) return;
            string aux = sampleCellArgs.Value.ToString();
            if (aux.Contains(DB.Properties.Samples.Std))
            {
                sampleCellArgs.CellStyle.BackColor = Color.Thistle;
            }
            else if (aux.Contains(DB.Properties.Samples.Mon))
            {
                sampleCellArgs.CellStyle.BackColor = Color.Salmon;
            }
            else if (aux.Contains(DB.Properties.Samples.RefMat))
            {
                sampleCellArgs.CellStyle.BackColor = Color.PapayaWhip;
            }
            else if (aux.Contains(DB.Properties.Samples.Smp))
            {
                sampleCellArgs.CellStyle.BackColor = Color.Salmon;
            }
            else sampleCellArgs.CellStyle.BackColor = Color.Honeydew;
        }

        public bool ShouldPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            int ind = e.ColumnIndex;
            if (sender.Equals(this.DGV3))
            {
                if (ind != this.InReactorColumn.Index && ind != 0) return false;
            }
            else if (ind != 0) return false;
            return true;
        }

        public void Predict(object sender, EventArgs e)
        {
            List<LINAA.SubSamplesRow> samplesToPredict = new List<LINAA.SubSamplesRow>();

            HashSet<int> indexes = new HashSet<int>();

            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                if (!indexes.Add(cell.RowIndex)) continue;

                DataGridViewRow r = DGV.Rows[cell.RowIndex];

                DataRowView vr = r.DataBoundItem as DataRowView;

                LINAA.SubSamplesRow s = vr.Row as LINAA.SubSamplesRow;

                samplesToPredict.Add(s);
            }

            IEnumerable<LINAA.SubSamplesRow> send = samplesToPredict.ToArray();

            ucPredict uc = new ucPredict(ref this.Linaa, ref send);
        }

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

        private void reportBtton_Click(object sender, EventArgs e)
        {
            if (Icrepo == null) Icrepo = new CReport(this.Linaa as DataSet);
            Icrepo.LoadACrystalReport(this.projectbox.Text, CReport.ReporTypes.ProjectReport);
        }

        public void RowAdded(ref DataRow row)
        {
            try
            {
                IEnumerable<LINAA.SubSamplesRow> samples = Dumb.Cast<LINAA.SubSamplesRow>(this.BS.List as DataView);
                IList<LINAA.SubSamplesRow> list = samples.ToList();
                list.Add((LINAA.SubSamplesRow)row);
                samples = list;
                this.Linaa.SetLabels(ref samples, projectbox.Text.ToUpper());
                this.Linaa.SetIrradiatioRequest(samples, ref irRow);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void AutoAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(AAFillHeight))
            {
                if (AAFillHeight.Checked == true) AARadius.Checked = false;
            }
            else if (AARadius.Checked == true) AAFillHeight.Checked = false;

            this.Linaa.CurrentPref.AAFillHeight = AAFillHeight.Checked;
            this.Linaa.CurrentPref.AARadius = AARadius.Checked;

            this.Linaa.Save<LINAA.PreferencesDataTable>();
        }
    }
}