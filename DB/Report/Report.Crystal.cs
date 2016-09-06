using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using DB.Interfaces;
using DB.Reports;

namespace DB
{
    public partial class LINAA : IReport
    {
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
        private  ReportDocument report = null;


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
                //    this.fcreport = new DB.Reports.FcReport();
                    report = (ReportDocument)new DB.Reports.FcReport();
                }
                else if (type == ReporTypes.MeasReport)
                {
                 //   this.measreport = new DB.Reports.MeasReport();
                    report = (ReportDocument)new DB.Reports.MeasReport();
                }
                else if (type == ReporTypes.ProjectReport)
                {
                   // this.projectreport = new Reports.ProjectReport();
                    report = (ReportDocument)new Reports.ProjectReport();
                }
                else if (type == ReporTypes.GenReport)
                {
                  //  this.genreport = new Reports.GenericReport();
                    report = (ReportDocument)new Reports.GenericReport();
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

            if (!ok)
            {
                string sendToMe = "Please send a Bug Report to Fulvio (right-click the notifier)";

                Msg(sendToMe, "Error when loading: " + Title, ok);
            }

            else
            {
                Msg("Report is being loaded", "Loading... " + Title, ok);

                string loaded = "Loaded";

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
                    loaded = "Not Loaded";
                }

                Msg("Report was " + loaded, loaded + "... " + Title, ok);
            }

            Cursor.Current = Cursors.Default;
        }
    }
}