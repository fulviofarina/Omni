using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;
using Rsx.DGV;

namespace k0X
{
    public partial class ucPeaks : UserControl
    {
        protected internal BindingSource ToDoBS;
        protected internal IEnumerable<ucSamples> openControls;

        public ucPeaks(ref object set)
        {
            this.InitializeComponent();
            this.SuspendLayout();

            //  DeLink();
            this.Linaa.Dispose();

            this.Linaa = null;
            this.Linaa = set as LINAA;

            SetupSR();

            this.CurrentFilter = string.Empty;

            Link();

            openControls = Program.UserControls.OfType<ucSamples>();

            this.ResumeLayout(false);

            AuxiliarForm form = new AuxiliarForm();
            UserControl control = this;
            form.Populate(ref control);
            form.MaximizeBox = true;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void SetupSR()
        {
            this.CurrentDGV = this.SRDGV;

            this.Gt.Tag = this.Gtbox;
            this.Gtbox.Tag = this.Gt;
            this.Ge.Tag = this.Gebox;
            this.Gebox.Tag = this.Ge;

            this.alpha.Tag = this.alphabox;
            this.alphabox.Tag = this.alpha;

            this.mass.Tag = this.massbox;
            this.massbox.Tag = this.mass;

            this.Fc.Tag = this.Fcbox;
            this.Fcbox.Tag = this.Fc;

            this.ppmbox.Tag = this.ppm;
            this.ppm.Tag = this.ppmbox;

            this.geo.Tag = this.Geobox;
            this.Geobox.Tag = this.geo;
            this.f.Tag = this.fbox;
            this.fbox.Tag = f.Tag;

            HashSet<string> array = new HashSet<string>();
            //ad columns of table to selector
            foreach (DataColumn col in this.Linaa.Peaks.Columns)
            {
                if (col.ColumnName[0] != '_' && col.ColumnName[0] != 'w') array.Add(col.ColumnName);
            }
            foreach (ToolStripItem item in xTableTS.Items)
            {
                if (item.GetType().Equals(typeof(ToolStripComboBox)))
                {
                    ToolStripComboBox combo = item as ToolStripComboBox;
                    combo.AutoCompleteCustomSource.AddRange(array.ToArray());
                    combo.Items.AddRange(array.ToArray());
                }
            }
        }

        protected void AnyLabel_Click(object sender, EventArgs e)
        {
            if (this.CurrentDGV.Tag == null) return;

            ToolStripLabel label = (ToolStripLabel)sender;
            if (label.Tag == null) return;

            IEnumerable<ToolStripTextBox> boxes = this.TS2.Items.OfType<ToolStripTextBox>();
            //erase errors indicators in the boxes
            foreach (ToolStripTextBox o in boxes) this.Error.SetError(o.TextBox, string.Empty); //ERROR IN PARENT label

            if (Sample == null)
            {
                MessageBox.Show("Please select just 1 sample.\nThe tweaking of parameters is dangerous when several samples are selected.\nIn that case I would go to the Project UI, use the Overrider and later Recalculate the samples\n"); //ERROR IN PARENT label
                return;    //always to check
            }

            bool originalReffi = ReEffi.Checked;
            bool originalReDecay = this.ReDecay.Checked;
            this.ReDecay.Checked = true;

            ToolStripTextBox tag = (ToolStripTextBox)label.Tag;
            try
            {
                double toSet = Convert.ToDouble(tag.Text);
                if (tag.Equals(this.ppmbox) || tag.Equals(this.Fcbox))
                {
                    Sample.Concentration = toSet;
                    return;
                }

                if (tag.Equals(this.Gebox))
                {
                    DataRowView v = this.AvgIsotopesBS.Current as DataRowView;
                    LINAA.IRequestsAveragesRow refe = v.Row as LINAA.IRequestsAveragesRow;
                    refe.SetGepithermal(toSet);
                }
                else if (tag.Equals(this.massbox)) Sample.Gross1 = toSet + Sample.Tare;
                else if (tag.Equals(this.alphabox)) Sample.Alpha = toSet;
                else if (tag.Equals(this.fbox)) Sample.f = toSet;
                else if (tag.Equals(this.Gtbox)) Sample.Gthermal = toSet;
                else if (tag.Equals(this.Geobox))
                {
                    Sample.GeometryName = tag.Text;
                    ReEffi.Checked = true;
                    this.ReDecay.Checked = false;
                }
                //recalculate stuff now...
            }
            catch (SystemException ex)
            {
                this.Error.SetError(tag.TextBox, ex.Message);
            }

            this.Refresh_Click(this.Calculate, e);

            ReEffi.Checked = originalReffi;
            ReDecay.Checked = originalReDecay;
        }

        private void box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            ToolStripTextBox box = (ToolStripTextBox)sender;
            AnyLabel_Click(box.Tag, e);
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            if (this.CCancel.Equals(sender) || this.CApply.Equals(sender)) this.CurrentDGV = this.SRDGV;

            this.CurrentDGV = this.SRDGV;

            bool calculate = sender.Equals(this.Calculate) || refreshASDT.Checked;
            if (calculate) ReCalculate();

            //in this order, dont change, xTable must reflect changes due to recalculation!

            this.RefreshSelectReject(this.CurrentDGV);
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            DeLink();
            this.Linaa.Peaks.RejectChanges();
            this.Linaa.IPeakAverages.RejectChanges();
            this.Linaa.IRequestsAverages.RejectChanges();
            Link();

            this.Refresh_Click(sender, e);
        }

