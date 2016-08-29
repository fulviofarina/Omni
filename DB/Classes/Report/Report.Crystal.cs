using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace DB
{
    public partial class LINAA : DB.Interfaces.IReport
    {
        protected void CrystalReport_FormClosing(object sender, FormClosingEventArgs e)
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

        private DB.Reports.FcReport fcreport;
        private DB.Reports.MeasReport measreport;
        private DB.Reports.ProjectReport projectreport;
        private DB.Reports.GenericReport genreport;

        public void LoadACrystalReport(String Title, ReporTypes type)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool ok = false;
            ReportDocument report = null;

            try
            {
                if (type == ReporTypes.FcReport)
                {
                    this.fcreport = new DB.Reports.FcReport();
                    report = (ReportDocument)fcreport;
                }
                else if (type == ReporTypes.MeasReport)
                {
                    this.measreport = new DB.Reports.MeasReport();
                    report = (ReportDocument)measreport;
                }
                else if (type == ReporTypes.ProjectReport)
                {
                    this.projectreport = new Reports.ProjectReport();
                    report = (ReportDocument)projectreport;
                }
                else if (type == ReporTypes.GenReport)
                {
                    this.genreport = new Reports.GenericReport();
                    report = (ReportDocument)genreport;
                }
                else throw new SystemException("LoadCrystalReport not Implemented");

                if (!report.IsLoaded) report.SetDataSource(this);

                cRV.ReportSource = report;

                ok = true;
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }

            if (!ok) Msg("Please send a Bug Report to Fulvio (right-click the notifier)", "Error when loading: " + Title, ok);
            else
            {
                Msg("Report is being loaded", "Loading... " + Title, ok);
                Form form = null;
                if (cRV.ParentForm == null)
                {
                    form = new Form();
                    form.MaximizeBox = true;
                    form.FormClosing += CrystalReport_FormClosing;
                    form.WindowState = FormWindowState.Maximized;
                    form.Text = Title;
                    form.Controls.Add(cRV);
                    form.Show();
                }
                report.Refresh();
                cRV.Show();
                Msg("Report was loaded", "Loaded... " + Title, ok);
            }

            Cursor.Current = Cursors.Default;
        }
    }
}