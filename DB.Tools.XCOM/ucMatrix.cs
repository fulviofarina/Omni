﻿using DB.Properties;
using DB.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VTools;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucMatrix : UserControl
    {
        protected internal Interface Interface;
        protected internal bool minimiZe = false;

        protected internal XCOM XCom;


        private void test()
        {
            foreach (var item in Interface.IDB.Matrix)

            {
                item.SetCompositionTableNull();
            //    item.MatrixComposition= item.MatrixComposition.Replace("(", " ");
             //   item.MatrixComposition= item.MatrixComposition.Replace(")", " ");
                //      IEnumerable<CompositionsRow> compos = item.GetCompositionsRows();
                //  foreach (var item2 in compos)
                //   {
                //    item2.Element= item2.Element.Replace("(", " ");
                //   item2.Element = item2.Element.Replace(")", " ");
                //  }


            }
           
        }
       // private IOptions op = null;
        private string path = string.Empty;
        private Action<int> resetProgress = null;
        private EventHandler showProg = null;
        public void Set(ref IOptions options)
        {
            // this.SuspendLayout();
       //     op = options;
            resetProgress = options.ResetProgress;
            showProg = options.ShowProgress;
            DBTLP.Controls.Add(options as UserControl);



            options.HelpClick += delegate
            {
                Uri helpFile;
                helpFile = new Uri("https://www.researchgate.net/publication/317902783_-Finder_A_Windows_program_for_photon_mass_attenuation_coefficients_between_1_keV_to_100_GeV");
                ucPicNav1.NavigateTo(helpFile);
             //   helpFile = getHelpFileµFinder();
               // System.Diagnostics.Process.Start(WINDOWS_EXPLORER, helpFile);
            };


            // this.ResumeLayout(true);
        }

        public void Set(ref UserControl ctrl)
        {
            // this.SuspendLayout();
            ctrl.Dock = DockStyle.Fill;
            DBTLP.Controls.Add(ctrl as UserControl);



            MatrixRow mlast =     Interface.IDB.Matrix.OrderBy(o => o.MatrixDate).FirstOrDefault();
            Interface.IBS.Matrix.Position = 1;

            Interface.IBS.Matrix.Position = Interface.IBS.Matrix.Find(Interface.IDB.Matrix.MatrixDateColumn.ColumnName, mlast.MatrixDate);
        


            // this.ResumeLayout(true);
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            ucMatrixSimple1.Set(ref Interface);
            ucMUES1.Set(ref Interface);


          
        }

        public void Set(ref IXCOMPreferences preferences)
        {
            // this.SuspendLayout();

            ucMUES1.Set(ref preferences);

          

            // this.ResumeLayout(true);
        }

        public void SetXCOM()
        {
            path = Interface.IStore.FolderPath + Resources.XCOMFolder;

            XCom = new XCOM();
         
            XCom.Set(path, callmeBack, resetProgress, showProg);
            XCom.Set(ref Interface);
            XCom.Reporter = Interface.IReport.Msg;
            XCom.ExceptionAdder = Interface.IStore.AddException;

         


            // XCOM.PictureExtension

            EventHandler navigatorRefresh = delegate
            {
                ucPicNav1.Set(path, "*", XCOM.PictureExtension);

                ucPicNav1.HideList(true);
                // ucPicNav1.HideList(true);
                ucPicNav1.HideList(false);
            };

            EventHandler enableCtrols = delegate
            {
                Interface.IBS.EnabledControls = !XCom.IsCalculating;
         
                ucCalculate1.EnableCalculate = !XCom.IsCalculating;
              
            //    ucMatrixSimple1.RefreshDGV();
                Application.DoEvents();

            };

            ucCalculate1.CancelMethod += delegate
            {
                XCom.IsCalculating = false;
            };
            ucCalculate1.CancelMethod += enableCtrols;
            ucCalculate1.CancelMethod += navigatorRefresh;


         //   ucCalculate1.CalculateMethod += enableCtrols;

            ucCalculate1.CalculateMethod += delegate
             {


            //     ucPicNav1.Set(path, "*", XCOM.PictureExtension);


                 ucCalculate1.EnableCalculate = false;
                 Application.DoEvents();
                 this.Validate(true);
                 Interface.IBS.IsCalculating =true;
                 Application.DoEvents();
                 //salva primero, ok
                 saveMethod();
                 Application.DoEvents();
                 XCom.Preferences = Interface.IPreferences.CurrentXCOMPref;
                 XCom.Offline = Interface.IPreferences.CurrentPref.Offline;
                 XCom.Rows = Interface.IDB.Matrix.Where(o => o.ToDo).ToList();
                 Application.DoEvents();
                 XCom.Calculate(null);


                

             };
            ucCalculate1.CalculateMethod += navigatorRefresh;
            ucCalculate1.CalculateMethod += enableCtrols;

            Interface.IDB.Matrix.CleanMUESHandler += navigatorRefresh;
         


            EventHandler changed = delegate
            {
                MatrixRow m = Interface.ICurrent.Matrix as MatrixRow;
                if (m != null)
                {

                    ucPicNav1.RefreshList(m.MatrixID.ToString(), ".*", "N");
                }
            };

            Interface.IBS.Matrix.CurrentChanged += navigatorRefresh;
            Interface.IBS.Matrix.CurrentChanged += changed;

            //  callmeBack += changed;

            this.Disposed += delegate
            {
                Interface.IDB.Matrix.CleanMUESHandler -= navigatorRefresh;
                Interface.IBS.Matrix.CurrentChanged -= navigatorRefresh;
                Interface.IBS.Matrix.CurrentChanged -= changed;
            };



            //  test();
        }
        /*
        protected internal void addCompositions(ref MatrixRow m, string responde)
        {
            IEnumerable<CompositionsRow> ros = m.GetCompositionsRows();
            Interface.IStore.Delete(ref ros);

            Application.DoEvents();

            ElementsDataTable ele = Interface.IDB.Elements;

            IList<string[]> ls = XCOM.ExtractComposition(responde, ref ele);

            Application.DoEvents();
            Interface.IPopulate.IGeometry.AddCompositions(ref m, ls);

            Application.DoEvents();
        }

      */

        private void callmeBack(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;

            if (m == null) return;

          
            if (!XCom.IsCalculating)
            {
                XCom.CheckCompletedOrCancelled();
          
                saveMethod();
            }

            Interface.IBS.CurrentChanged<MatrixRow>(m, true, true);

            ucMUES1.MakeFile(m.MatrixID.ToString(), path);

            ucPicNav1.RefreshList(m.MatrixID.ToString(), ".*", "N");

            //   ucMatrixSimple1.RefreshDGV();
            Interface.IBS.EnabledControls = !XCom.IsCalculating;

            if (XCom.IsCalculating) return;

          //  Interface.IBS.CurrentChanged<MatrixRow>(m, true, true);
          //  ucMUES1.MakeFile(m.MatrixID.ToString(), path);
            // ucPicNav1.RefreshList(m.MatrixID.ToString(), ".*");

            ucCalculate1.EnableCalculate = !XCom.IsCalculating;

          //  ucMatrixSimple1.RefreshDGV();
        }

        private void saveMethod()
        {
            if (Interface.IPreferences.CurrentPref.Offline)
            {
                Interface.IStore.SaveLocalCopy();
            }
            else
            {
                IEnumerable<DataTable> tables = new DataTable[] { Interface.IDB.Matrix, Interface.IDB.Compositions };
                Interface.IStore.SaveRemote(ref tables);
            }
        }

        public ucMatrix()
        {
            InitializeComponent();

            this.Load += delegate
          {
              if (minimiZe)
              {
                  splitContainer1.Panel2Collapsed = true;
              }
          };
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

        /*
     public bool Minimize
     {
         get
         {
             return minimiZe;
         }

         set
         {
             minimiZe = value;
         }
     }
     */
    }
}