        private void UpdateAll(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {
            this.Validate();

            DeLink();

            IEnumerable<DataRow> rows = this.Linaa.Peaks;
            this.Linaa.Save(ref rows);
            rows = this.Linaa.IPeakAverages;
            this.Linaa.Save(ref rows);
            rows = this.Linaa.IRequestsAverages;
            this.Linaa.Save(ref rows);
            rows = null;

            Link();

            xTable.CleanCells(ref this.SRDGV, 2);
        }

        private void Peaks_Click(object sender, EventArgs e)
        {
            KeyEventArgs args = null;

            if (sender.Equals(this.Peaks))
            {
                args = new KeyEventArgs(Keys.P);
            }
            else if (sender.Equals(this.Delete))
            {
                args = new KeyEventArgs(Keys.Delete);
            }
            else return;

            this.AnyDGV_KeyDown(this.CurrentDGV, args);
        }

        private void CoinDaughter_Click(object sender, EventArgs e)
        {
            DataRowView daugtherv = this.AvgPeakBS.Current as DataRowView;
            IEnumerable<DataRowView> view = (this.AvgPeakBS.List as DataView).Cast<DataRowView>();

            IEnumerable<LINAA.IPeakAveragesRow> references = Dumb.Cast<LINAA.IPeakAveragesRow>(view);
            LINAA.IPeakAveragesRow daughter = daugtherv.Row as LINAA.IPeakAveragesRow;

            DataTable table = this.Linaa.CalculateBranchFactor(ref daughter, ref references);

            VTools.Logger log = new VTools.Logger(table, "Coincident Daughter");
            log.Show();
        }

        private void MainTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                if (e.KeyCode == Keys.I)
                {
                    this.MainTab.SelectTab(this.InterferencesTab);
                    this.InterferencesSubTab.SelectTab(this.ISR);
                }
                else if (e.KeyCode == Keys.N)
                {
                    this.MainTab.SelectTab(this.InterferencesTab);
                    this.InterferencesSubTab.SelectTab(this.NISR);
                }
                else if (e.KeyCode == Keys.C)
                {
                    this.MainTab.SelectTab(this.InterferencesTab);
                    this.InterferencesSubTab.SelectTab(this.ICorrection);
                }
            }
            else if (e.Shift)
            {
                DGVLayouts(true);

                if (MSplitC.Panel1Collapsed) MSplitC.Panel1Collapsed = false;
                else MSplitC.Panel1Collapsed = true;

                DGVLayouts(false);
            }
        }

