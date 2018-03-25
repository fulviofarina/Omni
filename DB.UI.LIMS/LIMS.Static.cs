using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using Rsx.Dumb;
using VTools;
using static Rsx.DGV.Control;

namespace DB.UI
{
    public partial class LIMS
    {
        public static LIMS Form = null;
        public static Rsx.DGV.IFind IFind = null;
        public static Interface Interface = null;

        /// <summary>
        /// only kept for compatibility with k0-X, remove when possible
        /// make necessary changes
        /// </summary>
        public static LINAA Linaa = null;

        protected internal static Form aboutBox;

        public static IList<object> UserControls
        {
            get
            {
                return Creator.UserControls;
            }
        }
        public static void CreateForm(string title, ref UserControl control, bool show = true)
        {
            if (control == null) return;
            Auxiliar form = new Auxiliar();
            form.MaximizeBox = true;
            form.Populate(control);
            form.Text = title;
            form.Visible = show;
        }

        public static void CreateLIMS(ref Form _aboutbox)
        {
            aboutBox = _aboutbox;
            Interface.IReport.Msg("Starting LIMS", "Starting...");
            Linaa = Interface.Get();
            Form = new LIMS(); //make a new UI LIMS
            Form.ShowInTaskbar = false;
            Form.Opacity = 0;

            //FIRST THIS IN CASE i NEED TO RESTART AGAIN IN SQL INSTALL
            Interface.IReport.Msg("LIMS Set up", "LIMS OK");
        }




        public static UserControl CreateUI(string controlHeader, object[] args = null, bool noDGVControl = false)
        {
            UserControl control = null;
            Rsx.DGV.Control.Refresher refresher = null;
            Rsx.DGV.Control.Loader loader = null;
            System.EventHandler postRefresh = null;
            DataGridViewRowPostPaintEventHandler rowPostpainter = null;
            DataGridViewRowPrePaintEventHandler rowPrepainter = null;
            DataGridViewCellPaintingEventHandler cellpainter = null;
            Rsx.DGV.Control.CellPaintChecker shouldpaintCell = null;
            Rsx.DGV.Control.RowPostPaintChecker shouldpostpaintRow = null;
            Rsx.DGV.Control.RowPrePaintChecker shouldprepaintRow = null;
            Rsx.DGV.Control.AdderEraser addedRow = null;
            Rsx.DGV.Control.AdderEraser deleteRow = null;
            System.EventHandler preRefresh = null;

            createControl(controlHeader, ref control, ref refresher, ref loader, ref postRefresh, ref cellpainter, ref shouldpaintCell, ref addedRow, ref preRefresh, ref deleteRow);

            if (control == null)
            {
                Interface.IReport.Msg("Failed to generate the User interface", "Could not load " + controlHeader, false);
                return null;
            }
            Creator.UserControls.Add(control);

            if (noDGVControl) return control;

            //create the DGV controller... and
            //set methods...
            Rsx.DGV.Control cv = new Rsx.DGV.Control(refresher, Interface.IReport.Msg, ref IFind);
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
            cv.RowDeletedMethod = deleteRow;
            cv.SaveMethod = LIMS.Interface.IStore.Save;

            DataGridView[] dgvs = UIControl.GetChildControls<DataGridView>(control).ToArray();

            if (dgvs.Count() != 0)
            {
                cv.SetContext(controlHeader, ref dgvs, Form.CMS);

                //create events
                cv.CreateDGVEvents();
                // dgvs = null;
            }

            ToolStripButton[] items = UIControl.GetChildControls<ToolStrip>(control)
                .SelectMany(o => o.Items.OfType<ToolStripButton>()).ToArray();
            cv.CreateButtonEvents(ref items);
            items = null;

            control.Tag = cv;   //sets the CV as a TAG for the control
            cv.UsrControl = control; //set the control as a tag for the CV

            return control;
        }

        public static void Explore()
        {
            DataSet set = Interface.Get();
            Explorer explorer = new Explorer(ref set);

            // Rsx.DGV.Control.Refresher refresher = explorer.RefreshTable;
            Rsx.DGV.Control ctr = new Rsx.DGV.Control(explorer.RefreshTable, Interface.IReport.Msg, ref IFind);
            DataGridView[] dgv = new DataGridView[] { explorer.DGV };

            ctr.SetContext("Explorer", ref dgv, LIMS.Form.CMS);
            ctr.CreateDGVEvents();
            ctr.SaveMethod = Linaa.Save;
            ctr.SetSaver(explorer.saveBtton);

            Creator.UserControls.Add(explorer);

            Auxiliar form = new Auxiliar();
            form.Populate(explorer);
            form.Text = "DataSet Explorer";
            form.Show();

            explorer.Box.Text = "Exceptions";
        }

        public static void SaveWorkspaceXML(string file)
        {
            try
            {
                // this.Linaa.ToDoRes.Clear(); this.Linaa.ToDoResAvg.Clear();
                // this.Linaa.ToDoAvg.Clear(); this.Linaa.ToDoData.Clear();

                Linaa.WriteXml(file, System.Data.XmlWriteMode.WriteSchema);
                Interface.IReport.Msg("Workspace was saved on " + file, "Saved Workspace!", true);
            }
            catch (SystemException)
            {
                Interface.IReport.Msg("Workspace was NOT saved on " + file, "Not Saved Workspace!", false);
            }
        }

