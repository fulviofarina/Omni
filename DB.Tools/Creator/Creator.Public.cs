using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using Rsx.Dumb;
using Rsx.Generic;

namespace DB.Tools
{
    public partial class Creator
    {
        /// <summary>
        /// Method to call back
        /// </summary>
        public static Action CallBack
        {
            get { return Creator.mainCallBack; }
            set { Creator.mainCallBack = value; }
        }

        /// <summary>
        /// Another Call back method (last one)
        /// </summary>
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
        public static void Build(ref Interface inter)
        {
          

            if (inter != null)
            {
                inter.IAdapter.DisposeAdapters();
           
            }
            LINAA LINAA = new LINAA();
            inter = new Interface(ref LINAA);
            Interface = inter;
          //en este orden

            checkDirectories();

            MatSSF.StartupPath = Interface.IStore.FolderPath + Resources.SSFFolder;

            Interface.IReport.Msg(LOADING_DB, "Initializing...");

        //    Application.DoEvents();

            Interface.IDB.PopulateColumnExpresions();
            Interface.IPreferences.PopulatePreferences();
            Interface.IPreferences.SavePreferences();

        }

        /// <summary>
        /// Checks if the due directories exist
        /// </summary>
        private static void checkDirectories()
        {
            Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg(LOADING_DB, "Checking directories...");
            Application.DoEvents();

            //check basic directory
            populateBaseDirectory();
            //check for overriders
            bool overriderFound = populateOverriders();

            Help();

            //populate resources
            Interface.IReport.Msg(LOADING_DB, "Checking resources...");
            Application.DoEvents();
            populateResources(overriderFound);

            //BUG REPORT HERE IN CASE I OVERRIDE IT OR THERE ARE EXCEPTIONS

            Cursor.Current = Cursors.Default;
        }

