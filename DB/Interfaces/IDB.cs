using System.Data;
using static DB.LINAA;

namespace DB
{
    public interface IDB
    {
        //void PopulateColumnExpresions();

        // LINAA.AcquisitionsDataTable Acquisitions { get; }
        BlanksDataTable Blanks { get; }

        ChannelsDataTable Channels { get; }
        COINDataTable COIN { get; }
        CompositionsDataTable Compositions { get; }
        ContactPersonsDataTable ContactPersons { get; }
        CustomerDataTable Customer { get; }
        string DataSetName { get; }
        DetectorsAbsorbersDataTable DetectorsAbsorbers { get; }
        DetectorsCurvesDataTable DetectorsCurves { get; }
        DetectorsDimensionsDataTable DetectorsDimensions { get; }
        ElementsDataTable Elements { get; }
        ExceptionsDataTable Exceptions { get; }
        GammasDataTable Gammas { get; }
        GeometryDataTable Geometry { get; }

        HoldersDataTable Holders { get; }
        IRequestsAveragesDataTable IRequestsAverages { get; }
        IrradiationRequestsDataTable IrradiationRequests { get; }

        k0NAADataTable k0NAA { get; }
        MatrixDataTable Matrix { get; }
        MatSSFDataTable MatSSF { get; }
        string[] MatSSFTYPES { get; }
        MeasurementsDataTable Measurements { get; }

        MonitorsDataTable Monitors { get; }

        MonitorsFlagsDataTable MonitorsFlags { get; }

        MUESDataTable MUES { get; }

        NAADataTable NAA { get; }

        OrdersDataTable Orders { get; }

        PeaksDataTable Peaks { get; }

        PeaksHLDataTable PeaksHL { get; }

        PreferencesDataTable Preferences { get; }

        ProjectsDataTable Projects { get; }

        pValuesDataTable pValues { get; }

        ReactionsDataTable Reactions { get; }

        RefMaterialsDataTable RefMaterials { get; }

        DataRelationCollection Relations { get; }

        SamplesDataTable Samples { get; }

        SchAcqsDataTable SchAcqs { get; }

        SchemaSerializationMode SchemaSerializationMode { get; set; }

        SigmasDataTable Sigmas { get; }

        SigmasSalDataTable SigmasSal { get; }

        SolangDataTable Solang { get; }

        SpecPrefDataTable SpecPref { get; }

        // string FolderPath { get; set; }
        SSFPrefDataTable SSFPref { get; }

        StandardsDataTable Standards { get; }
        SubSamplesDataTable SubSamples { get; }
        DataTableCollection Tables { get; }
        ToDoDataTable ToDo { get; }
        ToDoAvgDataTable ToDoAvg { get; }
        ToDoAvgUncDataTable ToDoAvgUnc { get; }
        ToDoDataDataTable ToDoData { get; }

        ToDoResDataTable ToDoRes { get; }
        ToDoResAvgDataTable ToDoResAvg { get; }
        UnitDataTable Unit { get; }
        VialTypeDataTable VialType { get; }
        XCOMPrefDataTable XCOMPref { get; }
        YieldsDataTable Yields { get; }
        void AcceptChanges();

        void CheckMatrixToDoes();
    }
}