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
    public partial class ucSubSamples : UserControl
    {
        //  public ucProjectBox projectbox;
        private object daddy;

        private CReport Icrepo = null;
        private Interface Interface;

        private string filter;
        private string sort;

        public object Daddy
        {
            get { return daddy; }
            set { daddy = value; }
        }

        public ucSubSamples()
        {
            InitializeComponent();
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

                DataGridViewRow r = dataGridView1.Rows[cell.RowIndex];

                DataRowView vr = r.DataBoundItem as DataRowView;

                LINAA.SubSamplesRow s = vr.Row as LINAA.SubSamplesRow;

                samplesToPredict.Add(s);
            }

            IEnumerable<LINAA.SubSamplesRow> send = samplesToPredict.ToArray();

            ucPredict uc = new ucPredict(ref this.Linaa, ref send);
        }

        /// <summary>
        /// to do when a sample is added
        /// </summary>
        /// <param name="row"></param>
        public void RowAdded(ref DataRow row)
        {
            try
            {
                IEnumerable<LINAA.SubSamplesRow> samples = Dumb.Cast<LINAA.SubSamplesRow>(this.BS.List as DataView);

                IList<LINAA.SubSamplesRow> list = samples.ToList();
                list.Add((LINAA.SubSamplesRow)row);
                samples = list;

                Interface.IPopulate.ISamples
                    .SetLabels(ref samples, projectbox.Project.ToUpper());

                LINAA.IrradiationRequestsRow irr = projectbox.Irradiation;
                Interface.IPopulate.ISamples
                    .SetIrradiatioRequest(ref samples, ref irr);

                Interface.IPopulate.ISamples
                    .SetUnits(ref samples);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
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

        private UserControl ucssf;

        public UserControl ucSSF
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

        public void HideContent()
        {
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer2.Panel2Collapsed = true;
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

        private void reportBtton_Click(object sender, EventArgs e)
        {
            if (Icrepo == null) Icrepo = new CReport(this.Linaa as DataSet);
            Icrepo.LoadACrystalReport(this.projectbox.Project, CReport.ReporTypes.ProjectReport);
        }
    }
}