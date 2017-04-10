using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{
    public class Creator
    {
        private static Rsx.Loader worker = null;

        private static string askToSave = "Changes in the LIMS " + " has not been saved yet\n\nDo you want to save the changes on the following tables?\n\n";
        private static object dataset = null;
        private static Action lastCallBack = null;
        private static Action mainCallBack = null;
        private static String result = string.Empty;
        private static int toPopulate = 0;

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

        public static String Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// Builds a reference Linaa database, creating it if it does not exist, giving feedback through a notifyIcon and a handler to a method that will run after completition
        /// </summary>
        /// <param name="Linaa">referenced database to build (can be a null reference)</param>
        /// <param name="notify">referenced notifyIcon to give feedback of the process</param>
        /// <param name="handler">referenced handler to a method to run after completition </param>
        public static string Build(ref LINAA Linaa, ref System.Windows.Forms.NotifyIcon notify, ref Pop msn, int populNr)
        {
            //restarting = false;

            if (Linaa != null)
            {
                Linaa.DisposeAdapters();
                Dumb.FD<LINAA>(ref Linaa);
            }
            Linaa = new LINAA();
            //   dataset = null;
            dataset = Linaa;

            if (msn != null)
            {
                Linaa.Msn = msn;
            }

            if (notify != null)
            {
                Linaa.Notify = notify;
                Linaa.Msg(Linaa.DataSetName + "- Database loading in progress", "Please wait...");
            }

            //populate directories
            Linaa.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Linaa.FolderPath += Resources.k0XFolder; //cambiar esto

            Linaa.PopulateResourceDirectory(Linaa.FolderPath);

            Linaa.PopulateUserDirectories();
            //perform basic loading
            Linaa.PopulateColumnExpresions();
            Linaa.PopulatePreferences();

            Linaa.SavePreferences();

            Linaa.RestartingRoutine();

            Linaa.InitializeComponent();

            Linaa.InitializeAdapters(); //why was this after the next code? //check

            if (!Linaa.IsMainConnectionOk)
            {
                string title = DB.Properties.Errors.Error404;
                title += Linaa.Exception;
                result = title;
            }
            else
            {
                loadMethods(ref Linaa, populNr);
            }

            return result;
        }

        /// <summary>
        /// Closes the given LINAA database
        /// </summary>
        /// <param name="Linaa">database to close</param>
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

            Linaa.SavePreferences();

            bool savedremotely = Linaa.SaveRemote(ref tables, takeChanges);
            bool savedlocally = Linaa.SaveLocalCopy();

            eCancel = !savedremotely || !savedlocally;
            //if cancel, this means that a remote or a local copy could not be saved,
            //not good, this is the worst nightmare...
            //the backup

            return eCancel;
        }

        /// <summary>
        /// The methods are loaded already, just execute...
        /// rename maybe..
        /// </summary>
        public static void Load()
        {
            if (worker != null)
            {
                worker.RunWorkerAsync(dataset);
            }
            // else throw new SystemException("No Populate Method was assigned");
        }

        /// <summary>
        /// Creates a background worker that will feedback through an inputed runworkerCompleted handler
        /// </summary>
        /// <param name="Linaa">database to load</param>
        /// <param name="handler">required handler to report feedback when completed</param>
        public static void loadMethods(ref LINAA Linaa, int populNr)
        {
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
                //      auxM.Add(Linaa.PopulateUnits);

                report = Linaa.ReportProgress;
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
            LINAA Linaa = dataset as LINAA;
            mainCallBack?.Invoke(); //the ? symbol is to check first if its not null!!!
                                    //oh these guys changed the sintaxis?
                                    //wow...

            //DataTable d =  Linaa?.Acquisitions;

            //    Application.DoEvents();
            //      Linaa.ReportFinished();
            toPopulate++;

            loadMethods(ref Linaa, toPopulate);
            Load();
        }
    }
}