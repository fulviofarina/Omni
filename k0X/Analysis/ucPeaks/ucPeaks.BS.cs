using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx.Dumb; using Rsx;
using Rsx.DGV;

namespace k0X
{
  public interface IPeaks
  {
    void DeLink();

    void Link();

    DB.LINAA.SubSamplesRow Sample { get; set; }
    System.Collections.Generic.IEnumerable<global::DB.LINAA.SubSamplesRow> Samples { get; set; }

    void Save();

    void CleanSR();

    string CurrentFilter { get; set; }

    ucSamples FindUserControl(string project);

    ICollection<string> SetProject(string project);

    void SetSample(string sample);

    void SetElement(string sym);

    IList<string> FindElements();

    void LoadData();

    IPeriodicTable IPeriodicTable { get; set; }
  }

  public partial class ucPeaks : k0X.IPeaks
  {
    private IPeriodicTable iPeriodicTable;

    public IPeriodicTable IPeriodicTable
    {
      set
      {
        ucPeriodicTable ip = (ucPeriodicTable)value;
        this.components.Add(ip);
        this.SplitC.Panel2.Controls.Clear();
        ip.Dock = DockStyle.Fill;
        this.SplitC.Panel2.Controls.Add(ip);
        ip.IPeaks = this;
        iPeriodicTable = ip;
      }
      get
      {
        return iPeriodicTable;
      }
    }

    public IEnumerable<LINAA.SubSamplesRow> Samples
    {
      get { return samples; }
      set { samples = value; }
    }

    public LINAA.SubSamplesRow Sample
    {
      get { return sample; }
      set { sample = value; }
    }

    public IList<string> FindElements()
    {
      IList<string> eles = null;
      IEnumerable<DataRow> rows = null;
      string field = this.Linaa.IRequestsAverages.ElementColumn.ColumnName;
      if (EC.IsNuDelDetch(Sample))
      {
        rows = this.Samples.SelectMany(o => o.GetIRequestsAveragesRows());
        if (rows == null || rows.Count() == 0)
        {
          IEnumerable<DataRowView> view = Caster.Cast<DataRowView>(this.bs.List as DataView);
          rows = Caster.Cast<DataRow>(view);
          field = "Sym";
        }
      }
      else
      {
        rows = this.Sample.GetIRequestsAveragesRows();
      }
      eles = Hash.HashFrom<string>(rows, field);
      return eles;
    }

    public void CleanSR()
    {
      xTable.Clean(ref this.SRDGV);
    }

    public ucSamples FindUserControl(string project)
    {
      ucSamples s = null;
      Func<ucSamples, bool> Comparer = o =>
      {
        return (o.Name.CompareTo(project) == 0);
      };
      if (openControls.Count() != 0) s = openControls.FirstOrDefault(Comparer);
      return s;
    }

    public void SetSample(string sample)
    {
      this.Sample = null;
      if (sample.CompareTo("*") != 0)
      {
        sampleFilter = " AND Sample = '" + sample + "'";
        if (this.Samples != null && this.Samples.Count() != 0)
        {
          this.Sample = this.Samples.FirstOrDefault(s => s.SubSampleName.CompareTo(sample) == 0);
        }
        else this.Sample = this.Linaa.SubSamples.FirstOrDefault(s => s.SubSampleName.CompareTo(sample) == 0);
        this.Samples = null;
      }
      else sampleFilter = " AND Sample <> '*'";
    }

    public void SetElement(string sym)
    {
      elementFilter = string.Empty;
      if (sym.CompareTo("*") != 0) elementFilter = " AND (Sym = '" + sym + "')";
    }

    public ICollection<string> SetProject(string project)
    {
      this.sample = null;
      this.samples = null;
      if (project.CompareTo("*") != 0)
      {
        projectFilter = "Project = '" + project + "'";
        this.samples = this.Linaa.FindByProject(project);
      }
      else
      {
        this.samples = this.Linaa.SubSamples.AsEnumerable();
        projectFilter = "Project <> '*'";
      }

      return Hash.HashFrom<string>(this.Samples, "SubSampleName");
    }

