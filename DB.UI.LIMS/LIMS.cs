using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx.DGV;

namespace DB.UI
{
    public partial class LIMS : Form
    {
        protected internal void CMS_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            string header = (cms.SourceControl as DataGridView).Name;
            DataGridView dgv = cms.SourceControl as DataGridView;

            //set all items visibility as false except for samples
            IEnumerable<System.Windows.Forms.ToolStripMenuItem> items = this.CMS.Items.OfType<System.Windows.Forms.ToolStripMenuItem>().ToList();
            bool visible = true;
            if (header.CompareTo(ControlNames.SubSamples) != 0) visible = false;
            foreach (System.Windows.Forms.ToolStripMenuItem i in items) i.Visible = visible;
            items = null;

            //set today's date item
            if (dgv.CurrentCell != null)
            {
                Type tipo = dgv.Columns[dgv.CurrentCell.ColumnIndex].ValueType;
                if (tipo.Equals(typeof(DateTime))) setTodaysDate.Visible = true;
                else setTodaysDate.Visible = false;
            }
            else setTodaysDate.Visible = false;

            //other general items
            this.unlockProtectedCellsToolStripMenuItem.Visible = true;
            this.refreshT.Visible = true;
            this.undoT.Visible = true;

            //now according to the control...
            switch (header)
            {
                case ControlNames.Geometries:
                    {
                        this.setMatrix.Visible = true;
                        this.setVials.Text = "Set Vial";
                        this.setVials.Visible = true;

                        break;
                    }
                case ControlNames.Vials:
                    {
                        this.setMatrix.Visible = true;

                        break;
                    }

                case ControlNames.Detectors:
                    {
                        break;
                    }
                case ControlNames.Monitors:
                    {
                        this.setStandards.Visible = true;
                        this.setGeometry.Visible = true;
                        this.halflife.Visible = true;

                        this.updateIrradiationDateToolStripMenuItem.Visible = true;

                        break;
                    }
                case ControlNames.Standards:
                    {
                        this.setMatrix.Visible = true;
                        break;
                    }
                case ControlNames.SubSamples:
                    {
                        Rsx.DGV.Control cv = dgv.Tag as Rsx.DGV.Control;
                        ucSubSamples iS = (ucSubSamples)cv.UsrControl;
                        this.halflife.Visible = false;
                        this.setIrradCh.Visible = true;
                        //this is for monitors
                        this.updateIrradiationDateToolStripMenuItem.Visible = false;

                        //this.setVials.Text = "Set Irr. Container";
                        this.shareTirr.Click -= iS.ShareTirr;
                        this.predictToolStripMenuItem.Click -= iS.Predict;
                        this.shareTirr.Click += iS.ShareTirr;
                        this.predictToolStripMenuItem.Click += iS.Predict;
                        break;
                    }
                case ControlNames.MonitorsFlags:
                    {
                        break;
                    }
                case ControlNames.Samples:
                    {
                        break;
                    }
                case ControlNames.Irradiations:
                    {
                        this.setIrradCh.Visible = true;
                        break;
                    }

                case ControlNames.Orders:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        protected internal void limsFormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        protected internal void limsLoad(object sender, System.EventArgs e)
        {
            // if (Screen.AllScreens.Length == 2)
            {
                // this.Location = new System.Drawing.Point(Screen.AllScreens[1].WorkingArea.X + 100,
                // Screen.AllScreens[1].WorkingArea.Y + 600);
            }
        }

        protected internal void modules_DoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem mod = Modules.FocusedItem;
            string title = mod.Group.Header + " - " + mod.Text;

            UserControl control = LIMSUI.CreateUI(mod.Text);

            Creator.CreateAppForm(title, ref control);
        }

        protected internal void refreshT_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = sender as ToolStripMenuItem;
            ContextMenuStrip cms = t.GetCurrentParent() as ContextMenuStrip;
            DataGridView dgv = cms.SourceControl as DataGridView;

            KeyEventArgs args = null;

            if (sender.Equals(this.refreshT)) args = new KeyEventArgs(Keys.Control | Keys.R);
            else if (sender.Equals(this.undoT)) args = new KeyEventArgs(Keys.Control | Keys.Z);
            else if (sender.Equals(this.findToolStripMenuItem)) args = new KeyEventArgs(Keys.Control | Keys.F);
            else if (sender.Equals(this.setTodaysDate)) args = new KeyEventArgs(Keys.Control | Keys.N);
            Rsx.DGV.Control cv = dgv.Tag as Rsx.DGV.Control;

            if (args == null || dgv == null || cv == null) return;

            cv.KeyUp(dgv, args);
        }

        protected internal void unLockProtectedCells(object sender, System.EventArgs e)
        {
            DataGridView dgv = (this.CMS.SourceControl as DataGridView);

            DataTable dt = Rsx.DGV.Control.GetDataSource<DataTable>(ref dgv);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                bool IsreadOnly = dt.Columns[col.DataPropertyName].ReadOnly;

                if (IsreadOnly)
                {
                    col.ReadOnly = unlockProtectedCellsToolStripMenuItem.Checked;
                }
            }
        }

        public LIMS()
        {
            InitializeComponent();

        }

        private Interface Interface;
     

        public LIMS(ref Interface inter)
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Opacity = 0;
            Interface = inter;


            setItems();

            setTags();

            setHandlers();

        }

        private void setHandlers()
        {
            this.FormClosing += this.limsFormClosing;
            this.Load += this.limsLoad;

            this.updateIrradiationDateToolStripMenuItem.Click += delegate
            {
                Interface.IPopulate.ISamples.UpdateIrradiationDates();
            };
            this.unlockProtectedCellsToolStripMenuItem.Click += unLockProtectedCells;

            this.refreshT.Click += new System.EventHandler(this.refreshT_Click);
            this.undoT.Click += new System.EventHandler(this.refreshT_Click);
            this.findToolStripMenuItem.Click += new System.EventHandler(this.refreshT_Click);
            this.setTodaysDate.Click += new System.EventHandler(this.refreshT_Click);
            this.Modules.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.modules_DoubleClick);

            this.CMS.Opening += new System.ComponentModel.CancelEventHandler(this.CMS_Opening);
        }

        private void setTags()
        {
            this.setMatrix.Tag = ControlNames.Matrices;
            this.setGeometry.Tag = ControlNames.Geometries; //set geometries
            this.setVials.Tag = ControlNames.Vials; // set vials
            this.setMonitor.Tag = ControlNames.Monitors;
            this.setStandards.Tag = ControlNames.Standards;
            this.setSubSamples.Tag = ControlNames.Samples;
            this.setIrradCh.Tag = ControlNames.Channels;
            this.halflife.Tag = ControlNames.MonitorsFlags;
            this.setRabbit.Tag = ControlNames.Rabbits; //for rabitt capsules channel
        }

        private void setItems()
        {
            this.halflife.Click += LIMSUI.SetItem;
            this.setGeometry.Click += LIMSUI.SetItem;
            this.setVials.Click += LIMSUI.SetItem;
            this.setMonitor.Click += LIMSUI.SetItem;
            this.setStandards.Click += LIMSUI.SetItem;
            this.setSubSamples.Click += LIMSUI.SetItem;
            this.setMatrix.Click += LIMSUI.SetItem;
            this.setIrradCh.Click += LIMSUI.SetItem;
            this.setRabbit.Click += LIMSUI.SetItem;
        }
    }
}