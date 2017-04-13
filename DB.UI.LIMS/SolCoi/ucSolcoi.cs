using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx;
using Rsx.DGV;

namespace DB.UI
{
    public partial class ucSolcoi : UserControl
    {
        public ucDetectors ucDetectors;
        private string Canceltext = "Cancel";
        private string stopText = "User Cancelled. Please wait while running units finish their job";
        internal System.Collections.Hashtable solcois;
        private String solcoinPath = string.Empty;

        private Interface Interface;

        public ucSolcoi(ref Interface inter)
        {
            InitializeComponent();
            this.SuspendLayout();

            cancel.Visible = false;

            Interface = inter;

            Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();

            solcois = new System.Collections.Hashtable();
            solcoinPath = this.Linaa.FolderPath + DB.Properties.Resources.SolCoiFolder;

            Rsx.Dumb.LinkBS(ref this.BS, this.Linaa.Solang);

            // Rsx.Dumb.LinkBS(ref this.effiBS, this.Linaa.COIN);

            ucDetectors = new ucDetectors();
            ucDetectors.Set(ref inter);
            this.DetectorsSpecs.Controls.Add(ucDetectors);
            ucDetectors.Dock = DockStyle.Fill;

            Auxiliar form = new Auxiliar();

            UserControl control = this;
            form.Populate(control);
            form.Text = "SOLCOI Panel";

            IList<string> hs = Dumb.HashFrom<string>(this.Linaa.DetectorsCurves.RefGeometryColumn);
            Dumb.FillABox(refBox, hs, true, false);
            hs = Dumb.HashFrom<string>(this.Linaa.DetectorsCurves.RefPositionColumn);

            Dumb.FillABox(refPosBox, hs, true, false);

            refBox.SelectedItem = refBox.Items[0];
            refPosBox.SelectedItem = refPosBox.Items[0];

            RefreshDetectorsTV();
            RefreshGeometriesTV();

            this.ResumeLayout();
            form.Show();

            SolCoin.KillHanged();
        }

        #region Fills / Save

        private void FillSolang_Click(object sender, EventArgs e)
        {
            try
            {
                if (detSolangbox.Text != String.Empty)
                {
                    if (geoSolangbox.Text != String.Empty)
                    {
                        this.Linaa.TAM.SolangTableAdapter.FillByGeometryDetector(this.Linaa.Solang, this.geoSolangbox.Text, this.detSolangbox.Text);
                    }
                }
                if (this.Linaa.Solang.Rows.Count != 0)
                {
                    if (crossTableToolStripMenuItem.Checked)
                    {
                        System.Data.DataColumn posCol = this.Linaa.Solang.PositionColumn;
                        System.Data.DataColumn eneCol = this.Linaa.Solang.EnergyColumn;
                        xTable.New(2, ref posCol, ref eneCol, ref this.SolangDGV);
                        xTable.Fill_Xij(ref this.SolangDGV, 3, this.Linaa.Solang.SolangColumn.ColumnName, this.Linaa.Solang.IDColumn.ColumnName, 0);
                        DataGridViewColumn aux = this.SolangDGV.Columns[1];
                        xTable.Fill_Zij(ref aux, this.Linaa.Solang.DetectorColumn.ColumnName, null);
                        aux = this.SolangDGV.Columns[0];
                        xTable.Fill_Zij(ref aux, this.Linaa.Solang.GeometryColumn.ColumnName, null);
                        aux = this.SolangDGV.Columns[2];
                        xTable.Fill_Zij(ref aux, this.Linaa.Solang.EnergyColumn.ColumnName, null);

                        System.Data.DataTable table = xTable.DGVToTable(ref SolangDGV);

                        xTable.Clean(ref SolangDGV);
                        this.BS.DataSource = table;
                        this.SolangDGV.DataSource = BS;

                        int columns = table.Columns.Count - 3;
                        int reference = table.Columns.IndexOf(refPosBox.Text);

                        for (int j = 0; j < columns; j++)
                        {
                            table.Columns.Add("C" + table.Columns[3 + j].ColumnName, typeof(double), "[" + table.Columns[3 + j].ColumnName + "]" + "/" + "[" + table.Columns[reference].ColumnName + "]");
                        }
                    }
                    else //not a crossTable
                    {
                        xTable.Clean(ref this.SolangDGV);
                        this.BS.DataSource = this.Linaa.Solang;
                        SolangDGV.DataSource = this.BS;
                    }
                }
            }
            catch (SystemException exception)
            {
                this.Linaa.AddException(exception);
            }
        }

