using System;
using System.Collections.Generic;
using System.Linq;
using Rsx;

namespace DB.Tools
{
    public partial class Creator
    {
        private static string deniedTheInstall = "\n\nThe user denied the SQL Express installation";
        private static string nocontinueWOSQL = "Cannot continue without a SQL connection";
        private static string shouldInstallSQL = "Would you like to install SQL LocalDB?";
        private static string sqlDBEXE = "SqlLocalDB.exe";
        private static string sqlLocalDB = "SQL LocalDB Installation";
        private static string sqlPack32 = "localdbx32.msi";
        private static string sqlPack64 = "localdbx64.msi";
        private static string sqlStarted = "Installation of SQL LocalDB started. When finished click OK to restart";
        private static string triedToInstall = "\n\nThe user tried to install SQL Express";
    }

    public partial class Creator
    {
        private static string askToSave = "Changes in the database have not been saved yet\n\nDo you want to save the changes on the following tables?\n\n";
        private static string checkingSQL = "Checking the SQL connections";
        private static string couldNotConnect = "Could not connect to LIMS DataBase";
        private static string loading = "Database loading in progress";
        private static string noConnectionDetected = "Please check the k0X Database connection.\nThe Server might be down, not installed, misconfigured or just offline.\n\nError message:\n\n";
        private static string restartingOk = "Restarting succeeded...";
    }

    public partial class Creator
    {
        private static Interface Interface = null;

        private static Action lastCallBack = null;
        private static Action mainCallBack = null;

        private static int toPopulate = 0;

        private static Loader worker = null;

        /// <summary>
        /// disposes the worker that loads the data
        /// </summary>
        private static void disposeWorker()
        {
            if (worker != null)
            {
                worker.CancelAsync();
                worker.Dispose();
                worker = null;
            }
        }

        /// <summary>
        /// the last end routine
        /// </summary>
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

        private static void loadMethods(int populNr)
        {
            LINAA Linaa = Interface.Get();

            IList<Action> auxM = null;

            Action todo = null;

            Rsx.Loader.Reporter report = null;

            if (toPopulate == 1)
            {
                auxM = new Action[]
                {
             Linaa.PopulateElements,

       Linaa.PopulateReactions,
         Linaa.PopulatepValues,
                 Linaa.PopulateSigmas,
                   Linaa.PopulateSigmasSal,
                   Linaa.PopulateYields,
            };

                todo = lastCallBack;
            }
            else if (toPopulate == 0)
            {
                auxM = new Action[]
                {
           Linaa.PopulateChannels,
          Linaa.PopulateIrradiationRequests,
       Linaa.PopulateOrders,
        Linaa.PopulateProjects
                };

                IEnumerable<Action> enums = auxM;

                enums = enums.Union(Linaa.PMMatrix());
                enums = enums.Union(Linaa.PMStd());
                enums = enums.Union(Linaa.PMDetect());

                auxM = enums.ToList();
                // auxM.Add(Linaa.PopulateUnits);

                report = Interface.IReport.ReportProgress;
                todo = endRoutine;
            }

            if (auxM != null)
            {
                disposeWorker();

                worker = new Rsx.Loader();

                worker.Set(auxM, todo, report);
            }
            // else throw new SystemException("No Populate Method was assigned");
        }
    }
}