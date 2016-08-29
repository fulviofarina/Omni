using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace k0X
{
    public partial class ucEffiCOIS : UserControl
    {

        public DB.LINAA.efficienciesDataTable efficiencies2 = new DB.LINAA.efficienciesDataTable();


        public ucEffiCOIS()
        {
            InitializeComponent();

            ImportEffi.Enabled = false;
            EraseEffi.Enabled = false;
        }

        private void BindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Validate();
            this.BS2.EndEdit();

                     
         //   k0efficienciesTA.Update(efficiencies2);


            Cursor.Current = Cursors.Default;
        }

      
        private void Import_Click(object sender, EventArgs e)
        {
            progress.Value = 0;
            Cursor.Current = Cursors.WaitCursor;
           

            try
            {
               
                    
                    progress.PerformStep();
                    Insert();
                    progress.PerformStep();
                   
               
             }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                progress.Value = 0;
            }
            
            Cursor.Current = Cursors.Default;
            Reload_Click(sender, e);
         
        }

       
        private void Reload_Click(object sender, EventArgs e)
        {
            progress.Value = 0;
            Cursor.Current = Cursors.WaitCursor;

            if (detbox.Text.Length == 0 && !detbox.Focused) detbox.Text = "*";
            if (geobox.Text.Length == 0 && !geobox.Focused) geobox.Text = "*";
            this.BS2.DataSource = null;
        
            try
            {
                progress.PerformStep();
             //this.LinaaefficienciesTA.Fill(this.Linaa.efficiencies, detbox.Text, geobox.Text, "*", 0);
              //  Import_Click(sender, e);
          
               progress.PerformStep();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                progress.Value = 0;
            }

            Cursor.Current = Cursors.Default;


         }

        private void Reload2_Click(object sender, EventArgs e)
        {
            progress.Value = 0;
            Cursor.Current = Cursors.WaitCursor;

            double energy = 0;


            if (isobox.Text.Length == 0 && !isobox.Focused) isobox.Text = "*";
            if (energybox.Text.Length == 0 && !energybox.Focused)
            {
                energybox.Text = "*";
                energy = 0;
            }
            else if (!energybox.Text.Contains("*") && !energybox.Focused) energy = Convert.ToDouble(energybox.Text.Trim());


            try
            {
                progress.PerformStep();
            
               // this.LinaaefficienciesTA.Fill(this.efficiencies2, detbox.Text.Trim(), geobox.Text.Trim(), isobox.Text.Trim(), energy);
                this.BS2.DataSource = this.efficiencies2;
             
                progress.PerformStep();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                progress.Value = 0;
            }

            Cursor.Current = Cursors.Default;


        }


   

        private void Erase_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            progress.Value = 1;
            
            try
            {
                k0efficienciesTA.Delete(detbox.Text, geobox.Text, "*", 0);
                 progress.PerformStep();
                 
               
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                progress.Value = 0;
            }
            Reload_Click(sender, e);
          
            Cursor.Current = Cursors.Default;

           }

        private void Browse_Click(object sender, EventArgs e)
        {
            filebox.Text = null;
            progress.Value = 1;
        
            
            if (DialogResult.OK == fileopen.ShowDialog())
            {
                progress.PerformStep();
                Reload_Click(sender, e);   
            }
            else progress.Value = 0;
        }

        private void fileopen_FileOk(object sender, CancelEventArgs e)
        {
            filebox.Text = fileopen.FileName;
            try
            {
              FileInfo();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void k0efficienciesDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            isobox.Text = k0efficienciesDGV.CurrentRow.Cells[2].Value.ToString().Trim();
            energybox.Text = k0efficienciesDGV.CurrentRow.Cells[3].Value.ToString().Trim();
           // geobox.Text = ;
           // datetimebox.Text = k0efficienciesDGV.CurrentRow.Cells[14].Value.ToString().Trim();


        }



        public int FileInfo()
        {
            String x;
            int exit;
            string[] st = new string[5];

            if (System.IO.File.Exists(filebox.Text))
            {

                System.IO.FileStream fraw = new System.IO.FileStream(filebox.Text, System.IO.FileMode.Open);
                System.IO.StreamReader effiraw = new System.IO.StreamReader(fraw);


                while (!effiraw.EndOfStream)
                {

                    x = effiraw.ReadLine();


                    if (x.Contains("Geometry") == true)
                    {
                        st = x.Split(null);
                        if (st[4].Contains("14")) st[4] = "P";
                        else if (st[4].Contains("09")) st[4] = "C";
                        else if (st[4].Contains("04")) st[4] = "D";
                        else if (st[4].Contains("18")) st[4] = "H";
                        detbox.Text = st[4].Substring(0, 1);
                        geobox.Text = st[2];


                    }

                    else if (x.Contains("DATAFILES") == true)
                    {
                        st = x.Split(null);
                      //  datetimebox.Text = st[3] + " " + st[4];
                    }
                }
              
                exit = 0;
                ImportEffi.Enabled = true;
                EraseEffi.Enabled = true;

                fraw.Close();


            }

            else
            {
                ImportEffi.Enabled = false;
                EraseEffi.Enabled = false;
                exit = 6;
            }
          


            return exit;
        }




        public int Insert()
        {
            String x;
            String Iso;
            Double energy;
            int exit;

            Double[] effi = new Double[6];
            Double[] coi = new Double[6];
            string[] st = new string[11];
            int i = 0;
            int k = 0;

            string[] delimiter = new string[1];
            delimiter[0] = " ";

          //  DateTime date = new DateTime();

          //  date = Convert.ToDateTime(datetimebox.Text);
           
            detbox.Text = detbox.Text.Substring(0, 1);




            this.Linaa.efficiencies.Clear();


            if (System.IO.File.Exists(filebox.Text))
            {

                System.IO.FileStream fraw = new System.IO.FileStream(filebox.Text, System.IO.FileMode.Open);
                System.IO.StreamReader effiraw = new System.IO.StreamReader(fraw);

                while (!effiraw.EndOfStream)
                {
                    x = effiraw.ReadLine();

                    if (x.Contains("Isotope") == true)
                    {

                        Iso = x.Substring(10);

                        while (true)
                        {

                            x = effiraw.ReadLine();
                            if (x.CompareTo("_________________________________________________________________") == 0) k++;

                            if (k == 2)
                            {
                                k = 0;
                                break;
                            }

                        }

                        while (true)
                        {

                            x = effiraw.ReadLine();
                            if (x.CompareTo("_________________________________________________________________") == 0) break;
                            else
                            {

                                st = x.Split(delimiter, 11, StringSplitOptions.RemoveEmptyEntries);

                                energy = Convert.ToDouble(st[0]);

                                for (i = 0; i < 5; i++)
                                {

                                    effi[i + 1] = Convert.ToDouble(st[(i + 1) * 2]);
                                    coi[i + 1] = Convert.ToDouble(st[i * 2 + 1]);

                                }

                                          DateTime dates = DateTime.Today;

                                this.Linaa.efficiencies.AddefficienciesRow(detbox.Text, geobox.Text, Iso, energy, coi[1], effi[1], coi[2], effi[2], coi[3], effi[3], coi[4], effi[4], coi[5], effi[5], dates);



                             //   if (k0efficienciesTA.GetData(detbox.Text, geobox.Text, Iso, energy).Rows.Count == 0)
                              //  {

                               //     k0efficienciesTA.Insert(detbox.Text, geobox.Text, Iso, energy, coi[1], effi[1], coi[2], effi[2], coi[3], effi[3], coi[4], effi[4], coi[5], effi[5], date);
                              //  }




                               // else if (date.CompareTo(k0efficienciesTA.GetData(detbox.Text, geobox.Text, Iso, energy).Rows[0]["date"]) > 0)
                               // {

                                 //   k0efficienciesTA.Update(detbox.Text, geobox.Text, Iso, energy, coi[1], effi[1], coi[2], effi[2], coi[3], effi[3], coi[4], effi[4], coi[5], effi[5], date);
                              //  }


                            }
                        }
                    }
                }


                effiraw.Close();
                fraw.Close();

                ImportEffi.Visible = true;
                EraseEffi.Visible = true;

                exit = 0;
            }
            else
            {
                ImportEffi.Visible = false;
                EraseEffi.Visible = false;
                exit = 6;
            }
                return exit;
        }

      
      

              

    }
}
