using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Interfaces;
using DB.Tools;
using Rsx;

using Msn;




namespace DB.UI
{
    public partial class ucSSF : UserControl
    {

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

        //  private string input = "MATSSF_INP.TXT";
        //  private string output = "MATSSF_LST.TXT";

        //  private String DirectoryPath = ;

        //    private LINAA Aux = null;
        private bool Offline = false;

        private static string mf =  preFolder + matssfFolder + "lims.xml";

        static Environment.SpecialFolder folder = Environment.SpecialFolder.Personal;
       
        static string matssfFolder = DB.Properties.Resources.MatSSFFolder;
        static string preFolder = Environment.GetFolderPath(folder);

        // private Msn msn = null;
        ///cant remember what is this
        private Interface Interface = null;

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

        
          
        }
        private static string rowWithError = "The selected row has some incomplete cells or cells with errors.\nPlease fix before selecting";
        private static string noTemplate = "No rows found. Add one";
        private static string Error = "Warning";
  
        /// <summary>
        /// Adds a new row of type VialType or Channels, which is either a Rabbit/Vial or a Channel configuration
        /// </summary>
        /// <param name="sender">The Add button that was clicked</param>
        /// <param name="e"></param>
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
        /// sets the bindings only once
        /// </summary>
        private void setBindings()
        {
         //   errorB.Clear();

         

                MatSSF.Table = this.lINAA.MatSSF;

                string text = "Text";

                Dumb.LinkBS(ref this.ChannelBS, this.lINAA.Channels);

                Dumb.LinkBS(ref this.MatrixBS, this.lINAA.Matrix);
                string column = this.lINAA.VialType.IsRabbitColumn.ColumnName;
                string innerRadCol = this.lINAA.VialType.InnerRadiusColumn.ColumnName + " asc";
                Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
                Dumb.LinkBS(ref this.ContainerBS, this.lINAA.VialType, column + " = " + "True", innerRadCol);
                //  this.VialBS.Sort
                column = this.lINAA.Matrix.MatrixCompositionColumn.ColumnName;
                Binding mcompoBin = new Binding(text, this.MatrixBS, column, true);
             
                //  ChCfg.Items.AddRange(MatSSF.Types);

                DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
                LINAA.UnitDataTable Unit = this.lINAA.Unit;
                BindingSource bs = this.unitBS;
                bool t = true;



                column = Unit.DiameterColumn.ColumnName;
                Binding diam = new Binding(text, bs, column, t, mo);
                column = Unit.LenghtColumn.ColumnName;
                Binding leng = new Binding(text, bs, column, t, mo);
                column = Unit.ChDiameterColumn.ColumnName;
                Binding chdiam = new Binding(text, bs, column, t, mo);
                column = Unit.ChLenghtColumn.ColumnName;
                Binding chleng = new Binding(text, bs, column, t, mo);
                column = Unit.MassColumn.ColumnName;
                Binding mass = new Binding(text, bs, column, t, mo);
                column = Unit.DensityColumn.ColumnName;
                Binding density = new Binding(text, bs, column, t, mo);
                column = Unit.ChCfgColumn.ColumnName;
                Binding cfg = new Binding(text, bs, column, t, mo);
                column = Unit.ContentColumn.ColumnName;
                Binding matrix = new Binding(text, bs, column, t, mo);
                column = Unit.NameColumn.ColumnName;
                Binding name = new Binding(text, bs, column, t, mo);
                column = Unit.kepiColumn.ColumnName;
                Binding kepi = new Binding(text, bs, column, t, mo);
                column = Unit.kthColumn.ColumnName;
                Binding kth = new Binding(text, bs, column, t, mo);


                column = Unit.LastCalcColumn.ColumnName;
                Binding lastcalbs = new Binding(text, bs, column, t, mo);
                column = Unit.LastChangedColumn.ColumnName;
                Binding lastchgbs = new Binding(text, bs, column, t, mo);


                this.lastCal.TextBox.DataBindings.Add(lastcalbs);
                this.lastChg.TextBox.DataBindings.Add(lastchgbs);

               this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);


            this.lenghtbox.TextBox.DataBindings.Add(leng);
                this.diameterbox.TextBox.DataBindings.Add(diam);
                this.chdiamB.TextBox.DataBindings.Add(chdiam);
                this.chlenB.TextBox.DataBindings.Add(chleng);
                this.massB.TextBox.DataBindings.Add(mass);
                this.densityB.TextBox.DataBindings.Add(density);
                this.cfgB.ComboBox.DataBindings.Add(cfg);
                this.matrixB.DataBindings.Add(matrix);
                this.kepiB.TextBox.DataBindings.Add(kepi);
                this.kthB.TextBox.DataBindings.Add(kth);
            this.matrixRTB.DataBindings.Add(mcompoBin);

            this.nameB.ComboBox.DataBindings.Add(name);




                Dumb.LinkBS(ref this.unitBS, this.lINAA.Unit);

