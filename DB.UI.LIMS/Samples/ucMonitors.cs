using DB.Tools;
using Rsx.Dumb;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucMonitors : UserControl
    {
        private string filterCode = string.Empty;
        private string sortCode = "MonName desc";

        public ucMonitors()
        {
            InitializeComponent();
        }

        private Interface Interface;

        public void Set(ref Interface inter)
        {
            Interface = inter;

            Dumb.FD(ref this.Linaa);
            // Dumb.FD(ref this.Linaa);
            Dumb.FD(ref bs);

            DGV.DataSource = Interface.IBS.Monitors;

            // this.Linaa = inter.Get();

            //	Findbox.ComboBox.DataSource = this.BS;
            //Findbox.ComboBox.DisplayMember = this.Linaa.Monitors.MonNameColumn.ColumnName;
            //Findbox.ComboBox.ValueMember = this.Linaa.Monitors.MonNameColumn.ColumnName;

            MonCodebox.ComboBox.DisplayMember = Interface.IDB.Standards.MonitorCodeColumn.ColumnName;
            MonCodebox.ComboBox.ValueMember = Interface.IDB.Standards.MonitorCodeColumn.ColumnName;
            MonCodebox.ComboBox.DataSource = Interface.IDB.Standards;
            PostRefresh(null, EventArgs.Empty);
        }

        public void PostRefresh(object sender, EventArgs e)
        {
            Interface.IPopulate.ISamples.UpdateIrradiationDates();
            MonCodebox_SelectedIndexChanged(sender, e);
        }

        private void MonCodebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ccol = this.Linaa.Monitors.MonitorCodeColumn.ColumnName;
            string mmcol = this.Linaa.Monitors.MonNameColumn.ColumnName;
            string mCode = MonCodebox.Text.ToUpper();

            string min = minBox.Text.Trim();
            if (min.Length == 1) min = "00" + min;
            else if (min.Length == 2) min = "0" + min;

            string max = maxBox.Text.Trim();
            if (max.Length == 1) max = "00" + max;
            else if (max.Length == 2) max = "0" + max;

            filterCode = ccol + " = '" + mCode + "'";

            if (!string.IsNullOrWhiteSpace(min))
            {
                filterCode += " AND " + mmcol + " >= '" + mCode + min + "'";
            }
            if (!string.IsNullOrWhiteSpace(max))
            {
                filterCode += " AND " + mmcol + " <= '" + mCode + max + "'";
            }
            Interface.IBS.Monitors.Sort = sortCode;
            Interface.IBS.Monitors.Filter = filterCode;
            // Rsx.Dumb.BS.LinkBS(ref Interface.IBS.Monitors, this.Linaa.Monitors, filterCode, sortCode);
        }

        public void RowAdded(ref DataRow row)
        {
            LINAA.MonitorsRow m = row as LINAA.MonitorsRow;
            int last = 1;
            minBox.Text = string.Empty;
            maxBox.Text = string.Empty;
            System.Collections.Generic.IEnumerable<DataRowView> views = Interface.IBS.Monitors.List.OfType<DataRowView>();
            System.Collections.Generic.IEnumerable<LINAA.MonitorsRow> mons = Caster.Cast<LINAA.MonitorsRow>(views).Where(o => !o.IsMonNameNull());
            mons = mons.OrderByDescending(o => o.MonName);

            if (mons.Count() >= 1)
            {
                LINAA.MonitorsRow lastmon = mons.FirstOrDefault();
                last = Convert.ToInt32(lastmon.MonName.Replace(lastmon.MonitorCode, null));
                last++;
            }

            string number = string.Empty;
            if (last < 10) number = "00";
            else if (last >= 10 && last < 100) number = "0";
            m.MonName = MonCodebox.Text.ToUpper() + number + last.ToString();
        }

        public bool ShouldPaintCell(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // DataGridViewRow r = DGV.Rows[e.RowIndex]; if (r.Tag != null) return false; return true;
            return (e.ColumnIndex == this.DecayDaysColumn.Index || e.ColumnIndex == this.GeometryName.Index);
        }

        public void PaintCell(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewRow r = DGV.Rows[e.RowIndex];

            if (e.ColumnIndex == this.DecayDaysColumn.Index)
            {
                Color colorToPut = Color.MistyRose;
                LINAA.MonitorsRow mon = Caster.Cast<LINAA.MonitorsRow>(r);
                if (mon.IsIsLockedNull()) mon.IsLocked = false;
                if (mon.MonitorsFlagsRow != null)
                {
                    if (mon.IsNumberOfDaysNull()) mon.IsLocked = true;
                    else if (!mon.MonitorsFlagsRow.IsHalfLifeNull())
                    {
                        int nroOfDays = mon.NumberOfDays;
                        double hl = mon.MonitorsFlagsRow.HalfLife;
                        bool locked = mon.IsLocked;
                        if (nroOfDays >= hl * 30)
                        {
                            colorToPut = Color.Honeydew;
                            if (locked) mon.IsLocked = false;
                        }
                        else if (nroOfDays >= hl * 21)
                        {
                            colorToPut = Color.Lavender;
                            if (locked) mon.IsLocked = false;
                        }
                        else if (nroOfDays >= hl * 15)
                        {
                            colorToPut = Color.PapayaWhip;
                            if (!locked) mon.IsLocked = true;
                        }
                        else if (!locked) mon.IsLocked = true;
                    }
                }

                this.DGV[this.MonNameColumn.Name, e.RowIndex].Style.BackColor = colorToPut;
                this.DGV[this.LastIrradiationDateColumn.Name, e.RowIndex].Style.BackColor = colorToPut;
                this.DGV[this.DecayDaysColumn.Name, e.RowIndex].Style.BackColor = colorToPut;
            }
            else if (e.ColumnIndex == this.GeometryName.Index)
            {
                Color colorToPut = Color.MistyRose;
                LINAA.MonitorsRow mon = Caster.Cast<LINAA.MonitorsRow>(r);
                string text = string.Empty;
                if (mon.GeometryRow == null)
                {
                    colorToPut = Color.MistyRose;
                    text = "No Geometry is assigned!";
                }
                else
                {
                    colorToPut = Color.Honeydew;
                    if (mon.GeometryRow.IsCommentsNull()) text = "This geometry exist but has no comments!";
                    else text = mon.GeometryRow.Comments;
                }
                this.DGV[this.GeometryName.Name, e.RowIndex].Style.BackColor = colorToPut;
                this.DGV[this.GeometryName.Name, e.RowIndex].ToolTipText = text;
            }
        }
    }
}