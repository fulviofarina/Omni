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

                //create database
                Creator.Build(ref LIMS.Interface, ref con, ref msn);

                Creator.CheckDirectories();

                bool ok = Creator.Prepare(0);

                LIMS.Linaa = LIMS.Interface.Get();
                LIMS.UserControls = new List<object>();
                LIMS.Form = new LIMS(); //make a new UI LIMS
                LIMS.Form.Enabled = false;
                LIMS.Form.Visible = false;
             //   LIMS.Form.Visible = true;

                //    LIMS.Interface = new Interface(ref LIMS.Linaa);

                //  LIMS.Form.WindowState = FormWindowState.Minimized;

                //    DB.Tools.Creator.CallBack = delegate
                //    {
                // Application.DoEvents();

                //   };

                if (ok)
                {
                    DB.Tools.Creator.LastCallBack = delegate
                {
                    //make a new interface

                    //set user control list
                    //create LIMS form
         

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

                    //

                    form.Opacity = 100;

                    Application.DoEvents();
                    LIMS.Form.Enabled = true;
                    LIMS.Interface.IReport.ReportFinished();
                };

                    //GO GO GO GO
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

        /// <summary>
        /// Loads the Database, makes the ucControl
        /// </summary>
        /// <returns>the ParentForm of the ucControl</returns>
    }
}