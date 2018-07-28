using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using VTools;
using static Rsx.DGV.Control;

namespace DB.UI
{
    public partial class LIMSUI
    {


        public static Form CreateSSFApplication()
        {
            Bitmap icon = Resources.Logo;

            Form form = DBForm.CreateForm(ref icon);

            IPreferences preferences = GetPreferences<IPreferences>();
          
            IOptions options = GetOptions( 0);
            options.PreferencesClick += delegate
            {
              GetPreferences<IPreferences>(true);
            };
            options.HelpClick += delegate
            {
                string helpFile = string.Empty;
                helpFile = getHelpFileSSF();
                System.Diagnostics.Process.Start(WINDOWS_EXPLORER, helpFile);
            };
            //DEVELOPER MODE
            //      options.SetDeveloperMode(false);

            //1
            UserControl control = CreateUI(ControlNames.SubSamples);
            CreateForm("Samples", ref control, false);
            ucSubSamples ucSubSamples = control as ucSubSamples;
            ucSubSamples.ucContent.Set(ref Interface);

            //2
            IGenericBox ucProjectBox = Creator.GetProjectBox();

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


        public static IOptions GetOptions( int type )
        {


            IOptions[] optionArr = Creator.UserControls?.OfType<ucOptions>().ToArray();

            IOptions options = null;

            options = optionArr?.FirstOrDefault(o => o.Type == type);
            if (options != null) return options;

            //created but not in list
            options = Creator.GetOptions(type);

            //start BINDING
            options.AboutBoxClick += delegate
            {
                aboutBox?.Show();
            };

            //EXPLORER
                options.ExplorerClick += delegate
                {
                // if (!Interface.IPreferences.CurrentPref.AdvancedEditor) return;
                Explore();
                };
            //LIMS
            options.DatabaseClick += delegate
            {
                Form.Visible = true;
                Form.Opacity = 100;
                Form.BringToFront();
            };
            return options;
        }

        public static T GetPreferences<T>(bool show = false)
        {
            UserControl ucPref = null;
            string controlName = string.Empty;
            string formName = string.Empty;
            try
            {
                Type t = typeof(T);
                if (t.Equals(typeof(IPreferences)))
                {
                    ucPref = Creator.UserControls.OfType<ucPreferences>().FirstOrDefault();
                    formName = "Program Preferences";
                    controlName = ControlNames.Preferences;
                }
                else if (t.Equals(typeof(IXCOMPreferences)))
                {
                    ucPref = Creator.UserControls.OfType<ucXCOMPreferences>().FirstOrDefault();
                    formName = "µ-Finder Preferences";
                    controlName = ControlNames.XCOMPreferences;
                }
                else if (t.Equals(typeof(ISpecPreferences)))
                {
                    ucPref = Creator.UserControls.OfType<ucSpecPreferences>().FirstOrDefault();
                    formName = "SpecNav Preferences";
                    controlName = ControlNames.SpecPreferences;
                }
                Form f = null;
                if (ucPref == null)
                {
                    createPreference(controlName, ref ucPref);
                    Creator.UserControls.Add(ucPref);
                    CreateForm(formName, ref ucPref, false);
           
                    f = ucPref.ParentForm;
                    f.VisibleChanged += delegate
                    {
                        Interface.IPreferences.ReportChanges();
                    };
                    f.MaximizeBox = false;
                    f.ShowInTaskbar = false;
                }

                f = ucPref?.ParentForm;

                if (f != null)
                {
                    f.Visible = show;
                    f.TopMost = show;
                    f.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
            return ucPref as dynamic;
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
        protected static string WINDOWS_EXPLORER = "explorer.exe";
        protected static string HELP_FILE_SSF_PDF = "UserGuide.pdf";

        public static string HELP_FILE_µFINDER_PDF = "µFinderUserGuide.pdf";

        protected internal static string getHelpFileSSF()
        {
            return Application.StartupPath + Resources.DevFiles + HELP_FILE_SSF_PDF;
        }
        protected internal static string getHelpFileµFinder()
        {
            return Application.StartupPath + Resources.DevFiles + HELP_FILE_µFINDER_PDF;
        }

        public static void CreateMatrixApplication(out UserControl control, out Refresher refresher)
        {
          
        
         

            //ESTE ORDEN ES FUNDAMENTAL!!!
       

            Application.DoEvents();

            refresher = delegate
            {

                Interface.IBS.ApplyFilters();
                Interface.IBS.StartBinding();

                Interface.IBS.ShowErrors = false;

           

                bool offline = Interface.IPreferences.CurrentPref.Offline;
                if (offline)
                {
                
                    Creator.LoadFromFile();

                    Application.DoEvents();

                    Interface.IDB.MUES.Clear();
                    Interface.IDB.Preferences.Clear();

                    Application.DoEvents();
                    Creator.PopulatePreferences();
                    Interface.IPreferences.CurrentPref.Offline = offline;

                    Interface.IDB.AcceptChanges();

                    Interface.IDB.CheckMatrixToDoes(offline);

                }
                else Interface.IPopulate.IGeometry.PopulateMatrixSQL();

                Interface.IBS.ShowErrors = true;

                //y esto? quitar?
                Interface.IBS.EnabledControls = true;

                Application.DoEvents();

            //    Creator.SaveInFull(true);

                Interface.IReport.Msg("Database matrices and compositions were loaded", "Loaded", true);


            };

            refresher.Invoke();

        
            ucMatrix mat = new ucMatrix();
            mat.Set(ref Interface);
            control = (UserControl)mat;

            Application.DoEvents();

            IXCOMPreferences prefes = GetPreferences<IXCOMPreferences>();
            IOptions options = GetOptions(1);
            options.PreferencesClick += delegate
            {
                GetPreferences<IXCOMPreferences>(true);
            };

            options.HelpClick += delegate
            {
                string helpFile = string.Empty;
            
                helpFile = getHelpFileµFinder();
                System.Diagnostics.Process.Start(WINDOWS_EXPLORER, helpFile);
            };


            Refresher dos = refresher;

            options.RestoreFoldersClick += delegate
            {
                dos.Invoke();
            };
            Application.DoEvents();

            mat.Set(ref options);
            mat.Set(ref prefes);
            mat.SetXCOM();

            Application.DoEvents();

            IPop msn = Interface.IReport.Msn;
            UserControl ctrl = msn as UserControl;
            mat.Set(ref ctrl);

            Application.DoEvents();
            msn.ParentForm?.Dispose();
          

           
        }

     
    }
}