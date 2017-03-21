using System;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using DB;
using DB.Linq;
using DB.Tools;
using DB.UI;
using Msn;
using System.Linq;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(Start());
        }

        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        public static Form Start()
        {
            Form Mainform = new Form();
            Mainform.Opacity = 0;
            //   DB.LINAA db = DB.UI.LIMS.Linaa;

            try
            {
                //main form

                Mainform.Text = "SSF Panel by Fulvio Farina Arboccò";
                IntPtr Hicon = Properties.Resources.Logo.GetHicon();
                Icon myIcon = Icon.FromHandle(Hicon);
                Mainform.AutoSize = true;
                Mainform.Icon = myIcon;
                // Mainform.StartPosition = FormStartPosition.CenterScreen;
                Mainform.Opacity = 0;

                //POpper
                Pop msn = new Pop(true);
                msn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                msn.Location = new System.Drawing.Point(3, 32);
                msn.Name = "msn";
                msn.Padding = new System.Windows.Forms.Padding(9);
                msn.Size = new System.Drawing.Size(512, 113);
                msn.TabIndex = 6;
                Form form = msn.ParentForm;
                form.StartPosition = FormStartPosition.CenterScreen;

                NotifyIcon con = null;
                LIMS.UserControls = new System.Collections.Generic.List<object>();

                string result = Creator.Build(ref LIMS.Linaa, ref con, ref msn);

                //    DB.Tools.Creator.CallBack = delegate
                //    {
                // Application.DoEvents();
                //   };

                DB.Tools.Creator.LastCallBack = delegate
                {
                    //make a new interface
                    LIMS.Interface = new Interface(ref LIMS.Linaa);
                    //set user control list

                    ucSSF uc = new ucSSF(ref LIMS.Interface);
                    msn.Dock = DockStyle.Fill;
                    form.Visible = false;

                    Control ctrl = msn as Control;
                    uc.AttachMsn(ref ctrl);

                    form.Dispose();
                    Mainform.Controls.Add(uc);

                    //create LIMS form
                    LIMS.Form = new LIMS(); //make a new UI LIMS

                    lastCallBack(ref uc);

                    GAForm.Form gafrm = new GAForm.Form();
                    NAAControl ganaaControl = new NAAControl();
                    gafrm.IControl = ganaaControl;
                    ganaaControl.Interface = LIMS.Interface;
                    gafrm.Show();

                    DataRow[] rows = LIMS.Interface.IDB.SubSamples.AsEnumerable().ToArray();
                    string field = LIMS.Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName;
                    string fieldID = LIMS.Interface.IDB.SubSamples.SubSamplesIDColumn.ColumnName;
                    gafrm.FillExternalProblems(ref rows, field, fieldID);

                    LIMS.Form.WindowState = FormWindowState.Minimized;
                    LIMS.Form.Visible = true;

                    // Mainform.Opacity = 100;
                    //     Mainform.Opacity = 100;
                };

                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //   Connections_Click(null, EventArgs.Empty);
                }
                else
                {
                    DB.Tools.Creator.Load(ref LIMS.Linaa, 0);
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Program Error: " + ex.Message + "\n\n" + ex.StackTrace);
            }
            return Mainform;
        }

        private static void lastCallBack(ref ucSSF uc)
        {
            //create form for SubSamples
            UserControl control = LIMS.CreateUI(DB.UI.ControlNames.SubSamples);
            LIMS.CreateForm("Samples", ref control);

            //set bindings
            ucSubSamples ucSubSamples = control as ucSubSamples;
            ucSubSamples.ucContent.Set(ref LIMS.Interface);
            ucSubSamples.projectbox.Project = "X1701";

            //set child parent
            ucSubSamples.ucSSF = uc;
            uc.ParentUI = ucSubSamples;
            uc.LoadDatabase();

            //  uc.ucSubSamples.HideContent();
            //     CreateSSFDatablase(); //TAKE THIS AWAY
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

            ITable dt = original.GetTable<Unit>();
            ITable ita = destiny.GetTable(typeof(Unit));
            Insert(ref dt, ref ita);

            dt = original.GetTable<Matrix>();
            ita = destiny.GetTable(typeof(Matrix));
            Insert(ref dt, ref ita);

            dt = original.GetTable<VialType>();
            ita = destiny.GetTable(typeof(VialType));
            Insert(ref dt, ref ita);

            dt = original.GetTable<Channel>();
            ita = destiny.GetTable(typeof(Channel));
            Insert(ref dt, ref ita);

            dt = original.GetTable<Geometry>();
            ita = destiny.GetTable(typeof(Geometry));
            Insert(ref dt, ref ita);

            dt = original.GetTable<NAA>();
            ita = destiny.GetTable(typeof(NAA));
            Insert(ref dt, ref ita);

            dt = original.GetTable<k0NAA>();
            ita = destiny.GetTable(typeof(k0NAA));
            Insert(ref dt, ref ita);
        }

        private static void Insert(ref ITable dt, ref ITable ita)
        {
            foreach (var i in dt)
            {
                ita.InsertOnSubmit(i);
            }

            ita.Context.SubmitChanges();
        }

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
    }
}