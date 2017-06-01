using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;
using Msn;
using VTools;

namespace SSF
{
    internal static class Program
    {
        private static NotifyIcon con;

      

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form toReturn = null;
       //     Application.UseWaitCursor = true;
       
            try
            {
                //create database
                Creator.Build(ref LIMS.Interface);

                LIMS.CreateLIMS();

                bool ok = Creator.CheckConnections();

                if (ok) Creator.LoadMethods(0);
                else throw new Exception("Could not start loading the database");


                Action lastCallBack;
                Action midCallBack;
                Form aboutbox = new AboutBox();

                toReturn = LIMS.CreateSSFUserInterface(ref aboutbox, out lastCallBack, out midCallBack);

                Creator.LastCallBack = lastCallBack;
                Creator.CallBack = midCallBack;

                Creator.Run();

                Application.Run(toReturn);

            }
            catch (Exception ex)
            {
                LIMS.Interface.IStore.AddException(ex);
                LIMS.Interface.IStore.SaveExceptions();
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }

      //      Application.UseWaitCursor = false;
        }

       
      

  
      
    }
}