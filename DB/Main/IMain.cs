using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IMain
    {
        bool IsSpectraPathOk { get; }

        void Read(string filepath);

        string AppPath
        {
            get;
            set;
        }

        string FolderPath
        {
            get;
            set;
        }

        void Help();

        //    bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref DB.LINAA.TAMDeleteMethod remover);
    }
}