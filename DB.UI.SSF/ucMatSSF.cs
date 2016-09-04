using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Tools;
using Rsx;
using DB.Interfaces;

/*

namespace DB.UI
{
    public partial class ucMatSSF : UserControl
    {
        //understood
          private string input = "MATSSF_INP.TXT";
          private string output = "MATSSF_LST.TXT";

        bool go = true;

          private String DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + DB.Properties.Resources.MatSSFFolder;

          private LINAA Aux = null;

            private bool Offline = false;

        public ucMatSSF(ref LINAA Linaa, bool offline)
        {
            InitializeComponent();

            MatSSF.startupPath = DirectoryPath;

			Auxiliar form = new Auxiliar();
                  form.Populate(this);
			form.Text = "MATSSF Panel";
			form.Show();

                  Offline = offline;

                  mf = DirectoryPath + "lims.xml";

                  LoadDatabase(ref Linaa);
        }

        string mf ;

        private void LoadDatabase(ref LINAA Linaa)
        {
              errorbox.Clear();

              try
              {
                  Binding mcompoBin = new Binding("Text", this.MatrixBS, "MatrixComposition", true);
                  Binding mnameBin = new Binding("Text", this.MatrixBS, "MatrixName", true);

                  this.matrixRTB.DataBindings.Add(mcompoBin);
                  this.MatrixNameSelected.TextBox.DataBindings.Add(mnameBin);

                  Binding diam = new Binding("Text", bs, "Diameter", true);
                  Binding leng = new Binding("Text", bs, "Lenght", true);
                  Binding chdiam = new Binding("Text", bs, "ChDiameter", true);
                  Binding chleng = new Binding("Text", bs, "ChLenght", true);
                  Binding mass = new Binding("Text", bs, "Mass", true);

                  this.lenghtbox.TextBox.DataBindings.Add(leng);
                  this.diameterbox.TextBox.DataBindings.Add(diam);
                  this.chdiameterbox.TextBox.DataBindings.Add(chdiam);
                  this.chlenghtbox.TextBox.DataBindings.Add(chleng);
                  this.massbox.TextBox.DataBindings.Add(mass);

              if (!Offline)
              {
                    this.lINAA.Dispose();
                    this.lINAA = null;

                    this.lINAA = Linaa;
                     DB.IPopulate ipo = this.lINAA;

                   //  ipo.PopulateMatrix();
                    // ipo.PopulateVials();
                     ipo.PopulateUnits();
              }
              else
              {
                    this.lINAA = new LINAA();

                    if (System.IO.File.Exists(mf))
                    {
                          this.lINAA.ReadXml(mf, XmlReadMode.InferTypedSchema);
                    }
              }

              Aux = (LINAA)this.lINAA.Clone();
              Aux.Clear();

              Rsx.Dumb.LinkBS(ref this.MatrixBS, this.lINAA.Matrix);
              Rsx.Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType);
              Rsx.Dumb.LinkBS(ref this.ContainerBS, this.lINAA.VialType);
              Rsx.Dumb.LinkBS(ref bs, this.lINAA.Unit);
              Dumb.LinkBS(ref this.MATSSFBS, Aux.MatSSF);
              }
              catch (System.Exception ex)
              {
                    errorbox.Text += ex.Message + "\n" + ex.Source + "\n";
              }
        }

        private void matrixBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.MatrixBS.EndEdit();
            this.VialBS.EndEdit();

           SetUnit();

            if (!Offline)
            {
                  this.lINAA.IStore.Save<LINAA.MatrixDataTable>();
                  this.lINAA.IStore.Save<LINAA.VialTypeDataTable>();
                  this.lINAA.IStore.Save<LINAA.UnitDataTable>();
            }
            else
            {
                  System.IO.File.Delete(mf);

                  this.lINAA.WriteXml(mf, XmlWriteMode.WriteSchema);
            }
            this.lINAA.Msg("Saved", "Database updated!");
        }

        private bool SetUnit()
        {
            if (string.IsNullOrWhiteSpace(matrixbox.Text)) return false;

            LINAA.UnitRow u = currentUnit;

            if (u == null)
            {
                u = this.lINAA.Unit.NewUnitRow();
                this.lINAA.Unit.AddUnitRow(u);
            }

            u.Mass = getControlAs<double>(ref massbox);
            u.Lenght = getControlAs<double>(ref lenghtbox);
            u.Diameter = getControlAs<double>(ref diameterbox);
            u.Content = matrixbox.Text;

            u.ChDiameter = getControlAs<double>(ref chdiameterbox);
            u.ChLenght = getControlAs<double>(ref chlenghtbox);

            u.ChCfg = Convert.ToInt16(chtypebox.Text[0]);

            currentUnit = u;

            return true;
        }

        private LINAA.UnitRow currentUnit = null;

        private void Calculate_Click(object sender, EventArgs e)
        {
			Dumb.DeLinkBS(ref this.MATSSFBS);

            errorbox.Clear();

            bool hide = false;

            LINAA.SubSamplesRow aux = null;
            LINAA.MatSSFDataTable table = Aux.MatSSF;

            table.Clear();
            compositionsbox.Clear();

            try
            {
                System.IO.File.Delete(DirectoryPath + "MATSSF_INP.txt");

                if (SetUnit() == true)
                {
                    MatSSF.UNIT = currentUnit;
                    MatSSF.INPUT();
                    //arreglar esto
                    compositionsbox.LoadFile(DirectoryPath + input, RichTextBoxStreamType.PlainText);

                    if (DB.Tools.MatSSF.RUN(hide))
                    {
                        DB.Tools.MatSSF.OUTPUT(ref table, ref aux);

                        if (table.Count != 0)
                        {
                            Gtbox.Text = table.GtDensity[0];
                            densitybox2.Text = table.GtDensity[1];

                            compareDensities();
                        }
                        else errorbox.Text += "Problems Reading MATSSF Output\n";
                    }
                    else errorbox.Text += "MATSSF is still calculating stuff...\n";
                }
                else errorbox.Text += "Matrix Composition is empty\n";
            }
            catch (SystemException ex)
            {
                errorbox.Text += ex.Message + "\n" + ex.Source+"\n";
            }

            Dumb.LinkBS(ref this.MATSSFBS, Aux.MatSSF);
        }

        private void compareDensities()
        {
            double dens2 = getControlAs<double>(ref densitybox2);
            double dens1 = getControlAs<double>(ref densitybox);

            if (Math.Abs((dens1 / dens2) - 1) * 100 > 10)
            {
                this.error.SetError(densitybox2.TextBox, "Calculated density does not match input density");
            }
            else this.error.SetError(densitybox2.TextBox, null);
        }

        private void loadBoxes()
        {
            string[] lines = compositionsbox.Lines;
            int index = lines.ToList().FindIndex(o => o.Equals(""));

            this.massbox.Text = lines[index + 1];
            this.diameterbox.Text = lines[index + 2];
            this.lenghtbox.Text = lines[index + 3];
            string[] chs = lines[index + 4].Split(',');
            this.chdiameterbox.Text = chs[1];
            this.chlenghtbox.Text = chs[2];
            this.chtypebox.Text = chs[0];
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    SFD.ShowDialog();
            errorbox.Clear();
            if (cfgFile.Text.Equals(string.Empty))
            {
                errorbox.Text = "Configuration Name cannot be empty! Not saved";
                return;
            }
                try
            {
                string destfile = DirectoryPath + input;

                string filepath = DirectoryPath + this.cfgFile.Text + ".MSF";
                 compositionsbox.SaveFile( filepath, RichTextBoxStreamType.PlainText);

                 System.IO.File.Delete(destfile);
                 System.IO.File.Copy(filepath, destfile);

             //    LoadInputBox();

                 saveSSFTable();
            }
            catch (System.Exception ex)
            {
                errorbox.Text = ex.Message + "\n" + ex.Source + "\n";
            }

          //  fillCfgList();
        }

        private void errorbox_DoubleClick(object sender, EventArgs e)
        {
            errorbox.Clear();
        }

        private void viewOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorbox.Clear();
            try
            {
                Dumb.Process(new System.Diagnostics.Process(), DirectoryPath, "notepad.exe", output, false, false, 0);
            }
            catch (SystemException ex)
            {
                errorbox.Text += ex.Message + "\n" + ex.Source + "\n";
            }
        }

		private void dgvItemSelected(object sender, DataGridViewCellMouseEventArgs e)
		{
		      if (e.RowIndex < 0) return;

			   DataGridView dgv= sender as DataGridView;

            //check if it is the matrix dgv

               DataGridViewRow r = dgv.Rows[e.RowIndex];

               go = false; //de-activate text_changed

               errorbox.Clear();

               try
               {
                   if (dgv.Equals(this.matrixDGV))
                   {
                       if (this.MatrixBS.Count == 0) return;
                       LINAA.MatrixRow m = Dumb.Cast<LINAA.MatrixRow>(r);
                       m.RowError = string.Empty;
                       if (m.IsMatrixCompositionNull())
                       {
                           m.RowError = "Assign a matrix composition";
                           matrixbox.Text = string.Empty;
                       }
                       else
                       {
                           matrixbox.Text = m.MatrixComposition;
                           double density = m.MatrixDensity;
                           if (density != 0)
                           {
                               sender = this.lenghtbox;
                               densitybox.Text = Decimal.Round(Convert.ToDecimal(density), 2).ToString();
                           }
                           else sender = densitylabel;
                       }
                   }
                   else
                   {
                       if (dgv.Equals(vialDGV))
                       {
                           if (this.VialBS.Count == 0) return;
                           LINAA.VialTypeRow v = Dumb.Cast<LINAA.VialTypeRow>(r);
                           v.RowError = string.Empty;
                           if (v.IsInnerRadiusNull() || v.InnerRadius == 0) v.RowError = "Assign a radius in mm\n";
                           else this.diameterbox.Text = (v.InnerRadius * 2.0).ToString();
                           if (v.IsMaxFillHeightNull() || v.MaxFillHeight == 0) v.RowError += "Assign a height or lenght in mm\n";
                           else this.lenghtbox.Text = v.MaxFillHeight.ToString();
                           if (!string.IsNullOrWhiteSpace(v.RowError))
                           {
                               errorbox.Text = v.RowError;
                           }
                           sender = this.lenghtbox;
                       }
                       else
                       {
                           if (this.ContainerBS.Count == 0) return;

                           LINAA.VialTypeRow v = Dumb.Cast<LINAA.VialTypeRow>(r);
                           v.RowError = string.Empty;
                           if (v.IsInnerRadiusNull() || v.InnerRadius == 0) v.RowError = "Assign a channel radius in mm\n";
                           else this.chdiameterbox.Text = (v.InnerRadius * 2).ToString();
                           if (v.IsMaxFillHeightNull() || v.MaxFillHeight == 0) v.RowError += "Assign a channel height or lenght in mm\n";
                           else this.chlenghtbox.Text = v.MaxFillHeight.ToString();
                           if (!string.IsNullOrWhiteSpace(v.RowError))
                           {
                               errorbox.Text = v.RowError;
                           }
                           sender = null;
                       }
                   }
               }
               catch (System.Exception ex)
               {
                   errorbox.Text = ex.Message + "\n" ;
               }

               go = true;
               if (sender!=null)  densitylabel_Click(sender, e); //check textboxes

                SetUnit();
		}

            private void saveSSFTable()
            {
             //   DataTable table = (DataTable)this.MATSSFBS.DataSource;

           //     LINAA.MatSSFDataTable sf = SSF as LINAA.MatSSFDataTable;
              //  sf.GtColumn.Expression = Gtbox.Text;
            //    sf.pColumn.Expression = densitybox2.Text;

                System.IO.File.Delete(DirectoryPath + "export.xml");

                Aux.MatSSF.WriteXml(DirectoryPath + "export.xml");

                System.IO.File.Copy(DirectoryPath + "export.xml", DirectoryPath + this.cfgFile.Text + ".XML");
            }
            private void loadSSFTable()
            {
        //        DataTable table = (DataTable);

         //       LINAA.MatSSFDataTable sf = SSF;

                string filepath = DirectoryPath + this.cfgFile.Text + ".XML";
                bool exist = System.IO.File.Exists(filepath);
                Aux.MatSSF.Clear();
                if (exist) Aux.MatSSF.ReadXml(filepath);
            }

            bool boxesLoading = false;

            private void densitylabel_Click(object sender, EventArgs e)
            {
                // que es esto?
                if (!go) return;

                errorbox.Clear();
                bool getMass = false;
                //true if the shape has changed (given a mass, recalculate density)
                if (sender.Equals(diameterbox) || sender.Equals(lenghtbox) || sender.Equals(densitybox)) getMass = true;

                //if (getMass &&  string.IsNullOrWhiteSpace(((ToolStripTextBox)sender).Text)) return;

                double lenght = getControlAs<double>(ref lenghtbox);
                double diameter = getControlAs<double>(ref diameterbox);

                double Vol = getVolumen(diameter, lenght);
                double mass = getControlAs<double>(ref massbox);

                try
                {
                    if (sender.Equals(densitylabel) || sender.Equals(massbox))
                    {
                        decimal density = findDensity(mass, Vol);
                        go = false;
                        densitybox.Text = Decimal.Round(density, 2).ToString();
                        go = true;
                    }
                    else if (getMass)
                    {
                        double density = getControlAs<double>( ref densitybox);
                        if (density != 0 & Vol != 0)
                        {
                            mass = Vol * density;
                            go = false;
                            massbox.Text = Decimal.Round(Convert.ToDecimal(mass), 2).ToString();
                            go = true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    errorbox.Text += ex.Message + "\n";
                }
            }

            private decimal findDensity(double mass, double Vol)
            {
                decimal density = 0;

                try
                {
                    if (mass != 0 && Vol != 0) density = Convert.ToDecimal(mass / Vol);
                }
                catch (SystemException ex)
                {
                    errorbox.Text += ex.Message.ToString() + "\n";
                }
                return density;
            }

            public double getVolumen(double diameter, double lenght)
            {
                double volumen = 0;

                    if (diameter != 0 && lenght != 0)
                    {
                        volumen = (diameter * diameter * Math.PI * 0.5 * 0.5 * lenght);
                    }

                return volumen;
            }
            public T getControlAs<T>(ref ToolStripTextBox mbox)
            {
             //   ToolStripTextBox mbox = (ToolStripTextBox)control;

                T mass=default(T);
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

                       mass= (T)Convert.ChangeType(massAux, typeof(T));
                    }
                    else if (tipo.Equals(typeof(string)))
                    {
                        string massAux = mbox.Text.ToString();

                        mass = (T)Convert.ChangeType(massAux, typeof(T));
                    }
                }
                catch (SystemException ex)
                {
                    errorbox.Text += ex.Message.ToString() + "\n";
                }
                return mass;
            }
    }
}

 *
 *
 */

