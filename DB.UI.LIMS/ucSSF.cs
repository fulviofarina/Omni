using System;
using System.Windows.Forms;

using DB.Tools;
using System.Data;
using Rsx;

using System.Collections;
using System.Collections.Generic;
using DB.Properties;
using System.Drawing;

namespace DB.UI
{
    public partial class ucSSF : UserControl
    {
        // private bool Offline = false;
        private static string mf = preFolder + "lims.xml";

        private static Environment.SpecialFolder folder = Environment.SpecialFolder.Personal;

        //   private static string matssfFolder = Settings.Default.SSFFolder;
        private static string preFolder = Environment.GetFolderPath(folder);

        private Interface Interface = null;

        //private ucMatrixSimple ucMS = null;

        public void AttachMsn(ref Control msn)
        {
            this.unitTLP.Controls.Add(msn, 0, 1);
        }

        public ucSSF(ref Interface inter)
        {
            InitializeComponent();

            //  object db = Linaa;
            Interface = inter;

            string folder = Interface.IPreferences.CurrentSSFPref.Folder;
            MatSSF.StartupPath = preFolder + folder;

            this.loop.CheckedChanged += this.checkedChanged;
            this.calcDensity.CheckedChanged += this.checkedChanged;
            this.showMatSSF.CheckedChanged += this.checkedChanged;
            this.showOther.CheckedChanged += this.checkedChanged;

            this.workOffline.CheckedChanged += this.checkedChanged;

            this.doMatSSF.CheckedChanged += this.checkedChanged;

            this.doCK.CheckedChanged += this.checkedChanged;

            this.AutoLoad.CheckedChanged += this.checkedChanged;
        }

        public void Set(ref LINAA.UnitRow unit)
        {
            //search unit
            int ind = this.ucUnit.UnitBS.Find(Interface.IDB.Unit.NameColumn.ColumnName, unit.Name);
            this.ucUnit.UnitBS.Position = ind;
        }

        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string noTemplate = DB.UI.Properties.Resources.noTemplate;
            string Error = DB.UI.Properties.Resources.Error;

            ///check if table has no rows
            DataGridView dgv = sender as DataGridView;
            if (dgv.RowCount == 0)
            {
                Interface.IReport.Msg(noTemplate, Error); //report

                return;
            }
            DataRow row = Dumb.Cast<DataRow>(dgv.Rows[e.RowIndex]);

            try
            {
                string rowWithError = DB.UI.Properties.Resources.rowWithError;

                ///has errors
                if (row.HasErrors)
                {
                    ///is not a matrix row so exit and report

                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors

                    return;
                }

                //if (sender.Equals(this.ucUnit.Controls.c))
                if (MatSSF.UNIT == null)
                {
                    this.AddUnitBn_Click(null, EventArgs.Empty);
                }
                ///find which dgv called it
                bool isChannel = row.GetType().Equals(typeof(LINAA.ChannelsRow));

                if (isChannel)
                {
                    LINAA.ChannelsRow c = row as LINAA.ChannelsRow;
                    MatSSF.UNIT.SetChannel(ref c);
                }
                else
                {
                    LINAA.VialTypeRow v = row as LINAA.VialTypeRow;
                    if (!v.IsRabbit) MatSSF.UNIT.SubSamplesRow.VialTypeRow = v;
                    else MatSSF.UNIT.SubSamplesRow.VialTypeRowByChCapsule_SubSamples = v;
                    //       MatSSF.UNIT.SetVialContainer(ref v);

                    ///  MatSSF.UNIT.SubSamplesRow.find
                }
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
            }
        }

        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>
        private void dgvUnitSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            //  DataGridViewRow r = dgv.Rows[e.RowIndex];

            string noTemplate = DB.UI.Properties.Resources.noTemplate;
            string Error = DB.UI.Properties.Resources.Error;

            if (dgv.RowCount == 0)
            {
                Interface.IReport.Msg(noTemplate, Error); //report

                return;
            }

            DataRow row = Dumb.Cast<DataRow>(dgv.Rows[e.RowIndex]);

            //     LINAA lina = (LINAA)Interface.Get();
            string rowWithError = DB.UI.Properties.Resources.rowWithError;

