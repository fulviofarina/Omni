using System;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class Connections : Form
    {
        private DB.LINAA.PreferencesRow pref;
  
        private void Connections_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ucSQLLIMSCom.ConnectionString.Equals(string.Empty))
            {
                DialogResult res = MessageBox.Show("A connection to the LIMS database needs to be provided... Try again?", "Important", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (res == System.Windows.Forms.DialogResult.No) this.Dispose();
                else return;
            }

            try
            {
                pref.HL = ucSQLHLCom.ConnectionString;
                pref.LIMS = ucSQLLIMSCom.ConnectionString;

                pref.SpectraSvr = this.SpectraSvrRBT.Text.ToUpper();

                string spectra = this.SpectraRBT.Text.ToUpper();
                if (!spectra[spectra.Length - 1].ToString().Equals("\\")) spectra += "\\";
                pref.Spectra = spectra;
            }
            catch (SystemException ex)
            {
            }
            this.Dispose();
        }

        private void Connections_Load(object sender, EventArgs e)
        {
            try
            {
                ucSQLHLCom.ConnectionString = pref.HL;
                ucSQLLIMSCom.ConnectionString = pref.LIMS;
                ucSQLHLCom.Title = "HyperLab Server";
                ucSQLLIMSCom.Title = "LIMS Server";
                this.SpectraSvrRBT.Text = pref.SpectraSvr.ToUpper();

                this.SpectraRBT.Text = pref.Spectra.ToUpper();
            }
            catch (SystemException ex)
            {
            }
        }

        public Connections(ref LINAA.PreferencesRow preferences)
        {
            InitializeComponent();

            pref = preferences;
            this.Text = "Connections";
            this.FormClosing += this.Connections_FormClosing;
            this.Load += this.Connections_Load;

         
        }
    }
}