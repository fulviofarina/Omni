using System;
using System.Collections.Generic;
using System.Data;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IStore
    {
        void SaveSSF();

        bool SaveSSF(bool offline, string file);

        // void AddException(Exception ex);
        void Delete<T>(ref IEnumerable<T> rows);

        bool DeletePeaks(int measID);

        bool Save<T>(ref IEnumerable<T> rows);

        bool Save<T>();

        string SaveExceptions();

        void SavePreferences();

        //void LoadMonitorsFile(string file);
        void UpdateIrradiationDates();

        //   void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project);

        bool Save(string file);
    }
}