namespace DB.UI
{
    public partial class ucMatSSF : UserControl
    {
     

      //  private string input = "MATSSF_INP.TXT";
      //  private string output = "MATSSF_LST.TXT";

      //  private String DirectoryPath = ;

        //    private LINAA Aux = null;
        private bool Offline = false;

        private string mf;
    
        ///cant remember what is this
        private Interface Interface = null;

     
        public ucMatSSF(ref LINAA Linaa, bool offline)
        {
            InitializeComponent();
            object db = Linaa;
            Interface = new Interface(ref db);


            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + DB.Properties.Resources.MatSSFFolder;

            MatSSF.StartupPath = dir;

            Form form = new Form();
            form.Controls.Add(this);// Populate(this);
            form.Text = "MATSSF Panel";
            form.Show();

            Offline = offline;

            mf = dir + "lims.xml";

            LoadDatabase();

            setBindings();

        }



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

            //IS A VIAL OR CONTAINER
            if (!sender.Equals(this.addChParBn))
            {
                if (sender.Equals(this.AddUnitBn))
                {

                 
                    double kepi = getControlAs<double>(ref kepiB);
                    double kth = getControlAs<double>(ref kthB);


                    LINAA.UnitRow u = this.lINAA.Unit.NewUnitRow();
                    u.Name = "New Unit";
                    u.kepi = kepi;
                    u.kth = kth;
                    u.RowError = string.Empty;
                    //  u.ChCfg = getControlAs<string>(ref cfgB);
                    this.lINAA.Unit.AddUnitRow(u);
                 
                    MatSSF.UNIT = u;


                    colName = this.lINAA.Unit.UnitIDColumn.ColumnName;
                    bs = this.unitBS;
                    idnumber = MatSSF.UNIT.UnitID;

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
             //   }
                    //  newIndex = this.ChannelBS.Find(colName, ch.ChannelsID);
               // this.ChannelBS.Position = newIndex;
            }

            int newIndex = bs.Find(colName, idnumber);
            bs.Position = newIndex;


        }
      


