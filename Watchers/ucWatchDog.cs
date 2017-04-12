using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DB;
using DB.Reports;
using Rsx;
using Rsx.CAM;
using Rsx.DGV;
using DB.Tools;

namespace k0X
{
    public interface IWatchDog
    {
        string Project { get; set; }

        void CleanTables();

        void Link(ref Interface lims, string Project);

        void Report();

        void Watch();
    }

    public partial class ucWatchDog : UserControl, IWatchDog
    {
        private Rsx.IWD adder = null;
        private string basicmeasFilter = string.Empty;
        private LINAA.ExceptionsDataTable exceptionDT;
        private CReport Irepo = null;
        private int? irrId = null;

        private Interface Interface;
        private IEnumerable<LINAA.MeasurementsRow> meas = null;

        private string MeasFilter = string.Empty;

        private string MeasSort = "MeasurementStart desc";

        private string project = string.Empty;

        private CamFileReader reader;

        private string SampleFilter = string.Empty;

        private IEnumerable<LINAA.SubSamplesRow> samples = null;

        private string SampleSort = "SubSampleName asc";

        private DateTime start;

        private LINAA.PreferencesRow currentPreference;

        public ucWatchDog()
        {
            InitializeComponent();

            this.Linaa.InitializeComponent();
            this.Linaa.PopulateColumnExpresions();

            Auxiliar form = new Auxiliar();
            form.Populate(this);
            form.Text = "WatchDog";
            form.Show();
        }

        public string Project
        {
            get { return project; }
            set { project = value; }
        }

        public static IList<string> GetMeasurements(string specpath, string project, string sample)
        {
            IEnumerable<string> measurements = Rsx.Dumb.GetFileNames(specpath + project, ".CNF");
            if (measurements.Count() != 0) measurements = measurements.Where(m => m.Contains(sample));
            if (measurements.Count() != 0) measurements = measurements.Select(o => o.ToUpper());
            return measurements.ToList();
        }

        public static IList<string> GetSamples(string specpath, string project)
        {
            IEnumerable<string> samples = Rsx.Dumb.GetFileNames(specpath + project, ".CNF");
            if (samples.Count() != 0)
            {
                samples = samples.Select(o => o.Substring(0, o.Length - 3).ToUpper());
            }
            if (samples.Count() != 0) samples = new HashSet<string>(samples);

            return samples.ToList();
        }

        public static void ShowXTable(ref DataView view, ref DataGridView dgv, string detcol, string[] filt, string poscol)
        {
            xTable.New(ref view, detcol, string.Empty, filt, ref dgv, 1);

            dgv.SelectAll();
            IEnumerable<DataGridViewRow> rowss = dgv.SelectedRows.OfType<DataGridViewRow>().ToList();
            char s = ','; //separator
            foreach (DataGridViewRow r in rowss)
            {
                IEnumerable<LINAA.MeasurementsRow> rows = ((IList<DataRow>)dgv[0, r.Index].Tag).OfType<LINAA.MeasurementsRow>();

                foreach (DataGridViewCell c in r.Cells)
                {
                    if (c.ColumnIndex < 1) continue;
                    if (c.Tag == null) continue;

                    string dethead = dgv.Columns[c.ColumnIndex].HeaderText;
                    IEnumerable<LINAA.MeasurementsRow> aux = rows.Where(a => a.Detector.CompareTo(dethead) == 0);
                    aux = aux.OrderByDescending(o => o.Position);
                    IList<short> pos = Rsx.Dumb.HashFrom<short>(aux, poscol);
                    string value = string.Empty;
                    foreach (short i in pos) value += i.ToString() + s.ToString();
                    if (value.Last().CompareTo(s) == 0) value = value.Substring(0, value.Length - 1);
                    c.Value = value;
                    c.ErrorText = string.Empty;
                }
            }
            dgv.ClearSelection();
        }

