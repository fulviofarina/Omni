using System.Collections.Generic;
using System.Data;

namespace DB.Tools
{
    public interface ICurrent
    {
        DataRow Channel { get; }
        DataRow Irradiation { get; }

        DataRow Matrix { get; }
        DataRow Rabbit { get; }
        DataRow Vial { get; }
        DataRow SubSample { get; }
        DataRow SubSampleMatrix { get; }
        IEnumerable<DataRow> SubSamples { get; }
        DataRow Unit { get; }
        IEnumerable<DataRow> Units { get; }

        // string WindowsUser { get; }
        IEnumerable<string> SubSamplesNames { get; }

        IEnumerable<string> SubSamplesDescriptions { get; }
        DataRow Measurement { get; }
    }
}