                MouseEventArgs m = null;
                m = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
                DataGridViewCellMouseEventArgs args = null;
                args = new DataGridViewCellMouseEventArgs(-1, 0, 0, 0, m);
                this.dgvItemSelected(this.unitDGV, args);

                //    this.currentUnit = (LINAA.UnitRow)Dumb.Cast<LINAA.UnitRow>(this.unitBS.Current);

                //   loadBoxesfromUnit();
         
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

            //check if it is the matrix dgv

            DataGridViewRow r = dgv.Rows[e.RowIndex];

            DataRow row = Dumb.Cast<DataRow>(r);

            if (row.Table.Rows.Count == 0)
            {
          
                Interface.IReport.Msg(noTemplate, Error);
                return;

            }
         

            bool isUnit = dgv.Equals(this.unitDGV);
            bool isChannel = dgv.Equals(this.ChannelDGV);
            bool isMatrix = dgv.Equals(this.matrixDGV);

            if (row.HasErrors && !isMatrix)
            {
                Interface.IReport.Msg(rowWithError, Error);
                return;
            }

            try
            {
                if (!isUnit)
                {
                    LINAA.UnitRow u = MatSSF.UNIT;

                    if (u == null)
                    {
                        addNewVialChannel_Click(this.AddUnitBn, EventArgs.Empty);
                    }

                  
                    /*
                        if (string.IsNullOrWhiteSpace(matrixB.Text))
                        {
                            u.RowError = "Assign a matrix composition";
                        }
                        */

                }

                if (isUnit)
                {
                   // if (this.unitBS.Count == 0) return;
                    MatSSF.UNIT = row as LINAA.UnitRow;
                    MatSSF.ReadXML();

                    string column = this.lINAA.MatSSF.UnitIDColumn.ColumnName;
                    string sortCol = this.lINAA.MatSSF.TargetIsotopeColumn.ColumnName;
                    string unitID = MatSSF.UNIT.UnitID.ToString();

                    //  MatSSF.UNIT = u;

                    Dumb.LinkBS(ref this.MATSSFBS, this.lINAA.MatSSF, column + " is " + unitID, sortCol);

                    column = this.lINAA.VialType.VialTypeIDColumn.ColumnName;
                    this.VialBS.Position = this.VialBS.Find(column,MatSSF.UNIT.VialTypeID);
                 //   column = this.lINAA.VialType.VialTypeIDColumn.ColumnName;
                    this.ContainerBS.Position = this.ContainerBS.Find(column, MatSSF.UNIT.ContainerID);

                    column = this.lINAA.Matrix.MatrixIDColumn.ColumnName;
                    this.MatrixBS.Position = this.MatrixBS.Find(column, MatSSF.UNIT.MatrixID);

                    column = this.lINAA.Channels.ChannelsIDColumn.ColumnName;
                    this.ChannelBS.Position = this.ChannelBS.Find(column, MatSSF.UNIT.ChannelID);



                }
                else if (isChannel)
                {
                   // if (this.ChannelBS.Count == 0) return;
                    LINAA.ChannelsRow c = row as LINAA.ChannelsRow;
                    MatSSF.UNIT.ChannelsRow = c;
                    MatSSF.UNIT.SetChannel();
                }
                else if (isMatrix)
                {
                  //  if (this.MatrixBS.Count == 0) return;
                    LINAA.MatrixRow m = row as LINAA.MatrixRow;
                    MatSSF.UNIT.MatrixRow = m;
                    MatSSF.UNIT.SetMatrix();
                }
                else
                {
                 

                    LINAA.VialTypeRow v = row as LINAA.VialTypeRow;
                //    MatSSF.UNIT.VialTypeRow = v;
                    MatSSF.UNIT.SetVialContainer(ref v);
                }

            
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg(ex.StackTrace, ex.Message);
            }
        }

        /// <summary>
        /// Sets the new UnitRow if current is null
        /// </summary>
        /// <returns></returns>
      

        //
        //
        //

        /// <summary>
        /// Gets ToolStripTextBox control content as T-type
        /// </summary>
        /// <typeparam name="T">the type that you want to obtain from the control box Text Property</typeparam>
        /// <param name="mbox">the box</param>
        /// <returns></returns>
        public T getControlAs<T>(ref ToolStripTextBox mbox)
        {
            //   ToolStripTextBox mbox = (ToolStripTextBox)control;

            T mass = default(T);
            Type tipo = typeof(T);
            try
            {
                bool m = string.IsNullOrWhiteSpace(mbox.Text);

                if (m) error.SetError(mbox.TextBox, "Null or empty");
                else
                {
                    error.SetError(mbox.TextBox, null);
                }

                if (tipo.Equals(typeof(double)))
                {
                    double massAux = Convert.ToDouble(mbox.Text);

                    mass = (T)Convert.ChangeType(massAux, typeof(T));
                }
                else if (tipo.Equals(typeof(string)))
                {
                    string massAux = mbox.Text.ToString();

                    mass = (T)Convert.ChangeType(massAux, typeof(T));
                }
            }
            catch (SystemException ex)
            {
                //errorB.Text += ex.Message.ToString() + "\n";
            }
            return mass;
        }

        private void validateBS()
        {
            this.MatrixBS.EndEdit();
            this.VialBS.EndEdit();
            this.unitBS.EndEdit();
            this.ChannelBS.EndEdit();
            this.ContainerBS.EndEdit();
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
                    this.lINAA.Clear();

                    if (System.IO.File.Exists(mf))
                    {
                        this.lINAA.ReadXml(mf, XmlReadMode.InferTypedSchema);
                    }


                }


                
                setBindings();

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

        /*
            private void OFD_FileOk(object sender, CancelEventArgs e)
            {
                errorbox.Clear();
                try
                {
                      string filepath = OFD.FileName;
                      System.IO.File.Delete(DirectoryPath + input);
                      System.IO.File.Copy(filepath, DirectoryPath + input);
                      LoadInputBox();
                }
                catch (System.Exception ex)
                {
                      errorbox.Text += ex.Message + "\n" + ex.Source + "\n";
                }
            }

            private void SFD_FileOk(object sender, CancelEventArgs e)
            {
                errorbox.Clear();
                try
                {
                      string filepath = SFD.FileName;
                      compositionsbox.SaveFile(filepath, RichTextBoxStreamType.PlainText);
                      string destfile = DirectoryPath + input;
                      System.IO.File.Delete(destfile);
                      System.IO.File.Copy(filepath, destfile);
                      LoadInputBox();
                }
                catch (System.Exception ex)
                {
                      errorbox.Text += ex.Message + "\n" + ex.Source + "\n";
                }
            }
            */

        /*
    private void saveSSFTable()
    {
        //   DataTable table = (DataTable)this.MATSSFBS.DataSource;

        //     LINAA.MatSSFDataTable sf = SSF as LINAA.MatSSFDataTable;
        //  sf.GtColumn.Expression = Gtbox.Text;
        //    sf.pColumn.Expression = densitybox2.Text;

        //    Aux.MatSSF.WriteXml(DirectoryPath + "export.xml");

        //    System.IO.File.Copy(DirectoryPath + "export.xml", DirectoryPath + this.cfgFile.Text + ".XML");
    }
    */
        /*
            private void cfgList_SelectedIndexChanged(object sender, EventArgs e)
            {
                errorbox.Clear();
                try
                {
                    ListViewItem firstselected = cfgList.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
                    if (firstselected == null) return;
                    this.cfgFile.Text = firstselected.Text;
                    string filepath = DirectoryPath + this.cfgFile.Text + ".MSF";
                    System.IO.File.Delete(DirectoryPath + input);
                    System.IO.File.Copy(filepath, DirectoryPath + input);
                    //LoadInputBox();
                    boxesLoading = true;
                    loadBoxes();
                    boxesLoading = false;
                    loadSSFTable();
                }
                catch (System.Exception ex)
                {
                    errorbox.Text += ex.Message + "\n" + ex.Source + "\n";
                }
            }
        */
        /*
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //    SFD.ShowDialog();
            errorB.Clear();
            //      if (cfgFile.Text.Equals(string.Empty))
           // {
                errorB.Text = "Configuration Name cannot be empty! Not saved";
              //  return;
           // }
            try
            {
                string destfile = DirectoryPath + input;

                //       string filepath = DirectoryPath + this.cfgFile.Text + ".MSF";
                //      compositionsbox.SaveFile(filepath, RichTextBoxStreamType.PlainText);

                System.IO.File.Delete(destfile);
                //      System.IO.File.Copy(filepath, destfile);

                //    LoadInputBox();

                 System.IO.File.Delete(DirectoryPath + "export.xml");
            }
            catch (System.Exception ex)
            {
                errorB.Text = ex.Message + "\n" + ex.Source + "\n";
            }

            //  fillCfgList();
        }
        */

        /*
        private void viewOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorB.Clear();
            try
            {
                Dumb.Process(new System.Diagnostics.Process(), DirectoryPath, "notepad.exe", output, false, false, 0);
            }
            catch (SystemException ex)
            {
                errorB.Text += ex.Message + "\n" + ex.Source + "\n";
            }
        }
        */

        //NOT USED YET
        private void loadBoxesfromFile()
        {
            string[] lines = compositionsbox.Lines;
            int index = lines.ToList().FindIndex(o => o.Equals(""));

            this.massB.Text = lines[index + 1];
            this.diameterbox.Text = lines[index + 2];
            this.lenghtbox.Text = lines[index + 3];
            string[] chs = lines[index + 4].Split(',');
            this.chdiamB.Text = chs[1];
            this.chlenB.Text = chs[2];
            this.cfgB.Text = chs[0];
        }
    }
}