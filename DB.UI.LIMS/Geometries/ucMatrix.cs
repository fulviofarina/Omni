using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DB.Tools;
using Rsx;
using Rsx.Dumb;
using VTools;

namespace DB.UI
{
    public partial class ucMatrix : UserControl
    {
        private string WCalcPath;

        private Graph grapher;

        private bool plotting = false;
        private string plottitle = "Plot Att vs. E";
        private string plottintext = "Plotting...";

        private string defaultFilter = "MatrixName asc";
        private string afterAddfilter = "MatrixID desc";

        public ucMatrix()
        {
            InitializeComponent();
        }

        private Interface Interface;

        public void Set(ref Interface inter)
        {
            this.SuspendLayout();

            this.Cancelbtn.Visible = false;
            LINAA inpu = inter.Get();
            Interface = inter;
            LIMS.SwapLinaa(ref inpu, ref this.Linaa);

            //create greapher
            grapher = new Graph();
            grapher.Name = "graph";
            this.GraphB.Text = plottitle;

            WCalcPath = this.Linaa.FolderPath + DB.Properties.Resources.WCalc;
            webBrowser.Url = new Uri(WCalcPath);

            Rsx.Dumb.BS.LinkBS(ref this.bs, this.Linaa.Matrix, string.Empty, defaultFilter);
            Rsx.Dumb.BS.LinkBS(ref this.compoBS, this.Linaa.Compositions, string.Empty, afterAddfilter);

            PostRefresh(null, EventArgs.Empty);

            // this.bindingNavigatorAddNewItem.Click -= addingItem;

            //add this extra execution

            GraphB.PerformClick();

            this.ResumeLayout(true);
        }

        public void PreRefresh(object sender, EventArgs e)
        {
            rtbox.DataBindings.Clear();
        }

        public void PostRefresh(object sender, EventArgs e)
        {
            Binding binding = new Binding("Text", this.bs, this.Linaa.Matrix.MatrixCompositionColumn.ColumnName);
            rtbox.DataBindings.Add(binding);
        }

        private void XCOM_Click(object sender, EventArgs e)
        {
            this.bs.EndEdit();
            this.compoBS.EndEdit();
            this.Validate();

            this.Cancelbtn.Visible = true;

            int position = 0;

            if (loop.Checked)
            {
                this.bs.MoveLast();
                position = this.bs.Position;
                this.bs.MoveFirst();
            }
            else position = this.bs.Position;

            bool XcomCalled = sender.Equals(XCOM);

            progress.Value = 0;
            progress.Minimum = 0;
            progress.Step = 1;

            while (this.Cancelbtn.Visible)
            {
                LINAA.MatrixRow m = Matrix;

                try
                {
                    //finds the MUEs for each 1keV, given start and Totalend energies, by NrEnergies (keV) steps.
                    double start = Convert.ToDouble(energyStart.Text);
                    double Totalend = Convert.ToDouble(energyEnd.Text);
                    int NrEnergies = Convert.ToInt16(energyStep.Text);  // STEP (in keV) for retrieving MUES for each keV
                    double end = start + NrEnergies;

                    if (ASCIIOutput.Checked)
                    {
                        // m.SetColumnError(this.Linaa.Matrix.MatrixDensityColumn, null); //nullifies
                        // the error in Density column because density is not necessary
                        progress.Maximum += 2;
                        string startupPath = System.Windows.Forms.Application.StartupPath;

                        if (!ASCIIInput.Checked)
                        {
                            Tools.XCOM.FindTim(m.MatrixComposition, NrEnergies, start, Totalend, startupPath + "\\" + m.MatrixName.Trim() + ".txt", false);
                        }
                        else
                        {
                            string EnergiesList = System.IO.File.ReadAllText(this.Linaa.FolderPath + DB.Properties.Resources.XCOMEnergies);
                            Tools.XCOM.FindTim(DB.Tools.XCOM.MakeCompositionsList(m.MatrixComposition), EnergiesList, startupPath + "\\" + m.MatrixName.Trim() + ".txt", false);
                        }

                        progress.PerformStep();
                        IO.Process(new System.Diagnostics.Process(), startupPath + "\\", "notepad.exe", m.MatrixName.Trim() + ".txt", false, false, 1000);
                        progress.PerformStep();
                    }
                    else
                    {
                        m.SetColumnError(this.Linaa.Matrix.XCOMColumn, null);   //nullifies the error in XCOM column

                        bool goIn = ((m.XCOM || !XcomCalled) || force.Checked);
                        goIn = !m.HasErrors() && goIn;
                        if (goIn)
                        {
                            string responde = string.Empty;
                            if (XcomCalled)
                            {
                                progress.Step = NrEnergies;
                                progress.Maximum = Convert.ToInt16(Math.Ceiling(((Totalend - start) / NrEnergies))) * NrEnergies;
                                responde = GetMuesFromXCOM(ref start, Totalend, NrEnergies, ref end, ref m);
                            }

                            if (string.IsNullOrEmpty(responde))
                            {
                                responde = DB.Tools.XCOM.QueryXCOM(m.MatrixComposition, 17, 80, 3000);
                            }

                            if (string.IsNullOrEmpty(responde))
                            {
                                Interface.IReport.Msg("Problems comunicating with XCOM", "XCOM did not answer the query");
                                return;
                            }

                            progress.Maximum += 3;

                            IEnumerable<LINAA.CompositionsRow> ros = m.GetCompositionsRows();
                            Linaa.Delete(ref ros);

                            progress.PerformStep();
                            Application.DoEvents();

                            LINAA.ElementsDataTable ele = this.Linaa.Elements;

                            // Dumb.AcceptChanges(ref ros);
                            IList<string[]> ls = DB.Tools.XCOM.ExtractComposition(responde, ref ele);

                            progress.PerformStep();
                            Application.DoEvents();

                            // IList<LINAA.CompositionsRow> add = null; .MatrixID, ref ls, ref add
                            // m.CodeOrAddComposition(ref ls);
                            m.AddOrUpdateComposition(ls);

                            progress.PerformStep();
                            Application.DoEvents();
                        }
                    }

                    if (plotting) graph_Click(sender, e);

                    if (this.bs.Position == position) break;
                    else this.bs.MoveNext();
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(m, this.Linaa.Matrix.XCOMColumn, ex);
                    this.Linaa.AddException(ex);
                }
            }

            Application.DoEvents();
            if (XcomCalled) this.Linaa.PopulateMUESList();
            else
            {
                this.Linaa.Save<LINAA.CompositionsDataTable>();
                webBrowser.Url = new Uri(WCalcPath);
                webBrowser.Show();
            }

            if (Cancelbtn.Visible) progress.Value = 0;
            this.Cancelbtn.Visible = false;
        }

