using System;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class ToDoResRow
        {
            // internal decimal w = 0;
            // internal decimal w2 = 0;
            // internal decimal wX = 0;
            // internal decimal wX2 = 0;
        }

        partial class ToDoResAvgDataTable
        {
            protected internal string[] sRColumnNames;

            public string[] SRColumnNames
            {
                get
                {
                    if (sRColumnNames == null)
                    {
                        sRColumnNames = new string[] { ToDoNrColumn.ColumnName, sample1Column.ColumnName, sample2Column.ColumnName, Iso1Column.ColumnName, Iso2Column.ColumnName, EnergyColumn.ColumnName, Energy2Column.ColumnName };
                    }
                    return sRColumnNames;
                }
                set { sRColumnNames = value; }
            }

            public ToDoResAvgRow AddToDoResAvgRow(ToDoResRow res)
            {
                ToDoResAvgRow avi = NewToDoResAvgRow();
                AddToDoResAvgRow(avi);
                try
                {
                    //   avi.Key = res.Key;
                    avi.DP = res.DP;
                    //   avi.KeyID = avi.Key.Replace("," + avi.DP, null);
                    avi.ToDoNr = res.ToDoNr;

                    avi.Energy = res.Energy;
                    avi.Energy2 = res.Energy2;
                    avi.Iso1 = res.Iso1;
                    avi.Iso2 = res.Iso2;
                    avi.sample1 = res.sample1;
                    avi.sample2 = res.sample2;
                    if (avi.GetToDoResRows().FirstOrDefault(o => o.use) != null) avi.use = true;
                    else avi.use = false;
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(avi, ex);
                }
                return avi;
            }
        }

        partial class ToDoResDataTable
        {
            public ToDoResRow AddToDoResRow(ref LINAA.PeaksRow p, ref LINAA.PeaksRow p2, int todoNr)
            {
                LINAA.ToDoResRow res = this.NewToDoResRow();
                this.AddToDoResRow(res);
                try
                {
                    res.Meas1ID = p.MeasurementID;
                    res.Meas2ID = p2.MeasurementID;
                    res.Peak1ID = p.PeaksID;
                    res.Peak2ID = p2.PeaksID;
                    res.Iso1 = p.Iso;
                    res.Energy = p.Energy;
                    res.Energy2 = p2.Energy;
                    res.Iso2 = p2.Iso;
                    res.ToDoNr = todoNr;
                    res.use = res.Selected;
                }
                catch (SystemException ex)
                {
                    Rsx.EC.SetRowError(res, ex);
                }
                return res;
            }
        }
    }
}