using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.UI;
using Rsx.Dumb;
using VTools;

namespace DB.Tools
{
    public partial class Creator
    {
        public static IList<object> UserControls = new List<object>();

        protected static string HELP_FILE_SSF_PDF = "UserGuide.pdf";

        protected static string PROJECT_LABEL = "PROJECT";

        protected static string WINDOWS_EXPLORER = "explorer.exe";

        public static string HELP_FILE_µFINDER_PDF = "µFinderUserGuide.pdf";

        public static void ConnectionsUI()
        {
            LINAA.PreferencesRow prefe = Interface.IPreferences.CurrentPref;
            Action<SystemException> addException = Interface.IStore.AddException;
            Connections cform = new Connections(ref prefe, ref addException);
            cform.ShowDialog();

            if ((prefe as DataRow).RowState != DataRowState.Modified) return;

            DialogResult res = MessageBox.Show("Would you like to Save/Accept the connection changes?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
            {
                Interface.IPreferences.CurrentPref.RejectChanges();
                // Interface.IPreferences.RejectPreferencesChanges();
            }
            else
            {
                prefe.Check();
                Interface.IPreferences.SavePreferences();
                Application.Restart();
            }
        }

        public static T FindLastControl<T>(string name)
        {
            Func<T, bool> finder = o =>
            {
                bool found = false;
                UserControl os = o as UserControl;
                if (os.Name.ToUpper().CompareTo(name) == 0) found = true;
                return found;
            };
            return UserControls.OfType<T>().LastOrDefault(finder);
        }

        public static IOptions GetOptions(int type)
        {
            IOptions options = new ucOptions(type);
            options.Set();

            options.SaveClick += delegate
            {
                Creator.SaveInFull(true);
            };

            options.DropDownClicked += delegate
            {
                bool ok = Interface.IPreferences
                .CurrentPref.AdvancedEditor;

                options.DisableImportant = ok;

                bool ssf = type == 0;
                options.DisableBasic = ssf;

            };
            options.RestoreFoldersClick += delegate
            {


                Creator.CheckDirectories(true);

             //   Creator.SaveInFull(true);


            };
            options.HelpClick += delegate
            {
                string helpFile = string.Empty;
                if (type == 0) helpFile = getHelpFileSSF();
                else helpFile = getHelpFileµFinder();
                System.Diagnostics.Process.Start(WINDOWS_EXPLORER, helpFile);
            };
            options.ConnectionBox += delegate
            {
                Creator.ConnectionsUI();
            };
            //NOW ADD IT
            Creator.UserControls.Add(options);

            return options;
        }

        public static IGenericBox GetProjectBox()
        {
            return UserControls.OfType<ucGenericCBox>().FirstOrDefault(o => o.Label.CompareTo(PROJECT_LABEL) == 0);
        }

        public static void SetProjectBox(ref ucGenericCBox comboBox)
        {
            IGenericBox control = comboBox;

            string column;
            column = Interface.IDB.IrradiationRequests.IrradiationCodeColumn.ColumnName;
           
            //eveloper method
            control.DeveloperMethod += delegate
            {
                Interface.IPreferences.CurrentPref.AdvancedEditor = true;
                Interface.IPreferences.SavePreferences();

                control.TextContent = Interface.IPreferences.CurrentPref.LastIrradiationProject;

                // GetPreferences(true);
            };
            //populate method
            control.PopulateListMethod += delegate
            {
                if (Interface == null) return; //puede pasar debido al Designer y tener contendio no nulo
                control.InputProjects = Interface.IPopulate.IProjects.ProjectsList.ToArray();
            };
            //refresh method
            control.RefreshMethod += delegate
            {
                bool[] result = Interface.IPopulate.LoadProject(control.EnterPressed, control.TextContent);

                control.Rejected = result[0];
                control.WasRefreshed = result[1];

                if (control.Rejected)
                {
                    control.TextContent = Interface.IPreferences.CurrentPref.LastIrradiationProject;
                }
            };

            control.BindingField = column;
            control.SetBindingSource(ref Interface.IBS.Irradiations, false);

            control.Label = PROJECT_LABEL;
            control.LabelForeColor = System.Drawing.Color.Thistle;

            //ad to users controls...
            UserControls.Add(control);

            /*
            BindingSource bs = Interface.IBS.Irradiations;
            string column;
            column = Interface.IDB.IrradiationRequests.IrradiationCodeColumn.ColumnName;
            control.BindingField = column;
            control.SetBindingSource(ref bs);
            */
        }

        public static void SetSampleBox(ref IGenericBox sampleBox)
        {
            EventHandler endEdit = delegate
            {
                Interface.IBS.EndEdit();
            };

            IGenericBox control = sampleBox;
            // control.Label = "Sample"; control.LabelForeColor = Color.LemonChiffon;
            EventHandler fillsampleNames = delegate
            {
                control.InputProjects = Interface.ICurrent.SubSamplesNames.ToArray();
            };
            control.PopulateListMethod += fillsampleNames;

            //invoke the handlers...
            Interface.IBS.SubSamples.AddingNew += delegate
            {
                // filldescriptios.Invoke(null, EventArgs.Empty);
                fillsampleNames.Invoke(null, EventArgs.Empty);
            };

            IGenericBox cb = GetProjectBox();

            if (cb != null) cb.RefreshMethod += delegate
            {
                if (cb.WasRefreshed) fillsampleNames.Invoke(null, EventArgs.Empty);
            };
            // control.PopulateListMethod += endEdit;
        }

        /// <summary>
        /// Sets a ucGenericBox as a Project Box, with the corresponding methods
        /// </summary>
        /// <param name="comboBox"></param>
        public static void SetSampleDescriptionBox(ref IGenericBox sampleBox)
        {
            IGenericBox control2 = sampleBox;
            // control2.Label = "Description"; control2.LabelForeColor = Color.White;
            EventHandler filldescriptios = delegate
            {
                control2.InputProjects = Interface.ICurrent.SubSamplesDescriptions.ToArray();
            };
            control2.PopulateListMethod += filldescriptios;
            // control2.PopulateListMethod += endEdit;

            //invoke the handlers...
            Interface.IBS.SubSamples.AddingNew += delegate
            {
                filldescriptios.Invoke(null, EventArgs.Empty);
                // fillsampleNames.Invoke(null, EventArgs.Empty);
            };

            IGenericBox cb = GetProjectBox();
            if (cb != null) cb.RefreshMethod += delegate
            {
                if (cb.WasRefreshed) filldescriptios.Invoke(null, EventArgs.Empty);
            };
        }

        protected internal static string getHelpFileSSF()
        {
            return Application.StartupPath + Resources.DevFiles + HELP_FILE_SSF_PDF;
        }
        protected internal static string getHelpFileµFinder()
        {
            return Application.StartupPath + Resources.DevFiles + HELP_FILE_µFINDER_PDF;
        }
    }