        public void CleanTables()
        {
            basicmeasFilter = "Measurement LIKE " + project + "*";
            SampleFilter = "IrradiationRequestsID = '" + irrId + "'";

            Rsx.Dumb.LinkBS(ref this.sampleBS, this.Linaa.SubSamples, SampleFilter, SampleSort);
            Rsx.Dumb.LinkBS(ref this.measBS, this.Linaa.Measurements, basicmeasFilter, MeasSort);

            measurementsDataGridView.Sort(this.startedCol, ListSortDirection.Descending);

            if (samples.Count() != 0)
            {
                DataTable sdt = samples.CopyToDataTable();
                this.Linaa.SubSamples.Clear();
                this.Linaa.SubSamples.Merge(sdt);
                sdt.Dispose();
                sdt = null;
            }
            if (meas.Count() != 0)
            {
                DataTable sdt = meas.CopyToDataTable();
                this.Linaa.Measurements.Clear();
                this.Linaa.Measurements.Merge(sdt);
                sdt.Dispose();
                sdt = null;
            }
            if (XtableDGV.Tag != null) xTable.Clean(ref XtableDGV);
        }

        public void Link(ref Interface lims, string Project)
        {
            this.ParentForm.Text = "WatchDog - " + project;
            Interface = lims;
            exceptionDT = lims.IDB.Exceptions;
            this.Linaa.QTA = lims.IAdapter.QTA;
            currentPreference = lims.IPreferences.CurrentPref;
            //          Interface.IPreferences.CurrentPref = lims.IPreferences.CurrentPref;
        //    Interface.IReport.Notify = lims.IReport.Notify;

            project = Project.ToUpper();
            irrId = lims.IDB.IrradiationRequests.FindIrrReqID(project);
            samples = lims.IDB.SubSamples.FindByIrReqID(irrId).ToList();
            meas = samples.SelectMany(s => s.GetMeasurementsRows()).ToList();

            Dirbox.Text = lims.IPreferences.CurrentPref.Spectra + project;

            CleanTables();
        }

        public void Report()
        {
            if (Irepo == null) Irepo = new CReport(this.Linaa as DataSet);
            Irepo.LoadACrystalReport("Measurements Report - " + project, CReport.ReporTypes.MeasReport);
        }

        public void Watch()
        {
            if (this.List.Text.Contains("Watch"))
            {
                if (!System.IO.Directory.Exists(Dirbox.Text))
                {
                    MessageBox.Show("That directory does not exist");
                    return;
                }

                this.List.ImageKey = "red";
                this.List.Text = "Pause...";
                Dirbox.Enabled = false;

                CleanTables();

                string rootpath = Dirbox.Text;
                watcher.Path = rootpath; //start watching!
                watcher.EnableRaisingEvents = true;

                IList<System.IO.FileInfo> files = Rsx.Dumb.GetFiles(rootpath);
                files = files.Where(o => o.Extension.ToUpper().CompareTo(".CNF") == 0).ToList();
                if (files.Count() != 0) AddMeasurements(ref files);
                else Interface.IReport.Msg("File list is empty", "No measurement files were found for\n" + rootpath, false);
            }
            else
            {
                watcher.EnableRaisingEvents = false;
                Dirbox.Enabled = true;
                if (adder != null)
                {
                    adder.CancelAsync = true;
                }
                this.List.ImageKey = "green";
                this.List.Text = "Watch";
            }
        }

        public void xTableReport()
        {
            if (Irepo == null) Irepo = new CReport(this.Linaa as DataSet);

            Irepo.LoadACrystalReport("Measurements XTable Report - " + project, CReport.ReporTypes.GenReport);
        }

