using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rsx;
using System.Collections;
using DB.Tools;
using DB.Properties;

namespace DB.UI.Samples
{
    public partial class ucOptionsSSF : UserControl
    {
        public ucOptionsSSF()
        {
            InitializeComponent();

           
        }

        private Interface Interface;
        private Hashtable bindings, samplebindings;

        public void Set(ref Interface inter, ref Hashtable binding, ref Hashtable sampbindings)
        {
            Interface = inter;

            bindings = binding;
            samplebindings = sampbindings;
            loadPreferences();

            string col = Interface.IDB.SSFPref.LoopColumn.ColumnName;
            object source = Interface.IPreferences.CurrentSSFPref;
            Binding b = new Binding("Checked", source, col);

            this.OptionsBtn.DropDownClosed += delegate
            {
                setPreferences();
            };

            this.Save.Click += delegate

                  {
                      this.ParentForm.Validate();
                      saveMethod();
                  };
        }

        /// <summary>
        /// Loads preferences
        /// Invoked once inside a try catch does not need one
        /// </summary>
        private void loadPreferences()
        {
            //preferences
            IPreferences ip = Interface.IPreferences;

            if (!ip.CurrentPref.Offline)
            {
                // Interface.IPopulate.IGeometry.PopulateUnits();
            }
            else //fix this
            {
                //  Interface.IPopulate.IMain.Read(mf);
            }

            MatSSF.StartupPath = Interface.IMain.FolderPath + Resources.SSFFolder;
            // MatSSF.StartupPath += ip.CurrentSSFPref.Folder;

            N4.TextBox.Text = ip.CurrentSSFPref.Rounding;
       
        }

        private static Environment.SpecialFolder folder = Environment.SpecialFolder.Personal;

        private void saveMethod()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Interface.IBS.Matrix.EndEdit();
                Interface.IBS.Units.EndEdit();
                Interface.IBS.Vial.EndEdit();
                Interface.IBS.Rabbit.EndEdit();
                Interface.IBS.Channels.EndEdit();

                //    WHAT IS THIS
                MatSSF.WriteXML();

                //WHAT IS THIS
                bool off = Interface.IPreferences.CurrentPref.Offline;
                string savePath = Interface.IMain.FolderPath + "lims.xml";
                Interface.IStore.SaveSSF(off, savePath);

                IEnumerable<DataTable> tables = Interface.IDB.Tables.OfType<DataTable>();
                Interface.IStore.SaveRemote(ref tables, true);

                Interface.IReport.Msg("Saving", "Saving completed!");
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.Source + "\n", "Error", false);
            }
            Cursor.Current = Cursors.Default;
        }

    
     
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LIMS.UserControls.OfType<ucPreferences>().FirstOrDefault().ParentForm.Visible = true;
        }

        /// <summary>
        /// sets the preferences when the OptionsMenu closes
        /// </summary>
        private void setPreferences()
        {
            try
            {
                // calcDensity_Click();

                IPreferences ip = Interface.IPreferences;

               
                //  ip.CurrentSSFPref.SQL = this.SQL.Checked;

                //ROUNDING

                string format = N4.TextBox.Text;
                if (format.Length < 2) return;

                Dumb.ChangeBindingsFormat(format, ref bindings);
                Dumb.ChangeBindingsFormat(format, ref samplebindings);
                Interface.IPreferences.CurrentSSFPref.Rounding = format;

                //save preferences
                Interface.IPreferences.SavePreferences();
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.Source + "\n", "Error", false);
            }
        }
    }
}