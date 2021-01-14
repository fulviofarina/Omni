using Rsx.Dumb;
using System;
using System.Collections.Generic;
using DB.Properties;

using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DB.Tools
{

    /// <summary>
    /// Private components
    /// </summary>
    public partial class Creator
    {

        private const string NOTEPAD_APP = "notepad.exe";

        /// <summary>
        /// The interface class
        /// </summary>
        private static Interface Interface = null;

        
        private static EventHandler callBackLast = null;

       
        private static IList<object> userControls = new List<object>();


        private static EventHandler callBackMain = null;
     
        /// <summary>
        /// Loader app
        /// </summary>
        private static ILoader worker = null;
      
    }


    /// <summary>
    /// Public components
    /// </summary>
    public partial class Creator
    {



        /// <summary>
        /// List of open user controls
        /// </summary>
        public static IList<object> UserControls
        {
            get { return userControls; }
            set { userControls = value; }
        }
        /// <summary>
        /// The Last CallBack routine after creator runs
        /// </summary>
        public static EventHandler CallBack
        {
            get { return callBackMain; }
            set { callBackMain = value; }
        }

        /// <summary>
        /// The Main CallBack routine after creator runs
        /// </summary>
        public static EventHandler LastCallBack
        {
            get { return callBackLast; }
            set { callBackLast = value; }
        }

        /// <summary>
        /// Builds a reference Linaa database, creating it if it does not exist, giving feedback
        /// through a notifyIcon and a handler to a method that will run after completition
        public static Interface Set()
        {
            LINAA LINAA = new LINAA();
            Interface = new Interface(ref LINAA);

            Interface.IExpressions.PopulateColumnExpresions();


            return Interface;
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
                string ask = Util.UtilSQL.Strings.ASK_TO_SAVE + tablesToSave;
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
                Application.OpenForms[0].Invoke(callBackMain);
                Application.OpenForms[0].Invoke(callBackLast);

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
                    savedremotely = Interface.IStore.SaveTables(ref tables);
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





        public static void CreateAppForm(string title, ref UserControl control, bool show = true)
        {
            if (control == null) return;
            Auxiliar form = new Auxiliar();
            form.MaximizeBox = true;
            form.Populate(control);
            form.Text = title;
            form.Visible = show;
            form.StartPosition = FormStartPosition.CenterScreen;
        }
    }




}