        /// <summary>
        /// for post processing, basically speaking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMeasurement(ref object element, ref object toUseOnElement)
        {
            System.IO.FileInfo fin = element as System.IO.FileInfo;
            CamFileReader Areader = toUseOnElement as CamFileReader;

            string measurement = fin.Name.ToUpper().Replace(".CNF", null);
            string full = fin.FullName.ToUpper();

            Func<LINAA.MeasurementsRow, bool> measfinder = x =>
            {
                if (x.Measurement.ToUpper().CompareTo(measurement) == 0) return true;
                else return false;
            };

            LINAA.MeasurementsRow m = this.Linaa.Measurements.FirstOrDefault<LINAA.MeasurementsRow>(measfinder);
            if (m == null)
            {
                bool camRead = false;
                try
                {
                    m = this.Linaa.Measurements.NewMeasurementsRow();
                    this.Linaa.Measurements.AddMeasurementsRow(m);
                    m.Measurement = measurement;
                    m.Sample = m.Measurement.Substring(0, m.Measurement.Length - 3);
                    m.Detector = m.Measurement.Replace(m.Sample, null);
                    m.Position = Convert.ToInt16(m.Detector.Substring(1, 1));
                    m.MeasurementNr = m.Detector.Substring(2, 1);
                    m.Detector = m.Detector.Substring(0, 1).ToUpper();

                    if (Areader.Exception == null) camRead = Areader.GetData(full);
                }
                catch (Exception ex)
                {
                    m.RowError = ex.Message;
                    exceptionDT.AddExceptionsRow(ex);
                }

                try
                {
                    if (camRead)  //read genie data
                    {
                        m.MeasurementStart = Convert.ToDateTime(Areader.ASTIME);
                        m.CountTime = Areader.EREAL / 60;
                        m.LiveTime = Areader.ELIVE / 60;

                        if (m.DT > 40) m.SetColumnError("DT", "DeadTime is higher than 40%");
                        if (m.Detector.Length > 1) m.Detector = m.Detector[0].ToString();
                        string detector = Areader.DetectorName.ToUpper()[0].ToString();
                        if (!detector.Equals(m.Detector))
                        {
                            m.SetColumnError("Detector", "The detector code name in the spectrum filename does not match the detector used to acquire the spectrum");
                        }
                        m.Detector = detector;
                    }
                    else m.RowError = "Could not read CAM file";
                }
                catch (Exception ex)
                {
                    m.RowError = ex.Message;
                    exceptionDT.AddExceptionsRow(ex);
                }
            }

            try
            {
                if (m.Detector.Length > 1) m.Detector = m.Detector[0].ToString();
                m.CTSpan = new TimeSpan(0, 0, Convert.ToInt32(m.CountTime * 60));
                string ptf = full.Replace(".CNF", ".PTF");

                if (System.IO.File.Exists(ptf))
                {
                    m.HL = true;
                }
                else m.HL = false;

                string order = string.Empty;
                string des = string.Empty;
                string kaywin = full.Replace(".CNF", ".B01");
                if (System.IO.File.Exists(kaywin))
                {
                    byte[] array = System.IO.File.ReadAllBytes(kaywin);
                    string aux = Encoding.UTF8.GetString(array);
                    int index = aux.IndexOf('\0');
                    aux = aux.Substring(0, index);

                    string[] orderDesKW = aux.Split('/');

                    string first = orderDesKW.First();
                    string last = orderDesKW.Last();

                    order = first.Substring(1);
                    int ind = last.IndexOf(':');
                    m.KW = last.Substring(ind + 1, 2);

                    foreach (string s in orderDesKW)
                    {
                        if (s.CompareTo(first) == 0 || s.CompareTo(last) == 0) continue;
                        else des += s + " ";
                    }
                }
                else m.KW = string.Empty;

                if (!Rsx.EC.IsNuDelDetch(m.SubSamplesRow))
                {
                    m.SubSamplesRow.Detector = m.Detector;
                    m.SubSamplesRow.Position = m.Position;
                    m.SubSamplesRow.SubSampleDescription = des;
                    m.SubSamplesRow.Order = order;

                    if (!m.SubSamplesRow.IsOutReactorNull()) m.DecayTime = (m.MeasurementStart - m.SubSamplesRow.OutReactor).TotalMinutes;
                }

                if (!m.HL)
                {
                    LINAA.MeasurementsRow old = meas.FirstOrDefault(d => d.Measurement.CompareTo(m.Measurement) == 0);
                    if (!Rsx.EC.IsNuDelDetch(old)) m.HL = true;
                    else
                    {
                        int? measID = (int?)this.Linaa.QTA.GetMeasurementID(m.Measurement);
                        if (measID != null)
                        {
                            int? peakID = (int?)this.Linaa.QTA.IsDeconvoluted((int)measID);
                            if (peakID != null) m.HL = true;
                        }
                        else m.HL = false;
                    }
                }
            }
            catch (Exception ex)
            {
                m.RowError = ex.Message;
                exceptionDT.AddExceptionsRow(ex);
            }
        }

