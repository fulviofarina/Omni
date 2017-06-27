using System;
using System.Collections.Generic;
using System.Data;
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
        EventHandler showProg = null;
        Action<int> resetProgress = null;
        public void Set(ref IOptions options)
        {
             resetProgress = options.ResetProgress;
           showProg = options.ShowProgress;

            string path = Interface.IStore.FolderPath + Resources.XCOMFolder;
            XCom = new XCOM();
            XCom.Set(path, callmeBack, resetProgress, showProg);
            XCom.Set(ref Interface);
            ucCalculate1.CancelMethod += delegate
            {
                XCom.IsCalculating = false;
            };
            ucCalculate1.CalculateMethod += delegate
            {
                this.Validate();
                ucMUES1.Focus(false);
                XCom.Calculate(null);
                ucMUES1.Focus(true);

                UpdateMatrixMUESUserInterface();
             
            };

            DBTLP.Controls.Add(options as UserControl);
        }

        public void Set(ref UserControl ctrl)
        {
            ctrl.Dock = DockStyle.Fill;
            DBTLP.Controls.Add(ctrl as UserControl);
        }
        public void Set(ref Interface inter, ref IXCOMPreferences preferences)
        {
            this.SuspendLayout();

            Interface = inter;

            ucMatrixSimple1.Set(ref inter);

            Interface.IBS.Matrix.CurrentChanged += delegate
            {
                UpdateMatrixMUESUserInterface();
            };

            grapher.Name = "graph";

            this.ResumeLayout(true);

            ucMUES1.Set(ref inter, ref preferences);



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

      

        public void UpdateMatrixMUESUserInterface()
        {
            try
            {
                resetProgress(0);
                resetProgress(2);
                showProg(null, EventArgs.Empty);
                preparePlot();
                showProg(null, EventArgs.Empty);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void preparePlot()
        {
         
            object[] arr = XCom.GetDataToPlot();
            DataColumn ene = (DataColumn) arr[0];
            DataColumn mu = (DataColumn) arr[1];
          


            this.grapher.SetGraph(ref ene, 1, (bool)arr[2], 10, ref mu, 1, true, 10,(string)arr[3]);

            DataTable dt = ene.Table;

        

            Dumb.FD(ref dt);
        }

        private void callmeBack(object sender, EventArgs e)
        {
            object[] arr = (sender as object[]);
            string tempFile = arr[0] as string;
            string Response = arr[1] as string;
            ucMUES1.SendToBrowser(tempFile, Response);
        }
        private void ucLinaaMatrix_Load(object sender, EventArgs e)
        {
            if (minimiZe)
            {
                splitContainer1.Panel2Collapsed = true;
            }
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