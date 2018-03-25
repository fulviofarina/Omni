using System;
using System.Collections.Generic;
using System.Data;
using static DB.LINAA;

namespace DB
{
    public interface IStore
    {
        bool SaveMUES(ref MUESDataTable mu, ref MatrixRow m, bool sql = true);
        void Save<T>(ref T row);
        void CleanOthers();
        string FolderPath
        {
            get;
            set;
        }
    //    void InsertMUES(ref MUESDataTable mu, int matrixID);

        void AddException(Exception ex);

        void Read(string filepath);

        void CloneDataSet(ref LINAA set);

        // void AddException(Exception ex);
        void Delete<T>(ref IEnumerable<T> rows);

        IEnumerable<DataTable> GetTablesWithChanges();

        bool DeletePeaks(int measID);

        bool Save<T>(ref IEnumerable<T> rows);

        bool Save<T>();

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
        bool SaveLocalCopy();

        // void SavePreferences();

        /// <summary>
        /// Saves to Server
        /// </summary>
        bool SaveRemote(ref IEnumerable<DataTable> tables);

        /// <summary>
        /// not used
        /// </summary>
       // void SaveSSF();

        /// <summary>
        /// not used
        /// </summary>
       // bool SaveSSF(bool offline, string file);

        //void LoadMonitorsFile(string file);
   
        void CleanPreferences();

        // void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project);
    }
}