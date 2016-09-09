using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using Msn;

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

    private static int toPopulate = 0;

    private static String result = string.Empty;

    public static String Result
    {
      get { return result; }
      set { result = value; }
    }

    private static Action mainCallBack = null;

    private static Action lastCallBack = null;

    public static Action LastCallBack
    {
      get { return Creator.lastCallBack; }
      set { Creator.lastCallBack = value; }
    }

    public static Action CallBack
    {
      get { return Creator.mainCallBack; }
      set { Creator.mainCallBack = value; }
    }

    public static void Load(ref LINAA Linaa, int populNr)
    {
      IEnumerable<Action> auxM = null;

      Action todo = null;

      Rsx.Loader.Reporter report = null;

      if (toPopulate == 1)
      {
        auxM = Linaa.PMNAA();
        //      auxM = auxM.Union(Linaa.PMNAA());
        todo = lastCallBack;
      }
      else if (toPopulate == 0)
      {
        auxM = Linaa.PMZero();
        //  auxM = auxM.Union(Linaa.PMMatrix());
        // auxM = auxM.Union(Linaa.PMStd());
        // auxM = auxM.Union(Linaa.PMDetect());
        report = Linaa.ReportProgress;
        todo = EndUI;
      }

      if (auxM != null)
      {
        Rsx.Loader worker = null;
        worker = new Rsx.Loader();

        worker.Set(auxM.ToArray(), todo, report);
        worker.RunWorkerAsync(Linaa);
      }
      // else throw new SystemException("Not Populate Method was assigned");
    }

    private static object dataset = null;

    private static void EndUI()
    {
      LINAA Linaa = dataset as LINAA;
      if (mainCallBack != null) mainCallBack();
      Application.DoEvents();
      Linaa.ReportFinished();
      toPopulate++;

      Load(ref Linaa, toPopulate);
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

    /// <summary>
    /// Builds a reference Linaa database, creating it if it does not exist, giving feedback through a notifyIcon and a handler to a method that will run after completition
    /// </summary>
    /// <param name="Linaa">referenced database to build (can be a null reference)</param>
    /// <param name="notify">referenced notifyIcon to give feedback of the process</param>
    /// <param name="handler">referenced handler to a method to run after completition </param>
    public static string Build(ref LINAA Linaa, ref System.Windows.Forms.NotifyIcon notify, ref Pop msn)
    {
      //restarting = false;

      if (Linaa != null) Dispose(ref Linaa);

      if (Linaa == null)
      {
        dataset = null;
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
        Action[] populMethod = Linaa.PMBasic();
        foreach (Action a in populMethod)
        {
          a.Invoke();
        }
        Linaa.InitializeAdapters();
      }

      string cmd = Application.StartupPath + Resources.Restarting;
      if (System.IO.File.Exists(cmd))
      {
        //  restarting = true;
        string email = System.IO.File.ReadAllText(cmd);
        System.IO.File.Delete(cmd);
        Linaa.GenerateReport("Restarting succeeded...", string.Empty, string.Empty, Linaa.DataSetName, email);
      }

      if (!Linaa.IsMainConnectionOk)
      {
        string title = DB.Properties.Errors.Error404;
        title += Linaa.Exception;
        result = title;
      }

      return result;
    }

    private static void Dispose(ref LINAA Linaa)
    {
      Linaa.Clear();
      Linaa.DisposeAdapters();
      Linaa.Dispose();
      Linaa = null;
    }
  }
}