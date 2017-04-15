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
        void AddSamples(ref SubSamplesDataTable newsamples);
        int AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> hsamples, bool monitors = false);
        IEnumerable<LINAA.SubSamplesRow> CreateSamplesNamesFrom(ref IEnumerable<string> hsamples);

        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);
        void PopulateUnitsByProject(int irrReqId);
        void LoadMonitorsFile(string file);

        void BeginEndLoadData(bool load);

        void PopulateMonitorFlags();

        void PopulateMonitors();

        void PopulateStandards();

        void PopulateSubSamples(Int32 IrReqID);

        void SetIrradiatioRequest(ref IEnumerable<SubSamplesRow> samples, int IrrReqID);

        void SetLabels(ref IEnumerable<SubSamplesRow> samples, string project);

        void SetUnits(ref IEnumerable<SubSamplesRow> samples);
    }
}