using DB.Properties;
using DB.Tools;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VTools;

namespace DB.UI
{
    public partial class LIMSUI
    {
        public static Form createSSFApplication()
        {
            Bitmap icon = Resources.Logo;

            Form form = DBForm.CreateForm(ref icon);

            IPreferences preferences = Util.GetPreferences<IPreferences>();
            bool advEditor = Interface.IPreferences.CurrentPref.AdvancedEditor;
            bool connections = Interface.IPreferences.CurrentPref.Offline;
            IOptions options = setOptions(0, advEditor, true, true, connections);

            options.HelpClick += delegate
            {
                string helpFile = string.Empty;
                string HELP_FILE_SSF_PDF = "UserGuide.pdf";
                helpFile = Interface.IStore.FolderPath + Resources.DevFiles + HELP_FILE_SSF_PDF;
                System.Diagnostics.Process.Start(Rsx.Dumb.IO.EXPLORER_EXE, helpFile);
            };
            //DEVELOPER MODE
            //      options.SetDeveloperMode(false);

            //1
            UserControl control = CreateUI(ControlNames.SubSamples);
            Creator.CreateAppForm("Samples", ref control, false);
            ucSubSamples ucSubSamples = control as ucSubSamples;
            ucSubSamples.ucContent.Set(ref Interface);

            //2
            IGenericBox ucProjectBox = Util.GetProjectBox();

            //3
            BindingNavigator aBindingNavigator = ucSubSamples.BN;

            //4
            IPop msn = Interface.IReport.Msn;
            Form frm2 = msn.ParentForm;
            // firstCallBack.Invoke(null, EventArgs.Empty);

            ISSF ucSSF = new ucSSF();

            (ucSSF as UserControl).AutoSizeMode = AutoSizeMode.GrowOnly;

            EventHandler midCallBack;
            midCallBack = delegate
            {
                Interface.IBS.ShowErrors = false;

                //ESTE ORDEN ES FUNDAMENTAL!!!
                Interface.IBS.ApplyFilters();
                Interface.IBS.StartBinding();

                ucSSF.Set(ref Interface, ref preferences);
                ucSSF.Set(ref options);

                ucSSF.Set(ref ucProjectBox);
                ucSSF.IPanel.Set(ref aBindingNavigator);

                //si esto se dispara después,
                //me resetea las preferencias!!!
                Interface.IBS.EnabledControls = true;

                // ARREGLAR ESTO
                LINAA.SSFPrefRow pr = Interface.IPreferences.CurrentSSFPref;
                pr.DoMatSSF = true;
                pr.DoCK = false;
                pr.Loop = true;
                pr.CalcDensity = true;
                Interface.IPreferences.CurrentPref.AutoLoad = true;
                // ARREGLAR ESTO
            };

            EventHandler lastCallBack;
            lastCallBack = delegate
            {
                //load last porject
                bool autoload = Interface.IPreferences.CurrentPref.AutoLoad;
                string lastProject = string.Empty;
                if (autoload)
                {
                    lastProject = Interface.IPreferences.CurrentPref.LastIrradiationProject;
                }

                if (!autoload || string.IsNullOrEmpty(lastProject))
                {
                    Interface.IReport.GreetUser();
                    Interface.IReport.SpeakLoadingFinished();
                }
                else ucProjectBox.TextContent = lastProject;

                form.Controls.Add(ucSSF as UserControl);
                form.FormClosing += formClosing;

                Interface.IBS.ShowErrors = true;

                Application.DoEvents();
                frm2.Opacity = 0;
                ucSSF.IPanel.Set(ref msn);
                frm2.Dispose();

                ucSSF.SetTimer();

                form.Opacity = 100;

                //CleanSigmas();
            };

            Creator.LastCallBack = lastCallBack;

            Creator.CallBack = midCallBack;

            return form;
        }

