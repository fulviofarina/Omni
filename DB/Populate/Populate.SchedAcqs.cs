using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DB
{
    public partial class LINAA : ISchedAcqs
    {
        public void AddSchedule(string project, string sample, Int16 pos, string det, Int16 repeats, double preset, DateTime startOn, string useremail, bool cummu, bool Force)
        {
            DB.LINAA.SchAcqsRow sch = this.SchAcqs.FindASpecificSchedule(det, project, sample);
            DialogResult result;
            string Content = string.Empty;
            if (sch == null)
            {
                sch = this.SchAcqs.NewSchAcqsRow();
                this.SchAcqs.AddSchAcqsRow(sch);
            }
            else
            {
                Content = sch.GetReportString();
                if (Force) result = DialogResult.No;
                else result = MessageBox.Show("Sample " + sample + " was found in the schedule:\n\n" + Content + "\n\nDo you want to replace it with the new data?\nIf not, I will create a new one!", "Found a previous scheduled measurement", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    sch = this.SchAcqs.NewSchAcqsRow();
                    this.SchAcqs.AddSchAcqsRow(sch);
                }
                else if (result == DialogResult.Cancel) return;
            }

            sch.SetSchedule(project, sample, pos, det, repeats, preset, startOn, useremail, cummu);
            sch.Reset();

            Content = sch.GetReportString();

            if (Force) result = DialogResult.OK;
            else result = MessageBox.Show("The following scheduled measurement will be added:\n\n" + Content, "Please confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel) sch.Delete();

            this.Save<LINAA.SchAcqsDataTable>();
        }

        public IEnumerable<SchAcqsRow> FindLastSchedules()
        {
            return this.tableSchAcqs.FindLastSchedules();
        }

        public void PopulateScheduledAcqs()
        {
            try
            {
                this.tableSchAcqs.BeginLoadData();
                DB.LINAA.SchAcqsDataTable table = this.TAM.SchAcqsTableAdapter.GetData();
                this.tableSchAcqs.Merge(table, false, MissingSchemaAction.AddWithKey);
                this.tableSchAcqs.EndLoadData();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}