    protected internal int[] isoPeak;
    internal System.Windows.Forms.DataGridView CurrentDGV = null;
    private LINAA.SubSamplesRow sample = null;
    private IEnumerable<LINAA.SubSamplesRow> samples = null;
    private string currentFilter;
    private string elementFilter = string.Empty;
    private string sampleFilter = string.Empty;
    private string projectFilter = string.Empty;

    public string CurrentFilter
    {
      get
      {
        currentFilter = projectFilter + sampleFilter + elementFilter;
        return currentFilter;
      }
      set { currentFilter = value; }
    }

    public void LoadData()
    {
      this.bs.Filter = this.CurrentFilter + " AND Selected = 'TRUE'";
      this.bs.Sort = "T0 desc";

      if (this.bs.Count != 0)
      {
        Field_Click(this.Yi, EventArgs.Empty);
        Field_Click(this.Xij, EventArgs.Empty);
        LinkOtherBS(this.CurrentFilter, false);
        //in this order please!! KEEEP
      }

      if (ParentForm != null)
      {
        if (!this.ParentForm.Visible) this.ParentForm.Visible = true;
        this.ParentForm.Text = "Selector - " + this.CurrentFilter;
      }
      if (MainTab.SelectedTab.Equals(this.ArenaTab))
      {
        this.FindVs_Click(null, EventArgs.Empty);
      }
    }

    public void DeLink()
    {
      if (this.IsDisposed) return;
      DGVLayouts(true);

      try
      {
        isoPeak = new int[] { this.AvgIsotopesBS.Position, this.AvgPeakBS.Position };
      }
      catch (SystemException ex)
      {
      }
      this.Enabled = false;
      DeLinkBS(true);
      DeLinkOtherBS(true);
    }

    public void Link()
    {
      if (this.IsDisposed) return;

      LinkBS(this.CurrentFilter, true);
      this.LinkOtherBS(this.CurrentFilter, true);
      try
      {
        this.AvgIsotopesBS.Position = isoPeak[0];
        this.AvgPeakBS.Position = isoPeak[1];
      }
      catch (SystemException ex)
      {
      }

      this.Enabled = true;
      DGVLayouts(false);
    }

    internal void LinkBS(String Filter, bool attach)
    {
      if (attach || this.bs.DataSource == null || this.bs.DataMember == null)
      {
        BS.LinkBS(ref this.bs, this.Linaa.Peaks);
      }
      if (Filter.Equals(string.Empty))
      {
        this.bs.Filter = "Sym is NULL";
      }
      else this.bs.Filter = Filter + " AND Selected = 'TRUE'";
      this.bs.Sort = this.Zi.Text + " asc, " + this.Yi.Text + " asc, " + this.Linaa.Peaks.T0Column.ColumnName + " asc";

      this.Linaa.Peaks.EndLoadData();
      this.bs.ResumeBinding();
    }

    internal void DeLinkBS(bool dettach)
    {
      this.bs.SuspendBinding();
      this.Linaa.Peaks.BeginLoadData();

      if (dettach) BS.DeLinkBS(ref this.bs);
    }

