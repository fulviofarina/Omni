using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        public partial class MonitorsRow : IRow
        {
            public void Check(DataColumn Column)
            {
                if (Column == this.tableMonitors.MonGrossMass1Column || Column == this.tableMonitors.MonGrossMass2Column)
                {
                    bool one = EC.CheckNull(this.tableMonitors.MonGrossMass1Column, this);
                    bool two = EC.CheckNull(this.tableMonitors.MonGrossMass2Column, this);

                    if (one || two) return;

                    if (one || two) return;

                    double diff = Math.Abs(MonGrossMass1 - MonGrossMass2) * 100;

                    double pent = 0.03;

                    SetColumnError(this.tableMonitors.MonGrossMassAvgColumn, null);
                    if ((diff / MonGrossMass1) > pent)
                    {
                        SetColumnError(this.tableMonitors.MonGrossMassAvgColumn, "Difference between weights is higher than 0.03%\nPlease check");
                    }
                    else if ((diff / MonGrossMass2) > pent)
                    {
                        SetColumnError(this.tableMonitors.MonGrossMassAvgColumn, "Difference between weights is higher than 0.03%\nPlease check");
                    }
                }
                else if (Column == this.tableMonitors.MonNameColumn)
                {
                    if (EC.CheckNull(this.tableMonitors.MonNameColumn, this)) return;

                    if (Dumb.IsLower(MonName.Substring(1)))
                    {
                        MonName = MonName.ToUpper();
                    }
                    else MonitorCode = MonName.Substring(0, MonName.Length - 3);
                }
                else if (Column == this.tableMonitors.GeometryNameColumn)
                {
                    if (EC.CheckNull(Column, this))
                    {
                        if (!IsMonitorCodeNull()) GeometryName = MonitorCode.ToUpper();
                    }
                    else if (Dumb.IsLower(GeometryName.Substring(1)))
                    {
                        GeometryName = GeometryName.ToUpper();
                    }
                }
                else if (Column == this.tableMonitors.LastIrradiationDateColumn)
                {
                    if (IsLastIrradiationDateNull()) LastIrradiationDate = new DateTime(1999, 1, 1);
                    NumberOfDays = (DateTime.Today - LastIrradiationDate).Days;
                }
                else if (Column == this.tableMonitors.LastMassDateColumn)
                {
                    if (IsLastMassDateNull()) LastMassDate = new DateTime(1999, 1, 1);
                    Difference = (DateTime.Today - LastMassDate).Days;
                }
            }

            public void SetParent(ref DataRow row)
            {
                throw new NotImplementedException();
            }
        }

        partial class MonitorsDataTable : IColumn
        {
            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    MonitorsRow m = e.Row as MonitorsRow;

                    m.Check(e.Column);
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }

            public MonitorsRow FindByMonName(string MonName)
            {
                string field = this.MonNameColumn.ColumnName;
                MonitorsRow old = this.FirstOrDefault(LINAA.SelectorByField<MonitorsRow>(MonName.Trim().ToUpper(), field));
                return old;
            }
        }
    }
}