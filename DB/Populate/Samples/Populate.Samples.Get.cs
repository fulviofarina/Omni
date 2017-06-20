using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB
{
    public partial class LINAA : ISamples
    {

        public UnitRow GetUnitBySampleID(int subSampleID)
        {
            return Unit.FirstOrDefault(o => o.SampleID == subSampleID);
        }
      

        public int GetLastSampleNr(ref IEnumerable<SubSamplesRow> samplesToImport, string project)
        {
            string _projectNr = System.Text.RegularExpressions.Regex.Replace(project, "[a-z]", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string[] samplesNames = GetSampleNames(ref samplesToImport, _projectNr);
            int[] _samplesNrs = GetSampleNames(samplesNames);
            if (_samplesNrs == null) return 0;
            int _lastSampleNr = GetLastSampleNr(_samplesNrs);
            return _lastSampleNr;
        }

        public int GetLastSampleNr(int[] _samplesNrs)
        {
            int _lastSampleNr = 1;
            // while (!_samplesNrs.Add(_lastSampleNr)) _lastSampleNr++;
            if (_samplesNrs.Count() != 0)
            {
                _lastSampleNr = _samplesNrs.Max() + 1;
            }

            return _lastSampleNr;
        }

        public int[] GetSampleNames(string[] samplesNames)
        {
            if (samplesNames == null) return null;
            return samplesNames.Select(o => Convert.ToInt32(o)).ToArray();
        }

        public string[] GetSampleNames(ref IEnumerable<SubSamplesRow> samples, string _projectNr)
        {
            string[] samplesNames = null;

            samplesNames = samples.Where(o => o.MonitorsRow == null)
                .Where(o => !o.IsSubSampleNameNull())
                .Select(o => o.SubSampleName.Trim().ToUpper())
                .ToArray();
            if (string.IsNullOrEmpty(_projectNr)) return samplesNames;
            samplesNames = samplesNames.Select(o => o.Replace(_projectNr.Trim().ToUpper(), null))
            .ToArray();
            return samplesNames;
        }
    }
}
