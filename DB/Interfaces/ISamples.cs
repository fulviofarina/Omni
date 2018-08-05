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
        IRequestsAveragesRow AddIRequestsAverage(Int32 NAAID, ref SubSamplesRow s);

        SubSamplesRow AddSample(ref IrradiationRequestsRow ir, string sampleName = "");

        SubSamplesRow AddSample(string project, string sampleName = "");

        SubSamplesRow AddSample(string sampleName, int? IrrReqID = null);

        IEnumerable<SubSamplesRow> AddSamples(string project, ref IEnumerable<SubSamplesRow> hsamples, bool save = true);

        IEnumerable<SubSamplesRow> AddSamples(ref IEnumerable<string> hsamples, string project, bool save = true);

        void BeginEndLoadData(bool load);

        // Action<object, EventData> SpectrumCalcParametersHandler { set; }
        double CalculateAvgOfFCs(string irradiationCode);

        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);

        IEnumerable<SubSamplesRow> FindByProject(string project);

        IEnumerable<SubSamplesRow> FindSamplesByIrrReqID(int? IrReqID);

        UnitRow GetUnitBySampleID(int subSampleID);

        bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples);

        void PopulatedMonitors(string file);

        void PopulateMonitorFlags();

        void PopulateMonitors();

        void PopulateStandards();

        void PopulateSubSamples(Int32 IrReqID);

        void PopulateUnitsByProject(int irrReqId);

        void UpdateIrradiationDates();
        // MeasurementsDataTable PopulateMeasurementsGeneric(string project, bool merge);
        // PeaksHLDataTable PopulatePeaksHL(int? id, double minArea, double maxUnc); void
        // PopulatePeaksHL(int? id);
    }
}