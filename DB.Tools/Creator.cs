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
        /// <summary>
        /// a worker to keep in cache (static) for populating a database...
        /// </summary>

        /// <summary>
        /// Creates a background worker that will feedback through an inputed runworkerCompleted handler
        /// </summary>
        /// <param name="Linaa">database to load</param>
        /// <param name="handler">required handler to report feedback when completed</param>

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
        public static string Build(ref LINAA Linaa, ref System.Windows.Forms.NotifyIcon notify, ref Pop msn)
        {
            //restarting = false;
            dataset = null;

            if (Linaa != null)
            {
                Linaa.DisposeAdapters();
                Dumb.FD<LINAA>(ref Linaa);
            }
            Linaa = new LINAA();
            dataset = Linaa;
            Linaa.InitializeComponent();

            if (msn != null)
            {
                Linaa.Msn = msn;
            }

            if (notify != null)
            {
                Linaa.Notify = notify;
                Linaa.Msg(Linaa.DataSetName + "- Database loading in progress", "Please wait...");
            }

            //perform basic loading
            Action[] populMethod = Linaa.PMBasic(); //i think this only loads the preferences

            foreach (Action a in populMethod)
            {
                try
                {
                    a.Invoke();
                }
                catch (SystemException ex)
                {
                    Linaa.AddException(ex);
                }
            }

            string cmd = Application.StartupPath + Resources.Restarting;
            if (System.IO.File.Exists(cmd))
            {
                //  restarting = true;
                string email = System.IO.File.ReadAllText(cmd);
                System.IO.File.Delete(cmd);
                Linaa.GenerateReport("Restarting succeeded...", string.Empty, string.Empty, Linaa.DataSetName, email);
            }

            Linaa.InitializeAdapters(); //why was this after the next code? //check

            if (!Linaa.IsMainConnectionOk)
            {
                string title = DB.Properties.Errors.Error404;
                title += Linaa.Exception;
                result = title;
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

            IEnumerable<System.Data.DataTable> tables = Linaa.GetTablesWithChanges();

            System.Collections.Generic.IList<string> tablesLs = tables.Select(o => o.TableName).Distinct().ToList();

            bool takeChanges = false;

            if (tablesLs.Count != 0)
            {
                string tablesToSave = string.Empty;
                foreach (string s in tablesLs) tablesToSave += s + "\n";
                string ask = "Changes in the LIMS " + " has not been saved yet\n\nDo you want to save the changes on the following tables?\n\n" + tablesToSave;
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(ask, "Save Changes...", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.Yes) takeChanges = true;
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    eCancel = true;
                    return eCancel;
                }
            }

            try
            {
                Linaa.SavePreferences();
                if (takeChanges)
                {
                    foreach (System.Data.DataTable t in tables)
                    {
                        IEnumerable<DataRow> rows = t.AsEnumerable();
                        Linaa.Save(ref rows);
                    }
                }
            }
            catch (SystemException ex)
            {
                Linaa.AddException(ex);
            }
            try
            {
                string LIMSPath = Linaa.FolderPath + DB.Properties.Resources.Linaa;
                if (System.IO.File.Exists(LIMSPath))
                {
                    System.IO.File.Copy(LIMSPath, LIMSPath.Replace(".xml", "." + DateTime.Now.DayOfYear.ToString() + ".xml"), true);
                    System.IO.File.Delete(LIMSPath);
                }
                Linaa.WriteXml(LIMSPath, XmlWriteMode.WriteSchema);
                Linaa.SaveExceptions();
            }
            catch (SystemException ex)
            {
                eCancel = true;
                Linaa.AddException(ex);
            }

            return eCancel;
        }

        public static void Load(ref LINAA Linaa, int populNr)
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
                auxM.Add(Linaa.PopulateUnits);

                report = Linaa.ReportProgress;
                todo = EndUI;
            }

            if (auxM != null)
            {
                Rsx.Loader worker = null;
                worker = new Rsx.Loader();

                worker.Set(auxM, todo, report);
                worker.RunWorkerAsync(Linaa);
            }
            // else throw new SystemException("No Populate Method was assigned");
        }

        private static void DisposeWorker(ref Rsx.Loader worker)
        {
            if (worker != null)
            {
                worker.CancelAsync();
                worker.Dispose();
                worker = null;
            }
        }

        private static void EndUI()
        {
            LINAA Linaa = dataset as LINAA;
            if (mainCallBack != null) mainCallBack();
            Application.DoEvents();
            Linaa.ReportFinished();
            toPopulate++;

            Load(ref Linaa, toPopulate);
        }
    }
}