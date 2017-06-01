using System.Windows.Forms;

namespace DB.UI
{
    public partial class Auxiliar : Form
    {
        public UserControl DisplayedControl = null;

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
            DisplayedControl = control;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.FormClosing +=this.formClosing;

        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DisplayedControl.GetType().Equals(typeof(ucPreferences)))
            {
                LIMS.Interface.IPreferences.SavePreferences();
                e.Cancel = true;
              
            }
            if (!e.Cancel)
            {
                this.TLP.Controls.Remove(this.DisplayedControl);
            }
            else this.Visible = false;
        }

        private void controlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.Dispose();
            this.Dispose();
        }
    }
}