        /// <summary>
        /// sets the bindings only once
        /// </summary>
        private void setBindings()
        {
            errorB.Clear();

            try
            {
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

                this.matrixRTB.DataBindings.Add(mcompoBin);

                //  ChCfg.Items.AddRange(MatSSF.Types);

                this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);





                DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
                LINAA.UnitDataTable Unit = this.lINAA.Unit;
                BindingSource bs = this.unitBS;
                column = Unit.DiameterColumn.ColumnName;
                bool t = true;

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
            catch (System.Exception ex)
            {
                errorB.Text += ex.Message + "\n" + ex.Source + "\n";
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

            DataGridView dgv = sender as DataGridView;

            //check if it is the matrix dgv

            DataGridViewRow r = dgv.Rows[e.RowIndex];

            errorB.Clear();

            try
            {
                if (!dgv.Equals(this.unitDGV))
                {
                    setUnit();

                }



                if (dgv.Equals(this.unitDGV))
                {
                    if (this.unitBS.Count == 0) return;
                    MatSSF.UNIT  = Dumb.Cast<LINAA.UnitRow>(r);
                    MatSSF.ReadXML();

                    string column = this.lINAA.MatSSF.UnitIDColumn.ColumnName;
                    string sortCol = this.lINAA.MatSSF.TargetIsotopeColumn.ColumnName;
                    string unitID = MatSSF.UNIT.UnitID.ToString();

                    //  MatSSF.UNIT = u;
                   

                    Dumb.LinkBS(ref this.MATSSFBS, this.lINAA.MatSSF, column + " is " + unitID, sortCol);

                }
                else if (dgv.Equals(this.ChannelDGV))
                {
                    if (this.ChannelBS.Count == 0) return;
                    LINAA.ChannelsRow c = Dumb.Cast<LINAA.ChannelsRow>(r);
                     MatSSF.UNIT.SetChannel(ref c);

                }
                else if (dgv.Equals(this.matrixDGV))
                {
                    if (this.MatrixBS.Count == 0) return;
                    LINAA.MatrixRow m = Dumb.Cast<LINAA.MatrixRow>(r);
                     MatSSF.UNIT.SetMatrix(ref m);
                }
                else
                {

                    if (dgv.Equals(vialDGV))
                    {
                        if (this.VialBS.Count == 0) return;
                      
                    }
                    else
                    {
                        if (this.ContainerBS.Count == 0) return;

                       
                    }

                    LINAA.VialTypeRow v = Dumb.Cast<LINAA.VialTypeRow>(r);

                    MatSSF.UNIT.SetVialContainer(ref v);



                }



                DataRow row = Dumb.Cast<DataRow>(r);

                if (row != null) errorB.Text = row.RowError;


            }
            catch (System.Exception ex)
            {

                errorB.Text = ex.Message + "\n";
            }
        }

     

        /// <summary>
        /// Sets the new UnitRow if current is null
        /// </summary>
        /// <returns></returns>
        private bool setUnit()
        {

            LINAA.UnitRow u = MatSSF.UNIT;

            if (u == null)
            {

                addNewVialChannel_Click(this.AddUnitBn, EventArgs.Empty);

            }
           
            //should check for errors differently, inside DB namespace
            //fix this
            if (string.IsNullOrWhiteSpace(matrixB.Text))
            {
                u.RowError = "Assign a matrix composition";
            }

            return true;
        }


      


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
                errorB.Text += ex.Message.ToString() + "\n";
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



        private void LoadDatabase()
        {
           // errorB.Clear();

            try
            {
              

                if (!Offline)
                {
                    this.lINAA.Dispose();
                    this.lINAA = null;

                    this.lINAA = (LINAA)Interface.Get();
                  
                    Interface.IPopulate.IGeometry.PopulateUnits();

                    
                   
                }
                else
                {
                    this.lINAA = new LINAA();

                    if (System.IO.File.Exists(mf))
                    {
                        this.lINAA.ReadXml(mf, XmlReadMode.InferTypedSchema);
                    }
                }

                MatSSF.Table = this.lINAA.MatSSF;

                Interface.IReport.Msg("Database", "Units were loaded!");
                //    this.currentUnit = (LINAA.UnitRow)Dumb.Cast<LINAA.UnitRow>(bs.Current);

                //   loadBoxesfromUnit();
            }
            catch (System.Exception ex)
            {
                Interface.IReport.Msg("Error", ex.Message + "\n" + ex.Source + "\n");
              //  errorB.Text += ex.Message + "\n" + ex.Source + "\n";
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

            errorB.Clear();

          
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

                errorB.Text += ex.Message + "\n" + ex.Source + "\n";
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