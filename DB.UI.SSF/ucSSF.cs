using System;
using System.Windows.Forms;

using DB.Tools;
using System.Data;
using Rsx;
using Msn;
using System.Collections;
using System.Collections.Generic;
namespace DB.UI
{
    public partial class ucSSF : UserControl
    {



      
        private bool Offline = false;
        private static string mf = preFolder + matssfFolder + "lims.xml";
        private static Environment.SpecialFolder folder = Environment.SpecialFolder.Personal;
        private static string matssfFolder = DB.Properties.Resources.MatSSFFolder;
        private static string preFolder = Environment.GetFolderPath(folder);
        private Interface Interface = null;


       //private ucMatrixSimple ucMS = null;



        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        public static Form Start()
        {
            DB.LINAA db = null;
            Msn.Pop msn = new Msn.Pop();

            msn.ParentForm.StartPosition = FormStartPosition.CenterScreen;
            //   msn.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            msn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            msn.Location = new System.Drawing.Point(3, 32);
            msn.Name = "msn";
            msn.Padding = new System.Windows.Forms.Padding(9);
            msn.Size = new System.Drawing.Size(512, 113);
            msn.TabIndex = 6;

            NotifyIcon con = null;
            string result = Creator.Build(ref db, ref con, ref msn);

            //   DB.Tools.Creator.CallBack = this.CallBack;
            //  DB.Tools.Creator.LastCallBack = this.LastCallBack;

            if (!string.IsNullOrEmpty(result))
            {
                //    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //   Connections_Click(null, EventArgs.Empty);
            }
            else DB.Tools.Creator.Load(ref db, 0);

            // this.msn = this.lINAA.Msn;

            ucSSF uc = new ucSSF(ref db, false);

            //   DB.UI.Auxiliar aux = new DB.UI.Auxiliar();

            return uc.ParentForm;
        }

        public ucSSF(ref LINAA Linaa, bool offline)
        {
            InitializeComponent();

         

            object db = Linaa;
            Interface = new Interface(ref db);

            Offline = offline;

            System.Windows.Forms.Form form = null;

            Pop msn = null;
            msn = Linaa.Msn;
            form = msn.ParentForm;
            // this.msn = this.lINAA.Msn;
            this.unitTLP.Controls.Add(msn, 0, 1);
            msn.Dock = DockStyle.Fill;
            form.Dispose();

            form = new System.Windows.Forms.Form();
            form.AutoSize = true;
            form.Text = "SSF Panel";
            IntPtr Hicon = DB.UI.Properties.Resources.Logo.GetHicon();
            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(Hicon);
            form.Icon = myIcon;
            form.Controls.Add(this);// Populate(this);
            form.Show();

            MatSSF.StartupPath = preFolder + matssfFolder;

            loadDatabase();

             
            /*
            form = new System.Windows.Forms.Form();
            form.AutoSize = true;
            form.Text = "SSF Panel";
            Hicon = DB.UI.Properties.Resources.Logo.GetHicon();
            myIcon = System.Drawing.Icon.FromHandle(Hicon);
            form.Icon = myIcon;
            form.Controls.Add(unit);// Populate(this);
            form.Show();
            */


        }

        /// <summary>
        /// Adds a new row of type VialType or Channels, which is either a Rabbit/Vial or a Channel configuration
        /// </summary>
        /// <param name="sender">The Add button that was clicked</param>
        /// <param name="e"></param>
        /// 
     

        /// <summary>
        /// when a DGV-item is selected, take the necessary rows to compose the unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
          
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
                bool isChannel = row.GetType().Equals(typeof(LINAA.ChannelsRow));
             
                ///has errors
                if (row.HasErrors)
                {
                    ///is not a matrix row so exit and report
                 
                        Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                    
                        return;
                   
                }

            
                    if (MatSSF.UNIT == null)
                    {
                        this.AddUnitBn_Click(null, EventArgs.Empty);
                    }

                if (isChannel)
                {
                   
                    LINAA.ChannelsRow c = row as LINAA.ChannelsRow;
                    MatSSF.UNIT.ChannelsRow = c;
                    MatSSF.UNIT.SetChannel();
                }
             
