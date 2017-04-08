using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface ISamples
    {
        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);

        void LoadSampleData(bool load);

        void PopulateStandards();

        void PopulateMonitorFlags();

        void SetIrradiatioRequest(ref IEnumerable<SubSamplesRow> samples, int IrrReqID);

        void SetUnits(ref IEnumerable<SubSamplesRow> samples);

        void SetLabels(ref IEnumerable<SubSamplesRow> samples, string project);

        void PopulateMonitors();

        int AddSamples(string project, ref ICollection<string> hsamples);

        void PopulateSubSamples(Int32 IrReqID);

        void LoadMonitorsFile(string file);
    }
}