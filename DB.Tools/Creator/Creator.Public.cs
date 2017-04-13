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
        public static Action CallBack
        {
            get { return Creator.mainCallBack; }
            set { Creator.mainCallBack = value; }
        }

        public static Action LastCallBack
        {
            get { return Creator.lastCallBack; }
            set { Creator.lastCallBack = value; }
        }


        /// <summary>
        /// Builds a reference Linaa database, creating it if it does not exist, giving feedback
        /// through a notifyIcon and a handler to a method that will run after completition
        /// </summary>
        /// <param name="Linaa">  referenced database to build (can be a null reference)</param>
        /// <param name="notify"> referenced notifyIcon to give feedback of the process</param>
        /// <param name="handler">referenced handler to a method to run after completition</param>
        public static void Build(ref Interface inter, ref NotifyIcon notify, ref Pop msn)
        {
            //restarting = false;

            if (inter != null)
            {
                inter.IAdapter.DisposeAdapters();
                // Dumb.FD<LINAA>(ref Linaa);
            }
            LINAA LINAA = new LINAA();
            inter = new Interface(ref LINAA);

            Interface = inter;
            if (msn != null)
            {
                Interface.IReport.Msn = msn;
            }

            if (notify != null)
            {
                Interface.IReport.Notify = notify;
            }

            Interface.IReport.Msg(loading, "Please wait...");
        }

        public static void CheckDirectories()
        {
            //assign folderpath (like a App\Local folder)
            Interface.IMain.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Interface.IMain.FolderPath += Resources.k0XFolder; //cambiar esto

            //populate main directory at folderPath
            Interface.IPopulate.PopulateDirectory(Interface.IMain.FolderPath);

            //check for overriders
            bool overriderFound = Interface.IPopulate.PopulateOverriders();

            //populate resources
            Interface.IPopulate.PopulateResources(overriderFound);

            //perform basic loading

            Interface.IMain.PopulateColumnExpresions();

            Interface.IPreferences.PopulatePreferences();

            Interface.IPreferences.SavePreferences();

       

            //BUG REPORT HERE IN CASE I OVERRIDE IT OR THERE ARE EXCEPTIONS
            bool restartedOrReported = Interface.IPopulate.RestartingRoutine();

            // return result;
        }

      
        /// <summary>
        /// Closes the given LINAA database
        /// </summary>
        /// <param name="Linaa">      database to close</param>
        /// <param name="takeChanges">true to save changes</param>
        /// <returns></returns>
        public static bool Close(ref LINAA Linaa)
        {
            bool eCancel = false;

            IEnumerable<DataTable> tables = Linaa.GetTablesWithChanges();

            IList<string> tablesLs = tables.Select(o => o.TableName).Distinct().ToList();

            bool takeChanges = false;

            if (tablesLs.Count != 0)
            {
                string tablesToSave = string.Empty;
                foreach (string s in tablesLs) tablesToSave += s + "\n";
                string ask = askToSave + tablesToSave;
                MessageBoxButtons mb = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                DialogResult result = MessageBox.Show(ask, "Save Changes...", mb, icon);
                if (result == DialogResult.Yes) takeChanges = true;
                else if (result == DialogResult.Cancel)
                {
                    eCancel = true;
                    return eCancel;
                }
            }

            Interface.IPreferences.SavePreferences();

            bool savedremotely = Linaa.SaveRemote(ref tables, takeChanges);
            bool savedlocally = Linaa.SaveLocalCopy();

            eCancel = !savedremotely || !savedlocally;
            //if cancel, this means that a remote or a local copy could not be saved,
            //not good, this is the worst nightmare...
            //the backup

            return eCancel;
        }

        /// <summary>
        /// The methods are loaded already, just execute... rename maybe..
        /// </summary>
        public static void Load()
        {
            if (worker != null)
            {
                worker.RunWorkerAsync(Interface);
            }
            // else throw new SystemException("No Populate Method was assigned");
        }

        /// <summary>
        /// Creates a background worker that will feedback through an inputed runworkerCompleted handler
        /// </summary>
        /// <param name="Linaa">  database to load</param>
        /// <param name="handler">required handler to report feedback when completed</param>
        public static void loadMethods(int populNr)
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

        /// <summary>
        /// PREPARATE
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool Prepare(int populNr)
        {
            Interface.IReport.Msg(checkingSQL, "Please wait...");

            Interface.IPopulate.RestartSQLServer();

            Interface.IAdapter.InitializeComponent();

            Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

            bool ok = false;

            if (!Interface.IAdapter.IsMainConnectionOk)
            {

                Interface.IReport.UserInfo();

                 string title = noConnectionDetected;
                 title += Interface.IAdapter.Exception;
               
                  Interface.IPopulate.SendToRestartRoutine(title);
                 MessageBox.Show(title, couldNotConnect, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //could not connect
                Interface.IPopulate.PopulateSQLDatabase();
                //send this text to a textFile in order to report by email next Reboot
            }
            else
            {
                //ACUMULA LOS METODOS Y CREA EL WORKER, ESPERA FOR RUN...
                loadMethods(populNr);
                ok = true;
            }

            return ok;
        }

 
    }
}