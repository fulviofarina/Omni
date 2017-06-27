using System;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;

namespace µFinder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        private static void Main()
        {

            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form aboutbox = new AboutBox();

                LIMS.Interface = Creator.Build();
                //create database
                LIMS.CreateLIMS(ref aboutbox);
                Creator.CheckDirectories();
                Creator.PopulateBasic();
                LIMS.Interface.IPreferences.CurrentPref.Offline = true;
           //     LIMS.Interface.IPreferences.CurrentPref.AdvancedEditor = true;

                //  bool ok = Creator.CheckConnections(true, true);
                // if (ok) Creator.LoadMethods(0);
                //  else throw new Exception("Could not start loading the database");
                UserControl control = LIMS.CreateUI(ControlNames.Matrices);
              //  CreateMatrixApplication(out control, out refresher);

                LIMS.CreateForm("µ-Finder", ref control, false);
                Form toShow = control.ParentForm;

          //      LIMS.Interface.IPreferences.CurrentPref.AdvancedEditor = true;
                LIMS.Interface.IPreferences.CurrentPref.Offline = true;

              
                //     Creator.PopulateBasic();
                //      LIMS.Interface.IPreferences.CurrentPref.Offline = true;

                //    LIMS.Interface.IPopulate.IGeometry.PopulateMatrix();
                //    LIMS.CreateForm("µ-Finder", ref control, false);
                //   ucSubSamples ucSubSamples = control as ucSubSamples;
                //EventHandler firstCallBack;
                //   Form toShow = LIMS.CreateSSFApplication();

                //   Creator.Run();

                PainterTimer();

                Application.Run(toShow);

            }
            catch (Exception ex)
            {
           //     LIMS.Interface.IStore.AddException(ex);
               // LIMS.Interface.IStore.SaveExceptions();
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }

        }

        public static void PainterTimer()
        {

            //to repaint the form
            System.Timers.Timer painter = new System.Timers.Timer();
            painter.Elapsed += delegate
            {
                painter.Enabled = false;

                Application.OpenForms[0]?.Invalidate();

                painter.Interval = 60 * 10 * 1000; //10 minutes

                GC.Collect();

                painter.Enabled = true;

            };
            painter.Interval = 30000;
            painter.Enabled = true;
        }
      
    }
}
