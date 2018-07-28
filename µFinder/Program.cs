using System;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;

namespace µFinder
{
    static class Program
    {

       // public static string TITLE = "µ-Finder";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        private static void Main()
        {

            string crashFile = "crash.txt";
            bool offline = true;


         crashFile =    Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\" +crashFile;
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //create aboutBox
                Form aboutbox = new AboutBox();
                //associate interface to LIMS UI
                LIMSUI.Interface = Creator.Initialize();
                //create database manager UI
                LIMSUI.CreateLIMS(ref aboutbox);


                Creator.MainLIMSResource = Properties.Resources.LIMS;

                Creator.CheckDirectories();


               /// Properties.Resources.LIMS = "s";
            //    string fileResource = Properties.Resources.LIMS;


            

             

                Creator.PopulatePreferences();

                LIMSUI.Interface.IPreferences.CurrentPref.Offline = offline;
                LIMSUI.Interface.IPreferences.CurrentPref.AdvancedEditor = true;
//
                bool noDGVControls = false;
                UserControl control = LIMSUI.CreateUI(ControlNames.Matrices, null, noDGVControls);

       //         LIMSUI.Interface.IPreferences.CurrentPref.AdvancedEditor = true;



                bool show = false;
                LIMSUI.CreateForm(Application.ProductName, ref control, show);
                Form toShow = control.ParentForm;




           
                toShow.StartPosition = FormStartPosition.CenterScreen;
                toShow.WindowState = FormWindowState.Maximized;
              //  toShow.ControlBox = false;
             

                //      LIMSUI.Interface.IPreferences.CurrentPref.Offline = offline;


                //    readCrash(crashFile);


                Application.Run(toShow);

               

             

            }
            catch (Exception ex)
            {
                writeCrash(crashFile, ex);

            }

        }

        private static void readCrash(string crashFile)
        {
            if (!System.IO.File.Exists(crashFile)) return;
        
                string error = System.IO.File.ReadAllText(crashFile);
                string crashtitle = "The program just recovered from a crash";
                MessageBox.Show(error, crashtitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                System.IO.File.Delete(crashFile);
          
        }

        private static void writeCrash(string crashFile, Exception ex)
        {
            string error = "Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace;
            System.IO.File.WriteAllText(crashFile, error);
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
