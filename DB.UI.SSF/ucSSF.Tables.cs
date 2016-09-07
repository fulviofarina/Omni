using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using DB.Tools;
using Rsx;

namespace DB.UI
{
    public partial class ucSSF
    {
        /// <summary>
        /// sets the bindings only once
        /// </summary>
        private void setBindings()
        {
            //   errorB.Clear();

            MatSSF.Table = this.lINAA.MatSSF;

           
            Dumb.LinkBS(ref this.ChannelBS, this.lINAA.Channels);

            Dumb.LinkBS(ref this.MatrixBS, this.lINAA.Matrix);
            string column = this.lINAA.VialType.IsRabbitColumn.ColumnName;
            string innerRadCol = this.lINAA.VialType.InnerRadiusColumn.ColumnName + " asc";
            Dumb.LinkBS(ref this.VialBS, this.lINAA.VialType, column + " = " + "False", innerRadCol);
            Dumb.LinkBS(ref this.ContainerBS, this.lINAA.VialType, column + " = " + "True", innerRadCol);
            //  this.VialBS.Sort
          
            //  ChCfg.Items.AddRange(MatSSF.Types);

            DataSourceUpdateMode mo = DataSourceUpdateMode.OnPropertyChanged;
            bool t = true;
            string text = "Text";


            LINAA.UnitDataTable Unit = this.lINAA.Unit;


            BindingSource bs = this.unitBS;
          
        
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


            this.cfgB.ComboBox.Items.AddRange(MatSSF.Types);


            bs = this.MatrixBS;

            column = this.lINAA.Matrix.MatrixCompositionColumn.ColumnName;
            Binding mcompoBin = new Binding(text, bs, column, t, mo);
            this.matrixRTB.DataBindings.Add(mcompoBin);



            Dumb.LinkBS(ref this.unitBS, this.lINAA.Unit);




            BindingSource unitbs = this.unitBS;
            BindingSource ssfbs = this.MATSSFBS;

            //  ucUnit unit = new ucUnit();
            ucUnit.Set(ref unitbs, ref ssfbs);
            ucUnit.RowHeaderMouseClick = this.dgvUnitSelected;


       


          
           // this.dgvUnitSelected(null, args);

            //    this.currentUnit = (LINAA.UnitRow)Dumb.Cast<LINAA.UnitRow>(this.unitBS.Current);

            //   loadBoxesfromUnit();
        }




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
          
                bool m = string.IsNullOrWhiteSpace(mbox.Text);

              //  if (m) error.SetError(mbox.TextBox, "Null or empty");
             //   else
             //   {
                  //  error.SetError(mbox.TextBox, null);
              //  }

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