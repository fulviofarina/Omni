﻿using DB.Tools;
using DB.UI;
using System;
using System.Reflection;
using System.Windows.Forms;
using VTools;

namespace SSF
{
    internal static class Program
    {
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

        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
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

                IAboutBox aboutbox = new AboutBox();
                aboutbox.AssemblyToProvide = Assembly.GetExecutingAssembly();

                //file with resources
              
                Creator.RSX.MainMatSSFResource = Properties.Resources.MatSSF;
                Creator.RSX.MainSolCoiResource = Properties.Resources.solcoi;

                Creator.RSX.MainLIMSResource = Properties.Resources.LIMS;
                Creator.RSX.SQLResource = Properties.Resources.localDb;


                LIMSUI.Start(ref aboutbox, false, false);


             

                bool ok = Util.UtilSQL.CheckConnectionsRoutine(true, true, false);
                if (ok) Creator.LoadMethods(0);
                else throw new Exception("Could not start loading the database");

                //EventHandler firstCallBack;
            //    Form toShow = LIMSUI.createSSFApplication();


                //set the editor
                bool noDGVControls = true;
                UserControl control = LIMSUI.CreateUI(ControlNames.SSF, noDGVControls);

                bool showAlready = false;
                Creator.CreateAppForm(Application.ProductName, ref control, showAlready);

                Form toShow = control.ParentForm;

                toShow.WindowState = FormWindowState.Maximized;



                Creator.Run();

                // PainterTimer();

                Application.Run(toShow);
            }
            catch (Exception ex)
            {
                LIMSUI.Interface.IStore.AddException(ex);
                LIMSUI.Interface.IStore.SaveExceptions();
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }
        }
    }
}