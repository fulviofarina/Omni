using Rsx.Dumb;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DB
{
    public partial class LINAA
    {
        public partial class XCOMPrefDataTable : IColumn
        {
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                XCOMPrefRow p = e.Row as XCOMPrefRow;
                // p.Checking(e);
            }
        }

        public partial class XCOMPrefRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn item in this.tableXCOMPref.Columns)
                {
                    Check(item);
                }
            }

            public void Check(DataColumn Column)
            {
                // if (IsLoopNull()) Loop = false; if (IsUseListNull()) UseList = false;
                if (IsStartEnergyNull()) StartEnergy = 1;
                if (IsEndEnergyNull()) EndEnergy = 4000;
                if (IsStepsNull()) Steps = 100;
                if (IsAccumulateResultsNull()) AccumulateResults = true;
                if (IsRoundingNull()) Rounding = "e3";
                if (IsPENull()) PE = true;
                if (IsPPEFNull()) PPEF = true;
                if (IsPPNFNull()) PPNF = true;
                if (IsISNull()) IS = true;
                if (IsCSNull()) CS = true;
                if (IsTCSNull()) TCS = true;
                if (IsTNCSNull()) TNCS = true;
                if (IsListOfEnergiesNull())
                {
                    ListOfEnergies = Encoding.UTF8.GetBytes(Properties.Resources.XCOM);
                }

                bool nulow = EC.CheckNull(Column, this);

                this.EndEdit();
            }

            public void Checking(DataColumnChangeEventArgs e)
            {
                object val = e.Row[e.Column];
                object proposed = e.ProposedValue;

                if (e.Column == this.tableXCOMPref.EndEnergyColumn)
                {
                    if (!IsEndEnergyNull())
                    {
                        if ((double)proposed > 1e8)
                        {
                            e.Row[e.Column] = 1e8;

                            // return;
                        }
                    }
                }
                if (e.Column == this.tableXCOMPref.StartEnergyColumn)
                {
                    if (!IsStartEnergyNull())
                    {
                        if (StartEnergy < (double)proposed)
                        {
                            e.Row[e.Column] = 1;
                            // return;
                        }
                    }
                }
            }

            public new bool HasErrors()
            {
                return base.HasErrors;
            }
        }
    }
}