        protected internal void DGVLayouts(bool suspend)
        {
            if (suspend)
            {
                this.SRDGV.SuspendLayout();
                this.AvgPeakDGV.SuspendLayout();
                this.AvgIsotopeDGV.SuspendLayout();
                this.AvgElementDGV.SuspendLayout();
            }
            else
            {
                this.AvgPeakDGV.ResumeLayout();
                this.AvgIsotopeDGV.ResumeLayout();
                this.AvgElementDGV.ResumeLayout();
                this.SRDGV.ResumeLayout();
            }
        }

        private void hideChilean_CheckedChanged(object sender, EventArgs e)
        {
            chThDataGridViewTextBoxColumn1.Visible = !hideChilean.Checked;
            chEpiDataGridViewTextBoxColumn1.Visible = !hideChilean.Checked;
            sDensityDataGridViewTextBoxColumn1.Visible = !hideChilean.Checked;
        }

        private void CApply_Click(object sender, EventArgs e)
        {
            /*
            if (this.correction_in.Rows.Count != 0)
            {
                    foreach (DB.LINAA.PeaksRow row in this.Linaa.Peaks)
                    {
                            if ((row.RowState != DataRowState.Deleted) && (row.Energy == this.ienergy))
                            {
                                    row.CalculateFCorPPM(Properties.Resources.Def, false);

                                    foreach (DB.LINAA.PeaksRow row2 in this.correction_in)
                                    {
                                            if (row2.Measurement.ToUpper().CompareTo(row.Measurement.ToUpper()) != 0)
                                            {
                                                    continue;
                                            }
                                            if (row2.ID >0)
                                            {
                                                    if (this.k0.Project.Comparator)
                                                    {
                                                            row.Fc -= row2.Fc;
                                                            row.mois = row2.Fc;
                                                    }
                                                    else
                                                    {
                                                            row.ppm -= row2.ppm;
                                                            row.mois = row2.ppm;
                                                    }
                                                    continue;
                                            }
                                            row.mois = 0.0;
                                    }
                                    continue;
                            }
                    }
        Refresh_Click(sender, e);
                    this.MainTab.SelectTab(this.SelectRejectTab);
            }
               */
        }

        private void CCancel_Click(object sender, EventArgs e)
        {
            /*
            foreach (DB.LINAA.ProjectRow row in this.k0.Project)
            {
                    if ((row.RowState != DataRowState.Deleted) && (row.Energy == this.ienergy))
                    {
                            row.CalculateFCorPPM(Properties.Resources.Def, false);
                            row.mois = 0.0;
                    }
            }
    Refresh_Click(sender, e);
            this.MainTab.SelectTab(this.SelectRejectTab);
               */
        }