        private static UserControl createMatrixApplication(out EventHandler refresher)
        {
            bool advEditor = Interface.IPreferences.CurrentPref.AdvancedEditor;

            IXCOMPreferences prefes = Util.GetPreferences<IXCOMPreferences>();
            bool save = true;
            bool restore = true;
            bool connections = true;

            IOptions options = setOptions(1, advEditor, save, restore, connections);


            ucMatrix mat = new ucMatrix();
            mat.Set(ref Interface);

            refresher = delegate
            {
                Interface.IBS.ApplyFilters();
                Application.DoEvents();
                Interface.IBS.StartBinding();
                Application.DoEvents();
                Interface.IBS.ShowErrors = false;

                Application.DoEvents();
                bool offline = Interface.IPreferences.CurrentPref.Offline;
                if (offline)
                {
                    Creator.LoadFromFile();
                    Application.DoEvents();
                    Interface.IDB.MUES.Clear();
                    Application.DoEvents();
                    Interface.IDB.AcceptChanges();
                }
                else Interface.IPopulate.IGeometry.PopulateMatrixSQL();

                Application.DoEvents();
                Interface.IDB.CheckMatrixToDoes();
                Application.DoEvents();
                Interface.IBS.ShowErrors = true;
                //y esto? quitar?
                Interface.IBS.EnabledControls = true;
                Application.DoEvents();
                Interface.IReport.Msg("Database matrices and compositions were loaded", "Loaded", true);
            };

            refresher.Invoke(null, EventArgs.Empty);

            mat.Set(options);
            mat.Set(ref prefes);
            mat.SetXCOM();

            options.RestoreFoldersClick += refresher;

            IPop msn = Interface.IReport.Msn;
            Interface.IReport.Msn.ParentForm.Visible = false;
            UserControl ctrl = msn as UserControl;
            mat.Set(ref ctrl);

            return mat;
        }

       

        private static UserControl createSpecNavApplication()
        {
            ucHyperLab hl = new ucHyperLab();
            ISpecPreferences prefes = Util.GetPreferences<ISpecPreferences>();
            bool advEditor = Interface.IPreferences.CurrentPref.AdvancedEditor;
            bool connections = true;
            bool canSave = false;
            bool restore = false;
            IOptions options = setOptions(2, advEditor, canSave, restore, connections);

            Interface.IBS.ApplyFilters();
            Interface.IBS.StartBinding();
            Interface.IBS.ShowErrors = false;

            hl.Set(ref Interface);

            hl.Set(ref options);

      


            hl.Set(ref prefes);

            return hl;
        }




        private static IOptions setOptions(int type, bool advancedEdtior = false, bool save = true, bool restore = true, bool connections = true)
        {
         
            IOptions options = Util.GetOptions(type, advancedEdtior, save, restore, connections);



                //start BINDING
                options.AboutClick += delegate
                {
                    aboutBox?.Show();
                };
                //EXPLORER
                options.ExplorerClick += delegate
                {
                    Explore();
                };
                //LIMS
                options.DatabaseInterfaceClick += delegate
                {
                    Form.Visible = true;
                    Form.Opacity = 100;
                    Form.BringToFront();
                };
         
            return options;
        }


        /*
        public static IXCOMPreferences GetXCOMPreferences(bool show = false)
        {
            ucXCOMPreferences c = null;
            try
            {
                c = Creator.UserControls.OfType<ucXCOMPreferences>().FirstOrDefault();

                if (c == null)
                {
                    UserControl ucPref = null;
                    ucPref = CreateUI(ControlNames.XCOMPreferences);
                    CreateForm(ControlNames.XCOMPreferences, ref ucPref, false);
                    c = ucPref as ucXCOMPreferences;
                    c.ParentForm.VisibleChanged += delegate
                    {
                        Interface.IPreferences.ReportChanges();
                    };
                    c.ParentForm.MaximizeBox = false;
                    c.ParentForm.ShowInTaskbar = false;
                }

                c.ParentForm.Visible = show;
                c.ParentForm.TopMost = show;
                c.ParentForm.BringToFront();
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
            return c;
        }
*/



        private static void formClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = Creator.Close();
            if (!e.Cancel)
            {
                Application.ExitThread();
            }
        }
    }
}