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

        // [STAThread]
        public static Form CreateSSFUserInterface()
        {

      

            IucPreferences preferences = LIMS.PreferencesUI();
            Form aboutbox = new AboutBox();
            IucOptions options = LIMS.OptionsUI(ref aboutbox);

            UserControl control = LIMS.CreateUI(ControlNames.SubSamples);
            LIMS.CreateForm("Samples", ref control, false);
            ucSubSamples ucSubSamples = control as ucSubSamples;
            ucSubSamples.ucContent.Set(ref LIMS.Interface);
            ucProjectBox ucProjectBox = null;
            ucProjectBox = ucSubSamples.projectbox;
            BindingNavigator aBindingNavigator = ucSubSamples.BN;

            ucUnit units = LIMS.CreateUI(ControlNames.Units,null,true) as ucUnit;

            ucSSF ucSSF =new ucSSF();
            ucSSF.Set(ref LIMS.Interface);


        


            Pop msn = LIMS.Interface.IReport.Msn;
            Form form = msn.ParentForm;//


            Creator.CallBack = delegate
            {


               


                if (form == null) form = new Form();
              
             
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

                form.FormClosing += Form_FormClosing;

                form.Opacity = 0;

                LIMS.Interface.IBS.StartBinding();
                LIMS.Interface.IBS.ApplyFilters();

                ucSSF.AttachCtrl(ref msn);
                ucSSF.AttachCtrl(ref options);
                ucSSF.AttachCtrl(ref preferences);
                ucSSF.AttachCtrl(ref ucProjectBox);
                ucSSF.AttachCtrl(ref aBindingNavigator);
                ucSSF.AttachCtrl(ref units);

                form.Controls.Add(ucSSF);
                form.Opacity = 100;

            

            };
            // form.Enabled = false;

            Creator.LastCallBack = delegate
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

                Application.DoEvents();

                LIMS.Interface.IReport.SpeakLoadingFinished();
            };

      

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

                //FIRST THIS IN CASE i NEED TO RESTART AGAIN IN SQL INSTALL
                bool isMsmq = LIMS.Interface.IReport.CheckMSMQ();

                if (!isMsmq)
                {
                    Creator.InstallMSMQ();
                }
               
                //FIRST SQL
                UserControl IConn = new ucSQLConnection();
                bool ok = Creator.PrepareSQL(ref IConn);
             

                LIMS.Interface.IPreferences.SavePreferences();
                //CHECK RESTART FILE
                LIMS.Interface.IReport.CheckRestartFile();
                

                if (ok) Creator.LoadMethods(0);
                else throw new Exception("Could not start loading the database");

                toReturn = CreateSSFUserInterface();

                Creator.Run();

                Application.Run(toReturn);
            }
            catch (Exception ex)
            {
                LIMS.Interface.IStore.AddException(ex);
                LIMS.Interface.IStore.SaveExceptions();
                MessageBox.Show("Severe program error: " + ex.Message + "\n\nat code:\n\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
    }
}