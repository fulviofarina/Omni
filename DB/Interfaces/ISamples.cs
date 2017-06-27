using System;
using System.Collections.Generic;
using System.Data;
using Rsx.Dumb;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface ISamples
    {
        Action<object, EventData> SpectrumCalcParametersHandler { set; }

        IRequestsAveragesRow AddIRequestsAverage(Int32 NAAID, ref SubSamplesRow s);

        double CalculateAvgOfFCs(string irradiationCode);

        SubSamplesRow AddSamples(ref IrradiationRequestsRow ir, string sampleName = "");

        SubSamplesRow AddSamples(string project, string sampleName = "");

        IEnumerable<LINAA.SubSamplesRow> AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> hsamples, bool save = true);

        IEnumerable<LINAA.SubSamplesRow> AddSamples(ref IEnumerable<string> hsamples, string project, bool save = true);

        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);

        void PopulateUnitsByProject(int irrReqId);

        void PopulatedMonitors(string file);

        SubSamplesRow AddSamples(string sampleName, int? IrrReqID = null);

        void BeginEndLoadData(bool load);

        IEnumerable<SubSamplesRow> FindSamplesByIrrReqID(int? IrReqID);

        IEnumerable<SubSamplesRow> FindByProject(string project);

        bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples);

        void PopulateMonitorFlags();

        void PopulateMonitors();

        void PopulateStandards();

        void PopulateSubSamples(Int32 IrReqID);

        UnitRow GetUnitBySampleID(int subSampleID);
       MeasurementsDataTable PopulateMeasurementsGeneric(string project, bool merge);
        PeaksHLDataTable PopulatePeaksHL(int? id, double minArea, double maxUnc);
        void PopulatePeaksHL(int? id);
    }
}