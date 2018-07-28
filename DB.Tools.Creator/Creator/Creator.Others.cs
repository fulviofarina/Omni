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
    
   
        protected static string PROJECT_LABEL = "PROJECT";

        public static IList<object> UserControls = new List<object>();

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
          
            bool ok = Interface.IPreferences
            .CurrentPref.AdvancedEditor;

            IOptions options = new ucDBOptions(type,ok);

            /*
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
       
            options.ConnectionBox += delegate
            {
                Creator.ConnectionsUI();
            };
            //NOW ADD IT
            Creator.UserControls.Add(options);
            */
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

    }

   
}