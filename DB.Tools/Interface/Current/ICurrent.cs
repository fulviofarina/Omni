using System.Collections.Generic;
using System.Data;

namespace DB.Tools
{
    public interface ICurrent
    {
        DataRow Channel { get; }
        DataRow Irradiation { get;  }
        bool IsSpectraPathOk { get; }
        DataRow Matrix { get; }
        DataRow SubSample { get; }
        DataRow SubSampleMatrix { get; }
        IEnumerable<DataRow> SubSamples { get; }
        DataRow Unit { get; }
        IEnumerable<DataRow> Units { get; }
        string WindowsUser { get; }
    }
}