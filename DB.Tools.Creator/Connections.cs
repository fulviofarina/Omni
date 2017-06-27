using System;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class Connections : Form
    {
        protected internal Action<SystemException> exceptions = null;
        protected internal DB.LINAA.PreferencesRow pref;

        private void closingForm(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(ucSQLLIMSCom.ConnectionString))
            {
                MessageBox.Show("A connection to the LIMS database needs to be provided...", "Important", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else
            {
                try
                {
                    pref.SetConnections(ucSQLHLCom.ConnectionString, ucSQLLIMSCom.ConnectionString, this.SpectraSvrRBT.Text.ToUpper(), this.SpectraRBT.Text.ToUpper());
                }
                catch (SystemException ex)
                {
                    exceptions?.Invoke(ex);
                }
                e.Cancel = false;
            }
            if (!e.Cancel) this.Dispose();
        }

        private void loadForm(object sender, EventArgs e)
        {
            try
            {
                ucSQLHLCom.ConnectionString = pref.HL;
                ucSQLLIMSCom.ConnectionString = pref.LIMS;

                this.SpectraSvrRBT.Text = pref.SpectraSvr.ToUpper();

                this.SpectraRBT.Text = pref.Spectra.ToUpper();
            }
            catch (SystemException ex)
            {
                exceptions?.Invoke(ex);
            }
        }

        private void setTitles()
        {
            this.Text = "Connections";
            ucSQLHLCom.Title = "HyperLab Server";
            ucSQLLIMSCom.Title = "LIMS Server";
        }

        public Connections()
        {
        }

        public Connections(ref LINAA.PreferencesRow preferences, ref Action<SystemException> excepts)
        {
            InitializeComponent();

            pref = preferences;
            exceptions = excepts;
            setTitles();

            this.FormClosing += this.closingForm;
            this.Load += this.loadForm;
        }
    }
}