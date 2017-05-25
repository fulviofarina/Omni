using System;
using System.Data;
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
                    CheckMass();
                }
                else if (Column == this.tableMonitors.MonNameColumn)
                {
                    SetMonitorName();
                }
                else if (Column == this.tableMonitors.GeometryNameColumn)
                {
                    SetGeometryName();
                }
                else if (Column == this.tableMonitors.LastIrradiationDateColumn)
                {
                    SetNumberOfDays();
                }
                else if (Column == this.tableMonitors.LastMassDateColumn)
                {
                    SetLastMassDate();
                }
            }

            public void SetParent<T>(ref T rowParent, object[] args = null)
            {
                throw new NotImplementedException();
            }

        }
        public partial class MonitorsRow 
        {
            internal void CheckMass()
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

            internal void SetGeometryName()
            {
                if (EC.CheckNull(this.tableMonitors.GeometryNameColumn, this))
                {
                    if (!IsMonitorCodeNull()) GeometryName = MonitorCode.ToUpper();
                }
                else if (Dumb.IsLower(GeometryName.Substring(1)))
                {
                    GeometryName = GeometryName.ToUpper();
                }
            }

            internal void SetLastMassDate()
            {
                if (IsLastMassDateNull()) LastMassDate = new DateTime(1999, 1, 1);
                Difference = (DateTime.Today - LastMassDate).Days;
            }

            internal void SetMonitorName()
            {
                if (EC.CheckNull(this.tableMonitors.MonNameColumn, this)) return;

                if (Dumb.IsLower(MonName.Substring(1)))
                {
                    MonName = MonName.ToUpper();
                }
                else MonitorCode = MonName.Substring(0, MonName.Length - 3);
            }

            internal void SetNumberOfDays()
            {
                if (IsLastIrradiationDateNull()) LastIrradiationDate = new DateTime(1999, 1, 1);
                NumberOfDays = (DateTime.Today - LastIrradiationDate).Days;
            }
        }
    }
}