        private void AddMeasurements<T>(ref T files)
        {
            start = DateTime.Now;

            MeasSort = this.measBS.Sort;
            SampleSort = this.sampleBS.Sort;
            MeasFilter = this.measBS.Filter;

            Rsx.Dumb.DeLinkBS(ref this.sampleBS);
            Rsx.Dumb.DeLinkBS(ref this.measBS);

            Type tipo = typeof(T);
            IList<System.IO.FileInfo> ls = new List<System.IO.FileInfo>();
            if (tipo.Equals(typeof(IList<System.IO.FileInfo>)))
            {
                ls = ls.Union(files as IList<System.IO.FileInfo>).ToList();
            }
            else ls.Add(files as System.IO.FileInfo);
            int count = ls.Count();

            this.progress.Step = 1;
            this.progress.Value = 0;
            this.progress.Maximum = count;

            ///Create worker
            if (adder == null)
            {
                adder = new Rsx.WD();
                adder.CallBackMethod = CallBack;
                adder.WorkMethod = AddMeasurement;
                adder.ReportMethod = progress.PerformStep;
            }
            if (reader == null) reader = new CamFileReader();

            object[] args = new object[] { ls, reader, true };
            adder.Arguments = args;
            adder.Async();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            FBD.ShowDialog();

            Dirbox.Text = FBD.SelectedPath;

            FindMeasurements(sender, e);
        }

        private void CallBack()
        {
            Rsx.Dumb.LinkBS(ref this.sampleBS, this.Linaa.SubSamples, SampleFilter, SampleSort);
            Rsx.Dumb.LinkBS(ref this.measBS, this.Linaa.Measurements, MeasFilter, MeasSort);

            measurementsDataGridView.Sort(this.startedCol, ListSortDirection.Descending);

            if (TAB.SelectedTab == this.xTableTab) MakeXTable();

            this.Interface.IReport.Msg("Loaded in " + Decimal.Round(Convert.ToDecimal((DateTime.Now - start).TotalSeconds), 1) + " seconds", "Loaded measurements for " + this.project, true);
        }

        private void FilterMode_Click(object sender, EventArgs e)
        {
            if (FilterMode.Text.CompareTo("All Visible") == 0)
            {
                FilterMode.Text = "Filter Mode";
                FilterMode.ForeColor = Color.Red;
            }
            else
            {
                FilterMode.Text = "All Visible";
                FilterMode.ForeColor = Color.Black;
                MeasFilter = basicmeasFilter;
                this.measBS.Filter = MeasFilter;
            }
        }

        private void FindMeasurements(object sender, EventArgs e)
        {
            Watch();
        }

        private void MakeXTable()
        {
            DataView view = (this.measBS.List as DataView);
            DataGridView dgv = this.XtableDGV;
            string detcol = this.Linaa.Measurements.DetectorColumn.ColumnName;
            string[] filt = new string[] { this.Linaa.Measurements.SampleColumn.ColumnName };
            string poscol = this.Linaa.Measurements.PositionColumn.ColumnName;

            ShowXTable(ref view, ref dgv, detcol, filt, poscol);
        }

        private void measurementsDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow r = measurementsDataGridView.Rows[e.RowIndex];
            LINAA.MeasurementsRow m = Rsx.Dumb.Cast<LINAA.MeasurementsRow>(r);

            if (m.IsAcquiring) r.DefaultCellStyle.BackColor = System.Drawing.Color.PapayaWhip;
            else
            {
                if (m.HasErrors) r.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
                else r.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            }
        }

        private void ReportMeasurements(object sender, EventArgs e)
        {
            if (this.TAB.SelectedTab == this.xTableTab) xTableReport();
            else Report();
        }

