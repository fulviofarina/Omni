using System;

using System.Windows.Forms;

namespace DB.UI
{
  public interface IPickerForm
  {
    IPicker IPicker { get; set; }
    System.Windows.Forms.UserControl Module {  set; }
  }

  public partial class PickerForm : Form, IPickerForm
  {
    private UserControl DisplayedControl = null;

    public UserControl Module
        {

            set
            {
                DisplayedControl = value;
                SetControl();

            }
        }

        private void SetControl()
        {
            DisplayedControl.Dock = DockStyle.Fill;

            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            TLP.Controls.Add(DisplayedControl, 0, 1);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            Type tipo = DisplayedControl.GetType();
            // bool showDialog = true;

            if (tipo.Equals(typeof(ucSamples))) this.Text = "Pick the Parent Sample";
            else if (tipo.Equals(typeof(ucMonitors))) this.Text = "Pick the Monitors";
            else if (tipo.Equals(typeof(ucStandards))) this.Text = "Pick the Standards";
            else if (tipo.Equals(typeof(ucMatrix))) this.Text = "Pick the Matrix...";
            else if (tipo.Equals(typeof(ucVialType))) this.Text = "Pick the Vial/Container/Irradiation Capsule...";
            else if (tipo.Equals(typeof(ucGeometry))) this.Text = "Pick the Geometry...";
            else if (tipo.Equals(typeof(ucChannels)))
            {
                //showDialog = true;
                this.Text = "Pick the Channel";
            }

            picker.LinkDGVs();

            ShowDialog();
        }

        private IPicker picker = null;

    public IPicker IPicker
    {
      get { return picker; }
      set { picker = value; }
    }

    public PickerForm()
    {
      InitializeComponent();
    }

    private void Take_Click(object sender, EventArgs e)
    {
      bool cancel = false;
      if (sender.Equals(this.Take)) cancel = picker.Take();

      if (cancel) return;

      this.Visible = false;
      DisplayedControl.Dispose();

      picker.DeLinkDGVs();
      picker = null;

      this.Dispose();
    }

    private void PickerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Controls.Clear();
      DisplayedControl.Dispose();
    }
  }
}