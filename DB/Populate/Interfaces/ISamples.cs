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
        SubSamplesRow AddSamples(ref IrradiationRequestsRow ir,string sampleName = "", bool save = true);
        SubSamplesRow AddSamples(string project, string sampleName = "", bool save = true);
        IEnumerable<LINAA.SubSamplesRow> AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> hsamples, bool save =true);
        IEnumerable<LINAA.SubSamplesRow> AddSamples(ref IEnumerable<string> hsamples, string project, bool save = true);
 
        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);
        void PopulateUnitsByProject(int irrReqId);
        void PopulatedMonitors(string file);
        SubSamplesRow FindSample(string sampleName, bool AddifNull = false, int? IrrReqID = null);
        void BeginEndLoadData(bool load);
        IEnumerable<SubSamplesRow> FindSamplesByIrrReqID(int? IrReqID);
        IEnumerable<SubSamplesRow> FindByProject(string project);
        bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples);
        void PopulateMonitorFlags();

        void PopulateMonitors();

        void PopulateStandards();

        void PopulateSubSamples(Int32 IrReqID);

   
    }
}