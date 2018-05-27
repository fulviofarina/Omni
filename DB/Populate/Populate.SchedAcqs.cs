using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DB
{
    public partial class LINAA : ISchedAcqs
    {
        protected static string CREATE_NEW = "\n\nDo you want to update the scheduled measurement with the new data?\nIf not, I will create a new one!";
        protected static string MEASUREMENT_ADDED = "The following scheduled measurement will be added:\n\n";
        protected static string CONFIRM = "Please confirm";
        protected static string MEASUREMENT_FOUND = "Found a previous scheduled measurement";

        public SchAcqsRow FindASpecificSchedule(string det, string project, string sample)
        {
            IEnumerable<SchAcqsRow> todo = this.tableSchAcqs.Where(SelectorSchAcqsBy(false, false));
            todo = todo.Where(SelectorSchAcqsBy(det));
            return todo.FirstOrDefault(SelectorSchAcqsBy(project, sample));
        }

        public SchAcqsRow[] FindDetectorLastSchedule(string det)
        {
            IEnumerable<SchAcqsRow> todo = FindDetectorSchedules(det);
            DateTime ahora = DateTime.Now;
            SchAcqsRow NEXT = todo.FirstOrDefault(SelectorSchAcqsBy(ahora, false, false));
            SchAcqsRow NOW = todo.FirstOrDefault(SelectorSchAcqsBy(ahora, true, true));

            return new SchAcqsRow[] { NOW, NEXT };
        }

        public IEnumerable<SchAcqsRow> FindDetectorSchedules(string det)
        {
            IEnumerable<SchAcqsRow> todo = this.tableSchAcqs.Where(SelectorSchAcqsBy(false, false));
            todo = todo.Where(LINAA.SelectorSchAcqsBy(det));
            return todo;
        }

        public IEnumerable<SchAcqsRow> FindLastSchedules()
        {
            IEnumerable<SchAcqsRow> todo = this.tableSchAcqs.Where(SelectorSchAcqsBy(false, false));
            DateTime ahora = DateTime.Now;
            return todo.Where(SelectorSchAcqsBy(ahora, true, true));
        }
        private SchAcqsRow addSchedule()
        {
            SchAcqsRow sch = this.SchAcqs.NewSchAcqsRow();
            this.SchAcqs.AddSchAcqsRow(sch);
            return sch;
        }
        public void AddSchedule(string project, string sample, Int16 pos, string det, Int16 repeats, double preset, DateTime startOn, string useremail, bool cummu, bool Force)
        {
            SchAcqsRow sch = this.FindASpecificSchedule(det, project, sample);
            DialogResult result;
            string Content = string.Empty;
            if (sch == null) sch = addSchedule();
            else
            {
                Content = sch.GetReportString();
                if (Force) result = DialogResult.No;
                else
                {
                    string msg = "Sample " + sample + " was found in the schedule:\n\n" + Content + CREATE_NEW;
                    result = MessageBox.Show(msg, MEASUREMENT_FOUND, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                }

                if (result == DialogResult.No) sch = addSchedule();
                else if (result == DialogResult.Cancel) return;
            }

            sch.SetSchedule(project, sample, pos, det, repeats, preset, startOn, useremail, cummu);
            sch.Reset();

            Content = sch.GetReportString();

            if (Force) result = DialogResult.OK;
            else result = MessageBox.Show(MEASUREMENT_ADDED + Content, CONFIRM, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel) sch.Delete();

            this.Save<SchAcqsDataTable>();
        }

        public void PopulateScheduledAcqs()
        {
            try
            {
                this.tableSchAcqs.BeginLoadData();
                SchAcqsDataTable table = this.TAM.SchAcqsTableAdapter.GetData();
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