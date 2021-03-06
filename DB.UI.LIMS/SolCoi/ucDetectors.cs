﻿using DB.Tools;
using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class ucDetectors : UserControl
    {
        public ucDetectors()
        {
            InitializeComponent();
        }

        public void Set(ref Interface inter)
        {
            Dumb.FD(ref this.Linaa);
            this.Linaa = inter.Get();

            this.setContactLayerMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.ContactLayerMatrixIDColumn;
            this.setCrystalMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.CrystalMatrixIDColumn;
            this.setTopDeadLayerMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.TopDeadLayerMatrixIDColumn;
            this.setHolderSupportMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.HolderSupportMatrixIDColumn;
            this.setOtherAbsorberMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.OtherAbsorberMatrixIDColumn;
            this.setCanTopMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.CanTopMatrixIDColumn;
            this.setCanSideMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.CanSideMatrixIDColumn;
            this.setCrystalHolderMatrixToolStripMenuItem.Tag = this.Linaa.DetectorsAbsorbers.CrystalHolderMatrixIDColumn;

            IEnumerable<ToolStripMenuItem> tsmi = this.detabsCMS.Items.OfType<ToolStripMenuItem>().Where(s => s.Text.Contains("Set"));

            foreach (ToolStripMenuItem i in tsmi) i.Click += LIMSUI.SetItem;
        }

        private void detabsDGV_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;
                DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
                string colname = col.DataPropertyName;
                IEnumerable<ToolStripMenuItem> tsmi = this.detabsCMS.Items.OfType<ToolStripMenuItem>().Where(s => s.Text.Contains("Set"));
                ToolStripMenuItem i = tsmi.FirstOrDefault(o => o.Text.Contains(colname));
                if (i != null) i.PerformClick();
            }
        }

        private void RefreshTables_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Linaa.PopulateDetectorAbsorbers();

                this.Linaa.PopulateDetectorDimensions();

                this.Linaa.PopulateDetectorHolders();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void SaveDimensions_Click(object sender, EventArgs e)
        {
            System.Data.DataTable table = this.Linaa.Tables[BS.DataMember];
            Type t = table.GetType();
            if (t.Equals(typeof(LINAA.DetectorsCurvesDataTable)))
            {
                LIMSUI.Interface.IStore.SaveTable<LINAA.DetectorsCurvesDataTable>();
            }
            else if (t.Equals(typeof(LINAA.DetectorsDimensionsDataTable)))
            {
                LIMSUI.Interface.IStore.SaveTable<LINAA.DetectorsDimensionsDataTable>();
            }
            else if (t.Equals(typeof(LINAA.DetectorsAbsorbersDataTable)))
            {
                LIMSUI.Interface.IStore.SaveTable<LINAA.DetectorsAbsorbersDataTable>();
            }
            else if (t.Equals(typeof(LINAA.HoldersDataTable)))
            {
                LIMSUI.Interface.IStore.SaveTable<LINAA.HoldersDataTable>();
            }
        }

        private void MainTab_Selected(object sender, TabControlEventArgs e)
        {
            if (MainTab.SelectedTab == this.Curves) Rsx.Dumb.BS.LinkBS(ref BS, this.Linaa.DetectorsCurves);
            else if (MainTab.SelectedTab == this.AbsorbersTab) Rsx.Dumb.BS.LinkBS(ref BS, this.Linaa.DetectorsAbsorbers);
            else if (MainTab.SelectedTab == this.DimensionsTab) Rsx.Dumb.BS.LinkBS(ref BS, this.Linaa.DetectorsDimensions);
            else if (MainTab.SelectedTab == this.HoldersTab) Rsx.Dumb.BS.LinkBS(ref BS, this.Linaa.Holders);

            DataGridView dgv = MainTab.SelectedTab.Controls.OfType<TableLayoutPanel>().First().Controls.OfType<DataGridView>().First();
            dgv.DataSource = BS;
        }

        private void ucDetectors_Load(object sender, EventArgs e)
        {
            MainTab_Selected(sender, new TabControlEventArgs(this.MainTab.SelectedTab, MainTab.TabIndex, TabControlAction.Selected));
        }

        /*
         private void detdimDGV_MouseHover(object sender, EventArgs e)
         {
            if (sender.Equals(this.detabsDGV))
            {
               this.SaveAbsorbers.Enabled = Rsx.Dumb.HasChanges(this.Linaa.DetectorsAbsorbers);
            }
            else if (sender.Equals(this.detdimDGV))
            {
               this.SaveDimensions.Enabled = Rsx.Dumb.HasChanges(this.Linaa.DetectorsDimensions);
            }
            else if (sender.Equals(this.holdersDGV))
            {
               this.SaveHolders.Enabled = Rsx.Dumb.HasChanges(this.Linaa.Holders);
            }
         }
        */
    }
}