using System;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IMain
    {
        // void PopulateDirectory(string path);

        string FolderPath
        {
            get;
            set;
        }

   

        void AddException(Exception ex);

        // string AppPath { get; set; } void Help();

        // void PopulateUserDirectories();

        void Read(string filepath);

        // bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref
        // DB.LINAA.TAMDeleteMethod remover);
    }
}