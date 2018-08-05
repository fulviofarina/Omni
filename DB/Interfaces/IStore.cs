using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

namespace DB
{
    public interface IStore
    {
        string FolderPath
        {
            get;
            set;
        }

        void AddException(Exception ex);

        void CleanOthers();

        void CleanPreferences();

        void CloneDataSet(ref LINAA set);

        void Delete<T>(ref IEnumerable<T> rows);

        bool DeletePeaks(int measID);

        IEnumerable<DataTable> GetTablesWithChanges();

        void Read(string filepath);

        void Save<T>(ref T row);

        /// <summary>
        /// General Method
        /// </summary>
        bool Save(string file);

        /// <summary>
        /// Saves Exceptions
        /// </summary>
        string SaveExceptions();

        /// <summary>
        /// Saves to a File
        /// </summary>
        bool SaveLocalCopy(string LIMSPath = "");

        bool SaveMUES(ref MUESDataTable mu, ref MatrixRow m, bool sql = true);

        bool SaveRows<T>(ref IEnumerable<T> rows);

        bool SaveTable<T>();

        /// <summary>
        /// Saves to Server
        /// </summary>
        bool SaveTables(ref IEnumerable<DataTable> tables);
    }
}