using System;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IIrradiations
    {
        ChannelsRow AddNewChannel();

        Int32? FindIrradiationID(string project);

        // int? FindIrrReqID(String project);
        IrradiationRequestsRow AddNewIrradiation(string project);

        IrradiationRequestsRow FindIrradiationByCode(string project);

        void PopulateChannels();

        void PopulateIrradiationRequests();
    }
}