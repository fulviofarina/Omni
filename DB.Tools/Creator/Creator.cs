using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;

using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{
    public partial class Creator
    {
        private static string askToSave = "Changes in the database have not been saved yet\n\nDo you want to save the changes on the following tables?\n\n";
        private static string checkingSQL = "Checking the SQL connections";
        private static string couldNotConnect = "Could not connect to LIMS DataBase";
        private static string noConnectionDetected = "Please check the k0X Database connection.\nThe Server might be down, not installed, misconfigured or just offline.\n\nError message:\n\n";
        private static string loading = "Database loading in progress";

    }
    public partial class Creator
    {

        private static Interface Interface = null;

        private static Action lastCallBack = null;
        private static Action mainCallBack = null;

        private static int toPopulate = 0;

        private static Loader worker = null;

        private static void disposeWorker()
        {
            if (worker != null)
            {
                worker.CancelAsync();
                worker.Dispose();
                worker = null;
            }
        }

        private static void endRoutine()
        {
            // LINAA Linaa = Interface as LINAA;
            mainCallBack?.Invoke(); //the ? symbol is to check first if its not null!!!
                                    //oh these guys changed the sintaxis?
                                    //wow...

            //DataTable d =  Linaa?.Acquisitions;

            // Application.DoEvents(); Linaa.ReportFinished();
            toPopulate++;

            loadMethods(toPopulate);
            Load();
        }

    }

  
}