    internal void LinkOtherBS(String Filter, bool attach)
    {
      if (attach || AvgIsotopesBS.DataSource == null || AvgIsotopesBS.DataMember == null)
      {
        BS.LinkBS(ref AvgElementBS, this.Linaa.IRequestsAverages);
        BS.LinkBS(ref AvgIsotopesBS, this.Linaa.IRequestsAverages);

        if (this.Gtbox.TextBox.DataBindings.Count == 0)
        {
          this.Gtbox.TextBox.DataBindings.Add(new Binding("Text", this.AvgIsotopesBS, "Gt", true));
        }
        if (this.Gebox.TextBox.DataBindings.Count == 0)
        {
          this.Gebox.TextBox.DataBindings.Add(new Binding("Text", this.AvgIsotopesBS, "Ge", true));
        }
        if (this.ppmbox.TextBox.DataBindings.Count == 0)
        {
          this.ppmbox.TextBox.DataBindings.Add(new Binding("Text", this.AvgIsotopesBS, "ppm", true));
        }
        if (this.Fcbox.TextBox.DataBindings.Count == 0)
        {
          this.Fcbox.TextBox.DataBindings.Add(new Binding("Text", this.AvgIsotopesBS, "Fc", true));
        }
        if (this.CurrentIsotope.TextBox.DataBindings.Count == 0)
        {
          this.CurrentIsotope.TextBox.DataBindings.Add(new Binding("Text", this.AvgIsotopesBS, "Radioisotope", true));
        }
        if (this.CurrentSample.TextBox.DataBindings.Count == 0)
        {
          this.CurrentSample.TextBox.DataBindings.Add(new Binding("Text", this.AvgIsotopesBS, "Sample", true));
        }
      }

      if (attach || AvgPeakBS.DataSource == null)
      {
        BS.LinkBS(ref AvgPeakBS, this.Linaa.IPeakAverages);
      }

      if (Filter.Contains("Sym"))
      {
        Filter = Filter.Replace("Sym", "Element");
        AvgIsotopesBS.Filter = Filter;
        AvgPeakBS.Filter = Filter;
      }
      else
      {
        AvgIsotopesBS.Filter = "Element is NULL";
        AvgPeakBS.Filter = "Element is NULL";
      }
      AvgElementBS.Filter = string.Empty;

      AvgIsotopesBS.Sort = " Radioisotope asc";

      AvgElementBS.Sort = "  Radioisotope asc";

      AvgPeakBS.Sort = "Radioisotope asc, Energy asc";

      this.Linaa.IPeakAverages.EndLoadData();
      this.Linaa.IRequestsAverages.EndLoadData();

      AvgIsotopesBS.ResumeBinding();
      AvgElementBS.ResumeBinding();
      AvgPeakBS.ResumeBinding();
    }

    internal void DeLinkOtherBS(bool detach)
    {
      AvgIsotopesBS.SuspendBinding();
      AvgElementBS.SuspendBinding();
      AvgPeakBS.SuspendBinding();

      this.Linaa.IPeakAverages.BeginLoadData();
      this.Linaa.IRequestsAverages.BeginLoadData();

      if (detach)
      {
        this.ppmbox.TextBox.DataBindings.Clear();
        this.Fcbox.TextBox.DataBindings.Clear();
        this.CurrentIsotope.TextBox.DataBindings.Clear();
        this.CurrentSample.TextBox.DataBindings.Clear();
        this.Gtbox.TextBox.DataBindings.Clear();
        this.Gebox.TextBox.DataBindings.Clear();

        BS.DeLinkBS(ref this.AvgIsotopesBS);
        BS.DeLinkBS(ref this.AvgPeakBS);
        BS.DeLinkBS(ref this.AvgElementBS);
      }
    }

