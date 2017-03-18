using System;
using System.Windows.Forms;
using DB.Linq;
using Msn;
using DB.Tools;
using DB.UI;

using System.Data;
using System.Data.Linq;

using System.Collections.Generic;

namespace NSS
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      try
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Form toload = null;


     

        toload = Start();

        //  toload.WindowState = FormWindowState.Maximized;
        Application.Run(toload);
      }
      catch (SystemException ex)
      {
        MessageBox.Show("Program Error: " + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    private static DB.LINAA db = null;
    /// <summary>
    /// Function meant to Create a LINAA database datatables and load itto store and display data
    /// </summary>
    /// <returns>Form created with the respective ucSSF inner control</returns>
    public static Form Start()
    {
     
      Msn.Pop msn = new Msn.Pop(true);

      msn.ParentForm.StartPosition = FormStartPosition.CenterScreen;
      //   msn.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
      msn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      msn.Location = new System.Drawing.Point(3, 32);
      msn.Name = "msn";
      msn.Padding = new System.Windows.Forms.Padding(9);
      msn.Size = new System.Drawing.Size(512, 113);
      msn.TabIndex = 6;


     


      NotifyIcon con = null;
      string result = Creator.Build(ref db, ref con, ref msn);

      //   DB.Tools.Creator.CallBack = this.CallBack;
      //  DB.Tools.Creator.LastCallBack = this.LastCallBack;

      if (!string.IsNullOrEmpty(result))
      {
        //    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //   Connections_Click(null, EventArgs.Empty);
      }
      else DB.Tools.Creator.Load(ref db, 0);

    

      ucSSF uc = new ucSSF(ref db);


      Form form = null;
      form = msn.ParentForm;
      msn.Dock = DockStyle.Fill;
      uc.AttachMsn(msn);
      form.Dispose();


      CreateSSFDatabase(); //TAKE THIS AWAY

     // dolinking();

      return uc.ParentForm;
    }
   // private static LinqDataContext linq = null;

    public static void CreateSSFDatabase()
    {

       LinqDataContext destiny = new LinqDataContext(DB.Properties.Settings.Default.SSFSQL);

    //  new LinqDataContext(Idb)

      if (destiny.DatabaseExists()) destiny.DeleteDatabase();
      //{
           destiny.CreateDatabase();
      //}
      LinqDataContext original = new LinqDataContext(DB.Properties.Settings.Default.NAAConnectionString);

      Clone(ref original, ref destiny);


    }


    public static void Clone(ref LinqDataContext original, ref LinqDataContext destiny)
    {
     

      // Type tipo = typeof(T);

   //   Type o = typeof(Unit);

      IEnumerable<object> dt = original.GetTable<Unit >();
      ITable ita = destiny.GetTable(typeof(Unit));
      Insert(dt, ref ita);

       dt = original.GetTable<Matrix>();
      ita = destiny.GetTable(typeof(Matrix));
      Insert(dt, ref ita);

      dt = original.GetTable<VialType>();
      ita = destiny.GetTable(typeof(VialType));
      Insert(dt, ref ita);


      dt = original.GetTable<Channel>();
      ita = destiny.GetTable(typeof(Channel));
      Insert(dt, ref ita);

      dt = original.GetTable<Geometry>();
      ita = destiny.GetTable(typeof(Geometry));
      Insert(dt, ref ita);

      dt = original.GetTable<NAA>();
      ita = destiny.GetTable(typeof(NAA));
      Insert(dt, ref ita);

      dt = original.GetTable<k0NAA>();
      ita = destiny.GetTable(typeof(k0NAA));
      Insert(dt, ref ita);

      // Insert

    }

    private static void Insert(IEnumerable<object> dt, ref ITable ita)
    {
    //  IEnumerable<object> aux = dt as IEnumerable<object>;
      

      foreach (var i in dt)
      {
          ita.InsertOnSubmit(i);
      }

      ita.Context.SubmitChanges();
    //  linqo.SubmitChanges();
    }

    /// <summary>
    /// Loads the Database, makes the ucControl
    /// </summary>
    /// <returns>the ParentForm of the ucControl</returns>
  }

}