using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;


namespace DB.UI
{
    public partial class LIMS
    {
        //  public static System.Collections.Generic.IList<object> UserControls;

        public static void ShowToUser()
        {

            LIMS.Form.Visible = true;
            LIMS.Form.Opacity = 100;
            LIMS.Form.BringToFront();
        }

        public static void ShowPreferences(bool show)
        {

            try
            {
                UserControl c = LIMS.UserControls.OfType<ucPreferences>().FirstOrDefault();
               
                c.ParentForm.Visible = show;
                c.ParentForm.Opacity = 100;
                c.ParentForm.BringToFront();
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
            }



        }
        public static T FindLastControl<T>(string name)
        {
            Func<T, bool> finder = o =>
            {
                bool found = false;
                UserControl os = o as UserControl;
                if (os.Name.ToUpper().CompareTo(name) == 0) found = true;
                return found;
            };
            return UserControls.OfType<T>().LastOrDefault(finder);
        }

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

        public static void CreateForm(string title, ref UserControl control)
        {
            if (control == null) return;
            Auxiliar form = new Auxiliar();
            form.MaximizeBox = true;
            form.Populate(control);
            form.Text = title;
            form.Show();
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

            createControl(controlHeader, ref control, ref refresher, ref loader, ref postRefresh, ref cellpainter, ref shouldpaintCell, ref addedRow, ref preRefresh);

            if (control == null)
            {
                LIMS.Interface.IReport.Msg("Failed to generate the User interface", "Could not load " + controlHeader, false);
                return null;
            }
            UserControls.Add(control);

            //create the DGV controller... and
            //set methods...
            Rsx.DGV.Control cv = new Rsx.DGV.Control(refresher, Interface.IReport.Msg, ref LIMS.IFind);
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

            if (dgvs.Count() != 0)
            {
                cv.SetContext(controlHeader, ref dgvs, LIMS.Form.CMS);

                //create events
                cv.CreateEvents(ref dgvs);
                dgvs = null;
            }

            ToolStripButton[] items = Rsx.Dumb.GetChildControls<ToolStrip>(control)
                .SelectMany(o => o.Items.OfType<ToolStripButton>()).ToArray();
            cv.CreateEvents(ref items);
            items = null;

            control.Tag = cv;   //sets the CV as a TAG for the control
            cv.UsrControl = control; //set the control as a tag for the CV

            return control;
        }

