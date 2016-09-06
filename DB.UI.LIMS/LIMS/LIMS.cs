using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using DB.Interfaces;
using Rsx.DGV;

namespace DB.UI
{
    public partial class LIMS : Form
    {
        public static class uc
        {
            public const string Vials = "Vials";
            public const string Geometries = "Geometries";
            public const string Matrices = "Matrices";
            public const string Detectors = "Detectors";
            public const string Monitors = "Monitors";
            public const string Standards = "Standards";
            public const string SubSamples = "Sub Samples";
            public const string Samples = "Samples";
            public const string Irradiations = "Irradiations";
            public const string Channels = "Channels";
            public const string Capsules = "Capsules";
            public const string Orders = "Orders";
            public const string MonitorsFlags = "Monitors Flags";
        }

        public static void SwapLinaa(ref LINAA inpu, ref LINAA output)
        {
            if (output != null)
            {
                output.Dispose();
            }
            output = null;
            output = inpu;
        }

        public static DB.LINAA Linaa;

        public static Interface Interface
        {
            get
            {
                object db = Linaa;
                return new Interface(ref db);
            }
        }

        private static Rsx.DGV.IFind IFind;

        public static LIMS Form = null;

        /// <summary>
        /// Initializes the LIMS system
        /// </summary>
        /// <param name="set">LIMS dataset</param>
        /// <param name="ucControls">Controls HashTable of controls loaded</param>

        public static void Explore()
        {
            System.Data.DataSet set = Linaa;
            VTools.Explorer explorer = new VTools.Explorer(ref set);

            //   Rsx.DGV.Control.Refresher refresher = explorer.RefreshTable;
            Rsx.DGV.Control ctr = new Rsx.DGV.Control(explorer.RefreshTable, Interface.IReport.Msg, ref IFind);
            DataGridView[] dgv = new DataGridView[] { explorer.DGV };

            ctr.SetContext("Explorer", ref dgv, LIMS.Form.CMS);

            ctr.CreateEvents(ref dgv);

            ctr.SaveMethod = Linaa.Save;
            ctr.SetSaver(explorer.saveBtton);

            explorer.Box.Text = "Exceptions";
            Auxiliar form = new Auxiliar();

            form.Populate(explorer);
            form.Text = "DataSet Explorer";
            form.Show();

            controls.Add(explorer);
        }

        private static IList<object> controls = null;

        public static IList<object> UserControls
        {
            get { return controls; }
            set { controls = value; }
        }

        public LIMS()
        {
            InitializeComponent();

            LIMS.IFind = new ucSearch();

            //  LIMS.CMStrip = this.CMS;

            this.halflife.Click += LIMS.SetItem;
            this.setGeometry.Click += LIMS.SetItem;
            this.setIrradiation.Click += LIMS.SetItem;
            this.setMonitor.Click += LIMS.SetItem;
            this.setStandards.Click += LIMS.SetItem;
            this.setSubSamples.Click += LIMS.SetItem;
            this.setMatrix.Click += LIMS.SetItem;
            this.setIrradCh.Click += LIMS.SetItem;

            this.setMatrix.Tag = LIMS.uc.Matrices;
            this.setGeometry.Tag = LIMS.uc.Geometries;
            this.setIrradiation.Tag = LIMS.uc.Vials;
            this.setMonitor.Tag = LIMS.uc.Monitors;
            this.setStandards.Tag = LIMS.uc.Standards;
            this.setSubSamples.Tag = LIMS.uc.Samples;
            this.setIrradCh.Tag = LIMS.uc.Channels;
            this.halflife.Tag = LIMS.uc.MonitorsFlags;

            this.updateIrradiationDateToolStripMenuItem.Click += UpdateIrradiationDates;
            this.unlockProtectedCellsToolStripMenuItem.Click += UnLockProtectedCells;
        }

        private void UpdateIrradiationDates(object sender, System.EventArgs e)
        {
            LIMS.Linaa.UpdateIrradiationDates();
        }