        private void ArenaTab_Enter(object sender, EventArgs e)
        {
            try
            {
                GetToDoPanel();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        protected internal ucToDoPanel GetToDoPanel()
        {
            if (ArenaSC.Panel2.Controls.Count != 0)
            {
                return (ucToDoPanel)ArenaSC.Panel2.Controls.OfType<ucToDoPanel>().First();
            }

            ucToDoPanel panel = null;

            MainForm main = Application.OpenForms.OfType<MainForm>().First();
            panel = Program.UserControls.OfType<ucToDoPanel>().FirstOrDefault();
            if (panel == null)
            {
                main.ToDoPanel_Click(null, EventArgs.Empty);
                panel = Program.UserControls.OfType<ucToDoPanel>().FirstOrDefault();
                ArenaSC.Panel2.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                this.ArenaInternalSC.Panel2.Controls.Add(panel.ucToDoData);
                panel.ucToDoData.Dock = DockStyle.Fill;
                this.ToDoBS = panel.ucToDoData.ListBS;
            }
            return panel;
        }

        private void Vs_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ToDoBS.Current == null) return;

                LINAA.ToDoRow t = Rsx.Dumb.Cast<LINAA.ToDoRow>(this.ToDoBS.Current as DataRowView);
                this.Linaa.ToDoRes.Clear();
                this.Linaa.ToDoResAvg.Clear();
                IEnumerable<LINAA.MeasurementsRow> m1 = t.IRAvgRow.SubSamplesRow.GetMeasurementsRows();
                IEnumerable<LINAA.MeasurementsRow> m2 = t.IRAvgRow2.SubSamplesRow.GetMeasurementsRows();
                //	this.Linaa.ToDoRes.AddToDoResRow(ref m1, ref m2,t.ToDoNr, true,true);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void FindVs_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dumb.IsNuDelDetch(this.Sample)) return;
                ucToDoPanel uctodo = GetToDoPanel();
                if (uctodo == null) return;

                uctodo.ucToDoData.FindVs(this.Sample.SubSampleName);

                if (this.ToDoBS.Current == null) return;

                uctodo.Load.PerformClick();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void DGV_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (sender.Equals(this.AvgIsotopeDGV) && e.ColumnIndex == this.isoCol.Index)
            {
                PaintIsotopes(sender, e);
            }
            else if (sender.Equals(this.AvgElementDGV) && e.ColumnIndex == this.isoCol2.Index)
            {
                //PaintIsotopes(sender, e);
            }
            else if (sender.Equals(this.AvgPeakDGV) && e.ColumnIndex == this.energyCol.Index)
            {
                PaintPeaks(sender, e);
            }
        }

