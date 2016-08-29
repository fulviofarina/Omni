using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;
using Rsx.Math;
using NPlot;

namespace k0X
{
    public partial class ucToDoData : UserControl
    {
        private DB.Tools.IToDo TD = null;
        private LINAA.ToDoType tocalculate;
        public object Daddy = null;
        private bool escape = false;
        private bool chkchanging = false;
        private const char c = '@'; //separator

        public void FindVs(string sample)
        {
            int index = this.ListBS.Find(this.Linaa.ToDo.sampleColumn.ColumnName, sample);
            if (index > -1) this.ListBS.Position = index;
            else
            {
                index = this.ListBS.Find(this.Linaa.ToDo.sample2Column.ColumnName, sample);
                if (index > -1) this.ListBS.Position = index;
            }
        }

        public void CheckIfEmptyToDo()
        {
            if (this.ToDoLabelBox.Text.Equals(string.Empty))
            {
                int i = 0;
                string todo = "NEW TODO - " + i;
                System.Collections.Generic.IEnumerable<LINAA.ToDoRow> rows = this.Linaa.ToDo.Where(o => o.label.CompareTo(todo) == 0);
                while (rows.Count() != 0)
                {
                    i++;
                    todo = "NEW TODO - " + i;
                }

                this.ToDoLabelBox.Text = todo;
            }
        }

        public ucToDoData(ref LINAA set)
        {
            this.InitializeComponent();

            this.Linaa.Dispose();
            this.Linaa = null;
            this.Linaa = set;
            TD = new DB.Tools.ToDo(ref this.Linaa);

            IEnumerable<DataColumn> cols = this.Linaa.ToDoData.Columns.OfType<DataColumn>().Where(c => c.DataType.Equals(typeof(double)));
            IEnumerable<string> aux = cols.Select(o => o.ColumnName);
            this.Rawbox.Items.AddRange(aux.ToArray());
            cols = this.Linaa.ToDoAvg.Columns.OfType<DataColumn>().Where(c => c.DataType.Equals(typeof(double)));
            aux = cols.Select(o => o.ColumnName);
            this.Avgbox.Items.AddRange(aux.ToArray());
            cols = null;
            aux = null;

            this.Rawbox.Text = "Fc";
            this.Avgbox.Text = "Fc";

            this.PreparePlots(); //prepare plots

            Fill.PerformClick();
        }

        public void Fill_Click(object sender, EventArgs e)
        {
            try
            {
                this.Linaa.PopulateToDoes();

                Dumb.FillABox(this.ToDoLabelBox, this.Linaa.ToDoesList, true, false);

                if (this.Linaa.ToDoesList.Count == 0) return;
            }
            catch (SystemException exception)
            {
                this.Error.SetError(this.RawBN, exception.Message);
                this.Linaa.AddException(exception);
            }

            GetLastData();

            Link(true);
        }

        private void GetLastData()
        {
            try
            {
                Func<string, bool> a = string.IsNullOrWhiteSpace;   //delegate GENERIC!!!!!!!!! AMAZING, ALL THIS TIME WASTING TIME

                bool labelempty = a(this.ToDoLabelBox.Text);
                bool Groupempty = a(this.ToDoGroupBox.Text);

                if (this.Linaa.CurrentPref.IsLastToDoNull())
                {
                    if (labelempty && this.ToDoLabelBox.Items.Count != 0) this.ToDoLabelBox.Text = this.ToDoLabelBox.Items[0] as string;
                    if (Groupempty && this.ToDoGroupBox.Items.Count != 0) this.ToDoGroupBox.Text = this.ToDoGroupBox.Items[0] as string;
                    SetLastData();
                }
                else
                {
                    string[] lastOne = this.Linaa.CurrentPref.LastToDo.Split(c);
                    if (labelempty && !a(lastOne[0])) this.ToDoLabelBox.Text = lastOne[0];
                    if (Groupempty && !a(lastOne[1])) this.ToDoGroupBox.Text = lastOne[1];
                    if (!a(lastOne[2])) this.f0box.Text = lastOne[2];
                    if (!a(lastOne[3])) this.alpha0box.Text = lastOne[3];
                    if (!a(lastOne[4])) this.stepbox.Text = lastOne[4];
                }
            }
            catch (SystemException exception)
            {
                this.Error.SetError(this.ToDoLabelBox.ComboBox, exception.Message);
                this.Linaa.AddException(exception);
            }
        }

        private void SetLastData()
        {
            try
            {
                string msg = this.ToDoLabelBox.Text + c + this.ToDoGroupBox.Text + c + this.f0box.Text + c + this.alpha0box.Text + c + this.stepbox.Text;

                this.Linaa.CurrentPref.LastToDo = msg;
                this.Linaa.SavePreferences();
            }
            catch (SystemException exception)
            {
                this.Error.SetError(this.ToDoLabelBox.ComboBox, exception.Message);
                this.Linaa.AddException(exception);
            }
        }

