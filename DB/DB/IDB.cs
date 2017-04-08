using System.Collections.Generic;
using System.Data;

namespace DB
{
    public interface IDB
    {
        LINAA.IRequestsAveragesDataTable IRequestsAverages { get; }

        //   string FolderPath { get; set; }
        LINAA.SSFPrefDataTable SSFPref { get; }

        LINAA.PreferencesDataTable Preferences { get; }

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

        LINAA.ToDoResDataTable ToDoRes { get; }
        LINAA.ToDoResAvgDataTable ToDoResAvg { get; }
        LINAA.UnitDataTable Unit { get; }
        LINAA.VialTypeDataTable VialType { get; }
        LINAA.YieldsDataTable Yields { get; }
    }
}