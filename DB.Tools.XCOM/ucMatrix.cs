using DB.Properties;
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
        protected  Interface Interface;
        protected  XCOM XCom;
        Uri helpFile = new Uri("https://www.researchgate.net/publication/317902783_-Finder_A_Windows_program_for_photon_mass_attenuation_coefficients_between_1_keV_to_100_GeV");

        /*
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
         */


        private string path = string.Empty;
        private Action<int> resetProgress = null;
        private EventHandler showProg = null;
        public void Set( IOptions options)
        {

            if (options == null) return;
           
                resetProgress = options.ResetProgress;
                showProg = options.ShowProgress;
                DBTLP.Controls.Add(options as UserControl);
         
            EventHandler help = delegate
            {
                  ucPicNav1.NavigateTo(helpFile);
                //   helpFile = getHelpFileµFinder();
                // System.Diagnostics.Process.Start(WINDOWS_EXPLORER, helpFile);
            };

            options.HelpClick += help;

            saveHandler += delegate
             {

                 options.ClickSave();
             };
           
        }
        private  EventHandler saveHandler;
        public void Set(ref UserControl ctrl)
        {

            ctrl.Dock = DockStyle.Fill;
            DBTLP.Controls.Add(ctrl as UserControl);

                    

            pickLast();
        }

        /// <summary>
        /// debería estar en otro lado
        /// </summary>
        private void pickLast()
        {
            MatrixRow mlast = Interface.IDB.Matrix.OrderBy(o => o.MatrixDate).FirstOrDefault();
            Interface.IBS.Matrix.Position = 1;
            Interface.IBS.Matrix.Position = Interface.IBS.Matrix.Find(Interface.IDB.Matrix.MatrixDateColumn.ColumnName, mlast.MatrixDate);
        }

        /// <summary>
        /// Sets the interface
        /// </summary>
        /// <param name="inter"></param>
        public void Set(ref Interface inter)
        {
            Interface = inter;
            ucMatrixSimple1.Set(ref Interface);
            ucMUES1.Set(ref Interface);
          
        }

        /// <summary>
        /// Sets the preferences
        /// </summary>
        /// <param name="preferences"></param>
        public void Set(ref IXCOMPreferences preferences)
        {
            ucMUES1.Set(ref preferences);
        }

        /// <summary>
        /// Sets XCOM
        /// </summary>
        public void SetXCOM()
        {
            path = Interface.IStore.FolderPath + Resources.XCOMFolder;

            XCom = new XCOM();
            XCom.Set(path, callmeBack, resetProgress, showProg);
            XCom.Set(ref Interface);
            XCom.Reporter = Interface.IReport.Msg;
            XCom.ExceptionAdder = Interface.IStore.AddException;

            EventHandler navigatorRefresh = delegate
            {
                ucPicNav1.Set(path, "*", XCOM.PictureExtension, "FULL");
                ucPicNav1.HideList(true);
                ucPicNav1.HideList(false);
            };

            EventHandler enableCtrols = delegate
            {
                Interface.IBS.EnabledControls = !XCom.IsCalculating;
                ucCalculate1.EnableCalculate = !XCom.IsCalculating;
            //    ucMatrixSimple1.RefreshDGV();
                Application.DoEvents();
            };

            EventHandler cancelMethod  = delegate
            {
                XCom.IsCalculating = false;
            };


            //   ucCalculate1.CalculateMethod += enableCtrols;

            EventHandler calculateMethod  = delegate
             {
                 ucCalculate1.EnableCalculate = false;
                 Application.DoEvents();
                 this.Validate(true);
                 Interface.IBS.IsCalculating =true;
                 Application.DoEvents();
                 //salva primero, ok
                 saveHandler?.Invoke(null, EventArgs.Empty);
           
                 Application.DoEvents();
                 XCom.Preferences = Interface.IPreferences.CurrentXCOMPref;
                 XCom.Offline = Interface.IPreferences.CurrentPref.Offline;
                 XCom.Rows = Interface.IDB.Matrix.Where(o => o.ToDo).ToList();
                 Application.DoEvents();
                 XCom.Calculate(null);

             };



            ucCalculate1.CancelMethod += cancelMethod;
            ucCalculate1.CancelMethod += enableCtrols;
            ucCalculate1.CancelMethod += navigatorRefresh;


            ucCalculate1.CalculateMethod += calculateMethod;
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

            /// <summary>
            /// Method to execute after a sample is calculated
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void callmeBack(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;

            if (m == null) return;

            if (!XCom.IsCalculating)
            {
                XCom.CheckCompletedOrCancelled();
                saveHandler.Invoke(sender,e);
            }

            Interface.IBS.CurrentChanged<MatrixRow>(m, true, true);

            ucMUES1.MakeFile(m.MatrixID.ToString(), path);

            ucPicNav1.RefreshList(m.MatrixID.ToString(), ".*", "N");

            Interface.IBS.EnabledControls = !XCom.IsCalculating;

            if (XCom.IsCalculating) return;

            ucCalculate1.EnableCalculate = !XCom.IsCalculating;
    
        }


     

        /// <summary>
        /// Initializer, minimizes the second panel if necessary
        /// </summary>
        /// <param name="minimize"></param>
        public ucMatrix(bool minimized = false)
        {
            InitializeComponent();

            minimize = minimized;
            this.Load += load;
      
        }
        private bool minimize = false;
        private void load(object sender, EventArgs e)
        {
          
                splitContainer1.Panel2Collapsed = minimize;

                if (minimize)
                {
                    this.ParentForm.WindowState = FormWindowState.Normal;
                }
           
        }
    }
}