    public partial class Creator
    {
        private static Interface Interface = null;

        private static EventHandler lastCallBack = null;
        private static EventHandler mainCallBack = null;

        // private static int toPopulate = 0;

        private static ILoader worker = null;
        /// <summary>
        /// disposes the worker that loads the data
        /// </summary>
    }

    public partial class Creator
    {
        public static string TITLE = Application.ProductName + " v" + Application.ProductVersion + " (Beta)";

        public static bool CheckConnections(bool msmq, bool sql)
        {
            bool ok = false;
            if (msmq)
            {
                bool isMsmq = Interface.IReport.CheckMSMQ();
                if (!isMsmq)
                {
                    //this needs restart to activate so
                    //it will not give back a OK if queried
                    Interface.IReport.InstallMSMQ();
                }
            }
            if (sql)
            {
                //FIRST SQL
                UserControl IConn = new ucSQLConnection();
                //this is the OK that matters..
                //the connection
                ok = PrepareSQL(ref IConn);
            }

            Interface.IPreferences.SavePreferences();
            //CHECK RESTART FILE
            Interface.IReport.CheckRestartFile();

            return ok;
        }

        public static Form CreateForm(ref Bitmap image)
        {
            Form form = null;//
            form = new Form();
            form.Opacity = 0;
            form.AutoSizeMode = AutoSizeMode.GrowOnly;
            form.AutoSize = true;
            IntPtr Hicon = image.GetHicon();
            Icon myIcon = Icon.FromHandle(Hicon);
            form.Icon = myIcon;
            form.Text = TITLE;
            form.HelpButton = true;
            form.TopMost = false;
            form.ShowInTaskbar = true;
            form.ShowIcon = true;
            form.MaximizeBox = false;
            form.ControlBox = true;
            form.StartPosition = FormStartPosition.CenterParent;
            form.SetDesktopLocation(0, 0);

            return form;
        }
    }

