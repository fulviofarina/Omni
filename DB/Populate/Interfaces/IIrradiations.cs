﻿using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IIrradiations
    {
        void PopulateIrradiationRequests();

        void PopulateChannels();

        Int32? FindIrrReqID(string project);
    }
}