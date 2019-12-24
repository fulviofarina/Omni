using DB.Properties;
using DB.Tools;
using Rsx.Dumb;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VTools;
using static DB.LINAA;

namespace DB.UI
{
    public partial class ucMatrix : UserControl
    {
        protected Interface Interface;
        protected XCOM XCom;


        protected internal string path = string.Empty;
        protected internal Action<int> resetProgress = null;
        protected internal EventHandler showProg = null;

        public void Set(IOptions options)
        {
            if (options == null) return;

            resetProgress = options.ResetProgress;
            showProg = options.ShowProgress;
            DBTLP.Controls.Add(options as UserControl);

            options.HelpClick += delegate
            {
                Uri helpFile = null;
                helpFile = new Uri("https://sites.google.com/view/ufinder/home");

                IO.ProcessWebsite(helpFile);

            };

            saveHandler += delegate
             {
                 options.ClickSave();
             };
        }

        protected internal EventHandler saveHandler;

        public void Set(ref UserControl ctrl)
        {
            ctrl.Dock = DockStyle.Fill;
            DBTLP.Controls.Add(ctrl as UserControl);

            MatrixRow mlast = Interface.IDB.Matrix.OrderBy(o => o.MatrixDate).FirstOrDefault();
            if (!EC.IsNuDelDetch(mlast))
            {
                Interface.IBS.CurrentChanged(mlast, true, true, false);
            }
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

            ucPicNav1.Set(path, "*", XCOM.PictureExtension, "FULL");

            EventHandler changed = delegate
            {
                MatrixRow m = Interface.ICurrent.Matrix as MatrixRow;
                refreshList(m, EventArgs.Empty);
            };

            EventHandler enableCtrols = delegate
            {
                Interface.IBS.EnabledControls = !XCom.IsCalculating;
                ucCalculate1.EnableCalculate = !XCom.IsCalculating;

                Application.DoEvents();
            };

            EventHandler cancelMethod = delegate
           {
               XCom.IsCalculating = false;
           };

            EventHandler calculateMethod = delegate
            {
                ucCalculate1.EnableCalculate = false;
                Application.DoEvents();
                this.Validate(true);
                Interface.IBS.IsCalculating = true;
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
            ucCalculate1.CancelMethod += changed;

            ucCalculate1.CalculateMethod += calculateMethod;
            ucCalculate1.CalculateMethod += changed;
            ucCalculate1.CalculateMethod += enableCtrols;

            Interface.IDB.Matrix.CleanMUESHandler += changed;

            Interface.IBS.Matrix.CurrentChanged += changed;

            this.Disposed += delegate
            {
                Interface.IDB.Matrix.CleanMUESHandler -= changed;
                Interface.IBS.Matrix.CurrentChanged -= changed;
            };

            // test();
        }

        protected internal void refreshList(object sender, EventArgs empty)
        {
            MatrixRow m = (MatrixRow)sender;
            if (m != null)
            {
                ucPicNav1.RefreshList(m.MatrixID.ToString(), ".*", "N");
            }
        }


        /// <summary>
        /// Method to execute after a sample is calculated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        protected internal void callmeBack(object sender, EventArgs e)
        {
            MatrixRow m = sender as MatrixRow;

            if (m == null) return;

            if (!XCom.IsCalculating)
            {
                XCom.CheckCompletedOrCancelled();
                Interface.IDB.CheckMatrixToDoes();
                saveHandler.Invoke(sender, e);
            }

            Interface.IBS.EnabledControls = !XCom.IsCalculating;

            Interface.IBS.CurrentChanged<MatrixRow>(m, true, true);
            ucMUES1.MakeFile(m.MatrixID.ToString() + "." + m.MatrixName, path);

            refreshList(m, EventArgs.Empty);

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

        protected internal bool minimize = false;

        protected internal void load(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = minimize;

            if (minimize)
            {
                this.ParentForm.WindowState = FormWindowState.Normal;
            }
        }
    }
}