using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace DB.Reports
{
  public partial class CReport
  {
    private object set = null;

    public CReport(object db)
    {
      set = db;
    }

    private void crystalReport_FormClosing(object sender, FormClosingEventArgs e)
    {
      Form form = sender as Form;
      form.Controls.Remove(cRV);
    }

    public enum ReporTypes
    {
      FcReport = 1,
      MeasReport = 2,
      ProjectReport = 3,
      GenReport = 4
    };

    /*
    private FcReport fcreport;
    private MeasReport measreport;
    private ProjectReport projectreport;
    private GenericReport genreport;
    */
    private CrystalDecisions.Windows.Forms.CrystalReportViewer cRV;
    private ReportDocument report = null;

    /// <summary>
    /// Much better
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="type"></param>
    public void LoadACrystalReport(String Title, ReporTypes type)
    {
      Cursor.Current = Cursors.WaitCursor;

      bool ok = false;

      try
      {
        if (this.cRV == null)
        {
          this.cRV = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
          this.cRV.ActiveViewIndex = 0;
          this.cRV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.cRV.Dock = System.Windows.Forms.DockStyle.Fill;
          this.cRV.Location = new System.Drawing.Point(0, 0);
          this.cRV.Name = "cRV";
          this.cRV.Size = new System.Drawing.Size(150, 150);
          this.cRV.TabIndex = 0;
        }

        if (type == ReporTypes.FcReport)
        {
          report = (ReportDocument)new FcReport();
        }
        else if (type == ReporTypes.MeasReport)
        {
          report = (ReportDocument)new MeasReport();
        }
        else if (type == ReporTypes.ProjectReport)
        {
          report = (ReportDocument)new ProjectReport();
        }
        else if (type == ReporTypes.GenReport)
        {
          report = (ReportDocument)new GenericReport();
        }
        else throw new SystemException("CrystalReport Type not Implemented");

        if (!report.IsLoaded)
        {
          report.SetDataSource(set);
        }
        cRV.ReportSource = report;

        ok = true;
      }
      catch (SystemException ex)
      {
        MessageBox.Show(ex.Source, ex.Message, MessageBoxButtons.OK);

        //  this.AddException(ex);
      }

      if (!ok)
      {
        string sendToMe = "Please send a Bug Report to Fulvio (right-click the notifier)";

        MessageBox.Show(sendToMe, "Error when loading: " + Title, MessageBoxButtons.OK);

        // Msg(sendToMe, "Error when loading: " + Title, ok);
      }
      else
      {
        //  Msg("Report is being loaded", "Loading... " + Title, ok);

        string loaded = "Loaded ";

        if (cRV != null)
        {
          if (cRV.ParentForm == null)
          {
            Form form = null;
            form = new Form();
            form.MaximizeBox = true;
            form.FormClosing += crystalReport_FormClosing;
            form.WindowState = FormWindowState.Maximized;
            form.Text = Title;
            form.Controls.Add(cRV);
            form.Show();
          }
          report.Refresh();

          cRV.Show();
        }
        else
        {
          ok = false;
          loaded = "Not Loaded ";
          MessageBox.Show("Report was " + loaded, loaded + Title, MessageBoxButtons.OK);
        }
      }

      Cursor.Current = Cursors.Default;
    }
  }
}