        private static void createControl(string controlHeader, ref UserControl control, ref Rsx.DGV.Control.Refresher refresher, ref Rsx.DGV.Control.Loader loader, ref EventHandler postRefresh, ref DataGridViewCellPaintingEventHandler cellpainter, ref Rsx.DGV.Control.CellPaintChecker shouldpaintCell, ref Rsx.DGV.Control.Adder addedRow, ref EventHandler preRefresh)
        {
            switch (controlHeader)
            {
                case ControlNames.Geometries:
                    {
                        ucGeometry ucGeometry = new ucGeometry();
                        control = (UserControl)ucGeometry;
                        ucGeometry.Set(ref LIMS.Interface);

                        refresher = Interface.IPopulate.IGeometry.PopulateGeometry;
                        break;
                    }
                case ControlNames.Vials:
                    {
                        ucVialType ucVialType = new ucVialType();
                        control = (UserControl)ucVialType;
                        ucVialType.Set(ref LIMS.Interface);
                        refresher = Interface.IPopulate.IGeometry.PopulateVials;
                        break;
                    }
                case ControlNames.Matrices:
                    {
                        ucMatrix mat = new ucMatrix();
                        mat.Set(ref LIMS.Interface);
                        control = (UserControl)mat;
                        refresher = Interface.IPopulate.IGeometry.PopulateMatrix;
                        preRefresh = mat.PreRefresh;
                        postRefresh = mat.PostRefresh;
                        break;
                    }
                case ControlNames.Preferences:
                    {
                        ucPreferences ucPreferences = new ucPreferences();
                        ucPreferences.Set(ref LIMS.Interface);
                        control = (UserControl)ucPreferences;

                        break;
                    }
                case ControlNames.Detectors:
                    {
                        ucDetectors ucDetectors = new ucDetectors();
                        ucDetectors.Set(ref LIMS.Interface);
                        control = (UserControl)ucDetectors;

                        break;
                    }
                case ControlNames.Monitors:
                    {
                        ucMonitors mon = new ucMonitors();
                        mon.Set(ref LIMS.Interface);
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
                        ucStandards ucStandards = new ucStandards();
                        control = ucStandards as UserControl;
                        ucStandards.Set(ref LIMS.Interface);

                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case ControlNames.SubSamples:
                    {
                        ucSubSamples ucSubSamples = new ucSubSamples();

                        ucSubSamples.ucContent = CreateUI(ControlNames.SubSamplesContent) as ucSSContent;

                        ucSubSamples.Set(ref LIMS.Interface);
                        //   ucSubSamples.ucContent.Set(ref LIMS.Interface);

                        cellpainter = ucSubSamples.ucContent.PaintCells;
                        shouldpaintCell = ucSubSamples.ucContent.ShouldPaint;

                        refresher = ucSubSamples.projectbox.Refresher;
                        addedRow = ucSubSamples.RowAdded;

                        control = (UserControl)ucSubSamples;

                        break;
                    }
                case ControlNames.SubSamplesContent:
                    {
                        ucSSContent ucSSContent = new ucSSContent();

                        //     ucSSContent.Set(ref LIMS.Interface);

                        cellpainter = ucSSContent.PaintCells;
                        shouldpaintCell = ucSSContent.ShouldPaint;
                        //     refresher = ucSSContent.RefreshSubSamples;
                        //   addedRow = ucSSContent.RowAdded;
                        control = ucSSContent as UserControl;
                        break;
                    }
                case ControlNames.MonitorsFlags:
                    {
                        ucMonitorsFlags ucMonitorsFlags = new ucMonitorsFlags();
                        control = ucMonitorsFlags as UserControl;
                        ucMonitorsFlags.Set(ref LIMS.Interface);

                        refresher = Interface.IPopulate.ISamples.PopulateMonitorFlags;

                        break;
                    }
                case ControlNames.Samples:
                    {
                        ucSamples ucSamples = new ucSamples();
                        control = ucSamples as UserControl;
                        ucSamples.Set(ref LIMS.Interface);
                        refresher = Interface.IPopulate.ISamples.PopulateStandards;
                        break;
                    }
                case ControlNames.Irradiations:
                    {
                        ucIrradiationsRequests ucIrr = new ucIrradiationsRequests();
                        ucIrr.Set(ref LIMS.Interface);
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
                        ucChannels.Set(ref LIMS.Interface);

                        refresher = Interface.IPopulate.IIrradiations.PopulateChannels;
                        break;
                    }

                case ControlNames.Orders:
                    {
                        ucOrders ucOrders = new ucOrders();
                        control = ucOrders as UserControl;
                        ucOrders.Set(ref LIMS.Interface);

                        refresher = Interface.IPopulate.IOrders.PopulateOrders;
                        break;
                    }
                default:
                    break;
            }
        }

        public static bool Connections()
        {
           
            object prefe = LIMS.Interface.IPreferences.CurrentPref;
            if (prefe == null)
            {
                LIMS.Interface.IReport.Msg("Preferences object is null!", "Cannot load preferences!", false);
                return false;
            }
            Connections cform = new Connections(ref prefe);
            cform.ShowDialog();
            DialogResult res = MessageBox.Show("Save changes?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == System.Windows.Forms.DialogResult.No)
            {
                LIMS.Linaa.Preferences.RejectChanges();
                return false;
            }
            else LIMS.Interface.IPreferences.SavePreferences();
            return true;
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
                Interface.IReport.Msg("Workspace was saved on " + file, "Saved Workspace!", true);
            }
            catch (SystemException ex)
            {
                Interface.IReport.Msg("Workspace was NOT saved on " + file, "Not Saved Workspace!", false);
            }
        }

        /// <summary>
        /// For setting the item on the Picker Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SetItem(object sender, System.EventArgs e)
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
                //     LINAA.DetectorsAbsorbersRow abs = ((LINAA.DetectorsAbsorbersRow)((System.Data.DataRowView)dgv.CurrentRow.DataBoundItem).Row);

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

            UserControl control = LIMS.CreateUI(controlToSend);

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
            LINAA.ExceptionsDataTable exsDT = LIMS.Interface.IDB.Exceptions;
            frm.IPicker = new Picker(ref dgv, ref from, false, ref exsDT, rel);  //the picker algorithm class
            frm.Module = control;    //this will show the module to pick from
        }

        /// <summary>
        /// Destroys the ouput, and puts the input on the output
        /// </summary>
        /// <param name="inpu"></param>
        /// <param name="output"></param>
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