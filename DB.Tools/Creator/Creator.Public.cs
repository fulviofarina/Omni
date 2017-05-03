using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using Rsx;
using Rsx.Generic;

namespace DB.Tools
{
    public partial class Creator
    {
        /// <summary>
        /// a method to call back
        /// </summary>
        public static Action CallBack
        {
            get { return Creator.mainCallBack; }
            set { Creator.mainCallBack = value; }
        }

        /// <summary>
        /// The last call back method
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
            //restarting = false;



         //   Rsx.Dumb.RestartPC();


            Cursor.Current = Cursors.WaitCursor;

            if (inter != null)
            {
                inter.IAdapter.DisposeAdapters();
                // Dumb.FD<LINAA>(ref Linaa);
            }
            LINAA LINAA = new LINAA();
            inter = new Interface(ref LINAA);

            Interface = inter;

            Interface.IDB.PopulateColumnExpresions();

            Interface.IBS.ApplyFilters();
            Cursor.Current = Cursors.Default;

            Interface.IReport.Msg(loading, "Please wait...");
        }

        /// <summary>
        /// Checks if the due directories exist
        /// </summary>
        public static void CheckDirectories()
        {
            Cursor.Current = Cursors.WaitCursor;

            //assign folderpath (like a App\Local folder)
            Interface.IMain.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Interface.IMain.FolderPath += Resources.k0XFolder; //cambiar esto

            //populate main directory at folderPath
            try
            {
                Rsx.Dumb.MakeADirectory(Interface.IMain.FolderPath);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            //check for overriders
            bool overriderFound = populateOverriders();

            //populate resources
            PopulateResources(overriderFound);

            //perform basic loading

            //BUG REPORT HERE IN CASE I OVERRIDE IT OR THERE ARE EXCEPTIONS

            Cursor.Current = Cursors.Default;
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

            eCancel = !SaveInFull(takeChanges);
            //if cancel, this means that a remote or a local copy could not be saved,
            //not good, this is the worst nightmare...
            //the backup

            return eCancel;
        }

        public static void Help()
        {
            string path = Interface.IMain.FolderPath + DB.Properties.Resources.Features;
            if (!System.IO.File.Exists(path)) return;

            Dumb.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
        }

        public static void LoadMethods(int populNr)
        {
            Cursor.Current = Cursors.WaitCursor;

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

            Cursor.Current = Cursors.Default;

            // else throw new SystemException("No Populate Method was assigned");
        }

        public static void PopulateResources(bool overriderFound)
        {
            string path = string.Empty;
            try
            {
                path = Interface.IMain.FolderPath + Resources.Exceptions;
                Rsx.Dumb.MakeADirectory(path, overriderFound);

                path = Interface.IMain.FolderPath + Resources.Backups;
                Rsx.Dumb.MakeADirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            try
            {
                populateSolCoiResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }

            try
            {
                populateMatSSFResource(overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IMain.AddException(ex);//                throw;
            }
        }

        /// <summary>
        /// The methods are loaded already, just execute...
        /// </summary>
        public static void Run()
        {
            Cursor.Current = Cursors.WaitCursor;

            worker?.RunWorkerAsync(Interface);

            Cursor.Current = Cursors.Default;
            // else throw new SystemException("No Populate Method was assigned");
        }

        public static bool SaveInFull(bool takechanges)
        {
            bool ok = false;

            Cursor.Current = Cursors.WaitCursor;

            //    WHAT IS THIS
            //WHAT IS THIS
            bool off = Interface.IPreferences.CurrentPref.Offline;
            // string savePath = Interface.IMain.FolderPath + "lims.xml";
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
                    savedremotely = Interface.IStore.SaveRemote(ref tables, takechanges);
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
                Interface.IMain.AddException(ex);
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