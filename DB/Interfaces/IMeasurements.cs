using static DB.LINAA;

namespace DB
{
    public interface IMeasurements
    {

         void PopulatePeaksHL(int? id);

         PeaksHLDataTable PopulatePeaksHL(int? id, double minArea, double maxUnc);
         MeasurementsDataTable PopulateMeasurementsGeneric(string project, bool merge);
         MeasurementsRow AddMeasurement(string measName);

         MeasurementsRow FindByMeas(string measName);

    }
}