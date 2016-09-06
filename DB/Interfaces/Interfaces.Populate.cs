using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// DB (LINAA) interfaces
/// </summary>
namespace DB
{
    public interface IMain
    {
        void PopulateUserDirectories();

        bool RemoveDuplicates(DataTable table, string UniqueField, string IndexField, ref DB.LINAA.TAMDeleteMethod remover);

        void ReadLinaa(string filepath);

        void Help();
    }

    public interface ISamples
    {
        void LoadSampleData(bool load);

        void PopulateStandards();

        void PopulateMonitorFlags();

        void PopulateMonitors();

        int AddSamples(string project, ref ICollection<string> hsamples);

        void PopulateSubSamples(Int32 IrReqID);

        void LoadMonitorsFile(string file);
    }

    public interface ISchedAcqs
    {
        void PopulateScheduledAcqs();

        void AddSchedule(string project, string sample, Int16 pos, string det, Int16 repeats, double preset, DateTime startOn, string useremail, bool cummu, bool Force);

        IEnumerable<LINAA.SchAcqsRow> FindLastSchedules();
    }

    public interface IDetSol
    {
        void PopulateCOIList();

        void PopulateDetectorCurves();

        void PopulateDetectorAbsorbers();

        void PopulateDetectorDimensions();

        void PopulateDetectorHolders();
    }

    public interface IGeometry
    {
        void PopulateXCOMList();

        LINAA.GeometryRow DefaultGeometry { get; }

        LINAA.GeometryRow FindReferenceGeometry(string refName);

        void PopulateCompositions();

        void PopulateMatrix();

        void PopulateVials();

        void PopulateUnits();

        void PopulateGeometry();
    }

    public interface IIrradiations
    {
        void PopulateIrradiationRequests();

        void PopulateChannels();

        Int32? FindIrrReqID(string project);
    }

    public interface INuclear
    {
        void PopulateSigmas();

        void PopulatepValues();

        void PopulateSigmasSal();

        void PopulateYields();

        void PopulateReactions();

        void PopulateElements();
    }

    public interface IOrders
    {
        void PopulateOrders();

        Int32? FindOrderID(string LabOrdRef);
    }

    public interface IProjects
    {
        IList<LINAA.SubSamplesRow> FindByProject(string project);

        void PopulateToDoes();

        void PopulateProjects();
    }

    public interface IToDoes
    {
        void PopulateToDoes();
    }
}