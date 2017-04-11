using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSF : UserControl
    {
        // private bool Offline = false;

        private Interface Interface = null;

        public ucSSF()
        {
            InitializeComponent();
        }

        public void AttachBN(ref Control bn)
        {
            this.unitBN.Dispose();
            this.unitSC.Panel2.Controls.Add(bn);
        }

        public void AttachMsn(ref Control msn)
        {
            this.contronSC.Panel2.Controls.Add(msn);
        }

        public void AttachProjectBox(ref Control pro)
        {
            this.splitContainer1.Panel1.Controls.Add(pro);
        }

        public void AttachSampleCtrl(ref Control ctrl)
        {
            ctrl.Dock = DockStyle.Fill;
            this.SamplesTab.Controls.Add(ctrl);
        }

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {
                // OTHER CONTROLS
                ucCC1.Set(ref Interface);
                ucCC1.RowHeaderMouseClick = this.dgvItemSelected;

                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.dgvItemSelected;

                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.dgvItemSelected;

                ucUnit.Set(ref Interface);
                ucUnit.RowHeaderMouseClick = this.dgvUnitSelected;
            }
            catch (System.Exception ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }
            try
            {
                Hashtable samplebindings = setSampleBindings();

                Hashtable bindings = setUnitBindings();

                //types
                this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);

                ucCalcOptions.Set(ref Interface, ref bindings, ref samplebindings);

                Interface.IBS.EndEdit();

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            this.progress.Minimum = 0;
            this.progress.Maximum = 3;
            this.progress.Value = 1;
            //Go to Calculations/ Units Tab
            this.Tab.SelectedTab = this.CalcTab;

            //Clear InputFile RTF Control
            inputbox.Clear();

            Action showProgress = delegate
            {
                Application.DoEvents();
                this.progress.PerformStep();
                Application.DoEvents();
            };

            Interface.IBS.EndEdit();

            IList<UnitRow> units = null;
            bool shoulLoop = Interface.IPreferences.CurrentSSFPref.Loop;
            if (shoulLoop)
            {
                //take currents
                units = Interface.ICurrent.Units.OfType<LINAA.UnitRow>()
                    .Where(o => o.ToDo).ToList();
            }
            else
            {
                //take only current
                units = new List<UnitRow>();
                units.Add(Interface.ICurrent.Unit as UnitRow);
            }

            //loop through all samples to work to
            foreach (UnitRow item in units)
            {
                this.progress.Maximum += 5;
                MatSSF.UNIT = item;

                //update position in BS
                Interface.IBS.Update<LINAA.UnitRow>(item);
                CalculateUnit(ref showProgress);
            }

            //load files
            string file = MatSSF.StartupPath + MatSSF.InputFile;
            bool exist = System.IO.File.Exists(file);
            if (exist) inputbox.LoadFile(file, RichTextBoxStreamType.PlainText);

            showProgress();

            file = MatSSF.StartupPath + MatSSF.OutputFile;
            exist = System.IO.File.Exists(file);
            if (exist) outputBox.LoadFile(file, RichTextBoxStreamType.PlainText);
            //3
            showProgress();

            //      ucCalc.saveMethod();

            Cursor.Current = Cursors.Default;
        }

        private void CalculateUnit(ref Action showProgress)
        {
            try
            {
                IPreferences ip = Interface.IPreferences;

                bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

                bool doCk = (ip.CurrentSSFPref.DoCK);

                bool doSSF = (ip.CurrentSSFPref.DoMatSSF);

                //Validate Binding sources

                //1
                showProgress();

                MatSSF.INPUT();
                //2
                showProgress();
                Interface.IReport.Msg("Input metadata generated", "Starting Calculations...");

                bool runOk = false;

                if (doSSF) runOk = MatSSF.RUN(hide);

                //4
                showProgress();

                if (runOk)
                {
                    Interface.IReport.Msg("MatSSF Ran", "Reading Output");

                    MatSSF.OUTPUT();

                    if (MatSSF.Table.Count == 0)
                    {
                        throw new SystemException("Problems Reading MATSSF Output\n");
                    }
                }
                else if (doSSF)
                {
                    Interface.IReport.Msg("MatSSF hanged...", "Something happened executing MatSSF");

                    throw new SystemException("MATSSF hanged...\n");

                    // errorB.Text += "MATSSF is still calculating stuff...\n";
                }
                //5
                showProgress();

                Interface.IReport.Msg("MatSSF done", "Calculations completed!");

                if (doCk)
                {
                    MatSSF.CHILEAN();
                    Interface.IReport.Msg("CK done", "Calculations completed!");
                }

                //6
                showProgress();
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }
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
                bool isChannel = row.GetType().Equals(typeof(ChannelsRow));
                bool isMatrix = row.GetType().Equals(typeof(MatrixRow));
                if (isChannel)
                {
                    ChannelsRow c = row as ChannelsRow;
                    MatSSF.UNIT.SetChannel(ref c);
                }
                else if (!isMatrix)
                {
                    LINAA.VialTypeRow v = row as VialTypeRow;
                    if (!v.IsRabbit) MatSSF.UNIT.SubSamplesRow.VialTypeRow = v;
                    else MatSSF.UNIT.SubSamplesRow.VialTypeRowByChCapsule_SubSamples = v;
                }
                else
                {
                    MatrixRow m = row as MatrixRow;
                    MatSSF.UNIT.SubSamplesRow.MatrixID = m.MatrixID;
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
                ///has errors
                if (row.HasErrors)
                {
                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                //    return;
                }

                MatSSF.UNIT = row as UnitRow;

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

        private Hashtable setSampleBindings()
        {
            string rounding = "N4";
            rounding = Interface.IPreferences.CurrentSSFPref.Rounding;

            BindingSource bsSample = Interface.IBS.SubSamples;
            Hashtable samplebindings = null;
      

            SubSamplesDataTable SSamples = Interface.IDB.SubSamples;

            samplebindings = Dumb.ArrayOfBindings(ref bsSample, rounding);
            string column;
            //samples
            column = SSamples.RadiusColumn.ColumnName;
            this.radiusbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.radiusbox.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            column = SSamples.FillHeightColumn.ColumnName;
            this.lenghtbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.lenghtbox.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            column = SSamples.Gross1Column.ColumnName;
            Binding massbin = samplebindings[column] as Binding;
            this.massB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.massB.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            massbin.FormatString = "N2";
            samplebindings.Remove(massbin); //so it does not update its format!!!
            column = SSamples.SubSampleNameColumn.ColumnName;
            this.nameB.ComboBox.DataBindings.Add(samplebindings[column] as Binding);
            this.nameB.ComboBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            this.nameB.AutoCompleteSource = AutoCompleteSource.ListItems;
            column = SSamples.VolColumn.ColumnName;
            this.volLbl.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.volLbl.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            column = SSamples.CalcDensityColumn.ColumnName;
            this.densityB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            this.densityB.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            //  Binding b = new Binding();


            return samplebindings;
        }

        private Hashtable setUnitBindings()
        {
            string rounding = "N4";
            rounding = Interface.IPreferences.CurrentSSFPref?.Rounding;

            string column;
            //units
            UnitDataTable Unit = Interface.IDB.Unit;
            BindingSource bs = Interface.IBS.Units; //link to binding source;
            Hashtable bindings = null;

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

            Binding renabled = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AARadiusColumn.ColumnName);
            this.radiusbox.TextBox.DataBindings.Add(renabled);
            Binding renabled2 = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName);
            this.lenghtbox.TextBox.DataBindings.Add(renabled2);
            Binding renabled3 = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.CalcDensityColumn.ColumnName);
            this.densityB.TextBox.DataBindings.Add(renabled3);
            Binding renabled4 = new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.CalcMassColumn.ColumnName);
            this.massB.TextBox.DataBindings.Add(renabled4);

            //   Binding c = new Binding("Width", Interface.IDB.SubSamples, Interface.IDB.SubSamples.RadiusColumn.ColumnName);

            //      this.ParentForm.DataBindings.Add(c);
            //    this.ParentForm.Size.
            return bindings;
        }

     
    }
}