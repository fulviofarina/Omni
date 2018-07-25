﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using Rsx.Dumb;
using VTools;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucMatrix : UserControl
    {

        protected internal Interface Interface;
        protected internal bool minimiZe = false;

        protected internal XCOM XCom;



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
      private  EventHandler showProg = null;
       private  Action<int> resetProgress = null;
        public void Set(ref IOptions options)
        {
            resetProgress = options.ResetProgress;
          //  resetProgress(0);
            showProg = options.ShowProgress;

          

            string path = Interface.IStore.FolderPath + Resources.XCOMFolder;

            XCom = new XCOM();
            XCom.Set(path, callmeBack, resetProgress, showProg);
            XCom.Set(ref Interface);

            ucCalculate1.CancelMethod += delegate
            {
                XCom.IsCalculating = false;
                ucCalculate1.EnableCalculate = true;
            
            };
            ucCalculate1.CalculateMethod += delegate
            {

                Application.DoEvents();

                this.Validate();

                ucCalculate1.EnableCalculate = false;

                //    ucMatrixSimple1.ChangeCompositionView();
                //  ucMUES1.Focus(false);

                Application.DoEvents();

                XCom.Calculate(null);


                ucCalculate1.EnableCalculate = !XCom.IsCalculating;


                Application.DoEvents();


                this.ucMatrixSimple1.RefreshDGV();

             //   ucMUES1.Focus(true);

             //   Application.DoEvents();

                //    ucMatrixSimple1.ChangeCompositionView();
                //  preparePlot();

            };

            DBTLP.Controls.Add(options as UserControl);
        
        }

     

        public void Set(ref UserControl ctrl)
        {
            this.SuspendLayout();
            ctrl.Dock = DockStyle.Fill;
            DBTLP.Controls.Add(ctrl as UserControl);


           


            this.ResumeLayout(true);
        }
        public void Set(ref Interface inter)
        {
            this.SuspendLayout();

            Interface = inter;

        
          

         //   ucMUES1.Set(ref inter, ref preferences);

            this.ResumeLayout(true);

        }
        public void Set(ref IXCOMPreferences preferences)
        {
            this.SuspendLayout();

            ucMatrixSimple1.Set(ref Interface);

            ucMUES1.Set(ref Interface, ref preferences);

            Interface.IBS.Matrix.CurrentChanged += delegate
            {
                MatrixRow m = (Interface.ICurrent.Matrix as MatrixRow);
                //     ucMUES1.PrintDGV(XCom.StartupPath + m.MatrixID + "2.xml");
                if (m!=null) showInBrowser(m.MatrixID);
                //    callmeBack(m, EventArgs.Empty);

            };

            this.ResumeLayout(true);

        }

        protected internal void addCompositions(ref MatrixRow m, string responde)
        {
            // progress.Maximum += 3;

            IEnumerable<CompositionsRow> ros = m.GetCompositionsRows();
            Interface.IStore.Delete(ref ros);

            // progress.PerformStep();
            Application.DoEvents();

            ElementsDataTable ele = Interface.IDB.Elements;

            // Dumb.AcceptChanges(ref ros);
            IList<string[]> ls = XCOM.ExtractComposition(responde, ref ele);

            // progress.PerformStep();
            Application.DoEvents();

            // IList<LINAA.CompositionsRow> add = null; .MatrixID, ref ls, ref add
            // m.CodeOrAddComposition(ref ls);
            Interface.IPopulate.IGeometry.AddCompositions(ref m, ls);

            // progress.PerformStep();
            Application.DoEvents();
        }

      

   

        private void callmeBack(object sender, EventArgs e)
        {

            MatrixRow m = sender as MatrixRow;

            if (m == null) return;

            Interface.IBS.CurrentChanged<MatrixRow>(m, true, true);
            //     ucMUES1.PrintDGV(XCom.StartupPath + m.MatrixID + "2.xml");
            showInBrowser(m.MatrixID);
            //

         //   if (!XCom.IsCalculating)
           // {
                ucCalculate1.EnableCalculate = !XCom.IsCalculating;

            //  }

            ucMatrixSimple1.RefreshDGV();

         //   XCom.CheckIfFinished();
        }

        private void showInBrowser(int matrixID)
        {
            Uri uri = new Uri(XCom.StartupPath + matrixID + ".png");
            webBrowser1.Navigate(uri);
        }

        private void ucLinaaMatrix_Load(object sender, EventArgs e)
        {
            if (minimiZe)
            {
                splitContainer1.Panel2Collapsed = true;
            }

            this.ucMatrixSimple1.RefreshDGV();

        }

        public ucMatrix()
        {
            InitializeComponent();

            this.Load += this.ucLinaaMatrix_Load;
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
    }
}