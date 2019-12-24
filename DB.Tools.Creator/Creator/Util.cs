using DB.UI;
using System;
using System.Linq;
using System.Windows.Forms;
using VTools;

namespace DB.Tools
{
    public class Util
    {
        public class Strings
        {
            public const string PROJECT_LABEL = "PROJECT";

            public const string XCOMPreferences = "XCOMPreferences";
            public const string SpecPreferences = "SpecPreferences";
            public const string Preferences = "Preferences";

            public const string ProgramPreferences = "Program Preferences";

            public const string SpecNavPreferences = "SpecNav Preferences";

            public const string uFinderPreferences = "µFinder Preferences";
        }

        private static Interface Interface = null;

        /// <summary>
        /// Initializer / Link
        /// </summary>
        public static void Set(ref Interface inter)
        {
            Interface = inter;
        }

        /// <summary>
        /// PreferenceBox Setter
        /// </summary>
        private static void preferenceSet(string controlHeader, ref UserControl control)
        {
            switch (controlHeader)
            {
                case Strings.Preferences:
                    {
                        UI.IPreferences ucPreferences = new ucPreferences();
                        ucPreferences.IMain.Set(ref Interface);
                        ucPreferences.ISSF.Set(ref Interface);

                        control = (UserControl)ucPreferences;

                        break;
                    }
                case Strings.XCOMPreferences:
                    {
                        IXCOMPreferences ucPreferences = new ucXCOMPreferences();
                        ucPreferences.Set(ref Interface);
                        control = (UserControl)ucPreferences;

                        break;
                    }
                case Strings.SpecPreferences:
                    {
                        ISpecPreferences ucPreferences = new ucSpecPreferences();
                        ucPreferences.Set(ref Interface);
                        control = (UserControl)ucPreferences;

                        break;
                    }

                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the Preferences Box
        /// </summary>
        public static T GetPreferences<T>(bool show = false)
        {
            UserControl ucPref = null;

            string controlName = string.Empty;
            string formName = string.Empty;
            try
            {
                Type t = typeof(T);

                ucPref = preferenceGet(ref controlName, ref formName, t);

                bool postcript = false;

                if (ucPref == null)
                {
                    preferenceSet(controlName, ref ucPref);
                    Creator.UserControls.Add(ucPref);
                    Creator.CreateAppForm(formName, ref ucPref, false);
                    postcript = true;
                }

                preferenceSetForm(show, ref ucPref, postcript);
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
            return (T)(ucPref as object);
        }

        /// <summary>
        /// Sets the PreferenceBox Form
        /// </summary>
        private static void preferenceSetForm(bool show, ref UserControl ucPref, bool postcript)
        {
            Form f = null;

            if (postcript)
            {
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

        /// <summary>
        /// PreferenceBox Maker
        /// </summary>
        private static UserControl preferenceGet(ref string controlName, ref string formName, Type t)
        {
            UserControl ucPref = null;

            if (!t.Equals(typeof(IPreferences)))
            {
                if (t.Equals(typeof(IXCOMPreferences)))
                {
                    ucPref = Creator.UserControls.OfType<ucXCOMPreferences>().FirstOrDefault();
                    formName = Strings.uFinderPreferences;
                    controlName = Strings.XCOMPreferences;
                }
                else if (t.Equals(typeof(ISpecPreferences)))
                {
                    ucPref = Creator.UserControls.OfType<ucSpecPreferences>().FirstOrDefault();
                    formName = Strings.SpecNavPreferences;
                    controlName = Strings.SpecPreferences;
                }
            }
            else
            {
                ucPref = Creator.UserControls.OfType<ucPreferences>().FirstOrDefault();
                formName = Strings.ProgramPreferences;
                controlName = Strings.Preferences;
            }
            return ucPref;
        }

        /// <summary>
        /// Gets the Project Box
        /// </summary>
        public static IGenericBox GetProjectBox()
        {
            return Creator.UserControls.OfType<ucGenericCBox>().FirstOrDefault(o => o.Label.CompareTo(Strings.PROJECT_LABEL) == 0);
        }

        /// <summary>
        /// Sets a Project Box
        /// </summary>
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

            control.Label = Strings.PROJECT_LABEL;
            control.LabelForeColor = System.Drawing.Color.Thistle;

            //ad to users controls...
            Creator.UserControls.Add(control);

            /*
            BindingSource bs = Interface.IBS.Irradiations;
            string column;
            column = Interface.IDB.IrradiationRequests.IrradiationCodeColumn.ColumnName;
            control.BindingField = column;
            control.SetBindingSource(ref bs);
            */
        }

        /// <summary>
        /// Sets a Sample Box
        /// </summary>
        public static void SetSampleBox(ref IGenericBox sampleBox)
        {
            EventHandler _endEdit = delegate
            {
                Interface.IBS.EndEdit();
            };

            IGenericBox control = sampleBox;
            // control.Label = "Sample"; control.LabelForeColor = Color.LemonChiffon;
            EventHandler _fillsampleNames = delegate
            {
                control.InputProjects = Interface.ICurrent.SubSamplesNames.ToArray();
            };
            control.PopulateListMethod += _fillsampleNames;

            //invoke the handlers...
            Interface.IBS.SubSamples.AddingNew += delegate
            {
                // filldescriptios.Invoke(null, EventArgs.Empty);
                _fillsampleNames.Invoke(null, EventArgs.Empty);
            };

            IGenericBox cb = GetProjectBox();

            if (cb != null) cb.RefreshMethod += delegate
            {
                if (cb.WasRefreshed) _fillsampleNames.Invoke(null, EventArgs.Empty);
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

        /// <summary>
        /// Finds the last control of a given Name
        /// </summary>
        public static T FindLastControl<T>(string name)
        {
            Func<T, bool> finder = o =>
            {
                bool found = false;
                UserControl os = o as UserControl;
                if (os.Name.ToUpper().CompareTo(name) == 0) found = true;
                return found;
            };
            return Creator.UserControls.OfType<T>().LastOrDefault(finder);
        }

        /// <summary>
        /// Gets the options control of the given type or, creates a new one with the given parameters
        /// </summary>
        /// <param name="type">required</param>
        /// <param name="advancedEdtior"></param>
        /// <param name="save"></param>
        /// <param name="restore"></param>
        /// <param name="connections"></param>
        /// <returns></returns>
        public static IOptions GetOptions(int type, bool advancedEdtior = false, bool save = true, bool restore = true, bool connections = true)
        {
            IOptions[] optionArr = Creator.UserControls?.OfType<ucOptions>().ToArray();
            IOptions options = null;
            options = optionArr?.FirstOrDefault(o => o.Type == type);

            //returns the options
            if (options != null) return options;


            //sets a NEW ONE!!
            try
            {


                //created but not in list
                options = new ucOptions(type);

                options.Set();

                if (save)
                {
                    options.SaveClick += delegate
                    {
                        Creator.SaveInFull(true);
                    };
                }

                options.EnableAdv = advancedEdtior;
                //check if SSF program to activate other menues
                //  bool ssf = type == 0;
                options.EnableConnections = connections;


                options.PreferencesClick += delegate
                {
                    if (type == 1)
                    {
                        Util.GetPreferences<IXCOMPreferences>(true);
                    }
                    else if (type == 2)
                    {
                        Util.GetPreferences<ISpecPreferences>(true);
                    }
                    else if (type == 0)
                    {
                        Util.GetPreferences<IPreferences>(true);
                    }
                };


                /*

                DropDownClicked += delegate
                {
                    //disable others
                    DisableImportant = advanced;
                    //check if SSF program to activate other menues
                    bool ssf = type == 0;
                    DisableBasic = ssf;
                };

                */
                if (restore)
                {
                    options.RestoreFoldersClick += delegate
                    {
                        Creator.CheckDirectories(true);
                    };
                }
                if (connections)
                {
                    options.ConnectionsClick += delegate
                    {
                        UtilSQL.ConnectionsUI();
                    };
                }

                Creator.UserControls.Add(options);


            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
            return options;
        }



    }
}