        private void sampleDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow r = sampleDGV.Rows[e.RowIndex];
            DataGridViewCellStyle style = r.DefaultCellStyle;
            LINAA.SubSamplesRow s = Rsx.Dumb.Cast<LINAA.SubSamplesRow>(r);
            IEnumerable<LINAA.MeasurementsRow> meas = s.GetMeasurementsRows();
            if (meas.Count(o => o.IsAcquiring) != 0) style.BackColor = System.Drawing.Color.PapayaWhip;
            else
            {
                if (EC.HasErrors(meas)) style.BackColor = System.Drawing.Color.MistyRose;
                else style.BackColor = System.Drawing.Color.White;
            }
        }

        private void sampleDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (FilterMode.Text.CompareTo("Filter Mode") != 0) return;

            IEnumerable<DataRowView> rows = Rsx.Dumb.Cast<DataRowView>(this.sampleDGV.SelectedRows.OfType<DataGridViewRow>());

            if (rows.Count() == 0)
            {
                IEnumerable<DataGridViewCell> cells = this.sampleDGV.SelectedCells.OfType<DataGridViewCell>();
                if (cells.Count() == 1)
                {
                    DataGridViewRow r = this.sampleDGV.Rows[cells.First().RowIndex];
                    LINAA.SubSamplesRow s = (r.DataBoundItem as DataRowView).Row as LINAA.SubSamplesRow;
                    string field = sampleDGV.Columns[cells.First().ColumnIndex].DataPropertyName;
                    string valueToFilter = string.Empty;
                    if (field.CompareTo(this.Linaa.SubSamples.SubSampleNameColumn.ColumnName) == 0)
                    {
                        if (s.IsNull(field)) return;
                        valueToFilter = s.Field<object>(field).ToString();
                        field = "Sample";
                    }
                    else if (field.CompareTo("Position") == 0 || field.CompareTo("Detector") == 0)
                    {
                        if (s.IsNull(field)) return;
                        valueToFilter = s.Field<object>(field).ToString();
                    }
                    else return;
                    MeasFilter = basicmeasFilter + " AND " + field + " = '" + valueToFilter + "'";
                    MeasSort = "MeasurementStart desc";
                }
            }
            else
            {
                string field = this.Linaa.Measurements.SampleColumn.ColumnName;
                IList<string> samples = Rsx.Dumb.HashFrom<string>(rows, this.Linaa.SubSamples.SubSampleNameColumn.ColumnName);
                int x = samples.Count;
                MeasFilter = basicmeasFilter;
                if (x > 0) MeasFilter += " AND (";
                foreach (string samp in samples)
                {
                    MeasFilter += field + " = '" + samp + "'";
                    x--;
                    if (x != 0) MeasFilter += " OR ";
                }
                if (x > 0) MeasFilter += ")";
                MeasSort = "Sample, MeasurementStart desc";
            }

            this.measBS.Filter = MeasFilter;
            this.measBS.Sort = MeasSort;
        }

        private void TAB_Selected(object sender, TabControlEventArgs e)
        {
            if (this.TAB.SelectedTab == this.xTableTab)
            {
                MakeXTable();
            }
        }

        private void watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            string d = string.Empty;
        }

        private void watcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                string fullpath = e.FullPath;

                string filename = e.Name.Replace(".CNF", null);
                char measNr = filename.LastOrDefault();

                bool isDig = char.IsDigit(measNr);
                //is a digit? find out what is the measNr then...
                if (isDig)
                {
                    char det = filename[filename.Length - 2];
                    char pos = filename[filename.Length - 1];
                    string sample = filename.Substring(0, filename.Length - 2);

                    IList<string> measurements = GetMeasurements(currentPreference.Spectra, project, sample);
                    measurements = measurements.Where(o => o.CompareTo(filename) != 0).ToList();

                    string newName = Rsx.Dumb.GetNextName(sample + det + pos, measurements, true);
                    newName += ".CNF";
                    string path = e.FullPath.Replace(e.Name, null);

                    System.IO.File.Copy(fullpath, path + newName);
                    Application.DoEvents();
                    System.IO.File.Delete(fullpath);
                    return;
                }

                System.IO.FileInfo finfo = new System.IO.FileInfo(fullpath);

                if (finfo.Exists)
                {
                    this.AddMeasurements<System.IO.FileInfo>(ref finfo);
                }
            }
            catch (SystemException ex)
            {
                exceptionDT.AddExceptionsRow(ex);
            }
        }

        private void watcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
        }

        private void watcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            if (e.OldName.ToUpper().Contains(".TMP"))
            {
                if (e.Name.ToUpper().Contains(".CNF"))
                {
                    watcher_Created(sender, e);
                }
            }
        }
    }
}