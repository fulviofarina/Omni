using System;
using System.Windows.Forms;

using DB.Tools;
using System.Data;
using Rsx;
using Msn;

namespace DB.UI
{
    public partial class ucSSF : UserControl
    {


        private void dgvUnitSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            //  DataGridViewRow r = dgv.Rows[e.RowIndex];
            DataRow row = Dumb.Cast<DataRow>(dgv.Rows[e.RowIndex]);

            LINAA lina = (LINAA)Interface.Get();



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


                // if (this.unitBS.Count == 0) return;
                MatSSF.UNIT = row as LINAA.UnitRow;
                MatSSF.ReadXML();


                LINAA.UnitRow u = MatSSF.UNIT;
                string column = lina.MatSSF.UnitIDColumn.ColumnName;
                string sortCol = lina.MatSSF.TargetIsotopeColumn.ColumnName;
                string unitID = u.UnitID.ToString();

                Dumb.LinkBS(ref this.MATSSFBS, lina.MatSSF, column + " is " + unitID, sortCol);



                column = lina.VialType.VialTypeIDColumn.ColumnName;
                int id = u.VialTypeID;
                this.VialBS.Position = this.VialBS.Find(column, id);
                id = u.ContainerID;
                this.ContainerBS.Position = this.ContainerBS.Find(column, id);
                column = lina.Matrix.MatrixIDColumn.ColumnName;
                id = u.MatrixID;
                this.MatrixBS.Position = this.MatrixBS.Find(column, id);
                column = lina.Channels.ChannelsIDColumn.ColumnName;
                id = u.ChannelID;
                this.ChannelBS.Position = this.ChannelBS.Find(column, id);
                if (u.HasErrors)
                {
                    Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors

                }


            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
            }


        }



        #region variables
        private bool Offline = false;
        private static string mf = preFolder + matssfFolder + "lims.xml";
        private static Environment.SpecialFolder folder = Environment.SpecialFolder.Personal;
        private static string matssfFolder = DB.Properties.Resources.MatSSFFolder;
        private static string preFolder = Environment.GetFolderPath(folder);
        private Interface Interface = null;

        private static string rowWithError = "The selected row has some incomplete cells or cells with errors.\nPlease fix before selecting";
        private static string noTemplate = "No rows found. Add one";
        private static string Error = "Warning";


