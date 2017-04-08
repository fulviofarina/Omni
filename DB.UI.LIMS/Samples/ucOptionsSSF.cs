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

            loadPreferences();
            bindings = binding;
            samplebindings = sampbindings;

            this.OptionsBtn.DropDownClosed += delegate
            {
                setPreferences();
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

            N4.TextBox.Text = ip.CurrentSSFPref.Rounding;
            this.loop.Checked = ip.CurrentSSFPref.Loop;

            this.doCK.Checked = ip.CurrentSSFPref.DoCK;
            this.doMatSSF.Checked = ip.CurrentSSFPref.DoMatSSF;
            this.showMatSSF.Checked = ip.CurrentSSFPref.ShowMatSSF;
            this.AutoLoad.Checked = ip.CurrentSSFPref.AutoLoad;
            this.FolderPath.Text = ip.CurrentSSFPref.Folder;
            this.showOther.Checked = ip.CurrentSSFPref.ShowOther;

            this.calcDensity.Checked = ip.CurrentSSFPref.CalcDensity;
            this.findlength.Checked = ip.CurrentSSFPref.AARadius;
            this.findRadius.Checked = ip.CurrentSSFPref.AAFillHeight;

            this.workOffline.Checked = ip.CurrentPref.Offline;
        }

        /// <summary>
        /// sets the preferences when the OptionsMenu closes
        /// </summary>
        private void setPreferences()
        {
            try
            {
                IPreferences ip = Interface.IPreferences;

                ip.CurrentSSFPref.CalcDensity = this.calcDensity.Checked;

                ip.CurrentSSFPref.Loop = this.loop.Checked;

                ip.CurrentSSFPref.Loop = this.loop.Checked;

                ip.CurrentSSFPref.DoCK = this.doCK.Checked;
                ip.CurrentSSFPref.DoMatSSF = this.doMatSSF.Checked;
                ip.CurrentSSFPref.ShowMatSSF = this.showMatSSF.Checked;
                ip.CurrentSSFPref.AutoLoad = this.AutoLoad.Checked;

                ip.CurrentSSFPref.Folder = this.FolderPath.Text;
                ip.CurrentSSFPref.ShowOther = this.showOther.Checked;
                ip.CurrentPref.Offline = this.workOffline.Checked;

                ip.CurrentSSFPref.CalcDensity = this.calcDensity.Checked;
                ip.CurrentSSFPref.AARadius = this.findlength.Checked;
                ip.CurrentSSFPref.AAFillHeight = this.findRadius.Checked;

                ip.CurrentPref.Offline = this.workOffline.Checked;

                //  ip.CurrentSSFPref.SQL = this.SQL.Checked;

                //ROUNDING

                string format = N4.TextBox.Text;
                if (format.Length < 2) return;

                Dumb.ChangeBindingsFormat(format, ref bindings);
                Dumb.ChangeBindingsFormat(format, ref samplebindings);
                Interface.IPreferences.CurrentSSFPref.Rounding = format;

                Interface.IStore.SavePreferences();
            }
            catch (Exception ex)
            {
                Interface.IMain.AddException(ex);
                Interface.IReport.Msg(ex.Message + "\n" + ex.Source + "\n", "Error", false);
            }
        }
    }
}