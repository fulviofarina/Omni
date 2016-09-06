using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IStore
    {
        public IEnumerable<DataTable> GetTablesWithChanges()
        {
            IEnumerable<DataTable> tables = null;
            tables = this.Tables.OfType<DataTable>();
            Func<DataTable, bool> haschangesFunc = t =>
            {
                bool hasChanges = false;
                IEnumerable<DataRow> rows = t.AsEnumerable();
                IEnumerable<DataRow> rowsWithChanges = Dumb.GetRowsWithChanges(rows);
                hasChanges = rowsWithChanges.Count() != 0;
                return hasChanges;
            };

            return tables.Where(haschangesFunc);
        }

        public void SetIrradiatioRequest(IEnumerable<LINAA.SubSamplesRow> samples, ref IrradiationRequestsRow irRow)
        {
            foreach (LINAA.SubSamplesRow s in samples)
            {
                s.IrradiationRequestsRow = irRow;
            }
        }

        public void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project)
        {
            string _projectNr = System.Text.RegularExpressions.Regex.Replace(project, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Collections.Generic.HashSet<int> _samplesNrs = new System.Collections.Generic.HashSet<int>();

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.MonitorsRow == null)
                {
                    if (!s.IsSubSampleNameNull())
                    {
                        _samplesNrs.Add(Convert.ToInt16(s.SubSampleName.Replace(_projectNr, null)));
                    }
                }
            }

            int _lastSampleNr = 1;

            foreach (LINAA.SubSamplesRow s in samples)
            {
                if (s.MonitorsRow == null)
                {
                    if (s.IsSubSampleNameNull())
                    {
                        while (!_samplesNrs.Add(_lastSampleNr)) _lastSampleNr++;
                        if (_lastSampleNr >= 10) s.SubSampleName = _projectNr + _lastSampleNr.ToString();
                        else s.SubSampleName = _projectNr + "0" + _lastSampleNr.ToString();
                    }
                    //  s.IrradiationCode = project;
                    Dumb.CheckNull(this.SubSamples.SubSampleNameColumn, s);
                }
            }
        }
    }
}