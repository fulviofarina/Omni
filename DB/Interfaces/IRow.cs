﻿using System.Data;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IRow
    {
        void Check();

        bool HasErrors();

        void Check(DataColumn Column);

        void SetParent<T>(ref T rowParent, object[] args = null);
    }
   
}