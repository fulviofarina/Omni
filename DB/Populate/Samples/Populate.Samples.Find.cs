using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DB.LINAA;

namespace DB
{

    public partial class LINAA : ISamples
    {
        public IEnumerable<SubSamplesRow> FindByProject(string project)
        {
            string cd = DB.Properties.Misc.Cd;
            string IrReqField = this.tableSubSamples.IrradiationCodeColumn.ColumnName;
            project = project.Replace(cd, null);
            IEnumerable<SubSamplesRow> old = null;
            old = this.tableSubSamples
                .Where(LINAA.SelectorByField<SubSamplesRow>(project, IrReqField))
                .ToList();
            IEnumerable<SubSamplesRow> oldCD = this.tableSubSamples
                .Where(LINAA.SelectorByField<SubSamplesRow>(project + cd, IrReqField));

            old = old.Union(oldCD);

            return old.ToList();
        }

        public IEnumerable<VialTypeRow> FindCapsules(string coment)
        {
            return this.VialType.Where(o => !o.IsVialTypeRefNull() && o.Comments.ToUpper().Contains(coment));
        }

        public MonitorsRow FindMonitorByName(string MonName)
        {
            string field = this.Monitors.MonNameColumn.ColumnName;
            return this.Monitors.FirstOrDefault(LINAA.SelectorByField<MonitorsRow>(MonName.Trim().ToUpper(), field));
        }

        public SubSamplesRow FindSample(string sampleName)
        {
            string field = this.tableSubSamples.SubSampleNameColumn.ColumnName;
            string fieldVal = sampleName.Trim().ToUpper();
            SubSamplesRow sample = this.tableSubSamples
                .FirstOrDefault(LINAA.SelectorByField<SubSamplesRow>(fieldVal, field));
            return sample;
        }

        public IEnumerable<SubSamplesRow> FindSamplesByIrrReqID(int? IrReqID)
        {
            IEnumerable<SubSamplesRow> old = null;
            string IrReqField = this.tableSubSamples.IrradiationRequestsIDColumn.ColumnName;
            old = this.tableSubSamples
                .Where(LINAA.SelectorByField<SubSamplesRow>(IrReqID, IrReqField));
            return old.ToList();
        }
    }
}
