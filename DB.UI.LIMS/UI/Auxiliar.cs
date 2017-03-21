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
    }

    public void AuxiliarForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!e.Cancel)
      {
        this.TLP.Controls.Remove(this.DisplayedControl);
      }
    }

    private void TLP_ControlRemoved(object sender, ControlEventArgs e)
    {
      e.Control.Dispose();
      this.Dispose();
    }
  }
}