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
     //   void AddSamples(ref SubSamplesDataTable newsamples);
        int AddSamples(string project, ref IEnumerable<LINAA.SubSamplesRow> hsamples, bool monitors = false);
        IEnumerable<LINAA.SubSamplesRow> AddSamplesFromNames(ref IEnumerable<string> hsamples);

        DataTable CalculateBranchFactor(ref IPeakAveragesRow daugther, ref IEnumerable<IPeakAveragesRow> references);
        void PopulateUnitsByProject(int irrReqId);
        void PopulatedMonitors(string file);
        SubSamplesRow FindBySample(string sampleName, bool AddifNull = false, int? IrrReqID = null);
        void BeginEndLoadData(bool load);
        IEnumerable<SubSamplesRow> FindByIrReqID(int? IrReqID);
        IList<SubSamplesRow> FindByProject(string project);
        bool Override(String Alpha, String f, String Geo, String Gt, bool asSamples);
        void PopulateMonitorFlags();

        void PopulateMonitors();

        void PopulateStandards();

        void PopulateSubSamples(Int32 IrReqID);

     //   void setIrradiatioRequest(ref IEnumerable<SubSamplesRow> samples, int IrrReqID);

      //  void SetLabels(ref IEnumerable<SubSamplesRow> samples, string project);

       // IList<UnitRow> setUnits(ref IEnumerable<SubSamplesRow> samples);
    }
}