using System;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        partial class MonitorsDataTable
        {
            public MonitorsRow FindByMonName(string MonName)
            {
                string field = this.MonNameColumn.ColumnName;
                MonitorsRow old = this.FirstOrDefault(LINAA.SelectorByField<MonitorsRow>(MonName.Trim().ToUpper(), field));
                return old;
            }

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                try
                {
                    MonitorsRow m = e.Row as MonitorsRow;

                    if (e.Column == this.MonGrossMass1Column || e.Column == this.MonGrossMass2Column)
                    {
                        bool one = EC.CheckNull(this.MonGrossMass1Column, e.Row);
                        bool two = EC.CheckNull(this.MonGrossMass2Column, e.Row);

                        if (one || two) return;

                        if (one || two) return;

                        double diff = Math.Abs(m.MonGrossMass1 - m.MonGrossMass2) * 100;

                        double pent = 0.03;

                        m.SetColumnError(this.MonGrossMassAvgColumn, null);
                        if ((diff / m.MonGrossMass1) > pent)
                        {
                            m.SetColumnError(this.MonGrossMassAvgColumn, "Difference between weights is higher than 0.03%\nPlease check");
                        }
                        else if ((diff / m.MonGrossMass2) > pent)
                        {
                            m.SetColumnError(this.MonGrossMassAvgColumn, "Difference between weights is higher than 0.03%\nPlease check");
                        }
                    }
                    else if (e.Column == this.MonNameColumn)
                    {
                        if (EC.CheckNull(this.MonNameColumn, m)) return;

                        if (Dumb.IsLower(m.MonName.Substring(1)))
                        {
                            m.MonName = m.MonName.ToUpper();
                        }
                        else m.MonitorCode = m.MonName.Substring(0, m.MonName.Length - 3);
                    }
                    else if (e.Column == this.columnGeometryName)
                    {
                        if (EC.CheckNull(e.Column, e.Row))
                        {
                            if (!m.IsMonitorCodeNull()) m.GeometryName = m.MonitorCode.ToUpper();
                        }
                        else if (Dumb.IsLower(m.GeometryName.Substring(1)))
                        {
                            m.GeometryName = m.GeometryName.ToUpper();
                        }
                    }
                    else if (e.Column == this.LastIrradiationDateColumn)
                    {
                        if (m.IsLastIrradiationDateNull()) m.LastIrradiationDate = new DateTime(1999, 1, 1);
                        m.NumberOfDays = (DateTime.Today - m.LastIrradiationDate).Days;
                    }
                    else if (e.Column == this.LastMassDateColumn)
                    {
                        if (m.IsLastMassDateNull()) m.LastMassDate = new DateTime(1999, 1, 1);
                        m.Difference = (DateTime.Today - m.LastMassDate).Days;
                    }
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }
    }
}