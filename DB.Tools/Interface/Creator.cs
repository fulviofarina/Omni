using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Msn;
using Rsx;

namespace DB.Tools
{
    public class Creator
    {
        public static void CloneSQL(ref LinqDataContext original, ref LinqDataContext destiny)
        {
            // Type tipo = typeof(T);

            //   Type o = typeof(Unit);

            ITable dt = original.GetTable<Unit>();
            ITable ita = destiny.GetTable(typeof(Unit));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Matrix>();
            ita = destiny.GetTable(typeof(Matrix));
            InsertSQL(ref dt, ref ita);
            dt = original.GetTable<Composition>();
            ita = destiny.GetTable(typeof(Composition));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Reaction>();
            ita = destiny.GetTable(typeof(Reaction));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Sigma>();
            ita = destiny.GetTable(typeof(Sigma));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<SigmasSal>();
            ita = destiny.GetTable(typeof(SigmasSal));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Element>();
            ita = destiny.GetTable(typeof(Element));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<MonitorsFlag>();
            ita = destiny.GetTable(typeof(MonitorsFlag));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Yield>();
            ita = destiny.GetTable(typeof(Yield));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<tStudent>();
            ita = destiny.GetTable(typeof(tStudent));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<LINE>();
            ita = destiny.GetTable(typeof(LINE));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<LINES_FI>();
            ita = destiny.GetTable(typeof(LINES_FI));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<DetectorsAbsorber>();
            ita = destiny.GetTable(typeof(DetectorsAbsorber));
            InsertSQL(ref dt, ref ita);
            dt = original.GetTable<DetectorsCurve>();
            ita = destiny.GetTable(typeof(DetectorsCurve));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<DetectorsDimension>();
            ita = destiny.GetTable(typeof(DetectorsDimension));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Holder>();
            ita = destiny.GetTable(typeof(Holder));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<VialType>();
            ita = destiny.GetTable(typeof(VialType));
            InsertSQL(ref dt, ref ita);
            dt = original.GetTable<Standard>();
            ita = destiny.GetTable(typeof(Standard));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Monitor>();
            ita = destiny.GetTable(typeof(Monitor));
            InsertSQL(ref dt, ref ita);
            dt = original.GetTable<Channel>();
            ita = destiny.GetTable(typeof(Channel));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<Geometry>();
            ita = destiny.GetTable(typeof(Geometry));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<NAA>();
            ita = destiny.GetTable(typeof(NAA));
            InsertSQL(ref dt, ref ita);

            dt = original.GetTable<k0NAA>();
            ita = destiny.GetTable(typeof(k0NAA));
            InsertSQL(ref dt, ref ita);
        }

        private static void InsertSQL(ref ITable dt, ref ITable ita)
        {
            foreach (var i in dt)
            {
                ita.InsertOnSubmit(i);
            }

            ita.Context.SubmitChanges();
        }

        private static Loader worker = null;

        private static string askToSave = "Changes in the database " + " have not been saved yet\n\nDo you want to save the changes on the following tables?\n\n";
        private static string loading = "Database loading in progress";
        private static string checkingSQL = "Checking the SQL connections";
        private static string couldNotConnect = "Could not connect to LIMS DataBase";
        private static Interface Interface = null;

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
        public static void Build(ref Interface inter, ref NotifyIcon notify, ref Pop msn)
        {
            //restarting = false;

            if (inter != null)
            {
                inter.IAdapter.DisposeAdapters();
                //  Dumb.FD<LINAA>(ref Linaa);
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
            bool restartedOrReported = Interface.IReport.RestartingRoutine();

            //  return result;
        }

        /// <summary>
        /// PREPARATE
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool Prepare(int populNr)
        {
            Interface.IReport.Msg(checkingSQL, "Please wait...");

            Interface.IAdapter.InitializeComponent();

            Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

            bool ok = false;

            if (!Interface.IAdapter.IsMainConnectionOk)
            {
                //problems withe the connection
                string title = DB.Properties.Errors.Error404;
                title += Interface.IAdapter.Exception;

                Interface.IReport.SendToRestartRoutine(title);

                MessageBox.Show(title, couldNotConnect, MessageBoxButtons.OK, MessageBoxIcon.Error);

                DialogResult yesnot = MessageBox.Show("Would you like to install SQL LocalDB?", "SQL LocalDB Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (yesnot == DialogResult.Yes)
                {
                    //  System.Diagnostics.Process.Start("wpi://SQLExpress/");

                    System.Diagnostics.Process.Start(Application.StartupPath + "\\localdbx64.msi");

                    title = "\n\nThe user tried to install SQL Express";

                    MessageBox.Show("Installation of SQL LocalDB finished. Trying to start service and load database", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    title = "\n\nThe user denied the SQL Express installation";

                    MessageBox.Show("Cannot continue without a SQL connection", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //try to load the server now
                System.Diagnostics.Process.Start("SqlLocalDB.exe", "start");

                //create the database if it does not exist
                LinqDataContext destiny = new LinqDataContext(DB.Properties.Settings.Default.localDB);
                if (!destiny.DatabaseExists())
                {
                    destiny.CreateDatabase();
                    //                        destiny.DeleteDatabase();
                }
                //QUITAR DE AQUI
                //    LinqDataContext original = new LinqDataContext(DB.Properties.Settings.Default.NAAConnectionString);
                // CloneSQL(ref original, ref destiny);
                yesnot = MessageBox.Show("Do you want to restart the program now?", "Restart or Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (yesnot == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else System.Environment.Exit(-1);

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
        /// The methods are loaded already, just execute...
        /// rename maybe..
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
        /// <param name="Linaa">database to load</param>
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
                //      auxM.Add(Linaa.PopulateUnits);

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
            //   LINAA Linaa = Interface as LINAA;
            mainCallBack?.Invoke(); //the ? symbol is to check first if its not null!!!
                                    //oh these guys changed the sintaxis?
                                    //wow...

            //DataTable d =  Linaa?.Acquisitions;

            //    Application.DoEvents();
            //      Linaa.ReportFinished();
            toPopulate++;

            loadMethods(toPopulate);
            Load();
        }
    }
}