        private void UnLockProtectedCells(object sender, System.EventArgs e)
        {
            DataGridView dgv = (this.CMS.SourceControl as DataGridView);

            System.Data.DataTable dt = Rsx.DGV.Control.GetDataSource<System.Data.DataTable>(ref dgv);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                bool IsreadOnly = dt.Columns[col.DataPropertyName].ReadOnly;

                if (IsreadOnly)
                {
                    col.ReadOnly = unlockProtectedCellsToolStripMenuItem.Checked;
                }
            }
        }

        private void CMS_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            string header = (cms.SourceControl as DataGridView).Name;
            DataGridView dgv = cms.SourceControl as DataGridView;

            //set all items visibility as false except for samples
            IEnumerable<System.Windows.Forms.ToolStripMenuItem> items = this.CMS.Items.OfType<System.Windows.Forms.ToolStripMenuItem>().ToList();
            bool visible = true;
            if (header.CompareTo(LIMS.uc.SubSamples) != 0) visible = false;
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
                case uc.Geometries:
                    {
                        this.setMatrix.Visible = true;
                        this.setIrradiation.Text = "Set Vial";
                        this.setIrradiation.Visible = true;

                        break;
                    }
                case uc.Vials:
                    {
                        this.setMatrix.Visible = true;

                        break;
                    }

                case uc.Detectors:
                    {
                        break;
                    }
                case uc.Monitors:
                    {
                        this.setStandards.Visible = true;
                        this.setGeometry.Visible = true;
                        this.halflife.Visible = true;

                        this.updateIrradiationDateToolStripMenuItem.Visible = true;

                        break;
                    }
                case uc.Standards:
                    {
                        this.setMatrix.Visible = true;
                        break;
                    }
                case uc.SubSamples:
                    {
                        Rsx.DGV.Control cv = dgv.Tag as Rsx.DGV.Control;
                        DB.UI.ISubSamples iS = (DB.UI.ISubSamples)cv.UsrControl;
                        this.halflife.Visible = false;
                        this.setIrradCh.Visible = false;
                        this.updateIrradiationDateToolStripMenuItem.Visible = false;
                        this.setIrradiation.Text = "Set Irr. Container";
                        this.shareTirr.Click -= iS.ShareTirr;
                        this.predictToolStripMenuItem.Click -= iS.Predict;
                        this.shareTirr.Click += iS.ShareTirr;
                        this.predictToolStripMenuItem.Click += iS.Predict;
                        break;
                    }
                case uc.MonitorsFlags:
                    {
                        break;
                    }
                case uc.Samples:
                    {
                        break;
                    }
                case uc.Irradiations:
                    {
                        this.setIrradCh.Visible = true;
                        break;
                    }

                case uc.Orders:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        private void Modules_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string module = Modules.FocusedItem.Text;

            UserControl control = CreateUI(module);

            if (control == null) return;