    public partial class Creator
    {
        /// <summary>
        /// populates MatSSF resources only
        /// </summary>
        private static void populateMatSSFResource(bool overriderFound)
        {
            string matssf = Interface.IStore.FolderPath + Resources.SSFFolder;
            bool nossf = !Directory.Exists(matssf);
            if (nossf || overriderFound)
            {
                Directory.CreateDirectory(matssf);
                string resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.SSFResource + ".bak";
                string startexecutePath = Interface.IStore.FolderPath + Resources.SSFFolder;
                string destFile = startexecutePath + Resources.SSFResource + ".CAB";
                IO.UnpackCABFile(resourcePath, destFile, startexecutePath, true);

                // destFile = startexecutePath + "MATSSF_XSR.ZIP"; resourcePath = destFile;

                // IO.UnpackCABFile(resourcePath, destFile, startexecutePath, true);
            }
        }

        private static bool populateOverriders()
        {
            string path;
            //override preferences
            path = Interface.IStore.FolderPath + Resources.Preferences + ".xml";
            string developerPath = Application.StartupPath + Resources.DevFiles + Resources.Preferences + ".xml";
            populateReplaceFile(path, developerPath);

            path = Interface.IStore.FolderPath + Resources.SSFPreferences + ".xml";
            developerPath = Application.StartupPath + Resources.DevFiles + Resources.SSFPreferences + ".xml";
            populateReplaceFile(path, developerPath);

            // path = folderPath + Resources.SolCoiFolder;

            bool overriderFound = false;
            try
            {
                //does nothing
                path = Application.StartupPath + Resources.DevFiles + Resources.ResourcesOverrider;
                overriderFound = File.Exists(path);
                //TODO:
                if (overriderFound) File.Delete(path);
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            return overriderFound;
        }

        private static bool populateReplaceFile(string path, string developerPath)
        {
            try
            {
                //this overwrites the user preferences for the developers ones. in case I need to deploy them new preferences
                if (File.Exists(developerPath))
                {
                    File.Copy(developerPath, path, true);
                    File.Delete(developerPath);
                }
            }
            catch (SystemException ex)
            {
                Interface.IStore.AddException(ex);//                throw;
            }

            return File.Exists(path);
        }

        /// <summary>
        /// populates Solcoi resources only
        /// </summary>
        private static void populateSolCoiResource(bool overriderFound)
        {
            string solcoi = Interface.IStore.FolderPath + Resources.SolCoiFolder;
            bool nosolcoi = !Directory.Exists(solcoi);

            if (nosolcoi || overriderFound)
            {
                Directory.CreateDirectory(solcoi);
                string startexecutePath = Interface.IStore.FolderPath + Resources.SolCoiFolder;

                string resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.CurvesResource + ".bak";
                string destFile = startexecutePath + Resources.CurvesResource + ".bak";
                IO.UnpackCABFile(resourcePath, destFile, startexecutePath, false);

                resourcePath = Application.StartupPath + Resources.DevFiles
                    + Resources.SolCoiResource + ".bak";
                destFile = startexecutePath + Resources.SolCoiResource + ".bak";
                IO.UnpackCABFile(resourcePath, destFile, startexecutePath, false);
            }
        }
    }
}