#endregion


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

            this.lINAA.Dispose();
            this.lINAA = null;
            this.lINAA = Linaa;

            object db = Linaa;
            Interface = new Interface(ref db);

            Offline = offline;

            System.Windows.Forms.Form form = null;

            Pop msn = null;
            msn = this.lINAA.Msn;
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
        private void addNewVialChannel_Click(object sender, EventArgs e)
        {
            BindingSource bs = null;
            string colName = string.Empty;
            object idnumber = null;
            DataRow row = null;
            //IS A VIAL OR CONTAINER
            if (!sender.Equals(this.addChParBn))
            {
                if (sender.Equals(this.AddUnitBn))
                {
                    double kepi = getControlAs<double>(ref kepiB);
                    double kth = getControlAs<double>(ref kthB);

                    LINAA.UnitRow u = this.lINAA.Unit.NewUnitRow();

                    u.kepi = kepi;
                    u.kth = kth;
                    u.RowError = string.Empty;
                    //  u.ChCfg = getControlAs<string>(ref cfgB);
                    this.lINAA.Unit.AddUnitRow(u);

                    MatSSF.UNIT = u;

                    colName = this.lINAA.Unit.UnitIDColumn.ColumnName;
                    bs = this.unitBS;
                    idnumber = MatSSF.UNIT.UnitID;

                    row = u;
                }
                else
                {
                    bool isRabbit = !sender.Equals(this.bnVialAddItem);

                    LINAA.VialTypeRow v = this.lINAA.VialType.NewVialTypeRow();
                    v.IsRabbit = isRabbit;
                    this.lINAA.VialType.AddVialTypeRow(v);

                    colName = this.lINAA.VialType.VialTypeIDColumn.ColumnName;

                    if (isRabbit)
                    {
                        bs = this.ContainerBS;
                    }
                    else
                    {
                        bs = this.VialBS;
                    }
                    idnumber = v.VialTypeID;

                    row = v;
                }
            }
            //IS A CHANNEL
            else
            {
                //  {
                LINAA.ChannelsRow ch = this.lINAA.Channels.NewChannelsRow();
                this.lINAA.Channels.AddChannelsRow(ch);

                colName = this.lINAA.Channels.ChannelsIDColumn.ColumnName;
                bs = this.ChannelBS;
                idnumber = ch.ChannelsID;

                row = ch;
                //   }
                //  newIndex = this.ChannelBS.Find(colName, ch.ChannelsID);
                // this.ChannelBS.Position = newIndex;
            }

            if (row.HasErrors)
            {
                Interface.IReport.Msg(rowWithError, Error);
            }

            int newIndex = bs.Find(colName, idnumber);
            bs.Position = newIndex;
        }


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
                bool isChannel = dgv.Equals(this.ChannelDGV);
                bool isMatrix = dgv.Equals(this.matrixDGV);

                ///has errors
                if (row.HasErrors)
                {
                    ///is not a matrix row so exit and report
                    if (!isMatrix)
                    {
                        Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                    
                        return;
                    }
                    else ///it is a matrix row, so check if only density has error
                    {

                        string colError = row.GetColumnError(this.lINAA.Matrix.MatrixCompositionColumn);
                        if (!string.IsNullOrEmpty(colError)) ///matrix content has error, exit and report

                        {

                            Interface.IReport.Msg(rowWithError, Error); ///cannot process because it has errors
                       
                            return;

                        }

                        colError = row.GetColumnError(this.lINAA.Matrix.MatrixDensityColumn);
                        if (string.IsNullOrEmpty(colError)) ///should use density to massCalculate
                        {
                            ///do not exit, use preference auto-mass calculate
                        }
                        else ///density is null, should calculate density
                        {
                            ///do not exit, use calculate density from mass and dimensions

                        }

                    }
                }

            
                    if (MatSSF.UNIT == null)
                    {
                        addNewVialChannel_Click(this.AddUnitBn, EventArgs.Empty);
                    }

                if (isChannel)
                {
                   
                    LINAA.ChannelsRow c = row as LINAA.ChannelsRow;
                    MatSSF.UNIT.ChannelsRow = c;
                    MatSSF.UNIT.SetChannel();
                }
                else if (isMatrix)
                {
                   
                    LINAA.MatrixRow m = row as LINAA.MatrixRow;
                    MatSSF.UNIT.MatrixRow = m;
                    MatSSF.UNIT.SetMatrix();
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



        private void SaveItem_Click(object sender, EventArgs e)
        {
            // this.currentUnit.LastChanged = DateTime.Now;

            this.Validate();

            validateBS();
            //  setUnit();

            if (!Offline)
            {
                Interface.IStore.Save<LINAA.MatrixDataTable>();
                Interface.IStore.Save<LINAA.VialTypeDataTable>();
                Interface.IStore.Save<LINAA.UnitDataTable>();
                Interface.IStore.Save<LINAA.ChannelsDataTable>();
            }
            else
            {
                System.IO.File.Delete(mf);

                this.lINAA.WriteXml(mf, XmlWriteMode.WriteSchema);
            }
            Interface.IReport.Msg("Database", "Database updated!");
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            this.progress.Value = 0;

            //Validate Binding sources
            this.validateBS();

            // errorB.Clear();

            //Go to Calculations/ Units Tab
            this.Tab.SelectedTab = this.CalcTab;

            //Clear InputFile RTF Control
            compositionsbox.Clear();

            //Delink BS for SSF
            Dumb.DeLinkBS(ref this.MATSSFBS);
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
                this.lINAA.AddException(ex);

                //    errorB.Text += ex.Message + "\n" + ex.Source + "\n";
            }

            MatSSF.WriteXML();
            SaveItem_Click(sender, e);
            Dumb.LinkBS(ref this.MATSSFBS, this.lINAA.MatSSF);
            //7
            this.progress.PerformStep();
            Application.DoEvents();
        }



    }
}