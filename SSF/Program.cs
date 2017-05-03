using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DB.Tools;
using DB.UI;
using Msn;
using VTools;

namespace SSF
{
    internal static class Program
    {
        private static NotifyIcon con;

     

        private static string TITLE = "SSF Calculations";

    //    [STAThread]
        public static Form CreateSSFUserInterface()
        {

         

            ucSSF ucSSF = null;
            ucSSF = new ucSSF();
            ucSSF.Set(ref LIMS.Interface);

      

            IucPreferences preferences = PreferencesUI();
            IucOptions options = OptionsUI();

            ucSSF.AttachCtrl(ref options);
            ucSSF.AttachCtrl(ref preferences);


            UserControl control = LIMS.CreateUI(ControlNames.SubSamples);
            LIMS.CreateForm("Samples", ref control, false);
            ucSubSamples ucSubSamples = control as ucSubSamples;
            ucSubSamples.ucContent.Set(ref LIMS.Interface);
            ucProjectBox ucProjectBox = null;
            ucProjectBox = ucSubSamples.projectbox;
            BindingNavigator aBindingNavigator = ucSubSamples.BN;

            ucSSF.AttachCtrl(ref ucProjectBox);
            ucSSF.AttachCtrl(ref aBindingNavigator);


            Pop msn = LIMS.Interface.IReport.Msn;
            Form form = msn.ParentForm;//
            ucSSF.AttachCtrl(ref msn);
            form.Opacity = 0;
            // form.Dispose(); form = new Form();
            form.AutoSizeMode = AutoSizeMode.GrowOnly;
            form.AutoSize = true;
            IntPtr Hicon = Properties.Resources.Logo.GetHicon();
            Icon myIcon = Icon.FromHandle(Hicon);
            form.Icon = myIcon;
            form.Text = TITLE;
            form.TopMost = false;
            form.ShowInTaskbar = true;
            form.ShowIcon = true;
            form.MaximizeBox = false;
            form.ControlBox = true;
            form.StartPosition = FormStartPosition.CenterScreen;
           
        
            //  form.Enabled = false;

            Creator.CallBack = delegate
            {

          
                Application.DoEvents();
          

                bool autoload = LIMS.Interface.IPreferences.CurrentPref.AutoLoad;

                string lastProject = string.Empty;
                if (autoload)
                {
                    lastProject = LIMS.Interface.IPreferences.CurrentPref.LastIrradiationProject;
                }

                   ucProjectBox.Project = lastProject;
                   ucProjectBox.Refresher();

                LIMS.Interface.IReport.SpeakLoadingFinished();

            };


            form.Controls.Add(ucSSF);
            form.FormClosing += Form_FormClosing;

            form.Opacity = 100;

            return form;



        }

        /// <summary>
        /// Function meant to Create a LINAA database datatables and load itto store and display data
        /// </summary>
        /// <returns>Form created with the respective ucSSF inner control</returns>


        private static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = Creator.Close(ref LIMS.Linaa);
            if (!e.Cancel)
            {
                Application.ExitThread();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Form toReturn = null;

            try
            {


                //create database
                Creator.Build(ref LIMS.Interface);
                Creator.CheckDirectories();
                LIMS.Interface.IPreferences.PopulatePreferences();


                LIMS.Linaa = LIMS.Interface.Get();
                LIMS.Form = new LIMS(); //make a new UI LIMS
                LIMS.Form.ShowInTaskbar = false;
                LIMS.Form.Opacity = 0;

                LIMS.UserControls = new List<object>();

                LIMS.Interface.IReport.CheckMSMQ();

                bool ok = Creator.PrepareSQL();

                LIMS.Interface.IReport.CheckRestartFile();

                LIMS.Interface.IPreferences.SavePreferences();

                if (ok) Creator.LoadMethods(0);
                else throw new Exception("Could not start loading the database");

                toReturn = CreateSSFUserInterface();

                    Creator.Run();
        


                Application.Run(toReturn);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Program Error: " + ex.Message + "\n\n" + ex.StackTrace);
            }
        
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
            options.ExplorerClick = LIMS.Explore;

            options.PreferencesClick = delegate
            {
                LIMS.ShowPreferences(true);
            };
            options.DatabaseClick = LIMS.ShowToUser;

            return options;
        }

        //= new Pop(true);
        private static IucPreferences PreferencesUI()
        {
            UserControl ucPref = null;
            ucPref = LIMS.CreateUI(ControlNames.Preferences);
            LIMS.CreateForm("Preferences", ref ucPref, false);
            IucPreferences preferences = (IucPreferences)ucPref;
            return preferences;
        }

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
    }
}