        private static void populateBaseDirectory()
        {
            //assign folderpath (like a App\Local folder)
            Interface.IStore.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Interface.IStore.FolderPath += Resources.k0XFolder; //cambiar esto

            //populate main directory at folderPath
            try
            {
                Rsx.Dumb.IO.MakeADirectory(Interface.IStore.FolderPath);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
        }

        /// <summary>
        /// Closes the given LINAA database asking to Save!
        /// </summary>
        /// <param name="Linaa">      database to close</param>
        /// <param name="takeChanges">true to save changes</param>
        /// <returns></returns>
        public static bool Close(ref LINAA Linaa)
        {
            bool eCancel = false;

            //this is important otherwise it will ask to save this
            //   Interface.IDB.MatSSF.Clear();
            Interface.IDB.MatSSF.AcceptChanges();
            // Interface.IDB.MUES.Clear();
            Interface.IDB.MUES.AcceptChanges();
            //IMPORTANT FOR THE PROGRAM NOT TO ASK FOR THESE TABLES
            Interface.IStore.SaveExceptions();

            IEnumerable<DataTable> tables = Linaa.GetTablesWithChanges();

            IList<string> tablesLs = tables.Select(o => o.TableName).Distinct().ToList();

            bool takeChanges = false;

            if (tablesLs.Count != 0)
            {
                string tablesToSave = string.Empty;
                foreach (string s in tablesLs) tablesToSave += s + "\n";
                string ask = ASK_TO_SAVE + tablesToSave;
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

           // eCancel = !SaveInFull(takeChanges);
            SaveInFull(takeChanges);
            //if cancel, this means that a remote or a local copy could not be saved,
            //not good, this is the worst nightmare...
            //the backup

            return eCancel;
        }

        /// <summary>
        /// Seek Help
        /// </summary>
        public static void Help()
        {
            string path = Application.StartupPath + Resources.DevFiles + Resources.Features;
            string currentpath = Interface.IStore.FolderPath + Resources.Features;
            bool features = populateReplaceFile(currentpath, path);
            if (!features) return;
        //    if (!System.IO.File.Exists(path)) return;
            IO.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
        }

        public static void InstallMSMQ()
        {
            try
            {
                Interface.IReport.Msg(LOADING_DB, "Installing MSMQ...");

                Application.DoEvents();

                IO.InstallMSMQ(false);
            }
            catch (Exception ex)
            {
                // Interface.IReport.Msg(ex.InnerException.Message, ex.Message, false);
                Interface.IStore.AddException(ex);
            }
        }

        /// <summary>
        /// Load the list of methods to apply. It does not apply them until Run() is called
        /// </summary>
        public static void LoadMethods(int populNr)
        {
            Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg(LOADING_DB, "Setting methods...");

            Application.DoEvents();

            LINAA Linaa = Interface.Get();

            IList<Action> populators = null;
            IList<Action> populators2 = null;

            Action callback = null;

            Action<int> reporter = null;

            //before 0

            populators = new Action[]
            {
           Linaa.PopulateChannels,
          Linaa.PopulateIrradiationRequests,
       Linaa.PopulateOrders,
        Linaa.PopulateProjects
            };

            IEnumerable<Action> enums = populators;
            enums = enums.Union(Linaa.PMMatrix());
            enums = enums.Union(Linaa.PMStd());
            enums = enums.Union(Linaa.PMDetect());

            populators2 = new Action[]
         {
              // Application.DoEvents,
             Linaa.PopulateElements,
       Linaa.PopulateReactions,
         Linaa.PopulatepValues,
         Linaa.PopulatetStudent,
        Linaa.PopulateNAA,
        Linaa.Populatek0NAA,
                 Linaa.PopulateSigmas,
                   Linaa.PopulateSigmasSal,
                   Linaa.PopulateYields,
         };
            enums = enums.Union(populators2);
            populators = enums.ToList();

            reporter = Interface.IReport.ReportProgress;

            callback = delegate
        {
            Creator.mainCallBack?.Invoke(); //the ? symbol is to check first if its not null!!!
                                            //wow...
            Creator.lastCallBack?.Invoke();
        };

            //add save preferences
            //  populators.Add(Interface.IPreferences.SavePreferences);

            if (populators != null)
            {
                if (worker != null)
                {
                    worker.CancelAsync();
                    worker.Dispose();
                    worker = null;
                }
                worker = new Loader();
                worker.Set(populators, callback, reporter);
            }

            Interface.IReport.Msg(LOADING_DB, "Ok methods...");

            Application.DoEvents();

            Cursor.Current = Cursors.Default;

            // else throw new SystemException("No Populate Method was assigned");
        }

        /// <summary>
        /// Populates Solcoi, MatSSF and other resources
        /// </summary>
        private static void populateResources(bool overriderFound)
        {
            string path = string.Empty;
            string developerPath = string.Empty;

            try
            {
                path = Interface.IStore.FolderPath + Resources.XCOMEnergies;
                developerPath = Application.StartupPath + Resources.DevFiles + Resources.XCOMEnergies;
                populateReplaceFile(path, developerPath);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            try
            {
                path = Interface.IStore.FolderPath + Resources.WCalc;
                developerPath = Application.StartupPath + Resources.DevFiles + Resources.WCalc;
                populateReplaceFile(path, developerPath);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            try
            {
                path = Interface.IStore.FolderPath + Resources.Exceptions;
                Rsx.Dumb.IO.MakeADirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
            try
            {
                path = Interface.IStore.FolderPath + Resources.Backups;
                Rsx.Dumb.IO.MakeADirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            try
            {
                populateSolCoiResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            try
            {
                populateMatSSFResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
        }

        /// <summary>
        /// The methods should be loaded already, just execute...
        /// </summary>
        public static void Run()
        {
            Cursor.Current = Cursors.WaitCursor;

            worker?.RunWorkerAsync(Interface);

            Cursor.Current = Cursors.Default;
            // else throw new SystemException("No Populate Method was assigned");
        }

        /// <summary>
        /// Save the database in Full (remotely and locally)
        /// </summary>
        public static bool SaveInFull(bool takechanges)
        {
            bool ok = false;

            Cursor.Current = Cursors.WaitCursor;

            //    WHAT IS THIS
            //WHAT IS THIS
            bool off = Interface.IPreferences.CurrentPref.Offline;
            // string savePath = Interface.IStore.FolderPath + "lims.xml";
            // Interface.IStore.SaveSSF(off, savePath);

            Interface.IBS.EndEdit();
          
            // Interface.Get().BeginEndLoadData(false); Interface.IBS.EndEdit();
            Interface.IPreferences.SavePreferences();
            Interface.IStore.SaveExceptions();


            Interface.IReport.Msg("Saved preferences", "Saved!");

            try
            {
                // IEnumerable<DataTable> tables = Interface.IDB.Tables.OfType<DataTable>();
                IEnumerable<DataTable> tables = Interface.IStore.GetTablesWithChanges();

                bool savedremotely = true;
                if (!off)
                {
                    savedremotely = Interface.IStore.SaveRemote(ref tables);
                    Interface.IReport.Msg("Saved into SQL database", "Saved!");
                }

                bool savedlocaly = Interface.IStore.SaveLocalCopy();
                Interface.IReport.Msg("Saved into local XML file", "Saved!");

                // Interface.IReport.Msg("Saving", "Saving completed!");

                ok = savedlocaly && savedremotely;
                // Interface.IReport.Msg("Saving database", "Saved!");
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.StackTrace + "\n", "Error", false);
            }

            Cursor.Current = Cursors.Default;

            return ok;
        }

        /// <summary>
        /// Creates a background worker that will feedback through an inputed runworkerCompleted handler
        /// </summary>
        /// <param name="Linaa">  database to load</param>
        /// <param name="handler">required handler to report feedback when completed</param>
    }
}