        public void SaveToDo_Click(object sender, EventArgs e)
        {
            this.Error.SetError(this.ToDoLabelBox.ComboBox, string.Empty);
            try
            {
                this.Validate();
                this.ListBS.EndEdit();

                this.Linaa.Save<LINAA.ToDoDataTable>();

                Dumb.FillABox(this.ToDoLabelBox, this.Linaa.ToDoesList, true, false);
            }
            catch (SystemException exception)
            {
                this.Error.SetError(this.ToDoLabelBox.ComboBox, exception.Message);
                this.Linaa.AddException(exception);
            }
        }

        private void ToDoDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.ListBS.Count == 0) return;
            if (e.RowIndex < 0 || e.RowIndex > this.ToDoDGV.RowCount) return;
            if (this.ToDoDGV.Columns[e.ColumnIndex] != this.refCBC) return;

            //escape if several references are allowed...
            if (!onlyOne.Checked) return;

            //when only one
            LINAA.ToDoRow reft = Dumb.Cast<LINAA.ToDoRow>(this.ToDoDGV.Rows[e.RowIndex]);
            reft.use = true;
            IEnumerable<LINAA.ToDoRow> rows = Dumb.Cast<LINAA.ToDoRow>(this.ListBS.List.Cast<DataRowView>());
            rows = rows.SkipWhile(o => o.Iso.ToUpper().CompareTo(reft.Iso.ToUpper()) != 0);
            rows = rows.SkipWhile(o => o.Equals(reft));
            foreach (LINAA.ToDoRow t in rows) t._ref = false;
        }

        private void BestSD_CheckedChanged(object sender, EventArgs e)
        {
            if (chkchanging) return;

            chkchanging = true;

            bool ch = false;
            // ToolStripMenuItem item = sender as ToolStripMenuItem;
            //  item.CheckedChanged -= BestSD_CheckedChanged;
            if (sender.Equals(this.BestSD))
            {
                ch = !BestSD.Checked;
                BestFit.Checked = ch;
                iterationsToolStripMenuItem.Checked = ch;
            }
            else if (sender.Equals(this.BestFit))
            {
                ch = !BestFit.Checked;
                BestSD.Checked = ch;
                iterationsToolStripMenuItem.Checked = ch;
            }
            else if (sender.Equals(this.iterationsToolStripMenuItem))
            {
                ch = !iterationsToolStripMenuItem.Checked;
                BestSD.Checked = ch;
                BestFit.Checked = ch;
            }
            chkchanging = false;
        }

        private void Export_Click(object sender, EventArgs e)
        {
            try
            {
                this.Linaa.WriteXml(this.ToDoLabel.Text + ".xml", XmlWriteMode.WriteSchema);
                this.process.StartInfo.FileName = "excel.exe";
                this.process.StartInfo.Arguments = "/e " + this.ToDoLabel.Text + ".xml";
                this.process.Start();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        //collapsable

        #region Collapsable strings

        private string showfilter
        {
            get
            {
                return "use = '" + showUsed.Checked.ToString() + "'";
            }
        }

        private string MinimumFil
        {
            get
            {
                return "label = '" + ToDoLabelBox.Text + "' AND project IS NOT NULL";
            }
        }

        private string rawfil = string.Empty;
        private string listfil = string.Empty;
        private string avgfil = string.Empty;
        private string avguncfil = string.Empty;

        private string rawSort = "Iso, Energy asc";
        private string listSort = "Iso,Sym asc";
        private string avgSort = "Er asc";
        private string avgUncSort = "Iso,Sym asc";
        private string resavgSort = "Iso,Sym asc";
        private string resSort = "Iso,Sym asc";

        #endregion Collapsable strings

        private void Link(bool resetFilter)
        {
            try
            {
                if (resetFilter)
                {
                    listfil = MinimumFil;
                    rawfil = showfilter + " AND " + MinimumFil;
                    avgfil = showfilter + " AND " + MinimumFil;
                    avguncfil = showfilter + " AND " + MinimumFil;
                }

                Dumb.LinkBS(ref this.ListBS, this.Linaa.ToDo, listfil, listSort);
                Dumb.LinkBS(ref this.RawBS, this.Linaa.ToDoData, rawfil, rawSort);
                Dumb.LinkBS(ref this.AvgBS, this.Linaa.ToDoAvg, avguncfil, avgSort);
                Application.DoEvents();
                Dumb.LinkBS(ref this.AvgUncBS, this.Linaa.ToDoAvgUnc, showfilter, avgUncSort);
                Dumb.LinkBS(ref this.ResBS, this.Linaa.ToDoRes, showfilter, resSort);
                Dumb.LinkBS(ref this.ResAvgBS, this.Linaa.ToDoResAvg, showfilter, resavgSort);
            }
            catch (SystemException ex)
            {
                this.Error.SetError(this.ToDoLabelBox.ComboBox, ex.Message + "\n\n" + ex.StackTrace);
                this.Linaa.AddException(ex);
            }
        }

        private void DeLink()
        {
            try
            {
                string[] aux = Dumb.DeLinkBS(ref this.ListBS);
                listSort = aux[0];
                listfil = aux[1];
                aux = Dumb.DeLinkBS(ref this.RawBS);
                rawSort = aux[0];
                rawfil = aux[1];
                aux = Dumb.DeLinkBS(ref this.AvgBS);
                avgSort = aux[0];
                avgfil = aux[1];
                aux = Dumb.DeLinkBS(ref this.AvgUncBS);
                avgUncSort = aux[0];
                avguncfil = aux[1];
            }
            catch (SystemException ex)
            {
                this.Error.SetError(this.ToDoLabelBox.ComboBox, ex.Message + "\n\n" + ex.StackTrace);
                this.Linaa.AddException(ex);
            }

            Application.DoEvents();
        }

        private void showUsed_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView dgv = CMS.SourceControl as DataGridView;

            if (!GoodDGV(ref dgv)) return;
            if (splitContainer1.Panel1.Controls.Count == 0) return;

            VTools.TVFilter tvfil = splitContainer1.Panel1.Controls[0] as VTools.TVFilter;
            string minFil = MinimumFil;
            if (dgv.Equals(this.RawDGV) || dgv.Equals(this.AvgDGV)) minFil = showfilter + " AND " + MinimumFil;
            else if (!dgv.Equals(this.ToDoDGV)) minFil = showfilter;
            tvfil.MinFilter = minFil;
            tvfil.Filter();
        }

        private void ToDoLabelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Link(true);
            SetLastData();
        }

        private void ToDoGroupBox_TextUpdate(object sender, EventArgs e)
        {
            if (ToDoGroupBox.Text.Equals(string.Empty)) return;
            if (!ToDoGroupBox.Items.Contains(ToDoGroupBox.Text)) return;

            tocalculate = TD.SetToDoType(ToDoGroupBox.Text.ToUpper());
            if (tocalculate == LINAA.ToDoType.Q0determination) once.Checked = true;
            else if (tocalculate == LINAA.ToDoType.k0determination) once.Checked = true;
            else once.Checked = false;

            SetLastData();
        }

        public bool GoodDGV(ref DataGridView dgv)
        {
            if (dgv.CurrentCell == null) return false;
            if (dgv.Tag == null)
            {
                string filter = MinimumFil;
                if (dgv.Equals(this.RawDGV) || dgv.Equals(this.AvgDGV)) filter = showfilter + " AND " + MinimumFil;
                else if (!dgv.Equals(this.ToDoDGV)) filter = showfilter;
                CreateTVFilter(ref dgv, this.Linaa.ToDo.ColsToFilter.Select(c => c.ColumnName).ToArray(), filter);
            }
            return true;
        }

        /*
        private void filterByToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
           ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
           ContextMenuStrip s = tsmi.GetCurrentParent() as ContextMenuStrip;
           DataGridView dgv = s.SourceControl as DataGridView;
           DataGridViewCell cell = dgv.CurrentCell;

           if (dgv == null) return;
           string field = dgv.Columns[cell.ColumnIndex].DataPropertyName;
           string value = cell.Value.ToString();
           BindingSource bs = Rsx.DGV.Control.GetDataSource<BindingSource>(ref dgv);

           if (this.filterByToolStripMenuItem.Checked)
           {
              bs.Filter = showfilter + " AND " + field + " = '" + value + "'";
           }
           else bs.Filter = showfilter;
        }
          */

        private void ToDoDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (!GoodDGV(ref dgv)) return;
            if (dgv.CurrentCell == null) return;
            DataGridViewColumn col = dgv.Columns[dgv.CurrentCell.ColumnIndex];
            object o = col.Tag;
            if (o == null) return;

            VTools.TVFilter tvfil = col.Tag as VTools.TVFilter;
            if (tvfil.Nodes.Count == 0) tvfil.Fill.PerformClick();

            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel1.Controls.Add(tvfil);
        }

        private void CreateTVFilter(ref DataGridView dgv, string[] arrofCols, string filtertoPut)
        {
            BindingSource bs = Rsx.DGV.Control.GetDataSource<BindingSource>(ref dgv);
            if (bs.DataSource == null) return;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Tag != null) continue;

                bool ok = arrofCols.Contains(col.DataPropertyName);

                if (!ok) continue;

                VTools.TVFilter tvfil = new VTools.TVFilter();
                tvfil.Dock = DockStyle.Fill;
                tvfil.BS = bs;
                col.Tag = tvfil;
                tvfil.Tag = dgv;
                tvfil.Field = col.DataPropertyName;
                tvfil.MinFilter = filtertoPut;
            }

            dgv.Tag = true;
        }

        private void SRbton_Click(object sender, EventArgs e)
        {
            string result = string.Empty;

            //array of Z columns that SR

            DataGridView dgv = this.SRDGV;
            //take DataView
            result = TD.CreateSelectReject(ref dgv, ref this.ResAvgBS);

            this.Error.SetError(this.ToDoLabelBox.ComboBox, result);
        }

        private void propagateSR_Click(object sender, EventArgs e)
        {
            TD.PropagateSR();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            TD.Reset();
        }

        private void Ai_Click(object sender, EventArgs e)
        {
            TD.SetParameters(alpha0box.Text, f0box.Text, Optimize.Checked, useRefe.Checked);
            DeLink();
            TD.IAvgs = this.Linaa.ToDoAvg.AsEnumerable().ToList();
            TD.CalculateAi();
            Link(false);
            //	TD.SetToDoesGroup();
        }

        private void SetFC_Click(object sender, EventArgs e)
        {
            DeLink();
            TD.CalculateFC();
            Link(false);
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            string error = this.Error.GetError(this.ToDoLabelBox.ComboBox);
            if (!error.Equals(string.Empty)) return;

            this.Error.SetError(this.f0box.TextBox, string.Empty);
            this.Error.SetError(this.alpha0box.TextBox, string.Empty);

            bool delink = true;
            TD.IAvgs = this.Linaa.ToDoAvg.AsEnumerable().ToList();
            if (sender.Equals(this.calculateThis))
            {
                DataRowView v = (DataRowView)this.AvgBS.Current;
                LINAA.ToDoAvgRow r = Dumb.Cast<LINAA.ToDoAvgRow>(v);
                List<LINAA.ToDoAvgRow> hs = new List<LINAA.ToDoAvgRow>();
                hs.Add(r);
                TD.IAvgs = hs;

                delink = false;
            }

            bool k0 = tocalculate.Equals(LINAA.ToDoType.k0determination);
            bool onc = once.Checked || k0 || !delink;

            once.Checked = onc;

            //      DataView view = this.ListBS.List as DataView;
            //      string orgFil = view.RowFilter;
            //     view.RowFilter += " AND project IS NOT NULL AND project2 IS NOT NULL";
            //     IEnumerable<LINAA.ToDoRow> active = Dumb.Cast<LINAA.ToDoRow>(view).ToList();
            //     view.RowFilter = orgFil;
            //

            if (delink) DeLink();

            // this.Linaa.SetToDoDataRelations(k0);

            if (!tocalculate.Equals(LINAA.ToDoType.Q0determination))
            {
                if (k0) Calculatek0BySteps();
                else CalculatefAlphaBySteps();
            }
            else CalculateQoBySteps();

            if (delink) Link(false);

            SetLastData();
        }

        private void PrepareForCalculation(object sender, EventArgs e)
        {
            string result = string.Empty;
            DateTime start = DateTime.Now;

            Cursor.Current = Cursors.WaitCursor;

            this.Error.SetError(this.ToDoLabelBox.ComboBox, null);

            try
            {
                SetDGVColumns(tocalculate);
                string fc = this.Linaa.ToDoAvg.FcColumn.ColumnName;
                string f = this.Linaa.ToDoAvg.fColumn.ColumnName;
                bool cdcovered = tocalculate.Equals(LINAA.ToDoType.fAlphaCdCovered);
                //set value to average
                this.Rawbox.Text = fc;
                if (cdcovered) this.Avgbox.Text = fc;
                else this.Avgbox.Text = f;
            }
            catch (SystemException ex)
            {
                result = ex.Message + "\n\n" + ex.StackTrace;
                this.Linaa.AddException(ex);
            }

            //     if (tocalculate != LINAA.ToDoType.k0determination)
            {
                DataView view = this.ListBS.List as DataView;
                string orgFil = view.RowFilter;
                view.RowFilter += " AND project IS NOT NULL AND project2 IS NOT NULL";
                TD.IList = Dumb.Cast<LINAA.ToDoRow>(view);
                view.RowFilter = orgFil;
                DeLink();
                result += TD.Prepare(Convert.ToInt16(minPosBox.Text));  //VIP!!! Prepares the tables...
                Link(false);
            }
            this.Error.SetError(this.ToDoLabelBox.ComboBox, result);

            this.Linaa.Msg(result + " on: " + ToDoLabelBox.Text + " - " + ToDoGroupBox.Text, "Prepared in " + (DateTime.Now - start).Seconds + " seconds");

            Cursor.Current = Cursors.Default;
        }

        private void MainTab_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.escape = true;
        }

        private void CalculatefAlphaBySteps()
        {
            try
            {
                //initialiaze auxiliar variables
                int num = 0;
                double alpha0 = Convert.ToDouble(this.alpha0box.Text);
                double step = Convert.ToDouble(this.stepbox.Text);
                double sum_residuals = 0.0;
                double lastdev = 100.0;
                bool finished = false;
                DataColumn res = this.Linaa.ToDoAvg.ResColumn;
                string useCol = this.Linaa.ToDoAvg.useColumn.ColumnName;

                while (!this.escape)
                {
                    this.alpha0box.Text = decimal.Round(Convert.ToDecimal(alpha0), 6).ToString();

                    //calculations

                    TD.SetParameters(alpha0box.Text, f0box.Text, Optimize.Checked, useRefe.Checked);

                    TD.CalculateAi();
                    TD.CalculateFC();
                    this.RawColumn_Average();
                    this.falphaPlotIter();
                    Application.DoEvents();
                    //2nd stage
                    this.AvgColumn_Average();
                    alphaSD.Text = TD.Fit.AlphaSD.ToString();
                    this.falphaPlotIter2();
                    Application.DoEvents();

                    //everything is calculated for the current f0box and alphabox
                    //now guess the next one...

                    string f0Org = f0box.Text;

                    if (this.once.Checked || finished || escape) break;
                    else
                    {
                        if (tocalculate == LINAA.ToDoType.fAlphaCdRatio) f0box.Text = fbox.Text;
                        else if (tocalculate == LINAA.ToDoType.fAlphaBare) f0box.Text = AvgWAvg.Text;
                        else f0box.Text = "0";

                        if (BestSD.Checked) sum_residuals = Convert.ToDouble(this.AvgSDObs.Text);
                        else if (BestFit.Checked) sum_residuals = MyMath.ListFrom(res, 1, useCol, true).Sum();
                    }
                    //finish iteration?
                    if (sum_residuals > lastdev)
                    {
                        //seems this alpha+step with the "best" f did not give good results,
                        //therefore rool back alpha
                        alpha0 -= step;
                        finished = true;
                        f0box.Text = f0Org;  //and roll back f,
                        //because fbox has the best value for the current bad alpha!

                        //make a last computation, then break
                    }
                    else
                    {
                        //a new "best" f is obtained from the current alpha0
                        //now try add a step to alpha and with this f
                        //recompute
                        lastdev = sum_residuals;
                        alpha0 += step;
                        num++;
                    }

                    Application.DoEvents();
                }

                this.escape = false;
            }
            catch (SystemException exception)
            {
                this.Linaa.AddException(exception);
                this.Error.SetError(this.alpha0box.TextBox, exception.Message);
            }
        }

        private void CalculateQoBySteps()
        {
            try
            {
                //initialiaze auxiliar variables
                int num = 0;
                double alpha0 = Convert.ToDouble(this.alpha0box.Text);
                double step = Convert.ToDouble(this.stepbox.Text);
                double sum_residuals = 0.0;
                double lastdev = 1000000.0;
                bool finished = false;
                string useCol = this.Linaa.ToDoAvg.useColumn.ColumnName;
                double bestResidual = lastdev * 10;

                while (!this.escape)
                {
                    this.alpha0box.Text = decimal.Round(Convert.ToDecimal(alpha0), 6).ToString();

                    //calculations

                    TD.SetParameters(alpha0box.Text, f0box.Text, Optimize.Checked, useRefe.Checked);

                    TD.CalculateAi();
                    TD.CalculateFC();
                    this.QoErPlotIter();

                    if (this.once.Checked || finished || escape) break;
                    else
                    {
                        sum_residuals = MyMath.ListFrom(this.Linaa.ToDoAvg.dQoColumn, 2, "use", true).Sum();
                        sum_residuals = Math.Sqrt(sum_residuals);
                    }
                    //finish iteration?
                    if (sum_residuals > lastdev)
                    {
                        //seems this alpha+step with the "best" f did not give good results,
                        //therefore rool back alpha
                        alpha0 -= step;
                        double faux = Convert.ToDouble(f0box.Text);
                        f0box.Text = (faux - (faux * 0.01)).ToString();  //and roll back f,
                        //because fbox has the best value for the current bad alpha!
                        if (sum_residuals < bestResidual) bestResidual = lastdev;
                        else finished = true;
                        //make a last computation, then break
                    }
                    else
                    {
                        lastdev = sum_residuals;
                        alpha0 += step;
                        num++;
                    }

                    Application.DoEvents();
                }

                this.escape = false;
            }
            catch (SystemException exception)
            {
                this.Linaa.AddException(exception);
                this.Error.SetError(this.alpha0box.TextBox, exception.Message);
            }
        }

        private void Calculatek0BySteps()
        {
            try
            {
                //initialiaze auxiliar variables
                int num = 0;
                double alpha0 = Convert.ToDouble(this.alpha0box.Text);
                //  double step = Convert.ToDouble(this.stepbox.Text);
                //   double sum_residuals = 0.0;
                // double lastdev = 1000000.0;
                //  bool finished = false;
                //     string useCol = this.Linaa.ToDoAvg.useColumn.ColumnName;
                //    double bestResidual = lastdev * 10;

                this.alpha0box.Text = decimal.Round(Convert.ToDecimal(alpha0), 6).ToString();

                //calculations

                TD.SetParameters(alpha0box.Text, f0box.Text, Optimize.Checked, useRefe.Checked);

                //      IList<LINAA.ToDoRow> nonrefes = copy.Where(o => !o._ref).ToList();

                //    IList<LINAA.ToDoRow> refes = copy.Where(o => o._ref).ToList();

                //     copy = null;

                //  LINAA.ToDoDataDataTable toAvg = new LINAA.ToDoDataDataTable(false);
                //   toAvg.Constraints.Clear();

                //   while (!this.escape )
                //  {
                // IList<LINAA.ToDoRow> aux = nonrefes.ToList();
                //   aux.Add(refes.FirstOrDefault());
                //  //      nonrefes.Add(ARefe);
                //    TD.IList = aux.ToList();
                //  string result = TD.Prepare();

                //    if (!result.Equals(string.Empty))
                //   {
                // refes.RemoveAt(0);
                //  continue;
                //   }

                TD.CalculateAi();
                TD.CalculateFC();

                //     toAvg.Merge(this.Linaa.ToDoData, true, MissingSchemaAction.AddWithKey);

                Application.DoEvents();

                //  if (this.once.Checked ) break;

                //   refes.RemoveAt(0);

                ///  }

                //  this.Linaa.ToDoData.Clear();
                //  this.Linaa.ToDoData.Merge(toAvg, true, MissingSchemaAction.AddWithKey);
                //   this.Linaa.ToDoData.AcceptChanges();

                //   this.escape = false;
            }
            catch (SystemException exception)
            {
                this.Linaa.AddException(exception);
                this.Error.SetError(this.alpha0box.TextBox, exception.Message);
            }
        }

        private void SetDGVColumns(LINAA.ToDoType todoType)
        {
            /*
            bool visibility = false;
            visibility = todoType == LINAA.ToDoType.Q0determination || todoType == LINAA.ToDoType.fAlphaCdRatio || todoType == LINAA.ToDoType.k0determination;

            this.Energy.Visible = visibility;
            this.Energy2.Visible = visibility;
            this.k0.Visible = visibility;
            this.k0Unc.Visible = visibility;
            this.k02.Visible = visibility;
            this.k02Unc.Visible = visibility;
            this.dk0.Visible = visibility;
            this.dk02.Visible = visibility;
              */
        }

        private void falphaPlotIter2()
        {
            this.PlottyfAlpha.SuspendLayout();
            this.PlottyfAlpha.Title = this.ToDoGroup.Text + ": " + this.ToDoLabel.Text;

            PointPlot p3 = (PointPlot)this.PlottyfAlpha.Drawables[0];
            //      PointPlot p3 = (PointPlot)this.PlottyfAlpha.Drawables[2];
            //   p3.ShowInLegend = false;

            p3.OrdinateData = this.TD.Fit.Quantity;
            p3.AbscissaData = this.TD.Fit.Alphas;

            PointPlot p5 = (PointPlot)this.PlottyfAlpha.Drawables[1];
            //    PointPlot p5 = (PointPlot)this.PlottyfAlpha.Drawables[3];
            //   PointPlot p5 = new PointPlot(p4.Marker);
            //   p5.ShowInLegend = false;
            p5.AbscissaData = this.TD.Fit.Alphas;
            p5.OrdinateData = this.TD.Fit.Qo;

            /*
		   this.PlottyfAlpha.Remove(p2, true);
		   this.PlottyfAlpha.Remove(p3, true);
		   this.PlottyfAlpha.Remove(p4, true);
		   this.PlottyfAlpha.Remove(p5, true);

		   this.PlottyfAlpha.Add(p3);
		   this.PlottyfAlpha.Add(p2);
		   this.PlottyfAlpha.Add(p4);
		   this.PlottyfAlpha.Add(p5);
              */
            this.PlottyfAlpha.ResumeLayout();
            this.PlottyfAlpha.Refresh();
        }

        private void QoErPlotIter()
        {
            this.plotQoEr.SuspendLayout();
            this.plotQoEr.Title = this.ToDoGroup.Text + ": " + this.ToDoLabel.Text;

            PointPlot p3 = (PointPlot)this.plotQoEr.Drawables[0];
            //   PointPlot p3 = new PointPlot(p2.Marker);
            //	   p3.ShowInLegend = false;

            p3.AbscissaData = this.TD.Fit.XLog;
            p3.OrdinateData = this.TD.Fit.Qo;

            PointPlot p5 = (PointPlot)this.plotQoEr.Drawables[1];
            //   PointPlot p5 = new PointPlot(p4.Marker);
            //   p5.ShowInLegend = false;
            p5.AbscissaData = this.TD.Fit.XLog;
            p5.OrdinateData = this.TD.Fit.Quantity;
            /*
            plotQoEr.Remove(p2, true);
            plotQoEr.Remove(p4, true);
            plotQoEr.Remove(p3, true);
            plotQoEr.Remove(p5, true);
            plotQoEr.Add(p3);
            plotQoEr.Add(p5);
            plotQoEr.Add(p2);
            plotQoEr.Add(p4);
              */
            this.plotQoEr.ResumeLayout();
            plotQoEr.Refresh();
        }

        private void falphaPlotIter()
        {
            this.fbox.Clear();
            this.alphabox.Clear();

            this.PlotLogLog.SuspendLayout();

            this.PlotLogLog.Title = this.ToDoGroup.Text + ": " + this.ToDoLabel.Text;
            this.fbox.Text = TD.Fit.f.ToString();
            this.alphabox.Text = TD.Fit.Alpha.ToString();
            this.R2box.Text = TD.Fit.R2.ToString();
            this.SEfbox.Text = TD.Fit.SEf.ToString();
            this.SEAlphabox.Text = TD.Fit.SEAlpha.ToString();

            PointPlot p = (PointPlot)this.PlotLogLog.Drawables[0];
            p.OrdinateData = TD.Fit.YLog;
            p.AbscissaData = TD.Fit.XLog;
            LinePlot plot2 = (LinePlot)this.PlotLogLog.Drawables[3];
            plot2.OrdinateData = TD.Fit.YCalc;
            plot2.AbscissaData = p.AbscissaData;
            PointPlot plot3 = (PointPlot)this.PlotLogLog.Drawables[2];
            plot3.OrdinateData = TD.Fit.YErrorHigh;
            plot3.AbscissaData = p.AbscissaData;
            PointPlot plot4 = (PointPlot)this.PlotLogLog.Drawables[1];
            plot4.OrdinateData = TD.Fit.YErrorLow;
            plot4.AbscissaData = p.AbscissaData;
            PointPlot p2 = (PointPlot)this.PlotLogLog.Drawables[4];
            p2.OrdinateData = TD.Fit.Y;
            p2.AbscissaData = TD.Fit.X;

            //  LabelPointPlot lp = (LabelPointPlot)this.PlotLogLog.Drawables[5];
            //  lp.OrdinateData = TD.Fit.Isotopes;
            //  lp.AbscissaData = new HashSet<double>(TD.Fit.X);
            /*
     this.PlotLogLog.Remove(p, true);
     this.PlotLogLog.Remove(plot4, true);
     this.PlotLogLog.Remove(plot3, true);
     this.PlotLogLog.Remove(plot2, true);
     this.PlotLogLog.Remove(p2, true);
     //   this.PlotLogLog.Remove(lp, true);

     this.PlotLogLog.Add(p);
     this.PlotLogLog.Add(plot4);
     this.PlotLogLog.Add(plot3);
     this.PlotLogLog.Add(plot2);
     this.PlotLogLog.Add(p2);
     //   this.PlotLogLog.Add(lp);
              */
            this.PlotLogLog.ResumeLayout();

            this.PlotLogLog.Refresh();
        }

        private void PreparePlots()
        {
            try
            {
                #region LOGLOG

                this.PlotLogLog.Clear();

                Legend legend = new Legend();
                legend.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Right);
                this.PlotLogLog.Legend = legend;
                this.PlotLogLog.ShowCoordinates = true;
                this.PlotLogLog.AutoScaleAutoGeneratedAxes = true;
                PointPlot p = new PointPlot();
                p.Marker = new Marker(Marker.MarkerType.Square, 5, new Pen(Color.Green, 3f), true);
                p.ShowInLegend = true;
                p.Label = "Data";
                PointPlot p2 = new PointPlot();
                p2.Marker = new Marker(Marker.MarkerType.Square, 5, new Pen(Color.Red, 3f), true);
                p2.ShowInLegend = true;
                p2.Label = "Not Used";

                LinePlot plot2 = new LinePlot();
                plot2.ShowInLegend = true;
                plot2.Label = "Fitted";
                PointPlot plot3 = new PointPlot();
                plot3.Marker = new Marker(Marker.MarkerType.Square, 2, new Pen(Color.Blue, 2f), true);
                plot3.ShowInLegend = true;
                plot3.Label = "Errors";
                PointPlot plot4 = new PointPlot();
                plot4.Marker = new Marker(Marker.MarkerType.Square, 2, new Pen(Color.Blue, 2f), true);
                plot4.ShowInLegend = false;

                //    PointPlot labelp = new LabelPointPlot();
                //   labelp.ShowInLegend = true;
                //   labelp.Label = "Isotopes";

                this.PlotLogLog.Add(p);
                this.PlotLogLog.Add(plot4);
                this.PlotLogLog.Add(plot3);
                this.PlotLogLog.Add(plot2);
                this.PlotLogLog.Add(p2);
                //  this.PlotLogLog.Add(labelp);
                this.PlotLogLog.Refresh();

                #endregion LOGLOG

                #region fvsAlpha

                this.PlottyfAlpha.Clear();
                this.PlottyfAlpha.ShowCoordinates = true;
                this.PlottyfAlpha.AutoScaleAutoGeneratedAxes = true;
                Legend legend2 = new Legend();
                legend2.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Right);
                PlottyfAlpha.Legend = legend2;

                PointPlot pg = new PointPlot();
                pg.Marker = new Marker(Marker.MarkerType.Square, 5, new Pen(Color.Red, 3f), true);
                pg.ShowInLegend = true;
                pg.Label = "f";

                PointPlot ef = new PointPlot();
                ef.Marker = new Marker(Marker.MarkerType.Square, 5, new Pen(Color.Green, 3f), true);
                ef.ShowInLegend = true;
                ef.Label = "Qo(alpha)";

                //   PointPlot p3 = new PointPlot(p2.Marker);
                //    p3.ShowInLegend = false;

                this.PlottyfAlpha.Add(pg); //0
                this.PlottyfAlpha.Add(ef);    //1
                this.PlottyfAlpha.Refresh();

                #endregion fvsAlpha

                #region QoEr

                this.plotQoEr.Clear();
                this.plotQoEr.ShowCoordinates = true;
                this.plotQoEr.AutoScaleAutoGeneratedAxes = true;
                Legend legend3 = new Legend();
                legend3.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Right);
                plotQoEr.Legend = legend3;

                PointPlot d = new PointPlot();
                d.Marker = new Marker(Marker.MarkerType.Square, 5, new Pen(Color.Green, 3f), true);
                d.ShowInLegend = true;
                d.Label = "Qo(alpha)";

                PointPlot e = new PointPlot();
                e.Marker = new Marker(Marker.MarkerType.Square, 5, new Pen(Color.Red, 3f), true);
                e.ShowInLegend = true;
                e.Label = "f";

                plotQoEr.Add(d);
                plotQoEr.Add(e);
                plotQoEr.Refresh();

                #endregion QoEr
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void RawColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            RawColumn_Average();
        }

        private void AvgColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            AvgColumn_Average();
        }

        private void AvgColumn_Average()
        {
            string zero = "0";
            this.AvgAvg.Text = zero;
            this.AvgSD.Text = zero;
            this.AvgWAvg.Text = zero;
            this.AvgSDObs.Text = zero;
            alphaSD.Text = zero;

            this.Error.SetError(this.Avgbox.ComboBox, null);

            try
            {
                bool isf = Avgbox.Text.Equals(this.Linaa.ToDoAvg.fColumn.ColumnName);
                bool isAlpha = Avgbox.Text.Equals(this.Linaa.ToDoAvg.alphaColumn.ColumnName);

                string filter = this.Linaa.ToDoAvg.useColumn.ColumnName;

                string FieldUnc = string.Empty;

                if (isf) FieldUnc = this.Linaa.ToDoAvg.fUncColumn.ColumnName;
                else if (isAlpha) FieldUnc = this.Linaa.ToDoAvg.alphaUncColumn.ColumnName;
                else FieldUnc = this.Linaa.ToDoAvg.SDColumn.ColumnName;

                IEnumerable<DataRow> rows = this.AvgBS.List.OfType<DataRowView>().Select(o => o.Row);
                decimal[] averages = TD.Average(this.Avgbox.Text, FieldUnc, filter, ref rows);
                this.AvgAvg.Text = averages[0].ToString();
                this.AvgSD.Text = averages[1].ToString();
                this.AvgWAvg.Text = averages[2].ToString();
                this.AvgSDObs.Text = averages[3].ToString();
            }
            catch (SystemException ex)
            {
                this.Error.SetError(this.Avgbox.ComboBox, ex.Message);
                this.Linaa.AddException(ex);
            }
        }

        private void RawColumn_Average()
        {
            this.Error.SetError(this.Rawbox.ComboBox, null);
            string zero = "0";
            this.RawAvg.Text = zero;
            this.RawSD.Text = zero;
            this.RawWAvg.Text = zero;
            this.RawSDObs.Text = zero;
            try
            {
                string filter = this.Linaa.ToDoData.useColumn.ColumnName;
                string fieldSD = this.Linaa.ToDoData.SDColumn.ColumnName;

                IEnumerable<DataRow> rows = this.RawBS.List.OfType<DataRowView>().Select(o => o.Row);

                decimal[] averages = TD.Average(this.Rawbox.Text, fieldSD, filter, ref rows);
                this.RawAvg.Text = averages[0].ToString();
                this.RawSD.Text = averages[1].ToString();
                this.RawWAvg.Text = averages[2].ToString();
                this.RawSDObs.Text = averages[3].ToString();
            }
            catch (SystemException ex)
            {
                this.Error.SetError(this.Rawbox.ComboBox, ex.Message);
                this.Linaa.AddException(ex);
            }
        }

        private void addFilter_Click(object sender, EventArgs e)
        {
            string aux = this.filterColBox.Text;
            if (string.IsNullOrWhiteSpace(aux)) return;
            this.filterColBox.Items.Add(aux);
        }

        /*
   private void ToDoDGV_Enter(object sender, EventArgs e)
   {
       DataGridView dgv = sender as DataGridView;
       if (dgv.Equals(this.ResAvgDGV)) return;
       else if (dgv.Equals(this.ResDGV)) return;

       k0X.TVFilter tvfil = null;
       string newminFilt = "label = '" + ToDoLabelBox.Text + "'";

       if (dgv.Tag==null)
       {
          tvfil = new TVFilter();
          tvfil.Dock = DockStyle.Fill;
          BindingSource bs = Rsx.DGV.Control.GetDataSource<BindingSource>(ref dgv);
          tvfil.BS = bs;
          dgv.Tag = tvfil;
          tvfil.MinFilter = newminFilt;
       }
      else tvfil = dgv.Tag as k0X.TVFilter;

      string oldminFilter = tvfil.MinFilter;

      if (oldminFilter.CompareTo(newminFilt) != 0) tvfil.MinFilter = newminFilt;

      DataGridViewColumn col = dgv.Columns[dgv.CurrentCell.ColumnIndex];

      tvfil.Field = col.DataPropertyName;

      if (tvfil.BS.Equals(this.RawBS)) RawColumn_Average();
      else if (tvfil.BS.Equals(this.AvgBS)) AvgColumn_Average();

      splitContainer1.Panel1.Controls.Clear();
      splitContainer1.Panel1.Controls.Add(tvfil);
   }

       */
    }
}