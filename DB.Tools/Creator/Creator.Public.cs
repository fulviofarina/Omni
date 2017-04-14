using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;
using DB.Linq;
using DB.Properties;
using Msn;
using Rsx;

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
        public static void Build(ref Interface inter,  ref Pop msn)
        {
            //restarting = false;

            Cursor.Current = Cursors.WaitCursor;


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
              //  msn.ParentForm.Opacity = 100;
            }

          

            Cursor.Current = Cursors.Default;

            Interface.IMain.PopulateColumnExpresions();

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

     

            Interface.IPreferences.PopulatePreferences();

            Interface.IPreferences.SavePreferences();



            //BUG REPORT HERE IN CASE I OVERRIDE IT OR THERE ARE EXCEPTIONS
            bool restartedOrReported = checkBugFile();

            Cursor.Current = Cursors.Default;


         
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

          

            eCancel = !SaveInFull(takeChanges);
            //if cancel, this means that a remote or a local copy could not be saved,
            //not good, this is the worst nightmare...
            //the backup

            return eCancel;
        }

        public static bool  SaveInFull (bool takechanges)
        {
            bool ok = false;

            Cursor.Current = Cursors.WaitCursor;
          

            try
            {
                Interface.IBS.EndEdit();
          
                //    WHAT IS THIS
                //WHAT IS THIS
                bool off = Interface.IPreferences.CurrentPref.Offline;
                //     string savePath = Interface.IMain.FolderPath + "lims.xml";
                // Interface.IStore.SaveSSF(off, savePath);

                Interface.IPreferences.SavePreferences();
                Interface.IReport.Msg("Saving preferences", "Saved!");

                IEnumerable<DataTable> tables = Interface.IDB.Tables.OfType<DataTable>();

                bool savedremotely = true;
                if (!off)
                {
                    savedremotely = Interface.IStore.SaveRemote(ref tables, takechanges);
                    Interface.IReport.Msg("Saving to SQL database", "Saved!");
                }

                bool savedlocaly = Interface.IStore.SaveLocalCopy();
                Interface.IReport.Msg("Saving XML database", "Saved!");
                //   Interface.IReport.Msg("Saving", "Saving completed!");

                ok = savedlocaly && savedremotely;
                //   Interface.IReport.Msg("Saving database", "Saved!");
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
        /// The methods are loaded already, just execute...
        /// </summary>
        public static void Load()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (worker != null)
            {
                worker.RunWorkerAsync(Interface);
            }

            Cursor.Current = Cursors.Default;
            // else throw new SystemException("No Populate Method was assigned");
        }

        /// <summary>
        /// Creates a background worker that will feedback through an inputed runworkerCompleted handler
        /// </summary>
        /// <param name="Linaa">  database to load</param>
        /// <param name="handler">required handler to report feedback when completed</param>
    

        /// <summary>
        /// Prepare the needed methods and the worker
        /// </summary>
        /// <param name="populNr"></param>
        /// <returns></returns>
        public static bool Prepare(int populNr)
        {
            Cursor.Current = Cursors.WaitCursor;

            Interface.IReport.Msg(checkingSQL, "Please wait...");

            RestartSQLServer();

            Interface.IAdapter.InitializeComponent();

            Interface.IAdapter.InitializeAdapters(); //why was this after the next code? //check

            bool ok = false;

            if (!Interface.IAdapter.IsMainConnectionOk)
            {

              
                Interface.IReport.UserInfo();

                string title = noConnectionDetected;
                title += Interface.IAdapter.Exception;

                SendToRestartRoutine(title);

                Cursor.Current = Cursors.Default;

                MessageBox.Show(title, couldNotConnect, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //could not connect
                PopulateSQLDatabase();


                //send this text to a textFile in order to report by email next Reboot
            }
            else
            {
                //ACUMULA LOS METODOS Y CREA EL WORKER, ESPERA FOR RUN...
                loadMethods(populNr);
                ok = true;
            }

            Cursor.Current = Cursors.Default;

            return ok;
        }


    }
}