            try
            {
                ///check if table has no rows

                ///has errors
                if (row.HasErrors)
                {
                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                //    return;
                }

                if (this.ucUnit.Controls.Contains(sender as Control))
                {
                }
                else
                {
                }

                MatSSF.UNIT = row as LINAA.UnitRow;
                this.ucUnit.RefreshSSF();
                this.ucCC1.RefreshCC();
                this.ucVcc.RefreshVCC();
                this.ucMS.RefreshMatrix();

                if (row.HasErrors)
                {
                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                }
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
            }
        }

        private void AddUnitBn_Click(object sender, EventArgs e)
        {
            double kepi = Dumb.GetControlAs<double>(kepiB);
            double kth = Dumb.GetControlAs<double>(kthB);
            string chfg = cfgB.Text;

            MatSSF.UNIT = Interface.IDB.Unit.NewUnitRow(kepi, kth, chfg);

            this.ucUnit.RefreshSSF();
        }

        private void dgvMatrixSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;

            if (dgv.RowCount == 0) return;

            DataRow row = Dumb.Cast<DataRow>(dgv.Rows[e.RowIndex]);
            string rowWithError = DB.UI.Properties.Resources.rowWithError;
            string noTemplate = DB.UI.Properties.Resources.noTemplate;
            string Error = DB.UI.Properties.Resources.Error;

            try
            {
                ///check if table has no rows
                if (row == null)
                {
                    Interface.IReport.Msg(noTemplate, Error); //report
                                                              //  row.RowError = noTemplate;
                    return;
                }

                ///find which dgv called it
                //  bool isChannel = dgv.Equals(this.ChannelDGV);
                //  bool isMatrix = dgv.Equals(this.matrixDGV);

                ///has errors
                if (row.HasErrors)
                {
                    ///is not a matrix row so exit and report

                    LINAA.MatrixDataTable dt = Interface.IDB.Matrix;

                    string colError = row.GetColumnError(dt.MatrixCompositionColumn);
                    if (!string.IsNullOrEmpty(colError)) ///matrix content has error, exit and report

                    {
                        Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors

                        return;
                    }

                    // colError = row.GetColumnError(dt.MatrixDensityColumn);
                }

                if (MatSSF.UNIT == null)
                {
                    AddUnitBn_Click(null, EventArgs.Empty);
                }

                LINAA.MatrixRow m = row as LINAA.MatrixRow;
                MatSSF.UNIT.SubSamplesRow.MatrixID = m.MatrixID;
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
            }
        }

        private void SaveItem_Click(object sender, EventArgs e)
        {
            // this.currentUnit.LastChanged = DateTime.Now;

            this.Validate();

            this.ucUnit.UnitBS.EndEdit();

            this.ucMS.MatrixBS.EndEdit();

            this.ucCC1.EndEdit();
            this.ucVcc.EndEdit();

            //  setUnit();
            bool off = Interface.IPreferences.CurrentPref.Offline;
            Interface.IStore.SaveSSF(off, mf);
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            //Validate Binding sources
            this.ucUnit.UnitBS.EndEdit();

            this.ucMS.MatrixBS.EndEdit();

            this.ucCC1.EndEdit();
            this.ucVcc.EndEdit();

            this.ucUnit.DeLink();

            //Go to Calculations/ Units Tab
            this.Tab.SelectedTab = this.CalcTab;

            //Clear InputFile RTF Control
            compositionsbox.Clear();

            this.progress.Value = 0;

            try
            {
                bool hide = !(Interface.IPreferences.CurrentPref.ShowMatSSF);

                bool doCk = (Interface.IPreferences.CurrentPref.DoCK);

                bool doSSF = (Interface.IPreferences.CurrentPref.DoMatSSF);

                this.progress.PerformStep();

                Application.DoEvents();

                //  MatSSF.UNIT = currentUnit;
                //  MatSSF.Table = this.lINAA.MatSSF;
                MatSSF.INPUT();
                //2
                this.progress.PerformStep();
                Application.DoEvents();

                //arreglar esto
                string file = MatSSF.StartupPath + MatSSF.InputFile;
                compositionsbox.LoadFile(file, RichTextBoxStreamType.PlainText);
                //3
                this.progress.PerformStep();
                Application.DoEvents();

                bool runOk = false;

                if (doSSF) runOk = MatSSF.RUN(hide);

                //4
                this.progress.PerformStep();
                Application.DoEvents();

                if (runOk)
                {
                    MatSSF.OUTPUT();

                    if (MatSSF.Table.Count == 0)
                    {
                        throw new SystemException("Problems Reading MATSSF Output\n");
                    }
                }
                else if (doSSF)
                {
                    throw new SystemException("MATSSF is still calculating stuff...\n");
                    // errorB.Text += "MATSSF is still calculating stuff...\n";
                }
                //5
                this.progress.PerformStep();
                Application.DoEvents();

                if (doCk) MatSSF.CHILEAN();

                //6
                this.progress.PerformStep();
                Application.DoEvents();

                //  else errorB.Text += "Matrix Composition is empty\n";
            }
            catch (SystemException ex)
            {
                //  Interface.IReport.Msg("Database", "Database updated!");
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
                //    errorB.Text += ex.Message + "\n" + ex.Source + "\n";
            }

            MatSSF.WriteXML();

            SaveItem_Click(sender, e);

            this.ucUnit.RefreshSSF();

            //7
            this.progress.PerformStep();
            Application.DoEvents();

            Interface.IReport.Msg("Calculations", "Calculations completed!");
        }

        private Hashtable bindings = null;
        private Hashtable samplebindings = null;

        public ucSubSamples ParentUI;

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        private void setUnitBindings()
        {
            //SET THE PREFERENCES
            string format = Interface.IPreferences.CurrentSSFPref.Rounding;
            N4.TextBox.Text = format;

            LINAA.UnitDataTable Unit = Interface.IDB.Unit;
            BindingSource bs = this.ucUnit.UnitBS;
            LINAA.SubSamplesDataTable SSamples = Interface.IDB.SubSamples;
            BindingSource bsSample = this.ParentUI.BS;
            this.unitBN.BindingSource = bs;

            //unit bindings
            bindings = Dumb.ArrayOfBindings(ref bs, Interface.IPreferences.CurrentSSFPref.Rounding);
            samplebindings = Dumb.ArrayOfBindings(ref bsSample, Interface.IPreferences.CurrentSSFPref.Rounding);
            //  Dumb.ChangeBindingsFormat("N2", ref bindings);

            string column;

            column = SSamples.RadiusColumn.ColumnName;
            this.radiusbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);

            column = SSamples.FillHeightColumn.ColumnName;
            this.lenghtbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);

            column = Unit.ChDiameterColumn.ColumnName;
            this.chdiamB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ChLengthColumn.ColumnName;
            this.chlenB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = SSamples.Gross1Column.ColumnName;
            this.massB.TextBox.DataBindings.Add(samplebindings[column] as Binding);

            column = Unit.DensityColumn.ColumnName;
            this.densityB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChCfgColumn.ColumnName;
            this.cfgB.ComboBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ContentColumn.ColumnName;
            this.matrixB.DataBindings.Add(bindings[column] as Binding);
            column = Unit.kepiColumn.ColumnName;
            this.kepiB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.kthColumn.ColumnName;
            this.kthB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.NameColumn.ColumnName;
            this.nameB.ComboBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.VolColumn.ColumnName;
            this.volLbl.TextBox.DataBindings.Add(bindings[column] as Binding);
        }

        private void setPreferences()
        {
            IPreferences ip = Interface.IPreferences;

            this.loop.Checked = ip.CurrentSSFPref.Loop;
            this.calcDensity.Checked = ip.CurrentSSFPref.CalcDensity;
            this.doCK.Checked = ip.CurrentPref.DoCK;
            this.doMatSSF.Checked = ip.CurrentPref.DoMatSSF;
            this.showMatSSF.Checked = ip.CurrentPref.ShowMatSSF;
            this.AutoLoad.Checked = ip.CurrentSSFPref.AutoLoad;
            // this.SQL.Checked = ip.CurrentSSFPref.SQL;
            this.FolderPath.Text = ip.CurrentSSFPref.Folder;
            this.showOther.Checked = ip.CurrentSSFPref.ShowOther;
            this.workOffline.Checked = ip.CurrentPref.Offline;
        }

        public void LoadDatabase()
        {
            try
            {
                if (!Interface.IPreferences.CurrentPref.Offline)
                {
                    // Interface.IPopulate.IGeometry.PopulateUnits();
                }
                else //fix this
                {
                    //  Interface.IPopulate.IMain.Read(mf);
                }

                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.dgvItemSelected;
                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.dgvItemSelected;

                ucUnit.Set(ref Interface);
                ucUnit.RowHeaderMouseClick = this.dgvUnitSelected;
                ucIrradiationsRequests1.Set(ref Interface);
                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.dgvMatrixSelected;

                setPreferences();

                setUnitBindings();

                this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.Message + "\n" + ex.Source + "\n", "Error", false);
            }
        }

        private void checkedChanged(object sender, EventArgs e)
        {
            IPreferences ip = Interface.IPreferences;

            ip.CurrentSSFPref.CalcDensity = this.calcDensity.Checked;

            ip.CurrentSSFPref.Loop = this.loop.Checked;

            ip.CurrentSSFPref.Loop = this.loop.Checked;
            ip.CurrentSSFPref.CalcDensity = this.calcDensity.Checked;
            ip.CurrentPref.DoCK = this.doCK.Checked;
            ip.CurrentPref.DoMatSSF = this.doMatSSF.Checked;
            ip.CurrentPref.ShowMatSSF = this.showMatSSF.Checked;
            ip.CurrentSSFPref.AutoLoad = this.AutoLoad.Checked;
            // this.SQL.Checked = ip.CurrentSSFPref.SQL;
            ip.CurrentSSFPref.Folder = this.FolderPath.Text;
            ip.CurrentSSFPref.ShowOther = this.showOther.Checked;
            ip.CurrentPref.Offline = this.workOffline.Checked;

            Interface.IStore.SavePreferences();
        }

        /// <summary>
        /// ROUNDING FORMAT for bindings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void N4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string format = N4.TextBox.Text;
                if (format.Length < 2) return;

                Dumb.ChangeBindingsFormat(format, ref bindings);
                Interface.IPreferences.CurrentSSFPref.Rounding = format;
            }
            catch (Exception ex)
            {
                Interface.IReport.AddException(ex);
            }
        }
    }
}