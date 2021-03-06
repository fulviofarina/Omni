﻿using System;
using System.Collections.Generic;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface ISchedAcqs
    {
        void AddSchedule(string project, string sample, Int16 pos, string det, Int16 repeats, double preset, DateTime startOn, string useremail, bool cummu, bool Force);

        IEnumerable<SchAcqsRow> FindLastSchedules();

        void PopulateScheduledAcqs();
    }
}