        /// <summary>
        /// For setting the item on the Picker Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        public static void SetItem(object sender, EventArgs e)
        {
            //get the dgv associated to the tsmi
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            ContextMenuStrip cms = tsmi.GetCurrentParent() as ContextMenuStrip;
            DataGridView dgv = cms.SourceControl as DataGridView;

            string controlHeader = string.Empty;
            //the tag is a column
            if (tsmi.Tag is DataColumn)
            {
                DataColumn col = tsmi.Tag as DataColumn;
                // LINAA.DetectorsAbsorbersRow abs = ((LINAA.DetectorsAbsorbersRow)((System.Data.DataRowView)dgv.CurrentRow.DataBoundItem).Row);

                //GET the DGV COLUMN
                DataGridViewColumn dgvcol = dgv.Columns
                    .OfType<DataGridViewColumn>()
                    .FirstOrDefault(c => c.DataPropertyName.Equals(col.ColumnName));

                dgv.CurrentCell = dgv[dgvcol.Index, dgv.CurrentRow.Index];
                controlHeader = ControlNames.Matrices;
            }
            else controlHeader = tsmi.Tag as string; //the tag is a string

            //to distinguish between Rabbit and Vial relation and UserInterface (Control)
            string controlToSend = controlHeader;
            if (controlToSend == ControlNames.Rabbits)
            {
                controlToSend = ControlNames.Vials;
            }

            UserControl control = CreateUI(controlToSend);

            if (control == null) return;

            //take the control Tag as the DGV.Control
            DataGridView[] from = ((Rsx.DGV.Control)control.Tag).DGVs;
            //has no dgvs to pick from?
            if (from == null || from.Count() == 0)
            {
                control.Dispose();
                return;
            }

            int rel = 0;
            if (controlHeader == ControlNames.Rabbits) rel = 1; //for rabitt capsules channel)

            IPickerForm frm = new PickerForm();
            //pick from the following from dgvs, to this dgv,
            LINAA.ExceptionsDataTable exsDT = Interface.IDB.Exceptions;
            frm.IPicker = new Picker(ref dgv, ref from, false, ref exsDT, rel);  //the picker algorithm class
            frm.Module = control;    //this will show the module to pick from
        }
        protected internal static void createControl(string controlHeader, ref UserControl control, ref Rsx.DGV.Control.Refresher refresher, ref Rsx.DGV.Control.Loader stringOrFileLoader, ref EventHandler postRefresh, ref DataGridViewCellPaintingEventHandler cellpainter, ref Rsx.DGV.Control.CellPaintChecker shouldpaintCell, ref AdderEraser addedRow, ref EventHandler preRefresh, ref AdderEraser deletedRow)
        {
            switch (controlHeader)
            {
                case ControlNames.Geometries:
                    {
                        ucGeometry ucGeometry = new ucGeometry();
                        control = (UserControl)ucGeometry;
                        ucGeometry.Set(ref Interface);
                        refresher = Interface.IPopulate.IGeometry.PopulateGeometry;
                        break;
                    }
                    case ControlNames.SpecNavigator:
                    {
                        ucHyperLab hl = new ucHyperLab();
                        ISpecPreferences prefes = GetPreferences<ISpecPreferences>();
                        VTools.IOptions options = GetOptions(0);
                        options.PreferencesClick += delegate
                        {
                            GetPreferences<ISpecPreferences>(true);
                        };

                        hl.Set(ref Interface);
                        hl.Set(ref prefes);
                        hl.Set(ref options);
                        //   UserControl control = this;
                        hl.Dock = DockStyle.Fill;
                        // form.AutoSizeMode = AutoSizeMode.GrowOnly;
                        // form.AutoSize = true;
                        hl.AutoSizeMode = AutoSizeMode.GrowOnly;
                        hl.AutoSize = true;

                        control = (UserControl)hl;
                        break;
                    }
                case ControlNames.Vials:
                    {
                        ucVialType ucVialType = new ucVialType();
                        control = (UserControl)ucVialType;
                        ucVialType.Set(ref Interface);
                        refresher = Interface.IPopulate.IGeometry.PopulateVials;
                        break;
                    }
                case ControlNames.Matrices:
                    {
                        CreateMatrixApplication(out control, out refresher);

                        // preRefreshref = mat.PreRefresh; postRefresh = mat.PostRefresh;
                        break;
                    }
                case ControlNames.Detectors:
                    {
                        ucDetectors ucDetectors = new ucDetectors();
                        ucDetectors.Set(ref Interface);
                        control = (UserControl)ucDetectors;

                        break;
                    }
                case ControlNames.Monitors:
                    {
                        ucMonitors mon = new ucMonitors();
                        mon.Set(ref Interface);
                        control = (UserControl)mon;
                        refresher = Interface.IPopulate.ISamples.PopulateMonitors;
                        postRefresh = mon.PostRefresh;
                        shouldpaintCell = mon.ShouldPaintCell;
                        cellpainter = mon.PaintCell;
                        stringOrFileLoader = Interface.IPopulate.ISamples.PopulatedMonitors;
                        addedRow = mon.RowAdded;

                        break;
                    }
                case ControlNames.Standards:
                    {
                        ucStandards ucStandards = new ucStandards();
                        control = ucStandards as UserControl;
                        ucStandards.Set(ref Interface);

                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case ControlNames.SubSamples:
                    {
                        ucSubSamples ucSubSamples = new ucSubSamples();

                        ucSubSamples.ucContent = CreateUI(ControlNames.SubSamplesContent) as ucSSContent;

                        ucSubSamples.Set(ref Interface);
                        // ucSubSamples.ucContent.Set(ref LIMS.Interface);

                        cellpainter = ucSubSamples.ucContent.PaintCells;
                        shouldpaintCell = ucSubSamples.ucContent.ShouldPaint;

                        // refresher = ucSubSamples.projectbox.Refresher; addedRow =
                        // ucSubSamples.RowAdded; deletedRow = ucSubSamples.RowDeleted;
                        control = (UserControl)ucSubSamples;

                        break;
                    }
                case ControlNames.SubSamplesContent:
                    {
                        ucSSContent ucSSContent = new ucSSContent();

                        // ucSSContent.Set(ref LIMS.Interface);

                        cellpainter = ucSSContent.PaintCells;
                        shouldpaintCell = ucSSContent.ShouldPaint;
                        // refresher = ucSSContent.RefreshSubSamples; addedRow = ucSSContent.RowAdded;
                        control = ucSSContent as UserControl;
                        break;
                    }
                case ControlNames.MonitorsFlags:
                    {
                        ucMonitorsFlags ucMonitorsFlags = new ucMonitorsFlags();
                        control = ucMonitorsFlags as UserControl;
                        ucMonitorsFlags.Set(ref Interface);

                        refresher = Interface.IPopulate.ISamples.PopulateMonitorFlags;

                        break;
                    }
                case ControlNames.Samples:
                    {
                        ucSamples ucSamples = new ucSamples();
                        control = ucSamples as UserControl;
                        ucSamples.Set(ref Interface);
                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case ControlNames.Irradiations:
                    {
                        ucIrradiationsRequests ucIrr = new ucIrradiationsRequests();
                        ucIrr.Set(ref Interface);
                        control = (UserControl)ucIrr;
                        addedRow = ucIrr.RowAdded;
                        cellpainter = ucIrr.CellPainting;
                        shouldpaintCell = ucIrr.ShouldPaintCell;
                        refresher = Interface.IPopulate.IIrradiations.PopulateIrradiationRequests;
                        break;
                    }
                case ControlNames.Channels:
                    {
                        ucChannels ucChannels = new ucChannels();
                        control = ucChannels as UserControl;
                        ucChannels.Set(ref Interface);

                        refresher = Interface.IPopulate.IIrradiations.PopulateChannels;
                        break;
                    }

                case ControlNames.Orders:
                    {
                        ucOrders ucOrders = new ucOrders();
                        control = ucOrders as UserControl;
                        ucOrders.Set(ref Interface);

                        refresher = Interface.IPopulate.IOrders.PopulateOrders;
                        break;
                    }
                // case ControlNames.Units: { ucSSFResults ucUnit = new ucSSFResults(); control =
                // ucUnit as UserControl; ucUnit.Set(ref Interface); Rsx.DGV.IFind finder = new
                // Rsx.DGV.ucSearch(); Rsx.DGV.IDGVControl ctrl = cv; // new Rsx.DGV.Control(null,
                // Interface.IReport.Msg, ref finder);

                //  ctrl.ICreate.DGVs = new DataGridView[] { this.unitDGV, this.SSFDGV };
                // Saver = Interface.IStore.Save;
                // ctrl.ICreate.UsrControl = this;
                //ctrl.ICreate.CreateDGVEvents();

                // refresher = Interface.IPopulate.IOrders.PopulateOrders; break; }
                default:
                    break;
            }
        }

        protected internal static void createPreference(string controlHeader, ref UserControl control)
        {
            switch (controlHeader)
            {
             
                case ControlNames.Preferences:
                    {
                        IPreferences ucPreferences = new ucPreferences();
                        ucPreferences.IMain.Set(ref Interface);
                        ucPreferences.ISSF.Set(ref Interface);

                        control = (UserControl)ucPreferences;

                        break;
                    }
                case ControlNames.XCOMPreferences:
                    {
                        IXCOMPreferences ucPreferences = new ucXCOMPreferences();
                        ucPreferences.Set(ref Interface);
                        control = (UserControl)ucPreferences;

                        break;
                    }
                case ControlNames.SpecPreferences:
                    {
                        ISpecPreferences ucPreferences = new ucSpecPreferences();
                        ucPreferences.Set(ref Interface);
                        control = (UserControl)ucPreferences;

                        break;
                    }
              
                default:
                    break;
            }
        }

        protected internal static void formClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = Creator.Close();
            if (!e.Cancel)
            {
                Application.ExitThread();
            }
        }
    }
}