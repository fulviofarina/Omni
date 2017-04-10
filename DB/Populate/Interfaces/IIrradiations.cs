using System;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IIrradiations
    {
        Int32? FindIrrReqID(string project);

        void PopulateChannels();

        void PopulateIrradiationRequests();
    }
}