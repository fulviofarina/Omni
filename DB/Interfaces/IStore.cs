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

        bool SaveRows<T>(ref IEnumerable<T> rows);

        bool SaveTable<T>();

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

        // void SavePreferences();

        /// <summary>
        /// Saves to Server
        /// </summary>
        bool SaveTables(ref IEnumerable<DataTable> tables);

        /// <summary>
        /// not used
        /// </summary>
        // void SaveSSF();

        /// <summary>
        /// not used
        /// </summary>
        // bool SaveSSF(bool offline, string file);

        //void LoadMonitorsFile(string file);
    //    bool CleanMUESPics(ref MatrixRow u);
        void CleanPreferences();
      //  void SaveMatrices(bool offline);
        //    bool cleanMUES(ref MatrixRow m, bool sql);

        // void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project);
    }
}