        private string GetMuesFromXCOM(ref double start, double Totalend, int NrEnergies, ref double end, ref DB.LINAA.MatrixRow m)
        {
            string responseToKeep = string.Empty;

            try
            {
                this.Linaa.TAM.MUESTableAdapter.DeleteByMatrixID(m.MatrixID);
                string path = this.Linaa.FolderPath + "\\" + m.MatrixID + ".";
                string ext = ".htm";

                int i = 0;
                String Response = string.Empty;
                while (true)
                {
                    Response = DB.Tools.XCOM.QueryXCOM(m.MatrixComposition, NrEnergies, start, end);
                    if (i == 0) responseToKeep = Response;

                    System.IO.File.WriteAllText(path + i + ext, Response);

                    webBrowser.Url = null;
                    Uri uri = new Uri(path + i + ext);

                    System.IO.StreamReader reader = new System.IO.StreamReader(path + i + ext);
                    DB.LINAA.MUESDataTable mu = DB.Tools.XCOM.FindMu(m.MatrixDensity, ref reader);
                    webBrowser.Url = uri;
                    webBrowser.Show();

                    Application.DoEvents();
                    foreach (DB.LINAA.MUESRow row in mu.Rows)
                    {
                        this.Linaa.TAM.MUESTableAdapter.Insert(m.MatrixID, row.Energy, row.MACS, row.MAIS, row.PE, row.PPNF, row.PPEF, row.MATCS, row.MATNCS, row.Density, row.Edge);
                    }
                    Dumb.FD(ref mu);
                    progress.PerformStep();
                    Application.DoEvents();

                    if (end >= Totalend) break;
                    start += NrEnergies;
                    end = start + NrEnergies;
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            return responseToKeep;
        }

        private void energyStep_TextChanged(object sender, EventArgs e)
        {
            if (!energyStep.Text.Equals(String.Empty))
            {
                if (Convert.ToInt16(energyStep.Text) > 75) energyStep.Text = "75";
            }
        }

        private void ASCIIInput_Click(object sender, EventArgs e)
        {
            IO.Process(new System.Diagnostics.Process(), System.Windows.Forms.Application.StartupPath, "notepad.exe", this.Linaa.FolderPath + DB.Properties.Resources.XCOMEnergies, false, true, 100000);
        }

        private void graph_Click(object sender, EventArgs e)
        {
            try
            {
                if (GraphB.Text.Equals(plottitle))
                {
                    GraphB.Text = (plottintext);
                    plotting = true;
                    PrepareTLP(webBrowser, grapher);
                    BS_CurrentChanged(sender, e);
                }
                else
                {
                    GraphB.Text = (plottitle);
                    PrepareTLP(grapher, webBrowser);
                    plotting = false;
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        /*
         private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
         {
             this.matrixSave.PerformClick();
             this.BS.Sort = this.Linaa.Matrix.MatrixIDColumn.ColumnName + " desc ";
         }
          */

        // GetCompositionFromXCOM(this.Matrix);

        /*

       Application.DoEvents();
       LINAA.MatrixRow m = this.Matrix;

       //make formula long composition calculation

          System.Collections.Generic.IList<string[]> ls = LINAA.StripComposition(ref m);
          if (ls == null) return;

           if (m.MatrixComposition.Contains(","))
           {
           }

          foreach (string[] formCompo in ls)
          {
              HtmlElementCollection coll = this.webBrowser.Document.Body.Document.GetElementsByTagName("input");
              coll[1].SetAttribute("value", m.MatrixComposition);
              HtmlElement ef = coll[0];
              ef.RaiseEvent("onClick");

              Func<string, bool> test = x =>
              {
                  return  !string.IsNullOrWhiteSpace(x);
              };

              string aux = string.Empty;
              double weight = 0;
              string composition = string.Empty;

              aux = coll[2].GetAttribute("value");
              if (test(aux)) composition = aux;
              aux = coll[3].GetAttribute("value");
              if (test(aux)) weight = Convert.ToDouble(aux);

              for (int i = 4; i < coll.Count - 2; i += 2)
              {
                  aux = coll[i].GetAttribute("value");

                  if (!test(aux)) continue;

                  LINAA.CompositionRow c = this.Linaa.Composition.AddCompositionRow(m.MatrixID, aux, 0, weight);
                  aux = coll[i + 1].GetAttribute("value");
                  if (test(aux)) c.Quantity = Convert.ToDouble(aux);
                  else c.Quantity = 0;
              }
          }
          else
          {
              //no commas, so make table directly....

              this.Linaa.Composition.AddCompositionRow(m.MatrixID, formCompo[0], 0, Convert.ToDouble(formCompo[1]));
          }
      }

     */

        private void PrepareTLP(Control toRemove, Control toAdd)
        {
            if (TLP.ColumnCount == 1)
            {
                TLP.ColumnCount = 2;
            }

            if (TLP.Controls.Contains(toRemove))
            {
                this.TLP.Controls.Remove(toRemove);
            }
            if (!TLP.Controls.Contains(toAdd))
            {
                toAdd.Dock = DockStyle.Fill;
                this.TLP.Controls.Add(toAdd, 1, 1);
            }
        }

        public LINAA.MatrixRow Matrix
        {
            get
            {
                LINAA.MatrixRow matrix = null;
                DataRowView rv = this.bs.Current as DataRowView;
                if (rv != null) matrix = rv.Row as LINAA.MatrixRow;
                return matrix;
            }
        }

        // private ToolStripLabel label = new ToolStripLabel();

        /*
         private void CMS_Opening(object sender, System.ComponentModel.CancelEventArgs e)
         {
             this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
             this.WCalc,
             this.toolStripSeparator3,
             this.GraphB,
             this.toolStripSeparator1,
             this.XCOM,
             this.XCOMOptions});

             this.MatrixTS.Items.Add(label);
             label.Text = "Please select what to do....";
         }

         private void CMS_Closed(object sender, ToolStripDropDownClosedEventArgs e)
         {
             label.Text = string.Empty;
             this.MatrixTS.Items.Remove(label);

             this.MatrixTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
             this.WCalc,
             this.XCOMOptions,
             this.XCOM,
             this.toolStripSeparator1,
             this.GraphB,
             this.toolStripSeparator3});
         }
        */

        private void ucLinaaMatrix_Load(object sender, EventArgs e)
        {
            if (this.ParentForm.GetType().Equals(typeof(PickerForm)))
            {
                if (TLP.ColumnCount == 2)
                {
                    if (TLP.Controls.Contains(webBrowser)) TLP.Controls.Remove(webBrowser);
                    else if (TLP.Controls.Contains(grapher)) TLP.Controls.Remove(grapher);
                    TLP.ColumnCount = 1;
                }
            }
        }

        private void BS_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                this.compoBS.Filter = "MatrixID = '" + Matrix.MatrixID + "'";

                if (!plotting) return;
                double eh = Convert.ToDouble(this.energyEnd.Text);
                double el = Convert.ToDouble(this.energyStart.Text);
                DB.LINAA.MUESDataTable mues = this.Linaa.TAM.MUESTableAdapter.GetDataByMatrixIDAndEnergy(el, eh, Matrix.MatrixID);
                DataColumn ene = mues.EnergyColumn;
                DataColumn mu = mues.MUColumn;

                this.grapher.SetGraph(ref ene, 1, useLogScale.Checked, 10, ref mu, 1, true, 10, "Att coefficients for " + Matrix.MatrixName + " - Density: " + Matrix.MatrixDensity);

                Dumb.FD(ref mues);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            this.Cancelbtn.Visible = false;
        }
    }
}