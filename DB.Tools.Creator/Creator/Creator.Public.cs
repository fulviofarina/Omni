using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using Rsx.Dumb;

namespace DB.Tools
{
    public partial class Creator
    {
        /// <summary>
        /// Method to call back
        /// </summary>
        public static EventHandler CallBack
        {
            get { return Creator.mainCallBack; }
            set { Creator.mainCallBack = value; }
        }

        /// <summary>
        /// Another Call back method (last one)
        /// </summary>
        public static EventHandler LastCallBack
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
        public static Interface Initialize()
        {
            LINAA LINAA = new LINAA();
            Interface = new Interface(ref LINAA);
            Interface.IDB.PopulateColumnExpresions();
            return Interface;
        }

        /// <summary>
        /// Checks the directories. If restore = true, repopulation of resources is performed.
        /// Otherwise the FILE FLAG determines if restoration is performed
        /// </summary>
        /// <param name="restore"></param>
        public static void CheckDirectories(bool restore = false)
        {
            string checking = "Checking ";
            if (restore) checking = "Restoring ";
            string directories = "directories...";
            string resources = "resources...";

            Interface.IReport.Msg(checking, checking + directories);

            //check basic directory
            populateBaseDirectory(restore);
            bool overriderFound = false;
            if (!restore)
            {
                //check for FILE FLAG = overriders
                overriderFound = populateOverriders();
                Help();
            }
            else overriderFound = true;

            //populate resources
            Interface.IReport.Msg(checking, checking + resources);
            populateResources(overriderFound);

            //create developer file from mainLIMSResource
            Creator.PopulateDeveloperFile(Creator.MainLIMSResource, Resources.Linaa);


        }

        /// <summary>
        /// Closes the given LINAA database asking to Save!
        /// </summary>
        /// <param name="Linaa">      database to close</param>
        /// <param name="takeChanges">true to save changes</param>
        /// <returns></returns>
        public static bool Close()
        {
            bool eCancel = false;

            //this is important otherwise it will ask to save this
            //   Interface.IDB.MatSSF.Clear();
            Interface.IDB.Compositions.AcceptChanges();
            Interface.IDB.MatSSF.AcceptChanges();
            // Interface.IDB.MUES.Clear();
            Interface.IDB.MUES.AcceptChanges();
            //IMPORTANT FOR THE PROGRAM NOT TO ASK FOR THESE TABLES
            Interface.IStore.SaveExceptions();

            IEnumerable<DataTable> tables = Interface.IStore.GetTablesWithChanges();

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
            // if (!System.IO.File.Exists(path)) return;
            IO.Process(new System.Diagnostics.Process(), Application.StartupPath, "notepad.exe", path, false, false, 0);
        }
        /// <summary>
        /// reads from the backup LIMS.xml file or the Developers version
        /// </summary>
        public static void LoadFromFile()
        {

            string filePath = Interface.IStore.FolderPath + Resources.Backups + Resources.Linaa;
            //if it does not exist, then read the developer file
            if (!System.IO.File.Exists(filePath))
            {
                //esto cambio en uFinder asi que arreglar en MSTFF, usar lims.xml como un resource y empotrarlo
                filePath = Interface.IStore.FolderPath + Resources.DevFiles + Resources.Linaa;
            }
            Interface.Get().Clear();
            Interface.IDB.AcceptChanges();

            Interface.IStore.Read(filePath);

           
        }

       


        /// <summary>
        /// Load the list of methods to apply. It does not apply them until Run() is called
        /// </summary>
        public static void LoadMethods(int populNr)
        {
            // Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg("Methods setup", "Setting methods...");

            // Application.DoEvents();

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
            // mainCallBack?.Invoke(null,EventArgs.Empty); lastCallBack?.Invoke(null, EventArgs.Empty);
            Application.OpenForms[0].Invoke(mainCallBack);
            Application.OpenForms[0].Invoke(lastCallBack);

            // Creator.mainCallBack?.Invoke(null, EventArgs.Empty); //the ? symbol is to check first if its not null!!!
            //wow...
            //   Creator.lastCallBack?.Invoke(null,EventArgs.Empty);
        };

            //add save preferences
            //  populators.Add(Interface.IPreferences.SavePreferences);

            if (populators != null)
            {
                worker = new Loader();
                worker.Set(populators, callback, reporter);
            }

            Interface.IReport.Msg("Methods set", "Ok methods...");

            // Application.DoEvents();

            // Cursor.Current = Cursors.Default;

            // else throw new SystemException("No Populate Method was assigned");
        }

        public static void PopulatePreferences()
        {
            Interface.IReport.Msg("Populating database structure", "Initializing...");
         //   Interface.IPreferences.SavePreferences();
            Interface.IStore.CleanPreferences();
            Interface.IPreferences.PopulatePreferences();
            Interface.IPreferences.SavePreferences();
        }

        /// <summary>
        /// The methods should be loaded already, just execute...
        /// </summary>
        public static void Run()
        {
            // Cursor.Current = Cursors.WaitCursor;

            worker?.RunWorkerAsync(Interface);

            // Cursor.Current = Cursors.Default; else throw new SystemException("No Populate Method
            // was assigned");
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

        private static void populateBaseDirectory(bool restore = false)
        {
            //assign folderpath (like a App\Local folder)
            Interface.IStore.FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Interface.IStore.FolderPath += Resources.k0XFolder; //cambiar esto

            //populate main directory at folderPath
            try
            {
                if (restore) System.IO.Directory.Delete(Interface.IStore.FolderPath, true);
                IO.MakeADirectory(Interface.IStore.FolderPath);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
        }

        public static void PopulateDeveloperFile(string fileResourceContent, string fileName)
        {
            string path = Interface.IStore.FolderPath + Resources.DevFiles+fileName;
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            System.IO.File.WriteAllText(path, fileResourceContent);
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
                path = Interface.IStore.FolderPath + Resources.XCOMFolder;
                IO.MakeADirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            try
            {
              //  path = Interface.IStore.FolderPath + Resources.XCOMEnergies;
               // developerPath = Application.StartupPath + Resources.DevFiles + Resources.XCOMEnergies;
               // populateReplaceFile(path, developerPath);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
            /*
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
            */
            try
            {
                path = Interface.IStore.FolderPath + Resources.Exceptions;
                IO.MakeADirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
            try
            {
                path = Interface.IStore.FolderPath + Resources.Backups;
                IO.MakeADirectory(path, overriderFound);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }
            try
            {
         

                path = Interface.IStore.FolderPath + Resources.DevFiles;
                IO.MakeADirectory(path, overriderFound);


             


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
    }
}