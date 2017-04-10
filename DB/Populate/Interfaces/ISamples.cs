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
        int AddSamples(string project, ref ICollection<string> hsamples);

        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);

        void LoadMonitorsFile(string file);

        void LoadSampleData(bool load);

        void PopulateMonitorFlags();

        void PopulateMonitors();

        void PopulateStandards();

        void PopulateSubSamples(Int32 IrReqID);

        void SetIrradiatioRequest(ref IEnumerable<SubSamplesRow> samples, int IrrReqID);

        void SetLabels(ref IEnumerable<SubSamplesRow> samples, string project);

        void SetUnits(ref IEnumerable<SubSamplesRow> samples);
    }
}