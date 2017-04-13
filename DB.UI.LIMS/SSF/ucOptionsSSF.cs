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


            this.OptionsBtn.DropDownClosed += delegate
            {
                setPreferences();
            };

            this.Save.Click += delegate
                  {
                      this.ParentForm.Validate();
                      Creator.SaveInFull(true);
                  };

            this.limsTSMI.Click += delegate
            {
                LIMS.Form.Visible = true;
            };

            this.preferencesTSMI.Click += delegate
            {
                LIMS.ShowPreferences();
            };

        }

        /// <summary>
        /// Loads preferences
        /// Invoked once inside a try catch does not need one
        /// </summary>
        private void loadPreferences()
        {

            IPreferences ip = Interface.IPreferences;
            N4.TextBox.Text = ip.CurrentSSFPref.Rounding;

       
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