using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Rsx.Dumb;

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
        }

        public partial class XCOMPrefRow : IRow
        {
            public void Check()
            {
                // DoMatSSF = true; DoCK = false;

                foreach (DataColumn item in this.tableXCOMPref.Columns)
                {
                    Check(item);
                }
            }

            public void Check(DataColumn Column)
            {
                // if (IsLoopNull()) Loop = false; if (IsUseListNull()) UseList = false;
                if (IsStartEnergyNull()) StartEnergy = 1;
                if (IsEndEnergyNull()) EndEnergy = 3500;
                if (IsStepsNull()) Steps = 1;
                 if (IsForceNull()) Force = true;
                if (IsRoundingNull()) Rounding = "e3";
                if (IsPENull()) PE = true;
                if (IsPPEFNull()) PPEF = true;
                if (IsPPNFNull()) PPNF = true;
                if (IsISNull()) IS= true;
                if (IsCSNull()) CS = true;
                if (IsTCSNull()) TCS = true;
                if (IsTNCSNull()) TNCS = true;


                if (IsListOfEnergiesNull())
                {
                    ListOfEnergies = Encoding.UTF8.GetBytes(DB.Properties.Resources.XCOM);
                }
                    //  if (IsASCIIOutputNull()) ASCIIOutput = false;
                if (IsLogGraphNull()) LogGraph = true;

                // if ()
                bool nulow = EC.CheckNull(Column, this);
            }

            public new bool HasErrors()
            {
                return base.HasErrors;
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
               // throw new NotImplementedException();
            }
        }
    }
}