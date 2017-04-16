using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;
using Msn;

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
        static  Pop msn;//= new Pop(true);

        static NotifyIcon con;//= new NotifyIcon();
      
   
        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>
        public static Form Start()
        {

            try
            {

                msn = new Pop(true);
                //create database
                Creator.Build(ref LIMS.Interface, ref msn);

                Creator.CheckDirectories();

                DB.Tools.Creator.CallBack = delegate
                {
                    Application.DoEvents();
                };

                DB.Tools.Creator.LastCallBack = CreateSSFUserInterface;

                LIMS.Linaa = LIMS.Interface.Get();
                LIMS.UserControls = new List<object>();

                bool ok = Creator.Prepare(0);
                Creator.CheckBugFile();



                LIMS.Form = new LIMS(); //make a new UI LIMS
                LIMS.Form.Opacity = 0;
                LIMS.Form.ShowInTaskbar = false;
             
              
                //   LIMS.Form.Controls.Add(con as Control);

                if (ok)
                {
                    //GO GO GO GO
                    DB.Tools.Creator.Load();
                }
                else throw new Exception("Could not start loading the database");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program Error: " + ex.Message + "\n\n" + ex.StackTrace);
            }
            return LIMS.Form;
        }

        public static void CreateSSFUserInterface()
        {

            IucPreferences preferences = PreferencesUI();

            //create form for SubSamples
            UserControl control = LIMS.CreateUI(ControlNames.SubSamples);
            LIMS.CreateForm("Samples", ref control);
        //    control.ParentForm.Opacity = 0;
        //    control.ParentForm.ShowInTaskbar = false;
            //set bindings
            ucSubSamples ucSubSamples = control as ucSubSamples;
            ucSubSamples.ucContent.Set(ref LIMS.Interface);

            ucProjectBox ucProjectBox = null;
            ucProjectBox = ucSubSamples.projectbox;

            BindingNavigator aBindingNavigator = ucSubSamples.BN;


            IucOptions options = OptionsUI();

            ucSSF ucSSF = null;
            ucSSF = new ucSSF();
            ucSSF.Set(ref LIMS.Interface);


            bool autoload = LIMS.Interface.IPreferences.CurrentPref.AutoLoad;
            if (autoload)
            {
                string lastProject = LIMS.Interface.IPreferences.CurrentPref.LastIrradiationProject;
                ucSubSamples.projectbox.Project = lastProject;
                ucSubSamples.projectbox.Refresher();
            }
            //set child parent
            ///VOLVER A PONER ESTO EVENTUALMENTE
            ///


            Form form = msn.ParentForm;//

            ucSSF.AttachCtrl(ref msn);

            ucSSF.AttachCtrl(ref options);
            ///////////////////////

            ucSSF.AttachCtrl(ref ucProjectBox);

            ucSSF.AttachCtrl(ref aBindingNavigator);

            //     IucPreferences ucIPref = ucPref as IucPreferences;
            ucSSF.AttachCtrl(ref preferences);


            form.Dispose();
            form = new Form();
            form.AutoSizeMode = AutoSizeMode.GrowOnly;
            form.AutoSize = true;
            IntPtr Hicon = Properties.Resources.Logo.GetHicon();
            Icon myIcon = Icon.FromHandle(Hicon);
            form.Icon = myIcon;
            form.Text = title;

            form.Controls.Add(ucSSF);




            form.Show();
            form.Opacity = 100;


            Application.DoEvents();

            form.FormClosing += Form_FormClosing;     


            LIMS.Explore();

            LIMS.Interface.IReport.ReportFinished();
        }

        private static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = Creator.Close(ref LIMS.Linaa);
            if (!e.Cancel)
            {
                //Application.Exit();
                Application.ExitThread();
            }
        }

        private static IucPreferences PreferencesUI()
        {
            UserControl ucPref = null;
            ucPref = LIMS.CreateUI(ControlNames.Preferences); //; as ucPreferences;
            LIMS.CreateForm("Preferences", ref ucPref);
            LIMS.ShowPreferences(false);
            IucPreferences preferences = (IucPreferences)ucPref;
            return preferences;
        }

        private static IucOptions OptionsUI()
        {
            IucOptions options = new ucOptions();
            options.Set();
            options.AboutBoxAction = delegate
            {
                AboutBox box = new AboutBox();
                box.Show();
            };
            options.ConnectionBox = delegate
            {
                LIMS.Connections();
            };
            options.SaveClick = delegate
            {

                Creator.SaveInFull(true);
            };
            options.PreferencesClick = delegate
            {
                LIMS.ShowPreferences(true);
            };
            options.DatabaseClick = delegate
            {
                LIMS.ShowToUser();
            };
            return options;
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

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
    }
}