        private void FillEffiCOIs_Click(object sender, EventArgs e)
        {
            try
            {
                this.Linaa.TAM.COINTableAdapter.FillByDetectorGeometry(this.Linaa.COIN, this.deteffibox.Text, this.geoeffibox.Text);
                Dumb.FillABox(isoeffibox, Dumb.HashFrom<string>(this.Linaa.COIN.IsotopeColumn), true, true);
                Dumb.FillABox(energyeffibox, Dumb.HashFrom<string>(this.Linaa.COIN.EnergyColumn), true, false);
                if (this.Linaa.COIN.Count == 0) this.Linaa.PopulateCOIList();
            }
            catch (SystemException exception)
            {
                this.Linaa.AddException(exception);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.Validate();

            this.BS.EndEdit();
            this.Linaa.Save<LINAA.DetectorsCurvesDataTable>();
        }

        #endregion Fills / Save

        #region Go

        internal void Go_Reference(ref TreeNode detectorNode)
        {
            bool success = false;

            SolCoin solcoin = null;
            solcoin = new SolCoin(ref this.Linaa);

            success = solcoin.Gather(refBox.Text, Convert.ToInt16(refPosBox.Text), detectorNode.Text, refBox.Text, Convert.ToInt16(refPosBox.Text));

            if (success)
            {
                solcoin.CalculateCOIS = false;
                solcoin.StoreCOIS = false;
                solcoin.CleanEffiCOIS = false;
                solcoin.CalculateSolidAngles = CalculateSolang.Checked || CalculateCOIS.Checked;   //for cois I need solangs!!!!
                solcoin.CleanSolidAngles = cleanSolang.Checked;
                solcoin.IntegrationMode = SolCoin.IntegrationModes.AsPointSource;
                if (IntegrationRefBox.Text.CompareTo("As Non Disk") == 0) solcoin.IntegrationMode = SolCoin.IntegrationModes.AsNonDisk;
                else if (IntegrationRefBox.Text.CompareTo("As Disk") == 0) solcoin.IntegrationMode = SolCoin.IntegrationModes.AsDisk;
                solcoin.StoreSolidAngles = StoreSolang.Checked;
                solcoin.DoAll(HideCalc.Checked);
                solcoin.Merge();
                solcoin.Dispose();
                solcoin = null;
            }
            else
            {
                RTB.Text += detectorNode.Text + " - " + " - ERROR @ " + solcoin.Exception.InnerException + " - " + solcoin.Exception.Message + "\n\n";
                RTB.ScrollToCaret();
            }

            Application.DoEvents();
        }

        internal void Go_Geometry(ref TreeNode nodeDetector, ref TreeNode nodeGeometry)
        {
            bool success = false;

            SolCoin solcoin2 = new SolCoin(ref this.Linaa);
            success = solcoin2.Gather(nodeGeometry.Text, Convert.ToInt16(GeoPosBox.Text), nodeDetector.Text, refBox.Text, Convert.ToInt16(refPosBox.Text));

            if (success && solcoin2.Exception == null)
            {
                solcoin2.IntegrationMode = SolCoin.IntegrationModes.AsPointSource;
                if (IntegrationBox.Text.CompareTo("As Non Disk") == 0) solcoin2.IntegrationMode = SolCoin.IntegrationModes.AsNonDisk;
                else if (IntegrationBox.Text.CompareTo("As Disk") == 0) solcoin2.IntegrationMode = SolCoin.IntegrationModes.AsDisk;

                System.ComponentModel.BackgroundWorker worker = null;

                worker = new System.ComponentModel.BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                solcoin2.Tag = worker;

                solcois.Add(nodeDetector.Text + " - " + nodeGeometry.Text, solcoin2);
            }
            else
            {
                RTB.Text += nodeDetector.Text + " - " + nodeGeometry.Text + " - ERROR @ " + solcoin2.Exception.InnerException + " - " + solcoin2.Exception.Message + "\n\n";
                RTB.ScrollToCaret();
            }
            Application.DoEvents();
        }

        private bool CalcCoiOrg = false;

        internal void Go_Click(object sender, EventArgs e)
        {
            this.Linaa.Solang.Clear();

            if (this.RTB.Text.Length != 0) this.RTB.Text = "\n\n";
            this.RTB.Text += "  ------------  New Calculations on " + DateTime.Now.ToLongTimeString() + "  ------------\n\n";

            IEnumerable<TreeNode> detnodes = this.TVdet.Nodes.OfType<TreeNode>();
            if (!autoSel.Checked) detnodes = detnodes.Where(o => o.Checked).ToList();
            IEnumerable<TreeNode> geonodes = this.TVgeo.Nodes.OfType<TreeNode>();
            if (!autoSel.Checked) geonodes = geonodes.Where(o => o.Checked).ToList();

            CalcCoiOrg = CalculateCOIS.Checked;

            int ndet = detnodes.Count();
            int ngeo = geonodes.Count();

            finished = 0; //important (COUNTER)

            cancel.Text = Canceltext;
            cancel.Visible = true;

            progress.Value = 0;
            progress.Minimum = 0;
            progress.Maximum = ndet * ngeo;

            solcois.Clear();

            TreeNode auxdet = null;
            TreeNode auxgeo = null;

            Application.DoEvents();

            if (!Reverse.Checked)
            {
                foreach (TreeNode tvdeti in detnodes)
                {
                    TVdet.SelectedNode = tvdeti;
                    auxdet = null;

                    foreach (TreeNode tvgeoi in geonodes)
                    {
                        TVgeo.SelectedNode = tvgeoi;
                        auxgeo = null;
                        if (autoSel.Checked)
                        {
                            LINAA.GeometryRow g = tvgeoi.Tag as LINAA.GeometryRow;
                            if (g != null)
                            {
                                CalculateCOIS.Checked = CalcCoiOrg;
                                g.Position = 5;
                                g.Detector = tvdeti.Text.Trim();
                                g.AcceptChanges();
                                if (g.GetCOINRows().Count() == 0)
                                {
                                    auxgeo = tvgeoi;
                                    CalculateCOIS.Checked = true;
                                    g.Position = 0;
                                }
                            }
                        }
                        else auxgeo = tvgeoi;

                        if (auxgeo != null)
                        {
                            bool doRefFirst = false;
                            if (auxdet == null)
                            {
                                auxdet = tvdeti;
                                doRefFirst = true;
                            }
                            else if (!auxdet.Equals(tvdeti))
                            {
                                auxdet = tvdeti;
                                doRefFirst = true;
                            }
                            if (doRefFirst) Go_Reference(ref auxdet);
                            Go_Geometry(ref auxdet, ref auxgeo);
                        }
                    }
                }
            }
            else
            {
                HashSet<string> hs = new HashSet<string>();

                foreach (TreeNode tvgeoi in geonodes)
                {
                    TVgeo.SelectedNode = tvgeoi;
                    auxgeo = tvgeoi;
                    foreach (TreeNode tvdeti in this.TVdet.Nodes)
                    {
                        if (hs.Add(tvdeti.Text))
                        {
                            TVdet.SelectedNode = tvdeti;
                            auxdet = tvdeti;
                            Go_Reference(ref auxdet);
                        }

                        Go_Geometry(ref auxdet, ref auxgeo);
                    }
                }
                hs.Clear();
            }

            Timer.Interval = 3000;
            Timer.Enabled = true;
        }

        internal void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            SolCoin solcoin = (SolCoin)e.Result;
            //solcoin.Merge();
            if (solcoin.Exception != null)
            {
                RTB.Text += solcoin.DetectorName + " - " + solcoin.Geometry.GeometryName + " - ERROR @ " + solcoin.Exception.InnerException + " - " + solcoin.Exception.Message + "\n\n";
            }
            else
            {
                DateTime end = DateTime.Now;
                double seconds = (end - solcoin.SentAt).TotalSeconds;
                RTB.Text += solcoin.DetectorName + " - " + solcoin.Geometry.GeometryName + " - Finished at " + DateTime.Now + "\tTotal: " + seconds + " (CPU: " + (solcoin.SolangProcessTime + solcoin.COINProcessTime) + ") seconds\n\n";
            }
            RTB.ScrollToCaret();

            solcoin.Dispose();
            solcoin = null;

            finished--;

            progress.PerformStep();
        }