            Auxiliar form = new Auxiliar();
            form.MaximizeBox = true;
            form.Populate(control);
            form.Text = Modules.FocusedItem.Group.Header + " - " + module;
            form.Show();
        }

        public static void SaveWorkspaceXML(string file)
        {
            try
            {
                //   this.Linaa.ToDoRes.Clear();
                //  this.Linaa.ToDoResAvg.Clear();
                //  this.Linaa.ToDoAvg.Clear();
                //  this.Linaa.ToDoData.Clear();

                Linaa.WriteXml(file, System.Data.XmlWriteMode.WriteSchema);
                Linaa.Msg("Workspace was saved on " + file, "Saved Workspace!", true);
            }
            catch (SystemException ex)
            {
                Linaa.Msg("Workspace was NOT saved on " + file, "Not Saved Workspace!", false);
            }
        }

        public static void SetItem(object sender, System.EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            ContextMenuStrip cms = tsmi.GetCurrentParent() as ContextMenuStrip;
            DataGridView dgv = cms.SourceControl as DataGridView;
            string controlHeader = string.Empty;
            if (tsmi.Tag is System.Data.DataColumn)
            {
                System.Data.DataColumn col = tsmi.Tag as System.Data.DataColumn;
                //     LINAA.DetectorsAbsorbersRow abs = ((LINAA.DetectorsAbsorbersRow)((System.Data.DataRowView)dgv.CurrentRow.DataBoundItem).Row);
                DataGridViewColumn dgvcol = dgv.Columns.OfType<DataGridViewColumn>().FirstOrDefault(c => c.DataPropertyName.Equals(col.ColumnName));
                dgv.CurrentCell = dgv[dgvcol.Index, dgv.CurrentRow.Index];
                controlHeader = uc.Matrices;
            }
            else controlHeader = tsmi.Tag as string;

            UserControl control = LIMS.CreateUI(controlHeader);

            if (control == null) return;

            Rsx.DGV.Control cv = (Rsx.DGV.Control)control.Tag;
            DataGridView[] from = cv.DGVs;
            if (from == null || from.Count() == 0)
            {
                control.Dispose();
                return;
            }

            IPickerForm frm = new PickerForm();
            frm.IPicker = new Picker(ref dgv, ref from, false);  //the picker algorithm class
            frm.Module = control;    //this will show the module to pick from
        }

        public static UserControl CreateUI(string controlHeader)
        {
            return CreateUI(controlHeader, null);
        }

        /*

             System.Windows.Forms.UserControl control = LIMS.CreateUI("Matrices");
             System.Windows.Forms.UserControl control2 = LIMS.CreateUI("Vials");
             System.Windows.Forms.UserControl control3 = LIMS.CreateUI("Geometries");

             this.matTab.Content = control;
             this.vialTab.Content = control2;
             this.geoTab.Content = control3;

                 <TabControl HorizontalAlignment="Left" Name="MainTab" VerticalAlignment="Stretch" Width="527" HorizontalContentAlignment="Center" VerticalContentAlignment="Stretch">
       <TabItem Header="Matrices" Name="matTab"></TabItem>
       <TabItem Header="Vials" Name="vialTab" />
       <TabItem Header="Geometries" Name="geoTab" />
   </TabControl>

    */

        public static UserControl CreateUI(string controlHeader, object[] args)
        {
            UserControl control = null;
            Rsx.DGV.Control.Refresher refresher = null;
            Rsx.DGV.Control.Loader loader = null;
            System.EventHandler postRefresh = null;
            System.Windows.Forms.DataGridViewRowPostPaintEventHandler rowPostpainter = null;
            System.Windows.Forms.DataGridViewRowPrePaintEventHandler rowPrepainter = null;
            System.Windows.Forms.DataGridViewCellPaintingEventHandler cellpainter = null;
            Rsx.DGV.Control.CellPaintChecker shouldpaintCell = null;
            Rsx.DGV.Control.RowPostPaintChecker shouldpostpaintRow = null;
            Rsx.DGV.Control.RowPrePaintChecker shouldprepaintRow = null;
            Rsx.DGV.Control.Adder addedRow = null;
            System.EventHandler preRefresh = null;

            switch (controlHeader)
            {
                case uc.Geometries:
                    {
                        control = (UserControl)new ucGeometry();
                        refresher = Interface.IPopulate.IGeometry.PopulateGeometry;
                        break;
                    }
                case uc.Vials:
                    {
                        control = (UserControl)new ucVialType();
                        refresher = Interface.IPopulate.IGeometry.PopulateVials;
                        break;
                    }
                case uc.Matrices:
                    {
                        ucMatrix mat = new ucMatrix(ref LIMS.Linaa);
                        control = (UserControl)mat;
                        refresher = Interface.IPopulate.IGeometry.PopulateMatrix;
                        preRefresh = mat.PreRefresh;
                        postRefresh = mat.PostRefresh;
                        break;
                    }
                case uc.Detectors:
                    {
                        control = (UserControl)new ucDetectors();
                        break;
                    }
                case uc.Monitors:
                    {
                        ucMonitors mon = new ucMonitors();
                        control = (UserControl)mon;
                        refresher = Interface.IPopulate.ISamples.PopulateMonitors;
                        postRefresh = mon.PostRefresh;
                        shouldpaintCell = mon.ShouldPaintCell;
                        cellpainter = mon.PaintCell;
                        loader = Interface.IPopulate.ISamples.LoadMonitorsFile;
                        addedRow = mon.RowAdded;

                        break;
                    }
                case uc.Standards:
                    {
                        control = (UserControl)new ucStandards();
                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case uc.SubSamples:
                    {
                        ISubSamples ucSS = new ucSubSamples();

                        control = (UserControl)ucSS;

                        refresher = ucSS.RefreshSubSamples;
                        cellpainter = ucSS.PaintCells;
                        shouldpaintCell = ucSS.ShouldPaint;
                        addedRow = ucSS.RowAdded;
                        break;
                    }
                case uc.MonitorsFlags:
                    {
                        control = (UserControl)new ucMonitorsFlags();
                        refresher = Interface.IPopulate.ISamples.PopulateMonitorFlags;

                        break;
                    }
                case uc.Samples:
                    {
                        control = (UserControl)new ucSamples();
                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case uc.Irradiations:
                    {
                        control = (UserControl)new ucIrradiationsRequests();
                        refresher = Interface.IPopulate.IIrradiations.PopulateIrradiationRequests;
                        break;
                    }
                case uc.Channels:
                    {
                        control = (UserControl)new ucChannels();
                        refresher = Interface.IPopulate.IIrradiations.PopulateChannels;
                        break;
                    }

                case uc.Orders:
                    {
                        control = (UserControl)new ucOrders();
                        refresher = Interface.IPopulate.IOrders.PopulateOrders;
                        break;
                    }
                default:
                    break;
            }

            if (control == null)
            {
                LIMS.Interface.IReport.Msg("Failed to generate the User interface", "Could not load " + controlHeader, false);
                return null;
            }
            //create the DGV controller... and
            //set methods...
            Rsx.DGV.Control cv = new Rsx.DGV.Control(refresher, Linaa.Msg, ref LIMS.IFind);
            cv.LoadMethod = loader;
            cv.PostRefresh = postRefresh;
            cv.PreRefresh = preRefresh;
            cv.PaintCellsMethod = cellpainter;
            cv.ShouldPaintCellMethod = shouldpaintCell;
            cv.PostPaintRowsMethod = rowPostpainter;
            cv.ShouldPostPaintRowMethod = shouldpostpaintRow;
            cv.PrePaintRowsMethod = rowPrepainter;
            cv.ShouldPrePaintRowMethod = shouldprepaintRow;
            cv.RowAddedMethod = addedRow;
            cv.SaveMethod = LIMS.Interface.IStore.Save;

            DataGridView[] dgvs = Rsx.Dumb.GetChildControls<DataGridView>(control).ToArray();
            if (dgvs.Count() == 0) return null;

            cv.SetContext(controlHeader, ref dgvs, LIMS.Form.CMS);

            //create events
            cv.CreateEvents(ref dgvs);

            dgvs = null;

            ToolStripButton[] items = Rsx.Dumb.GetChildControls<ToolStrip>(control).SelectMany(o => o.Items.OfType<ToolStripButton>()).ToArray();

            cv.CreateEvents(ref items);

            items = null;

            control.Tag = cv;   //sets the CV as a TAG for the control
            cv.UsrControl = control; //set the control as a tag for the CV

            controls.Add(control);

            return control;
        }

        private void LIMS_Load(object sender, System.EventArgs e)
        {
            //  if (Screen.AllScreens.Length == 2)
            {
                //  this.Location = new System.Drawing.Point(Screen.AllScreens[1].WorkingArea.X + 100, Screen.AllScreens[1].WorkingArea.Y + 600);
            }
        }

        private void LIMS_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void refreshT_Click(object sender, EventArgs e)
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
    }
}