using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface INuclear
    {
        void PopulateSigmas();

        void PopulatepValues();

        void PopulateSigmasSal();

        void PopulateYields();

        void PopulateReactions();

        void PopulateElements();
    }
}