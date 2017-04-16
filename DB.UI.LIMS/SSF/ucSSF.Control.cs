using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucSSFData : UserControl
    {
        // private bool cancelCalculations = false;
        private Interface Interface = null;

        // private static Size currentSize;
        private Hashtable unitBindings, sampleBindings;
        // private Action<int> resetProgress; private Action showProgress;

        /// <summary>
        /// Attachs the respectivo SuperControls to the SSF Control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pro"></param>
        public void AttachCtrl<T>(ref T pro)
        {
            Control destiny = null;
            if (pro.GetType().Equals(typeof(Msn.Pop)))
            {
                (pro as Msn.Pop).Dock = DockStyle.Fill;
                destiny = this.UnitSSFSC.Panel2;
            }
            else if (pro.GetType().Equals(typeof(BindingNavigator)))
            {
                BindingNavigator b = pro as BindingNavigator;
                b.Items["SaveItem"].Visible = false;
                b.Parent.Controls.Remove(b);
                // this.unitBN.Dispose();
                destiny = this.unitSC.Panel2;
            }
            else if (pro.GetType().Equals(typeof(ucPreferences)))
            {
                IucPreferences pref = pro as IucPreferences;
                pref.SetRoundingBinding(ref unitBindings, ref sampleBindings);
                destiny = null;
            }
            destiny?.Controls.Add(pro as Control);
        }

        public void CalculateUnit(Action showProgress, ref bool cancelCalculations)
        {
            try
            {
                IPreferences ip = Interface.IPreferences;

                bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

                bool doCk = (ip.CurrentSSFPref.DoCK);

                bool doSSF = (ip.CurrentSSFPref.DoMatSSF);

                //1
                bool isOK = MatSSF.UNIT.Check(); //has Unit errors??
                isOK = MatSSF.UNIT.SubSamplesRow.CheckUnit() && isOK; //has Sample Errors?


                if (isOK)
                {
                    Interface.IReport.Msg("Input data is OK for Unit " + MatSSF.UNIT.Name, "Checking data...");
                }
                else
                {
                    throw new SystemException("Input data is NOT OK for Unit " + MatSSF.UNIT.Name);
                }

                //at least activate MatSSF!!!
                if (!doSSF && !doCk)
                {
                    ip.CurrentSSFPref.DoMatSSF = true;
                    doSSF = true;
                    //throw new SystemException("No Calculation Method has been selected. Check the Preferences!");
                }


                // MatSSF.Table.Clear();

                showProgress?.Invoke();

                MatSSF.INPUT();
                //2
                showProgress?.Invoke();

                Interface.IReport.Msg("Input metadata generated for Unit " + MatSSF.UNIT.Name, "Starting calculations...");

                bool runOk = false;

                if (doSSF && !cancelCalculations) runOk = MatSSF.RUN(hide);

                //4
                showProgress?.Invoke();

                if (!cancelCalculations)
                {
                    if (runOk)
                    {
                        Interface.IReport.Msg("MatSSF ran OK for Unit " + MatSSF.UNIT.Name, "Reading MatSSF Output file");

                        MatSSF.OUTPUT();

                        if (MatSSF.Table.Count == 0)
                        {
                           // Interface.IReport.Msg("MatSSF ran OK for Unit " + MatSSF.UNIT.Name, "Reading MatSSF Output file");

                            throw new SystemException("Problems Reading MATSSF Output for Unit " + MatSSF.UNIT.Name + "\n");
                        }
                    }
                    else if (doSSF)
                    {
                        Interface.IReport.Msg("MatSSF calculations hanged for Unit " + MatSSF.UNIT.Name + "\n", "Something wrong loading MatSSF");

                        throw new SystemException("MatSSF hanged for Unit " + MatSSF.UNIT.Name + "\n");

                        // errorB.Text += "MATSSF is still calculating stuff...\n";
                    }
                    // Interface.IReport.Msg("MatSSF done", "Calculations completed!");
                    Interface.IReport.Msg("MatSSF calculations done for Unit " + MatSSF.UNIT.Name, "MatSSF Calculations");
                }
                else Interface.IReport.Msg("MatSSF cancelled", "Calculations cancelled!");

                //5
                showProgress?.Invoke();

                // Interface.IReport.Msg("MatSSF done", "Calculations completed!");

                if (doCk && !cancelCalculations)
                {
                    MatSSF.CHILEAN();
                    Interface.IReport.Msg("CKS calculations done for Unit " + MatSSF.UNIT.Name, "CKS Calculations");
                }

                //convert table into subTable of Units
                if (!cancelCalculations)
                {
                    MatSSF.WriteXML();
                    //this also saves the UNITS!!
                    Interface.IStore.Save<SubSamplesDataTable>();
                    // Interface.IStore.Save<LINAA.SubSamplesDataTable>();
                    Interface.IStore.Save<LINAA.UnitDataTable>();
                }
                //6
                showProgress?.Invoke();
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message, "ERROR", false);
            }
        }

        public void Disabler(bool enable)
        {
            //turns off or disables the controls.
            //necessary protection for user interface
            IEnumerable<ToolStrip> strips = inputTLP.Controls.OfType<ToolStrip>();

            foreach (var item in strips)
            {
                item.Enabled = enable;
            }

            nameToolStrip.Enabled = enable;
            this.matrixB.Enabled = enable;
        }

        /// <summary>
        /// sets the bindings for the ControlBoxes and others
        /// </summary>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            try
            {
                Dumb.FD(ref this.SampleBS);
                //desaparece esto porque tiene controles para developers, los checkboxes
                this.tableLayoutPanel1.Visible = false;

                sampleDGV.DataSource = Interface.IBS.SelectedSubSample;

                //otherwise this shit shows weird text over text
                Interface.IBS.SelectedSubSample.CurrentChanged += delegate
                {
                    this.sampleDGV.Select();
                };
               

                //link to bindings
                sampleBindings = setSampleBindings();
                unitBindings = setUnitBindings();

                setCheckBindings();
                setEnabledBindings();

                errorProvider1.DataMember = Interface.IDB.Unit.TableName;
                errorProvider1.DataSource = Interface.IBS.Units;
                errorProvider2.DataMember = Interface.IDB.SubSamples.TableName;
                errorProvider2.DataSource = Interface.IBS.SubSamples;

                //set calculation options

                // Interface.IBS.EndEdit();

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IMain.AddException(ex);
                // Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }
        }
        private void checkedChanged(object sender, EventArgs e)
        {
            bool state = (sender as CheckBox).Checked;

            DataGridViewColumn columna = null;

            if (sender.Equals(checkBox1))
            {
                columna = this.radiusDataGridViewTextBoxColumn;
            }
            else if (sender.Equals(checkBox2))
            {
                columna = this.fillHeightDataGridViewTextBoxColumn;
            }
            else if (sender.Equals(checkBox4))
            {
                columna = this.gross1DataGridViewTextBoxColumn;
            }
            else if (sender.Equals(checkBox3))
            {
                columna = this.calcDensityDataGridViewTextBoxColumn;
            }
            Color color2 = Color.Yellow;
            Color colors = Color.Gray;
            // Color colors2 = Color.White;
            if (!state)
            {
                colors = Color.White;
                color2 = Color.Black;
                // colors2 = Color.Gray;
            }
            columna.ReadOnly = state;
            columna.DefaultCellStyle.BackColor = colors;
            columna.DefaultCellStyle.ForeColor = color2;
            //this.fillHeightDataGridViewTextBoxColumn.ReadOnly = !state;
            //this.fillHeightDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = colors2;
        }

        private void setCheckBindings()
        {
            string column;
            Hashtable checkBindings = Dumb.ArrayOfBindings(ref Interface.IBS.SSFPreferences, string.Empty, "Checked");

            column = Interface.IDB.SSFPref.AARadiusColumn.ColumnName;
            Binding renabled = checkBindings[column] as Binding; //new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AARadiusColumn.ColumnName);

            checkBox1.Visible = true;
            checkBox1.DataBindings.Add(renabled);
            checkBox1.CheckedChanged += checkedChanged;

            //this.radiusDataGridViewTextBoxColumn.TextBox.DataBindings.Add(renabled);
            column = Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName;
            Binding renabled2 = checkBindings[column] as Binding;//new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AAFillHeightColumn.ColumnName);
            checkBox2.DataBindings.Add(renabled2);
            checkBox2.CheckedChanged += checkedChanged;
            // this.lenghtbox.TextBox.DataBindings.Add(renabled2);
            column = Interface.IDB.SSFPref.CalcDensityColumn.ColumnName;
            Binding renabled3 = checkBindings[column] as Binding;
            checkBox3.DataBindings.Add(renabled3);
            checkBox3.CheckedChanged += checkedChanged;
            //this.densityB.TextBox.DataBindings.Add(renabled3);
            column = Interface.IDB.SSFPref.CalcMassColumn.ColumnName;
            Binding renabled4 = checkBindings[column] as Binding;
            checkBox4.DataBindings.Add(renabled4);
            checkBox4.CheckedChanged += checkedChanged;
            // this.massB.TextBox.DataBindings.Add(renabled4);

            // return column;
        }

        private void setEnabledBindings()
        {
            string column;

            column = Interface.IDB.SSFPref.DoCKColumn.ColumnName;
            Binding renabled5 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.kthB.TextBox.DataBindings.Add(renabled5);
            Binding other = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.kepiB.TextBox.DataBindings.Add(other);

            column = Interface.IDB.SSFPref.DoMatSSFColumn.ColumnName;
            Binding renabled6 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.chlenB.TextBox.DataBindings.Add(renabled6);
            Binding other2 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.chdiamB.TextBox.DataBindings.Add(other2);
            Binding other3 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);

            this.cfgB.ComboBox.DataBindings.Add(other3);

            this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);

            /*
            //types
            this.cfgB.ComboBox.DisplayMember = Interface.IDB.Channels.FluxTypeColumn.ColumnName;
            this.cfgB.ComboBox.DataSource = Interface.IBS.Channels;
            this.cfgB.ComboBox.ValueMember = Interface.IDB.Channels.FluxTypeColumn.ColumnName;

            */
            // this.cfgB.AutoCompleteMode = AutoCompleteMode.//.OfType<string>();
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

            // this.sampleDGV.DataBindings.Add(new Binding("Columns.", Interface.IBS.SSFPreferences, "DoMatSSF"));

            //    this.sampleDGV.Columns[0].o
            //samples
            //     column = SSamples.RadiusColumn.ColumnName
            //  this.radiusbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            //    this.radiusbox.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            //   column = SSamples.FillHeightColumn.ColumnName;
            //    this.lenghtbox.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            //    this.lenghtbox.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            // column = SSamples.Gross1Column.ColumnName; Binding massbin = samplebindings[column] as
            // Binding; this.massB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            // this.massB.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            //volver a poner??
            //    massbin.FormatString = "N2";

            // samplebindings.Remove(massbin); //so it does not update its format!!!
            column = SSamples.SubSampleNameColumn.ColumnName;
            this.nameB.ComboBox.DataBindings.Add(samplebindings[column] as Binding);
            // this.nameB.ComboBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            this.nameB.ComboBox.DisplayMember = column;
            this.nameB.ComboBox.ValueMember = column;
            this.nameB.ComboBox.DataSource = bsSample;

            this.nameB.AutoCompleteSource = AutoCompleteSource.ListItems;
            // column = SSamples.VolColumn.ColumnName;
            // this.volLbl.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            // this.volLbl.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            //
            // column = SSamples.CalcDensityColumn.ColumnName;
            // this.densityB.TextBox.DataBindings.Add(samplebindings[column] as Binding);
            // this.densityB.TextBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            column = SSamples.SubSampleDescriptionColumn.ColumnName;
            this.descripBox.TextBox.DataBindings.Add(samplebindings[column] as Binding);

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

            column = Unit.BellFactorColumn.ColumnName;
            this.bellfactorBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChCfgColumn.ColumnName;
            this.cfgB.ComboBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ContentColumn.ColumnName;
            this.matrixB.DataBindings.Add(bindings[column] as Binding);
            column = Unit.kepiColumn.ColumnName;
            this.kepiB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.kthColumn.ColumnName;
            this.kthB.TextBox.DataBindings.Add(bindings[column] as Binding);

            return bindings;
        }
        // CheckBox check = new CheckBox();
        public ucSSFData()
        {
            InitializeComponent();
        }
    }
}