                else
                {
                    LINAA.VialTypeRow v = row as LINAA.VialTypeRow;
                  
                    MatSSF.UNIT.SetVialContainer(ref v);
                }
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
            }


        }
        private void dgvUnitSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            //  DataGridViewRow r = dgv.Rows[e.RowIndex];
            DataRow row = Dumb.Cast<DataRow>(dgv.Rows[e.RowIndex]);

            //     LINAA lina = (LINAA)Interface.Get();

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

                ///has errors
                if (row.HasErrors)
                {
                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                //    return;
                }


                MatSSF.UNIT = row as LINAA.UnitRow;
                this.ucUnit.RefreshSSF();



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
            double kth = Dumb.GetControlAs<double>( kthB);
            string chfg =  cfgB.Text;

         //   LINAA.UnitDataTable dt = Interface.IDB.Unit;

            MatSSF.UNIT = Interface.IDB.Unit.NewUnitRow(kepi, kth, chfg);


            this.ucUnit.RefreshSSF();

           // row = u;



        }

        private void dgvMatrixSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;

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

                    colError = row.GetColumnError(dt.MatrixDensityColumn);
                    if (string.IsNullOrEmpty(colError)) ///should use density to massCalculate
                    {
                        ///do not exit, use preference auto-mass calculate
                    }
                    else ///density is null, should calculate density
                    {
                        ///do not exit, use calculate density from mass and dimensions

                    }


                }


                if (MatSSF.UNIT == null)
                {

                    AddUnitBn_Click(null, EventArgs.Empty);

                }


                LINAA.MatrixRow m = row as LINAA.MatrixRow;
                MatSSF.UNIT.MatrixRow = m;
                MatSSF.UNIT.SetMatrix();

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


            this.ucVcc.EndEdit();

            //  setUnit();

            Interface.IStore.SaveSSF(Offline, mf);


       
        }

        private void Calculate_Click(object sender, EventArgs e)
        {

            //Validate Binding sources
            this.ucUnit.UnitBS.EndEdit();

            this.ucMS.MatrixBS.EndEdit();


            this.ucVcc.EndEdit();

            this.ucUnit.DeLink();


            // errorB.Clear();

            //Go to Calculations/ Units Tab
            this.Tab.SelectedTab = this.CalcTab;

            //Clear InputFile RTF Control
            compositionsbox.Clear();


            this.progress.Value = 0;


            //Delink BS for SSF
         
            //1
            this.progress.PerformStep();
            Application.DoEvents();

            bool hide = false;

            try
            {
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

                bool runOk = DB.Tools.MatSSF.RUN(hide);
                //4
                this.progress.PerformStep();
                Application.DoEvents();

                if (runOk)
                {
                    DB.Tools.MatSSF.OUTPUT();

                    if (MatSSF.Table.Count == 0)
                    {
                        throw new SystemException("Problems Reading MATSSF Output\n");
                    }
                }
                else
                {
                    throw new SystemException("MATSSF is still calculating stuff...\n");
                    // errorB.Text += "MATSSF is still calculating stuff...\n";
                }
                //5
                this.progress.PerformStep();
                Application.DoEvents();

                MatSSF.CHILEAN();
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


      

        private void setUnitBindings()
        {


            //SET THE PREFERENCES


            this.loop.Checked = Interface.IPreferences.CurrentSSFPref.Loop;
            this.autoCalcDensity.Checked = Interface.IPreferences.CurrentSSFPref.CalcDensity;





            LINAA.UnitDataTable Unit = Interface.IDB.Unit;
            BindingSource bs = this.ucUnit.UnitBS;

            this.unitBN.BindingSource = bs;

            Hashtable bindings = Dumb.ArrayOfBindings(ref bs);

            string column;

            column = Unit.DiameterColumn.ColumnName;
            this.diameterbox.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.LenghtColumn.ColumnName;
            this.lenghtbox.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ChDiameterColumn.ColumnName;
            this.chdiamB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.ChLenghtColumn.ColumnName;
            this.chlenB.TextBox.DataBindings.Add(bindings[column] as Binding);
            column = Unit.MassColumn.ColumnName;
            this.massB.TextBox.DataBindings.Add(bindings[column] as Binding);
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

/*
            
            column = Interface.IDB.SSFPref.CalcDensityColumn.ColumnName;

            Binding b1 = new Binding("Checked", Interface.IPreferences.CurrentSSFPref, column);

         
            column = Interface.IDB.SSFPref.LoopColumn.ColumnName;

            Binding b2 = new Binding("Checked", Interface.IPreferences.CurrentSSFPref, column);

            */


           // this.autoCalcDensity.bin
        }


        private void loadDatabase()
        {
            // errorB.Clear();

            try
            {
                if (!Offline)
                {
                    Interface.IPopulate.IGeometry.PopulateUnits();
                }
                else //fix this
                {
                    Interface.IPopulate.IMain.Read(mf);
                       
                }

                ucVcc.Set(ref Interface);
                ucVcc.RowHeaderMouseClick = this.dgvItemSelected;

                ucUnit.Set(ref Interface);
                ucUnit.RowHeaderMouseClick = this.dgvUnitSelected;

                ucMS.Set(ref Interface);
                ucMS.RowHeaderMouseClick = this.dgvMatrixSelected;

                setUnitBindings();

                this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);


                Interface.IReport.Msg("Database", "Units were loaded!");
                //    this.currentUnit = (LINAA.UnitRow)Dumb.Cast<LINAA.UnitRow>(bs.Current);

                //   loadBoxesfromUnit();
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.Message + "\n" + ex.Source + "\n", "Error", false);
                //    errorB.Text += ex.Message + "\n" + ex.Source + "\n";
            }
        }

        private void checkedChanged(object sender, EventArgs e)
        {


            Interface.IPreferences.CurrentSSFPref.CalcDensity = this.autoCalcDensity.Checked;

            Interface.IPreferences.CurrentSSFPref.Loop = this.loop.Checked;

            Interface.IStore.SavePreferences();
        }
    }
}