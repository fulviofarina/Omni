using System;
using System.Drawing;
using System.Windows.Forms;

namespace k0X
{
    public partial class Auxiliar : Form
    {
        public UserControl DisplayedControl = null;

        public Auxiliar()
        {
            InitializeComponent();
        }

        private Point PreviousLocation = Point.Empty;
        private Size OriginalSize = Size.Empty;
        private Size HiddenSize = Size.Empty;

        public void Populate(UserControl control)
        {
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TLP.Controls.Clear();
            control.Dock = DockStyle.Fill;
            TLP.Controls.Add(control, 0, 0);
            DisplayedControl = control;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            OriginalSize = new Size(control.Size.Width, control.Size.Height);
            HiddenSize = new Size(control.Size.Width, 0);
        }

        public void AuxiliarForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Type typo = this.DisplayedControl.GetType();

            if (typo.Equals(typeof(ucDetWatch)))
            {
                ucDetWatch uc = this.DisplayedControl as ucDetWatch;
                if (!Rsx.Dumb.IsNuDelDetch(uc.LSchAcq)) uc.LSchAcq.NotCrashed = false;
            }

            if (!e.Cancel)
            {
                this.DisplayedControl.Dispose();
            }
        }

        private void TLP_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.Dispose();
        }
    }
}