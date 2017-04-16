using System;
using System.Collections.Generic;
using System.Data;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IStore
    {
        // void AddException(Exception ex);
        void Delete<T>(ref IEnumerable<T> rows);
        IEnumerable<DataTable> GetTablesWithChanges();
        bool DeletePeaks(int measID);

        bool Save<T>(ref IEnumerable<T> rows);

        bool Save<T>();

        bool Save(string file);

        string SaveExceptions();

        bool SaveLocalCopy();

        //  void SavePreferences();

        bool SaveRemote(ref IEnumerable<DataTable> tables, bool takeChanges);

        void SaveSSF();

        bool SaveSSF(bool offline, string file);

        //void LoadMonitorsFile(string file);
        void UpdateIrradiationDates();

        //   void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project);
    }
}