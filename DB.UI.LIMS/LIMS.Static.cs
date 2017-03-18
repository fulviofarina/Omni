using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class LIMS
    {
        public static LIMS Form = null;

        public static Interface Interface = null;

        /// <summary>
        /// database
        /// </summary>
        public static LINAA Linaa = null;

        private static Rsx.DGV.IFind IFind = null;

        public static IList<object> UserControls = null;

        public static UserControl CreateUI(string controlHeader)
        {
            return CreateUI(controlHeader, null);
        }

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
                case ControlNames.Geometries:
                    {
                        control = (UserControl)new ucGeometry();
                        refresher = Interface.IPopulate.IGeometry.PopulateGeometry;
                        break;
                    }
                case ControlNames.Vials:
                    {
                        control = (UserControl)new ucVialType();
                        refresher = Interface.IPopulate.IGeometry.PopulateVials;
                        break;
                    }
                case ControlNames.Matrices:
                    {
                        ucMatrix mat = new ucMatrix(ref LIMS.Linaa);
                        control = (UserControl)mat;
                        refresher = Interface.IPopulate.IGeometry.PopulateMatrix;
                        preRefresh = mat.PreRefresh;
                        postRefresh = mat.PostRefresh;
                        break;
                    }
                case ControlNames.Detectors:
                    {
                        control = (UserControl)new ucDetectors();
                        break;
                    }
                case ControlNames.Monitors:
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
                case ControlNames.Standards:
                    {
                        control = (UserControl)new ucStandards();
                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case ControlNames.SubSamples:
                    {
                        ISubSamples ucSS = new ucSubSamples();

                        control = (UserControl)ucSS;

                        refresher = ucSS.RefreshSubSamples;
                        cellpainter = ucSS.PaintCells;
                        shouldpaintCell = ucSS.ShouldPaint;
                        addedRow = ucSS.RowAdded;
                        break;
                    }
                case ControlNames.MonitorsFlags:
                    {
                        control = (UserControl)new ucMonitorsFlags();
                        refresher = Interface.IPopulate.ISamples.PopulateMonitorFlags;

                        break;
                    }
                case ControlNames.Samples:
                    {
                        control = (UserControl)new ucSamples();
                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case ControlNames.Irradiations:
                    {
                        control = (UserControl)new ucIrradiationsRequests();
                        refresher = Interface.IPopulate.IIrradiations.PopulateIrradiationRequests;
                        break;
                    }
                case ControlNames.Channels:
                    {
                        control = (UserControl)new ucChannels();
                        refresher = Interface.IPopulate.IIrradiations.PopulateChannels;
                        break;
                    }

                case ControlNames.Orders:
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

            UserControls.Add(control);

            return control;
        }

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

            UserControls.Add(explorer);
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
                controlHeader = ControlNames.Matrices;
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

        public static void SwapLinaa(ref LINAA inpu, ref LINAA output)
        {
            if (output != null)
            {
                output.Dispose();
            }
            output = null;
            output = inpu;
        }
    }
}