        internal void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SolCoin solcoin = (SolCoin)e.Argument;

            solcoin.CalculateCOIS = CalculateCOIS.Checked;
            solcoin.CalculateSolidAngles = CalculateSolang.Checked || CalculateCOIS.Checked;

            solcoin.CleanEffiCOIS = cleanEffiCois.Checked;
            solcoin.CleanSolidAngles = cleanSolang.Checked;

            solcoin.StoreSolidAngles = StoreSolang.Checked;
            solcoin.StoreCOIS = storeCOIN.Checked;

            solcoin.DoAll(HideCalc.Checked);
            e.Result = e.Argument;
        }

        #endregion Go

        #region Fine Tunning

        private void OFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FineTuneFilebox.Text = OFD.FileName;

            try
            {
                this.Linaa.Solang.Clear();
                this.Linaa.Solang.ReadXml(FineTuneFilebox.Text);
                System.Data.DataColumn poscol = this.Linaa.Solang.PositionColumn;
                System.Data.DataColumn enecol = this.Linaa.Solang.EnergyColumn;
                xTable.New(2, ref poscol, ref enecol, ref this.TuneDGV);
                xTable.Fill_Xij(ref this.TuneDGV, 3, this.Linaa.Solang.SolangColumn.ColumnName, null, 0);
                DataGridViewColumn aux = this.TuneDGV.Columns[0];

                xTable.Fill_Zij(ref aux, this.Linaa.Solang.DetectorColumn.ColumnName, null);
                aux = this.TuneDGV.Columns[1];
                xTable.Fill_Zij(ref aux, this.Linaa.Solang.GeometryColumn.ColumnName, null);
                aux = this.TuneDGV.Columns[2];
                xTable.Fill_Zij(ref aux, this.Linaa.Solang.EnergyColumn.ColumnName, null);

                System.Data.DataTable exp = xTable.DGVToTable(ref TuneDGV);

                xTable.Clean(ref TuneDGV);
                this.Linaa.Solang.Clear();

                int columns = exp.Columns.Count - 3;
                int reference = exp.Columns.IndexOf(refPosBox.Text);

                for (int j = 0; j < columns; j++)
                {
                    exp.Columns.Add("E" + exp.Columns[3 + j].ColumnName, typeof(double), "[" + exp.Columns[3 + j].ColumnName + "]" + "/" + "[" + exp.Columns[reference].ColumnName + "]");
                }

                crossTableToolStripMenuItem.Checked = true;

                detSolangbox.Text = exp.Rows[0]["Detector"].ToString();
                geoSolangbox.Text = exp.Rows[0]["Geometry"].ToString();

                TuneDGV.DataSource = exp;

                //detector dimentions

                LINAA.DetectorsDimensionsRow detdim = this.Linaa.DetectorsDimensions.Select("Detector = '" + detSolangbox.Text + "'")[0] as LINAA.DetectorsDimensionsRow;

                VGbox.Text = detdim.VacuumGap.ToString();
                TDLbox.Text = detdim.TopDeadLayerThickness.ToString();
            }
            catch (SystemException eX)
            {
            }
        }

        private void Tune_Click(object sender, EventArgs e)
        {
            System.Data.DataTable exp = (System.Data.DataTable)TuneDGV.DataSource;
            System.Data.DataTable previous = null;

            SolCoin solcoin = new SolCoin(ref this.Linaa);

            solcoin.CalculateCOIS = false;
            solcoin.CalculateSolidAngles = true;
            solcoin.CleanEffiCOIS = false;
            solcoin.CleanSolidAngles = true;
            solcoin.IntegrationMode = SolCoin.IntegrationModes.AsNonDisk;

            solcoin.StoreCOIS = false;

            solcoin.StoreSolidAngles = true;

            try
            {
                solcoin.Gather(geoSolangbox.Text, 0, detSolangbox.Text, refBox.Text, Convert.ToInt16(refPosBox.Text));
                solcoin.Energies = Dumb.HashFrom<double>(exp.Columns[this.Linaa.Solang.EnergyColumn.ColumnName]).ToArray();
                solcoin.DetectorDimension.VacuumGap = Convert.ToDouble(VGbox.Text);
                solcoin.DetectorDimension.TopDeadLayerThickness = Convert.ToDouble(TDLbox.Text);

                int totaliters = Convert.ToInt16(Iterbox.Text);
                int iteration = 1;
                int rowind = 0;
                double diff = 0;
                double previousvalue = 0;

                Application.DoEvents();

                //iteration should start here!!!!
                while (iteration <= totaliters)
                {
                    solcoin.DoAll(HideCalc.Checked);
                    this.FillSolang_Click(sender, e); //fill solang data
                    System.Data.DataTable calc = xTable.DGVToTable(ref this.SolangDGV);

                    Application.DoEvents();

                    foreach (System.Data.DataColumn col in calc.Columns)
                    {
                        if (col.ColumnName.Contains("C"))  //the column is a calculated solang ratio
                        {
                            String position = col.ColumnName.Replace("C", null).ToString();
                            foreach (System.Data.DataRow row in col.Table.Rows)
                            {
                                try
                                {
                                    rowind = calc.Rows.IndexOf(row);
                                    diff = 1 - (Convert.ToDouble(row[col.ColumnName]) / Convert.ToDouble(exp.Rows[rowind]["E" + position]));
                                    diff = diff * 100;
                                    if (iteration > 1)
                                    {
                                        previousvalue = Math.Abs(Convert.ToDouble(previous.Rows[rowind][position]));
                                        if (previousvalue < Math.Abs(diff)) row.SetColumnError(position, "worst");
                                    }
                                    row[position] = Decimal.Round(Convert.ToDecimal(diff), 1);
                                }
                                catch (SystemException ex)
                                {
                                }
                            }
                        }
                    }

                    previous = calc;
                    this.BS.DataSource = calc;
                    Application.DoEvents();
                    Application.DoEvents();
                    iteration++;
                    this.VGbox.Text = solcoin.DetectorDimension.VacuumGap.ToString();
                    this.TDLbox.Text = solcoin.DetectorDimension.TopDeadLayerThickness.ToString();

                    if (this.VGTune.Checked) solcoin.DetectorDimension.VacuumGap += 0.1;
                    if (this.TDLTune.Checked) solcoin.DetectorDimension.TopDeadLayerThickness += 0.01;

                    // iteration ends
                }
            }
            catch (SystemException eX)
            {
            }
        }

        private void BrowseFineTune_Click(object sender, EventArgs e)
        {
            OFD.ShowDialog();
        }

        #endregion Fine Tunning

        #region TreeViews

        private void RefreshDetectorsTV()
        {
            System.Collections.Generic.ICollection<string> detectors = this.Linaa.DetectorsList.ToList();

            Dumb.FillABox(detSolangbox, detectors, true, false);
            Dumb.FillABox(deteffibox, detectors, true, false);

            TVdet.Nodes.Clear();

            foreach (string det in detectors)
            {
                TreeNode tnode = new TreeNode(det);    //add a node for each detector
                TVdet.Nodes.Add(tnode);
            }

            detectors = null;
        }

        private void RefreshGeometriesTV()
        {
            string geoname = this.Linaa.Geometry.GeometryNameColumn.ColumnName;
            geoSolangbox.ComboBox.DataSource = this.Linaa.Geometry;
            geoSolangbox.ComboBox.DisplayMember = geoname;
            geoSolangbox.ComboBox.ValueMember = geoname;

            geoeffibox.ComboBox.DataSource = this.Linaa.Geometry;
            geoeffibox.ComboBox.DisplayMember = geoname;
            geoeffibox.ComboBox.ValueMember = geoname;

            TVgeo.Nodes.Clear();

            IEnumerable<LINAA.GeometryRow> geos = this.Linaa.Geometry.AsEnumerable().ToArray();
            foreach (LINAA.GeometryRow geo in geos)
            {
                //add a node for each geometry

                TreeNode tnode = new TreeNode();
                if (!geo.HasErrors && !geo.MatrixRow.HasErrors)
                {
                    tnode.Text = geo.GeometryName;
                    tnode.ToolTipText = "Matrix: " + geo.MatrixName.Trim() + "\nVial: " + geo.VialTypeRef.Trim();
                    tnode.ForeColor = System.Drawing.Color.Green;
                    tnode.Tag = geo;
                }
                else
                {
                    if (!geo.IsGeometryNameNull()) tnode.Text = geo.GeometryName;
                    else tnode.Text = geo.GeometryID.ToString();
                    tnode.ToolTipText = "Contains Errors";
                    tnode.ForeColor = System.Drawing.Color.Red;
                    tnode.Tag = null;
                }
                TVgeo.Nodes.Add(tnode);
            }
            geos = null;
        }

        private void TVCMS_Click(object sender, EventArgs e)
        {
            TreeView TV = this.TVCMS.SourceControl as TreeView;

            if (sender.Equals(this.RefreshTVs))
            {
                if (TV.Equals(TVgeo)) RefreshGeometriesTV();
                else if (TV.Equals(TVdet)) RefreshDetectorsTV();
                return;
            }
            else
            {
                bool isDetNode = TV.Equals(TVdet);

                bool sAll = false;
                bool inverse = false;
                if (sender.Equals(this.selectAllTV)) sAll = true;
                else if (sender.Equals(this.inverseSelectTV)) inverse = true;

                foreach (TreeNode node in TV.Nodes)
                {
                    if (node.Tag == null && !isDetNode) node.Checked = false; //has errores
                    else
                    {
                        if (inverse)
                        {
                            if (node.Checked) node.Checked = false;
                            else node.Checked = true;
                        }
                        else node.Checked = sAll;
                    }
                }
            }
        }

        #endregion TreeViews

        private void SOLCOIFiles_Click(object sender, EventArgs e)
        {
            IEnumerable<string> dirs = System.IO.Directory.EnumerateDirectories(solcoinPath);
            string deal = "deal.bat";
            foreach (string d in dirs)
            {
                if (System.IO.File.Exists(d + "\\" + deal)) continue;
                Dumb.Process(new Process(), d, deal, string.Empty, true, false, 0);
                System.IO.File.Delete(d + "\\" + deal);
            }

            Dumb.Process(new Process(), solcoinPath, "explorer.exe", solcoinPath, false, false, 1000);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)CMS.SourceControl;

            if (dgv.Equals(SolangDGV)) FillSolang_Click(sender, e);
            else if (dgv.Equals(effiDGV)) FillEffiCOIs_Click(sender, e);
        }

        private int finished = 0;

        private int maxLoad
        {
            get
            {
                int max = 3;
                try
                {
                    max = Convert.ToInt16(maxload.Text);
                }
                catch (SystemException ex)
                {
                    maxload.Text = max.ToString();
                }
                return max;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (finished >= maxLoad) return;

            Application.DoEvents();

            IEnumerable<System.Collections.DictionaryEntry> fins = null;
            fins = this.solcois.OfType<System.Collections.DictionaryEntry>().ToList();

            if (fins.Count() != 0 && cancel.Text.CompareTo(Canceltext) == 0)
            {
                foreach (System.Collections.DictionaryEntry sol in fins)
                {
                    SolCoin solcoi = sol.Value as SolCoin;

                    if (!solcoi.IsDisposed && !solcoi.Finished)
                    {
                        System.ComponentModel.BackgroundWorker worker = solcoi.Tag as System.ComponentModel.BackgroundWorker;
                        if (!worker.IsBusy)
                        {
                            worker.RunWorkerAsync(solcoi);
                            finished++;
                            if (solcois.Contains(sol.Key)) solcois.Remove(sol.Key);
                        }
                    }

                    if (finished >= maxLoad) break;
                }
            }
            else
            {
                if (cancel.Text.CompareTo(stopText) == 0)
                {
                    cancel.Visible = false;
                    progress.Value = 0;
                }
                CalculateCOIS.Checked = CalcCoiOrg;
                Timer.Enabled = false;
            }
            fins = null;
        }

        private static void GCollect()
        {
            long though = GC.GetTotalMemory(false);
            GC.AddMemoryPressure(though);
            GC.Collect(0, GCCollectionMode.Forced);
            Application.DoEvents();
        }

        private void progress_Click(object sender, EventArgs e)
        {
            GCollect();
        }

        private void energyeffibox_TextChanged(object sender, EventArgs e)
        {
            string filter = string.Empty;

            filter = "Isotope = '" + isoeffibox.Text + "'";

            if (energyeffibox.Text.CompareTo("0") != 0) filter += " AND Energy = '" + energyeffibox.Text + "'";

            this.effiBS.Filter = filter;
        }

        private void TVdet_AfterCheck(object sender, TreeViewEventArgs e)
        {
        }

        private void autoSel_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSel.Checked)
            {
                this.Reverse.Enabled = false;
                this.Reverse.Checked = false;
            }
            else this.Reverse.Enabled = true;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            cancel.Text = stopText;
            Application.DoEvents();
        }
    }
}