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
        string FolderPath
        {
            get;
            set;
        }

        bool IsSpectraPathOk { get; }

        void AddException(Exception ex);

        //   string AppPath
        //  {
        //      get;
        //      set;
        //  }
        void Help();

        void PopulateUserDirectories();

        void Read(string filepath);

        //    bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref DB.LINAA.TAMDeleteMethod remover);
    }
}