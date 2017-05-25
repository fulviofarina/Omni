using System;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IIrradiations
    {
        Int32? FindIrradiationID(string project);
      //  int? FindIrrReqID(String project);
        LINAA.IrradiationRequestsRow AddIrradiation(string project);
        LINAA.IrradiationRequestsRow FindIrradiationByCode(string project);
        void PopulateChannels();

        void PopulateIrradiationRequests();
    }
}