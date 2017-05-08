using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;
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
                pref.CheckChanged = delegate
                {
                    paintColumns();
                };
                pref.SetRoundingBinding(ref unitBindings, ref sampleBindings);

                destiny = null;
            }
            destiny?.Controls.Add(pro as Control);
        }


       private Action showProgress;


        public void CalculateUnit(Action ShowProgress, ref bool cancelCalculations)
        {
            try
            {

                                showProgress = ShowProgress;


                //1
                bool isOK = MatSSF.UNIT.CheckErrors(); //has Unit errors??
                isOK = MatSSF.UNIT.SubSamplesRow.CheckUnit() && isOK; //has Sample Errors?

                if (isOK)
                {
                    Interface.IReport.Msg("Input data is OK for Unit " + MatSSF.UNIT.Name, "Checking data...");
                }
                else
                {
                    throw new SystemException("Input data is NOT OK for Unit " + MatSSF.UNIT.Name);
                }

         

                showProgress?.Invoke();
                MatSSF.InputFile = MatSSF.UNIT.Name;
                MatSSF.OutputFile = MatSSF.UNIT.Name;
                MatSSF.INPUT(!Interface.IPreferences.CurrentSSFPref.Overrides);
                //2
                showProgress?.Invoke();

                Interface.IReport.Msg("Input metadata generated for Unit " + MatSSF.UNIT.Name, "Starting calculations...");

            
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message, "ERROR", false);
            }
        }

        
       
          public  void Watcher_Changed(object sender, FileSystemEventArgs e)
      {

          Cursor.Current = Cursors.WaitCursor;
          Interface.IBS.SuspendBindings();
          int percent = 50;
          try
          {

           

              bool runOk = false;
          IPreferences ip = Interface.IPreferences;

          bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

          bool doCk = (ip.CurrentSSFPref.DoCK);

          bool doSSF = (ip.CurrentSSFPref.DoMatSSF);


          string sampleName = e.Name.Replace(".txt", null);
          MatSSF.UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);



                Action progress = delegate
                {
                    showProgress?.Invoke();
                    Application.DoEvents();
                //    Interface.IReport.ReportProgress(percent);
                    Application.DoEvents();
                };

                Action dele5 = delegate
                {

                    Application.DoEvents();
                    Interface.IStore.Save<LINAA.SubSamplesDataTable>();
                    Interface.IStore.Save<LINAA.UnitDataTable>();
                    Interface.IBS.Update<LINAA.UnitRow>(MatSSF.UNIT);
                    string msg = "Finished for Unit " + MatSSF.UNIT.Name;
                    Interface.IReport.Msg(msg, "Done");
                //    Interface.IReport.Speak(msg);

                };




                runOk =MatSSF.OUTPUT(sampleName);
              //50
              percent += 10;
                //  if (!string.IsNullOrEmpty(result)) runOk = true;
             //   if (InvokeRequired)
                {
                    this.Invoke(progress);
                }
              //      MatSSF.OUTPUT();

              if (!runOk)
              {
                  // Interface.IReport.Msg("MatSSF ran OK for Unit " + MatSSF.UNIT.Name,
                  // "Reading MatSSF Output file");
                  //   Interface.IReport.Msg("MatSSF calculations hanged for Unit " + MatSSF.UNIT.Name + "\n", "Something wrong loading MatSSF");
                

               //     if (InvokeRequired)
                    {
                        Action hanged = delegate
                        {
                            Application.DoEvents();
                            Interface.IReport.Msg("MatSSF calculations hanged for Unit " + MatSSF.UNIT.Name + "\n", "Something wrong loading MatSSF");
                        };
                        this.Invoke(hanged);
                    }
                  //  throw new SystemException("MatSSF hanged for Unit " + MatSSF.UNIT.Name + "\n");
                  throw new SystemException("Problems Reading MATSSF Output for Unit " + MatSSF.UNIT.Name + "\n");

                  // errorB.Text += "MATSSF is still calculating stuff...\n";

              }
              else
              {
               

                 //   if (InvokeRequired)
                    {
                        Action okTodo = delegate
                        {
                            Application.DoEvents();
                            Interface.IReport.Msg("MatSSF calculations done for Unit " + MatSSF.UNIT.Name, "MatSSF Calculations");
                        };
                        this.Invoke(okTodo);
                    }
                   //

              }
              //60
              percent += 10;
              if (doCk )
          {
                  LINAA.MatSSFDataTable table = Interface.IDB.MatSSF;
                   byte[] array =  MatSSF.UNIT.SSFTable;
              Tables.ReadDTBytes(MatSSF.StartupPath,ref array, ref table);
              MatSSF.CHILEAN();
          }
               // if (InvokeRequired)
                {
                    this.Invoke(progress);
                }
            
             //   if (InvokeRequired)
                {
                    Action newDelegate = delegate
                    {
                        Application.DoEvents();
                        Interface.IReport.Msg("CKS calculations done for Unit " + MatSSF.UNIT.Name, "CKS Calculations");
                    };

                    percent += 15;
                    this.Invoke(newDelegate);
                    this.Invoke(progress);

                    this.Invoke(dele5);
                    //70
                    percent += 15;
                    this.Invoke(progress);

                }

          }
          catch (Exception ex)
          {

              Interface.IStore.AddException(ex);
          }

          Cursor.Current = Cursors.Default;
         // Interface.IBS.ResumeBindings();

      }

      




        /*
        public  void Watcher_Changed(object sender, FileSystemEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            Interface.IBS.SuspendBindings();
            int percent = 50;
            try
            {

                Action progress = delegate
                {
                    showProgress?.Invoke();
                    Application.DoEvents();
                    Interface.IReport.ReportProgress(percent);
                    Application.DoEvents();
                };

                    Action dele5 = delegate
                {
                 
                    Application.DoEvents();
                    Interface.IStore.Save<LINAA.SubSamplesDataTable>();
                    Interface.IStore.Save<LINAA.UnitDataTable>();
                    Interface.IBS.Update<LINAA.UnitRow>(MatSSF.UNIT);
                    Interface.IReport.Msg("Finished for Unit " + MatSSF.UNIT.Name, "Done");
                 
                };

                bool runOk = false;
            IPreferences ip = Interface.IPreferences;

            bool hide = !(ip.CurrentSSFPref.ShowMatSSF);

            bool doCk = (ip.CurrentSSFPref.DoCK);

            bool doSSF = (ip.CurrentSSFPref.DoMatSSF);


            string sampleName = e.Name.Replace(".txt", null);
            MatSSF.UNIT = Interface.IDB.Unit.FirstOrDefault(o => o.Name.CompareTo(sampleName) == 0);
    
               runOk =MatSSF.OUTPUT(sampleName);
                //50
                percent += 10;
                //  if (!string.IsNullOrEmpty(result)) runOk = true;
                this.BeginInvoke(progress);
                //      MatSSF.OUTPUT();

                if (!runOk)
                {
                    // Interface.IReport.Msg("MatSSF ran OK for Unit " + MatSSF.UNIT.Name,
                    // "Reading MatSSF Output file");
                    //   Interface.IReport.Msg("MatSSF calculations hanged for Unit " + MatSSF.UNIT.Name + "\n", "Something wrong loading MatSSF");
                    Action hanged = delegate
                    {
                        Application.DoEvents();
                        Interface.IReport.Msg("MatSSF calculations hanged for Unit " + MatSSF.UNIT.Name + "\n", "Something wrong loading MatSSF");
                    };

                    this.BeginInvoke(hanged);

                    //  throw new SystemException("MatSSF hanged for Unit " + MatSSF.UNIT.Name + "\n");
                    throw new SystemException("Problems Reading MATSSF Output for Unit " + MatSSF.UNIT.Name + "\n");

                    // errorB.Text += "MATSSF is still calculating stuff...\n";

                }
                else
                {
                    Action okTodo = delegate
                    {
                        Application.DoEvents();
                        Interface.IReport.Msg("MatSSF calculations done for Unit " + MatSSF.UNIT.Name, "MatSSF Calculations");
                    };

                 
                    this.BeginInvoke(okTodo);
                     //

                }
                //60
                percent += 10;
                if (doCk )
            {
                    LINAA.MatSSFDataTable table = Interface.IDB.MatSSF;
                     byte[] array =  MatSSF.UNIT.SSFTable;
                Tables.ReadDTBytes(MatSSF.StartupPath,ref array, ref table);
                MatSSF.CHILEAN();
            }

                this.BeginInvoke(progress);
                Action newDelegate = delegate
                {
                    Application.DoEvents();
                    Interface.IReport.Msg("CKS calculations done for Unit " + MatSSF.UNIT.Name, "CKS Calculations");
                };

                percent += 10;
                this.BeginInvoke(newDelegate);
                this.BeginInvoke(progress);

                this.BeginInvoke(dele5);
                //70
                percent += 10;
                this.BeginInvoke(progress);

             

            }
            catch (Exception ex)
            {

                Interface.IStore.AddException(ex);
            }

            Cursor.Current = Cursors.Default;
           // Interface.IBS.ResumeBindings();

        }
        */
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
            this.ucComposition1.Controls[0].Visible = enable;

            this.sampleDGV.Enabled = enable;

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

                sampleDGV.DataSource = Interface.IBS.SelectedSubSample;

                //otherwise this shit shows weird text over text
          //      Interface.IBS.SelectedSubSample.CurrentChanged += delegate
             //   {
                 //   this.sampleDGV.Refresh();
                    // this.sampleDGV.Select();
            //    };

                //link to bindings
                sampleBindings = setSampleBindings();
                unitBindings = setUnitBindings();

                setEnabledBindings();

                ucComposition1.Set(ref Interface);

                DataGridViewColumn col = this.volDataGridViewTextBoxColumn;
                paintColumn(true, ref col);

                paintColumns();

                errorProvider1.DataMember = Interface.IDB.Unit.TableName;
                errorProvider1.DataSource = Interface.IBS.Units;
                errorProvider2.DataMember = Interface.IDB.SubSamples.TableName;
                errorProvider2.DataSource = Interface.IBS.SubSamples;

                Interface.IReport.Msg("Database", "Units were loaded!");
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void paintColumns()
        {
            bool readOnly = Interface.IPreferences.CurrentSSFPref.AAFillHeight;
            DataGridViewColumn columna = this.fillHeightDataGridViewTextBoxColumn;
            paintColumn(readOnly, ref columna);

            // Interface.IPreferences.CurrentSSFPref.EndEdit();
            columna = this.gross1DataGridViewTextBoxColumn;
            readOnly = Interface.IPreferences.CurrentSSFPref.CalcMass;
            paintColumn(readOnly, ref columna);

            readOnly = Interface.IPreferences.CurrentSSFPref.AARadius;
            columna = this.radiusDataGridViewTextBoxColumn;
            paintColumn(readOnly, ref columna);

            readOnly = Interface.IPreferences.CurrentSSFPref.CalcDensity;
            columna = this.calcDensityDataGridViewTextBoxColumn;
            paintColumn(readOnly, ref columna);
        }

        private static void paintColumn(bool readOnly, ref DataGridViewColumn columna)
        {
            Color color2 = Color.Yellow;
            Color colors = Color.Gray;
            // Color colors2 = Color.White;
            if (!readOnly)
            {
                colors = Color.White;
                color2 = Color.Black;
                // colors2 = Color.Gray;
            }
            columna.ReadOnly = readOnly;
            columna.DefaultCellStyle.BackColor = colors;
            columna.DefaultCellStyle.ForeColor = color2;
            columna.DefaultCellStyle.SelectionBackColor = colors;
            columna.DefaultCellStyle.SelectionForeColor = color2;
        }

        /*
        private void setCheckBindings()
        {
            string column;
            Hashtable checkBindings = Dumb.BS.ArrayOfBindings(ref Interface.IBS.SSFPreferences, string.Empty, "Checked");

            column = Interface.IDB.SSFPref.AARadiusColumn.ColumnName;
            Binding renabled = checkBindings[column] as Binding; //new Binding("ReadOnly", Interface.IDB.SSFPref, Interface.IDB.SSFPref.AARadiusColumn.ColumnName);

           // checkBox1.Visible = true;
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
        */

        private void setEnabledBindings()
        {
            string column;

            column = Interface.IDB.SSFPref.DoCKColumn.ColumnName;
            Binding renabled5 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.kthB.TextBox.DataBindings.Add(renabled5);
            Binding other = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.kepiB.TextBox.DataBindings.Add(other);
            Binding other10 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.pEpiBox.TextBox.DataBindings.Add(other10);
            Binding other11 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.pThBox.TextBox.DataBindings.Add(other11);


            column = Interface.IDB.SSFPref.DoMatSSFColumn.ColumnName;

            Binding renabled6 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.chlenB.TextBox.DataBindings.Add(renabled6);
            Binding other2 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.chdiamB.TextBox.DataBindings.Add(other2);
            Binding other3 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.cfgB.ComboBox.DataBindings.Add(other3);

            Binding other4 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            Binding other5 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            Binding other6 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.bellfactorBox.TextBox.DataBindings.Add(other4);
            this.WGtBox.TextBox.DataBindings.Add(other5);
            this.nFactorBox.TextBox.DataBindings.Add(other6);



            this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);
            /*
            column = Interface.IDB.SSFPref.OverridesColumn.ColumnName;

            Binding renabled7 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            Binding renabled8 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            Binding renabled9 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            Binding renabled10 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            Binding renabled11 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
         //   Binding renabled12 = new Binding("Enabled", Interface.IBS.SSFPreferences, column);
            this.pEpiBox.TextBox.DataBindings.Add(renabled10);
            this.pThBox.TextBox.DataBindings.Add(renabled7);
            this.WGtBox.TextBox.DataBindings.Add(renabled8);
            this.nFactorBox.TextBox.DataBindings.Add(renabled9);
            this.bellfactorBox.TextBox.DataBindings.Add(renabled11);
            */
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

            samplebindings = BS.ArrayOfBindings(ref bsSample, rounding);
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

            Binding b1 = BS.ABinding(ref bsSample, column);
            this.samp1lbl.TextBox.DataBindings.Add(b1);
            Binding b2 = BS.ABinding(ref bsSample, column);
            this.samp2lbl.TextBox.DataBindings.Add(b2);
            Binding b3 = BS.ABinding(ref bsSample, column);
            this.samp3lbl.TextBox.DataBindings.Add(b3);

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
            string rounding = "N3";
            rounding = Interface.IPreferences.CurrentSSFPref?.Rounding;
          
            //units
            UnitDataTable Unit = Interface.IDB.Unit;
            BindingSource bs = Interface.IBS.Units; //link to binding source;
            Hashtable bindings = BS.ArrayOfBindings(ref bs, rounding);

            string column;
            column = Unit.ChRadiusColumn.ColumnName;
            this.chdiamB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChLengthColumn.ColumnName;
            this.chlenB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.BellFactorColumn.ColumnName;
            this.bellfactorBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.WGtColumn.ColumnName;
            this.WGtBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.nFactorColumn.ColumnName;
            this.nFactorBox.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.ChCfgColumn.ColumnName;
            this.cfgB.ComboBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.kepiColumn.ColumnName;
            this.kepiB.TextBox.DataBindings.Add(bindings[column] as Binding);

            column = Unit.kthColumn.ColumnName;
            this.kthB.TextBox.DataBindings.Add(bindings[column] as Binding);

            BindingSource bschannel = Interface.IBS.SelectedChannel;
            Hashtable channelBindings = BS.ArrayOfBindings(ref bschannel);

            column = Interface.IDB.Channels.pThColumn.ColumnName;
            this.pThBox.TextBox.DataBindings.Add(channelBindings[column] as Binding);

            column = Interface.IDB.Channels.pEpiColumn.ColumnName;
            this.pEpiBox.TextBox.DataBindings.Add(channelBindings[column] as Binding);
  

            return bindings;
        }

        // CheckBox check = new CheckBox();
        public ucSSFData()
        {
            InitializeComponent();
        }
    }
}