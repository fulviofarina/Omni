using System;
using System.Collections.Generic;
using DB.LINAATableAdapters;

namespace DB
{
    public interface IPreferences
    {
        void PopulatePreferences();

        LINAA.PreferencesRow CurrentPref { get; set; }
    }

    public interface IReport
    {
        void GenerateReport(string labelReport, object path, string extra, string module, string email);

        void GenerateBugReport();

        void Speak(string text);

        void Msg(string msg, string title, bool ok);

        void Msg(string msg, string title);

        void AddException(Exception ex);

        LINAA.ExceptionsDataTable Exceptions { get; }

        void ReportProgress(int percentage);

        bool IsSpectraPathOk { get; }
    }

    public interface ITables
    {
        void PopulateColumnExpresions();

        void PopulateSelectedExpression(bool setexpression);
    }

    public interface IStore
    {
        // void AddException(Exception ex);
        void Delete<T>(ref IEnumerable<T> rows);

        bool DeletePeaks(int measID);

        bool Save<T>(ref IEnumerable<T> rows);

        bool Save<T>();

        string SaveExceptions();

        void SavePreferences();

        //void LoadMonitorsFile(string file);
        void UpdateIrradiationDates();

        void SetIrradiatioRequest(IEnumerable<LINAA.SubSamplesRow> samples, ref LINAA.IrradiationRequestsRow irRow);

        void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project);
    }

    public interface IAdapter
    {
        void DisposeAdapters();

        void DisposeSolCoinAdapters();

        void InitializeAdapters();

        void InitializeSolCoinAdapters();

        TableAdapterManager TAM { get; set; }

        bool IsMainConnectionOk { get; }
        string Exception { get; }
    }
}