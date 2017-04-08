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

        private static string preFolder = Environment.GetFolderPath(folder);

        private Interface Interface = null;

        public void AttachMsn(ref Control msn)
        {
            this.contronSC.Panel2.Controls.Add(msn);
        }

        public void AttachProjectBox(ref Control pro)
        {
            this.inputTLP.Controls.Add(pro);
        }

        public void AttachBN(ref Control bn)
        {
            this.unitBN.Dispose();
            this.unitSC.Panel2.Controls.Add(bn);
        }

        public ucSSF()
        {
            InitializeComponent();
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
                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                    return;
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

                MatSSF.UNIT = row as LINAA.UnitRow;

                //        this.ucUnit.RefreshSSF();

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

            Interface.IBS.Matrix.EndEdit();
            Interface.IBS.Units.EndEdit();
            Interface.IBS.Vial.EndEdit();
            Interface.IBS.Rabbit.EndEdit();
            Interface.IBS.Channels.EndEdit();

            bool off = Interface.IPreferences.CurrentPref.Offline;
            Interface.IStore.SaveSSF(off, mf);
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            //Validate Binding sources
            Interface.IBS.Matrix.EndEdit();
            Interface.IBS.Units.EndEdit();
            Interface.IBS.Vial.EndEdit();
            Interface.IBS.Rabbit.EndEdit();
            Interface.IBS.Channels.EndEdit();

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

            //    this.ucUnit.RefreshSSF();

            //7
            this.progress.PerformStep();
            Application.DoEvents();

            Interface.IReport.Msg("Calculations", "Calculations completed!");
        }

        public ucSubSamples ParentUI;
        private Hashtable bindings = null;

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>

        public void Set(ref Interface inter)
        {
            //  object db = Linaa;
            Interface = inter;
            try
            {
                // OTHER CONTROLS
                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.dgvItemSelected;
                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.dgvItemSelected;

                ucUnit.Set(ref Interface);
                ucUnit.RowHeaderMouseClick = this.dgvUnitSelected;
                ucIrradiationsRequests1.Set(ref Interface);
                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.dgvMatrixSelected;

                string folder = Interface.IPreferences.CurrentSSFPref.Folder;
                MatSSF.StartupPath = preFolder + folder;

                //SET THE PREFERENCES
                string format = Interface.IPreferences.CurrentSSFPref.Rounding;
                N4.TextBox.Text = format;

                BindingSource bsSample = Interface.IBS.SubSamples;

                Hashtable samplebindings = null;
                //unit bindings
                string rounding = Interface.IPreferences.CurrentSSFPref.Rounding;

                samplebindings = Dumb.ArrayOfBindings(ref bsSample, rounding);
                //  Dumb.ChangeBindingsFormat("N2", ref bindings);

                LINAA.SubSamplesDataTable SSamples = Interface.IDB.SubSamples;
                string column;
                //samples
                column = SSamples.RadiusColumn.ColumnName;
                this.radiusbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
                column = SSamples.FillHeightColumn.ColumnName;
                this.lenghtbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
                column = SSamples.Gross1Column.ColumnName;
                this.massB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
                column = SSamples.SubSampleNameColumn.ColumnName;
                this.nameB.ComboBox.DataBindings.Add(samplebindings[column] as Binding);
                this.nameB.AutoCompleteSource = AutoCompleteSource.ListItems;
                column = SSamples.VolColumn.ColumnName;
                this.volLbl.TextBox.DataBindings.Add(samplebindings[column] as Binding);
                column = SSamples.CalcDensityColumn.ColumnName;
                this.densityB.TextBox.DataBindings.Add(samplebindings[column] as Binding);

                //units
                LINAA.UnitDataTable Unit = Interface.IDB.Unit;
                BindingSource bs = Interface.IBS.Units; //link to binding source;
                bindings = Dumb.ArrayOfBindings(ref bs, rounding);

                column = Unit.ChDiameterColumn.ColumnName;
                this.chdiamB.TextBox.DataBindings.Add(bindings[column] as Binding);
                column = Unit.ChLengthColumn.ColumnName;
                this.chlenB.TextBox.DataBindings.Add(bindings[column] as Binding);

                column = Unit.ChCfgColumn.ColumnName;
                this.cfgB.ComboBox.DataBindings.Add(bindings[column] as Binding);
                column = Unit.ContentColumn.ColumnName;
                this.matrixB.DataBindings.Add(bindings[column] as Binding);
                column = Unit.kepiColumn.ColumnName;
                this.kepiB.TextBox.DataBindings.Add(bindings[column] as Binding);
                column = Unit.kthColumn.ColumnName;
                this.kthB.TextBox.DataBindings.Add(bindings[column] as Binding);

                //types
                this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);

                if (!Interface.IPreferences.CurrentPref.Offline)
                {
                    // Interface.IPopulate.IGeometry.PopulateUnits();
                }
                else //fix this
                {
                    //  Interface.IPopulate.IMain.Read(mf);
                }

                //preferences
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

                this.loop.CheckedChanged += this.checkedChanged;
                this.calcDensity.CheckedChanged += this.checkedChanged;
                this.showMatSSF.CheckedChanged += this.checkedChanged;
                this.showOther.CheckedChanged += this.checkedChanged;
                this.workOffline.CheckedChanged += this.checkedChanged;
                this.doMatSSF.CheckedChanged += this.checkedChanged;
                this.doCK.CheckedChanged += this.checkedChanged;
                this.AutoLoad.CheckedChanged += this.checkedChanged;

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