        private void PaintPeaks(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewRow r = dgv.Rows[e.RowIndex];
            if (r == null) return;

            DataRowView v = (r.DataBoundItem as DataRowView);
            if (v == null) return;
            LINAA.IPeakAveragesRow ip = v.Row as LINAA.IPeakAveragesRow;
            if (ip == null) return;

            DataGridViewCellStyle style = r.DefaultCellStyle;
            if (!ip.IsCodek0Null())
            {
                if (ip.Codek0 == 1) style.BackColor = System.Drawing.Color.Honeydew;
                else
                {
                    if (ip.Codek0 == 2) style.BackColor = System.Drawing.Color.Azure;
                    else style.BackColor = System.Drawing.Color.MistyRose;
                }
            }

            if (e.ColumnIndex == this.energyCol.Index)
            {
                if (Dumb.IsNuDelDetch(ip.k0NAARow)) return;
                if (!ip.k0NAARow.IsCommentsNull())
                {
                    r.Cells[e.ColumnIndex].ToolTipText = ip.k0NAARow.Comments;
                }
            }
            else if (e.ColumnIndex == sdPeakCol.Index)
            {
                if (!ip.IsSDNull())
                {
                    double maxUnc = 1;
                    maxUnc = Convert.ToDouble(this.maxUncbox.Text);
                    DataGridViewCell cell = dgv[this.sdPeakCol.Index, e.RowIndex];
                    if (ip.SD >= maxUnc) cell.Style.ForeColor = System.Drawing.Color.Red;
                    else cell.Style.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void PaintIsotopes(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            DataGridViewRow r = dgv.Rows[e.RowIndex];
            if (r == null) return;

            DataRowView v = (r.DataBoundItem as DataRowView);
            if (v == null) return;
            LINAA.IRequestsAveragesRow ir = v.Row as LINAA.IRequestsAveragesRow;
            if (ir == null) return;
            LINAA.NAARow n = ir.NAARow;
            DataGridViewCellStyle style = r.DefaultCellStyle;
            int colind = e.ColumnIndex;
            if (colind == this.isoCol.Index || colind == this.isoCol2.Index)
            {
                if (n != null && !n.IsCommentsNull())
                {
                    dgv[e.ColumnIndex, e.RowIndex].ToolTipText = n.Comments;
                }
            }

            if (this.sdIsoCol.Index == colind)
            {
                if (!ir.IsSDNull())
                {
                    double maxUnc = 1;
                    maxUnc = Convert.ToDouble(this.maxUncbox.Text);
                    DataGridViewCell cell = dgv[this.sdIsoCol.Index, e.RowIndex];
                    if (ir.SD >= maxUnc)
                    {
                        style.BackColor = System.Drawing.Color.MistyRose;
                        cell.Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        style.BackColor = System.Drawing.Color.Honeydew;
                        cell.Style.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        private void calcRatios_Click(object sender, EventArgs e)
        {
        }

        private void interfbuton_Click(object sender, EventArgs e)
        {
            interfWorker.RunWorkerAsync();
        }

        private void switchbttn_Click(object sender, EventArgs e)
        {
            string ene = this.Linaa.Peaks.EnergyColumn.ColumnName;
            string meas = this.Linaa.Peaks.MeasurementColumn.ColumnName;
            if (Yi.Text.CompareTo(ene) != 0)
            {
                Yi.Text = string.Empty;
                Xj.Text = meas;
                Yi.Text = ene;
            }
            else
            {
                Yi.Text = string.Empty;
                Xj.Text = ene;
                Yi.Text = meas;
            }

            Field_Click(this.Xij, EventArgs.Empty);
        }

        protected void SRDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Lock.Checked = false;
        }

        private void AvgDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = null;

            DataGridView dgv = sender as DataGridView;

            DataGridViewTextBoxColumn i = null;
            DataGridViewTextBoxColumn el = null;
            bool query = false;
            if (dgv.Equals(this.AvgIsotopeDGV))
            {
                query = true;
                i = isoCol;
                el = eleCol;
            }
            else
            {
                i = isoCol2;
                el = eleCol2;
            }

            if (i == null || el == null) return;

            bool iso = e.ColumnIndex == i.Index;
            bool ele = e.ColumnIndex == el.Index;

            if (iso || ele)
            {
                row = dgv.Rows[e.RowIndex];
            }

            if (row == null) return;

            LINAA.IRequestsAveragesRow res = Dumb.Cast<LINAA.IRequestsAveragesRow>(row);

            string isotope = res.NAARow.Iso;
            string text = "Nuclear Decay data for " + res.Radioisotope;
            string uri = DB.Tools.NuDat.GetIsotopeUrl(isotope);

            if (ele)
            {
                text = "Element Information for " + res.Element;
                uri = DB.Tools.NuDat.GetElementUrl(res.NAARow.ElementsRow.ElementNameEn);
            }
            else
            {
                if (query)
                {
                    string html = DB.Tools.NuDat.Query(isotope);
                    LINAA.YieldsDataTable dt = DB.Tools.NuDat.HtmlToTable(html, isotope);

                    LINAA.k0NAARow[] rows = res.NAARow.Getk0NAARows();

                    foreach (LINAA.YieldsRow y in dt)
                    {
                        y.Select = false;
                        y.NAAID = res.NAAID;
                        LINAA.k0NAARow k = rows.FirstOrDefault(o => o.Energy + 0.2 >= y.Energy && o.Energy - 0.2 <= y.Energy);
                        if (k != null)
                        {
                            y.k0NAAID = k.ID;
                            y.Iso = k.Iso;
                        }
                    }

                    VTools.Logger l = new VTools.Logger(dt, isotope);
                    l.ShowDialog();

                    IEnumerable<LINAA.YieldsRow> todel = dt.Rows.OfType<LINAA.YieldsRow>().Where(o => !o.Select).ToList();
                    this.Linaa.Delete(ref todel);

                    this.Linaa.Yields.Merge(dt, false);

                    return;
                }
            }

            aux = Dumb.NavigateTo(text, uri);
        }

        private WebBrowser aux = null;
    }
}