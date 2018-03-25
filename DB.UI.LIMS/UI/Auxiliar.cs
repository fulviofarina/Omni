using System;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class Auxiliar : Form
    {
        private UserControl _displayedControl = null;

        public Auxiliar()
        {
            InitializeComponent();
        }

        public void Populate(UserControl control)
        {
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TLP.Controls.Clear();
            control.Dock = DockStyle.Fill;
            TLP.Controls.Add(control, 0, 0);
            _displayedControl = control;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.SetDesktopLocation(0,0);
         //   this.Location.Y = 0;
            this.FormClosing += this.formClosing;
        }

        protected internal void formClosing(object sender, FormClosingEventArgs e)
        {
            Type tipo = this._displayedControl.GetType();
            if (tipo.Equals(typeof(ucPreferences)))
            {
                LIMS.Interface.IPreferences.SavePreferences();
                e.Cancel = true;
            }
            else if (tipo.Equals(typeof(ucXCOMPreferences)))
            {
                LIMS.Interface.IPreferences.SavePreferences();
                e.Cancel = true;
            }
            if (!e.Cancel)
            {
                this.TLP.Controls.Remove(this._displayedControl);
            }
            else this.Visible = false;
        }

        protected internal void controlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.Dispose();
            this.Dispose();
        }
    }
}