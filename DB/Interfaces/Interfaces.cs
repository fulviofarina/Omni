using System;
using System.Collections.Generic;
using DB.LINAATableAdapters;

using System.Data;

namespace DB
{
    public interface IDB
    {

        //bool Read(string file);

        // void PopulateColumnExpresions();

      
LINAA.SSFPrefDataTable SSFPref { get; }
            ICollection<string> ActiveProjectsList { get; }
        LINAA.PreferencesRow CurrentPref { get; set; }
        LINAA.GeometryRow DefaultGeometry { get; }
        string Exception { get; }
        string FolderPath { get; set; }
        bool IsMainConnectionOk { get; }
        bool IsSpectraPathOk { get; }
        ICollection<string> OrdersList { get; }
        IList<string> ProjectsList { get; }

        LINAA.AcquisitionsDataTable Acquisitions { get; }
        LINAA.BlanksDataTable Blanks { get; }
        LINAA.ChannelsDataTable Channels { get; }
        LINAA.COINDataTable COIN { get; }
        LINAA.CompositionsDataTable Compositions { get; }
        LINAA.ContactPersonsDataTable ContactPersons { get; }
        LINAA.CustomerDataTable Customer { get; }
        LINAA.DetectorsAbsorbersDataTable DetectorsAbsorbers { get; }
        LINAA.DetectorsCurvesDataTable DetectorsCurves { get; }
        LINAA.DetectorsDimensionsDataTable DetectorsDimensions { get; }
        ICollection<string> DetectorsList { get; set; }
        LINAA.ElementsDataTable Elements { get; }
        LINAA.ExceptionsDataTable Exceptions { get; }
        LINAA.GammasDataTable Gammas { get; }
        LINAA.GeometryDataTable Geometry { get; }
        LINAA.HelloWorldDataTable HelloWorld { get; }
        LINAA.HoldersDataTable Holders { get; }
        LINAA.IrradiationRequestsDataTable IrradiationRequests { get; }
        LINAA.k0NAADataTable k0NAA { get; }
        LINAA.MatrixDataTable Matrix { get; }
        LINAA.MatSSFDataTable MatSSF { get; }
        LINAA.MeasurementsDataTable Measurements { get; }
        LINAA.MonitorsDataTable Monitors { get; }
        LINAA.MonitorsFlagsDataTable MonitorsFlags { get; }
        LINAA.MUESDataTable MUES { get; }
        LINAA.NAADataTable NAA { get; }
        LINAA.OrdersDataTable Orders { get; }
        LINAA.PeaksDataTable Peaks { get; }
        LINAA.PeaksHLDataTable PeaksHL { get; }
        LINAA.PreferencesDataTable Preferences { get; }
        LINAA.ProjectsDataTable Projects { get; }
        LINAA.pValuesDataTable pValues { get; }
        LINAA.ReactionsDataTable Reactions { get; }
        LINAA.RefMaterialsDataTable RefMaterials { get; }
        DataRelationCollection Relations { get; }
        LINAA.SamplesDataTable Samples { get; }
        LINAA.SchAcqsDataTable SchAcqs { get; }
        SchemaSerializationMode SchemaSerializationMode { get; set; }
        LINAA.SigmasDataTable Sigmas { get; }
        LINAA.SigmasSalDataTable SigmasSal { get; }
        LINAA.SolangDataTable Solang { get; }
        LINAA.StandardsDataTable Standards { get; }
        LINAA.SubSamplesDataTable SubSamples { get; }
        DataTableCollection Tables { get; }
        LINAA.ToDoDataTable ToDo { get; }
        LINAA.ToDoAvgDataTable ToDoAvg { get; }
        LINAA.ToDoAvgUncDataTable ToDoAvgUnc { get; }
        LINAA.ToDoDataDataTable ToDoData { get; }
        IList<string> ToDoesList { get; }
        LINAA.ToDoResDataTable ToDoRes { get; }
        LINAA.ToDoResAvgDataTable ToDoResAvg { get; }
        LINAA.UnitDataTable Unit { get; }
        LINAA.VialTypeDataTable VialType { get; }
        LINAA.YieldsDataTable Yields { get; }
    }


    public interface IPreferences
    {
        void PopulatePreferences();

        LINAA.PreferencesRow CurrentPref { get; set; }

        LINAA.SSFPrefRow CurrentSSFPref { get; set; }

       // void PopulateSSFPreferences();

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

        void SetIrradiatioRequest(IEnumerable<LINAA.SubSamplesRow> samples, ref LINAA.IrradiationRequestsRow irRow);

        void SetLabels(ref IEnumerable<LINAA.SubSamplesRow> samples, string project);


        bool Save(string file);
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