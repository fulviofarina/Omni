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

namespace SSF
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

        private static string title = "SSF Panel by Fulvio Farina Arboccò";

        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        public static Form Start()
        {
            //   Form Mainform = new Form();

            try
            {
                /*
                //main form
                Mainform.Opacity = 0; //do not show
                Mainform.Text = title;

                Icon myIcon = Icon.FromHandle(Hicon);
                Mainform.AutoSize = true;
                Mainform.Icon = myIcon;
                */
                // Mainform.StartPosition = FormStartPosition.CenterScreen;

                //Popper
                Pop msn = new Pop(true);
                msn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                msn.Location = new System.Drawing.Point(3, 32);
                msn.Name = "msn";
                msn.Padding = new System.Windows.Forms.Padding(9);
                msn.Size = new System.Drawing.Size(512, 113);
                msn.TabIndex = 6;
                msn.ParentForm.StartPosition = FormStartPosition.CenterScreen;

                NotifyIcon con = null;
                LIMS.UserControls = new List<object>();

                //create database
                string result = Creator.Build(ref LIMS.Linaa, ref con, ref msn, 0);

                //    DB.Tools.Creator.CallBack = delegate
                //    {
                // Application.DoEvents();

                //   };

                DB.Tools.Creator.LastCallBack = delegate
                {
                    //make a new interface
                    LIMS.Interface = new Interface(ref LIMS.Linaa);
                    //set user control list
                    //create LIMS form
                    LIMS.Form = new LIMS(); //make a new UI LIMS
                    LIMS.Form.WindowState = FormWindowState.Minimized;

                    //create form for SubSamples
                    UserControl control = LIMS.CreateUI(ControlNames.SubSamples);
                    LIMS.CreateForm("Samples", ref control);
                    control.ParentForm.Opacity = 0;
                    control.ParentForm.ShowInTaskbar = false;

                    //set bindings
                    ucSubSamples ucSubSamples = control as ucSubSamples;

                    ucSubSamples.ucContent.Set(ref LIMS.Interface);

                    bool autoload = LIMS.Interface.IPreferences.CurrentPref.AutoLoad;
                    if (autoload)
                    {
                        string lastProject = LIMS.Interface.IPreferences.CurrentPref.LastIrradiationProject;
                        ucSubSamples.projectbox.Project = lastProject;
                    }
                    //set child parent
                    ///VOLVER A PONER ESTO EVENTUALMENTE
                    ///
                    msn.Dock = DockStyle.Fill;
                    Form form = msn.ParentForm;
                    form.Opacity = 0;
                    ucSSF uc = new ucSSF();
                    Control ctrl = msn as Control;
                    uc.AttachMsn(ref ctrl);
                    form.Dispose();

                    form = new Form();
                    form.AutoSizeMode = AutoSizeMode.GrowOnly;
                    form.AutoSize = true;
                    IntPtr Hicon = Properties.Resources.Logo.GetHicon();
                    Icon myIcon = Icon.FromHandle(Hicon);
                    form.Icon = myIcon;
                    form.Text = title;

                    form.Controls.Add(uc);

                    form.Show();
                    //   form.ControlBox = true;
                    //  form.TopMost = false;

                    //     ucSubSamples.ucSSF = uc;

                    Control ctrl2 = ucSubSamples.projectbox;
                    uc.AttachProjectBox(ref ctrl2);
                    Control ctrl3 = ucSubSamples.BN;
                    uc.AttachBN(ref ctrl3);

                    uc.Set(ref LIMS.Interface);

                    //    Control ctrl4 = ucSubSamples;
                    //   uc.AttachSampleCtrl(ref ctrl4);

                    LIMS.Form.Visible = true;

                    form.Opacity = 100;
                };

                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(result, "Could not connect to LIMS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //   Connections_Click(null, EventArgs.Empty);
                    // Application.Restart();
                }
                else
                {
                    DB.Tools.Creator.Load();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Program Error: " + ex.Message + "\n\n" + ex.StackTrace);
            }
            return LIMS.Form;
        }

        private static void lastCallBack(ref ucSSF uc)
        {
            //     ucSubSamples.Predict(null, EventArgs.Empty);

            //////////////////////QUITAR ESTA LINEAAAAAAAAAAAA
            //    uc.ParentForm.Visible = false;

            //  ucSubSamples.DeLink();
            //  uc.ucSubSamples.HideContent();
            //     CreateSSFDatablase(); //TAKE THIS AWAY
        }

        // private static LinqDataContext linq = null;

        public static void CreateSSFDatabase()
        {
            LinqDataContext destiny = new LinqDataContext(DB.Properties.Settings.Default.NAAConnectionString);

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