﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Reports;
using DB.Tools;
using Rsx;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSubSamples : UserControl
    {
        private object daddy;
        private string filter;

        private CReport Icrepo = null;
        private Interface Interface;
        private string sort;
        // private LINAA.SubSamplesRow currentSample = null;

        private ucSSContent uccontent;

        // private ucSSF ucssf;
        public object Daddy
        {
            get { return daddy; }
            set { daddy = value; }
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
                this.Link();
                this.SaveItem.Enabled = false;
            }
            this.ParentForm.Visible = !this.ParentForm.Visible;
        }

        /// <summary>
        /// delinks to the binding source
        /// </summary>
        public void DeLink()
        {
            this.filter = Interface.IBS.SubSamples.Filter;
            this.sort = Interface.IBS.SubSamples.Sort;
            Dumb.DeLinkBS(ref Interface.IBS.SubSamples);
            // ucContent.DeLink();
        }

        /// <summary>
        /// links to the binding source
        /// </summary>
        public void Link()
        {
            if (this.ParentForm != null) this.ParentForm.Text = projectbox.Project + " - Samples";
        }

        public void Predict(object sender, EventArgs e)
        {
            List<SubSamplesRow> samplesToPredict = new List<SubSamplesRow>();

            HashSet<int> indexes = new HashSet<int>();

            foreach (DataGridViewRow r in DGV.Rows)
            {
                // if (!indexes.Add(r.RowIndex)) continue;

                var vr = r.DataBoundItem as DataRowView;
                SubSamplesRow s = vr.Row as SubSamplesRow;

                samplesToPredict.Add(s);
            }

            // samplesToPredict = samplesToPredict.ToArray();

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
            // this.Linaa.Save<LINAA.IRequestsAveragesRow>();
            ///QUITAR ESTA LINEAAAAAAAAAAAAAAAAAAAAAAAAAAA
            // uc.Visible = false;
        }

        public void PredictOLDVERSION(object sender, EventArgs e)
        {
            List<SubSamplesRow> samplesToPredict = new List<SubSamplesRow>();

            HashSet<int> indexes = new HashSet<int>();

            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                if (!indexes.Add(cell.RowIndex)) continue;

                DataGridViewRow r = DGV.Rows[cell.RowIndex];

                DataRowView vr = r.DataBoundItem as DataRowView;

                SubSamplesRow s = vr.Row as SubSamplesRow;

                samplesToPredict.Add(s);
            }

            IEnumerable<SubSamplesRow> send = samplesToPredict.ToArray();

            LINAA newLina = Interface.Get().Clone() as DB.LINAA;
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


            int IrrReqID = projectbox.IrrReqID;
            string project = projectbox.Project;

            IEnumerable<SubSamplesRow> samples = Interface.ICurrent.SubSamples.OfType<SubSamplesRow>();
            IList<SubSamplesRow> list = samples.ToList();
            list.Add((SubSamplesRow)row);
            samples = list;

            Interface.IPopulate.ISamples.AddSamples(project, ref samples, false);

          //  Link(this.filter, this.sort);

          
            Interface.IBS.Update(row, true, true);
        }

        /// <summary>
        /// set interface and basics
        /// </summary>
        /// <param name="inter"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            Dumb.FD(ref Linaa);
            Dumb.FD(ref this.BS);

            this.DGV.DataSource = Interface.IBS.SubSamples;
            this.BN.BindingSource = Interface.IBS.SubSamples;

          //  Interface.IBS.SubSamples.CurrentChanged += BS_CurrentChanged;
            projectbox.Set(ref Interface, Link);

       

          //  Interface.IBS.SubSamples = this.BS;
        }

        /// <summary>
        /// several sample share the irradiation times
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        public void ShareTirr(object sender, EventArgs e)
        {
            SubSamplesRow current = Interface.ICurrent.SubSample as SubSamplesRow;

            if (current.IsInReactorNull() || current.IsIrradiationTotalTimeNull())
            {
                MessageBox.Show("Please fill in the irradiations date/times for this sample in order to propagate it to the others");
                return;
            }
            IEnumerable<SubSamplesRow> others = Dumb.Cast<SubSamplesRow>(Interface.IBS.SubSamples.List as DataView);
            others = others.Where(o => o != current).ToList();
            foreach (SubSamplesRow s in others)
            {
                if (s.Equals(current)) continue;
                s.InReactor = current.InReactor;
                s.IrradiationTotalTime = current.IrradiationTotalTime;
            }
        }

       
     

        /// <summary>
        /// something about a crystal report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void reportBtton_Click(object sender, EventArgs e)
        {
            if (Icrepo == null) Icrepo = new CReport(Interface.Get() as DataSet);
            Icrepo.LoadACrystalReport(this.projectbox.Project, CReport.ReporTypes.ProjectReport);
        }

        public ucSubSamples()
        {
            InitializeComponent();

            //these two functions work flawlessly for selecting the rowHeader in the
            //dgv and for updating of child row positions
        

            reportBtton.Visible = false;
        }

    
    }
}