    /// <summary>
    /// This Creates, Refreshes, Formats and Paints the given DataGridView for the given Project Table  according toRow the xTable Class
    /// </summary>
    /// <param name="Project">Table with fromRow Project data</param>
    /// <param name="DGV">DataGridView toRow convert toRow a xTable</param>
    internal void MakeSelectReject(ref DataView view, ref DataGridView DGV)
    {
      try
      {
        if (view != null && view.Count != 0 && DGV != null && ShowxTable.Checked)
        {
          DGV.SuspendLayout();
          xTable.New(ref view, this.Xj.Text, this.Linaa.Peaks.T0Column.ColumnName, new String[] { this.Zi.Text, this.Yi.Text }, ref DGV, 50);

          #region painting

          string[] comments = new string[] { "Measurement Start: ", "Decay Time: ", "Count Time: " };
          string start = this.Linaa.Measurements.MeasurementStartColumn.ColumnName;
          string decay = this.Linaa.Measurements.DecayTimeColumn.ColumnName;
          string count = this.Linaa.Measurements.CountTimeColumn.ColumnName;
          string[] parentfields = new string[] { start, decay, count };
          xTable.Tip.ColumnHeaderWithCommonRowDataFrom(ref this.CurrentDGV, 2, comments, "Measurements_Peaks", parentfields);

          string codek0 = this.Linaa.IPeakAverages.Codek0Column.ColumnName;
          xTable.Paint.ByCodes(ref this.CurrentDGV, codek0, "IPeakAverages_Peaks", 1, 2, 2);

          #endregion painting

          DGV.ResumeLayout();
        }
      }
      catch (SystemException ex)
      {
        this.Linaa.AddException(ex);
        MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    internal void RefreshSelectReject(DataGridView DGV)
    {
      try
      {
        if (!this.Xij.Text.Equals(string.Empty) && DGV != null && DGV.Tag != null && ShowxTable.Checked)
        {
          DGV.SuspendLayout();
          if (this.XijTip.Text.Equals(string.Empty)) this.XijTip.Text = "AreaUncertainty";
          if (this.TipDigits.Text.Equals(string.Empty)) TipDigits.Text = "1";
          if (this.TipDigits.Text.Equals(string.Empty)) DigitsBox.Text = "2";
          xTable.Fill_Xij(ref DGV, 2, this.Xij.Text, this.XijTip.Text, Convert.ToInt16(TipDigits.Text));
          foreach (DataGridViewColumn col in DGV.Columns)
          {
            if (col.Index > 1) col.DefaultCellStyle.Format = "N" + DigitsBox.Text;
          }
          // xTable.CleanCells(DGV, 2);
          xTable.Paint.BySwitch(ref DGV, this.Linaa.Peaks.IDColumn.ColumnName, 1, Color.Red, Color.Black, 2);

          DGV.ResumeLayout();
        }
      }
      catch (SystemException ex)
      {
        this.Linaa.AddException(ex);
        MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    internal void ReCalculate()
    {
      DeLink();

      if (this.Xij.Text.Equals(string.Empty)) return;
      Type tipo = this.Linaa.Peaks.Columns[Xij.Text].DataType;
      if (!tipo.Equals(typeof(double))) return;

      String quantity = Xij.Text.Trim();
      if (!this.Linaa.IPeakAverages.Columns.Contains(quantity))
      {
        string newColumnExpression = "Avg(Child(IPeakAverages_Peaks)." + quantity + ")";
        this.Linaa.IPeakAverages.Columns.Add(quantity, this.Linaa.Peaks.Columns[quantity].DataType, newColumnExpression);
      }
      if (!this.Linaa.IRequestsAverages.Columns.Contains(quantity))
      {
        string newColumnExpression = "Avg(Child(IRequestsAverages_Peaks)." + quantity + ")";
        this.Linaa.IRequestsAverages.Columns.Add(quantity, this.Linaa.Peaks.Columns[quantity].DataType, newColumnExpression);
      }

      bool calculate = ReEffi.Checked || ReDecay.Checked || matSSF.Checked || chilean.Checked;

      IEnumerable<LINAA.SubSamplesRow> samples = this.Samples;
      if (this.Sample != null)
      {
        List<LINAA.SubSamplesRow> list = new List<LINAA.SubSamplesRow>();
        list.Add(this.Sample);
        samples = list;
      }

      if (ReEffi.Checked)
      {
        LINAA.GeometryRow reference = this.Linaa.DefaultGeometry;
        DB.Tools.WC.SetCOINSolid(this.Sample, true, true, ref reference);
        DB.Tools.WC.SetCOINSolid(this.Sample, false, true, ref reference);
      }
      else if (calculate)
      {
        DB.Tools.WC.Calculate(ref samples, matSSF.Checked, chilean.Checked, ReDecay.Checked);
      }
      DB.Tools.WC.FindSDs(ref samples);

      Link();
    }

    internal void interfWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
      if (this.NISRDGV.Tag != null)
      {
        DataView noninter = (this.NISRDGV.Tag as DataView);
        this.NISRDGV.DataSource = noninter;
      }
      if (this.ISRDGV.Tag != null)
      {
        DataView inter = (this.ISRDGV.Tag as DataView);
        this.ISRDGV.DataSource = inter;
      }
    }

    internal void interfWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      string energyColName = this.Linaa.IPeakAverages.EnergyColumn.ColumnName;

      if (EC.IsNuDelDetch(this.Sample)) return;

      IEnumerable<LINAA.PeaksRow> Interpeaks = this.Sample.GetPeaksRows().ToList();

      IList<LINAA.PeaksRow> intercummu = new List<LINAA.PeaksRow>();
      IList<LINAA.PeaksRow> nonintercummu = new List<LINAA.PeaksRow>();
      foreach (DataGridViewRow r in this.AvgPeakDGV.Rows)
      {
        LINAA.IPeakAveragesRow peak = Caster.Cast<LINAA.IPeakAveragesRow>(r);
        double highEnergy = (peak.Energy + 0.7);
        double lowEnergy = (peak.Energy - 0.7);
        IEnumerable<LINAA.PeaksRow> Inter = Interpeaks.Where(o => o.Sample.Equals(peak.Sample) && !o.Iso.Equals(peak.Radioisotope) && o.Energy >= lowEnergy && o.Energy <= highEnergy).ToList();

        IEnumerable<LINAA.PeaksRow> NonInter = Interpeaks.Where(o => o.Sample.Equals(peak.Sample) && o.Iso.Equals(peak.Radioisotope) && o.Energy > highEnergy && o.Energy < lowEnergy).ToList();

        intercummu = intercummu.Union(Inter).ToList();
        nonintercummu = nonintercummu.Union(NonInter).ToList();

        if (Inter.Count() != 0 && NonInter.Count() != 0) peak.SetColumnError(energyColName, "Interfered");

        if (Inter.Count() != 0 && NonInter.Count() == 0) peak.SetColumnError(energyColName, "Interfered, but cannot be corrected");
      }

      if (intercummu.Count() == 0) return;
      else this.ISRDGV.Tag = intercummu.CopyToDataTable().AsDataView();

      if (nonintercummu.Count() != 0) this.NISRDGV.Tag = nonintercummu.CopyToDataTable().AsDataView();

      xTable.Paint.CellWithErrorColumn(ref AvgPeakDGV, "Energy", "Interfered", Color.PaleGreen, 0);
      xTable.Paint.CellWithErrorColumn(ref AvgPeakDGV, "Energy", "Corrected", Color.PapayaWhip, 0);
    }

    /// <summary>
    /// Determines and Computes the Interference Gamma Correction Factors for the given peak
    /// </summary>
    /// <param name="Iso">Isotope giving rise toRow interfered peak</param>
    /// <param name="Energy">Energy being interfered</param>
    /// <param name="ProjectName">Project giving rise toRow interfered peak/Isotope</param>
    /// <param name="sample">Sample giving rise toRow interfered peak/Isotope</param>
    internal void FindGammaCorrections(string Iso, double Energy, string sample)
    {
      string highEnergy = (Energy + 0.7).ToString();
      string lowEnergy = (Energy - 0.7).ToString();
      DataView viewInter = this.Linaa.Peaks.AsDataView();
      viewInter.RowFilter = "Sample = '" + sample + "' AND Iso <> '" + Iso + "' AND Energy >= '" + lowEnergy + "' AND Energy <= '" + highEnergy + "'";
      MakeSelectReject(ref viewInter, ref ISRDGV);

      DataView viewNonInter = this.Linaa.Peaks.AsDataView();
      viewNonInter.RowFilter = "Sample = '" + sample + "' AND Iso = '" + Iso + "' AND Energy > '" + highEnergy + "' AND Energy < '" + lowEnergy + "'";
      MakeSelectReject(ref viewNonInter, ref NISRDGV);
    }
  }
}